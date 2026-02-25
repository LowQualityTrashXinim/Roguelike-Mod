using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Transfixion.Perks.PerkContents;
internal class Perfectionist : Perk {
	public override void SetDefaults() {
		CanBeStack = false;
	}
	public override void UpdateEquip(Player player) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		if (player.statLife == player.statLifeMax2) {
			handler.UpdateCritDamage += 1;
			player.GetDamage(DamageClass.Generic) += .5f;
			player.GetCritChance(DamageClass.Generic) += 25;
		}
		if (player.statMana == player.statManaMax2) {
			handler.UpdateDefenseBase.Base += 20;
			handler.EnergyRegen.Base += 10;
			player.endurance += .05f;
		}
	}
}
