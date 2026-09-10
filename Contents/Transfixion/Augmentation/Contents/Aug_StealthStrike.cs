using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using System.Linq;
using Terraria;
using Terraria.ID;

namespace Roguelike.Contents.Transfixion.Augmentation.Contents;
public class StealthStrike : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Microsoft.Xna.Framework.Color.DarkGray;
		ItemTypeID = ItemID.ShadowOrb;
	}
	public override int[] UpgradeAvailable() => [ItemID.ShroomiteBar];
	public override string Description2(Player player, AugmentsWeapon acc, Item item, string Extra) {
		string desc = base.Description2(player, acc, item, Extra);
		switch (Extra) {
			case "1":
				return Get_FormattedDescription(acc, desc, ItemID.ShroomiteBar);
			default:
				return desc;
		}
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.FullHPDamage, 2f);
	}
	public override void ModifyHitNPCWithItem(Player player, AugmentsWeapon acc, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (acc.AugmentUpgrade.Contains(ItemID.ShroomiteBar)) {
			if (player.invis)
				modifiers.SourceDamage += .25f;
		}
	}
	public override void ModifyHitNPCWithProj(Player player, AugmentsWeapon acc, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (acc.AugmentUpgrade.Contains(ItemID.ShroomiteBar)) {
			if (proj.Check_ItemTypeSource(player.HeldItem.type)) {
				if (player.invis)
					modifiers.SourceDamage += .25f;
			}
		}
	}
}
