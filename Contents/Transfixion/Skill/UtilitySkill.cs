using Microsoft.Xna.Framework;
using Roguelike.Common.Systems;
using Roguelike.Common.Utils;
using Roguelike.Contents.Transfixion.Perks;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

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
public class Utility_CopyRandomSkill : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 100;
		Skill_Duration = 1;
	}
	public override bool OnAddSkill(Player player, SkillHandlePlayer skillplayer, int[] currentSkills, ref List<ModSkill> activeskill, int currentindex, ref int energy, ref int duration) {
		if (currentindex > currentSkills.Length - 1) {
			return false;
		}
		ModSkill skill = null;
		List<ModSkill> skilllist = new();
		for (int i = currentindex + 1; i < currentSkills.Length; i++) {
			skill = SkillModSystem.GetSkill(currentSkills[i]);
			if (skill == null) {
				continue;
			}
			skilllist.Add(skill);
		}
		if (skilllist.Count < 1) {
			return false;
		}
		skill = Main.rand.Next(skilllist);
		activeskill.Add(skill);
		return false;
	}
}
public class Utility_SetDurationToNone : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 0;
		Skill_Duration = 0;
	}
	public override void ModifyNextSkillStats(out StatModifier energy, out StatModifier duration) {
		energy = new();
		duration = new();
		duration *= 0;
	}
}
public class Utility_SwapDurationWithEnergy : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 0;
		Skill_Duration = 0;
	}
	public override bool OnAddSkill(Player player, SkillHandlePlayer skillplayer, int[] currentSkill, ref List<ModSkill> activeskill, int currentindex, ref int energy, ref int duration) {
		int cacheE = energy;
		int cacheD = duration;
		duration = cacheE;
		energy = cacheD;
		return true;
	}
}
