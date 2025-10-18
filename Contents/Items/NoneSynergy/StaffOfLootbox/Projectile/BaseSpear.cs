using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;
using Terraria;

namespace Roguelike.Contents.NPCs.LootBoxLord.HostileProjectile;
public class BaseSpear : BaseProjectile {
	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
		float length = Projectile.Size.Length();
		Vector2 rotationVector = Projectile.rotation.ToRotationVector2();
		if (ModUtils.Collision_PointAB_EntityCollide(targetHitbox, Projectile.Center.IgnoreTilePositionOFFSET(rotationVector, length / 2f), Projectile.Center.IgnoreTilePositionOFFSET(rotationVector, -length / 2))) {
			return true;
		}
		return false;
	}
}
public class LBL_SpearAttackOne : BaseSpear {
	public override void SetHostileDefaults() {
		Projectile.penetrate = -1;
		Projectile.width = Projectile.height = 32;
		Projectile.tileCollide = false;
	}
	public Vector2 TowardTo = Vector2.Zero;
	public override void AI() {
		if (++Projectile.ai[0] >= 30) {
			Projectile.velocity = TowardTo;
		}
		else {
			Projectile.velocity *= .96f;
		}
		Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
	}
}
