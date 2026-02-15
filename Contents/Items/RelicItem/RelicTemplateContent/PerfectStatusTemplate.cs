using System;
using Terraria;
using Terraria.ModLoader;
using Roguelike.Common.Utils;
using Microsoft.Xna.Framework;
using Roguelike.Common.Global;

namespace Roguelike.Contents.Items.RelicItem.RelicTemplateContent;

public class PerfectStatusTemplate : RelicTemplate {
	public override void SetStaticDefaults() {
		relicType = RelicType.Stat;
		RelicTierUPValue = .1f;
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
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		string Name = Enum.GetName(stat) ?? string.Empty;
		string valuestring;
		if (stat == PlayerStats.CritChance
			|| stat == PlayerStats.Defense) {
			valuestring = RelicTemplateLoader.RelicValueToNumber(value.Base);
		}
		else {
			valuestring = RelicTemplateLoader.RelicValueToPercentage(value.Additive);
		}
		return string.Format(Description, [Color.Yellow.Hex3(), Name, valuestring]);
	}
	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		if (stat == PlayerStats.CritChance) {
			return new StatModifier(1, 1, 0, Main.rand.Next(15, 21));
		}
		if (stat == PlayerStats.Defense) {
			return new StatModifier(1, 1, 0, Main.rand.Next(20, 26));
		}
		return new StatModifier(MathF.Round(Main.rand.NextFloat(1.25f, 1.3f), 2), 1);
	}
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		for (int i = 0; i < player.buffType.Length; i++) {
			int type = player.buffType[i];
			if (type == 0) {
				continue;
			}
			//Attempt to avoid minion and pet buff
			if (!Main.buffNoTimeDisplay[type] || !Main.lightPet[type] && !Main.vanityPet[type]) {
				continue;
			}
			modplayer.AddStatsToPlayer(stat, value);
			return;
		}
		modplayer.AddStatsToPlayer(stat, value.Additive * 1.4f, value.Multiplicative, value.Flat * 1.4f, value.Base * 1.4f);
	}
}
