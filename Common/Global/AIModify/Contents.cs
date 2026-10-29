using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;
using Terraria;
using Terraria.ID;

namespace Roguelike.Common.Global.AIModify;
public class AIModify_Orbiting : ProjectileAIModify {
	public override void PostAI(Projectile projectile) {
		projectile.velocity = projectile.velocity.RotatedBy(MathHelper.ToRadians(5));
	}
}
public class AIModify_AddTrailWhite : ProjectileAIModify {
	public override void PostAI(Projectile projectile) {
		Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.GemDiamond);
		dust.noGravity = true;
		dust.velocity = projectile.velocity * .1f;
		dust.scale += Main.rand.NextFloat(-.2f, .2f);
	}
}
public class AIModify_HelixPattern : ProjectileAIModify {
	public override void PostAI(Projectile projectile) {
		if (projectile.GetGlobalProjectile<RoguelikeGlobalProjectile>().constanttimer < 5) {
			return;
		}
		if (projectile.GetGlobalProjectile<RoguelikeGlobalProjectile>().constanttimer % 20 < 10) {
			projectile.velocity = projectile.velocity.RotatedBy(MathHelper.ToRadians(-10));
		}
		else {
			projectile.velocity = projectile.velocity.RotatedBy(MathHelper.ToRadians(10));
		}
	}
}
public class AIModify_PingPongPattern : ProjectileAIModify {
	public override void PostAI(Projectile projectile) {
		if (projectile.GetGlobalProjectile<RoguelikeGlobalProjectile>().constanttimer % 30 == 0) {
			projectile.velocity = -projectile.velocity;
		}
	}
}
public class AIModify_FlyTowardPlayer : ProjectileAIModify {
	public override void PostAI(Projectile projectile) {
		if (projectile.owner < 0 || projectile.owner >= Main.player.Length) {
			return;
		}
		Player player = Main.player[projectile.owner];
		if (!player.active || player.dead) {
			return;
		}
		projectile.velocity += (player.Center - projectile.Center).SafeNormalize(Vector2.Zero);
	}
}
public class AIModify_ShootItselfDownWard : ProjectileAIModify {
	public override void PostAI(Projectile projectile) {
		if (projectile.GetGlobalProjectile<RoguelikeGlobalProjectile>().constanttimer % 10 == 0) {
			Projectile.NewProjectile(projectile.GetSource_FromAI(), projectile.Center, Vector2.UnitY.Vector2RotateByRandom(10) * 15, projectile.type, projectile.damage, projectile.knockBack, projectile.owner);
		}
	}
}
public class AIModify_ShootItselfUpWard : ProjectileAIModify {
	public override void PostAI(Projectile projectile) {
		if (projectile.GetGlobalProjectile<RoguelikeGlobalProjectile>().constanttimer % 10 == 0) {
			Projectile.NewProjectile(projectile.GetSource_FromAI(), projectile.Center, Vector2.UnitY.Vector2RotateByRandom(10) * -15, projectile.type, projectile.damage, projectile.knockBack, projectile.owner);
		}
	}
}
public class AIModify_ShootItselfChaotically : ProjectileAIModify {
	public override void PostAI(Projectile projectile) {
		if (projectile.GetGlobalProjectile<RoguelikeGlobalProjectile>().constanttimer % 30 == 0) {
			Projectile.NewProjectile(projectile.GetSource_FromAI(), projectile.Center, Main.rand.NextVector2CircularEdge(15, 15) * Main.rand.NextFloat(.9f, 1.1f), projectile.type, projectile.damage, projectile.knockBack, projectile.owner);
		}
	}
}
