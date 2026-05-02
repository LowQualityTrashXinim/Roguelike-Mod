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
	protected virtual float HoldoutRangeMin => 50f;
	protected virtual float HoldoutRangeMax => 200f;
	protected virtual int SpearType => ProjectileID.Spear;
	public override void AI() {
		Player player = Main.player[Projectile.owner];
		int duration = player.itemAnimationMax;
		player.heldProj = Projectile.whoAmI;
		if (Projectile.timeLeft > duration) {
			Projectile.timeLeft = duration;
			Vector2 posOffSet = Projectile.Center.PositionOFFSET(Projectile.velocity, HoldoutRangeMax / 2f);
			Projectile proj;
			float timer = 0;
			for (int i = 0; i < 3; i++) {
				proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), posOffSet + Main.rand.NextVector2Circular(20, 20), Projectile.velocity, ModContent.ProjectileType<SimplePiercingProjectile2>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 15, 5, 20 + i * 3);
				if (proj.ModProjectile is SimplePiercingProjectile2 piercing2) {
					piercing2.ScaleX = 2;
					piercing2.ScaleY = .25f;
					piercing2.ProjectileColor = Color.White;
					piercing2.FollowPlayer = true;
					proj.usesLocalNPCImmunity = true;
					proj.localNPCHitCooldown = -1;
					proj.usesIDStaticNPCImmunity = false;
				}
				timer = 20 + i * 3;
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
		Projectile.velocity = Vector2.Normalize(Projectile.velocity);
		float halfDuration = duration * 0.75f;
		float progress;
		if (Projectile.timeLeft < halfDuration) {
			progress = Projectile.timeLeft / halfDuration;
		}
		else {
			progress = (duration - Projectile.timeLeft) / halfDuration;
		}
		Projectile.Center = player.MountedCenter + Vector2.Lerp(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, ModUtils.InExpo(progress, 11));
		Projectile.rotation = Projectile.velocity.ToRotation();
		Projectile.rotation += Projectile.spriteDirection == -1 ? MathHelper.PiOver4 : MathHelper.PiOver4 + MathHelper.PiOver2;
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
