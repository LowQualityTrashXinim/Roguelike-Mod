using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;
using Terraria;

namespace Roguelike.Contents.NPCs.LootBoxLord.HostileProjectile;
public class BaseHostileSpear : BaseHostileProjectile {
	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
		float length = Projectile.Size.Length();
		Vector2 rotationVector = Projectile.rotation.ToRotationVector2();
		if (ModUtils.Collision_PointAB_EntityCollide(targetHitbox, Projectile.Center.IgnoreTilePositionOFFSET(rotationVector, length / 2), Projectile.Center.IgnoreTilePositionOFFSET(rotationVector, -length / 2))) {
			return true;
		}
		return false;
	}
}
