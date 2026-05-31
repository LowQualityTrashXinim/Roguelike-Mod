using Microsoft.Xna.Framework;
using Roguelike.Common.Systems;
using Roguelike.Common.Utils;
using Roguelike.Contents.Transfixion.Perks;
using System.Collections.Generic;
using Terraria;

namespace Roguelike.Contents.Transfixion.Skill;
public class Utility_SummonPerk : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 100000;
		Skill_Duration = 1;
		Skill_CanBeSelect = false;
	}
	public override void OnTrigger(Player player, SkillHandlePlayer skillplayer, int duration, int energy) {
		var modplayer = Main.LocalPlayer.GetModPlayer<PerkPlayer>();
		var listOfPerk = new List<int>();
		for (int i = 0; i < ModPerkLoader.TotalCount; i++) {
			if (modplayer.perks.ContainsKey(i)) {
				if (!ModPerkLoader.GetPerk(i).CanBeStack && modplayer.perks[i] > 0
					|| modplayer.perks[i] >= ModPerkLoader.GetPerk(i).StackLimit) {
					continue;
				}
			}
			if (!ModPerkLoader.GetPerk(i).SelectChoosing()) {
				continue;
			}
			if (!ModPerkLoader.GetPerk(i).CanBeChoosen) {
				continue;
			}
			listOfPerk.Add(i);
		}
		int perkType = Main.rand.Next(listOfPerk);
		UniversalSystem.AddPerk(perkType);
		ModUtils.CombatTextRevamp(Main.LocalPlayer.Hitbox, Color.AliceBlue, ModPerkLoader.GetPerk(perkType).DisplayName);
	}
}
public class Utility_SummonWeapon : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 50000;
		Skill_Duration = 1;
		Skill_CanBeSelect = false;
	}
	public override void OnTrigger(Player player, SkillHandlePlayer skillplayer, int duration, int energy) {
		ModUtils.GetWeaponSpoil(player.GetSource_Misc("Spoil"), 1);
	}
}
