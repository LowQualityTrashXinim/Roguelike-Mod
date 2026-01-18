using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Roguelike.Texture;
using Roguelike.Common.Utils;
using Microsoft.Xna.Framework.Graphics;

namespace Roguelike.Contents.Projectiles;
internal class SpiritProjectile : ModProjectile {
	public override string Texture => ModTexture.SMALLWHITEBALL;
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailingMode[Type] = 0;
		ProjectileID.Sets.TrailCacheLength[Type] = 50;
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 15;
		Projectile.penetrate = 1;
		Projectile.tileCollide = false;
		Projectile.light = 1f;
		Projectile.friendly = true;
		Projectile.extraUpdates = 10;
		Projectile.timeLeft = ModUtils.ToSecond(100);
	}
	public override void AI() {
		if (Projectile.ai[0] % 15 == 0) {
			Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GemDiamond);
			dust.noGravity = true;
			dust.velocity = Vector2.Zero;
			dust.scale = Main.rand.NextFloat(.85f, 1.25f);
		}
		if (++Projectile.ai[0] > 300) {
			float progress = MathHelper.Lerp(0, 2, Math.Clamp(++Projectile.ai[1] / 300f, 0, 1));
			Projectile.Center.LookForHostileNPC(out NPC npc, 2500, true);
			if (npc != null) {
				Projectile.velocity = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * progress;
			}
		}
		if (--Projectile.ai[2] <= 0) {
			Projectile.ai[2] = Main.rand.Next(1, 10) * 50;
			Projectile.spriteDirection *= -1;
		}
		if (!Projectile.velocity.IsLimitReached(1))
			Projectile.velocity -= Projectile.velocity * .9f;
	}
	public override void OnKill(int timeLeft) {
		for (int i = 0; i < 10; i++) {
			int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.GemDiamond);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = Main.rand.NextVector2Circular(3, 3);
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		Projectile.DrawTrail(lightColor, .02f);
		ModUtils.Draw_SetUpToDrawGlow(Main.spriteBatch);
		Texture2D texture = ModContent.Request<Texture2D>(ModTexture.Glow_Big).Value;
		Vector2 origin = texture.Size() * .5f;
		Vector2 drawpos = Projectile.Center - Main.screenPosition;
		Main.EntitySpriteDraw(texture, drawpos, null, lightColor, 0, origin, Projectile.scale, SpriteEffects.None);
		ModUtils.Draw_ResetToNormal(Main.spriteBatch);
		return false;
	}
}
