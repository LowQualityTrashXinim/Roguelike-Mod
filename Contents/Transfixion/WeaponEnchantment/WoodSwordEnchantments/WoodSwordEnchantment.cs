using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.RoguelikeMode.ItemOverhaul.Specific;
using Roguelike.Common.Utils;
using Roguelike.Contents.Projectiles;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Transfixion.WeaponEnchantment.WoodSwordEnchantments;

public class WoodenSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.WoodenSword;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += 5;
	}
	public override void ModifyItemScale(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float scale) {
		scale += .2f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
		player.GetModPlayer<PlayerStatsHandle>().EnchantmentCoolDown -= .15f;
	}
}
public class AshWoodSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.AshWoodSword;
	}
	public override string ModifyDesc(string desc) {
		return string.Format(desc, $"[i:{ItemID.WoodenSword}]" +
			$"[i:{ItemID.BorealWoodSword}]" +
			$"[i:{ItemID.PalmWoodSword}]" +
			$"[i:{ItemID.EbonwoodSword}]" +
			$"[i:{ItemID.ShadewoodSword}]" +
			$"[i:{ItemID.RichMahoganySword}]" +
			$"[i:{ItemID.PearlwoodSword}]", $"[i:{ItemID.AshWoodSword}]");
	}
	public override bool ApplyCondition(int index, Player player, EnchantmentGlobalItem globalItem, Item item) {
		return TerrariaArrayID.AllWoodSword.Contains(item.type);
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
		if (player.ItemAnimationActive && player.itemAnimation == player.itemAnimationMax) {
			if (++globalItem.Item_Counter2[index] >= 5) {
				globalItem.Item_Counter2[index] = 0;
				Vector2 pos = new Vector2(Main.MouseWorld.X + (Main.rand.Next(-150, 150)), player.Center.Y - 1000);
				Vector2 vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 20;
				Projectile.NewProjectile(player.GetSource_ItemUse(item), pos, vel, ModContent.ProjectileType<AshwoodSwordProjectile>(), (int)(player.GetWeaponDamage(item) * 1.25f) + 1, item.knockBack, player.whoAmI, 1);
			}
			if (item.type == ItemID.AshWoodSword && globalItem.Item_Counter1[index] <= 0) {
				globalItem.Item_Counter2[index] += 5;
				globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 30);
				var pos = Main.MouseWorld.Subtract(Main.rand.NextFloat(-100, 100), Main.rand.NextFloat(50, 100));
				var vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 20;
				int projec = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, vel, ModContent.ProjectileType<SwordProjectileSpear>(), (int)(player.GetWeaponDamage(item) * 1.25f) + 1, item.knockBack, player.whoAmI);
				if (Main.projectile[projec].ModProjectile is SwordProjectileSpear woodproj)
					woodproj.ItemIDtextureValue = ItemIDType;
			}
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (player.HeldItem.type == ItemID.AshWoodSword) {
			return;
		}
		if (globalItem.Item_Counter1[index] <= 0 && proj.type != ModContent.ProjectileType<SwordProjectile2>()) {
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 60);
			var pos = target.Center.Subtract(Main.rand.NextFloat(-100, 100), Main.rand.NextFloat(50 + target.height, 100 + target.height));
			var vel = (target.Center - pos).SafeNormalize(Vector2.Zero) * 12;
			int projec = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, vel, ModContent.ProjectileType<SwordProjectileSpear>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[projec].ModProjectile is SwordProjectileSpear woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (player.HeldItem.type == ItemID.AshWoodSword) {
			return;
		}
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 60);
			var pos = target.Center.Subtract(Main.rand.NextFloat(-100, 100), Main.rand.NextFloat(50 + target.height, 100 + target.height));
			var vel = (target.Center - pos).SafeNormalize(Vector2.Zero) * 12;
			int projec = Projectile.NewProjectile(player.GetSource_ItemUse(item), pos, vel, ModContent.ProjectileType<SwordProjectileSpear>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[projec].ModProjectile is SwordProjectileSpear woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
}
public class BorealWoodSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.BorealWoodSword;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += 1;
	}
	public override void ModifyItemScale(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float scale) {
		scale += .1f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0 && proj.type != ModContent.ProjectileType<SwordProjectile2>()) {
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 60);
			var pos = player.Center + Main.rand.NextVector2CircularEdge(60, 60) * 60;
			var vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 12;
			int projec = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, vel, ModContent.ProjectileType<SwordProjectile2>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[projec].ModProjectile is SwordProjectile2 woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 60);
			var pos = player.Center + Main.rand.NextVector2CircularEdge(60, 60) * 60;
			var vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 12;
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), pos, vel, ModContent.ProjectileType<SwordProjectile2>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[proj].ModProjectile is SwordProjectile2 woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
}
public class PalmWoodSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.PalmWoodSword;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += 1;
	}
	public override void ModifyItemScale(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float scale) {
		scale += .1f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0 && proj.type != ModContent.ProjectileType<SwordProjectile2>()) {
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 60);
			var pos = player.Center + Vector2.One.Vector2DistributeEvenly(3, 360, index) * 60;
			var vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 12;
			int projec = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, vel, ModContent.ProjectileType<SwordProjectile2>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[projec].ModProjectile is SwordProjectile2 woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 60);
			var pos = player.Center + Vector2.One.Vector2DistributeEvenly(3, 360, index) * 60;
			var vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 12;
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), pos, vel, ModContent.ProjectileType<SwordProjectile2>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[proj].ModProjectile is SwordProjectile2 woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
}
public class RichMahoganySword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.RichMahoganySword;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += 1;
	}
	public override void ModifyItemScale(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float scale) {
		scale += .1f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0 && proj.type != ModContent.ProjectileType<SwordProjectile2>()) {
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 60);
			var pos = player.Center + Vector2.One.Vector2DistributeEvenly(3, 360, index) * 60;
			var vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 12;
			int projec = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, vel, ModContent.ProjectileType<SwordProjectile2>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[projec].ModProjectile is SwordProjectile2 woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 60);
			var pos = player.Center + Vector2.One.Vector2DistributeEvenly(3, 360, index) * 60;
			var vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 12;
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), pos, vel, ModContent.ProjectileType<SwordProjectile2>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[proj].ModProjectile is SwordProjectile2 woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
}
public class ShadewoodSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.ShadewoodSword;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += 1;
	}
	public override void ModifyItemScale(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float scale) {
		scale += .1f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0 && proj.type != ModContent.ProjectileType<SwordProjectile2>()) {
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 60);
			var pos = player.Center + Vector2.One.Vector2DistributeEvenly(3, 360, index) * 60;
			var vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 12;
			int projec = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, vel, ModContent.ProjectileType<SwordProjectile2>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[projec].ModProjectile is SwordProjectile2 woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 60);
			var pos = player.Center + Vector2.One.Vector2DistributeEvenly(3, 360, index) * 60;
			var vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 12;
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), pos, vel, ModContent.ProjectileType<SwordProjectile2>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[proj].ModProjectile is SwordProjectile2 woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
}
public class EbonwoodSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.EbonwoodSword;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += 1;
	}
	public override void ModifyItemScale(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref float scale) {
		scale += .1f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0 && proj.type != ModContent.ProjectileType<SwordProjectile2>()) {
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 60);
			var pos = player.Center + Vector2.One.Vector2DistributeEvenly(3, 360, index) * 60;
			var vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 12;
			int projec = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, vel, ModContent.ProjectileType<SwordProjectile2>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[projec].ModProjectile is SwordProjectile2 woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 60);
			var pos = player.Center + Vector2.One.Vector2DistributeEvenly(3, 360, index) * 60;
			var vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 12;
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), pos, vel, ModContent.ProjectileType<SwordProjectile2>(), hit.Damage, hit.Knockback, player.whoAmI);
			if (Main.projectile[proj].ModProjectile is SwordProjectile2 woodproj)
				woodproj.ItemIDtextureValue = ItemIDType;
		}
	}
}
