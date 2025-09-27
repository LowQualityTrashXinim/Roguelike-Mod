using System;
using Terraria;
using Terraria.ModLoader;
using Roguelike.Common.Utils;
using Microsoft.Xna.Framework;
using Roguelike.Common.Global;

namespace Roguelike.Contents.Items.RelicItem.RelicTemplateContent {
	public class LowHealthTemplate : RelicTemplate {
		public override void SetStaticDefaults() {
			relicType = RelicType.Stat;
			RelicTierUPValue = 1.2f;
		}
		public override PlayerStats StatCondition(Relic relic, Player player) {
			return Main.rand.Next([
				PlayerStats.RegenHP,
			PlayerStats.Defense,
		]);
		}
		public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
			string Name = Enum.GetName(stat) ?? string.Empty;
			return string.Format(Description, [Color.Yellow.Hex3(), Name, RelicTemplateLoader.RelicValueToNumber(value.Base)]);
		}
		public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
			if (stat == PlayerStats.RegenHP) {
				return new StatModifier(1, 1, 0, Main.rand.NextFloat(4, 5) * 2);
			}
			return new StatModifier(1, 1, 0, Main.rand.Next(7, 11));
		}
		public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
			if (!player.IsHealthAbovePercentage(.35f)) {
				modplayer.AddStatsToPlayer(stat, value);
			}
		}
	}
}
