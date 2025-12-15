using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Common.Mode.RoguelikeMode.RoguelikeBiome.GeneralGenPassess;
using Roguelike.Common.RoguelikeMode;
using Roguelike.Common.Subworlds;
using Roguelike.Common.Systems.ObjectSystem;
using Roguelike.Common.Systems.ObjectSystem.Contents;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using SubworldLibrary;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace Roguelike.Common.Mode.RoguelikeMode.RoguelikeBiome;
internal class Subworld_SlimeWorld : Subworld {
	public override int Width => 2000;
	public override int Height => 800;
	public override List<GenPass> Tasks =>
		new() {
			new GeneralGenPass_PlayerSpawnLocaltion(.05f, .5f),
			new GenPass_SlimeWorldSW("Generating Slime",0),
			new GenPass_SlimeWorldBossArena(),
		};
}
public class GenPass_SlimeWorldSW : GenPass {
	public GenPass_SlimeWorldSW(string name, double loadWeight) : base(name, loadWeight) {
	}

	protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration) {
		GenerationHelper.Create_WorldBiome(2000, 800, RogueLikeWorldGen.dict_BiomeBundle[Bid.Slime]);
	}
}

public class GenPass_SlimeWorldBossArena : GenPass {
	public GenPass_SlimeWorldBossArena() : base("Generarting slime area", .01) {
	}
	protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration) {
		Rectangle rect = new Rectangle((int)(Main.maxTilesX * .9f), (int)(Main.maxTilesY * .5f), 150, 150);
		rect = RogueLikeWorldGen.Rect_CentralizeRect(rect);
		ModContent.GetInstance<SlimeWorld_System>().NPCObjectPos = rect.Center;
		for (int i = 0; i < 150; i++) {
			for (int j = 0; j < 150; j++) {
				int x = i + rect.X;
				int y = j + rect.Y;
				Vector2 pos = new(x, y);
				if ((pos - rect.Center()).LengthSquared() <= 5625) {
					GenerationHelper.FastRemoveTile(x, y);
				}
			}
		}
	}
}
public class SlimeWorld_GlobalItem : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.SlimeCrown;
	}
	public override bool CanUseItem(Item item, Player player) {
		return (ModContent.GetInstance<SlimeWorld_System>().NPCObjectPos.ToWorldCoordinates() - player.Center).LengthSquared() <= 360000;
	}
	public override bool? UseItem(Item item, Player player) {
		Point spawnPosotion = ModContent.GetInstance<SlimeWorld_System>().NPCObjectPos;
		NPC.SpawnBoss(spawnPosotion.X * 16, spawnPosotion.Y * 16, NPCID.KingSlime, player.whoAmI);
		return true;
	}
}
public class SlimeWorld_System : ModSystem {
	public Point NPCObjectPos = Point.Zero;
	public override void PostUpdateEverything() {
		if (!SubworldSystem.IsActive<Subworld_SlimeWorld>()) {
			return;
		}
		if (!Main.LocalPlayer.active || NPCObjectPos == Point.Zero) {
			return;
		}
		if (!NPC.downedSlimeKing && !NPC.AnyNPCs(NPCID.KingSlime)) {
			if (!ObjectSystem.AnyModObjects(ModObject.GetModObjectType<KSsealed>())) {
				ModObject.NewModObject(NPCObjectPos.ToVector2() * 16f, Vector2.Zero, ModObject.GetModObjectType<KSsealed>());
			}
		}
	}
}
public class KSsealed : NPCSealedObject {
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
		if (NPC.downedSlimeKing || NPC.AnyNPCs(NPCID.KingSlime) || !SubworldSystem.IsActive<Subworld_SlimeWorld>()) {
			this.Kill();
		}
	}
	public override void Inner_Draw(SpriteBatch spritebatch) {
		Main.instance.LoadNPC(NPCTypeToFollow);
		Texture2D texture = TextureAssets.Npc[NPCTypeToFollow].Value;
		Vector2 origin = texture.Size() * .5f;
		Vector2 drawpos = position - Main.screenPosition + origin * .75f;
		Color color = Color.White;
		frameCounter = Math.Clamp(frameCounter, 0, frame);
		spritebatch.Draw(texture, drawpos, texture.Frame(1, frame, 0, frameCounter), color, 0, origin, 1f, SpriteEffects.None, 1);
		BasicSealAuraEffect(spritebatch, AuraCounter, Vector2.UnitX * origin.X * .75f - Vector2.UnitY * origin.Y / (float)frame * .5f, Color.DodgerBlue, 4f);
	}
}
