using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Roguelike.Common.Utils;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Projectiles;
internal class SlashEdgeProjectile : ModProjectile {
	public override string Texture => ModUtils.GetVanillaTexture<Projectile>(ProjectileID.NightsEdge);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 16;
		Projectile.friendly = true;
		Projectile.penetrate = 10;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = -1;
		Projectile.ownerHitCheck = true;
		Projectile.stopsDealingDamageAfterPenetrateHits = true;
	}
	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
		float coneLength2 = 94f * Projectile.scale;
		float num3 = (float)Math.PI * 2f / 25f * Projectile.ai[0];
		float maximumAngle2 = (float)Math.PI / 4f;
		float num4 = Projectile.rotation + num3;
		if (targetHitbox.IntersectsConeSlowMoreAccurate(Projectile.Center, coneLength2, num4, maximumAngle2))
			return true;

		float num5 = Utils.Remap(Projectile.localAI[0], Projectile.ai[1] * 0.3f, Projectile.ai[1] * 0.5f, 1f, 0f);
		if (num5 > 0f) {
			float coneRotation2 = num4 - (float)Math.PI / 4f * Projectile.ai[0] * num5;
			if (targetHitbox.IntersectsConeSlowMoreAccurate(Projectile.Center, coneLength2, coneRotation2, maximumAngle2))
				return true;
		}

		return false;
	}
	public override void CutTiles() {
		Vector2 vector2 = (Projectile.rotation - (float)Math.PI / 4f).ToRotationVector2() * 60f * Projectile.scale;
		Vector2 vector3 = (Projectile.rotation + (float)Math.PI / 4f).ToRotationVector2() * 60f * Projectile.scale;
		float num2 = 60f * Projectile.scale;
		Utils.PlotTileLine(Projectile.Center + vector2, Projectile.Center + vector3, num2, DelegateMethods.CutTiles);
	}
	public override void AI() {
		Projectile.localAI[0] += 1f;
		Player player = Main.player[Projectile.owner];
		float num = Projectile.localAI[0] / Projectile.ai[1];
		float num2 = Projectile.ai[0];
		float num3 = Projectile.velocity.ToRotation();
		float num4 = (float)Math.PI * num2 * num + num3 + num2 * (float)Math.PI + player.fullRotation;
		Projectile.rotation = num4;
		float num5 = 0.2f;
		float num6 = 1f;

		//Projectile.Center = player.RotatedRelativePoint(player.MountedCenter) - Projectile.velocity;
		Projectile.scale = num6 + num * num5;

		if (Math.Abs(num2) < 0.2f) {
			Projectile.rotation += (float)Math.PI * 4f * num2 * 10f * num;
			float num7 = Utils.Remap(Projectile.localAI[0], 10f, Projectile.ai[1] - 5f, 0f, 1f);
			Projectile.scale += num7 * 0.4f;
		}
		Projectile.scale *= Projectile.ai[2];
		if (Projectile.localAI[0] >= Projectile.ai[1])
			Projectile.Kill();
	}
	public override bool PreDraw(ref Color lightColor) {
		Vector2 vector = Projectile.Center - Main.screenPosition;
		Asset<Texture2D> asset = TextureAssets.Projectile[Projectile.type];
		Rectangle rectangle = asset.Frame(1, 4);
		Vector2 origin = rectangle.Size() / 2f;
		float num = Projectile.scale * 1.1f;
		SpriteEffects effects = ((!(Projectile.ai[0] >= 0f)) ? SpriteEffects.FlipVertically : SpriteEffects.None);
		float num2 = Projectile.localAI[0] / Projectile.ai[1];
		float num3 = Utils.Remap(num2, 0f, 0.6f, 0f, 1f) * Utils.Remap(num2, 0.6f, 1f, 1f, 0f);
		float num4 = 0.975f;
		float fromValue = Lighting.GetColor(Projectile.Center.ToTileCoordinates()).ToVector3().Length() / (float)Math.Sqrt(3.0);
		fromValue = Utils.Remap(fromValue, 0.2f, 1f, 0f, 1f);
		Color color = new Color(40, 20, 60);
		Main.spriteBatch.Draw(asset.Value, vector, rectangle, color * fromValue * num3, Projectile.rotation + Projectile.ai[0] * ((float)Math.PI / 4f) * -1f * (1f - num2), origin, num * num4, effects, 0f);
		Color color2 = new Color(80, 40, 180);
		Color color3 = Color.White * num3 * 0.5f;
		color3.A = (byte)((float)(int)color3.A * (1f - fromValue));
		Color color4 = color3 * fromValue * 0.5f;
		color4.G = (byte)((float)(int)color4.G * fromValue);
		color4.R = (byte)((float)(int)color4.R * (0.25f + fromValue * 0.75f));
		Main.spriteBatch.Draw(asset.Value, vector, rectangle, color4 * 0.15f, Projectile.rotation + Projectile.ai[0] * 0.01f, origin, num, effects, 0f);
		Main.spriteBatch.Draw(asset.Value, vector, rectangle, new Color(80, 30, 160) * fromValue * num3 * 0.3f, Projectile.rotation, origin, num * 0.8f, effects, 0f);
		Main.spriteBatch.Draw(asset.Value, vector, rectangle, color2 * fromValue * num3 * 0.7f, Projectile.rotation, origin, num * num4, effects, 0f);
		Main.spriteBatch.Draw(asset.Value, vector, asset.Frame(1, 4, 0, 3), Color.White * 0.3f * num3 * (1f - fromValue * 0.7f), Projectile.rotation + Projectile.ai[0] * 0.01f, origin, num, effects, 0f);
		return false;
	}
}
