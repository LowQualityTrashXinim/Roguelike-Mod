using System;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Roguelike.Contents.BuffAndDebuff;
using Roguelike.Contents.Projectiles;
using Roguelike.Texture;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Common.Mode.RoguelikeMode.RoguelikeChange.ItemOverhaul.ItemOverhaul.Specific;
using Humanizer;

namespace Roguelike.Contents.Transfixion.WeaponEnchantment;
public class Musket : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.Musket;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.GetModPlayer<PlayerStatsHandle>().Range_CritDamage += .25f;
		if (player.ItemAnimationActive) {
			if (player.itemAnimation == 1) {
				globalItem.Item_Counter2[index] = PlayerStatsHandle.WE_CoolDown(player, 120);
			}
		}
		else {
			globalItem.Item_Counter2[index] = ModUtils.CountDown(globalItem.Item_Counter2[index]);
		}
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		if (globalItem.Item_Counter2[index] <= 0) {
			damage += .5f;
		}
	}
	public override void ModifyCriticalStrikeChance(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float crit) {
		crit += 5;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (item.ammo != AmmoID.Bullet) {
			return;
		}
		if (++globalItem.Item_Counter1[index] >= 15) {
			type = item.useAmmo == AmmoID.Bullet ? type : ProjectileID.Bullet;
			int proj = Projectile.NewProjectile(source, position, velocity * 2f, type, damage, knockback, player.whoAmI);
			Main.projectile[proj].CritChance = 101;
			globalItem.Item_Counter1[index] = 0;
		}
	}
}
public class FlintlockPistol : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.FlintlockPistol;
	}
	public override string ModifyDesc(string desc) {
		return string.Format(desc, $"[i:{ItemID.FlintlockPistol}]");
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		int roll = 1;
		if (item.type == ItemID.FlintlockPistol) {
			roll += 4;
		}
		for (int i = 0; i < roll; i++) {
			if (Main.rand.NextFloat() <= .75f) {
				Projectile.NewProjectile(source, position, velocity, ProjectileID.Bullet, damage, knockback, player.whoAmI);
			}
		}
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage += .1f;
		if (item.type == ItemID.FlintlockPistol) {
			damage += .4f;
		}
	}
}
public class Revolver : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.Revolver;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (Main.rand.NextBool(5)) {
			Projectile.NewProjectile(source, position, velocity, ProjectileID.Bullet, damage, knockback, player.whoAmI);
		}
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter2[index] = ModUtils.CountDown(globalItem.Item_Counter2[index]);
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage += .05f * globalItem.Item_Counter1[index];
	}
	public override void ModifyCriticalStrikeChance(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float crit) {
		crit += 5 * globalItem.Item_Counter1[index];
	}
	public override void OnHitByNPC(int index, EnchantmentGlobalItem globalItem, Player player, NPC npc, Player.HurtInfo hurtInfo) {
		globalItem.Item_Counter1[index] = 0;
	}
	public override void OnHitByProjectile(int index, EnchantmentGlobalItem globalItem, Player player, Projectile proj, Player.HurtInfo hurtInfo) {
		globalItem.Item_Counter1[index] = 0;
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter2[index] > 0) {
			return;
		}
		globalItem.Item_Counter1[index] = Math.Clamp(globalItem.Item_Counter1[index] + 1, 0, 6);
		globalItem.Item_Counter2[index] = PlayerStatsHandle.WE_CoolDown(player, player.itemAnimationMax + 60);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter2[index] > 0) {
			return;
		}
		globalItem.Item_Counter1[index] = Math.Clamp(globalItem.Item_Counter1[index] + 1, 0, 6);
		globalItem.Item_Counter2[index] = PlayerStatsHandle.WE_CoolDown(player, player.itemAnimationMax + 60);
	}
}
public class Minishark : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.Minishark;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
		if (player.ItemAnimationActive) {
			if (player.itemAnimation == player.itemAnimationMax && item.useAmmo == AmmoID.Bullet && Main.rand.NextFloat() <= .01f) {
				player.AddBuff<Freezy>(ModUtils.ToSecond(5));
			}
			if (globalItem.Item_Counter1[index] <= 0) {
				int type = ProjectileID.Bullet;
				if (player.PickAmmo(item, out int proj, out float speed, out int damage, out float knockback, out int ammoID)) {
					if (item.useAmmo == AmmoID.Bullet) {
						type = proj;
					}
				}
				if (speed < 3) {
					speed = 7;
				}
				Vector2 vel = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero).Vector2RotateByRandom(15);
				Projectile.NewProjectile(player.GetSource_ItemUse_WithPotentialAmmo(item, ammoID), player.Center, vel * speed, type, (int)(damage * .15f), knockback, player.whoAmI);
				globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 8);
			}
		}
	}
}
public class TheUndertaker : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.TheUndertaker;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (Main.rand.NextBool(4)) {
			type = item.useAmmo == AmmoID.Bullet ? type : ProjectileID.Bullet;
			float length = velocity.Length();
			for (int i = 0; i < 6; i++) {
				Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(5).Vector2RandomSpread(length * .75f, Main.rand.NextFloat(.35f, .76f)), type, damage, knockback, player.whoAmI);
			}
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == ItemID.TheUndertaker) {
			if (Main.rand.NextBool(10)) {
				player.Heal(1);
			}
			target.AddBuff(ModContent.BuffType<CrimsonAbsorbtion>(), 120);
		}
	}
}
public class Boomstick : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.Boomstick;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (++globalItem.Item_Counter1[index] >= 7) {
			type = item.useAmmo == AmmoID.Bullet ? type : ProjectileID.Bullet;
			for (int i = 0; i < 4; i++) {
				Projectile.NewProjectile(source, position,
					velocity.Vector2RandomSpread(2, Main.rand.NextFloat(.9f, 1.1f)).Vector2RotateByRandom(30), type, damage, knockback, player.whoAmI);
			}
			globalItem.Item_Counter1[index] = 0;
		}
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage += .1f;
	}
}
public class QuadBarrelShotgun : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.QuadBarrelShotgun;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (++globalItem.Item_Counter1[index] >= 21) {
			type = item.useAmmo == AmmoID.Bullet ? type : ProjectileID.Bullet;
			for (int i = 0; i < 6; i++) {
				Projectile.NewProjectile(source, position,
					velocity.Vector2RandomSpread(2, Main.rand.NextFloat(.8f, 1.15f)).Vector2RotateByRandom(40), type, damage, knockback, player.whoAmI);
			}
			globalItem.Item_Counter1[index] = 0;
		}
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage += .12f;
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		++globalItem.Item_Counter1[index];
	}
}
public class Handgun : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.Handgun;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.ModPlayerStats().UpdateCritDamage += .25f;
	}
	public override void ModifyCriticalStrikeChance(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float crit) {
		crit += 5;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage += .1f;
	}
	public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (item.ammo != AmmoID.Bullet) {
			return;
		}
		if (type == ProjectileID.Bullet && Main.rand.NextBool(4)) {
			type = ProjectileID.BulletHighVelocity;
		}
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (item.ammo != AmmoID.Bullet) {
			return;
		}
		if (Main.rand.NextBool()) {
			Projectile.NewProjectile(source, position, velocity, ProjectileID.Bullet, damage, knockback, player.whoAmI);
		}
	}
}
public class Marked : ModBuff {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetStaticDefaults() {
		Main.debuff[Type] = true;
	}
}
public class DemonBow : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.DemonBow;
	}
	public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (item.useAmmo == AmmoID.Arrow && type == ProjectileID.WoodenArrowFriendly) {
			type = ProjectileID.UnholyArrow;
		}
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.FullHPDamage, Additive: 2.5f);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.type == ProjectileID.UnholyArrow && proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type && Main.rand.NextFloat() <= .15f) {
			target.AddBuff(BuffID.ShadowFlame, ModUtils.ToSecond(4));
		}
	}
}
public class TendonBow : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.TendonBow;
	}
	public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (globalItem.Item_Counter3[index] == 0) {
			player.statLife = Math.Clamp(player.statLife - 5, 100, player.statLifeMax2);
			damage = (int)(damage * 1.35);
			for (int i = 0; i < 15; i++) {
				Vector2 vec = Main.rand.NextVector2Circular(7.5f, 7.5f);
				int dust = Dust.NewDust(player.Center, 0, 0, DustID.Crimson, Scale: Main.rand.NextFloat(.75f, 1.25f));
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = vec;
			}
			globalItem.Item_Counter3[index] = PlayerStatsHandle.WE_CoolDown(player, ModUtils.ToSecond(3));
		}
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
		globalItem.Item_Counter3[index] = ModUtils.CountDown(globalItem.Item_Counter3[index]);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.minion) {
			return;
		}
		if (globalItem.Item_Counter1[index] <= 0) {
			target.AddBuff(BuffID.Ichor, 240);
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 12);
			if (++globalItem.Item_Counter2[index] >= 5) {
				player.Heal(1);
				globalItem.Item_Counter2[index] = 0;
			}
		}
	}
}
public class SnowballCannon : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.SnowballCannon;
	}
	public override void ModifyUseSpeed(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float useSpeed) {
		useSpeed += .05f;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(3), ProjectileID.SnowBallFriendly, (int)(damage * .55f), knockback, player.whoAmI);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.type == ProjectileID.SnowBallFriendly && proj.Check_ItemTypeSource(player.HeldItem.type) && Main.rand.NextBool()) {
			target.AddBuff(BuffID.Frostburn, ModUtils.ToSecond(3));
		}
	}
}
public class Blowpipe : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.Blowpipe;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void ModifyCriticalStrikeChance(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float crit) {
		if (globalItem.Item_Counter1[index] <= 0) {
			crit += 15;
		}
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, ModUtils.ToSecond(4));
		Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(3), ProjectileID.Seed, (int)(damage * .35f), knockback, player.whoAmI);
		if (++globalItem.Item_Counter2[index] >= 5) {
			globalItem.Item_Counter2[index] = 0;
			Projectile proj = Projectile.NewProjectileDirect(source, position, velocity.Vector2RotateByRandom(3), ProjectileID.Seed, damage, knockback, player.whoAmI);
			proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().SetCrit++;
		}
	}
}
public class PainterPaintballGun : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.PainterPaintballGun;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (globalItem.Item_Counter1[index] <= 0) {
			for (int i = 0; i < 3; i++) {
				Projectile.NewProjectile(source, position, velocity.Vector2DistributeEvenlyPlus(3, 30, i), ProjectileID.PainterPaintball, (int)(damage * .45f), knockback, player.whoAmI, 0, Main.rand.NextFloat());
			}
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 60);
		}
	}
}
public class RedRyder : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.RedRyder;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (globalItem.Item_Counter1[index] <= 30) {
			if (++globalItem.Item_Counter2[index] >= 5) {
				globalItem.Item_Counter2[index] = 0;
				damage = damage * 5 + 1;
			}
			else {
				damage = (int)(damage * 1.25f) + 1;
			}
			Projectile.NewProjectile(source, position, velocity * 2f, ProjectileID.BulletHighVelocity, (int)(damage * 1.25f) + 1, knockback, player.whoAmI);
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 30 + player.itemAnimationMax);
		}
	}
}
public class BloodRainBow : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.BloodRainBow;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.RangeDMG, 1.05f);
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		Vector2 pos = Main.rand.NextVector2Circular(25, 300).Add(0, Main.rand.NextFloat(-100, 100) + 800) + Main.MouseWorld;
		Vector2 vel = (Main.MouseWorld + Main.rand.NextVector2Circular(20, 20) - pos).SafeNormalize(Vector2.Zero) * (item.shootSpeed > 0 ? item.shootSpeed : 5);
		Projectile.NewProjectile(source, pos, vel, ProjectileID.BloodArrow, (int)(damage * .85f), knockback, player.whoAmI);
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		IEntitySource entitySource = player.GetSource_ItemUse(item);
		int damage = (int)(hit.Damage * .85f);
		for (int i = 0; i < 3; i++) {
			Vector2 pos = Main.rand.NextVector2Circular(25, 100).Add(0, Main.rand.NextFloat(-100, 100) + 800) + Main.MouseWorld;
			Vector2 vel = (pos - Main.MouseWorld).SafeNormalize(Vector2.Zero);
			Projectile.NewProjectile(entitySource, pos, vel, ProjectileID.BloodArrow, damage, hit.Knockback, player.whoAmI);
		}
	}
}
public class MoltenFury : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.MoltenFury;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.RangeDMG, 1.15f);
	}
	public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (type == ProjectileID.WoodenArrowFriendly) {
			type = ProjectileID.FireArrow;
		}
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (velocity.Length() < 3) {
			velocity = velocity.SafeNormalize(Vector2.Zero) * 7;
		}
		for (int i = 0; i < 2; i++) {
			int proj = Projectile.NewProjectile(source, position + Main.rand.NextVector2Circular(40, 40), velocity, ProjectileID.FireArrow, damage, knockback, player.whoAmI);
			Main.projectile[proj].extraUpdates += 1;
		}
		globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, ModUtils.ToSecond(3));
	}
}
public class BeeKnees : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.BeesKnees;
	}
	public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (type == ProjectileID.WoodenArrowFriendly) {
			type = ProjectileID.BeeArrow;
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType != player.HeldItem.type) {
			return;
		}

		target.AddBuff(BuffID.Poisoned, ModUtils.ToSecond(Main.rand.Next(8, 15)));

		if (Main.rand.NextBool(3)) {
			int amount = Main.rand.Next(1, 4);
			float speed = proj.velocity.Length();
			int damage = (int)(proj.damage * .77f);
			if (player.strongBees) {
				damage = (int)(damage * 1.12f);
				amount += 2;
				speed *= 1.5f;
			}
			for (int i = 0; i < amount; i++) {
				Vector2 pos = target.Center + Main.rand.NextVector2CircularEdge(target.width, target.height) * 1.2f;
				Vector2 vel = (pos - target.Center).SafeNormalize(Vector2.Zero) * speed;
				Projectile.NewProjectile(proj.GetSource_OnHit(target), pos, vel, ProjectileID.Bee, damage, proj.knockBack, player.whoAmI);
			}
		}
	}
}
public class ClockworkAssaultRifle : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.ClockworkAssaultRifle;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		if (player.ItemAnimationJustStarted) {
			globalItem.Item_Counter1[index] = Math.Clamp(globalItem.Item_Counter1[index] + 1, 0, 11);
		}
		if (globalItem.Item_Counter2[index] == 1) {
			SoundEngine.PlaySound(item.UseSound);
			for (int i = 0; i < 3; i++) {
				Vector2 velocity = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero).Vector2RotateByRandom(20) * ((item.shootSpeed > 3 ? item.shootSpeed : 4) + Main.rand.NextFloat(1, 3));
				Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, velocity, item.shoot, item.damage, item.knockBack, player.whoAmI);
			}
			globalItem.Item_Counter1[index] = 0;
			globalItem.Item_Counter2[index] = 0;
		}
	}
	public override void ModifyUseSpeed(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float useSpeed) {
		if (globalItem.Item_Counter2[index] == 0 && globalItem.Item_Counter1[index] > 3) {
			if (globalItem.Item_Counter1[index] >= 7) {
				globalItem.Item_Counter2[index] = 1;
			}
			useSpeed += 2;
		}
		else {
			useSpeed += .2f;
		}
	}
}

