using System;
using Terraria;
using Terraria.ModLoader;
using Roguelike.Common.Utils;
using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Contents.Transfixion.Skill;

namespace Roguelike.Contents.Items.RelicItem.RelicTemplateContent {
	public class SkillActivationTemplate : RelicTemplate {
		public override void SetStaticDefaults() {
			relicType = RelicType.Stat;
			RelicTierUPValue = .44f;
		}
		public override PlayerStats StatCondition(Relic relic, Player player) {
			return Main.rand.Next([
			PlayerStats.PureDamage,
			PlayerStats.CritChance,
			PlayerStats.CritDamage,
			PlayerStats.AttackSpeed,
			PlayerStats.Defense,
		]);
		}
		public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
			string Name = Enum.GetName(stat) ?? string.Empty;
			string Number = stat == PlayerStats.CritChance ? RelicTemplateLoader.RelicValueToNumber(value.Base) : RelicTemplateLoader.RelicValueToPercentage(value.Additive);
			return string.Format(Description, [Color.Yellow.Hex3(), Name, Number]);
		}

		public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
			if (stat == PlayerStats.PureDamage) {
				return new StatModifier(1 + MathF.Round(Main.rand.NextFloat(.05f, .11f), 2), 1, 0, 0);
			}
			if (stat == PlayerStats.CritChance) {
				return new StatModifier(1, 1, 0, Main.rand.Next(5, 11));
			}
			if (stat == PlayerStats.CritDamage) {
				return new StatModifier(1 + MathF.Round(Main.rand.NextFloat(.1f, .2f), 2), 1, 0, 0);
			}
			if (stat == PlayerStats.AttackSpeed) {
				return new StatModifier(1 + MathF.Round(Main.rand.NextFloat(.05f, .11f), 2), 1, 0, 0);
			}
			return new StatModifier(1 + MathF.Round(Main.rand.NextFloat(.1f, .15f), 2), 1, 0, 0);
		}
		public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
			SkillHandlePlayer skillPlayer = player.GetModPlayer<SkillHandlePlayer>();
			if (skillPlayer.Activate) {
				modplayer.AddStatsToPlayer(stat, value.Additive * 1.34f, value.Multiplicative, value.Flat * 1.34f, value.Base * 1.34f);
			}
			else {
				modplayer.AddStatsToPlayer(stat, value);
			}
		}
	}
}
