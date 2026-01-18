using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Contents.Projectiles;
using Roguelike.Texture;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Transfixion.Skill;
public class BroadSwordSpirit : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 545;
		Skill_Duration = ModUtils.ToSecond(1);
		Skill_Type = SkillTypeID.Skill_Summon;
	}
	public override void OnTrigger(Player player, SkillHandlePlayer skillplayer, int duration, int energy) {
		if (player.ownedProjectileCounts[ModContent.ProjectileType<SwordProjectile3>()] < 1) {
			for (int i = 0; i < 3; i++) {
				int damage = (int)player.GetTotalDamage(DamageClass.Melee).ApplyTo(skillplayer.SkillDamage(34));
				float knockback = (int)player.GetTotalKnockback(DamageClass.Melee).ApplyTo(3);
				int proj = Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<SwordProjectile3>(), damage, knockback, player.whoAmI, 0, 0, i);
				if (Main.projectile[proj].ModProjectile is SwordProjectile3 woodproj)
					woodproj.ItemIDtextureValue = Main.rand.Next(TerrariaArrayID.AllOreBroadSword);
			}
		}
	}
}
public class WoodSwordSpirit : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 645;
		Skill_Duration = ModUtils.ToSecond(3);
		Skill_Type = SkillTypeID.Skill_Summon;
	}
	public override void OnTrigger(Player player, SkillHandlePlayer skillplayer, int duration, int energy) {
		if (player.ownedProjectileCounts[ModContent.ProjectileType<SoulWoodSword>()] < 1) {
			int damage = (int)player.GetTotalDamage(DamageClass.Melee).ApplyTo(skillplayer.SkillDamage(24));
			float knockback = (int)player.GetTotalKnockback(DamageClass.Melee).ApplyTo(5);
			int proj = Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<SoulWoodSword>(), damage, knockback, player.whoAmI);
			if (Main.projectile[proj].ModProjectile is SwordProjectile2 woodproj)
				woodproj.ItemIDtextureValue = Main.rand.Next(TerrariaArrayID.AllWoodSword);
		}
	}
}

public class WilloFreeze : ModSkill {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<WilloFreeze>();
	public override void SetDefault() {
		Skill_EnergyRequire = 1185;
		Skill_Duration = ModUtils.ToSecond(4);
		Skill_Type = SkillTypeID.Skill_Summon;
	}
	public override void OnTrigger(Player player, SkillHandlePlayer skillplayer, int duration, int energy) {
		if (player.ownedProjectileCounts[ModContent.ProjectileType<WilloFreezeProjectile>()] < 1) {
			int damage = (int)player.GetTotalDamage(DamageClass.Magic).ApplyTo(skillplayer.SkillDamage(36));
			float knockback = (int)player.GetTotalKnockback(DamageClass.Magic).ApplyTo(5);
			for (int i = 0; i < 4; i++) {
				Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<WilloFreezeProjectile>(), damage, knockback, player.whoAmI, 75, i, 4);
			}
		}
	}
}

public class PowerPlant : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 525;
		Skill_Duration = ModUtils.ToSecond(4);
		Skill_Type = SkillTypeID.Skill_Summon;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		if (player.ownedProjectileCounts[ModContent.ProjectileType<PowerPlantProjectile>()] < 1) {
			Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<PowerPlantProjectile>(), 0, 0, player.whoAmI);
		}
	}
}
public class TransferStation : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 525;
		Skill_Duration = ModUtils.ToSecond(4);
		Skill_Type = SkillTypeID.Skill_Summon;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		if (player.ownedProjectileCounts[ModContent.ProjectileType<TransferStationProjectile>()] < 1) {
			Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<TransferStationProjectile>(), 0, 0, player.whoAmI);
		}
	}
}
public class OrbOfPurity : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 325;
		Skill_Duration = ModUtils.ToSecond(4);
		Skill_Type = SkillTypeID.Skill_Summon;
	}
	public override void OnTrigger(Player player, SkillHandlePlayer skillplayer, int duration, int energy) {
		if (player.ownedProjectileCounts[ModContent.ProjectileType<DiamondSwotaffOrb>()] < 1) {
			int damage = skillplayer.SkillDamage(10);
			Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<DiamondSwotaffOrb>(), damage, 0, player.whoAmI);
		}
	}
}

public class PhoenixBlazingTornado : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 325;
		Skill_Duration = ModUtils.ToSecond(5);
		Skill_Type = SkillTypeID.Skill_Summon;
	}
	public override void OnTrigger(Player player, SkillHandlePlayer skillplayer, int duration, int energy) {
		if (player.ownedProjectileCounts[ModContent.ProjectileType<BlazingTornado>()] < 1) {
			int damage = skillplayer.SkillDamage(120);
			Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<BlazingTornado>(), damage, 0, player.whoAmI);
		}
	}
}

