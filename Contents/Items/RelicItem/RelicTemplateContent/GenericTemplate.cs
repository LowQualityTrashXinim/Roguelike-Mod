using System;
using Terraria;
using Terraria.ModLoader;
using Roguelike.Common.Utils;
using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Contents.Perks;
using Roguelike.Contents.Perks.BlessingPerk;

namespace Roguelike.Contents.Items.RelicItem.RelicTemplateContent;
/// <summary>
/// This will be serves as the generalize base stats for across relic, use this template as reference
/// </summary>
public class GenericTemplate : RelicTemplate {
	public override void SetStaticDefaults() {
		relicType = RelicType.Stat;
		RelicTierUPValue = .75f;
	}
	public override PlayerStats StatCondition(Relic relic, Player player) {
		var perkplayer = player.GetModPlayer<PerkPlayer>();
		if (perkplayer.HasPerk<BlessingOfSolar>()) {
			if (Main.rand.NextFloat() <= .35f) {
				return PlayerStats.MeleeDMG;
			}
		}
		else if (perkplayer.HasPerk<BlessingOfVortex>()) {
			if (Main.rand.NextFloat() <= .35f) {
				return PlayerStats.RangeDMG;
			}
		}
		else if (perkplayer.HasPerk<BlessingOfNebula>()) {
			if (Main.rand.NextFloat() <= .35f) {
				return Main.rand.Next([
					PlayerStats.MagicDMG,
					PlayerStats.MaxMana,
				]);
			}
		}
		else if (perkplayer.HasPerk<BlessingOfStardust>()) {
			if (Main.rand.NextFloat() <= .35f) {
				return PlayerStats.SummonDMG;
			}
			else if (Main.rand.NextFloat() <= .25f) {
				return Main.rand.Next([
					PlayerStats.MaxMinion,
					PlayerStats.MaxSentry,
				]);
			}
		}
		else if (perkplayer.HasPerk<BlessingOfTitan>()) {
			if (Main.rand.NextFloat() <= .35f) {
				return Main.rand.Next([
					PlayerStats.Thorn,
					PlayerStats.Defense,
					PlayerStats.MaxHP,
				]);
			}
		}
		else if (perkplayer.HasPerk<BlessingOfSynergy>()) {
			if (Main.rand.NextFloat() <= .5f) {
				return PlayerStats.SynergyDamage;
			}
		}
		return Main.rand.Next([
			PlayerStats.MeleeDMG,
			PlayerStats.RangeDMG,
			PlayerStats.MagicDMG,
			PlayerStats.SummonDMG,

			PlayerStats.RegenHP,
			PlayerStats.RegenMana,
			PlayerStats.MaxHP,
			PlayerStats.MaxMana,
			PlayerStats.Defense,

			PlayerStats.MaxMinion,
			PlayerStats.MaxSentry,

			PlayerStats.EnergyCap,

			PlayerStats.MovementSpeed,
			PlayerStats.JumpBoost,

			PlayerStats.Thorn,

			PlayerStats.DebuffDamage,
			PlayerStats.FullHPDamage,
			PlayerStats.SynergyDamage,
		]);
	}
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		string valuestring;
		if (stat == PlayerStats.Defense
			|| stat == PlayerStats.MaxMana
			|| stat == PlayerStats.MaxHP
			|| stat == PlayerStats.RegenMana
			|| stat == PlayerStats.RegenHP
			|| stat == PlayerStats.MaxMinion
			|| stat == PlayerStats.MaxSentry
			|| stat == PlayerStats.EnergyCap
			|| stat == PlayerStats.Thorn
			) {
			valuestring = RelicTemplateLoader.RelicValueToNumber(value.Base);
		}
		else {
			valuestring = RelicTemplateLoader.RelicValueToPercentage(value.Additive);
		}
		return string.Format(Description, [Color.Yellow.Hex3(), Name, valuestring,]);
	}
	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		if (stat == PlayerStats.JumpBoost || stat == PlayerStats.MovementSpeed) {
			return new StatModifier(MathF.Round(Main.rand.NextFloat(1.06f, 1.1f), 2), 1);
		}
		if (stat == PlayerStats.MaxMinion || stat == PlayerStats.MaxSentry) {
			return new StatModifier(1, 1, 0, 1);
		}
		if (stat == PlayerStats.RegenHP || stat == PlayerStats.RegenMana) {
			return new StatModifier(1, 1, 0, Main.rand.Next(1, 5));
		}
		if (stat == PlayerStats.MaxHP || stat == PlayerStats.MaxMana) {
			return new StatModifier(1, 1, 0, Main.rand.Next(1, 3) * 10);
		}
		if (stat == PlayerStats.Defense) {
			return new StatModifier(1, 1, 0, Main.rand.Next(1, 5));
		}
		if (stat == PlayerStats.MeleeDMG
			|| stat == PlayerStats.RangeDMG
			|| stat == PlayerStats.MagicDMG
			|| stat == PlayerStats.SummonDMG) {
			return new StatModifier(MathF.Round(Main.rand.NextFloat(1.03f, 1.9f), 2), 1);
		}
		if (stat == PlayerStats.EnergyCap) {
			return new StatModifier(1, 1, 0, MathF.Round(Main.rand.NextFloat(5, 6) * 20));
		}
		if (stat == PlayerStats.Thorn) {
			return new StatModifier(1, 1, 0, Main.rand.Next(10, 20));
		}
		if (stat == PlayerStats.DebuffDamage) {
			return new StatModifier(MathF.Round(Main.rand.NextFloat(1.06f, 1.09f), 2), 1, 0, 0);
		}
		if (stat == PlayerStats.FullHPDamage) {
			return new StatModifier(MathF.Round(Main.rand.NextFloat(1.15f, 1.25f), 2), 1, 0, 0);
		}
		if (stat == PlayerStats.SynergyDamage) {
			return new StatModifier(MathF.Round(Main.rand.NextFloat(1.05f, 1.07f), 2), 1, 0, 0);
		}
		return new StatModifier(MathF.Round(Main.rand.NextFloat(1.1f, 1.25f), 2), 1);
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		modplayer.AddStatsToPlayer(stat, value);
	}
}