public class SandGun : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.Sandgun;
	}

	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index]--;
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.CritChance, Base: 10);
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (Main.rand.NextFloat() <= .35f) {
			if (!velocity.IsLimitReached(7)) {
				velocity = velocity.SafeNormalize(Vector2.Zero) * 7;
			}
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<SandProjectile>(), damage / 2, 0, player.whoAmI);
		}
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (target.life - damageDone <= 0 && globalItem.Item_Counter1[index] <= 0)
			for (int i = 0; i < 15; i++) {
				Projectile.NewProjectile(player.GetSource_OnHit(target), target.Center, Main.rand.NextVector2Circular(15, 15), ModContent.ProjectileType<SandProjectile>(), 15, 0, player.whoAmI);
				globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 5);
			}
	}

	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (target.life - damageDone <= 0 && globalItem.Item_Counter1[index] <= 0)
			for (int i = 0; i < 15; i++) {
				Projectile.NewProjectile(player.GetSource_OnHit(target), target.Center, Main.rand.NextVector2Circular(15, 15), ModContent.ProjectileType<SandProjectile>(), 15, 0, player.whoAmI);
				globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 5);
			}
	}
}

public class PalladiumRepeater : PalladiumEnchantment {

	public override void SetDefaults() {
		ItemIDType = ItemID.PalladiumRepeater;
	}

}

