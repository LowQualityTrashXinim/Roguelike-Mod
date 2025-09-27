using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;

namespace Roguelike.Contents.Items.RelicItem;

public class SynergyTemplate : RelicTemplate {
	public override void SetStaticDefaults() {
		relicType = RelicType.Stat;
		RelicTierUPValue = 1.22f;
	}
	public override PlayerStats StatCondition(Relic relic, Player player) {
		return Main.rand.Next([
			PlayerStats.PureDamage,
			PlayerStats.CritChance,
			PlayerStats.CritDamage,
			PlayerStats.Defense,
			PlayerStats.AttackSpeed
			]);
	}
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) =>
		string.Format(Description, [
			Color.Yellow.Hex3(),
			RelicTemplateLoader.RelicValueToNumber(value.Flat),
			new Color(100, 255, 255).Hex3(),
			Enum.GetName(stat),
			RelicTemplateLoader.RelicValueToPercentage(value.Additive),
		]);

	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		return new StatModifier(1 + MathF.Round(Main.rand.NextFloat(.04f, .06f), 2), 1, Main.rand.Next(3, 11), 0);
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		modplayer.AddStatsToPlayer(PlayerStats.SynergyDamage, Flat: value.Flat);
		modplayer.AddStatsToPlayer(stat, Additive: value.Additive);
	}
}
public class StrikeFullHPTemplate : RelicTemplate {
	public override void SetStaticDefaults() {
		relicType = RelicType.Stat;
		RelicTierUPValue = 1.5f;
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
public class SkillDurationTemplate : RelicTemplate {
	public override void SetStaticDefaults() {
		relicType = RelicType.Stat;
		RelicTierUPValue = 1;
	}
	public override PlayerStats StatCondition(Relic relic, Player player) => PlayerStats.SkillDuration;
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		return string.Format(Description, [
			Color.Yellow.Hex3(),
			Name,
			RelicTemplateLoader.RelicValueToNumber(value.Base / 60 + relic.RelicTier - 1)
	]);
	}

	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		return new StatModifier(1, 1, 0, ModUtils.ToSecond(Main.rand.Next(1, 4)));
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		modplayer.AddStatsToPlayer(stat, Base: value.Base + ModUtils.ToSecond(relic.RelicTier - 1));
	}
}
