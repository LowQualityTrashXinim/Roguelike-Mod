using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.NPCs.LootBoxLord.HostileProjectile;
//This code did not follow the above rule and it should be change to follow the above rule
public abstract class BaseProjectile : ModProjectile {
	public sealed override void SetDefaults() {
		Projectile.hostile = false;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.penetrate = -1;
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
	public bool IsNPCActive(out Projectile npc) {
		npc = null;
		if (NPC_WhoAmI < 0 || NPC_WhoAmI > 1000) {
			return false;
		}
		npc = Main.projectile[NPC_WhoAmI];
		if (npc.active && npc.timeLeft > 0) {
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
				Main.EntitySpriteDraw(texture, drawPos + new Vector2(2, 2), null, new Color(0, 255, 0, 30), Projectile.rotation, origin, Projectile.scale, effect, 0);
				Main.EntitySpriteDraw(texture, drawPos + new Vector2(-2, 2), null, new Color(0, 255, 0, 30), Projectile.rotation, origin, Projectile.scale, effect, 0);
				Main.EntitySpriteDraw(texture, drawPos + new Vector2(2, -2), null, new Color(0, 255, 0, 30), Projectile.rotation, origin, Projectile.scale, effect, 0);
				Main.EntitySpriteDraw(texture, drawPos + new Vector2(-2, -2), null, new Color(0, 255, 0, 30), Projectile.rotation, origin, Projectile.scale, effect, 0);
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
