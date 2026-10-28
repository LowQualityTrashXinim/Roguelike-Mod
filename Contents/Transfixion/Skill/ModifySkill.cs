using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.Global.AIModify;
using Roguelike.Common.Systems.Skill;
using Roguelike.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

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
		Skill_EnergyRequire = 400;
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
		Skill_EnergyRequire = 10;
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
		Skill_EnergyRequire = 1;
		Skill_Duration = 0;
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
		Skill_EnergyRequire = 1;
		Skill_Duration = 0;
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
		Skill_EnergyRequire = 1;
		Skill_Duration = 0;
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
		Skill_EnergyRequire = 1;
		Skill_Duration = 0;
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
		Skill_EnergyRequire = 1;
		Skill_Duration = 0;
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
		Skill_EnergyRequire = 1;
		Skill_Duration = 0;
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
		Skill_EnergyRequire = 1;
		Skill_Duration = 0;
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
		Skill_EnergyRequire = 1;
		Skill_Duration = 0;
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

public class Modify_Orbiting : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 50;
		Skill_Duration = 60;
		Skill_Type = SkillTypeID.Modify;
	}
	public override void Post_SpawnProjectile(Player player, SkillHandlePlayer skillplayer, ref Projectile projectile, int useTime) {
		RoguelikeGlobalProjectile.Add_projectileAImodify(ProjectileAIModify.Get_ModifyType<AIModify_Orbiting>(), ref projectile);
	}
}
public class Modify_AddTrailWhiteGem : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 10;
		Skill_Duration = 0;
		Skill_Type = SkillTypeID.Modify;
	}
	public override void Post_SpawnProjectile(Player player, SkillHandlePlayer skillplayer, ref Projectile projectile, int useTime) {
		RoguelikeGlobalProjectile.Add_projectileAImodify(ProjectileAIModify.Get_ModifyType<AIModify_AddTrailWhite>(), ref projectile);
	}
}
public class Modify_HelixPathPattern : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 50;
		Skill_Duration = 60;
		Skill_Type = SkillTypeID.Modify;
	}
	public override void Post_SpawnProjectile(Player player, SkillHandlePlayer skillplayer, ref Projectile projectile, int useTime) {
		RoguelikeGlobalProjectile.Add_projectileAImodify(ProjectileAIModify.Get_ModifyType<AIModify_HelixPattern>(), ref projectile);
	}
}
public class Modify_PingPongPattern : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 50;
		Skill_Duration = 60;
		Skill_Type = SkillTypeID.Modify;
	}
	public override void Post_SpawnProjectile(Player player, SkillHandlePlayer skillplayer, ref Projectile projectile, int useTime) {
		RoguelikeGlobalProjectile.Add_projectileAImodify(ProjectileAIModify.Get_ModifyType<AIModify_PingPongPattern>(), ref projectile);
	}
}
public class Modify_TargetPlayer : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 50;
		Skill_Duration = 60;
		Skill_Type = SkillTypeID.Modify;
	}
	public override void Post_SpawnProjectile(Player player, SkillHandlePlayer skillplayer, ref Projectile projectile, int useTime) {
		RoguelikeGlobalProjectile.Add_projectileAImodify(ProjectileAIModify.Get_ModifyType<AIModify_FlyTowardPlayer>(), ref projectile);
	}
}
public class Modify_ShootDownWard : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 450;
		Skill_Duration = 0;
		Skill_Type = SkillTypeID.Modify;
	}
	public override void Pre_SpawnProjectile(Player player, SkillHandlePlayer skillplayer, int amount, ref Vector2 position, ref Vector2 velocity, ref int damage, ref float knockback, ref int type, ref int useTime) {
		useTime += 30;
	}
	public override void Post_SpawnProjectile(Player player, SkillHandlePlayer skillplayer, ref Projectile projectile, int useTime) {
		RoguelikeGlobalProjectile.Add_projectileAImodify(ProjectileAIModify.Get_ModifyType<AIModify_ShootItselfDownWard>(), ref projectile);
	}
}
public class Modify_ShootUpWard : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 450;
		Skill_Duration = 0;
		Skill_Type = SkillTypeID.Modify;
	}
	public override void Pre_SpawnProjectile(Player player, SkillHandlePlayer skillplayer, int amount, ref Vector2 position, ref Vector2 velocity, ref int damage, ref float knockback, ref int type, ref int useTime) {
		useTime += 30;
	}
	public override void Post_SpawnProjectile(Player player, SkillHandlePlayer skillplayer, ref Projectile projectile, int useTime) {
		RoguelikeGlobalProjectile.Add_projectileAImodify(ProjectileAIModify.Get_ModifyType<AIModify_ShootItselfUpWard>(), ref projectile);
	}
}

public class Modify_ShootChaotically : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 250;
		Skill_Duration = 0;
		Skill_Type = SkillTypeID.Modify;
	}
	public override void Pre_SpawnProjectile(Player player, SkillHandlePlayer skillplayer, int amount, ref Vector2 position, ref Vector2 velocity, ref int damage, ref float knockback, ref int type, ref int useTime) {
		useTime += 30;
	}
	public override void Post_SpawnProjectile(Player player, SkillHandlePlayer skillplayer, ref Projectile projectile, int useTime) {
		RoguelikeGlobalProjectile.Add_projectileAImodify(ProjectileAIModify.Get_ModifyType<AIModify_ShootItselfChaotically>(), ref projectile);
	}
}
