using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;
using Terraria;
using Terraria.DataStructures;

namespace Roguelike.Contents.NPCs.LootBoxLord.HostileProjectile;
public abstract class BaseShortSword : BaseProjectile {
	public override void SetHostileDefaults() {
		Projectile.width = Projectile.height = 32;
		Projectile.tileCollide = false;
		Projectile.penetrate = -1;
	}
	protected int OnSpawnDirection = 0;
	public override void OnSpawn(IEntitySource source) {
		OnSpawnDirection = Projectile.velocity.X > 0 ? 1 : -1;
		base.OnSpawn(source);
	}
}
class LBL_ShortSwordAttackOne : BaseShortSword {
	public override void AI() {
		Projectile.Center.LookForHostileNPC(out NPC player, 1000);
		if(player == null) {
			return;
		}
		if (Projectile.ai[0] == 1) {
			if (Projectile.timeLeft > 30)
				Projectile.velocity += (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
			if (Projectile.timeLeft > 160)
				Projectile.timeLeft = 160;
			return;
		}
		if (Projectile.velocity.IsLimitReached(3)) {
			Projectile.velocity -= Projectile.velocity * .05f;
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
		}
		else {
			Projectile.ai[0] = 1;
		}
	}
}
class LBL_ShortSwordAttackTwo : BaseShortSword {
	public override void AI() {
		Projectile.Center.LookForHostileNPC(out NPC player, 1000);
		if (player == null) {
			return;
		}
		var LeftOfPlayer = new Vector2(player.Center.X + 400 * OnSpawnDirection, player.Center.Y);
		if (Projectile.ai[1] < 0) {
			Projectile.ai[1]++;
			return;
		}
		if (Projectile.ai[1] == 0) {
			if (!Projectile.Center.IsCloseToPosition(LeftOfPlayer, 10f)) {
				var distance = LeftOfPlayer - Projectile.Center;
				float length = distance.Length();
				if (length > 5) {
					length = 5;
				}
				Projectile.velocity -= Projectile.velocity * .08f;
				Projectile.velocity += distance.SafeNormalize(Vector2.Zero) * length;
				Projectile.velocity = Projectile.velocity.LimitedVelocity(20);
			}
			else {
				Projectile.velocity = -Vector2.UnitX * OnSpawnDirection;
				Projectile.timeLeft = 150;
				Projectile.ai[1] = 1;
				Projectile.Center = LeftOfPlayer;
			}
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
		}
		else {
			Projectile.ai[1]++;
			if (Projectile.ai[1] >= 30) {
				if (Projectile.ai[1] >= 45) {
					Projectile.velocity -= Vector2.UnitX * 2 * OnSpawnDirection;
					return;
				}
				Projectile.velocity += Vector2.UnitX * OnSpawnDirection;
				return;
			}
		}
	}
}
class LBL_ShortSwordDesperation : BaseSwordBroad {
	public override void AI() {
		Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
	}
}
