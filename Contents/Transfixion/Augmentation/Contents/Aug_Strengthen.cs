using Roguelike.Common.Utils;
using System.Linq;
using Terraria;
using Terraria.ID;

namespace Roguelike.Contents.Transfixion.Augmentation.Contents;
public class Strengthen : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Microsoft.Xna.Framework.Color.IndianRed;
		ItemTypeID = ItemID.DestroyerEmblem;
	}
	public override int[] UpgradeAvailable() => [ItemID.CelestialShell, ItemID.MoonStone, ItemID.SunStone, ItemID.CelestialStone];
	public override string Description2(Player player, AugmentsWeapon acc, Item item, string Extra) {
		string desc = base.Description2(player, acc, item, Extra);
		switch (Extra) {
			case "1":
				return Get_FormattedDescription(acc, desc, ItemID.SunStone);
			case "2":
				return Get_FormattedDescription(acc, desc, ItemID.MoonStone);
			case "3":
				return Get_FormattedDescription(acc, desc, ItemID.CelestialStone);
			case "4":
				return Get_FormattedDescription(acc, desc, ItemID.CelestialShell);
			default:
				return desc;
		}
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, Item item) {
		var stathandle = player.ModPlayerStats();
		float multiplier = 1;
		multiplier += acc.AugmentUpgrade.Where(i =>
		i == ItemID.MoonStone
		|| i == ItemID.SunStone).Count();
		multiplier += acc.AugmentUpgrade.Contains(ItemID.CelestialStone) ? 1.5f : 0;
		multiplier += acc.AugmentUpgrade.Contains(ItemID.CelestialShell) ? 2 : 0;

		stathandle.AddStatsToPlayer(PlayerStats.PureDamage, 1.03f * multiplier);
		stathandle.AddStatsToPlayer(PlayerStats.CritDamage, 1.06f * multiplier);
		stathandle.AddStatsToPlayer(PlayerStats.Defense, Base: 2 * multiplier);
		stathandle.AddStatsToPlayer(PlayerStats.MaxHP, Base: 5 * multiplier);
		stathandle.AddStatsToPlayer(PlayerStats.MaxMana, Base: 5 * multiplier);
		stathandle.AddStatsToPlayer(PlayerStats.RegenHP, Base: 1 * multiplier);
		stathandle.AddStatsToPlayer(PlayerStats.CritChance, Base: 1 * multiplier);
	}
}
