using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;
using Roguelike.Contents.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Transfixion.WeaponEnchantment.WoodBowEnchantments;
public class WoodenBow : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.WoodenBow;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		Projectile.NewProjectile(source, position.PositionOFFSET(velocity, 30) + Main.rand.NextVector2Circular(10, 10), velocity, ProjectileID.WoodenArrowFriendly, damage, knockback, player.whoAmI);
		if (Main.rand.NextBool(4)) {
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<WindShot>(), damage, knockback, player.whoAmI);
		}
	}
}
public class AshWoodBow : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.AshWoodBow;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		Projectile.NewProjectile(source, position.PositionOFFSET(velocity, 30) + Main.rand.NextVector2Circular(10, 10), velocity, ProjectileID.WoodenArrowFriendly, damage, knockback, player.whoAmI);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.type == ProjectileID.WoodenArrowFriendly && Main.rand.NextFloat() <= .3f) {
			target.AddBuff(BuffID.OnFire, ModUtils.ToSecond(5));
		}
	}
}
public class BorealWoodBow : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.BorealWoodBow;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		Projectile.NewProjectile(source, position.PositionOFFSET(velocity, 30) + Main.rand.NextVector2Circular(10, 10), velocity, ProjectileID.WoodenArrowFriendly, damage, knockback, player.whoAmI);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.type == ProjectileID.WoodenArrowFriendly && Main.rand.NextFloat() <= .3f) {
			target.AddBuff(BuffID.Frostburn, ModUtils.ToSecond(5));
		}
	}
}
public class RichMahoganyBow : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.RichMahoganyBow;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		Projectile.NewProjectile(source, position.PositionOFFSET(velocity, 30) + Main.rand.NextVector2Circular(10, 10), velocity, ProjectileID.WoodenArrowFriendly, damage, knockback, player.whoAmI);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.type == ProjectileID.WoodenArrowFriendly && Main.rand.NextFloat() <= .3f) {
			target.AddBuff(BuffID.Poisoned, ModUtils.ToSecond(5));
		}
	}
}
public class EbonwoodBow : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.EbonwoodBow;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		Projectile.NewProjectile(source, position.PositionOFFSET(velocity, 30) + Main.rand.NextVector2Circular(10, 10), velocity, ProjectileID.WoodenArrowFriendly, damage, knockback, player.whoAmI);
	}
}
public class ShadewoodBow : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.ShadewoodBow;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		Projectile.NewProjectile(source, position.PositionOFFSET(velocity, 30) + Main.rand.NextVector2Circular(10, 10), velocity, ProjectileID.WoodenArrowFriendly, damage, knockback, player.whoAmI);
	}
}
public class PalmWoodBow : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.PalmWoodBow;
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		Projectile.NewProjectile(source, position.PositionOFFSET(velocity, 30) + Main.rand.NextVector2Circular(10, 10), velocity, ProjectileID.WoodenArrowFriendly, damage, knockback, player.whoAmI);
	}
}
