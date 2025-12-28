using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Common.Utils;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Projectiles;
public class pearlSwordProj : ModProjectile {
	public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.PearlwoodSword);
	public override void SetDefaults() {
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 3;

		Projectile.width = Projectile.height = 24;
		Projectile.penetrate = 1;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 240;
		Projectile.aiStyle = -1;
		Projectile.alpha = 250;
		Projectile.ArmorPenetration = 10;
		Projectile.extraUpdates = 2;
	}

	float flare = 0;

	public override void PostDraw(Color lightColor) {
		Vector2 position = Projectile.Center - Main.screenPosition;
		DrawPrettyStarSparkle(Projectile.Opacity, SpriteEffects.None, position, new Color(255, 255, 255, 0) * 125, new Color(255, 0, 122, 0) * 125, flare, 0f, 0.5f, 0.5f, 1f, 0f, Vector2.One * 2f, Vector2.One * 2f);
	}
	private static void DrawPrettyStarSparkle(float opacity, SpriteEffects dir, Vector2 drawpos, Color drawColor, Color shineColor, float flareCounter, float fadeInStart, float fadeInEnd, float fadeOutStart, float fadeOutEnd, float rotation, Vector2 scale, Vector2 fatness) {
		Texture2D sparkleTexture = TextureAssets.Extra[ExtrasID.SharpTears].Value;
		Color bigColor = shineColor * opacity * 0.5f;
		bigColor.A = 0;
		Vector2 origin = sparkleTexture.Size() / 2f;
		Color smallColor = drawColor * 0.5f;
		float lerpValue = Utils.GetLerpValue(fadeInStart, fadeInEnd, flareCounter, clamped: true) * Utils.GetLerpValue(fadeOutEnd, fadeOutStart, flareCounter, clamped: true);
		Vector2 scaleLeftRight = new Vector2(fatness.X * 0.5f, scale.X) * lerpValue;
		Vector2 scaleUpDown = new Vector2(fatness.Y * 0.5f, scale.Y) * lerpValue;
		bigColor *= lerpValue;
		smallColor *= lerpValue;
		// Bright, large part
		Main.EntitySpriteDraw(sparkleTexture, drawpos, null, bigColor, MathHelper.PiOver2 + rotation, origin, scaleLeftRight, dir);
		Main.EntitySpriteDraw(sparkleTexture, drawpos, null, bigColor, 0f + rotation, origin, scaleUpDown, dir);
		// Dim, small part
		Main.EntitySpriteDraw(sparkleTexture, drawpos, null, smallColor, MathHelper.PiOver2 + rotation, origin, scaleLeftRight * 0.6f, dir);
		Main.EntitySpriteDraw(sparkleTexture, drawpos, null, smallColor, 0f + rotation, origin, scaleUpDown * 0.6f, dir);
	}
	public override void AI() {

		if (flare < 1f)
			flare += 0.01f;

		Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
		Projectile.ai[0]++;
		if (Projectile.alpha > 0)
			Projectile.alpha -= 4;

		if (Projectile.ai[0] >= 84) {
			Vector2 dir = Projectile.velocity.SafeNormalize(Vector2.UnitY);
			Projectile.velocity = dir * 15;
		}
		if (Main.rand.NextBool(3)) {
			for (int i = 0; i < 5; i++) {
				var dust = Dust.NewDust(Projectile.Center + Projectile.velocity, 4, 4, DustID.PinkTorch);
				Main.dust[dust].noGravity = true;
			}
		}
	}

	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.TrueExcalibur,
		new ParticleOrchestraSettings { PositionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox) },
		Projectile.owner);
		modifiers.DisableCrit();
	}
}
