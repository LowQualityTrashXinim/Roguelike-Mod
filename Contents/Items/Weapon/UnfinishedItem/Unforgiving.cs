using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.Weapon.UnfinishedItem;

public class Unforgiving : SynergyModItem {
	public override string Texture => ModTexture.Get_MissingTexture("Synergy");
	public override void SetDefaults() {
		Item.BossRushDefaultRange(32, 32, 24, 10f, 10, 10, ItemUseStyleID.Shoot, ProjectileID.ShadowFlameArrow, 12, true, AmmoID.Arrow);
	}
	public override bool CanShoot(Player player) {
		if (GunMode) {
			Item.useAmmo = AmmoID.Bullet;
		}
		else {
			Item.useAmmo = AmmoID.Arrow;
		}
		Item ammo = player.ChooseAmmo(Item);
		if (ammo == null) {
			GunMode = !GunMode;
			if (GunMode) {
				Item.useAmmo = AmmoID.Bullet;
			}
			else {
				Item.useAmmo = AmmoID.Arrow;
			}
			ammo = player.ChooseAmmo(Item);
			if (ammo == null) {
				return false;
			}
		}
		Pre_ShootType = ammo.shoot;
		return player.GetModPlayer<Unforgiving_ModPlayer>().AttackCD <= 0;
	}
	public override void SynergyUpdateInventory(Player player, PlayerSynergyItemHandle modplayer) {
		player.GetModPlayer<Unforgiving_ModPlayer>().Unforgiving_GunMode = GunMode;
	}
	public int GlobalCounter = 0;
	int Pre_ShootType = 0;
	/// <summary>
	/// If gun mode is false then currently is bow mode<br/>
	/// Otherwise it is gun mode obviously
	/// </summary>
	public bool GunMode = false;
	public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (GunMode) {
			type = Pre_ShootType;
		}
		else {
			if (Pre_ShootType == ProjectileID.WoodenArrowFriendly) {
				type = ProjectileID.ShadowFlameArrow;
			}
			else {
				type = Pre_ShootType;
			}
		}
	}
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		CanShootItem = false;
		GlobalCounter = ModUtils.Safe_SwitchValue(GlobalCounter, 24);
		Unforgiving_ModPlayer unforgive = player.GetModPlayer<Unforgiving_ModPlayer>();
		int counter = unforgive.Counter;
		unforgive.Counter = -player.itemAnimationMax;
		int CD = unforgive.CD;
		Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
		if (counter >= 120) {
			if (GunMode) {
				proj.damage = (int)(proj.damage * 1.5f);
				for (int i = 0; i < 4; i++) {
					Projectile blackbolt = Projectile.NewProjectileDirect(source, position, velocity.Vector2DistributeEvenlyPlus(4, 45, i), ProjectileID.BlackBolt, damage * 2, knockback, player.whoAmI);
					blackbolt.scale = .56f;
					blackbolt.extraUpdates += 3;
				}
			}
			else {
				proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().CustomDataValue = 1;
				proj.extraUpdates += 2;
			}
		}
		if (GlobalCounter % 5 == 0) {
			GunMode = !GunMode;
			unforgive.AttackCD = 30;
		}
		if (GlobalCounter >= 24) {
			player.AddBuff<Unforgiving_BoltFury>(ModUtils.ToSecond(2));
		}
		if (CD <= 0) {
			unforgive.CD = 60;
			if (GunMode) {
				Projectile.NewProjectile(source, position, velocity, ProjectileID.BlackBolt, damage * 3, knockback, player.whoAmI);
			}
			else {
				for (int i = 0; i < 5; i++) {
					Projectile.NewProjectile(source, position + Main.rand.NextVector2CircularEdge(100, 100) * Main.rand.NextFloat(.8f, 2), Vector2.Zero, ModContent.ProjectileType<Roguelike_SpiritFlame>(), damage, knockback, player.whoAmI);
				}
			}
		}
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.OnyxBlaster)
			.AddIngredient(ItemID.ShadowFlameBow)
			.Register();
	}
}
public class Roguelike_SpiritFlame : ModProjectile {
	public override string Texture => ModUtils.GetVanillaTexture<Projectile>(ProjectileID.SpiritFlame);
	public override void SetDefaults() {
		Main.projFrames[Type] = 4;
		Projectile.CloneDefaults(ProjectileID.SpiritFlame);
		Projectile.aiStyle = -1;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.penetrate = 1;
		Projectile.width = 18;
		Projectile.height = 32;
		Projectile.timeLeft = 60;
	}
	NPC npc = null;
	public override void AI() {
		if (++Projectile.frameCounter > 4) {
			Projectile.frameCounter = 0;
			if (++Projectile.frame >= 4)
				Projectile.frame = 0;
		}
		if (npc != null) {
			if (!npc.active) {
				npc = null;
			}
		}
		float num1052 = 3f;
		float num1053 = 12.5f;
		float num1054 = 1f;
		int num1055 = 420;
		if (Projectile.localAI[0] > 0f)
			Projectile.localAI[0]--;

		if (Projectile.localAI[0] == 0f && npc == null && Projectile.owner == Main.myPlayer) {
			Projectile.localAI[0] = 5f;
			if (Projectile.Center.LookForHostileNPC(out NPC target, 900)) {
				npc = target;
			}

			if (npc != null) {
				Projectile.timeLeft = num1055;
				Projectile.netUpdate = true;
			}
		}

		if (Projectile.timeLeft > 30 && Projectile.alpha > 0)
			Projectile.alpha -= 12;

		if (Projectile.timeLeft > 30 && Projectile.alpha < 128 && Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
			Projectile.alpha = 128;

		if (Projectile.alpha < 0)
			Projectile.alpha = 0;

		float num1058 = 0.5f;
		if (Projectile.timeLeft < 120)
			num1058 = 1.1f;

		if (Projectile.timeLeft < 60)
			num1058 = 1.6f;

		Projectile.ai[1]++;
		for (float num1060 = 0f; num1060 < 3f; num1060++) {
			if (Main.rand.Next(3) == 0) {
				Dust dust61 = Main.dust[Dust.NewDust(Projectile.Center, 0, 0, DustID.Shadowflame, 0f, -2f)];
				dust61.position = Projectile.Center + Vector2.UnitY.RotatedBy(num1060 * ((float)Math.PI * 2f) / 3f + Projectile.ai[1]) * 10f;
				dust61.noGravity = true;
				dust61.velocity = Projectile.DirectionFrom(dust61.position);
				dust61.scale = num1058;
				dust61.fadeIn = 0.5f;
				dust61.alpha = 200;
			}
		}

		if (npc != null) {
			if (npc.active) {
				if (Projectile.Distance(npc.Center) > num1054) {
					Vector2 vector174 = Projectile.DirectionTo(npc.Center);
					if (vector174.HasNaNs())
						vector174 = Vector2.UnitY;

					Projectile.velocity = (Projectile.velocity * (num1052 - 1f) + vector174 * num1053) / num1052;
				}

			}
			return;
		}
	}
}
public class Unforgiving_ModPlayer : ModPlayer {
	public int Counter = 0;
	public int CD = 0;
	public int Duration25th = 0;
	public int DamageBucket = 0;
	int DamageBucketCap = 0;
	public bool Unforgiving_GunMode = false;
	public int AttackCD = 0;
	public override void ResetEffects() {
		AttackCD = ModUtils.CountDown(AttackCD);
		CD = ModUtils.CountDown(CD);
		if (++Counter >= 270) {
			Counter = 270;
		}
		if (AttackCD > 0) {
			return;
		}
	}
	public override void UpdateEquips() {
		if (Player.HasBuff<Unforgiving_BoltFury>()) {
			int bufftime = Player.buffTime[Player.FindBuffIndex(ModContent.BuffType<Unforgiving_BoltFury>())];
			if (bufftime % 5 == 0) {
				Vector2 newPos = Player.Center + Main.rand.NextVector2CircularEdge(50, 50) * Main.rand.NextFloat(.9f, 1.4f);
				Vector2 vel = (Main.MouseWorld - newPos).SafeNormalize(Vector2.Zero);
				int damage = 5;
				if (Player.IsHeldingModItem<Unforgiving>()) {
					damage += Player.GetWeaponDamage(Player.HeldItem) / 2;
				}
				Projectile.NewProjectile(Player.GetSource_ItemUse(Player.HeldItem),
					newPos, Main.rand.NextVector2CircularEdge(2, 2) + vel, ModContent.ProjectileType<Unforgiving_Bolt>(), damage, 3f, Player.whoAmI);
			}
		}
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.Check_ItemTypeSource<Unforgiving>()) {
			int value = proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().CustomDataValue;
			if (Main.rand.NextBool(4) || Counter >= 120 && Unforgiving_GunMode) {
				int time = ModUtils.ToSecond(Main.rand.Next(1, 3));
				target.AddBuff(BuffID.ShadowFlame, time);
			}
			if (DamageBucketCap == 0) {
				DamageBucketCap = hit.Damage * 10;
			}
			DamageBucket += hit.Damage;
			if (value == 1) {
				target.AddBuff<Unforgiving_Curse>(ModUtils.ToSecond(Main.rand.Next(1, 5)));
			}
		}
		if (proj.type == ModContent.ProjectileType<Unforgiving_Bolt>() && target.HasBuff(BuffID.ShadowFlame)) {
			Projectile.NewProjectile(proj.GetSource_FromThis(), target.Center + Main.rand.NextVector2CircularEdge(100, 100), Vector2.Zero, ModContent.ProjectileType<Roguelike_SpiritFlame>(), 1 + hit.Damage / 3, 5, Player.whoAmI);
		}
	}
}
public class Unforgiving_BoltFury : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
}
public class Unforgiving_Bolt : ModProjectile {
	public override string Texture => ModTexture.SMALLWHITEBALL;
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 100;
		ProjectileID.Sets.TrailingMode[Type] = 0;
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 15;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.penetrate = 1;
		Projectile.timeLeft = 6000;
		Projectile.extraUpdates = 10;
	}
	Vector2 InitialMousePos = Vector2.Zero;
	public override void OnSpawn(IEntitySource source) {
		InitialMousePos = Main.MouseWorld;
		Projectile.ai[0] = -1;
	}
	public NPC getNPC() {
		int whoAmI = (int)Projectile.ai[0];
		if (whoAmI == -1 || whoAmI >= Main.maxNPCs) {
			return null;
		}
		NPC npc = Main.npc[whoAmI];
		if (!npc.active || npc.life <= 0) {
			return null;
		}
		return npc;
	}
	public override void AI() {
		if (InitialMousePos == Vector2.Zero) {
			return;
		}
		NPC npc = getNPC();
		if (npc == null) {
			Projectile.velocity += (InitialMousePos - Projectile.Center) * .1f;
			if (Projectile.Center.LookForHostileNPC(out NPC target, 600)) {
				Projectile.ai[0] = target.whoAmI;
			}
			if (Projectile.Center.IsCloseToPosition(InitialMousePos, 30f)) {
				Projectile.Kill();
			}
		}
		else {
			if (Projectile.ai[1] == 0) {
				Projectile.ai[1] = Projectile.velocity.Length();
			}
			if (Projectile.ai[1] > 2) {
				Projectile.ai[1] = 2;
			}
			Projectile.velocity = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * Projectile.ai[1];
		}
		Projectile.velocity = Projectile.velocity.LimitedVelocity(2);
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff<Unforgiving_Mark>(ModUtils.ToSecond(Main.rand.Next(1, 5)));
	}
	public override bool PreDraw(ref Color lightColor) {
		lightColor.R = 255;
		lightColor.B = 255;
		lightColor.G = 0;
		Projectile.DrawTrail(lightColor, .01f);
		return false;
	}
}
public class Unforgiving_Mark : ModBuff {
	public override string Texture => ModTexture.EMPTYDEBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
}
public class Unforgiving_Curse : ModBuff {
	public override string Texture => ModTexture.EMPTYDEBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
}
public class Unforgiving_BlastWave : ModProjectile {
	public override string Texture => ModTexture.SMALLWHITEBALL;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 15;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.penetrate = 1;
		Projectile.timeLeft = 6000;
		Projectile.extraUpdates = 10;
	}
}
