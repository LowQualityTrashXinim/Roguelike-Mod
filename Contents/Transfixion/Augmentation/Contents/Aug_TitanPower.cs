using Roguelike.Common.Global;
using System.Linq;
using Terraria;
using Terraria.ID;

namespace Roguelike.Contents.Transfixion.Augmentation.Contents;
public class TitanPower : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Microsoft.Xna.Framework.Color.Blue;
		ItemTypeID = ItemID.TitanGlove;
	}
	public override int[] UpgradeAvailable() => [ItemID.TitaniumBar];
	public override string Description2(Player player, AugmentsWeapon acc, Item item, string Extra) {
		string desc = base.Description2(player, acc, item, Extra);
		switch (Extra) {
			case "1":
				return Get_FormattedDescription(acc, desc, ItemID.TitaniumBar);
			default:
				return desc;
		}
	}
	public override void ModifyHitNPCWithItem(Player player, AugmentsWeapon acc, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		int damage = (int)player.GetWeaponKnockback(item);
		modifiers.SourceDamage.Base += damage;
		if (acc.AugmentUpgrade.Contains(ItemID.TitaniumBar)) {
			int knockbackStrength = (int)(player.GetWeaponDamage(item) * .05f);
			modifiers.Knockback += knockbackStrength;
		}
	}
	public override void ModifyHitNPCWithProj(Player player, AugmentsWeapon acc, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type) {
			int damage = (int)player.GetWeaponKnockback(player.HeldItem);
			modifiers.SourceDamage.Base += damage;
			if (acc.AugmentUpgrade.Contains(ItemID.TitaniumBar)) {
				int knockbackStrength = (int)(player.GetWeaponDamage(player.HeldItem) * .05f);
				modifiers.Knockback += knockbackStrength;
			}
		}
	}
}
