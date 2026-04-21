using System;
using Terraria;
using Terraria.ModLoader;
using Roguelike.Common.Utils;
using Roguelike.Common.Global;
using Microsoft.Xna.Framework;

namespace Roguelike.Contents.Items.RelicItem.RelicTemplateContent;

public class BattleMountTemplate : RelicTemplate {
	public override void SetStaticDefaults() {
		relicType = RelicType.Stat;
		RelicTierUPValue = .34f;
	}
	public override PlayerStats StatCondition(Relic relic, Player player) {
		return Main.rand.Next([
			PlayerStats.MeleeDMG,
			PlayerStats.RangeDMG,
			PlayerStats.MagicDMG,
			PlayerStats.SummonDMG,
			PlayerStats.MeleeCritChance,
			PlayerStats.RangeCritChance,
			PlayerStats.MagicCritChance,
			PlayerStats.SummonCritChance,
			PlayerStats.MeleeCritDmg,
			PlayerStats.RangeCritDmg,
			PlayerStats.MagicCritDmg,
			PlayerStats.SummonCritDmg,
		]);
	}
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		string valuestring; string Name = Enum.GetName(stat) ?? string.Empty;
		if (stat == PlayerStats.MeleeCritChance
			|| stat == PlayerStats.RangeCritChance
			|| stat == PlayerStats.MagicCritChance
			|| stat == PlayerStats.SummonCritChance
			) {
			valuestring = RelicTemplateLoader.RelicValueToNumber(value.Base);
		}
		else {
			valuestring = RelicTemplateLoader.RelicValueToPercentage(value.Additive);
		}
		return string.Format(Description, [Color.Yellow.Hex3(), Name, valuestring]);
	}
	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		if (stat == PlayerStats.MeleeCritChance
			|| stat == PlayerStats.RangeCritChance
			|| stat == PlayerStats.MagicCritChance
			|| stat == PlayerStats.SummonCritChance) {
			return new StatModifier(1, 1, 0, Main.rand.Next(5, 11));
		}
		return new StatModifier(MathF.Round(Main.rand.NextFloat(1.05f, 1.11f), 2), 1);
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		if (player.mount.Active) {
			if (stat == PlayerStats.MeleeCritChance
			|| stat == PlayerStats.RangeCritChance
			|| stat == PlayerStats.MagicCritChance
			|| stat == PlayerStats.SummonCritChance) {
				modplayer.AddStatsToPlayer(stat, value.Additive, value.Multiplicative, value.Flat * 1.22f, value.Base * 1.22f);
			}
			else {
				modplayer.AddStatsToPlayer(stat, value.Additive * 1.22f, value.Multiplicative, value.Flat * 1.22f, value.Base * 1.22f);
			}
		}
		else {
			modplayer.AddStatsToPlayer(stat, value);
		}
	}
}
