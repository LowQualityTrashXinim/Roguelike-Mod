using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using Terraria.DataStructures;
using System.Collections.Generic;
using Terraria.Localization;
using Roguelike.Common.Systems;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;

namespace Roguelike.Common.Systems.HellishEndeavour;
public class HellishEndeavourPlayer : ModPlayer {
	public override void UpdateEquips() {
		if (RoguelikeWorldProperty.HellishEndeavour) {
			PlayerStatsHandle.AddStatsToPlayer(Player, PlayerStats.LootDropIncrease, Multiplicative: 0);
		}
	}
	public override void OnHurt(Player.HurtInfo info) {
		if (RoguelikeWorldProperty.HellishEndeavour) {
			Player.KillMe(PlayerDeathReason.ByCustomReason(NetworkText.FromLiteral($"{Player.name} has fail the challenge")), 9999999999, info.HitDirection);
			return;
		}
	}
}
