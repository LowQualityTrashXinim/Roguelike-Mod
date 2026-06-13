using Terraria.ID;

namespace Roguelike.Contents.Transfixion.Skill;
public class Skill_Arrow : ModSkill {
	public override void SetDefault() {
		Skill_Duration = 60;
		Skill_EnergyRequire = 3;
		Skill_CoolDown = 20;
		Skill_Type = SkillTypeID.Projectile;

		Skill_ShootType = ProjectileID.WoodenArrowFriendly;
		Skill_Damage = 8;
		Skill_KnockBack = 1;
	}
}
public class Skill_Bullet : ModSkill {
	public override void SetDefault() {
		Skill_Duration = 60;
		Skill_EnergyRequire = 12;
		Skill_CoolDown = 20;
		Skill_Type = SkillTypeID.Projectile;

		Skill_ShootType = ProjectileID.Bullet;
		Skill_Damage = 12;
		Skill_KnockBack = 2;
	}
}
public class Skill_Dart : ModSkill {
	public override void SetDefault() {
		Skill_Duration = 60;
		Skill_EnergyRequire = 8;
		Skill_CoolDown = 15;
		Skill_Type = SkillTypeID.Projectile;

		Skill_ShootType = ProjectileID.PoisonDartBlowgun;
		Skill_Damage = 9;
		Skill_KnockBack = 1.5f;
	}
}
public class Skill_Flamelash : ModSkill {
	public override void SetDefault() {
		Skill_Duration = 60;
		Skill_EnergyRequire = 30;
		Skill_CoolDown = 60;
		Skill_Type = SkillTypeID.Projectile;

		Skill_ShootType = ProjectileID.Flamelash;
		Skill_Damage = 15;
		Skill_KnockBack = 4.5f;
	}
}
public class Skill_Waterbolt : ModSkill {
	public override void SetDefault() {
		Skill_Duration = 60;
		Skill_EnergyRequire = 22;
		Skill_CoolDown = 44;
		Skill_Type = SkillTypeID.Projectile;

		Skill_ShootType = ProjectileID.WaterBolt;
		Skill_Damage = 12;
		Skill_KnockBack = 4.5f;
	}
}
public class Skill_StarWrath : ModSkill {
	public override void SetDefault() {
		Skill_Duration = 60;
		Skill_EnergyRequire = 88;
		Skill_CoolDown = 90;
		Skill_Type = SkillTypeID.Projectile;

		Skill_ShootType = ProjectileID.StarWrath;
		Skill_Damage = 34;
		Skill_KnockBack = 8.5f;
	}
}
