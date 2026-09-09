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
		RelicTierUPValue = .45f;
	}
	PlayerStats[] stats = new[] { PlayerStats.MagicDMG, PlayerStats.MagicCritDmg, PlayerStats.MagicCritChance, PlayerStats.MagicAtkSpeed };
	public override PlayerStats StatCondition(Relic relic, Player player) => Main.rand.Next(stats);
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		string name = Enum.GetName(stat) ?? string.Empty;
		if (stat == PlayerStats.MagicCritChance) {
			return string.Format(Description, [
				Color.Yellow.Hex3(),name,$"{RelicTemplateLoader.RelicValueToNumber(value.Base)}%",
				]);
		}
		return string.Format(Description, [
			Color.Yellow.Hex3(),name, RelicTemplateLoader.RelicValueToPercentage(value.Additive)
		]);
	}

	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		switch (stat) {
			case PlayerStats.MagicDMG:
				return new StatModifier(MathF.Round(Main.rand.NextFloat(1.09f, 1.15f), 2), 1, 0, 0);
			case PlayerStats.MagicCritChance:
				return new StatModifier(1, 1, 0, Main.rand.Next(6, 11));
			case PlayerStats.MagicCritDmg:
				return new StatModifier(MathF.Round(Main.rand.NextFloat(1.1f, 1.21f), 2), 1, 0, 0);
			case PlayerStats.MagicAtkSpeed:
				return new StatModifier(MathF.Round(Main.rand.NextFloat(1.05f, 1.11f), 2), 1, 0, 0);
		}
		return new StatModifier(MathF.Round(Main.rand.NextFloat(1.02f, 1.11f), 2), 1, 0, 0);
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		modplayer.AddStatsToPlayer(stat, value);
		player.manaCost += .15f;
	}
}
