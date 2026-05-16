using Terraria.ID;

namespace Roguelike.Contents.Transfixion.Skill;
public class Skill_Arrow : ModSkill {
	public override void SetDefault() {
		Skill_Duration = 60;
		Skill_EnergyRequire = 10;
		Skill_CoolDown = 20;
		Skill_Type = SkillTypeID.Projectile;

		Skill_ShootType = ProjectileID.WoodenArrowFriendly;
		Skill_Damage = 30;
		Skill_KnockBack = 2;
	}
}
public class Skill_Bullet : ModSkill {
	public override void SetDefault() {
		Skill_Duration = 60;
		Skill_EnergyRequire = 10;
		Skill_CoolDown = 20;
		Skill_Type = SkillTypeID.Projectile;

		Skill_ShootType = ProjectileID.Bullet;
		Skill_Damage = 60;
		Skill_KnockBack = 2;
	}

}
