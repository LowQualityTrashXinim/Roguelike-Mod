using Roguelike.Common.Global;
using Roguelike.Common.Systems.Skill;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Transfixion.Skill;
public class DamageUp : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 50;
		Skill_Duration = 0;
		Skill_Type = SkillTypeID.Stats;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		skillplayer.skilldamage += 1f;
	}
}
public class GreaterDamageUp : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 350;
		Skill_Duration = 0;
		Skill_Type = SkillTypeID.Stats;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		skillplayer.skilldamage += 3f;
	}
}
public class CriticalUp : ModSkill {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<CriticalUp>();
	public override void SetDefault() {
		Skill_EnergyRequire = 350;
		Skill_Duration = 0;
		Skill_Type = SkillTypeID.Stats;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		skillplayer.ProjectileCritDamage += .25f;
		skillplayer.ProjectileCritChance += 15;
	}
}
public class PowerBank : ModSkill {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<PowerBank>();
	public override void SetDefault() {
		Skill_EnergyRequire = 0;
		Skill_Duration = 0;
		Skill_Type = SkillTypeID.Stats;
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
		Skill_Duration = 0;
		Energy.Flat = 900;
		Energy *= .5f;
		Skill_Type = SkillTypeID.Stats;
	}
}
public class FastForward : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 200;
		Skill_Duration = 0;
		Skill_CoolDown = ModUtils.ToSecond(20);
		Skill_Type = SkillTypeID.Stats;
	}
	public override void ModifyNextSkillStats(out StatModifier energy, out StatModifier duration) {
		energy = new();
		duration = new();
		duration -= .5f;
		energy -= .5f;
	}
}
public class PowerCord : ModSkill {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<PowerCord>();
	public override void SetDefault() {
		Skill_EnergyRequire = 100;
		Energy *= .25f;
		Skill_Duration = 0;
		Skill_Type = SkillTypeID.Stats;
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
		Skill_Type = SkillTypeID.Stats;
	}
}
public class Weaken : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = -100;
		Skill_Duration = ModUtils.ToSecond(3);
		Skill_Type = SkillTypeID.Stats;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		skillplayer.skilldamage -= .2f;
	}
}
public class EnergyToPower : ModSkill {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<EnergyToPower>();
	public override void SetDefault() {
		Skill_EnergyRequire = 500;
		Skill_Duration = 0;
		Skill_Type = SkillTypeID.Stats;
	}
	public override void OnTrigger(Player player, SkillHandlePlayer skillplayer, int duration, int energy) {
		skillplayer.ToPower += skillplayer.Energy;
		skillplayer.Modify_EnergyAmount(-99999999);
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.PureDamage, Additive: 1 + player.GetModPlayer<SkillHandlePlayer>().ToPower * .01f);
	}
}
