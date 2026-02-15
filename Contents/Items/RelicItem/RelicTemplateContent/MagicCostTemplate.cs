using System;
using Terraria;
using Terraria.ModLoader;
using Roguelike.Common.Utils;
using Microsoft.Xna.Framework;
using Roguelike.Common.Global;

namespace Roguelike.Contents.Items.RelicItem.RelicTemplateContent;
public class MagicCostTemplate : RelicTemplate {
	public override void SetStaticDefaults() {
		relicType = RelicType.MultiStats;
		RelicTierUPValue = .09f;
	}
	public override PlayerStats StatCondition(Relic relic, Player player) => PlayerStats.MagicDMG;
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		return string.Format(Description, [
			Color.Yellow.Hex3(),
		$"{Math.Round(value.Multiplicative, 2)}",
		RelicTemplateLoader.RelicValueToNumber(value.Flat)
		]);
	}

	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		return new StatModifier(1, MathF.Round(Main.rand.NextFloat(1.02f, 1.11f), 2), Main.rand.Next(1, 6), 0);
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		modplayer.AddStatsToPlayer(stat, value);
		player.manaCost += .15f;
	}
}
