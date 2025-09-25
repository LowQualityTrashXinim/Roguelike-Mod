using System;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;

namespace Roguelike.Contents.Items.RelicItem.RelicTemplateContent
{
	public class CombatHighHPTemplate : RelicTemplate {
		public override void SetStaticDefaults() {
			relicType = RelicType.Stat;
			RelicTierUPValue = .3f;
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
		]);
		}
		public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
			string Name = Enum.GetName(stat) ?? string.Empty;
			string valuestring;
			if (stat == PlayerStats.MeleeCritChance
				|| stat == PlayerStats.RangeCritChance
				|| stat == PlayerStats.MagicCritChance
				|| stat == PlayerStats.SummonCritChance) {
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
				return new StatModifier(1, 1, 0, Main.rand.Next(10, 16));
			}
			return new StatModifier(MathF.Round(Main.rand.NextFloat(1.15f, 1.2f), 2), 1);
		}
		public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
			if (player.IsHealthAbovePercentage(.9f)) {
				if (stat == PlayerStats.MeleeCritChance
				|| stat == PlayerStats.RangeCritChance
				|| stat == PlayerStats.MagicCritChance
				|| stat == PlayerStats.SummonCritChance) {
					modplayer.AddStatsToPlayer(stat, value);
				}
				else {
					modplayer.AddStatsToPlayer(stat, value);
				}
			}
		}
	}
}
