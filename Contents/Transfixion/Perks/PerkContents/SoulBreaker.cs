using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Contents.Transfixion.Perks;
using Terraria;

namespace Roguelike.Contents.Transfixion.Perks.PerkContents;
internal class SoulBreaker : Perk {
	public override void SetDefaults() {
		CanBeStack = false;
	}
	public override void UpdateEquip(Player player) {
		var handler = player.ModPlayerStats();
		handler.TrueDamage += .5f;
		handler.PercentageDamage += .001f;
		handler.AddStatsToPlayer(PlayerStats.PureDamage, 1, .25f, 0, 0);
	}
}