public class OrichalcumRepeater : OrichalcumEnchantment {

	public override void SetDefaults() {
		ItemIDType = ItemID.OrichalcumRepeater;
	}

}

public class TitaniumRepeater : TitaniumEnchantment {

	public override void SetDefaults() {
		ItemIDType = ItemID.TitaniumRepeater;
	}

}

public class OnyxBlaster : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.OnyxBlaster;
	}
	public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (velocity.IsLimitReached(10)) {
			velocity = velocity.SafeNormalize(Vector2.Zero) * 10;
		}
		type = ModContent.ProjectileType<OnyxBulletProjectile>();
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		int chance = Math.Max(1, player.itemAnimationMax - 50);
		if (Main.rand.NextBool(chance)) {
			Projectile.NewProjectile(source, position, velocity, ProjectileID.BlackBolt, damage * 3, knockback * 3, player.whoAmI);
		}
	}
}
public class PhoenixBlaster : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.PhoenixBlaster;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		if (player.ItemAnimationActive && globalItem.Item_Counter1[index] >= 90) {
			globalItem.Item_Counter1[index] = -player.itemAnimationMax;
			if (player.ItemAnimationJustStarted) {
				Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 15, ProjectileID.DD2PhoenixBowShot, player.GetWeaponDamage(item) * 3, 8f, player.whoAmI);
			}
			return;
		}
		if (++globalItem.Item_Counter1[index] > 90) {
			globalItem.Item_Counter1[index] = 90;
		}
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (++globalItem.Item_Counter2[index] >= 10) {
			Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(30), ProjectileID.Flamelash, (int)(damage * 1.5f), knockback, player.whoAmI);
			globalItem.Item_Counter2[index] = -1;
		}
	}
}
public class StarCannon : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.StarCannon;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		if (++globalItem.Item_Counter2[index] >= 5) {
			globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
		}
		if (globalItem.Item_Counter1[index] >= 100) {
			player.GetDamage(DamageClass.Generic) += .15f;
			player.GetCritChance(DamageClass.Generic) += 5;
		}
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (globalItem.Item_Counter1[index] >= 200) {
			globalItem.Item_Counter1[index] -= 200;
			damage = 100 + (int)(damage * .35f);
			Projectile.NewProjectile(source, position, velocity, ProjectileID.StarCannonStar, damage, knockback, player.whoAmI);
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.Check_ItemTypeSource(player.HeldItem.type)) {
			globalItem.Item_Counter1[index] += hit.Damage;
		}
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		globalItem.Item_Counter1[index] += hit.Damage;
	}
}
public class HellwingBow : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.HellwingBow;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage += .15f;
		if (item.useAmmo == AmmoID.Arrow) {
			damage += .2f;
		}
	}
	public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (type == ProjectileID.FireArrow) {
			type = ProjectileID.HellfireArrow;
		}
		if (type == ProjectileID.WoodenArrowFriendly) {
			type = ProjectileID.FireArrow;
		}
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (++globalItem.Item_Counter1[index] >= 3) {
			globalItem.Item_Counter1[index] = 0;
			Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(1), ProjectileID.Hellwing, damage, knockback, player.whoAmI, velocity.X, velocity.Y);
		}
	}
}
public class Marrow : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.Marrow;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage += .15f;
		if (item.useAmmo == AmmoID.Arrow) {
			damage += .1f;
		}
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (++globalItem.Item_Counter1[index] >= 3) {
			globalItem.Item_Counter1[index] = 0;
			Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(1), ProjectileID.BoneArrow, damage, knockback, player.whoAmI);
		}
	}
	public override void ModifyHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (!proj.Check_ItemTypeSource(player.HeldItem.type)) {
			return;
		}
		modifiers.Defense -= .1f;
	}
}
public class IceBow : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.IceBow;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage += .05f;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (++globalItem.Item_Counter1[index] >= 5) {
			globalItem.Item_Counter1[index] = 0;
			Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(1), ProjectileID.FrostArrow, damage, knockback, player.whoAmI);
		}
	}
	public override void ModifyHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.Frostburn) || target.HasBuff(BuffID.Frostburn2)) {
			modifiers.SourceDamage += .35f;
		}
	}
	public override void ModifyHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.Frostburn) || target.HasBuff(BuffID.Frostburn2)) {
			modifiers.SourceDamage += .35f;
		}
	}
}
public class DaedalusStormbow : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.DaedalusStormbow;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, ModUtils.ToSecond(1));
			int damage = player.GetWeaponDamage(item) / 3 + 10;
			for (int i = 0; i < 3; i++) {
				Vector2 pos = target.Center.Add(Main.rand.Next(-100, 100), target.height + 500);
				Projectile.NewProjectile(player.GetSource_ItemUse(item), pos, (target.Center - pos + Main.rand.NextVector2Circular(20, 20)).SafeNormalize(Vector2.Zero), ProjectileID.WoodenArrowFriendly, damage, 1f, player.whoAmI);
			}
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (!proj.Check_ItemTypeSource(player.HeldItem.type) || proj.minion) {
			return;
		}
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, ModUtils.ToSecond(3));
			int damage = hit.Damage / 3 + 10;
			for (int i = 0; i < 3; i++) {
				Vector2 pos = target.Center.Add(Main.rand.Next(-100, 100), target.height + 500);
				Projectile.NewProjectile(proj.GetSource_OnHit(target), pos, (target.Center - pos + Main.rand.NextVector2Circular(20, 20)).SafeNormalize(Vector2.Zero), ProjectileID.WoodenArrowFriendly, damage, 1f, player.whoAmI);
			}
		}
	}
}
public class ShadowFlameBow : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.ShadowFlameBow;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage += .05f;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (++globalItem.Item_Counter1[index] >= 5) {
			globalItem.Item_Counter1[index] = 0;
			Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(1), ProjectileID.ShadowFlameArrow, damage, knockback, player.whoAmI);
		}
	}
	public override void ModifyHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.ShadowFlame)) {
			modifiers.SourceDamage += .35f;
		}
	}
	public override void ModifyHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.ShadowFlame)) {
			modifiers.SourceDamage += .35f;
		}
	}
}
public class PulseBow : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.PulseBow;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage += .11f;
	}
	public override void ModifyCriticalStrikeChance(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float crit) {
		crit += 10;
	}
	public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		type = ProjectileID.PulseBolt;
		velocity = velocity.SafeNormalize(Vector2.Zero) * 20;
	}
	public override void ModifyHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.Check_ItemTypeSource(player.HeldItem.type) && proj.type != ProjectileID.PulseBolt && !proj.minion) {
			modifiers.SourceDamage += .3f;
			modifiers.Defense -= .2f;
		}
	}
}
public class HallowedRepeater : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.HallowedRepeater;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage += .2f;
	}
	public override void ModifyCriticalStrikeChance(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float crit) {
		crit += 5;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0 && proj.Check_ItemTypeSource(player.HeldItem.type) && proj.arrow) {
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 30);
			target.AddBuff(ModContent.BuffType<HallowedGaze>(), ModUtils.ToSecond(5));
		}
	}
}
public class ChlorophyteShotbow : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.ChlorophyteShotbow;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (globalItem.Item_Counter1[index] > 0) {
			return;
		}
		for (int i = 0; i < 3; i++) {
			Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(10),
				Main.rand.NextBool(3) ? ProjectileID.ChlorophyteArrow : ProjectileID.WoodenArrowFriendly, damage, knockback, player.whoAmI);
		}
		globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 120);
	}
}
public class StakeLauncher : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.StakeLauncher;
	}
	public override void ModifyHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.type == NPCID.Vampire || target.type == NPCID.VampireBat) {
			modifiers.SourceDamage += 10;
		}
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (globalItem.Item_Counter1[index] > 0) {
			return;
		}
		if (item.useAmmo == AmmoID.Arrow) {
			Projectile.NewProjectile(source, position, velocity, ProjectileID.Stake, (int)(damage * 1.5f), 5, player.whoAmI);
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, ModUtils.ToSecond(3));
		}
	}
}
public class Shotgun : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.Shotgun;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage -= .1f;
	}
	public override void ModifyCriticalStrikeChance(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float crit) {
		crit -= 10;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (globalItem.Item_Counter1[index] > 0) {
			return;
		}
		if (item.useAmmo == AmmoID.Bullet) {
			for (int i = 0; i < 5; i++) {
				Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(40) * Main.rand.NextFloat(.7f, 1.1f), type, damage, knockback, player.whoAmI);
			}
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, ModUtils.ToSecond(7));
		}
	}
}
public class Gatligator : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.Gatligator;
	}
	public override void ModifyUseSpeed(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float useSpeed) {
		useSpeed += .5f;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage -= .1f;
	}
	public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		velocity = velocity.Vector2RotateByRandom(30);
	}
}
public class Uzi : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.Uzi;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage -= .3f;
		if (item.useAmmo == AmmoID.Bullet)
			damage += .2f;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(15), ProjectileID.BulletHighVelocity, damage, knockback, player.whoAmI);
	}
}
public class Megashark : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.Megashark;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
		if (player.ItemAnimationActive) {
			if (globalItem.Item_Counter1[index] <= 0) {
				int type = ProjectileID.Bullet;
				if (player.PickAmmo(item, out int proj, out float speed, out int damage, out float knockback, out int ammoID)) {
					if (item.useAmmo == AmmoID.Bullet) {
						type = proj;
					}
				}
				if (speed < 3) {
					speed = 7;
				}
				Vector2 vel = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero).Vector2RotateByRandom(7);
				Projectile.NewProjectile(player.GetSource_ItemUse_WithPotentialAmmo(item, ammoID), player.Center, vel * speed, type, (int)(damage * .34f), knockback, player.whoAmI);
				if (Main.rand.NextBool(4)) {
					Projectile.NewProjectile(player.GetSource_ItemUse_WithPotentialAmmo(item, ammoID), player.Center, vel.Vector2RotateByRandom(10) * speed, type, (int)(damage * .34f), knockback, player.whoAmI);
				}
				globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 7);
			}
		}
	}
}
public class VenusMagnum : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.VenusMagnum;
	}
	public override string ModifyDesc(string desc) {
		return string.Format(desc, $"[i:{ItemID.VenusMagnum}]");
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage += .3f;
		if (item.useAmmo == AmmoID.Bullet) {
			damage += .2f;
		}
		if (item.type == ItemID.VenusMagnum) {
			damage += .5f;
		}
	}
	public override void ModifyCriticalStrikeChance(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float crit) {
		crit += 3f;
		if (item.useAmmo == AmmoID.Bullet) {
			crit += 5;
		}
		if (item.type == ItemID.VenusMagnum) {
			crit += 5f;
		}
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		if (globalItem.Item_Counter1[index] == 59) {
			Effect(player);
		}
		if (++globalItem.Item_Counter1[index] >= 60) {
			globalItem.Item_Counter1[index] = 60;
		}
	}
	public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (globalItem.Item_Counter1[index] >= 60) {
			damage *= 2;
		}
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (globalItem.Item_Counter1[index] >= 60) {
			globalItem.Item_Counter1[index] = -player.itemAnimationMax;
			for (int i = 0; i < 5; i++) {
				if (item.useAmmo == AmmoID.Bullet) {
					Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(30) * Main.rand.NextFloat(.56f, 1f), ProjectileID.ChlorophyteBullet, (int)(damage * .33f) + 10, knockback, player.whoAmI);
				}
				Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(30) * Main.rand.NextFloat(.56f, 1f), ProjectileID.SporeCloud, (int)(damage * .43f) + 10, knockback, player.whoAmI);
			}
		}
	}
	private void Effect(Player player) {
		for (int o = 0; o < 100; o++) {
			float scale = Main.rand.NextFloat(.76f, 1.1f);
			int dust = Dust.NewDust(player.Center, 0, 0, DustID.GemDiamond, 0, 0, 0, Color.Green, scale);
			Main.dust[dust].velocity = Main.rand.NextVector2CircularEdge(5, 5);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].Dust_GetDust().FollowEntity = true;
			Main.dust[dust].Dust_BelongTo(player);
		}
	}
}
public class TacticalShotgun : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.TacticalShotgun;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage -= .1f;
	}
	public override void ModifyCriticalStrikeChance(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float crit) {
		crit -= 10;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (!Main.rand.NextBool(4)) {
			return;
		}
		int amount = Main.rand.Next(4, 7);
		if (item.useAmmo != AmmoID.Bullet) {
			type = ProjectileID.Bullet;
			amount += Main.rand.Next(1, 7);
		}
		for (int i = 0; i < amount; i++) {
			Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(40) * Main.rand.NextFloat(.7f, 1.1f), type, (int)(damage * .4f), knockback, player.whoAmI);
		}
	}
}
public class SniperRifle : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.SniperRifle;
	}
	public override string ModifyDesc(string desc) {
		return desc.FormatWith([ItemID.SniperRifle, ItemID.Musket]);
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage += .5f;
		if (item.type == ItemID.SniperRifle || item.type == ItemID.Musket) {
			damage += 1;
			damage.Base += 50;
		}
	}
	public override void ModifyCriticalStrikeChance(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float crit) {
		crit += 20;
		if (item.type == ItemID.SniperRifle || item.type == ItemID.Musket) {
			crit += 10;
		}
	}
	public override void ModifyUseSpeed(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float useSpeed) {
		useSpeed -= .5f;
	}
	public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (type == ProjectileID.Bullet) {
			type = ProjectileID.BulletHighVelocity;
		}
	}
}
public class CandyCornRifle : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.CandyCornRifle;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage += .1f;
	}
	public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (item.useAmmo == AmmoID.Bullet) {
			type = ProjectileID.CandyCorn;
			if (velocity.LengthSquared() < 100) velocity = velocity.SafeNormalize(Vector2.Zero) * 10;
		}
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (++globalItem.Item_Counter1[index] >= 5) {
			globalItem.Item_Counter1[index] = 0;
			Vector2 vel = velocity.SafeNormalize(Vector2.Zero) * 10;
			for (int i = 0; i < 3; i++) {
				Projectile.NewProjectile(source, position, vel.Vector2DistributeEvenlyPlus(3, 40, i), ProjectileID.CandyCorn, (int)(damage * .3f), knockback, player.whoAmI);

			}
		}
	}
}
