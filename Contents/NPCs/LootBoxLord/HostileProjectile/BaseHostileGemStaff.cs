using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;
using Terraria;
using Terraria.ID;

namespace Roguelike.Contents.NPCs.LootBoxLord.HostileProjectile;
public abstract class BaseHostileGemStaff : BaseHostileProjectile {
	public int ProjectileType = ProjectileID.AmethystBolt;
	public override void SetHostileDefaults() {
		Projectile.width = 40;
		Projectile.height = 42;
		Projectile.tileCollide = false;
		Projectile.penetrate = -1;
		CanDealContactDamage = false;
	}
}
class GemStaffAttackOne : BaseHostileGemStaff {
	public override void AI() {
		if (Projectile.timeLeft > 180)
			Projectile.timeLeft = 180;
		Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4 + MathHelper.ToRadians(Projectile.ai[1] - 70);
		if (++Projectile.ai[0] >= 35) {
			ModUtils.NewHostileProjectile(Projectile.GetSource_FromAI(), Projectile.Center, (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * 10f, ProjectileType, Projectile.damage, 1 );
			Projectile.ai[0] = 0;
		}
		Projectile.ai[1] += 2;
	}
}
class GemStaffAttackTwo : BaseHostileGemStaff {
	//Projectile.ai[2] act as projectile index
	public override void AI() {
		if (++Projectile.ai[1] <= 60) {
			Projectile.ai[0] = Projectile.ai[2] * 90 / 6;
			Projectile.velocity *= .985f;
			Projectile.rotation = MathHelper.ToRadians(Projectile.ai[1] * 10);
			return;
		}
		if (IsNPCActive(out var npc)) {
			npc.TargetClosest();
			var player = Main.player[npc.target];
			if (!player.active || player.dead) {
				Projectile.velocity = Vector2.Zero;
				return;
			}
			var pos = npc.Center + Vector2.One.Vector2DistributeEvenlyPlus(TerrariaArrayID.AllGemStaffPHM.Length + 1, 360, Projectile.ai[2]).RotatedBy(MathHelper.ToRadians(Projectile.ai[1] * .5f)) * 460;
			Projectile.velocity = (pos - Projectile.Center).SafeNormalize(Vector2.Zero) * (pos - Projectile.Center).Length() / 32f;
			var specializePlayerVelocity = player.velocity;
			specializePlayerVelocity.X *= 32;
			specializePlayerVelocity.Y *= 8;
			float rotateToPlayer = (player.Center + specializePlayerVelocity - Projectile.Center).ToRotation();
			Projectile.rotation = rotateToPlayer + MathHelper.PiOver4;
			if (Projectile.timeLeft > 300)
				Projectile.timeLeft = 300;
			if (++Projectile.ai[0] >= 90) {
				ModUtils.NewHostileProjectile(Projectile.GetSource_FromAI(), Projectile.Center, (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * 6, ProjectileType, Projectile.damage, 1 );
				Projectile.ai[0] = 0;
			}
		}
	}
}
