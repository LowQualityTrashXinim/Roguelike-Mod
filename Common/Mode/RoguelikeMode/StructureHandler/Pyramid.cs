using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Common.RoguelikeMode;
using Roguelike.Common.Systems.ObjectSystem;
using Roguelike.Common.Systems.ObjectSystem.Contents;
using Roguelike.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.Mode.RoguelikeMode.StructureHandler;
internal class Pyramid : ModSystem {
	public HashSet<Point> PyramidModObjectSpawningPoint() {
		return [ new Point(95,17),
new Point(114,17),
new Point(129,30),
new Point(81,30),
new Point(91,29),
new Point(99,29),
new Point(110,29),
new Point(118,29),
new Point(133,44),
new Point(77,44),
new Point(149,58),
new Point(62,58),
new Point(55,73),
new Point(65,73),
new Point(60,73),
new Point(45,73),
new Point(50,73),
new Point(144,73),
new Point(149,73),
new Point(154,73),
new Point(159,73),
new Point(164,73),
new Point(105,88),//Center of bottom floor
new Point(83,92),
new Point(62,92),
new Point(41,92),
new Point(127,92),
new Point(147,92),
new Point(167,92),];
	}
	public Rectangle Pos_Pyramid => ModContent.GetInstance<RogueLikeWorldGen>().Pyramid;
	public bool IsWithinRange = false;
	public override void PostUpdateEverything() {
		Player player = Main.LocalPlayer;
		if (player.Center.IsCloseToPosition(Pos_Pyramid.Center().ToWorldCoordinates(), 1500)) {
			if (!IsWithinRange) {
				HashSet<Point> points = PyramidModObjectSpawningPoint();
				foreach (Point point in points) {
					Vector2 worldPos = (Pos_Pyramid.Location + point).ToWorldCoordinates();
					if (point.X == 105 && point.Y == 88) {
						ModObject.NewModObject(worldPos, Vector2.Zero, ModObject.GetModObjectType<Sealed_SandElemental>());
						continue;
					}
					ModObject.NewModObject(worldPos, Vector2.Zero, ModObject.GetModObjectType<Sealed_SandPoacher>());
				}
			}
			IsWithinRange = true;
		}
		else {
			IsWithinRange = false;
		}
	}
}
public class Sealed_SandPoacher : NPCSealedObject {
	public override int NPCTypeToFollow => NPCID.DesertScorpionWall;
	public override void NPCObject_SetDefaults() {
		frame = Main.npcFrameCount[NPCTypeToFollow];
	}
	Vector2 shaken = Vector2.Zero;
	int counter = 0;
	bool BreakSeal = false;
	public override void Inner_AI() {
		if (!ModContent.GetInstance<Pyramid>().IsWithinRange) {
			Kill();
			return;
		}
		Player player = Main.LocalPlayer;
		shaken = Vector2.Zero;
		if (player.Center.IsCloseToPosition(Center, 50)) {
			shaken += Main.rand.NextVector2CircularEdge(5, 5);
			BreakSeal = true;
		}
		if (BreakSeal) {
			counter++;
		}
		if (counter > 50) {
			SoundEngine.PlaySound(SoundID.Shatter with { Pitch = -1 });
			Kill();
		}
	}
	public override void Inner_Draw(SpriteBatch spritebatch) {
		BasicNPCDrawing(spritebatch);
		BasicSealAuraEffect(spritebatch, counter, shaken, Color.Yellow, 1.5f);
	}
	public override void OnKill() {
		if (BreakSeal) {
			NPC.NewNPCDirect(GetSource_FromThis(), (int)position.X, (int)position.Y, NPCTypeToFollow);
			BreakSeal = false;
		}
	}
}
public class Sealed_SandElemental : NPCSealedObject {
	public override int NPCTypeToFollow => NPCID.SandElemental;
	public override void NPCObject_SetDefaults() {
		frame = Main.npcFrameCount[NPCTypeToFollow];
	}
	Vector2 shaken = Vector2.Zero;
	int counter = 0;
	bool BreakSeal = false;
	public override void Inner_AI() {
		if (!ModContent.GetInstance<Pyramid>().IsWithinRange) {
			Kill();
			return;
		}
		Player player = Main.LocalPlayer;
		shaken = Vector2.Zero;
		if (player.Center.IsCloseToPosition(Center, 50)) {
			shaken += Main.rand.NextVector2CircularEdge(5, 5);
			BreakSeal = true;
		}
		if (BreakSeal) {
			counter++;
			Vector2 pos = Center + Main.rand.NextVector2Circular(50, 50);
			Dust dust = Dust.NewDustDirect(pos, 0, 0, DustID.Sand);
			dust.velocity = (Center - pos).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(5, 10);
			dust.scale += Main.rand.NextFloat(.1f, .5f);
		}
		if (counter > 150) {
			SoundEngine.PlaySound(SoundID.Shatter with { Pitch = -1 });
			Kill();
		}
	}
	public override void Inner_Draw(SpriteBatch spritebatch) {
		BasicNPCDrawing(spritebatch);
		BasicSealAuraEffect(spritebatch, counter, shaken, Color.Yellow, 2.5f);
	}
	public override void OnKill() {
		if (BreakSeal) {
			NPC npc = NPC.NewNPCDirect(GetSource_FromThis(), (int)position.X, (int)position.Y, NPCTypeToFollow);
			BreakSeal = false;
		}
	}
}
