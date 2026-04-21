using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.Systems.ArtifactSystem;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.Consumable.Potion;
using Roguelike.Texture;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Transfixion.Artifacts;
internal class OuterGodTheNihilismArtifact : Artifact {
	public override string TexturePath => ModTexture.Get_MissingTexture("Artifact");
	public override Color DisplayNameColor => Color.Black;
}
public class OuterGodTheNihilismModPlayer : ModPlayer {
	public bool artifact = false;
	public override void ResetEffects() {
		artifact = Player.HasArtifact<OuterGodTheNihilismArtifact>();
		if (artifact) {
			PlayerStatsHandle.Set_Chance_SecondLifeCondition(Player, "OuterGod_Nihilism", .8f);
		}
	}
	public override void MeleeEffects(Item item, Rectangle hitbox) {
	}
	public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		return base.Shoot(item, source, position, velocity, type, damage, knockback);
	}
	public override void UpdateEquips() {
		if (!artifact) {
			return;
		}
		Player.ModPlayerStats().UpdateDefenseBase -= 1;
		if (Player.immune) {
			if (Main.rand.NextBool(10)) {
				Projectile.NewProjectile(Player.GetSource_ItemUse(Player.HeldItem), Main.rand.NextVector2FromRectangle(Player.Hitbox), Vector2.Zero, ModContent.ProjectileType<VoidParticle>(), 1, 4f, Player.whoAmI, 70);
			}
		}
	}
	public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
		if (!artifact) {
			return;
		}
		modifiers.SourceDamage += 1.5f;
	}
	public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
		if (!artifact) {
			return;
		}
		modifiers.SourceDamage += 1.5f;
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		if (!artifact) {
			return;
		}
		if (Player.HeldItem.IsAWeapon(true)) {
			Item item = Player.HeldItem;

			NPC.HitInfo hitinfo = new();
			hitinfo.Damage = (int)(Player.GetWeaponDamage(item) * 1.1f) + 1;
			hitinfo.Crit = Main.rand.Next(1, 101) >= Player.GetWeaponCrit(item);
			hitinfo.Knockback = 0;
			Player.StrikeNPCDirect(npc, hitinfo);

			int amount = Main.rand.Next(7, 11);
			for (int i = 0; i < amount; i++) {
				Projectile.NewProjectile(Player.GetSource_ItemUse(Player.HeldItem), npc.Center + Main.rand.NextVector2CircularEdge(100 + npc.width, 100 + npc.height), Vector2.Zero, ModContent.ProjectileType<VoidParticle>(), 1, 4f, Player.whoAmI, 70);
			}
		}
		Player.AddBuff(ModContent.BuffType<ShatterShell>(), ModUtils.ToSecond(4));
	}
	public override void PostUpdate() {
		if (artifact && Player.ModPlayerStats().HasDodgeInThisInstance) {
			if (Player.HeldItem.IsAWeapon(true)) {
				Item item = Player.HeldItem;
				if (Player.Center.LookForHostileNPC(out NPC npc, 9999)) {
					NPC.HitInfo hitinfo = new();
					hitinfo.Damage = (int)(Player.GetWeaponDamage(item) * 2f) + 1;
					hitinfo.Crit = Main.rand.Next(1, 101) >= Player.GetWeaponCrit(item);
					hitinfo.Knockback = 0;
					Player.StrikeNPCDirect(npc, hitinfo);
				}
				int amount = Main.rand.Next(7, 11);
				for (int i = 0; i < amount; i++) {
					Projectile.NewProjectile(Player.GetSource_ItemUse(Player.HeldItem), npc.Center + Main.rand.NextVector2CircularEdge(100 + npc.width, 100 + npc.height), Vector2.Zero, ModContent.ProjectileType<VoidParticle>(), 1, 4f, Player.whoAmI, 70);
				}
			}
		}
	}
	public override bool FreeDodge(Player.HurtInfo info) {
		if (artifact && Main.rand.NextFloat() <= .4f) {
			Player.ModPlayerStats().HasDodgeInThisInstance = true;
			Player.immune = true;
			Player.AddImmuneTime(info.CooldownCounter, 44);
			return true;
		}
		return base.FreeDodge(info);
	}
	public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource) {
		if (PlayerStatsHandle.Get_Chance_SecondLife(Player, "OuterGod_Nihilism")) {
			Player.Heal(100);
			Player.immune = true;
			Player.AddImmuneTime(-1, 90);
			return false;
		}
		return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genDust, ref damageSource);
	}
}
public class ShatterShell : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff(true);
	}
	public override bool ReApply(Player player, int time, int buffIndex) {
		player.buffTime[buffIndex] += time;
		return true;
	}
	public override void Update(Player player, ref int buffIndex) {
		player.endurance -= 1;
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.LifeSteal -= 1;
		handler.UpdateDefenseBase -= 1;
		handler.HealEffectiveness -= 1;
		handler.BuffTime -= 1;
	}
}
public class VoidParticle : ModProjectile {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 5;
		Projectile.timeLeft = 300;
		Projectile.penetrate = 1;
		Projectile.tileCollide = false;
		Projectile.friendly = true;
		Projectile.hide = true;
	}
	public override void AI() {
		int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Granite, newColor: Color.Black);
		Main.dust[dust].noGravity = true;
		Main.dust[dust].velocity = Main.rand.NextVector2Circular(3, 3);
		if (Projectile.Center.LookForHostileNPC(out NPC npc, 300)) {
			Projectile.timeLeft = 100;
			Vector2 distance = npc.Center - Projectile.Center;
			float length = distance.Length();
			if (length > 1) {
				length = 1;
			}
			Projectile.velocity -= Projectile.velocity * .1f;
			Projectile.velocity += distance.SafeNormalize(Vector2.Zero) * length;
			Projectile.velocity = Projectile.velocity.LimitedVelocity(20);
			return;
		}
		Projectile.velocity.Y = 1;
		Projectile.velocity -= Projectile.velocity * .01f;
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		modifiers.FinalDamage -= 1;
		modifiers.FinalDamage.Flat += Projectile.ai[0];
		modifiers.ScalingArmorPenetration += 1;
	}
}