public class DebugCommand : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 777;
		Skill_Duration = ModUtils.ToSecond(10);
		Skill_Type = SkillTypeID.Skill_Summon;
	}
	public override void OnTrigger(Player player, SkillHandlePlayer skillplayer, int duration, int energy) {
		int damage = skillplayer.SkillDamage(2000);
		player.Center.LookForHostileNPC(out var npclist, 2500);
		foreach (var npc in npclist) {
			player.StrikeNPCDirect(npc, npc.CalculateHitInfo(damage, ModUtils.DirectionFromPlayerToNPC(player.Center.X, npc.Center.X)));
		}
		player.AddImmuneTime(-1, duration / 10);
		player.immune = true;
		player.AddBuff<DebugStatus>(ModUtils.ToSecond(duration / 10));
	}
}
public class DebugStatus : ModBuff {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		for (int i = 0; i < player.buffImmune.Length; i++) {
			int buffType = i;
			if (Main.debuff[buffType] && !ModItemLib.TrueDebuff.Contains(buffType)) {
				player.buffImmune[buffIndex] = true;
			}
		}
		player.potionDelayTime = 0;
	}
}

public class LucidNightmares : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 850;
		Skill_Duration = ModUtils.ToSecond(4);
		Skill_Type = SkillTypeID.Skill_Summon;
	}
	public override void OnTrigger(Player player, SkillHandlePlayer skillplayer, int duration, int energy) {
		int damage = skillplayer.SkillDamage(53);
		for (int i = 0; i < 3; i++) {
			Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.UnitX.Vector2DistributeEvenly(3, 360, i) * Main.rand.NextFloat(4, 7), ModContent.ProjectileType<NightmaresProjectile>(), damage, 0, player.whoAmI);
		}
		player.AddBuff<AbyssalAbsorption>(duration);

	}
	public class AbyssalAbsorption : ModBuff {
		public override string Texture => ModTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultBuff();
		}
		public override void Update(Player player, ref int buffIndex) {
			player.ModPlayerStats().TrueDamage.Flat += 10;
		}
	}
}

public class SacrificialWormhole : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 950;
		Skill_Duration = ModUtils.ToSecond(14);
		Skill_Type = SkillTypeID.Skill_Summon;
	}
	public override void OnTrigger(Player player, SkillHandlePlayer skillplayer, int duration, int energy) {
		int damage = skillplayer.SkillDamage(50);
		Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero,
			ModContent.ProjectileType<SacrificialWormholeProjectile>(), damage, 0, player.whoAmI);
		player.AddBuff<LifeLoss>(ModUtils.ToSecond(60));
	}
	public class LifeLoss : ModBuff {
		public override string Texture => ModTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultDeBuff();
		}
		public override void Update(Player player, ref int buffIndex) {
			PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.MaxHP, .5f);
		}
	}
}

