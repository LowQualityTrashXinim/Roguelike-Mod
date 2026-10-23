using Microsoft.Xna.Framework;
using Roguelike.Common.Systems.Skill;
using Roguelike.Common.Utils;
using System.Collections.Generic;
using Terraria;

namespace Roguelike.Contents.Transfixion.Skill;
public class Modify_DuplicateProjectile : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 100;
		Skill_Duration = 60;
		Skill_Type = SkillTypeID.Modify;
	}
	public override void ModifyShootProjectile(Player player, SkillHandlePlayer skillplayer, ref int amount, ref List<Vector2> position, ref List<Vector2> velocity) {
		amount += 2;
		while (position.Count < amount) {
			position.Add(player.Center);
		}
		while (velocity.Count < amount) {
			velocity.Add(velocity[0].Vector2RotateByRandom(10));
		}
	}
}
public class Modify_QuadShooting : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 200;
		Skill_Duration = 60;
		Skill_Type = SkillTypeID.Modify;
	}
	public override void ModifyShootProjectile(Player player, SkillHandlePlayer skillplayer, ref int amount, ref List<Vector2> position, ref List<Vector2> velocity) {
		amount += 3;
		while (position.Count < amount) {
			position.Add(player.Center);
		}
		while (velocity.Count < amount) {
			velocity.Add(velocity[0].Vector2RotateByRandom(10));
		}
		velocity[velocity.Count - 1] = Vector2.UnitX * 5;
		velocity[velocity.Count - 2] = Vector2.UnitX * -5;
		velocity[velocity.Count - 3] = Vector2.UnitY * 5;
		velocity[velocity.Count - 4] = Vector2.UnitY * -5;
	}
}
public class Modify_DistanceCast : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 100;
		Skill_Duration = 60;
		Skill_Type = SkillTypeID.Modify;
	}
	public override void ModifyShootProjectile(Player player, SkillHandlePlayer skillplayer, ref int amount, ref List<Vector2> position, ref List<Vector2> velocity) {
		for (int i = 0; i < position.Count; i++) {
			position[i] = position[i].IgnoreTilePositionOFFSET(Main.MouseWorld - player.Center, 100);
		}
	}
}
public class Modify_CastOffSetUp : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 50;
		Skill_Duration = 60;
		Skill_Type = SkillTypeID.Modify;
	}
	public override void ModifyShootProjectile(Player player, SkillHandlePlayer skillplayer, ref int amount, ref List<Vector2> position, ref List<Vector2> velocity) {
		for (int i = 0; i < position.Count; i++) {
			position[i] = position[i].Add(0, 100);
		}
	}
}
public class Modify_CastOffSetLeft : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 50;
		Skill_Duration = 60;
		Skill_Type = SkillTypeID.Modify;
	}
	public override void ModifyShootProjectile(Player player, SkillHandlePlayer skillplayer, ref int amount, ref List<Vector2> position, ref List<Vector2> velocity) {
		for (int i = 0; i < position.Count; i++) {
			position[i] = position[i].Add(-100, 0);
		}
	}
}

public class Modify_CastOffSetRight : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 50;
		Skill_Duration = 60;
		Skill_Type = SkillTypeID.Modify;
	}
	public override void ModifyShootProjectile(Player player, SkillHandlePlayer skillplayer, ref int amount, ref List<Vector2> position, ref List<Vector2> velocity) {
		for (int i = 0; i < position.Count; i++) {
			position[i] = position[i].Add(100, 0);
		}
	}
}

public class Modify_CastOffSetDown : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 50;
		Skill_Duration = 60;
		Skill_Type = SkillTypeID.Modify;
	}
	public override void ModifyShootProjectile(Player player, SkillHandlePlayer skillplayer, ref int amount, ref List<Vector2> position, ref List<Vector2> velocity) {
		for (int i = 0; i < position.Count; i++) {
			position[i] = position[i].Add(0, -100);
		}
	}
}
public class Modify_CastUp : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 50;
		Skill_Duration = 60;
		Skill_Type = SkillTypeID.Modify;
	}
	public override void ModifyShootProjectile(Player player, SkillHandlePlayer skillplayer, ref int amount, ref List<Vector2> position, ref List<Vector2> velocity) {
		for (int i = 0; i < position.Count; i++) {
			Vector2 vel = velocity[i];
			vel.Y = -1 * velocity[i].Length();
			velocity[i] = vel;
		}
	}
}
public class Modify_CastDown : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 50;
		Skill_Duration = 60;
		Skill_Type = SkillTypeID.Modify;
	}
	public override void ModifyShootProjectile(Player player, SkillHandlePlayer skillplayer, ref int amount, ref List<Vector2> position, ref List<Vector2> velocity) {
		for (int i = 0; i < position.Count; i++) {
			Vector2 vel = velocity[i];
			vel.Y = 1 * velocity[i].Length();
			velocity[i] = vel;
		}
	}
}
public class Modify_CastLeft : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 50;
		Skill_Duration = 60;
		Skill_Type = SkillTypeID.Modify;
	}
	public override void ModifyShootProjectile(Player player, SkillHandlePlayer skillplayer, ref int amount, ref List<Vector2> position, ref List<Vector2> velocity) {
		for (int i = 0; i < position.Count; i++) {
			Vector2 vel = velocity[i];
			vel.X = -1 * velocity[i].Length();
			velocity[i] = vel;
		}
	}
}
public class Modify_CastRight : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 50;
		Skill_Duration = 60;
		Skill_Type = SkillTypeID.Modify;
	}
	public override void ModifyShootProjectile(Player player, SkillHandlePlayer skillplayer, ref int amount, ref List<Vector2> position, ref List<Vector2> velocity) {
		for (int i = 0; i < position.Count; i++) {
			Vector2 vel = velocity[i];
			vel.X = 1 * velocity[i].Length();
			velocity[i] = vel;
		}
	}
}
