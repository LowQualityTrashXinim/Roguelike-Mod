using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;
using Terraria;
using Terraria.ID;

namespace Roguelike.Contents.NPCs.LootBoxLord.HostileProjectile;
public abstract class BaseHostileBow : BaseHostileProjectile {
	public override void SetHostileDefaults() {
		Projectile.width = 16;
		Projectile.height = 32;
		Projectile.tileCollide = true;
		Projectile.penetrate = -1;
	}
}
class WoodBowAttackOne : BaseHostileBow {
	public override void AI() {
		Projectile.velocity = Projectile.velocity * .98f;
		CanDealContactDamage = false;
		if (++Projectile.ai[0] >= Projectile.ai[2]) {
			ModUtils.NewHostileProjectile(Projectile.GetSource_FromAI(), Projectile.Center + Main.rand.NextVector2Circular(5, 5), Projectile.rotation.ToRotationVector2() * 10f, ProjectileID.WoodenArrowHostile, Projectile.damage, 1, AdjustHostileProjectileDamage: false);
			Projectile.ai[0] = 0;
		}
	}
}
class WoodBowAttackTwo : BaseHostileBow {
	public override void AI() {
		CanDealContactDamage = false;
		int Requirement = 35;
		if (Projectile.ai[1] <= 0)
			Projectile.rotation = (-Projectile.velocity).ToRotation();
		else {
			Requirement += 15;
		}
		if (Projectile.timeLeft > 90)
			Projectile.timeLeft = 90;
		if (++Projectile.ai[0] >= Requirement) {
			if (Projectile.ai[1] <= 0) {
				ModUtils.NewHostileProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.rotation.ToRotationVector2() * 10f, ProjectileID.WoodenArrowHostile, Projectile.damage, 1);
			}
			else {
				Projectile.rotation = Vector2.UnitY.ToRotation();
				ModUtils.NewHostileProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.UnitY * 10f, ProjectileID.WoodenArrowHostile, Projectile.damage, 1, AdjustHostileProjectileDamage: false);
			}
			Projectile.ai[0] = 0;
			Projectile.ai[1]++;
		}
		Projectile.velocity -= Projectile.velocity * .05f;
	}
}
class OreBowAttackOne : BaseHostileBow {
	public override void AI() {
		CanDealContactDamage = false;
		Projectile.rotation = Projectile.velocity.ToRotation();
		if (Projectile.ai[1] > 0) {
			Projectile.ai[1]--;
			Projectile.velocity *= .98f;
			return;
		}
		var player = Main.player[Projectile.owner];
		var vel = (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
		if (Projectile.timeLeft > 150)
			Projectile.timeLeft = 150;
		if (++Projectile.ai[0] >= 50) {
			ModUtils.NewHostileProjectile(Projectile.GetSource_FromAI(), Projectile.Center, vel * 15, ProjectileID.WoodenArrowHostile, Projectile.damage, 1); ;
			Projectile.ai[0] = 0;
		}
		Projectile.rotation = vel.ToRotation();
	}
}
class OreBowAttackTwo : BaseHostileBow {
	public override void AI() {
		CanDealContactDamage = false;
		Projectile.rotation = Vector2.UnitY.ToRotation();
		var player = Main.player[Projectile.owner];
		if (Projectile.timeLeft > 150)
			Projectile.timeLeft = 150;
		var vel = (player.Center.Add(Main.rand.Next(-200, 200), 200) - Projectile.Center).SafeNormalize(Vector2.Zero);
		Projectile.velocity += vel;
		Projectile.velocity.Y = 0;
		if (++Projectile.ai[0] >= 30) {
			ModUtils.NewHostileProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.UnitY, ProjectileID.WoodenArrowHostile, Projectile.damage, 1, AdjustHostileProjectileDamage: false);
			Projectile.ai[0] = 0;
		}
	}
}
