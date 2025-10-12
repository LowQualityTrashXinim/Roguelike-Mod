using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.NPCs.LootBoxLord.HostileProjectile;
//This code did not follow the above rule and it should be change to follow the above rule
public abstract class BaseHostileProjectile : ModProjectile {
	public sealed override void SetDefaults() {
		Projectile.hostile = true;
		Projectile.friendly = false;
		Projectile.tileCollide = false;
		Projectile.alpha = 255;
		SetHostileDefaults();
	}
	public virtual void SetHostileDefaults() { }
	public override string Texture => ModTexture.MissingTexture_Default;
	public int IDtextureValue = 1;
	int NPC_WhoAmI = -1;
	public bool CanDealContactDamage = true;
	public bool UseProjectileTexture = false;
	public bool DrawRedOutline = true;
	public bool FadewithAlpha = false;
	public bool DisableKillEffect = false;
	public override bool CanHitPlayer(Player target) {
		return CanDealContactDamage;
	}
	public bool IsNPCActive(out NPC npc) {
		npc = null;
		if (NPC_WhoAmI < 0 || NPC_WhoAmI > 255) {
			return false;
		}
		npc = Main.npc[NPC_WhoAmI];
		if (npc.active && npc.life > 0) {
			return true;
		}
		else {
			return false;
		}
	}
	public void SetNPCOwner(int whoAmI) {
		NPC_WhoAmI = whoAmI;
	}
	public override Color? GetAlpha(Color lightColor) {
		if (FadewithAlpha) {
			lightColor = lightColor.ScaleRGB(Projectile.alpha / 255f);
		}
		return lightColor with { A = (byte)Projectile.alpha };
	}
	public virtual void PreDrawDraw(Texture2D texture, Vector2 drawPos, Vector2 origin, ref Color lightColor, out bool DrawOrigin) { DrawOrigin = true; }
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(Projectile.type);
		Texture2D texture;
		if (UseProjectileTexture) {
			texture = ModContent.Request<Texture2D>(ModUtils.GetVanillaTexture<Projectile>(IDtextureValue)).Value;
		}
		else {
			texture = ModContent.Request<Texture2D>(ModUtils.GetVanillaTexture<Item>(IDtextureValue)).Value;
		}
		var origin = texture.Size() * .5f;
		var drawPos = Projectile.position - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
		var effect = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically;
		if (DrawRedOutline) {
			for (int i = 0; i < 3; i++) {
				Main.EntitySpriteDraw(texture, drawPos + new Vector2(2, 2), null, new Color(255, 0, 0, 30), Projectile.rotation, origin, Projectile.scale, effect, 0);
				Main.EntitySpriteDraw(texture, drawPos + new Vector2(-2, 2), null, new Color(255, 0, 0, 30), Projectile.rotation, origin, Projectile.scale, effect, 0);
				Main.EntitySpriteDraw(texture, drawPos + new Vector2(2, -2), null, new Color(255, 0, 0, 30), Projectile.rotation, origin, Projectile.scale, effect, 0);
				Main.EntitySpriteDraw(texture, drawPos + new Vector2(-2, -2), null, new Color(255, 0, 0, 30), Projectile.rotation, origin, Projectile.scale, effect, 0);
			}
		}
		lightColor = Projectile.GetAlpha(lightColor);
		PreDrawDraw(texture, drawPos, origin, ref lightColor, out bool DrawOrigin);
		if (DrawOrigin) {
			Main.EntitySpriteDraw(texture, drawPos, null, lightColor, Projectile.rotation, origin, Projectile.scale, effect, 0);
		}
		return false;
	}
	public sealed override void OnKill(int timeLeft) {
		if (DisableKillEffect) {
			return;
		}
		for (int i = 0; i < 10; i++) {
			var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Smoke);
			dust.noGravity = true;
			dust.velocity = Main.rand.NextVector2CircularEdge(3, 3) * Main.rand.NextFloat(.75f, 1.25f);
			dust.scale = Main.rand.NextFloat(2, 3.5f);
		}
	}
}
public class ProjectileRing : BaseHostileProjectile {
	public override void SetHostileDefaults() {
		DrawRedOutline = false;
		CanDealContactDamage = true;
		FadewithAlpha = true;
		Projectile.timeLeft = 9999;
		Projectile.tileCollide = false;
		Projectile.alpha = 0;
		DisableKillEffect = true;
	}
	Vector2 lastKnownPosition = Vector2.Zero;
	Vector2 NPCtruePos = Vector2.Zero;
	public override void AI() {
		if (IsNPCActive(out NPC npc)) {
			if (Projectile.alpha < 255) {
				Projectile.alpha++;
			}
			NPCtruePos = npc.Center;
			if (lastKnownPosition == Vector2.Zero) {
				lastKnownPosition = npc.Center;
			}
			else {
				Vector2 dis = NPCtruePos - lastKnownPosition;
				lastKnownPosition = lastKnownPosition + dis.SafeNormalize(Vector2.Zero) * dis.Length() / 64f;
			}
		}
		else {
			CanDealContactDamage = false;
			if (Projectile.alpha > 0) {
				Projectile.alpha--;
			}
			if (Projectile.alpha == 0) {
				Projectile.Kill();
			}
		}
		Projectile.Center = lastKnownPosition + Vector2.One.RotatedBy(Projectile.ai[1] + Projectile.timeLeft / 40f) * Projectile.ai[0];
		Projectile.rotation += MathHelper.ToRadians(10);
		if (Projectile.timeLeft <= 100) {
			Projectile.timeLeft += (int)(MathHelper.Pi * 1000);
		}
	}
}
