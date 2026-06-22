using Roguelike.Common.Utils;
using Terraria;

namespace Roguelike.Contents.Transfixion.Perks.PerkContents;
internal class SoulBreaker : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 10;
	}
	public override void UpdateEquip(Player player) {
		var handler = player.ModPlayerStats();
		handler.TrueDamage += .1f * StackAmount(player);
		handler.PercentageDamage += .001f * StackAmount(player);
		handler.AddStatsToPlayer(PlayerStats.PureDamage, 1, .95f, 0, 0);
	}
}
