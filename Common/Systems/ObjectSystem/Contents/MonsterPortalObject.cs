using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Common.Systems.ObjectSystem.Contents;
public class MonsterPortalObject : ModObject {
	List<int> list_NPCID = new();
	public override void SetDefaults() {
		timeLeft = 9999;
		direction = 1;
		list_NPCID = new();
	}
	public void Add_NPCToList(int[] npcarry) {
		list_NPCID.AddRange(npcarry);
	}
	public bool CloseEnoughToStart = false;
	float ColorSwaping = 0;
	int timer = 0;
	const int maxTimer = 120;
	public override void AI() {
		if (!Main.LocalPlayer.Center.IsCloseToPosition(position, 1500)) {
			timeLeft = 9999;
			return;
		}
		if (direction == 1) {
			timer += direction;
			if (timer >= maxTimer) {
				direction = -1;
			}
		}
		else if (direction == -1) {
			timer += direction;
			if (timer <= 0) {
				direction = 1;
			}
		}
		Lighting.AddLight(position, (Color.Red with { A = 125 }).ToVector3());
		Dust dust = Dust.NewDustDirect(position, 0, 0, ModContent.DustType<Monster_Dust>());
		dust.color = Main.rand.Next([Color.Black, Color.Red]);
		if (dust.color == Color.Red) {
			dust.color = dust.color with { A = 0 };
			dust.velocity = Vector2.UnitX.RotatedBy(MathHelper.TwoPi * Main.rand.NextFloat()) * Main.rand.NextFloat(9, 17);
		}
		else {
			dust.color = dust.color with { A = 100 };
			dust.velocity = Vector2.UnitX.RotatedBy(MathHelper.TwoPi * Main.rand.NextFloat()) * Main.rand.NextFloat(4, 7);
		}
		dust.rotation += Main.rand.NextFloat();
		dust.velocity = Vector2.UnitX.RotatedBy(MathHelper.TwoPi * Main.rand.NextFloat()) * Main.rand.NextFloat(4, 7);
		dust.scale += Main.rand.NextFloat(.5f, .7f) + .5f;
		ColorSwaping = timer / (float)maxTimer;
		rotation += MathHelper.ToRadians(1f);
		if (Main.LocalPlayer.Center.IsCloseToPosition(position, 200)) {
			CloseEnoughToStart = true;
		}
		if (!CloseEnoughToStart) {
			timeLeft = 9999;
		}
		else {
			if (timeLeft % 60 == 0) {
				if (list_NPCID.Count < 1) {
					Kill();
					return;
				}
				int X = (int)position.X;
				int Y = (int)position.Y;
				NPC.NewNPC(Entity.GetSource_NaturalSpawn(), X, Y, list_NPCID[0]);
				list_NPCID.RemoveAt(0);
			}
		}
	}
	public override void Draw(SpriteBatch spritebatch) {
		Texture2D texture = ModContent.Request<Texture2D>(ModTexture.MonsterPortal).Value;
		Vector2 drawpos = position - Main.screenPosition;
		Vector2 origin = texture.Size() * .5f;

		Texture2D glowy = ModContent.Request<Texture2D>(ModTexture.Glow_Big).Value;
		Vector2 originGlow = glowy.Size() * .5f;
		ModUtils.Draw_SetUpToDrawGlowAdditive(Main.spriteBatch);
		Main.EntitySpriteDraw(glowy, drawpos, null, Color.Lerp(Color.OrangeRed, Color.Red, ColorSwaping), rotation, originGlow, 2, SpriteEffects.None);
		ModUtils.Draw_ResetToNormal(Main.spriteBatch);

		Main.EntitySpriteDraw(texture, drawpos, null, Color.Lerp(Color.DarkRed, Color.Black, ColorSwaping) with { A = 50 }, rotation, origin, .75f, SpriteEffects.FlipHorizontally);
		Main.EntitySpriteDraw(texture, drawpos, null, Color.Lerp(Color.OrangeRed, Color.Black, 1 - ColorSwaping), rotation, origin, .5f, SpriteEffects.None);

	}
}
public class Monster_Dust : Roguelike_Dust_ModDust3x3 {
	public override void SetStaticDefaults() {
		RoguelikeGlobalDust.TrailLength[Type] = 30;
	}
	public override bool Update(Dust dust) {
		dust.velocity *= .9f;
		if (dust.color.R == 255) {
			Lighting.AddLight(dust.position, (Color.Red with { A = 25 }).ToVector3());
			dust.velocity = dust.velocity.RotatedBy(MathHelper.ToRadians(2));
		}
		else {
			dust.velocity = dust.velocity.RotatedBy(MathHelper.ToRadians(10));
		}
		dust.scale -= .05f;
		if (dust.scale <= 0) {
			dust.active = false;
		}
		else {
			dust.position += dust.velocity;
		}
		return false;
	}
	public override bool PreDraw(Dust dust) {
		var moddust = dust.Dust_GetDust();
		var texture = Texture2D.Value;
		var origin = texture.Size() * .5f;

		if (moddust == null) {
			return false;
		}
		if (moddust.Dust == null) {
			return false;
		}
		for (int i = 0; i < moddust.oldPos.Length; i++) {
			var drawpos = moddust.oldPos[i] - Main.screenPosition;
			Main.EntitySpriteDraw(texture, drawpos, null, dust.color, moddust.oldRot[i], origin, dust.scale * (1 - i / (float)moddust.oldPos.Length), SpriteEffects.None);
		}
		return false;
	}
}
