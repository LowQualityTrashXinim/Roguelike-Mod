using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;
using Terraria;

namespace Roguelike.Contents.Items.NoneSynergy.StaffOfLootbox.Projectiles;
public abstract class BaseSwordBroad : BaseProjectile {
	public override void SetHostileDefaults() {
		Projectile.width = Projectile.height = 36;
		Projectile.tileCollide = false;
		Projectile.penetrate = -1;
	}
}
class LBL_SwordBroadAttackOne : BaseSwordBroad {
	bool AiChange = false;
	public override void AI() {
		if (++Projectile.ai[0] <= 40) {
			Projectile.velocity *= .96f;
			Projectile.rotation = MathHelper.ToRadians(Projectile.ai[0] * 10);
			return;
		}
		if (IsNPCActive(out _)) {
			if (Projectile.Center.LookForHostileNPC(out NPC player, 1000)) {
				if (!AiChange) {
					Projectile.velocity = (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 15;
					Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
					Projectile.timeLeft = 150 + (int)(player.Center - Projectile.Center).Length();
					AiChange = !AiChange;
				}
			}
			else {
				Projectile.velocity = Vector2.Zero;
			}
		}
		else {
			Projectile.Kill();
		}
	}
}
class LBL_SwordBroadAttackTwo : BaseSwordBroad {
	public override void AI() {
		Projectile.rotation = MathHelper.PiOver4 + MathHelper.PiOver2;
		if (Projectile.ai[1] == 1) {
			if (Projectile.timeLeft > 30)
				Projectile.timeLeft = 30;
			Projectile.velocity.Y = 50;
			Projectile.velocity.X = 0;
			return;
		}
		if (Projectile.ai[1] > 1) {
			Projectile.velocity.Y += -.5f;
			Projectile.ai[1]--;
			Projectile.velocity -= Projectile.velocity * .1f;
		}
		else {
			Projectile.ai[1] = 1;
			Projectile.velocity = Vector2.Zero;
		}
	}
}
class LBL_SwordBroadDesperation : BaseSwordBroad {
	public override void AI() {
		Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
	}
}
