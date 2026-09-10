using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using System.Linq;
using Terraria;
using Terraria.ID;

namespace Roguelike.Contents.Transfixion.Augmentation.Contents;
public class Vampire : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Microsoft.Xna.Framework.Color.DarkRed;
		ItemTypeID = ItemID.VampireKnives;
	}
	public override int[] UpgradeAvailable() => [ItemID.VampireFrogStaff, ItemID.SharpTears];
	public override string Description2(Player player, AugmentsWeapon acc, Item item, string Extra) {
		string desc = base.Description2(player, acc, item, Extra);
		switch (Extra) {
			case "1":
				return Get_FormattedDescription(acc, desc, ItemID.VampireFrogStaff);
			case "2":
				return Get_FormattedDescription(acc, desc, ItemID.SharpTears);
			default:
				return desc;
		}
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, Item item) {
		player.GetModPlayer<PlayerStatsHandle>().LifeSteal += 0.01f;
		if (!player.IsHealthAbovePercentage(.6f) && acc.AugmentUpgrade.Contains(ItemID.VampireFrogStaff))
			player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.CritDamage, Multiplicative: 1.5f);
		if (!player.IsHealthAbovePercentage(.8f) && acc.AugmentUpgrade.Contains(ItemID.SharpTears))
			PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.RegenHP, 1.25f, Base: 3);
	}
}
