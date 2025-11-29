using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		if (frame < 1) {
			return;
		}
		Inner_Draw(spritebatch);
	}
}
