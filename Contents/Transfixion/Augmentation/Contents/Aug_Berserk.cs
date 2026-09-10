using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Transfixion.Augmentation.Contents;
public class Berserk : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.OrangeRed;
		ItemTypeID = ItemID.AvengerEmblem;
	}
	public override int[] UpgradeAvailable() => [ItemID.SoulofFright];
	public override string Description2(Player player, AugmentsWeapon acc, Item item, string Extra) {
		string desc = base.Description2(player, acc, item, Extra);
		switch (Extra) {
			case "1":
				return Get_FormattedDescription(acc, desc, ItemID.SoulofFright);
			default:
				return desc;
		}
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, Item item) {
		if (acc.AugmentUpgrade.Contains(ItemID.SoulofFright)) {
			if (!player.IsHealthAbovePercentage(.7f)) {
				player.GetDamage(DamageClass.Generic) += .25f;
			}
		}
	}
	public override void ModifyHitNPCWithItem(Player player, AugmentsWeapon acc, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		float percentage = player.statLife / (float)player.statLifeMax2;
		modifiers.SourceDamage += .5f * percentage;
	}
	public override void ModifyHitNPCWithProj(Player player, AugmentsWeapon acc, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type && !proj.minion) {
			float percentage = player.statLife / (float)player.statLifeMax2;
			modifiers.SourceDamage += .5f * percentage;
		}
	}
}
