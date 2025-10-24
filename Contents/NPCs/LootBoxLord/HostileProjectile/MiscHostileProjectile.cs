using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using Terraria;
using Terraria.ID;

namespace Roguelike.Contents.NPCs.LootBoxLord.HostileProjectile;
internal class LBL_HealingProjectile : BaseHostileProjectile {
	public override string Texture => ModTexture.SMALLWHITEBALL;
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 50;
		ProjectileID.Sets.TrailingMode[Type] = 0;
	}
	public override void SetHostileDefaults() {
		Projectile.width = Projectile.height = 10;
		CanDealContactDamage = false;
		DrawRedOutline = false;
		DisableKillEffect = true;
		UseProjectileTexture = true;
		Projectile.extraUpdates = 10;
		Projectile.GetGlobalProjectile<RoguelikeGlobalProjectile>().CanThisProjectileBeDevoured = false;
	}
	public override void AI() {
		if (++Projectile.ai[0] <= 60 * Projectile.extraUpdates) {
			Projectile.velocity *= .98f;
			return;
		}
		if (IsNPCActive(out NPC npc)) {
			Projectile.timeLeft = 2;
			Vector2 distance = npc.Center - Projectile.Center;
			Projectile.velocity = distance.SafeNormalize(Vector2.Zero) * Projectile.ai[1];
			Projectile.ai[1] += .01f;
			if (distance.LengthSquared() <= 100) {
				Projectile.Kill();
				npc.Heal((int)Projectile.ai[2]);
			}
		}
		else {
			Projectile.Kill();
		}
	}
	public override Color? GetAlpha(Color lightColor) {
		return Color.Green with { A = 0 };
	}
	public override void PreDrawDraw(Texture2D texture, Vector2 drawPos, Vector2 origin, ref Color lightColor, out bool DrawOrigin) {
		DrawOrigin = false;
		Projectile.DrawTrailWithoutColorAdjustment(lightColor, .02f);

	}
}
