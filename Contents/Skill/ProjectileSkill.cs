using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Roguelike.Common.Utils;
using Roguelike.Contents.Projectiles;
 
using Roguelike.Common.Global;

namespace Roguelike.Contents.Skill;
public class Skill_BoneArrow : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 65;
		Skill_Duration = ModUtils.ToSecond(1);
		Skill_Type = SkillTypeID.Skill_Projectile;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		SkillHandlePlayer modplayer = player.GetModPlayer<SkillHandlePlayer>();
		if (modplayer.Duration % 30 != 0) {
			return;
		}
		int damage = (int)player.GetTotalDamage(DamageClass.Ranged).ApplyTo(10);
		float knockback = (int)player.GetTotalKnockback(DamageClass.Ranged).ApplyTo(2);
		for (int i = 0; i < 16; i++) {
			Vector2 vel = Vector2.One.Vector2DistributeEvenlyPlus(16, 360, i).RotatedBy(MathHelper.ToRadians(modplayer.Duration));
			List<Projectile> projlist = skillplayer.NewSkillProjectile(player.GetSource_FromThis(), player.Center, vel, 16f, ProjectileID.BoneArrow, damage, knockback);
			foreach (var proj in projlist) {
				proj.tileCollide = false;
				proj.timeLeft = 180;
			}
		}
	}
}
public class WoodenArrowRain : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 145;
		Skill_Duration = ModUtils.ToSecond(1);
		Skill_Type = SkillTypeID.Skill_Projectile;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		SkillHandlePlayer modplayer = player.GetModPlayer<SkillHandlePlayer>();
		if (modplayer.Duration % 5 != 0) {
			return;
		}
		int damage = (int)player.GetTotalDamage(DamageClass.Ranged).ApplyTo(6);
		float knockback = (int)player.GetTotalKnockback(DamageClass.Ranged).ApplyTo(2);
		Vector2 position = player.Center;
		position.Y -= 1000;
		position.X += Main.rand.NextFloat(0, 2000) * -modplayer.Skill_DirectionPlayerFaceBeforeSkillActivation;
		List<Projectile> projlist = skillplayer.NewSkillProjectile(player.GetSource_FromThis(), position, Vector2.One * modplayer.Skill_DirectionPlayerFaceBeforeSkillActivation, Main.rand.NextFloat(20, 24), ProjectileID.WoodenArrowFriendly, damage, knockback);
		foreach (var proj in projlist) {
			proj.tileCollide = false;
			proj.timeLeft = 180;
		}
		for (int l = 0; l < 2; l++) {
			int dust = Dust.NewDust(position, 0, 0, DustID.Smoke, Scale: Main.rand.NextFloat(3, 4));
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = Main.rand.NextVector2Circular(2, 2);
		}
	}
}
public class ChaosArrowRain : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 385;
		Skill_Duration = ModUtils.ToSecond(1.2f);
		Skill_Type = SkillTypeID.Skill_Projectile;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		SkillHandlePlayer modplayer = player.GetModPlayer<SkillHandlePlayer>();
		if (modplayer.Duration % 5 != 0) {
			return;
		}
		int damage = (int)player.GetTotalDamage(DamageClass.Ranged).ApplyTo(Main.rand.Next(6, 19));
		float knockback = (int)player.GetTotalKnockback(DamageClass.Ranged).ApplyTo(2);
		Vector2 position = Main.MouseWorld;
		position.Y -= 500;
		position.X += Main.rand.NextFloat(-75, 75);
		Vector2 vel = Vector2.UnitY * Main.rand.NextFloat(20, 24);
		int type = Main.rand.Next(TerrariaArrayID.Arrow);
		if (type == ProjectileID.ShimmerArrow) {
			vel *= -1;
			position.Y += 1000;
		}
		List<Projectile> projlist = skillplayer.NewSkillProjectile(player.GetSource_FromThis(), position, vel, Main.rand.NextFloat(20, 24), type, damage, knockback);
		foreach (var proj in projlist) {
			proj.tileCollide = false;
			proj.timeLeft = 180;
		}
		for (int l = 0; l < 2; l++) {
			int dust = Dust.NewDust(position, 0, 0, DustID.Smoke, Scale: Main.rand.NextFloat(3, 4));
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = Main.rand.NextVector2Circular(2, 2);
		}
	}
}

