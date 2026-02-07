using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;
using Terraria;

namespace Roguelike.Contents.NPCs.LootBoxLord.HostileProjectile;
public abstract class BaseHostileSwordBroad : BaseHostileProjectile {
	public override void SetHostileDefaults() {
		Projectile.width = Projectile.height = 36;
		Projectile.tileCollide = false;
		Projectile.penetrate = -1;
	}
}
class SwordBroadAttackOne : BaseHostileSwordBroad {
	bool AiChange = false;
	public override void AI() {
		if (++Projectile.ai[0] <= 40) {
			Projectile.velocity *= .96f;
			Projectile.rotation = MathHelper.ToRadians(Projectile.ai[0] * 10);
			return;
		}
		if (IsNPCActive(out var npc)) {
			npc.TargetClosest();
			var player = Main.player[npc.target];
			if (!player.active || player.dead) {
				Projectile.velocity = Vector2.Zero;
				return;
			}
			if (!AiChange) {
				Projectile.velocity = (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 15;
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
				Projectile.timeLeft = 150 + (int)(player.Center - Projectile.Center).Length();
				AiChange = !AiChange;
			}
		}
		else {
			Projectile.Kill();
		}
	}
}
class SwordBroadAttackTwo : BaseHostileSwordBroad {
	public override void AI() {
		if (Projectile.ai[1] == 1) {
			if (Projectile.timeLeft > 50) {
				Projectile.timeLeft = 50;
			}
			Projectile.velocity += new Vector2(Projectile.ai[0], Projectile.ai[2]) * -3;
			Projectile.velocity = Projectile.velocity.LimitedVelocity(25);
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
			return;
		}
		if (Projectile.ai[1] > 1) {
			Projectile.velocity -= Projectile.velocity * .1f;
			if (Projectile.ai[0] == 0 && Projectile.ai[2] == 0) {
				Projectile.ai[0] = Projectile.velocity.X;
				Projectile.ai[2] = Projectile.velocity.Y;
			}
			Projectile.rotation = (-new Vector2(Projectile.ai[0], Projectile.ai[2])).ToRotation() + MathHelper.PiOver4;
			Projectile.ai[1]--;
		}
	}
}
class SwordBroadDesperation : BaseHostileSwordBroad {
	public override void AI() {
		Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
	}
}
