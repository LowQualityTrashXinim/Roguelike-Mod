using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;
using Terraria;
using Terraria.ID;

namespace Roguelike.Contents.Items.NoneSynergy.StaffOfLootbox.Projectiles;
public abstract class BaseBow : BaseProjectile {
	public override void SetHostileDefaults() {
		Projectile.width = 16;
		Projectile.height = 32;
		Projectile.tileCollide = true;
		Projectile.penetrate = -1;
	}
}
class LBL_WoodBowAttackOne : BaseBow {
	public override void AI() {
		Projectile.velocity = Projectile.velocity * .98f;
		CanDealContactDamage = false;
		if (++Projectile.ai[0] >= Projectile.ai[2]) {
			Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + Main.rand.NextVector2Circular(5, 5), Projectile.rotation.ToRotationVector2() * 10f, ProjectileID.WoodenArrowFriendly, Projectile.damage, 1, Projectile.owner);
			Projectile.ai[0] = 0;
		}
	}
}
class LBL_WoodBowAttackTwo : BaseBow {
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
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.rotation.ToRotationVector2() * 10f, ProjectileID.WoodenArrowFriendly, Projectile.damage, 1, Projectile.owner);
			}
			else {
				Projectile.rotation = Vector2.UnitY.ToRotation();
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.UnitY * 10f, ProjectileID.WoodenArrowFriendly, Projectile.damage, 1, Projectile.owner);
			}
			Projectile.ai[0] = 0;
			Projectile.ai[1]++;
		}
		Projectile.velocity -= Projectile.velocity * .05f;
	}
}
class LBL_OreBowAttackOne : BaseBow {
	public override void AI() {
		CanDealContactDamage = false;
		Projectile.rotation = Projectile.velocity.ToRotation();
		if (Projectile.ai[1] > 0) {
			Projectile.ai[1]--;
			Projectile.velocity *= .98f;
			return;
		}
		var trueplayer = Main.player[Projectile.owner];
		if (trueplayer.dead || !trueplayer.active) {
			return;
		}
		if (Projectile.Center.LookForHostileNPC(out NPC player, 1000)) {
			var vel = (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
			if (Projectile.timeLeft > 150)
				Projectile.timeLeft = 150;
			if (++Projectile.ai[0] >= 50) {
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, vel * 15, ProjectileID.WoodenArrowFriendly, Projectile.damage, 1, Projectile.owner);
				Projectile.ai[0] = 0;
			}
			Projectile.rotation = vel.ToRotation();
		}
	}
}
class LBL_OreBowAttackTwo : BaseBow {
	public override void AI() {
		CanDealContactDamage = false;
		Projectile.rotation = Vector2.UnitY.ToRotation();
		var trueplayer = Main.player[Projectile.owner];
		if (trueplayer.dead || !trueplayer.active) {
			return;
		}
		if (Projectile.Center.LookForHostileNPC(out NPC player, 1000)) {
			if (Projectile.timeLeft > 150)
				Projectile.timeLeft = 150;
			var vel = (player.Center.Add(Main.rand.Next(-200, 200), 200) - Projectile.Center).SafeNormalize(Vector2.Zero);
			Projectile.velocity += vel;
			Projectile.velocity.Y = 0;
			if (++Projectile.ai[0] >= 30) {
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.UnitY, ProjectileID.WoodenArrowFriendly, Projectile.damage, 1, Projectile.owner);
				Projectile.ai[0] = 0;
			}
		}
	}
}
