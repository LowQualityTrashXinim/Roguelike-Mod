using Microsoft.Xna.Framework;
using Mono.Cecil;
using Roguelike.Common.Global;
using Roguelike.Common.RoguelikeMode.ItemOverhaul.Specific;
using Roguelike.Common.Utils;
using Roguelike.Contents.BuffAndDebuff;
using Roguelike.Contents.Projectiles;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Roguelike.Contents.Transfixion.WeaponEnchantment.MagnetSphere;

namespace Roguelike.Contents.Transfixion.WeaponEnchantment;
public class AmethystStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.AmethystStaff;
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		reduce -= .15f;
	}
	public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		type = ProjectileID.AmethystBolt;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (!velocity.IsLimitReached(3)) {
			velocity = velocity.SafeNormalize(Vector2.Zero) * 4;
		}
		if (Main.rand.NextFloat() <= .65f) {
			Vector2 newPos = position + Main.rand.NextVector2Circular(40, 40);
			int proj = Projectile.NewProjectile(source, newPos, (Main.MouseWorld - newPos).SafeNormalize(Vector2.Zero) * velocity.Length(), ProjectileID.AmethystBolt, damage, knockback, player.whoAmI);
			Main.projectile[proj].extraUpdates += 1;
		}
		if (Main.rand.NextFloat() <= .37f) {
			for (int i = 0; i < 3; i++) {
				Vector2 newVel = velocity.Vector2DistributeEvenlyPlus(3, 30, i);
				Projectile.NewProjectile(source, position, newVel, ProjectileID.AmethystBolt, damage, knockback, player.whoAmI);
			}
		}
	}
}
public class TopazStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.TopazStaff;
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		reduce -= .25f;
	}
	public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		type = ProjectileID.TopazBolt;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (!velocity.IsLimitReached(3)) {
			velocity = velocity.SafeNormalize(Vector2.Zero) * 4;
		}
		if (Main.rand.NextFloat() <= .47f) {
			Vector2 newPos = position + Main.rand.NextVector2Circular(40, 40);
			Projectile.NewProjectile(source, newPos, (Main.MouseWorld - newPos).SafeNormalize(Vector2.Zero) * velocity.Length(), ProjectileID.TopazBolt, damage, knockback, player.whoAmI);
		}
	}
}
public class SapphireStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.SapphireStaff;
	}
	public override void OnConsumeMana(int index, Player player, EnchantmentGlobalItem globalItem, Item item, int consumedMana) {
		if (Main.rand.NextFloat() <= .18f) {
			player.statMana = Math.Clamp(player.statMana + consumedMana, 0, player.statManaMax2);
		}
	}
	public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		type = ProjectileID.SapphireBolt;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (!velocity.IsLimitReached(3)) {
			velocity = velocity.SafeNormalize(Vector2.Zero) * 4;
		}
		if (Main.rand.NextFloat() <= .37f) {
			Vector2 newPos = position + Main.rand.NextVector2Circular(40, 40);
			Projectile.NewProjectile(source, newPos, (Main.MouseWorld - newPos).SafeNormalize(Vector2.Zero) * velocity.Length(), ProjectileID.SapphireBolt, damage, knockback, player.whoAmI);
		}
	}
}
public class EmeraldStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.EmeraldStaff;
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		reduce -= .15f;
	}
	public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		type = ProjectileID.EmeraldBolt;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (!velocity.IsLimitReached(3)) {
			velocity = velocity.SafeNormalize(Vector2.Zero) * 4;
		}
		if (Main.rand.NextFloat() <= .47f) {
			Vector2 newPos = position + Main.rand.NextVector2Circular(40, 40);
			int proj = Projectile.NewProjectile(source, newPos, (Main.MouseWorld - newPos).SafeNormalize(Vector2.Zero) * velocity.Length(), ProjectileID.EmeraldBolt, damage, knockback, player.whoAmI);
			if (Main.rand.NextFloat() <= .32f) {
				Main.projectile[proj].extraUpdates++;
				Main.projectile[proj].damage += (int)(damage * .5f);
			}
		}
	}
}
public class RubyStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.RubyStaff;
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		multi += .2f;
	}
	public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		type = ProjectileID.RubyBolt;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (!velocity.IsLimitReached(3)) {
			velocity = velocity.SafeNormalize(Vector2.Zero) * 4;
		}
		if (Main.rand.NextFloat() <= .17f) {
			Vector2 newPos = position + Main.rand.NextVector2Circular(40, 40);
			Projectile.NewProjectile(source, newPos, (Main.MouseWorld - newPos).SafeNormalize(Vector2.Zero) * velocity.Length(), ProjectileID.RubyBolt, damage, knockback, player.whoAmI);
		}
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnMissingMana(int index, Player player, EnchantmentGlobalItem globalItem, Item item, int neededMana) {
		if (globalItem.Item_Counter1[index] > 0) {
			return;
		}
		globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, ModUtils.ToSecond(7));
		Vector2 TowardMouse = Main.MouseWorld - player.Center;
		for (int i = 0; i < 3; i++) {
			Vector2 vel = Vector2.UnitX.RotatedBy(TowardMouse.ToRotation()) * 10;
			Vector2 position = player.Center + Vector2.One.Vector2DistributeEvenlyPlus(3, 60, i) * 50;
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), position, vel, ProjectileID.RubyBolt, player.GetWeaponDamage(item), player.GetWeaponKnockback(item), player.whoAmI);
			Main.projectile[proj].usesLocalNPCImmunity = true;
		}
		int manaheal = (int)(player.statManaMax2 * .5f);
		player.ManaEffect(manaheal);
		player.statMana += manaheal;
	}
}
public class DiamondStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.DiamondStaff;
	}
	public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		type = ProjectileID.DiamondBolt;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (!velocity.IsLimitReached(3)) {
			velocity = velocity.SafeNormalize(Vector2.Zero) * 4;
		}
		if (Main.rand.NextFloat() <= .17f) {
			Vector2 newPos = position + Main.rand.NextVector2Circular(40, 40);
			Projectile.NewProjectile(source, newPos, (Main.MouseWorld - newPos).SafeNormalize(Vector2.Zero) * velocity.Length(), ProjectileID.DiamondBolt, damage, knockback, player.whoAmI);
		}
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnMissingMana(int index, Player player, EnchantmentGlobalItem globalItem, Item item, int neededMana) {
		if (globalItem.Item_Counter1[index] > 0) {
			return;
		}
		globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, ModUtils.ToSecond(4));
		for (int i = 0; i < 5; i++) {
			Vector2 vel = Vector2.One.Vector2DistributeEvenly(5, 360, i) * 2;
			Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, vel, ProjectileID.DiamondBolt, player.GetWeaponDamage(item), player.GetWeaponKnockback(item), player.whoAmI);
		}
	}
}
public class AmberStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.AmberStaff;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		if (item.DamageType == DamageClass.Magic) {
			damage.Flat += 5;
			damage += .15f;
		}
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		reduce -= .15f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		if (item.shoot == ProjectileID.None) {
			item.shoot = ProjectileID.AmberBolt;
		}
	}
	public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		type = ProjectileID.AmberBolt;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (!velocity.IsLimitReached(3)) {
			velocity = velocity.SafeNormalize(Vector2.Zero) * 4;
		}
		if (Main.rand.NextFloat() <= .17f) {
			Vector2 newPos = position + Main.rand.NextVector2Circular(40, 40);
			Projectile.NewProjectile(source, newPos, (Main.MouseWorld - newPos).SafeNormalize(Vector2.Zero) * velocity.Length(), ProjectileID.AmberBolt, damage, knockback, player.whoAmI);
		}
	}
}
public class WandOfSparking : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.WandofSparking;
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		reduce -= .1f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] > 0) {
			return;
		}
		if (proj.DamageType == DamageClass.Magic) {
			target.AddBuff(BuffID.OnFire, ModUtils.ToSecond(3));
		}
		if (Main.rand.NextBool() && proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type) {
			Projectile.NewProjectile(proj.GetSource_OnHit(target), proj.Center, Main.rand.NextVector2CircularEdge(6, 6), ProjectileID.WandOfSparkingSpark, player.GetWeaponDamage(player.HeldItem), proj.knockBack, player.whoAmI);
			globalItem.Item_Counter1[index] = ModUtils.ToSecond(1);
		}
	}
}
public class WandOfFrosting : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.WandofFrosting;
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		reduce -= .1f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] > 0) {
			return;
		}
		if (proj.DamageType == DamageClass.Magic) {
			target.AddBuff(BuffID.Frostburn, ModUtils.ToSecond(3));
		}
		if (Main.rand.NextBool() && proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type) {
			Projectile.NewProjectile(proj.GetSource_OnHit(target), proj.Center, Main.rand.NextVector2CircularEdge(6, 6), ProjectileID.WandOfFrostingFrost, player.GetWeaponDamage(player.HeldItem), proj.knockBack, player.whoAmI);
			globalItem.Item_Counter1[index] = ModUtils.ToSecond(1);
		}
	}
}
public class WaterBolt : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.WaterBolt;
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		reduce -= .08f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (globalItem.Item_Counter1[index] <= 0) {
			if (!velocity.IsLimitReached(3)) {
				velocity = velocity.SafeNormalize(Vector2.Zero) * 3;
			}
			for (int i = 0; i < 3; i++) {
				int proj = Projectile.NewProjectile(source, position, velocity.Vector2DistributeEvenly(3, 12, i), ProjectileID.WaterBolt, damage, knockback, player.whoAmI);
				Main.projectile[proj].timeLeft = ModUtils.ToSecond(3);
			}
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, ModUtils.ToSecond(2));
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter2[index] > 0 && proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type) {
			return;
		}
		int projectile = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem),
			player.Center + Main.rand.NextVector2Circular(700, 700),
			Main.rand.NextVector2CircularEdge(4, 4), ProjectileID.WaterBolt, player.GetWeaponDamage(player.HeldItem), proj.knockBack, player.whoAmI);
		Main.projectile[projectile].timeLeft = ModUtils.ToSecond(5);
		globalItem.Item_Counter2[index] = PlayerStatsHandle.WE_CoolDown(player, ModUtils.ToSecond(5));
	}
}
public class DemonScythe : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.DemonScythe;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage *= 1.1f;
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		reduce += .1f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (globalItem.Item_Counter1[index] <= 0) {
			for (int i = 0; i < 3; i++) {
				Projectile.NewProjectile(source, position, velocity.Vector2DistributeEvenly(3, 12, i), ProjectileID.DemonScythe, damage, knockback, player.whoAmI);
			}
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, ModUtils.ToSecond(1));
		}
	}
}
public class BookOfSkulls : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.BookofSkulls;
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		multi += .12f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (Main.rand.NextBool(3)) {
			if (!velocity.IsLimitReached(3)) {
				velocity = (Main.MouseWorld - position).SafeNormalize(Vector2.Zero) * 3;
			}
			Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(10), ProjectileID.BookOfSkullsSkull, damage, knockback, player.whoAmI);
		}
	}
	public override void OnMissingMana(int index, Player player, EnchantmentGlobalItem globalItem, Item item, int neededMana) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, ModUtils.ToSecond(5));
			int damage = (int)player.GetDamage(DamageClass.Magic).ApplyTo(item.damage);
			for (int i = 0; i < 5; i++) {
				Vector2 vel = Vector2.One.Vector2DistributeEvenly(5, 360, i) * 5;
				Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, vel, ProjectileID.BookOfSkullsSkull, damage, item.knockBack, player.whoAmI);
			}
		}
	}
}
public class ThunderStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.ThunderStaff;
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		reduce -= .12f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			Vector2 pos = target.Center + Main.rand.NextVector2CircularEdge(150, 150);
			Projectile.NewProjectile(player.GetSource_ItemUse(item), pos, (target.Center - pos).SafeNormalize(Vector2.Zero) * 10, ProjectileID.ThunderStaffShot, player.GetWeaponDamage(player.HeldItem), 4, player.whoAmI);
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, ModUtils.ToSecond(0.2f));
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.type == ProjectileID.ThunderStaffShot && Main.rand.NextBool(10) || globalItem.Item_Counter1[index] <= 0 && proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type && !proj.minion) {
			Vector2 pos = target.Center + Main.rand.NextVector2CircularEdge(150, 150);
			Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, (target.Center - pos).SafeNormalize(Vector2.Zero) * 10, ProjectileID.ThunderStaffShot, player.GetWeaponDamage(player.HeldItem), 4, player.whoAmI);
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, ModUtils.ToSecond(0.2f));
		}
	}
}
public class ZapinatorGray : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.ZapinatorGray;
	}
	public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		int chance = 20;
		if (item.type == ItemID.LaserRifle || item.type == ItemID.SpaceGun
			|| item.type == ItemID.ZapinatorGray || item.type == ItemID.ZapinatorOrange) {
			chance -= 10;
		}
		if (Main.rand.NextBool(chance)) {
			damage += 100;
		}
		if (Main.rand.NextBool(chance)) {
			damage *= 10;
		}
		if (Main.rand.NextBool(chance)) {
			position = position.PositionOFFSET(velocity.Vector2RotateByRandom(360), Main.rand.Next(100, 150));
		}
		if (Main.rand.NextBool(chance)) {
			velocity *= .1f;
		}
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		int chance = 20;
		if (item.type == ItemID.LaserRifle || item.type == ItemID.SpaceGun
			|| item.type == ItemID.ZapinatorGray || item.type == ItemID.ZapinatorOrange) {
			chance -= 10;
		}
		if (Main.rand.NextBool(chance)) {
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
		}
	}
}
public class ZapinatorOrange : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.ZapinatorOrange;
	}
	public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		int chance = 200;
		if (item.type == ItemID.LaserRifle || item.type == ItemID.SpaceGun
			|| item.type == ItemID.ZapinatorGray || item.type == ItemID.ZapinatorOrange) {
			chance -= 100;
		}
		if (Main.rand.NextBool(chance)) {
			damage += 1000;
		}
		if (Main.rand.NextBool(chance)) {
			damage *= 100;
		}
		if (Main.rand.NextBool(chance)) {
			position = position.PositionOFFSET(velocity.Vector2RotateByRandom(360), Main.rand.Next(100, 150));
		}
		if (Main.rand.NextBool(chance)) {
			velocity *= .01f;
		}
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		int chance = 200;
		if (item.type == ItemID.LaserRifle || item.type == ItemID.SpaceGun
			|| item.type == ItemID.ZapinatorGray || item.type == ItemID.ZapinatorOrange) {
			chance -= 100;
		}
		if (Main.rand.NextBool(chance)) {
			int initalDamage = damage;
			Vector2 vel = velocity;
			Vector2 pos = position;
			for (int i = 0; i < 10; i++) {
				velocity = velocity.Vector2RotateByRandom(40);
				ModifyShootStat(index, player, globalItem, item, ref pos, ref vel, ref type, ref initalDamage, ref knockback);
				Projectile.NewProjectile(source, position, velocity, type, initalDamage, knockback, player.whoAmI);
				initalDamage = damage;
				vel = velocity;
				pos = position;
			}
		}
	}
}
public class SpaceGun : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.SpaceGun;
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		if (player.spaceGun) {
			multi *= 0;
		}
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (++globalItem.Item_Counter1[index] >= 3) {
			if (!velocity.IsLimitReached(4)) {
				velocity = velocity.SafeNormalize(Vector2.Zero) * 4;
			}
			Projectile.NewProjectile(source, position, velocity, ProjectileID.GreenLaser, damage, knockback, player.whoAmI);
			if (globalItem.Item_Counter1[index] >= 6) {
				globalItem.Item_Counter1[index] = 0;
			}
		}
	}
}
public class BeeGun : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.BeeGun;
	}
	public override void OnHitByAnything(Player player) {
		player.AddBuff(BuffID.Honey, 360);
		int damage = (int)(30 + player.GetWeaponDamage(player.HeldItem) * .1f);
		for (int i = 0; i < 5; i++) {
			Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Main.rand.NextVector2CircularEdge(5, 5), ProjectileID.Bee, damage, 4f, player.whoAmI);
		}
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		int damage2 = (int)(20 + damage * .1f);
		if (Main.rand.NextBool(5) || player.strongBees && Main.rand.NextBool(2)) {
			if (!velocity.IsLimitReached(3)) {
				velocity = velocity.SafeNormalize(Vector2.Zero) * 3;
			}
			Projectile.NewProjectile(source, position, velocity, ProjectileID.Bee, damage2, knockback, player.whoAmI);
		}
	}
}
public class Vilethorn : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.Vilethorn;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
		if (player.ItemAnimationActive && player.ItemAnimationJustStarted) {
			if (++globalItem.Item_Counter2[index] >= 5) {
				for (int i = 0; i < 12; i++) {
					Vector2 vel = Vector2.One.Vector2DistributeEvenlyPlus(12, 360, i) * 20;
					Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, vel, ProjectileID.VilethornBase, player.GetWeaponDamage(item), 1f, player.whoAmI);
					Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, vel, ProjectileID.VilethornTip, player.GetWeaponDamage(item), 1f, player.whoAmI);
				}
				globalItem.Item_Counter2[index] = 0;
			}
		}
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			Vector2 velocity = Main.rand.NextVector2CircularEdge(17, 17);
			Projectile.NewProjectile(player.GetSource_FromThis(), target.Center, velocity, ProjectileID.VilethornBase, player.GetWeaponDamage(item), 1f, player.whoAmI);
			Projectile.NewProjectile(player.GetSource_FromThis(), target.Center, velocity, ProjectileID.VilethornTip, player.GetWeaponDamage(item), 1f, player.whoAmI);
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 60);
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.type != ProjectileID.VilethornBase && proj.type != ProjectileID.VilethornTip && globalItem.Item_Counter1[index] <= 0 && proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type) {
			Vector2 velocity = Main.rand.NextVector2CircularEdge(17, 17);
			Projectile.NewProjectile(player.GetSource_FromThis(), target.Center, velocity, ProjectileID.VilethornBase, player.GetWeaponDamage(player.HeldItem), 1f, player.whoAmI);
			Projectile.NewProjectile(player.GetSource_FromThis(), target.Center, velocity, ProjectileID.VilethornTip, player.GetWeaponDamage(player.HeldItem), 1f, player.whoAmI);
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 60);
		}
	}
}
public class CrimsonRod : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.CrimsonRod;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		if (globalItem.Item_Counter2[index] > 0) {
			for (int i = 0; i < 5; i++) {
				Projectile.NewProjectile(player.GetSource_FromThis(), player.Center.Add(Main.rand.NextFloat(-500, 500), 1000), Vector2.UnitY * Main.rand.NextFloat(4, 12), ProjectileID.BloodRain, player.GetWeaponDamage(item), .2f, player.whoAmI);
			}
		}
		globalItem.Item_Counter2[index] = ModUtils.CountDown(globalItem.Item_Counter2[index]);
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
		if (player.ItemAnimationActive && globalItem.Item_Counter1[index] <= 0) {
			Projectile.NewProjectile(player.GetSource_FromThis(), Main.MouseWorld.Add(Main.rand.NextFloat(-30, 30), 500), Vector2.UnitY * Main.rand.NextFloat(4, 12), ProjectileID.BloodRain, player.GetWeaponDamage(item), .2f, player.whoAmI);
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 9);
		}
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		multi -= .08f;
	}
	public override void OnMissingMana(int index, Player player, EnchantmentGlobalItem globalItem, Item item, int neededMana) {
		if (globalItem.Item_Counter2[index] <= 0) {
			globalItem.Item_Counter2[index] = ModUtils.ToSecond(3);
		}
	}
}
public class MagicMissile : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.MagicMissile;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		if (item.shoot != ProjectileID.None) {
			return;
		}
		globalItem.Item_Counter2[index] = ModUtils.CountDown(globalItem.Item_Counter2[index]);
		if (!player.CheckMana(14, true)) {
			return;
		}
		if (player.ItemAnimationActive && globalItem.Item_Counter2[index] <= 0) {
			Vector2 velToMouse = (player.Center - Main.MouseWorld).SafeNormalize(Vector2.Zero);
			Vector2 positionBehindPlayer = player.Center.PositionOFFSET(velToMouse.Vector2RotateByRandom(25), Main.rand.Next(80, 110));
			Vector2 vel = (Main.MouseWorld - positionBehindPlayer).SafeNormalize(Vector2.Zero) * 12;
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), positionBehindPlayer, vel, ProjectileID.MagicMissile, item.damage, item.knockBack, player.whoAmI);
			Main.projectile[proj].tileCollide = false;
			globalItem.Item_Counter2[index] = PlayerStatsHandle.WE_CoolDown(player, 90);
		}
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (globalItem.Item_Counter1[index] >= 3) {
			if (!player.CheckMana(14, true)) {
				return;
			}
			Vector2 velocityNew = velocity.SafeNormalize(Vector2.Zero) * 14;
			Projectile.NewProjectile(source, position, velocityNew.Vector2RotateByRandom(30), ProjectileID.MagicMissile, damage, knockback, player.whoAmI);
			globalItem.Item_Counter1[index] = 0;
		}
		else {
			globalItem.Item_Counter1[index]++;
		}
	}
}
class Flamelash : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.Flamelash;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (Main.rand.NextBool(5)) {
			Projectile.NewProjectile(source, position, velocity, ProjectileID.Flamelash, damage, knockback, player.whoAmI);
		}
	}
	public override void ModifyHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.OnFire3) || target.HasBuff(BuffID.OnFire)) {
			modifiers.SourceDamage += .33f;
		}
	}
	public override void ModifyHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.OnFire3) || target.HasBuff(BuffID.OnFire)) {
			modifiers.SourceDamage += .33f;
		}
	}
}
class AquaScepter : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.AquaScepter;
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		multi -= .09f;
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		HitEnemy(player, target, (int)player.GetDamage(DamageClass.Magic).ApplyTo(item.damage * .5f), 3);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.type != ProjectileID.WaterStream) {
			if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type) {
				HitEnemy(player, target, (int)player.GetDamage(DamageClass.Magic).ApplyTo(proj.damage * .5f), 3);
			}
		}
		else {
			player.statMana += 10;
		}
	}
	public void HitEnemy(Player player, NPC target, int damage, float knockback) {
		if (!Main.rand.NextBool(3)) {
			return;
		}
		Vector2 pos = target.Center + Main.rand.NextVector2CircularEdge(10, 10) * 30;
		Vector2 vel = (target.Center - pos).SafeNormalize(Vector2.Zero) * 15;
		Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, vel, ProjectileID.WaterStream, damage, knockback, player.whoAmI);
	}
}
class WeatherPain : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.WeatherPain;
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		multi += .11f;
	}
	public override void ModifyHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.DamageType == DamageClass.Magic) {
			modifiers.SourceDamage += .12f;
			modifiers.ArmorPenetration += 10;
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(20)) {
			Item item = player.HeldItem;
			int projectile = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Main.rand.NextVector2CircularEdge(5, 5), ProjectileID.WeatherPainShot, item.damage, item.knockBack, player.whoAmI);
			Main.projectile[projectile].timeLeft = 120;
		}
	}
}
class FlowerOfFire : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.FlowerofFire;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (++globalItem.Item_Counter1[index] >= 5) {
			if (player.CheckMana(5, true, true)) {
				damage += 20;
			}
			Projectile.NewProjectile(source, position, velocity, ProjectileID.BallofFire, damage, knockback, player.whoAmI);
			globalItem.Item_Counter1[index] = 0;
		}
	}
	public override void ModifyHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.OnFire3) || target.HasBuff(BuffID.OnFire)) {
			modifiers.SourceDamage += .27f;
		}
	}
	public override void ModifyHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.OnFire3) || target.HasBuff(BuffID.OnFire)) {
			modifiers.SourceDamage += .27f;
		}
	}
}
public class LaserRifle : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.LaserRifle;
	}
	public override void ModifyCriticalStrikeChance(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float crit) {
		if (item.type == ItemID.LaserRifle || item.type == ItemID.SpaceGun) {
			crit += 5f;
		}
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		if (item.type == ItemID.LaserRifle || item.type == ItemID.SpaceGun) {
			damage += .5f;
		}
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		globalItem.Item_Counter1[index]++;
		if (globalItem.Item_Counter1[index] >= 5) {
			if (Main.rand.NextFloat() <= .2f) {
				int amount = Main.rand.Next(5, 7);
				for (int i = 0; i < amount; i++) {
					Vector2 pos = position + Main.rand.NextVector2CircularEdge(50, 50) * Main.rand.NextFloat(1, 2);
					Vector2 vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 15;
					Projectile proj = Projectile.NewProjectileDirect(source, pos, vel, ProjectileID.PurpleLaser, damage / 3, knockback, player.whoAmI);
					proj.penetrate = 1;
					proj.usesLocalNPCImmunity = true;
					proj.localNPCHitCooldown = 1;
				}
			}
			Projectile projectile = Projectile.NewProjectileDirect(source, position, velocity.SafeNormalize(Vector2.Zero) * 15, ProjectileID.PurpleLaser, damage, knockback, player.whoAmI);
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 10;
			if (globalItem.Item_Counter1[index] >= 8) {
				globalItem.Item_Counter1[index] = -1;
			}
		}
	}
}
public class SkyFacture : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.SkyFracture;
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		multi += .12f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.MagicDMG, 1.22f);
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = 12;
			Vector2 pos = player.Center + Main.rand.NextVector2CircularEdge(Main.rand.NextFloat(40, 50), Main.rand.NextFloat(40, 50));
			Vector2 vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(10, 14);
			Projectile.NewProjectile(player.GetSource_ItemUse(item), pos, vel, ProjectileID.SkyFracture, 22 + item.damage, item.knockBack, player.whoAmI);
		}
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 12);
			Vector2 pos = target.Center + Main.rand.NextVector2CircularEdge(target.width + Main.rand.NextFloat(100, 150), target.height + Main.rand.NextFloat(100, 150));
			Vector2 vel = (target.Center - pos).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(10, 14);
			Projectile.NewProjectile(player.GetSource_ItemUse(item), pos, vel, ProjectileID.SkyFracture, 22 + item.damage, item.knockBack, player.whoAmI);
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0 && proj.type != ProjectileID.SkyFracture) {
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 12);
			Vector2 pos = target.Center + Main.rand.NextVector2CircularEdge(target.width + Main.rand.NextFloat(100, 150), target.height + Main.rand.NextFloat(100, 150));
			Vector2 vel = (target.Center - pos).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(10, 14);
			Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, vel, ProjectileID.SkyFracture, 22 + player.HeldItem.damage, player.HeldItem.knockBack, player.whoAmI);
		}
	}
}
public class CrystalSerpent : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.CrystalSerpent;
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		multi += .15f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.MagicDMG, 1.19f);
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
		globalItem.Item_Counter2[index] = ModUtils.CountDown(globalItem.Item_Counter2[index]);
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 42);
			Vector2 pos = player.Center + Main.rand.NextVector2CircularEdge(Main.rand.NextFloat(40, 50), Main.rand.NextFloat(40, 50));
			Vector2 vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(7, 9);
			Projectile.NewProjectile(player.GetSource_ItemUse(item), pos, vel, ProjectileID.CrystalPulse, 30 + item.damage, item.knockBack, player.whoAmI);
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (!proj.Check_ItemTypeSource(player.HeldItem.type) && globalItem.Item_Counter2[index] <= 0) {
			return;
		}
		if (Main.rand.NextBool(10) && player.ownedProjectileCounts[ModContent.ProjectileType<CrystalSerpentProjectile>()] < 5) {
			globalItem.Item_Counter2[index] = PlayerStatsHandle.WE_CoolDown(player, ModUtils.ToSecond(5));
			Item item = player.HeldItem;
			Projectile.NewProjectile(player.GetSource_ItemUse(item), target.Center + Main.rand.NextVector2CircularEdge(Main.rand.NextFloat(300, 500), Main.rand.NextFloat(300, 500)), Vector2.Zero, ModContent.ProjectileType<CrystalSerpentProjectile>(), 30 + item.damage, item.knockBack, player.whoAmI);
		}
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(10) && globalItem.Item_Counter2[index] <= 0 && player.ownedProjectileCounts[ModContent.ProjectileType<CrystalSerpentProjectile>()] < 5) {
			globalItem.Item_Counter2[index] = PlayerStatsHandle.WE_CoolDown(player, ModUtils.ToSecond(5));
			Projectile.NewProjectile(player.GetSource_ItemUse(item), target.Center + Main.rand.NextVector2CircularEdge(Main.rand.NextFloat(300, 500), Main.rand.NextFloat(300, 500)), Vector2.Zero, ModContent.ProjectileType<CrystalSerpentProjectile>(), 30 + item.damage, item.knockBack, player.whoAmI);
		}
	}
	public class CrystalSerpentProjectile : ModProjectile {
		public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.CrystalSerpent);
		public override void SetDefaults() {
			Projectile.width = 40;
			Projectile.height = 48;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 300;
			Projectile.penetrate = 1;
		}
		public override bool? CanDamage() {
			return false;
		}
		int TimeLeft = 300;
		NPC npc = null;
		bool OneTimeTimeLeftReset = true;
		public override void AI() {
			if (++Projectile.ai[1] <= 1) {
				for (int i = 0; i < 50; i++) {
					Dust dust1 = Dust.NewDustDirect(Projectile.Center, 0, 0, Main.rand.NextBool() ? DustID.CrystalPulse : DustID.CrystalPulse2);
					dust1.velocity = Main.rand.NextVector2CircularEdge(3, 5) * Main.rand.NextFloat(1, 3);
					dust1.position += Main.rand.NextVector2Circular(5, 5);
					dust1.noGravity = true;
				}
			}
			if (npc == null) {
				if (Projectile.Center.LookForHostileNPC(out NPC npc, 1000)) {
					this.npc = npc;
				}
				return;
			}
			else {
				if (!npc.active || npc.life <= 0) {
					npc = null;
					return;
				}
			}
			Projectile.spriteDirection = ModUtils.DirectionFromPlayerToNPC(Projectile.Center.X, npc.Center.X);
			Projectile.rotation = (npc.Center - Projectile.Center).ToRotation();
			Projectile.rotation += Projectile.spriteDirection == -1 ? MathHelper.PiOver4 + MathHelper.PiOver2 : MathHelper.PiOver4;
			Point point = Projectile.position.ToTileCoordinates();
			if (!WorldGen.TileEmpty(point.X, point.Y) || !Projectile.Center.IsCloseToPosition(npc.Center, 150 + npc.Size.Length())) {
				Projectile.velocity = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * (npc.Center - Projectile.Center).Length() / 64f;
				if (OneTimeTimeLeftReset) {
					Projectile.timeLeft = TimeLeft;
				}
			}
			else {
				OneTimeTimeLeftReset = false;
				if (++Projectile.ai[0] >= 45) {
					Vector2 vel = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.PositionOFFSET(vel, 30), vel * 15, ProjectileID.CrystalPulse, Projectile.damage, Projectile.knockBack, Projectile.owner);
					Projectile.ai[0] = 0;
				}
				Projectile.velocity *= .9f;
			}
			Dust dust = Dust.NewDustDirect(Projectile.Center.IgnoreTilePositionOFFSET(
				(Projectile.rotation + (Projectile.spriteDirection == 1 ? -MathHelper.PiOver4 : -MathHelper.PiOver2 - MathHelper.PiOver4)).ToRotationVector2(), 24), 0, 0, Main.rand.NextBool() ? DustID.CrystalPulse : DustID.CrystalPulse2);
			dust.velocity = -Vector2.UnitY.Vector2RotateByRandom(3) * Main.rand.NextFloat(1, 3);
			dust.position += Main.rand.NextVector2Circular(5, 5);
			dust.noGravity = true;
		}
	}
}
public class FlowerOfFrost : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.FlowerofFrost;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (++globalItem.Item_Counter1[index] >= 5) {
			if (player.CheckMana(5, true, true)) {
				damage += 20;
			}
			Projectile.NewProjectile(source, position, velocity, ProjectileID.BallofFrost, damage, knockback, player.whoAmI);
			globalItem.Item_Counter1[index] = 0;
		}
	}
	public override void ModifyHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.Frostburn) || target.HasBuff(BuffID.Frostburn2)) {
			modifiers.SourceDamage += .27f;
		}
	}
	public override void ModifyHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.Frostburn) || target.HasBuff(BuffID.Frostburn2)) {
			modifiers.SourceDamage += .27f;
		}
	}
}
public class FrostStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.FrostStaff;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (Main.rand.NextBool(10)) {
			Projectile.NewProjectile(source, position, velocity, ProjectileID.FrostBoltStaff, damage, knockback, player.whoAmI);
		}
		else if (item.mana > 0 && item.DamageType == DamageClass.Magic && Main.rand.NextBool(3)) {
			Projectile.NewProjectile(source, position, velocity, ProjectileID.FrostBoltStaff, damage, knockback, player.whoAmI);
		}
	}
}
public class RainbowRod : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.RainbowRod;
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		reduce += .5f;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (globalItem.Item_Counter1[index] >= 3) {
			globalItem.Item_Counter1[index] = 0;
			Projectile.NewProjectile(source, position, velocity.SafeNormalize(Vector2.Zero) * 10, ProjectileID.RainbowRodBullet, damage, knockback, player.whoAmI);
		}
		if (player.CheckMana(20, true)) {
			Projectile.NewProjectile(source, position, velocity.SafeNormalize(Vector2.Zero) * 10, ProjectileID.RainbowRodBullet, damage, knockback, player.whoAmI);
		}
	}
}
public class CrystalVileShard : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.CrystalVileShard;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] > 0) {
			return;
		}
		globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 120);
		int damage = player.GetWeaponDamage(item);
		for (int i = 0; i < 16; i++) {
			Projectile.NewProjectileDirect(player.GetSource_ItemUse(item), player.Center, Vector2.One.Vector2DistributeEvenly(16, 360, i) * 20, ProjectileID.CrystalVileShardShaft, damage, 1f, player.whoAmI);
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(5) && (proj.type != ProjectileID.CrystalVileShardShaft && proj.type != ProjectileID.CrystalVileShardHead || proj.Check_ItemTypeSource(player.HeldItem.type))) {
			int damage = player.GetWeaponDamage(player.HeldItem);
			Projectile.NewProjectileDirect(player.GetSource_OnHit(target), target.Center, Vector2.One.Vector2RotateByRandom(360) * 10, ProjectileID.CrystalVileShardShaft, damage, 1f, player.whoAmI);
		}
	}
}
public class LifeDrain : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = 3006;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		int[] slots = globalItem.EnchantmenStlot;
		int count = 0;
		for (int i = 0; i < slots.Length; i++) {
			if (slots[i] == ItemIDType) {
				count++;
			}
			if (index > i) {
				return;
			}
		}
		for (int i = 0; i < 25; i++) {
			Dust dust = Dust.NewDustDirect(player.Center + Main.rand.NextVector2CircularEdge(250, 250), 0, 0, DustID.LifeDrain);
			dust.velocity = Vector2.Zero;
			dust.noGravity = true;
			dust.scale += .25f;
			dust.Dust_GetDust().FollowEntity = true;
			dust.Dust_BelongTo(player);
		}
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
		player.Center.LookForHostileNPC(out List<NPC> npclist, 250);
		if (npclist.Count < 1) {
			return;
		}
		foreach (var npc in npclist) {
			if (globalItem.Item_Counter1[index] < 0) {
				player.StrikeNPCDirect(npc, npc.CalculateHitInfo(1 * count, 1));
				globalItem.Item_Counter1[index] = 5;
				for (int i = 0; i < 15; i++) {
					Dust dust = Dust.NewDustDirect(npc.Center, 0, 0, DustID.LifeDrain);
					dust.velocity = Main.rand.NextVector2Circular(5, 5);
					dust.noGravity = true;
					dust.scale += .25f;
				}
			}
			player.ModPlayerStats().UpdateHPRegen.Base += 1 * count;
		}
	}
}
public class MeteorStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.MeteorStaff;
	}
	public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (Main.rand.NextBool(20) || item.DamageType == DamageClass.Magic && item.mana > 0) {
			if (item.type != ItemID.MeteorStaff) {
				int[] met = new int[] { ProjectileID.Meteor1, ProjectileID.Meteor2, ProjectileID.Meteor3 };
				type = Main.rand.Next(met);
				velocity = velocity.SafeNormalize(Vector2.Zero) * 19;
			}
			if (item.DamageType == DamageClass.Magic && item.mana > 0) {
				damage = (int)(damage * 1.25f);
			}
			else {
				damage *= 3;
			}
		}
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (item.type == ItemID.MeteorStaff) {
			int[] met = new int[] { ProjectileID.Meteor1, ProjectileID.Meteor2, ProjectileID.Meteor3 };
			type = Main.rand.Next(met);
			position = player.Center;
			Vector2 vel = (Main.MouseWorld - position).SafeNormalize(Vector2.Zero) * item.shootSpeed;
			position = position.PositionOFFSET(vel, item.Size.Length());
			int proj = Projectile.NewProjectile(source, position, vel.Vector2RotateByRandom(5), type, damage, knockback, player.whoAmI, ai1: Main.rand.NextFloat(.5f, 1f));
			Main.projectile[proj].tileCollide = false;
			Main.projectile[proj].extraUpdates += 1;
		}
	}
}
public class PoisonStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.PoisonStaff;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (Main.rand.NextBool(10) || item.DamageType == DamageClass.Magic && item.mana > 0) {
			int amount = Main.rand.Next(4, 7);
			if (item.type == ItemID.VenomStaff || item.type == ItemID.PoisonStaff) {
				amount += Main.rand.Next(4, 7);
			}
			for (int i = 0; i < amount; i++) {
				Projectile proj = Projectile.NewProjectileDirect(source, position, velocity.SafeNormalize(Vector2.Zero).Vector2RotateByRandom(60) * Main.rand.NextFloat(3.9f, 4.5f), ProjectileID.PoisonFang, (int)(damage * .55f), knockback, player.whoAmI);
				proj.penetrate = 1;
			}
		}
	}
}
public class VenomStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.VenomStaff;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (Main.rand.NextBool()) {
			if (item.DamageType != DamageClass.Magic || item.mana <= 0) {
				damage = (int)(damage * .8f);
			}
			Projectile.NewProjectile(source, position, velocity, ProjectileID.VenomFang, damage, knockback, player.whoAmI);
		}
		if (item.type == ItemID.VenomStaff || item.type == ItemID.PoisonStaff) {
			if (++globalItem.Item_Counter1[index] >= 12) {
				globalItem.Item_Counter1[index] = 0;
				for (int i = 0; i < 12; i++) {
					Projectile.NewProjectile(source, position, velocity.Vector2DistributeEvenlyPlus(12, 360, i), ProjectileID.VenomFang, damage, knockback, player.whoAmI);
				}
			}
		}
	}
}
public class UnholyTrident : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.UnholyTrident;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		if (item.DamageType == DamageClass.Magic) {
			damage += .35f;
		}
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		if (item.DamageType == DamageClass.Magic) {
			multi += .2f;
		}
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		Projectile projectile = Projectile.NewProjectileDirect(source, position + Main.rand.NextVector2Circular(30, 30), velocity, ProjectileID.UnholyTridentFriendly, (int)(damage * .35f), knockback, player.whoAmI);
		projectile.scale -= .5f;
		projectile.width = projectile.width / 2;
		projectile.height = projectile.height / 2;
		projectile.penetrate = 1;
		if (item.DamageType == DamageClass.Magic && globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 60);
			Projectile.NewProjectileDirect(source, position + Main.rand.NextVector2Circular(30, 30), velocity, ProjectileID.UnholyTridentFriendly, damage, knockback, player.whoAmI);
		}
	}
	public override void ModifyHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.type == ProjectileID.UnholyTridentFriendly) {
			if (proj.Check_ItemTypeSource(ItemID.UnholyTrident)) {
				if (target.GetGlobalNPC<RoguelikeGlobalNPC>().Amount_CurrentDebuffInflicted > 0) {
					modifiers.SourceDamage *= 1.2f;
				}
				target.AddBuff(BuffID.OnFire3, Main.rand.Next(60, 90));
				target.AddBuff(BuffID.Frostburn2, Main.rand.Next(60, 90));
				target.AddBuff(BuffID.CursedInferno, Main.rand.Next(60, 90));
				target.AddBuff(BuffID.ShadowFlame, Main.rand.Next(60, 90));
				target.AddBuff<HolyFlame>(Main.rand.Next(60, 90));
			}
		}
	}
}
public class NettleBurst : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.NettleBurst;
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(10)) {
			Projectile.NewProjectile(player.GetSource_ItemUse(item), target.Center, Main.rand.NextVector2CircularEdge(20, 20), ProjectileID.NettleBurstEnd, player.GetWeaponDamage(item), 1, player.whoAmI);
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(20)) {
			Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), target.Center, Main.rand.NextVector2CircularEdge(20, 20), ProjectileID.NettleBurstEnd, player.GetWeaponDamage(player.HeldItem), 1, player.whoAmI);
		}
	}
}
public class BatScepter : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.BatScepter;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage -= .35f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.ModPlayerStats().LifeSteal.Base += 1;
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (item.DamageType == DamageClass.Magic) {
			Projectile.NewProjectile(source, position, velocity, ProjectileID.Bat, damage, knockback, player.whoAmI);
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.type == ProjectileID.Bat && proj.Check_ItemTypeSource(player.HeldItem.type) && globalItem.Item_Counter1[index] <= 0) {
			player.Heal(1);
			globalItem.Item_Counter1[index] = 60;
		}
	}
}
public class BlizzardStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.BlizzardStaff;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		if (globalItem.Item_Counter1[index] >= 100) {
			globalItem.Item_Counter2[index] = 60;
			globalItem.Item_Counter1[index] = 0;
		}
		if (globalItem.Item_Counter2[index] > 0) {
			globalItem.Item_Counter2[index] = ModUtils.CountDown(globalItem.Item_Counter2[index]);
			Vector2 pos = player.Center;
			Vector2 vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 15;
			int damage = player.GetWeaponDamage(item);
			Projectile.NewProjectile(player.GetSource_ItemUse(item), pos, vel.Vector2RotateByRandom(10), ProjectileID.Blizzard, damage, 1f, player.whoAmI);
		}
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		Vector2 pos = player.Center;
		Vector2 vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * Math.Max(velocity.Length(), 10);
		Projectile.NewProjectile(source, pos, vel.Vector2RotateByRandom(10), ProjectileID.Blizzard, damage, knockback, player.whoAmI);
		globalItem.Item_Counter1[index]++;
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.type == ProjectileID.Blizzard && proj.Check_ItemTypeSource(player.HeldItem.type)) {
			target.AddBuff(BuffID.Frostburn2, 30);
		}
	}
}
public class InfernoFork : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.InfernoFork;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage += 1;
	}
	public override void ModifyUseSpeed(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float useSpeed) {
		useSpeed -= .5f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (globalItem.Item_Counter1[index] > 0) {
			return;
		}
		globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, ModUtils.ToSecond(5));
		Projectile.NewProjectile(source, position, velocity, ProjectileID.InfernoFriendlyBlast, damage * 5, knockback, player.whoAmI);
	}
	public override void ModifyHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.OnFire) || target.HasBuff(BuffID.OnFire3)) {
			modifiers.SourceDamage *= 2;
		}
	}
	public override void ModifyHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.OnFire) || target.HasBuff(BuffID.OnFire3)) {
			modifiers.SourceDamage *= 2;
		}
	}
}
public class ShadowbeamStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.ShadowbeamStaff;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (item.type == ItemID.ShadowbeamStaff) {
			for (int i = 0; i < 2; i++) {
				Projectile.NewProjectile(source, position, velocity.Vector2DistributeEvenlyPlus(2, globalItem.Item_Counter1[index], i),
					type, damage, knockback, player.whoAmI);
			}
			globalItem.Item_Counter1[index] = ModUtils.Safe_SwitchValue(globalItem.Item_Counter1[index], 30, 10, 2);
		}
		else {
			int cooldown = ModUtils.ToSecond(1.5f);
			if (item.DamageType == DamageClass.Magic) {
				cooldown = 60;
			}
			if (globalItem.Item_Counter1[index] > 0) {
				return;
			}
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, cooldown);
			Projectile.NewProjectile(source, position, velocity, type, damage * 2, knockback, player.whoAmI);
		}
	}
}
public class SpectreStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.SpectreStaff;
	}
	public override void ModifyShootStat(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (item.DamageType != DamageClass.Magic) {
			type = ProjectileID.LostSoulFriendly;
		}
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (item.DamageType == DamageClass.Magic) {
			Projectile.NewProjectile(source, position, velocity, ProjectileID.LostSoulFriendly, damage, knockback, player.whoAmI);
		}
	}
}
public class Razorpine : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.Razorpine;
	}
	public override void ModifyUseSpeed(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float useSpeed) {
		useSpeed += .25f;
		if (item.DamageType == DamageClass.Magic) {
			useSpeed += .25f;
		}
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (item.DamageType == DamageClass.Magic) {
			if (Main.rand.NextBool(5)) {
				Projectile.NewProjectile(source, position, velocity.SafeNormalize(Vector2.Zero).Vector2RotateByRandom(10) * 20, ProjectileID.PineNeedleFriendly, damage, knockback, player.whoAmI);
			}
		}
	}
}
/*
StaffofEarth.Description:
	'''
	'''
 */
