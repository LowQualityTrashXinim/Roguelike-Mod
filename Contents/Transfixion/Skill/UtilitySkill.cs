using Microsoft.Xna.Framework;
using Roguelike.Common.Systems;
using Roguelike.Common.Systems.Skill;
using Roguelike.Common.Utils;
using Roguelike.Contents.Transfixion.Perks;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Transfixion.Skill;
public class Utility_SummonPerk : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 100000;
		Skill_Duration = 1;
		Skill_CanBeSelect = false;
		Skill_Type = SkillTypeID.Utility;
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
		Skill_Type = SkillTypeID.Utility;
	}
	public override void OnTrigger(Player player, SkillHandlePlayer skillplayer, int duration, int energy) {
		ModUtils.GetWeaponSpoil(player.GetSource_Misc("Spoil"), 1);
	}
}
public class Utility_CopyRandomSkill : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 100;
		Skill_Duration = 1;
		Skill_Type = SkillTypeID.Utility;
	}
	public override bool OnAddSkill(Player player, SkillHandlePlayer skillplayer, int[] currentSkills, ref List<ModSkill> activeskill, ref int currentindex, ref int energy, ref int duration) {
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
		Skill_Type = SkillTypeID.Utility;
	}
	public override bool OnAddSkill(Player player, SkillHandlePlayer skillplayer, int[] currentSkill, ref List<ModSkill> activeskill, ref int currentindex, ref int energy, ref int duration) {
		duration = 0;
		return false;
	}
}
public class Utility_SwapDurationWithEnergy : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 0;
		Skill_Duration = 0;
		Skill_Type = SkillTypeID.Utility;
	}
	public override bool OnAddSkill(Player player, SkillHandlePlayer skillplayer, int[] currentSkill, ref List<ModSkill> activeskill, ref int currentindex, ref int energy, ref int duration) {
		int cacheE = energy;
		int cacheD = duration;
		duration = cacheE;
		energy = cacheD;
		return true;
	}
}
public class Utility_Replay : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 100;
		Skill_Duration = 0;
		Skill_Type = SkillTypeID.Utility;
	}
	public override bool OnAddSkill(Player player, SkillHandlePlayer skillplayer, int[] currentSkill, ref List<ModSkill> activeskill, ref int currentindex, ref int energy, ref int duration) {
		StatModifier energyS = new(), durationS = new();
		float percentageEnergy = 1;
		for (int i = 0; i < currentindex; i++) {
			ModSkill skill = SkillModSystem.GetSkill(currentSkill[i]);
			energy += (int)energyS.ApplyTo(skill.EnergyRequire * 2);
			percentageEnergy *= skill.EnergyPercentage;
			skill.ModifyNextSkillStats(out energyS, out durationS);
			skill.ModifySkillSet(player, skillplayer, ref i, ref energyS, ref durationS);
			if (skill.OnAddSkill(player, skillplayer, currentSkill, ref activeskill, ref i, ref energy, ref duration)) {
				activeskill.Add(skill);
			}
		}
		energy = (int)(energy * percentageEnergy);
		return false;
	}
}
public class Utility_Rewind : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 100;
		Skill_Duration = 0;
		Skill_Type = SkillTypeID.Utility;
	}
	public override bool OnAddSkill(Player player, SkillHandlePlayer skillplayer, int[] currentSkill, ref List<ModSkill> activeskill, ref int currentindex, ref int energy, ref int duration) {
		StatModifier energyS = new(), durationS = new();
		float percentageEnergy = 1;
		for (int i = currentindex - 1; i >= 0; i--) {
			ModSkill skill = SkillModSystem.GetSkill(currentSkill[i]);
			energy += (int)energyS.ApplyTo(skill.EnergyRequire);
			percentageEnergy *= skill.EnergyPercentage;
			skill.ModifyNextSkillStats(out energyS, out durationS);
			skill.ModifySkillSet(player, skillplayer, ref i, ref energyS, ref durationS);
			if (skill.OnAddSkill(player, skillplayer, currentSkill, ref activeskill, ref i, ref energy, ref duration)) {
				activeskill.Add(skill);
			}
		}
		energy = (int)(energy * percentageEnergy);
		currentindex = currentSkill.Length - 1;
		return false;
	}
}
public class Utility_Last : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 0;
		Skill_Duration = 0;
		Skill_Type = SkillTypeID.Utility;
	}
	public override bool OnAddSkill(Player player, SkillHandlePlayer skillplayer, int[] currentSkill, ref List<ModSkill> activeskill, ref int currentindex, ref int energy, ref int duration) {
		int index = currentSkill.Length - 1;
		if(index < 0 || index == currentindex) {
			return false;
		}
		int cache = currentSkill[currentindex];
		currentSkill[currentindex] = currentSkill[index];
		currentSkill[index] = cache;
		currentindex--;
		return false;
	}
}
