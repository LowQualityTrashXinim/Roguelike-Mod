using System;
using Terraria;
using Terraria.ModLoader;
using Roguelike.Common.Utils;
using Microsoft.Xna.Framework;
using Roguelike.Common.Global;

namespace Roguelike.Contents.Items.RelicItem.RelicTemplateContent {
	public class DebuffedHealthTemplate : RelicTemplate {
		public override void SetStaticDefaults() {
			relicType = RelicType.Stat;
			RelicTierUPValue = 1.33f;
		}
		public override PlayerStats StatCondition(Relic relic, Player player) {
			return Main.rand.Next([
				PlayerStats.RegenHP,
				PlayerStats.Defense,
			]);
		}
		public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
			string Name = Enum.GetName(stat) ?? string.Empty;
			value.Base += value.Base * (relic.RelicTier - 1) / 3f;
			return string.Format(Description, args: [Color.Yellow.Hex3(), Name, RelicTemplateLoader.RelicValueToNumber(value.Base)]);
		}
		public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
			if (stat == PlayerStats.RegenHP) {
				return new StatModifier(1, 1, 0, Main.rand.NextFloat(3, 4) * 2.25f);
			}
			return new StatModifier(1, 1, 0, Main.rand.NextFloat(4, 5) * 1.5f);
		}
		public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
			for (int i = 0; i < player.buffType.Length; i++) {
				if (player.buffType[i] == 0) continue;
				if (Main.debuff[player.buffType[i]]) {
					modplayer.AddStatsToPlayer(stat, value);
					break;
				}
			}
		}
	}
}
