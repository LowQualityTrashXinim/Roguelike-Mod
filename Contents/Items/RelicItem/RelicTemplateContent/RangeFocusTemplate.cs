using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.RelicItem.RelicTemplateContent;
internal class RangeFocusTemplate : RelicTemplate {
	public override void SetStaticDefaults() {
		relicType = RelicType.Stat;
		RelicTierUPValue = .84f;
	}
	public override PlayerStats StatCondition(Relic relic, Player player) {
		return Main.rand.Next([
			PlayerStats.RangeDMG,
			PlayerStats.RangeCritChance,
			PlayerStats.RangeCritDmg,
			PlayerStats.RangeAtkSpeed,
			PlayerStats.RangeNonCritDmg,
		]);
	}
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		string Number = stat == PlayerStats.RangeCritChance ? RelicTemplateLoader.RelicValueToNumber(value.Base) : RelicTemplateLoader.RelicValueToPercentage(value.Additive);
		StatModifier newstatForMelee = new StatModifier((1 - value.Additive) * .5f + 1, 1, -value.Flat, -value.Base);
		string Number2 = stat == PlayerStats.RangeCritChance ? RelicTemplateLoader.RelicValueToNumber(newstatForMelee.Base) : RelicTemplateLoader.RelicValueToPercentage(newstatForMelee.Additive);
		return string.Format(Description, [Color.Yellow.Hex3(), Name, Number, ConversionRangeToMelee(stat), Number2]);
	}

	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		if (stat == PlayerStats.RangeDMG) {
			return new StatModifier(1 + MathF.Round(Main.rand.NextFloat(.2f, .3f), 2), 1, 0, 0);
		}
		if (stat == PlayerStats.RangeCritChance) {
			return new StatModifier(1, 1, 0, Main.rand.Next(10, 21));
		}
		if (stat == PlayerStats.RangeCritDmg) {
			return new StatModifier(1 + MathF.Round(Main.rand.NextFloat(.3f, .4f), 2), 1, 0, 0);
		}
		if (stat == PlayerStats.RangeAtkSpeed) {
			return new StatModifier(1 + MathF.Round(Main.rand.NextFloat(.05f, .12f), 2), 1, 0, 0);
		}
		return new StatModifier(1 + MathF.Round(Main.rand.NextFloat(.2f, .25f), 2), 1, 0, 0);
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		modplayer.AddStatsToPlayer(stat, value);
		StatModifier newstatForMelee = new StatModifier((1 - value.Additive) * .5f + 1, 1, -value.Flat, -value.Base);
		modplayer.AddStatsToPlayer(ConversionRangeToMelee(stat), newstatForMelee);
	}
	private PlayerStats ConversionRangeToMelee(PlayerStats stat) {
		switch (stat) {
			case PlayerStats.RangeDMG:
				return PlayerStats.MeleeDMG;
			case PlayerStats.RangeCritChance:
				return PlayerStats.MeleeCritChance;
			case PlayerStats.RangeCritDmg:
				return PlayerStats.MeleeCritDmg;
			case PlayerStats.RangeAtkSpeed:
				return PlayerStats.MeleeAtkSpeed;
			case PlayerStats.RangeNonCritDmg:
				return PlayerStats.MeleeNonCritDmg;
		}
		return PlayerStats.MeleeDMG;
	}
}
