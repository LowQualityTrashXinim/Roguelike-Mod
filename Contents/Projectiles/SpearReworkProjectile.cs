using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Projectiles;
public abstract class SpearReworkProjectile : ModProjectile {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 30;
		Projectile.penetrate = -1;
		Projectile.aiStyle = -1;
		Projectile.alpha = 0;

		Projectile.hide = true;
		Projectile.ownerHitCheck = true;
		Projectile.DamageType = DamageClass.Melee;
		Projectile.tileCollide = false;
		Projectile.friendly = true;
	}
	protected virtual int SpearType => ProjectileID.Spear;
	protected virtual float HoldoutRangeMin => 50f;
	protected virtual float HoldoutRangeMax => 200f;
	public override void AI() {
		Player player = Main.player[Projectile.owner];
		int duration = player.itemAnimationMax;
		player.heldProj = Projectile.whoAmI;
		if (Projectile.timeLeft > duration) {
			Projectile.timeLeft = duration;
		}
		Projectile.velocity = Vector2.Normalize(Projectile.velocity);
		float halfDuration = duration * 0.75f;
		float progress;
		if (Projectile.timeLeft < halfDuration) {
			progress = Projectile.timeLeft / halfDuration;
		}
		else {
			if (Projectile.timeLeft == halfDuration) {
				SpawnProjectile();
			}
			Projectile.velocity = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero);
			progress = (duration - Projectile.timeLeft) / halfDuration;
		}
		Projectile.Center = player.MountedCenter + Vector2.Lerp(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, ModUtils.InExpo(progress, 11));
		Projectile.rotation = Projectile.velocity.ToRotation();
		Projectile.rotation += Projectile.spriteDirection == -1 ? MathHelper.PiOver4 : MathHelper.PiOver4 + MathHelper.PiOver2;
	}
	public virtual void SpawnProjectile() {
		Vector2 posOffSet = Projectile.Center.PositionOFFSET(Projectile.velocity, HoldoutRangeMax / 2f);
		Projectile proj;
		float timer = 0;
		for (int i = 0; i < 3; i++) {
			proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), posOffSet + Main.rand.NextVector2Circular(20, 20), Projectile.velocity, ModContent.ProjectileType<SimplePiercingProjectile2>(), Projectile.damage / 3, Projectile.knockBack, Projectile.owner, 15, 5, i * 3);
			if (proj.ModProjectile is SimplePiercingProjectile2 piercing2) {
				piercing2.ScaleX = 2;
				piercing2.ScaleY = .25f;
				piercing2.ProjectileColor = Color.White;
				piercing2.FollowPlayer = true;
				proj.usesLocalNPCImmunity = true;
				proj.localNPCHitCooldown = -1;
				proj.usesIDStaticNPCImmunity = false;
			}
			timer = i * 3;
		}
		proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), posOffSet, Projectile.velocity, ModContent.ProjectileType<SimplePiercingProjectile2>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 15, 10, timer + 3);
		if (proj.ModProjectile is SimplePiercingProjectile2 piercing) {
			piercing.ScaleX = 5;
			piercing.ScaleY = .5f;
			piercing.ProjectileColor = Color.White;
			piercing.FollowPlayer = true;
			proj.usesLocalNPCImmunity = true;
			proj.localNPCHitCooldown = -1;
			proj.usesIDStaticNPCImmunity = false;
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(SpearType);
		Texture2D texture = TextureAssets.Projectile[SpearType].Value;
		Vector2 origin = texture.Size() * .5f;
		Vector2 drawPos = Projectile.Center - Main.screenPosition;
		Main.EntitySpriteDraw(texture, drawPos, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None);
		return false;
	}
}
public class Roguelike_Spear : SpearReworkProjectile {
	protected override float HoldoutRangeMax => 65;
	protected override float HoldoutRangeMin => -10;
	protected override int SpearType => ProjectileID.Spear;
}
public class SpearThrownProjectile : ModProjectile {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 30;
		Projectile.penetrate = -1;
		Projectile.aiStyle = -1;
		Projectile.alpha = 0;

		Projectile.ownerHitCheck = true;
		Projectile.DamageType = DamageClass.Melee;
		Projectile.tileCollide = true;
		Projectile.friendly = true;
		Projectile.extraUpdates = 5;
		Projectile.timeLeft = 600;
		Projectile.light = 1f;
	}
	protected virtual int SpearType => ProjectileID.Spear;
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(SpearType);
		Texture2D texture = TextureAssets.Projectile[SpearType].Value;
		Vector2 origin = texture.Size() * .5f;
		Vector2 drawPos = Projectile.Center - Main.screenPosition;
		Main.EntitySpriteDraw(texture, drawPos, null, lightColor, Projectile.rotation + MathHelper.PiOver4 + MathHelper.PiOver2, origin, Projectile.scale, SpriteEffects.None);
		return false;
	}
	public override void AI() {
		Projectile.rotation = Projectile.velocity.ToRotation();
		if (++Projectile.ai[0] >= 5) {
			Projectile.ai[0] = 0;
			Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<SpearDust_Extra>());
			dust.noGravity = true;
			dust.velocity = Projectile.velocity * .1f;
			dust.scale = Main.rand.NextFloat(.5f, 2.1f);
			dust.position += Main.rand.NextVector2Circular(15, 15);
		}
	}
}
public class SpearDust_Extra : ModDust {
	public override string Texture => ModUtils.GetVanillaTexture<Projectile>(ProjectileID.PiercingStarlight);
	public override bool Update(Dust dust) {
		Lighting.AddLight(dust.position, dust.color.ToVector3());
		dust.velocity *= .98f;
		dust.scale -= .05f;
		dust.rotation = dust.velocity.ToRotation();
		if (dust.scale <= 0) {
			dust.active = false;
		}
		else {
			dust.position += dust.velocity;
		}
		return false;
	}
	public override bool PreDraw(Dust dust) {
		Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;

		ModUtils.Draw_SetUpToDrawGlowAdditive(Main.spriteBatch);
		Vector2 scaler = new Vector2(dust.scale, dust.scale * .5f);
		Main.spriteBatch.Draw(texture, dust.position - Main.screenPosition, null, dust.color, dust.rotation, texture.Size() * .5f, scaler, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(texture, dust.position - Main.screenPosition, null, Color.White, dust.rotation, texture.Size() * .5f, scaler * .5f, SpriteEffects.None, 0);
		ModUtils.Draw_ResetToNormal(Main.spriteBatch);
		return false;
	}
}