public class SpiritBurst : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 210;
		Skill_Duration = ModUtils.ToSecond(1);
		Skill_CoolDown = ModUtils.ToSecond(10);
		Skill_Type = SkillTypeID.Skill_Projectile;
	}
	public override void OnTrigger(Player player, SkillHandlePlayer skillplayer, int duration, int cooldown, int energy) {
		for (int i = 0; i < 5; i++) {
			int damage = (int)player.GetTotalDamage(DamageClass.Magic).ApplyTo(44);
			float knockback = (int)player.GetTotalKnockback(DamageClass.Magic).ApplyTo(2);
			Vector2 vel = Main.rand.NextVector2CircularEdge(4, 4);
			skillplayer.NewSkillProjectile(player.GetSource_FromThis(), player.Center, vel, 1, ModContent.ProjectileType<SpiritProjectile>(), damage, knockback);
		}
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		SkillHandlePlayer modplayer = player.GetModPlayer<SkillHandlePlayer>();
		if (Main.rand.NextBool(10) || modplayer.Duration % 15 == 0) {
			int damage = (int)player.GetTotalDamage(DamageClass.Magic).ApplyTo(44);
			float knockback = (int)player.GetTotalKnockback(DamageClass.Magic).ApplyTo(2);
			Vector2 vel = Main.rand.NextVector2CircularEdge(4, 4);
			List<Projectile> projlist = skillplayer.NewSkillProjectile(player.GetSource_FromThis(), player.Center, vel, 1, ModContent.ProjectileType<SpiritProjectile>(), damage, knockback);
		}
	}
}
public class Icicle : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 220;
		Skill_Duration = ModUtils.ToSecond(1f);
		Skill_Type = SkillTypeID.Skill_Projectile;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		SkillHandlePlayer modplayer = player.GetModPlayer<SkillHandlePlayer>();
		if (modplayer.Duration % 20 != 0) {
			return;
		}
		int damage = (int)player.GetTotalDamage(DamageClass.Magic).ApplyTo(9);
		float knockback = (int)player.GetTotalKnockback(DamageClass.Magic).ApplyTo(2);
		float rotation = MathHelper.ToRadians(Main.rand.NextFloat(90));
		int Index = Main.rand.Next(6);
		for (int i = 0; i < 6; i++) {
			if (i != Index) {
				continue;
			}
			Vector2 pos = player.Center + Vector2.One.Vector2DistributeEvenlyPlus(6, 360, i).RotatedBy(rotation) * 50;
			for (int l = 0; l < 6; l++) {
				int dust = Dust.NewDust(pos, 0, 0, DustID.Cloud, Scale: Main.rand.NextFloat(1, 2));
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = Main.rand.NextVector2Circular(2, 2);
			}
			Vector2 vel = (Main.MouseWorld + Main.rand.NextVector2Circular(50, 50) - pos).SafeNormalize(Vector2.Zero);
			skillplayer.NewSkillProjectile(player.GetSource_FromThis(), pos, vel, 10 + Main.rand.NextFloat(-3, 3), ProjectileID.Blizzard, damage, knockback);
		}
	}
	public override void AlwaysUpdate(Player player, SkillHandlePlayer skillplayer) {
		skillplayer.ProjectileCritDamage += .1f;
		skillplayer.ProjectileCritChance += 5;
	}
}
public class StarFury : ModSkill {
	public override string Texture => ModUtils.GetTheSameTextureAs<StarFury>("StarFall");
	public override void SetDefault() {
		Skill_EnergyRequire = 325;
		Skill_Duration = ModUtils.ToSecond(2);
		Skill_Type = SkillTypeID.Skill_Projectile;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		SkillHandlePlayer modplayer = player.GetModPlayer<SkillHandlePlayer>();
		if (modplayer.Duration % 90 != 0) {
			return;
		}
		int damage = (int)player.GetTotalDamage(DamageClass.Generic).ApplyTo(SkillDamage(player, 1000));
		float knockback = (int)player.GetTotalKnockback(DamageClass.Generic).ApplyTo(10);
		Vector2 position = player.Center.Add(0, 1000);
		Vector2 velocity = (Main.MouseWorld - position).SafeNormalize(Vector2.Zero) * 20f;
		int proj = Projectile.NewProjectile(player.GetSource_FromThis(), position, velocity, ProjectileID.StarWrath, damage, knockback, player.whoAmI);
		Main.projectile[proj].friendly = true;
		Main.projectile[proj].hostile = false;
		Main.projectile[proj].tileCollide = false;
		Main.projectile[proj].timeLeft = 600;
	}
}
public class MeteorShower : ModSkill {
	public override string Texture => ModUtils.GetTheSameTextureAs<MeteorShower>("MeteorStrike");
	public override void SetDefault() {
		Skill_EnergyRequire = 220;
		Skill_Duration = ModUtils.ToSecond(.4f);
		Skill_CoolDown = ModUtils.ToSecond(12);
		Skill_Type = SkillTypeID.Skill_Projectile;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		SkillHandlePlayer modplayer = player.GetModPlayer<SkillHandlePlayer>();
		if (modplayer.Duration % 5 != 0) {
			return;
		}
		int damage = (int)player.GetTotalDamage(DamageClass.Magic).ApplyTo(SkillDamage(player, 44));
		float knockback = (int)player.GetTotalKnockback(DamageClass.Generic).ApplyTo(10);
		Vector2 position = player.Center.Add(Main.rand.Next(-1000, 1000), 1000);
		Vector2 velocity = (Main.MouseWorld - position + Main.rand.NextVector2Circular(200, 200)).SafeNormalize(Vector2.Zero) * 10f;
		int proj = Projectile.NewProjectile(player.GetSource_FromThis(), position, velocity, Main.rand.Next(new int[] { ProjectileID.Meteor1, ProjectileID.Meteor2, ProjectileID.Meteor3 }), damage, knockback, player.whoAmI, ai1: Main.rand.NextFloat(1, 2));
		Main.projectile[proj].friendly = true;
		Main.projectile[proj].hostile = false;
		Main.projectile[proj].tileCollide = false;
		Main.projectile[proj].timeLeft = 600;
	}
}
public class BulletStorm : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 255;
		Skill_Duration = ModUtils.ToSecond(2);
		Skill_CoolDown = ModUtils.ToSecond(19);
		Skill_Type = SkillTypeID.Skill_Projectile;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		if (skillplayer.Duration % 3 != 0) {
			return;
		}
		Vector2 spawn = player.Center.Add(0, 1000);
		int damage = (int)player.GetDamage(DamageClass.Ranged).ApplyTo(SkillDamage(player, 14));
		for (int i = 0; i < 3; i++) {
			float RandomizeX = Main.rand.NextFloat(-300, 300);
			Projectile.NewProjectile(player.GetSource_Misc("Skill"), spawn.Add(RandomizeX, 0) + Main.rand.NextVector2Circular(300, 300), Vector2.UnitY * Main.rand.Next(10, 14), Main.rand.Next(TerrariaArrayID.Bullet), damage, 2);
		}
	}
}
public class EnergyBolt : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 200;
		Skill_Duration = ModUtils.ToSecond(1);
		Skill_CoolDown = ModUtils.ToSecond(5);
		Skill_Type = SkillTypeID.Skill_Projectile;
	}
	public override void ModifySkillSet(Player player, SkillHandlePlayer modplayer, ref int index, ref StatModifier energy, ref StatModifier duration, ref StatModifier cooldown) {
		int[] currentskillset = modplayer.GetCurrentActiveSkillHolder();
		for (int i = index + 1; i < currentskillset.Length; i++) {
			ModSkill skill = SkillModSystem.GetSkill(currentskillset[i]);
			if (skill == null) {
				continue;
			}
			energy += .1f;
			modplayer.SkillDamageWhileActive += .1f;
		}
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		SkillHandlePlayer modplayer = player.GetModPlayer<SkillHandlePlayer>();
		if (modplayer.Duration % 10 != 0) {
			return;
		}
		int damage = SkillDamage(player, 30);
		Vector2 pos = player.Center + Main.rand.NextVector2Circular(150, 150);
		Vector2 toMouse = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero).Vector2RotateByRandom(20);
		List<Projectile> projlist = skillplayer.NewSkillProjectile(player.GetSource_Misc("Skill"), pos, toMouse, 3, ModContent.ProjectileType<EnergyBoltProjectile>(), damage, 2f);
		foreach (var item in projlist) {
			item.extraUpdates += 3;
			item.GetGlobalProjectile<RoguelikeGlobalProjectile>().EnergyRegainOnHit += damage / 3;
		}
	}
}
public class ElectricChain : ModSkill {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<ElectricChain>();
	public override void SetDefault() {
		Skill_EnergyRequire = 145;
		Skill_Duration = ModUtils.ToSecond(1.5f);
		Skill_CoolDown = ModUtils.ToSecond(15);
		Skill_Type = SkillTypeID.Skill_Projectile;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		SkillHandlePlayer modplayer = player.GetModPlayer<SkillHandlePlayer>();
		if (modplayer.Duration % 50 != 0) {
			return;
		}
		int damage = (int)player.GetTotalDamage(DamageClass.Magic).ApplyTo(32);
		float knockback = (int)player.GetTotalKnockback(DamageClass.Magic).ApplyTo(2);
		Projectile.NewProjectileDirect(player.GetSource_FromThis(), player.Center, Main.rand.NextVector2CircularEdge(1, 1), ModContent.ProjectileType<ElectricChainBolt>(), damage, knockback, player.whoAmI);
	}
}
public class BulletHell : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 666;
		Skill_Duration = ModUtils.ToSecond(6.66f);
		Skill_CoolDown = ModUtils.ToSecond(15);
		Skill_Type = SkillTypeID.Skill_Projectile;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		SkillHandlePlayer modplayer = player.GetModPlayer<SkillHandlePlayer>();
		int weaponDamage = (int)(player.GetWeaponDamage(player.HeldItem) * .5f);
		int damage = (int)player.GetTotalDamage(DamageClass.Ranged).ApplyTo(22) + weaponDamage;
		damage = (int)modplayer.skilldamage.ApplyTo(damage);
		float knockback = (int)player.GetTotalKnockback(DamageClass.Magic).ApplyTo(2);
		if (modplayer.Duration % 100 == 0) {
			for (int i = 0; i < 32; i++) {
				Projectile proj = Projectile.NewProjectileDirect(player.GetSource_FromThis(), player.Center, Vector2.UnitX.Vector2DistributeEvenlyPlus(32, 360, i) * 10, ProjectileID.GoldenBullet, damage, knockback, player.whoAmI);
				proj.tileCollide = false;
				proj.timeLeft = 120;
			}
		}
		for (int i = 0; i < 8; i++) {
			Projectile proj;
			if (i % 2 == 0) {
				proj = Projectile.NewProjectileDirect(player.GetSource_FromThis(), player.Center, Vector2.UnitX.RotatedBy(MathHelper.ToRadians(modplayer.Duration + 45 * i)) * 10, ProjectileID.Bullet, damage, knockback, player.whoAmI);
			}
			else {
				proj = Projectile.NewProjectileDirect(player.GetSource_FromThis(), player.Center, Vector2.UnitX.RotatedBy(MathHelper.ToRadians(-modplayer.Duration - 45 * i)) * 10, ProjectileID.Bullet, damage, knockback, player.whoAmI);
			}
			proj.timeLeft = 90;
			proj.tileCollide = true;
		}
	}
}
