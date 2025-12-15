using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace Roguelike.Common.Systems.ObjectSystem.Contents;
public abstract class NPCSealedObject : ModObject {
	public virtual int NPCTypeToFollow => 0;
	public int Counter = 0;
	public int frame = 1;
	public int frameCounter = 0;
	public sealed override void SetDefaults() {
		timeLeft = 9999;
		NPCObject_SetDefaults();
	}
	public virtual void NPCObject_SetDefaults() { }
	public sealed override void AI() {
		if (NPCTypeToFollow == 0) {
			Kill();
			return;
		}
		timeLeft = 9999;
		Inner_AI();
	}
	public virtual void Inner_AI() { }
	public virtual void Inner_Draw(SpriteBatch spritebatch) { }
	public sealed override void Draw(SpriteBatch spritebatch) {
		if (NPCTypeToFollow == 0) {
			return;
		}
		Inner_Draw(spritebatch);
	}
	public void BasicNPCDrawing(SpriteBatch spritebatch, int framecounter = 1) {
		Main.instance.LoadNPC(NPCTypeToFollow);
		Texture2D texture = TextureAssets.Npc[NPCTypeToFollow].Value;
		Vector2 drawpos = Center - Main.screenPosition;
		Color color = Color.White;
		Rectangle framing = texture.Frame(1, frame, 0, framecounter);
		spritebatch.Draw(texture, drawpos, framing, color, 0, framing.Size() * .5f, 1f, SpriteEffects.None, 1);
	}
	public void BasicSealAuraEffect(SpriteBatch spritebatch, int AuraCounter, Vector2 offset, Color color, float scale = 1f) {
		Texture2D glow = ModContent.Request<Texture2D>(ModTexture.OuterInnerGlow).Value;
		spritebatch.End();
		spritebatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
		float percentalge = Math.Clamp(1 - AuraCounter / 300f, .5f, 1f);
		spritebatch.Draw(glow, position - Main.screenPosition + offset, null, color * percentalge, 0, glow.Size() * .5f, scale + AuraCounter * .01f, SpriteEffects.None, 0);
		spritebatch.End();
		spritebatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
	}
}
