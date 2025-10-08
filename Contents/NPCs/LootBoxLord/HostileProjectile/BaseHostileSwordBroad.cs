using Microsoft.Xna.Framework;
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
