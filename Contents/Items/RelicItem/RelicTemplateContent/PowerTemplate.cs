using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Contents.Transfixion.Skill;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.RelicItem.RelicTemplateContent;
internal class PowerTemplate : RelicTemplate {
	public override void SetStaticDefaults() {
		relicType = RelicType.Stat;
		RelicTierUPValue = .1f;
	}
	public override PlayerStats StatCondition(Relic relic, Player player) {
		return Main.rand.Next([
			PlayerStats.MeleeDMG,
			PlayerStats.RangeDMG,
			PlayerStats.MagicDMG,
			PlayerStats.SummonDMG,
		]);
	}
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		string Number = RelicTemplateLoader.RelicValueToPercentage(value.Additive);
		return string.Format(Description, [Color.Yellow.Hex3(), Name, Number]);
	}

	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		return new StatModifier(1 + MathF.Round(Main.rand.NextFloat(.2f, .25f), 2), 1, 0, 0);
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		modplayer.EnergyRegen.Base -= 1;
		if (player.GetModPlayer<SkillHandlePlayer>().Energy > 0) {
			modplayer.AddStatsToPlayer(stat, value.Additive * 1.16f, value.Multiplicative, value.Flat * 1.16f, value.Base * 1.16f);
		}
		else {
			modplayer.AddStatsToPlayer(stat, value);
		}
	}
}
