using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Transfixion.WeaponEnchantment.OreBowEnchantments;

public class CopperBow : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.CopperBow;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += 1;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] > 0 || proj.type == ProjectileID.WoodenArrowFriendly && proj.ai[2] == 9999 || !proj.arrow) {
			return;
		}
		globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 12);
		Vector2 pos = player.Center + Main.rand.NextVector2Circular(40, 40);
		Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, (target.Center - pos).SafeNormalize(Vector2.Zero) * 15, ProjectileID.WoodenArrowFriendly, player.GetWeaponDamage(player.HeldItem), 2, player.whoAmI, 0, 0, 9999);
	}
}
public class TinBow : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.TinBow;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += 1;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] > 0 || proj.type == ProjectileID.WoodenArrowFriendly && proj.ai[2] == 9999 || !proj.arrow) {
			return;
		}
		globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 12);
		Vector2 pos = player.Center + Main.rand.NextVector2Circular(40, 40);
		Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, (target.Center - pos).SafeNormalize(Vector2.Zero) * 15, ProjectileID.WoodenArrowFriendly, player.GetWeaponDamage(player.HeldItem), 2, player.whoAmI, 0, 0, 9999);
	}
}
public class IronBow : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.IronBow;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += 1;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] > 0 || proj.type == ProjectileID.WoodenArrowFriendly && proj.ai[2] == 9999 || !proj.arrow) {
			return;
		}
		globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 12);
		Vector2 pos = player.Center + Main.rand.NextVector2Circular(40, 40);
		Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, (target.Center - pos).SafeNormalize(Vector2.Zero) * 15, ProjectileID.WoodenArrowFriendly, player.GetWeaponDamage(player.HeldItem), 2, player.whoAmI, 0, 0, 9999);
	}
}
public class LeadBow : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.LeadBow;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += 1;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] > 0 || proj.type == ProjectileID.WoodenArrowFriendly && proj.ai[2] == 9999 || !proj.arrow) {
			return;
		}
		globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 12);
		Vector2 pos = player.Center + Main.rand.NextVector2Circular(40, 40);
		Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, (target.Center - pos).SafeNormalize(Vector2.Zero) * 15, ProjectileID.WoodenArrowFriendly, player.GetWeaponDamage(player.HeldItem), 2, player.whoAmI, 0, 0, 9999);
	}
}
public class SilverBow : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.SilverBow;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += 1;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] > 0 || proj.type == ProjectileID.WoodenArrowFriendly && proj.ai[2] == 9999 || !proj.arrow) {
			return;
		}
		globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 12);
		Vector2 pos = player.Center + Main.rand.NextVector2Circular(40, 40);
		Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, (target.Center - pos).SafeNormalize(Vector2.Zero) * 15, ProjectileID.WoodenArrowFriendly, player.GetWeaponDamage(player.HeldItem), 2, player.whoAmI, 0, 0, 9999);
	}
}
public class TungstenBow : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.TungstenBow;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += 1;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] > 0 || proj.type == ProjectileID.WoodenArrowFriendly && proj.ai[2] == 9999 || !proj.arrow) {
			return;
		}
		globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 12);
		Vector2 pos = player.Center + Main.rand.NextVector2Circular(40, 40);
		Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, (target.Center - pos).SafeNormalize(Vector2.Zero) * 15, ProjectileID.WoodenArrowFriendly, player.GetWeaponDamage(player.HeldItem), 2, player.whoAmI, 0, 0, 9999);
	}
}
public class GoldBow : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.GoldBow;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += 1;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] > 0 || proj.type == ProjectileID.WoodenArrowFriendly && proj.ai[2] == 9999 || !proj.arrow) {
			return;
		}
		globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 12);
		Vector2 pos = player.Center + Main.rand.NextVector2Circular(40, 40);
		Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, (target.Center - pos).SafeNormalize(Vector2.Zero) * 15, ProjectileID.WoodenArrowFriendly, player.GetWeaponDamage(player.HeldItem), 2, player.whoAmI, 0, 0, 9999);
	}
}
public class PlatinumBow : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.PlatinumBow;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += 1;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] > 0 || proj.type == ProjectileID.WoodenArrowFriendly && proj.ai[2] == 9999 || !proj.arrow) {
			return;
		}
		globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 12);
		Vector2 pos = player.Center + Main.rand.NextVector2Circular(40, 40);
		Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, (target.Center - pos).SafeNormalize(Vector2.Zero) * 15, ProjectileID.WoodenArrowFriendly, damageDone, 2, player.whoAmI, 0, 0, 9999);
	}
}