public class BulletHell : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 666;
		Skill_Duration = ModUtils.ToSecond(6.66f);
		Skill_Type = SkillTypeID.Skill_Summon;
	}
	public override void OnTrigger(Player player, SkillHandlePlayer skillplayer, int duration, int energy) {
		var modplayer = player.GetModPlayer<SkillHandlePlayer>();
		int weaponDamage = (int)(player.GetWeaponDamage(player.HeldItem) * .5f);
		int damage = (int)player.GetTotalDamage(DamageClass.Ranged).ApplyTo(22) + weaponDamage;
		damage = (int)modplayer.skilldamage.ApplyTo(damage);
		float knockback = (int)player.GetTotalKnockback(DamageClass.Magic).ApplyTo(2);
		Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<BulletHell_Projectile>(), damage, knockback, player.whoAmI);
	}
	public class BulletHell_Projectile : ModProjectile {
		public override string Texture => ModTexture.MissingTexture_Default;
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 1;
			Projectile.penetrate = -1;
			Projectile.hide = true;
			Projectile.friendly = true;
			Projectile.timeLeft = ModUtils.ToSecond(6.66f);
		}
		public override bool? CanDamage() {
			return false;
		}
		public override void AI() {
			int damage = Projectile.damage;
			float knockback = Projectile.knockBack;
			if (Projectile.timeLeft % 100 == 0) {
				for (int i = 0; i < 32; i++) {
					var proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.UnitX.Vector2DistributeEvenlyPlus(32, 360, i) * 10, ProjectileID.GoldenBullet, damage, knockback, Projectile.owner);
					proj.tileCollide = false;
					proj.timeLeft = 120;
				}
			}
			for (int i = 0; i < 8; i++) {
				Projectile proj;
				if (i % 2 == 0) {
					proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.UnitX.RotatedBy(MathHelper.ToRadians(Projectile.timeLeft * 5 + 45 * i)) * 10, ProjectileID.Bullet, damage, knockback, Projectile.owner);
				}
				else {
					proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.UnitX.RotatedBy(MathHelper.ToRadians(Projectile.timeLeft * 5 - 45 * i)) * 10, ProjectileID.Bullet, damage, knockback, Projectile.owner);
				}
				proj.timeLeft = 90;
				proj.tileCollide = true;
			}
		}
	}
}
public class WoodenArrowRain : ModSkill {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<WoodenArrowRain>();
	public override void SetDefault() {
		Skill_EnergyRequire = 1045;
		Skill_Duration = ModUtils.ToSecond(15);
		Skill_Type = SkillTypeID.Skill_Summon;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		var modplayer = player.GetModPlayer<SkillHandlePlayer>();
		if (modplayer.Duration % 5 != 0) {
			return;
		}
		int damage = (int)player.GetTotalDamage(DamageClass.Ranged).ApplyTo(16);
		float knockback = (int)player.GetTotalKnockback(DamageClass.Ranged).ApplyTo(2);
		var position = player.Center;
		position.Y -= 1000;
		position.X += Main.rand.NextFloat(0, 2000) * -modplayer.Skill_DirectionPlayerFaceBeforeSkillActivation;
		Vector2 vel = Vector2.One;
		vel.X *= modplayer.Skill_DirectionPlayerFaceBeforeSkillActivation;
		Projectile proj = skillplayer.NewSkillProjectile(player.GetSource_FromThis("skill"), position, vel, Main.rand.NextFloat(20, 24), ProjectileID.WoodenArrowFriendly, damage, knockback);
		proj.tileCollide = false;
		proj.timeLeft = 180;
		for (int l = 0; l < 2; l++) {
			int dust = Dust.NewDust(position, 0, 0, DustID.Smoke, Scale: Main.rand.NextFloat(3, 4));
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = Main.rand.NextVector2Circular(2, 2);
		}
	}
}

