using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;

namespace Roguelike.Contents.Items.RelicItem.RelicTemplateContent;

public class StrikeFullHPTemplate : RelicTemplate {
	public override void SetStaticDefaults() {
		relicType = RelicType.Stat;
		RelicTierUPValue = .25f;
	}
	public override PlayerStats StatCondition(Relic relic, Player player) => PlayerStats.FullHPDamage;
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		return string.Format(Description, [Color.Yellow.Hex3(), RelicTemplateLoader.RelicValueToPercentage(value.Additive),]);
	}
	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		return new StatModifier(MathF.Round(Main.rand.NextFloat(.75f, 1f) + 1, 2), 1, 0, 0);
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		modplayer.AddStatsToPlayer(stat, value);
	}
}
