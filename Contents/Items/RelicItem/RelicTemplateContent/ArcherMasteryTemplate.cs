using System;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;

namespace Roguelike.Contents.Items.RelicItem.RelicTemplateContent
{
	public class ArcherMasteryTemplate : RelicTemplate {
		public override void SetStaticDefaults() {
			relicType = RelicType.MultiStats;
			RelicTierUPValue = .17f;
		}
		public override PlayerStats StatCondition(Relic relic, Player player) => PlayerStats.RangeDMG;
		public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
			return string.Format(Description, [
					Color.Yellow.Hex3(),
			RelicTemplateLoader.RelicValueToPercentage(value.Additive),
			RelicTemplateLoader.RelicValueToNumber(value.Base),
			RelicTemplateLoader.RelicValueToPercentage(value.Additive * 2),
		]);
		}

		public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
			return new StatModifier(1 + MathF.Round(Main.rand.NextFloat(.05f, .1f), 2), 1, 0, Main.rand.Next(3, 5));
		}
		public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
			if (player.HeldItem.useAmmo == AmmoID.Arrow) {
				modplayer.AddStatsToPlayer(stat, value);
				modplayer.AddStatsToPlayer(PlayerStats.CritChance, Base: value.Base);
				modplayer.AddStatsToPlayer(PlayerStats.CritDamage, value.Additive * 2);
			}
		}
	}
}
