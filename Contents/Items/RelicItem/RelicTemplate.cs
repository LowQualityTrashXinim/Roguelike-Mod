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
			RelicTemplateLoader.RelicValueToNumber(value.Flat + value.Flat * .12f * (relic.RelicTier - 1)),
			new Color(100, 255, 255).Hex3(),
			Enum.GetName(stat),
			RelicTemplateLoader.RelicValueToPercentage(value.Additive + value.Additive * .12f  * (relic.RelicTier - 1)),
		]);

	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		return new StatModifier(1 + MathF.Round(Main.rand.NextFloat(.04f, .06f), 2), 1, Main.rand.Next(3, 11), 0);
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		float tierValue = .12f * (relic.RelicTier - 1);
		value.Flat += value.Flat * tierValue;
		value += value.Additive * tierValue;
		modplayer.AddStatsToPlayer(PlayerStats.SynergyDamage, Flat: value.Flat);
		modplayer.AddStatsToPlayer(stat, Additive: value.Additive);
	}
}
public class StrikeFullHPTemplate : RelicTemplate {
	public override void SetStaticDefaults() {
		relicType = RelicType.Stat;
	}
	public override PlayerStats StatCondition(Relic relic, Player player) => PlayerStats.FullHPDamage;
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		return string.Format(Description, [Color.Yellow.Hex3(), RelicTemplateLoader.RelicValueToPercentage(value.Additive + (value.Additive - 1) * (.5f * (relic.RelicTier - 1))),]);
	}
	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		return new StatModifier(MathF.Round(Main.rand.NextFloat(.75f, 1f) + 1, 2), 1, 0, 0);
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		modplayer.AddStatsToPlayer(stat, value, .5f * (relic.RelicTier - 1));
	}
}
public class SkillDurationTemplate : RelicTemplate {
	public override void SetStaticDefaults() {
		relicType = RelicType.Stat;
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
public class CombatV4Template : RelicTemplate {
	public override void SetStaticDefaults() {
		relicType = RelicType.Stat;
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
		return string.Format(Description, [Color.Yellow.Hex3(), Name, RelicTemplateLoader.RelicValueToNumber(value.Base + relic.RelicTier - 1),]);
	}
	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		var value = new StatModifier();
		value.Base += Main.rand.Next(1, 4);
		return value;
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		value.Base += relic.RelicTier - 1;
		modplayer.AddStatsToPlayer(stat, value);
	}
}
