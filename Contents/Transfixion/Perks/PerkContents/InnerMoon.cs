using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Terraria;

namespace Roguelike.Contents.Transfixion.Perks.PerkContents;
internal class InnerMoon : Perk{
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 3;
	}
	public override void UpdateEquip(Player player) {
		if (player.IsHealthAbovePercentage(.6f)) {
			return;
		}
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.RegenHP, 1.25f, Flat: 10 * StackAmount(player));
		modplayer.AddStatsToPlayer(PlayerStats.CritDamage, Multiplicative: 1.4f + .1f * StackAmount(player));
		modplayer.AddStatsToPlayer(PlayerStats.MovementSpeed, 1.55f + .1f * StackAmount(player));
		modplayer.AddStatsToPlayer(PlayerStats.JumpBoost, 1.55f + .1f * StackAmount(player));
	}
}
