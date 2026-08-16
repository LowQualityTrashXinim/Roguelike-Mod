using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.NPCs.EntityOfWeaponry;
internal class GhostWeapon1 : ModNPC {
	public override string Texture => ModTexture.DIAMONDSWOTAFFORB;
	public override void SetStaticDefaults() {
	}
	public override void SetDefaults() {
		NPC.lifeMax = 2000;
		NPC.damage = 100;
		NPC.defense = 20;
		NPC.friendly = false;
		NPC.width = NPC.height = 30;
		NPC.lavaImmune = true;
		NPC.trapImmune = true;
		NPC.knockBackResist = .4f;
		NPC.noTileCollide = true;
		NPC.noGravity = true;
	}
	public override void AI() {
		if (NPC.target == -1 || NPC.target >= 255) {
			NPC.TargetClosest();
			return;
		}
		Player player = Main.player[NPC.target];
		Vector2 towardPlayer = (player.Center - NPC.Center).SafeNormalize(Vector2.Zero);
		if (player.Center.IsCloseToPosition(NPC.Center, 300)) {
			NPC.velocity -= towardPlayer;
		}
		if (!player.Center.IsCloseToPosition(NPC.Center, 500)) {
			NPC.velocity += towardPlayer;
		}
		else {
			NPC.velocity *= .98f;
		}
		NPC.velocity = NPC.velocity.LimitedVelocity(15);
		if (++NPC.ai[0] >= 240) {
			NPC.ai[0] = 0;
			Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, towardPlayer * 5, ModContent.ProjectileType<Hostile_MagicalProjectile>(), NPC.damage, NPC.knockBackResist);
			NPC.velocity -= towardPlayer.Vector2RotateByRandom(10) * 7.5f;
		}
	}
}
public class Hostile_MagicalProjectile : ModProjectile {
	public override string Texture => ModTexture.DIAMONDSWOTAFFORB;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 30;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 600;
		Projectile.penetrate = -1;
		Projectile.friendly = false;
		Projectile.hostile = true;
		Projectile.light = 1f;
	}
	public override void AI() {
		Projectile.velocity *= .99f;
		if (Projectile.timeLeft % 60 == 0) {
			for (int i = 0; i < 100; i++) {
				Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.GemDiamond);
				dust.velocity = Main.rand.NextVector2CircularEdge(5, 5) * Main.rand.NextFloat(.9f, 1.1f);
				dust.scale += Main.rand.NextFloat(.2f, .4f);
				dust.noGravity = true;
			}
			for (int i = 0; i < 12; i++) {
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.One.Vector2DistributeEvenlyPlus(12, 360, i) * 4, ModContent.ProjectileType<Hostile_MagicalProjectile_Small>(), Projectile.damage, Projectile.knockBack);
			}
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(Type);
		Texture2D texture = ModContent.Request<Texture2D>(ModTexture.Glow_Big).Value;
		Vector2 drawPos = Projectile.Center - Main.screenPosition;
		Vector2 origin = texture.Size() * .5f;

		ModUtils.Draw_SetUpToDrawGlowAdditive(Main.spriteBatch);
		Main.EntitySpriteDraw(texture, drawPos, null, Projectile.GetAlpha(lightColor) with { A = 120 }, Projectile.rotation, origin, Projectile.scale + 1, SpriteEffects.None, 0);
		ModUtils.Draw_ResetToNormal(Main.spriteBatch);

		Texture2D texture2 = TextureAssets.Projectile[Type].Value;
		Vector2 origin2 = texture2.Size() * .5f;
		Main.EntitySpriteDraw(texture2, drawPos, null, Projectile.GetAlpha(lightColor) with { A = 0 }, Projectile.rotation, origin2, Projectile.scale, SpriteEffects.None, 0);

		return false;
	}
}
public class Hostile_MagicalProjectile_Small : ModProjectile {
	public override string Texture => ModTexture.SMALLWHITEBALL;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 10;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 120;
		Projectile.penetrate = -1;
		Projectile.friendly = false;
		Projectile.hostile = true;
		Projectile.light = .5f;
	}
}
