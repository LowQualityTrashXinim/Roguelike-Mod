using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Common.Systems.ObjectSystem;
using Roguelike.Common.Systems.ObjectSystem.Contents;
using Roguelike.Common.Utils;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.RoguelikeMode.StructureHandler;
internal class KingSlimeStructure : ModSystem {
	public Point Point_ModObject_KingSlime => new Point(40, 29);
	public Rectangle Pos_KSstructure => ModContent.GetInstance<RogueLikeWorldGen>().KingSlimeStructure;
	public bool IsWithinRange = false;
	public override void PostUpdateEverything() {
		if (!ModContent.GetInstance<RogueLikeWorldGen>().RoguelikeWorld) {
			return;
		}
		var player = Main.LocalPlayer;
		if (player.Center.IsCloseToPosition(Pos_KSstructure.Center().ToWorldCoordinates(), 1500)) {
			if (!IsWithinRange) {
				var worldPos = (Pos_KSstructure.Location + Point_ModObject_KingSlime).ToWorldCoordinates();
				ModObject.NewModObject(worldPos, Vector2.Zero, ModObject.GetModObjectType<Sealed_KingSlime>());
			}
			IsWithinRange = true;
		}
		else {
			IsWithinRange = false;
		}
	}
}
public class Fix_SlimeCrown : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.SlimeCrown;
	}
	public override bool CanUseItem(Item item, Player player) {
		return (ModContent.GetInstance<RogueLikeWorldGen>().KingSlimeStructure.Center.ToWorldCoordinates() - player.Center).LengthSquared() <= 360000;
	}
	public override bool? UseItem(Item item, Player player) {
		var spawnPosotion = ModContent.GetInstance<RogueLikeWorldGen>().KingSlimeStructure.Location.ToWorldCoordinates().ToPoint();
		NPC.SpawnBoss(spawnPosotion.X, spawnPosotion.Y, NPCID.KingSlime, player.whoAmI);
		return true;
	}
}
public class Sealed_KingSlime : NPCSealedObject {
	public int AuraCounter = 0;
	public bool Switch = false;
	public override int NPCTypeToFollow => NPCID.KingSlime;
	public override void NPCObject_SetDefaults() {
		frame = 6;
	}
	public override void Inner_AI() {
		if (AuraCounter < 100 && !Switch) {
			AuraCounter++;
		}
		if (AuraCounter > 0 && Switch) {
			AuraCounter--;
		}
		if (AuraCounter >= 100) {
			Switch = true;
		}
		if (AuraCounter <= 0) {
			Switch = false;
		}
		if (++Counter >= 10) {
			frameCounter = ModUtils.Safe_SwitchValue(frameCounter, frame - 1);
			Counter = 0;
		}
		if (NPC.downedSlimeKing || NPC.AnyNPCs(NPCID.KingSlime)) {
			Kill();
		}
	}
	public override void Inner_Draw(SpriteBatch spritebatch) {
		Main.instance.LoadNPC(NPCTypeToFollow);
		var texture = TextureAssets.Npc[NPCTypeToFollow].Value;
		var origin = texture.Size() * .5f;
		var drawpos = position - Main.screenPosition + origin * .75f;
		var color = Color.White;
		frameCounter = Math.Clamp(frameCounter, 0, frame);
		spritebatch.Draw(texture, drawpos, texture.Frame(1, frame, 0, frameCounter), color, 0, origin, 1f, SpriteEffects.None, 1);
		BasicSealAuraEffect(spritebatch, AuraCounter, Vector2.UnitX * origin.X * .75f - Vector2.UnitY * origin.Y / frame * .5f, Color.DodgerBlue, 4f);
	}
}
