using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace Roguelike.Contents.NPCs.LootBoxLord.HostileProjectile;
public abstract class BaseHostileGun : BaseHostileProjectile {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void PreDrawDraw(Texture2D texture, Vector2 drawPos, Vector2 origin, ref Color lightColor, out bool DrawOrigin) {
		var effect = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically;
		Main.EntitySpriteDraw(texture, drawPos, null, lightColor, Projectile.rotation, origin, Projectile.scale, effect, 0);
		DrawOrigin = false;
	}
}
public class HostileMinishark : BaseHostileGun {
	public override void SetHostileDefaults() {
		Projectile.width = 54;
		Projectile.height = 20;
		Projectile.penetrate = -1;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 300;
		CanDealContactDamage = false;
	}
	public override void AI() {
		if (IsNPCActive(out var npc)) {
			npc.TargetClosest();
			if (Projectile.ai[2] == 0) {
				Projectile.ai[2] = Main.rand.NextBool().ToDirectionInt();
			}
			var player = Main.player[npc.target];
			float rotation = MathHelper.ToRadians(Projectile.timeLeft * 3 * Projectile.ai[2]);
			var TowardPlayer = rotation.ToRotationVector2();
			Projectile.velocity = (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * (player.Center - Projectile.Center).Length() / 128f;
			Projectile.rotation = rotation;
			if (++Projectile.ai[1] <= 40) {
				return;
			}
			if (++Projectile.ai[0] >= 8) {
				Projectile.ai[0] = 0;
				ModUtils.NewHostileProjectile(Projectile.GetSource_FromAI(), Projectile.Center, TowardPlayer.Vector2RotateByRandom(7) * Main.rand.NextFloat(7, 11), ProjectileID.Bullet, Projectile.damage / 3, 1);
			}
		}
		else {
			Projectile.Kill();
		}
	}
}
public class HostileMusket : BaseHostileGun {
	public override void SetHostileDefaults() {
		Projectile.width = 56;
		Projectile.height = 18;
		Projectile.penetrate = -1;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 300;
		CanDealContactDamage = false;
	}
	public override void AI() {
		if (IsNPCActive(out var npc)) {
			npc.TargetClosest();
			var player = Main.player[npc.target];
			if (Projectile.ai[2] == 0) {
				Projectile.ai[2] = 1;
			}
			var BesidePlayer = player.Center + new Vector2(Main.rand.Next(-50, 50) - 600 * Projectile.ai[2], Main.rand.Next(-10, 10));
			var TowardPlayer = (player.Center - Projectile.Center + player.velocity).SafeNormalize(Vector2.Zero);
			Projectile.velocity = (BesidePlayer - Projectile.Center).SafeNormalize(Vector2.Zero) * 10;
			Projectile.velocity = Projectile.velocity.LimitedVelocity((BesidePlayer - Projectile.Center).Length() * .05f);
			if (++Projectile.ai[0] >= 40) {
				SoundEngine.PlaySound(SoundID.Item38 with {
					Pitch = 1f
				}, Projectile.Center);
				Projectile.ai[0] = 0;
				ModUtils.NewHostileProjectile(Projectile.GetSource_FromAI(), Projectile.Center, TowardPlayer * 15f, ProjectileID.Bullet, Projectile.damage, 1);
				for (int i = 0; i < 30; i++) {
					int dust = Dust.NewDust(Projectile.Center.PositionOFFSET(TowardPlayer, 10), 0, 0, DustID.Torch);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity = Main.rand.NextVector2Unit(-MathHelper.PiOver4 * .5f, MathHelper.PiOver4).RotatedBy(Projectile.rotation) * Main.rand.NextFloat(7f, 19f);
					Main.dust[dust].scale = Main.rand.NextFloat(.9f, 1.5f);
				}
			}
			Projectile.rotation = TowardPlayer.ToRotation();
			Projectile.spriteDirection = (int)Projectile.ai[2];
		}
		else {
			Projectile.Kill();
		}
	}
}
public class HostileBoomStick : BaseHostileGun {
	public override void SetHostileDefaults() {
		Projectile.width = 56;
		Projectile.height = 18;
		Projectile.penetrate = -1;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 60;
		CanDealContactDamage = false;
	}
	public override void AI() {
		if (IsNPCActive(out var npc)) {
			npc.TargetClosest();
			var player = Main.player[npc.target];
			Projectile.velocity *= .9f;
			var TowardPlayer = (player.Center - Projectile.Center + player.velocity).SafeNormalize(Vector2.Zero);
			Projectile.rotation = TowardPlayer.ToRotation();
			if (Projectile.timeLeft < 30 && Projectile.ai[0] == 0) {
				Projectile.ai[0]++;
				Projectile.velocity = -TowardPlayer * 10f;
				SoundEngine.PlaySound(SoundID.Item38 with {
					Pitch = 1f
				}, Projectile.Center);
				for (int i = 0; i < 4; i++) {
					ModUtils.NewHostileProjectile(Projectile.GetSource_FromAI(), Projectile.Center, TowardPlayer.Vector2RotateByRandom(30) * 12f * Main.rand.NextFloat(.5f, 1f), ProjectileID.Bullet, Projectile.damage, 1);
				}
				for (int i = 0; i < 15; i++) {
					int dust = Dust.NewDust(Projectile.Center.PositionOFFSET(TowardPlayer, 10), 0, 0, DustID.Torch);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity = Main.rand.NextVector2Unit(-MathHelper.PiOver4 * .5f, MathHelper.PiOver4).RotatedBy(Projectile.rotation) * Main.rand.NextFloat(4f, 11f);
					Main.dust[dust].scale = Main.rand.NextFloat(.9f, 1.5f);
				}
			}
		}
		else {
			Projectile.Kill();
		}
	}
}
public class HostilePistolAttackOne : BaseHostileGun {
	public override void SetHostileDefaults() {
		Projectile.width = 32;
		Projectile.height = 32;
		Projectile.penetrate = -1;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 300;
		CanDealContactDamage = false;
	}
	public override void AI() {
		if (IsNPCActive(out var npc)) {
			Vector2 TowardTo = Vector2.One.RotatedBy(MathHelper.ToRadians(Projectile.ai[0] + Projectile.timeLeft * 4));
			Projectile.Center = npc.Center + TowardTo * 50;
			Projectile.rotation = TowardTo.ToRotation();
			if (++Projectile.ai[1] <= 40) {
				return;
			}
			if (++Projectile.ai[2] >= 6) {
				Projectile.ai[2] = 0;
				ModUtils.NewHostileProjectile(Projectile.GetSource_FromAI(), Projectile.Center, TowardTo * 3, ProjectileID.Bullet, Projectile.damage, 1);
			}
		}
		else {
			Projectile.Kill();
		}
	}
}
public class HostilePistolAttackTwo : BaseHostileGun {
	public override void SetHostileDefaults() {
		Projectile.width = 32;
		Projectile.height = 32;
		Projectile.penetrate = -1;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 300;
		CanDealContactDamage = false;
	}
	public override void AI() {
		if (IsNPCActive(out var npc)) {
			npc.TargetClosest();
			var player = Main.player[npc.target];
			Projectile.velocity *= .9f;
			var TowardPlayer = (player.Center - Projectile.Center + player.velocity).SafeNormalize(Vector2.Zero);
			Projectile.direction = ModUtils.DirectionFromEntityAToEntityB(Projectile.Center.X, player.Center.X);
			Projectile.spriteDirection = Projectile.direction;
			Projectile.rotation = TowardPlayer.ToRotation();
			if (++Projectile.ai[0] >= 20) {
				Projectile.ai[0] = 0;
				SoundEngine.PlaySound(SoundID.Item38 with {
					Pitch = 1f
				}, Projectile.Center);
				ModUtils.NewHostileProjectile(Projectile.GetSource_FromAI(), Projectile.Center, TowardPlayer * 12f, ProjectileID.Bullet, Projectile.damage, 1);

				for (int i = 0; i < 15; i++) {
					int dust = Dust.NewDust(Projectile.Center.PositionOFFSET(TowardPlayer, 10), 0, 0, DustID.Torch);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity = Main.rand.NextVector2Unit(-MathHelper.PiOver4 * .5f, MathHelper.PiOver4).RotatedBy(Projectile.rotation) * Main.rand.NextFloat(4f, 11f);
					Main.dust[dust].scale = Main.rand.NextFloat(.9f, 1.5f);
				}
			}
		}
		else {
			Projectile.Kill();
		}
	}
}
public class HostileMinisharkDesperation : BaseHostileGun {
	public override void SetHostileDefaults() {
		Projectile.width = 54;
		Projectile.height = 20;
		Projectile.penetrate = -1;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 300;
		CanDealContactDamage = false;
		IDtextureValue = ItemID.Minishark;
	}
	public override void AI() {
		if (IsNPCActive(out var npc)) {
			Projectile.timeLeft = 100;
			npc.TargetClosest();
			var player = Main.player[npc.target];
			Projectile.velocity *= .9f;
			var TowardPlayer = (player.Center - Projectile.Center + player.velocity).SafeNormalize(Vector2.Zero);
			Projectile.direction = ModUtils.DirectionFromEntityAToEntityB(Projectile.Center.X, player.Center.X);
			Projectile.spriteDirection = Projectile.direction;
			Projectile.rotation = TowardPlayer.ToRotation();
			if (++Projectile.ai[0] >= 8) {
				Projectile.ai[0] = 0;
				ModUtils.NewHostileProjectile(Projectile.GetSource_FromAI(), Projectile.Center, TowardPlayer * 7f, ProjectileID.Bullet, Projectile.damage, 1);

				for (int i = 0; i < 3; i++) {
					int dust = Dust.NewDust(Projectile.Center.PositionOFFSET(TowardPlayer, 10), 0, 0, DustID.Torch);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity = Main.rand.NextVector2Unit(-MathHelper.PiOver4 * .5f, MathHelper.PiOver4).RotatedBy(Projectile.rotation) * Main.rand.NextFloat(4f, 11f);
					Main.dust[dust].scale = Main.rand.NextFloat(.9f, 1.5f);
				}
			}
		}
		else {
			Projectile.Kill();
		}
	}
}
