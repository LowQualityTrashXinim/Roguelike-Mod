using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Skill;
public class DamageUp : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 50;
		Skill_Duration = 0;
		Skill_Type = SkillTypeID.Skill_Stats;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		skillplayer.SkillDamageWhileActive += 1f;
	}
}
public class GreaterDamageUp : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 350;
		Skill_Duration = 0;
		Skill_Type = SkillTypeID.Skill_Stats;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		skillplayer.SkillDamageWhileActive += 3f;
	}
}
public class PowerBank : ModSkill {
	public override string Texture => ModUtils.GetTheSameTextureAs<PowerBank>();
	public override void SetDefault() {
		Skill_EnergyRequire = 0;
		Skill_Duration = 0;
		Skill_Type = SkillTypeID.Skill_Stats;
	}
	public override void ModifyNextSkillStats(out StatModifier energy, out StatModifier duration) {
		energy = StatModifier.Default;
		energy.Base -= 300;
		duration = StatModifier.Default;
		duration -= 1;
	}
}
public class PowerSaver : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 300;
		Skill_Duration = 0;
		Skill_EnergyRequirePercentage = -.5f;
		Skill_Type = SkillTypeID.Skill_Stats;
	}
}
public class FastForward : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 200;
		Skill_Duration = 0;
		Skill_CoolDown = ModUtils.ToSecond(20);
		Skill_Type = SkillTypeID.Skill_Stats;
	}
	public override void ModifyNextSkillStats(out StatModifier energy, out StatModifier duration) {
		energy = new();
		duration = new();
		duration -= .5f;
		energy -= .5f;
	}
}
public class Skip1 : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 0;
		Skill_Duration = 0;
		Skill_Type = SkillTypeID.Skill_Stats;
	}
	public override void ModifySkillSet(Player player, SkillHandlePlayer modplayer, ref int index, ref StatModifier energy, ref StatModifier duration) {
		int[] currentskillset = modplayer.GetCurrentActiveSkillHolder();
		for (int i = index + 1; i < currentskillset.Length; i++) {
			ModSkill skill = SkillModSystem.GetSkill(currentskillset[i]);
			if (skill == null) {
				continue;
			}
			index = index + 1;
			energy.Base -= 600;
			break;
		}
	}
}

public class PowerCord : ModSkill {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<PowerCord>();
	public override void SetDefault() {
		Skill_EnergyRequire = 100;
		Skill_EnergyRequirePercentage = .25f;
		Skill_Duration = 0;
		Skill_Type = SkillTypeID.Skill_Stats;
	}
	public override void OnEnded(Player player) {
		player.AddBuff(ModContent.BuffType<PowerCordBuff>(), ModUtils.ToSecond(12));
	}
	public class PowerCordBuff : ModBuff {
		public override string Texture => ModTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			Main.debuff[Type] = false;
		}
		public override void Update(Player player, ref int buffIndex) {
			player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.EnergyRecharge, 2.5f);
		}
	}
}
public class TranquilMind : ModSkill {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<TranquilMind>();
	public override void SetDefault() {
		Skill_EnergyRequire = 550;
		Skill_Duration = ModUtils.ToSecond(5);
		Skill_Type = SkillTypeID.Skill_Stats;
	}
}