public class SpiritBurst : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 1010;
		Skill_Duration = ModUtils.ToSecond(7);
		Skill_Type = SkillTypeID.Skill_Summon;
	}
	public override void OnTrigger(Player player, SkillHandlePlayer skillplayer, int duration, int energy) {
		for (int i = 0; i < 8; i++) {
			int damage = (int)player.GetTotalDamage(DamageClass.Magic).ApplyTo(44) * 3;
			float knockback = (int)player.GetTotalKnockback(DamageClass.Magic).ApplyTo(2);
			var vel = Main.rand.NextVector2CircularEdge(3, 3);
			skillplayer.NewSkillProjectile(player.GetSource_FromThis("skill"), player.Center, vel, 1, ModContent.ProjectileType<SpiritProjectile>(), damage, knockback);
		}
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		var modplayer = player.GetModPlayer<SkillHandlePlayer>();
		if (Main.rand.NextBool(10) || modplayer.Duration % 15 == 0) {
			int damage = (int)player.GetTotalDamage(DamageClass.Magic).ApplyTo(44);
			float knockback = (int)player.GetTotalKnockback(DamageClass.Magic).ApplyTo(2);
			var vel = Main.rand.NextVector2CircularEdge(2, 2);
			skillplayer.NewSkillProjectile(player.GetSource_FromThis("skill"), player.Center, vel, 1, ModContent.ProjectileType<SpiritProjectile>(), damage, knockback);
		}
	}
}
public class MeteorShower : ModSkill {
	public override string Texture => ModUtils.GetTheSameTextureAs<MeteorShower>("MeteorStrike");
	public override void SetDefault() {
		Skill_EnergyRequire = 1220;
		Skill_Duration = ModUtils.ToSecond(15);
		Skill_Type = SkillTypeID.Skill_Summon;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		Projectile proj;
		int damage;
		float knockback;
		Vector2 position, velocity;
		if (skillplayer.Duration % 9 == 0) {
			damage = (int)player.GetTotalDamage(DamageClass.Magic).ApplyTo(SkillDamage(player, 50));
			knockback = (int)player.GetTotalKnockback(DamageClass.Generic).ApplyTo(10);
			position = player.Center.Add(Main.rand.Next(-200, 200), 1000);
			velocity = (player.Center - position + Main.rand.NextVector2Circular(200, 200)).SafeNormalize(Vector2.Zero);
			proj = skillplayer.NewSkillProjectile(player.GetSource_FromThis(), position, velocity, 20, ProjectileID.StarWrath, damage, knockback);
			proj.friendly = true;
			proj.hostile = false;
			proj.tileCollide = false;
			proj.timeLeft = 600;
		}
		if (skillplayer.Duration % 5 == 0) {
			damage = (int)player.GetTotalDamage(DamageClass.Magic).ApplyTo(SkillDamage(player, 44));
			knockback = (int)player.GetTotalKnockback(DamageClass.Generic).ApplyTo(10);
			position = player.Center.Add(Main.rand.Next(-1000, 1000), 1000);
			velocity = (player.Center - position + Main.rand.NextVector2Circular(200, 200)).SafeNormalize(Vector2.Zero);
			proj = skillplayer.NewSkillProjectile(player.GetSource_FromThis(), position, velocity, 10, Main.rand.Next(new int[] { ProjectileID.Meteor1, ProjectileID.Meteor2, ProjectileID.Meteor3 }), damage, knockback);
			proj.friendly = true;
			proj.hostile = false;
			proj.tileCollide = false;
			proj.timeLeft = 600;
			proj.ai[1] = Main.rand.NextFloat(1, 2);
		}
		skillplayer.SkillDamageWhileActive += .2f;
	}
}
public class EnergyChainReaction : ModSkill {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<EnergyChainReaction>();
	public override void SetDefault() {
		Skill_EnergyRequire = 1300;
		Skill_Duration = ModUtils.ToSecond(16);
		Skill_Type = SkillTypeID.Skill_Summon;
	}
	public override void OnTrigger(Player player, SkillHandlePlayer skillplayer, int duration, int energy) {
		Projectile.NewProjectileDirect(player.GetSource_FromThis("skill"), player.Center, Main.rand.NextVector2CircularEdge(1, 1), ModContent.ProjectileType<EnergyChainOrb>(), 1, 1, player.whoAmI);
	}
	public override void ModifySkillSet(Player player, SkillHandlePlayer modplayer, ref int index, ref StatModifier energy, ref StatModifier duration) {
		int[] currentskillset = modplayer.GetCurrentActiveSkillHolder();
		for (int i = index + 1; i < currentskillset.Length; i++) {
			var skill = SkillModSystem.GetSkill(currentskillset[i]);
			if (skill == null) {
				continue;
			}
			energy += .1f;
			modplayer.SkillDamageWhileActive += .1f;
		}
	}
	public class EnergyChainOrb : ModProjectile {
		public override string Texture => ModTexture.Glow_Big;
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 38;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = ModUtils.ToSecond(14);
		}
		public override void AI() {
			Projectile.velocity *= .9f;
			Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Electric);
			dust.velocity = Main.rand.NextVector2Circular(5, 5);
			dust.noGravity = true;
			dust.scale = Main.rand.NextFloat(.7f, 1.2f);
			Player player = Main.player[Projectile.owner];
			int damage;
			SkillHandlePlayer modplayer = player.GetModPlayer<SkillHandlePlayer>();
			if (Projectile.timeLeft % 50 == 0) {
				damage = (int)player.GetTotalDamage(DamageClass.Magic).ApplyTo(32);
				float knockback = (int)player.GetTotalKnockback(DamageClass.Magic).ApplyTo(2);
				modplayer.NewSkillProjectile(player.GetSource_FromThis("skill"), Projectile.Center, Main.rand.NextVector2CircularEdge(1, 1), 1, ModContent.ProjectileType<ElectricChainBolt>(), damage, knockback);
			}
			if (Projectile.timeLeft % 10 == 0) {
				damage = SkillDamage(player, 30);
				var pos = Projectile.Center;
				var toMouse = Main.rand.NextVector2CircularEdge(1, 1);
				Projectile proj = modplayer.NewSkillProjectile(player.GetSource_Misc("Skill"), pos, toMouse, 3, ModContent.ProjectileType<EnergyBoltProjectile>(), damage, 2f);
				proj.extraUpdates += 3;
				proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().EnergyRegainOnHit += damage / 3;

			}
		}
		public override bool PreDraw(ref Color lightColor) {
			ModUtils.Draw_SetUpToDrawGlow(Main.spriteBatch);
			Main.instance.LoadProjectile(Type);
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			Vector2 origin = texture.Size() * .5f;
			Vector2 drawpos = Projectile.position - Main.screenPosition + origin;
			Main.EntitySpriteDraw(texture, drawpos, null, Color.Cyan, 0, origin, 3, SpriteEffects.None);
			ModUtils.Draw_ResetToNormal(Main.spriteBatch);
			return false;
		}
	}
}