/// <summary>
/// This is a example for mod enchantment and how to utilize most of the stuff
/// </summary>
public class DirtBlock : ModEnchantment {
	public override void SetDefaults() {
		//This is important as it is required for the enchantment to be recognized and work
		ItemIDType = ItemID.DirtBlock;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		//We can add very basic stats increases, here we gonna increases player's defense by 10 when player held this item
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.Defense, Base: 10);
		//This will teach you the what the index and the globalItem do and why it exist here ( tho mainly for ease of uses )
		//The following effect will spawn a ring of 12 dirt projectiles for every 5s
		if (++globalItem.Item_Counter1[index] >= ModUtils.ToSecond(5)) {
			//the globalItem.ItemCounter1 is a array of counter that applied to specific enchantment slot, the index is the index of this enchantment
			//Now we spawn a ring of dirt
			for (int i = 0; i < 12; i++) {
				Vector2 rotate = Vector2.UnitX.Vector2DistributeEvenlyPlus(12, 360, i) * 6f;
				Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.Center, rotate, ModContent.ProjectileType<DirtProjectile>(), 20, 3f, player.whoAmI);
			}
			//Now we reset the counter of this enchantment
			globalItem.Item_Counter1[index] = 0;
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		//This is a enchantment hack with enchantment slot ID, do this only if you know the in and out of the system
		//The following code attempt to activate enchantment that have OnHit effect with NPC but only if the projectile type is this mod Dirt Projectile
		if (proj.type != ModContent.ProjectileType<DirtProjectile>()) {
			return;
		}
		//Iterating through enchantment array of the item
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			//we skip this index cause we don't want to cause infinite loop
			if (i == index) {
				continue;
			}
			//The Enchantment array of this item consist of ItemID, so it make sense to use EnchantmentLoader.GetEnchantmentItemID(int)
			ModEnchantment enchantment = EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]);
			//We are checking if the enchantment exist or if the enchantment is the same as this enchantment, preventing null exception and infinite loop
			if (enchantment == null || enchantment.ItemIDType == ItemIDType) {
				continue;
			}
			//Activate the enchantment effect
			enchantment.OnHitNPCWithProj(i, player, globalItem, proj, target, hit, damageDone);
		}
	}
}
public class CrystalStorm : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.CrystalStorm;
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		multi -= .15f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		if (globalItem.Item_Counter1[index] > 0) {
			int damage = player.GetWeaponDamage(item);
			for (int i = 0; i < 3; i++) {
				Vector2 pos = player.Center;
				Vector2 vel = Main.rand.NextVector2Circular(2, 2) * 4;
				Projectile.NewProjectile(player.GetSource_FromAI(), pos, vel, ProjectileID.CrystalStorm, damage, 1, player.whoAmI);
			}
		}
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
		globalItem.Item_Counter2[index] = ModUtils.CountDown(globalItem.Item_Counter2[index]);
	}
	public override void OnMissingMana(int index, Player player, EnchantmentGlobalItem globalItem, Item item, int neededMana) {
		if (globalItem.Item_Counter2[index] > 0) {
			return;
		}
		globalItem.Item_Counter1[index] = ModUtils.ToSecond(3);
		globalItem.Item_Counter2[index] = PlayerStatsHandle.WE_CoolDown(player, ModUtils.ToSecond(5));
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		int damage = player.GetWeaponDamage(player.HeldItem);
		for (int i = 0; i < 3; i++) {
			Vector2 pos = target.Center + Main.rand.NextVector2CircularEdge(target.width, target.height);
			Vector2 vel = (target.Center - pos).SafeNormalize(Vector2.Zero) * 5;
			Projectile.NewProjectile(player.GetSource_FromAI(), pos, vel, ProjectileID.CrystalStorm, damage, 1, player.whoAmI);
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.Check_ItemTypeSource(player.HeldItem.type)) {
			int damage = player.GetWeaponDamage(player.HeldItem);
			for (int i = 0; i < 3; i++) {
				Vector2 pos = target.Center + Main.rand.NextVector2CircularEdge(target.width, target.height);
				Vector2 vel = (target.Center - pos).SafeNormalize(Vector2.Zero) * 5;
				Projectile.NewProjectile(player.GetSource_FromAI(), pos, vel, ProjectileID.CrystalStorm, damage, 1, player.whoAmI);
			}
		}
	}
}
public class CursedFlames : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.CursedFlames;
	}
	public override void ModifyManaCost(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) {
		multi += .35f;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (++globalItem.Item_Counter1[index] >= 5) {
			if (player.CheckMana(5, true, true)) {
				damage += 20;
			}
			Projectile.NewProjectile(source, position, velocity, ProjectileID.CursedFlameFriendly, damage, knockback, player.whoAmI);
			globalItem.Item_Counter1[index] = 0;
		}
	}
	public override void ModifyHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.CursedInferno)) {
			modifiers.SourceDamage += .47f;
		}
	}
	public override void ModifyHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.CursedInferno)) {
			modifiers.SourceDamage += .47f;
		}
	}
}
public class MagnetSphere : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.MagnetSphere;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		if (globalItem.Item_Counter1[index] >= 1000 || globalItem.Item_Counter1[index] < 0) {
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Enchantment_MagnetOrbProjectile>()] < 1) {
				globalItem.Item_Counter1[index] = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ModContent.ProjectileType<Enchantment_MagnetOrbProjectile>(), 1, 1, player.whoAmI);
			}
		}
		else {
			Projectile proj = Main.projectile[globalItem.Item_Counter1[index]];
			if (Main.projectile[globalItem.Item_Counter1[index]] != null) {
				if (proj.active) {
					proj.timeLeft = 100;
				}
				else {
					globalItem.Item_Counter1[index] = -1;
				}
			}
		}
	}
	public class Enchantment_MagnetOrbProjectile : ModProjectile {
		public override string Texture => ModUtils.GetVanillaTexture<Projectile>(ProjectileID.MagnetSphereBall);
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 40;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Main.projFrames[Type] = 5;
		}
		public override bool? CanDamage() => false;
		private void MovementHandle(Player player) {
			Projectile.velocity *= .98f;
			Vector2 positionToBeAt = player.Center + Vector2.UnitX * 50 * -player.direction;
			if (!Projectile.Center.IsCloseToPosition(player.Center, 2000)) {
				Projectile.Center = player.Center;
			}
			if (!Projectile.Center.IsCloseToPosition(positionToBeAt, 200)) {
				Projectile.velocity += (positionToBeAt - Projectile.Center).SafeNormalize(Vector2.Zero) / 32f * 10;
			}
			else if (!Projectile.Center.IsCloseToPosition(positionToBeAt, 100)) {
				Projectile.velocity += (positionToBeAt - Projectile.Center).SafeNormalize(Vector2.Zero) / 64f * 10;
			}
			else {
				Projectile.velocity += (positionToBeAt - Projectile.Center).SafeNormalize(Vector2.Zero) / 128f * 10;
			}
		}
		public override void AI() {
			if (Projectile.timeLeft < 100) {
				Projectile.alpha = (byte)MathHelper.Lerp(255, 0, Projectile.timeLeft / 100f);
			}
			else {
				Projectile.alpha = ModUtils.CountDown(Projectile.alpha);
			}
			Player player = Main.player[Projectile.owner];
			Projectile.damage = player.GetWeaponDamage(player.HeldItem) + 1;
			MovementHandle(player);
			if (Main.rand.NextBool(5)) {
				Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Electric);
				dust.velocity = Main.rand.NextVector2CircularEdge(10, 10);
				dust.noGravity = true;
				dust.scale = Main.rand.NextFloat(.2f, .6f);
			}

			if (Projectile.velocity.X > 0f)
				Projectile.rotation += (Math.Abs(Projectile.velocity.Y) + Math.Abs(Projectile.velocity.X)) * 0.001f;
			else
				Projectile.rotation -= (Math.Abs(Projectile.velocity.Y) + Math.Abs(Projectile.velocity.X)) * 0.001f;

			Projectile.frameCounter++;
			if (Projectile.frameCounter > 6) {
				Projectile.frameCounter = 0;
				Projectile.frame++;
				if (Projectile.frame > 4)
					Projectile.frame = 0;
			}

			if (++Projectile.ai[2] < 10) {
				return;
			}
			Projectile.Center.LookForHostileNPC(out List<NPC> npclist, 650);
			foreach (var npc in npclist) {
				Vector2 vel = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 10;
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, vel, ProjectileID.MagnetSphereBolt, Projectile.damage, Projectile.knockBack, Projectile.owner);
			}
			if (npclist.Count > 0)
				Projectile.ai[2] = 0;
		}
	}
}
public class GoldenShower : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.GoldenShower;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		if (player.ItemAnimationActive) {
			if (Main.rand.NextBool(10)) {
				int damage = player.GetWeaponDamage(item);
				Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Main.rand.NextVector2CircularEdge(15, 15) * Main.rand.NextFloat(.8f, 1.2f),
					ProjectileID.GoldenShowerFriendly, damage, 1, player.whoAmI);
			}
		}
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff(BuffID.Ichor, ModUtils.ToSecond(Main.rand.Next(3, 5)));
	}
	public override void ModifyHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.Ichor)) {
			if (proj.type == ProjectileID.GoldenShowerFriendly) {
				modifiers.SourceDamage += .5f;
				modifiers.SourceDamage.Flat += 10;
			}
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff(BuffID.Ichor, ModUtils.ToSecond(Main.rand.Next(3, 5)));
	}
}
public class LeafBlower : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.LeafBlower;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (Main.rand.NextBool(10)) {
			Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(10), ProjectileID.Leaf, damage, knockback, player.whoAmI);
		}
		if (item.DamageType == DamageClass.Magic) {
			if (Main.rand.NextBool(3)) {
				Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(10), ProjectileID.Leaf, damage, knockback, player.whoAmI);
			}
		}
		if (item.type == ItemID.LeafBlower) {
			Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(10), ProjectileID.Leaf, (int)(damage * .45f), knockback, player.whoAmI);
		}
	}
}
public class HeatRay : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.HeatRay;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		if (--globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 10);
			player.Center.LookForHostileNPC(out List<NPC> npclist, 1000);
			if (npclist.Count <= 0) {
				return;
			}
			int damage = (int)(player.GetWeaponDamage(item) * .35f) + 10;
			foreach (NPC target in npclist) {
				Vector2 vel = (target.Center - player.Center).SafeNormalize(Vector2.Zero) * 4;
				Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, vel, ProjectileID.HeatRay, damage, 1, player.whoAmI);
			}
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.type == ProjectileID.HeatRay) {
			int count = target.GetGlobalNPC<Roguelike_HeatRay_GlobalNPC>().HeatRay_HitCount;
			count = Math.Clamp(count + 1, 0, 200);
			target.GetGlobalNPC<Roguelike_HeatRay_GlobalNPC>().HeatRay_HitCount = count;
		}
	}
}
public class MagicDagger : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.MagicDagger;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage += .15f;
		if (item.DamageType == DamageClass.Magic) {
			damage += .3f;
		}
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] > 0) {
			return;
		}
		globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 60);
		int damage = player.GetWeaponDamage(item);
		int amount = Main.rand.Next(7, 14);
		for (int i = 0; i < amount; i++) {
			Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Main.rand.NextVector2CircularEdge(10, 10) * Main.rand.NextFloat(.8f, 1.2f), ProjectileID.MagicDagger, damage, 1, player.whoAmI);
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter2[index] > 0) {
			return;
		}
		globalItem.Item_Counter2[index] = PlayerStatsHandle.WE_CoolDown(player, 180);
		int damage = player.GetWeaponDamage(player.HeldItem);
		int amount = Main.rand.Next(7, 14);
		for (int i = 0; i < amount; i++) {
			Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), target.Center, Main.rand.NextVector2CircularEdge(10, 10) * Main.rand.NextFloat(.8f, 1.2f), ProjectileID.MagicDagger, damage, 1, player.whoAmI);
		}
	}
}
public class NimbusRod : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.NimbusRod;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		if (globalItem.Item_Counter1[index] >= 1000 || globalItem.Item_Counter1[index] < 0) {
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Enchantment_NimbusRodProjectile>()] < 1) {
				globalItem.Item_Counter1[index] = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ModContent.ProjectileType<Enchantment_NimbusRodProjectile>(), 1, 1, player.whoAmI);
			}
		}
		else {
			Projectile proj = Main.projectile[globalItem.Item_Counter1[index]];
			if (Main.projectile[globalItem.Item_Counter1[index]] != null) {
				if (proj.active) {
					proj.timeLeft = 100;
				}
				else {
					globalItem.Item_Counter1[index] = -1;
				}
			}
		}
	}
	public class Enchantment_NimbusRodProjectile : ModProjectile {
		public override string Texture => ModUtils.GetVanillaTexture<Projectile>(ProjectileID.RainCloudRaining);
		public override void SetDefaults() {
			Main.projFrames[Type] = Main.projFrames[ProjectileID.RainCloudRaining];
			Projectile.width = Projectile.height = 40;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
		}
		public override bool? CanDamage() => false;
		private void MovementHandle(Player player) {
			Projectile.velocity *= .98f;
			Vector2 positionToBeAt = player.Center + Vector2.UnitY * -150;
			if (!Projectile.Center.IsCloseToPosition(player.Center, 2000)) {
				Projectile.Center = player.Center;
			}
			if (!Projectile.Center.IsCloseToPosition(positionToBeAt, 200)) {
				Projectile.velocity += (positionToBeAt - Projectile.Center).SafeNormalize(Vector2.Zero) / 32f * 20;
			}
			else if (!Projectile.Center.IsCloseToPosition(positionToBeAt, 100)) {
				Projectile.velocity += (positionToBeAt - Projectile.Center).SafeNormalize(Vector2.Zero) / 64f * 20;
			}
			else {
				Projectile.velocity += (positionToBeAt - Projectile.Center).SafeNormalize(Vector2.Zero) / 128f * 20;
			}
		}
		public override void AI() {
			if (Projectile.timeLeft < 100) {
				Projectile.alpha = (byte)MathHelper.Lerp(255, 0, Projectile.timeLeft / 100f);
			}
			else {
				Projectile.alpha = ModUtils.CountDown(Projectile.alpha);
			}
			Player player = Main.player[Projectile.owner];
			Projectile.damage = player.GetWeaponDamage(player.HeldItem) + 1;
			MovementHandle(player);
			if (Main.rand.NextBool(5)) {
				Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Electric);
				dust.velocity = Main.rand.NextVector2CircularEdge(10, 10);
				dust.noGravity = true;
				dust.scale = Main.rand.NextFloat(.2f, .6f);
			}

			Projectile.rotation = 0;

			Projectile.frameCounter++;
			if (Projectile.frameCounter > 6) {
				Projectile.frameCounter = 0;
				Projectile.frame++;
				if (Projectile.frame >= Main.projFrames[Type])
					Projectile.frame = 0;
			}

			if (++Projectile.ai[2] < 3) {
				return;
			}
			Projectile.NewProjectile(Projectile.GetSource_FromAI(),
				Projectile.Center + Main.rand.NextVector2Circular(20, 10),
				Vector2.UnitY * Main.rand.NextFloat(3, 9),
				ProjectileID.RainFriendly,
				Projectile.damage,
				1,
				Projectile.owner);
			Projectile.ai[2] = 0;
		}
	}
}
public class MedusaHead : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.MedusaHead;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.buffImmune[BuffID.Stoned] = true;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage += .1f;
	}
}
public class SpiritFlame : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.SpiritFlame;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		if (player.Center.LookForAnyHostileNPC(500)) {
			if (globalItem.Item_Counter2[index] <= 0) {
				int damage = (int)(player.GetWeaponDamage(item) * 1.5f) + 20;
				Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center + Main.rand.NextVector2CircularEdge(100, 100) * Main.rand.NextFloat(.8f, 1.2f), Vector2.Zero, ProjectileID.SpiritFlame, damage, 1f, player.whoAmI);
				globalItem.Item_Counter2[index] = PlayerStatsHandle.WE_CoolDown(player, 180);
			}
		}
		globalItem.Item_Counter2[index] = ModUtils.CountDown(globalItem.Item_Counter2[index]);
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage += .1f;
		if (item.DamageType == DamageClass.Magic) {
			damage += .35f;
		}
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (++globalItem.Item_Counter1[index] >= 3) {
			Projectile.NewProjectile(source, position + Main.rand.NextVector2CircularEdge(100, 100) * Main.rand.NextFloat(.8f, 1.2f), Vector2.Zero, ProjectileID.SpiritFlame, damage, knockback, player.whoAmI);
			globalItem.Item_Counter1[index] = 0;
		}
	}
}
