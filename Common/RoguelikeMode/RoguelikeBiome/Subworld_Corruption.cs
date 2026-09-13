using Roguelike.Common.Global;
using Roguelike.Common.RoguelikeMode;
using Roguelike.Common.RoguelikeMode.RoguelikeBiome.GeneralGenPassess;
using Roguelike.Common.Utils;
using SubworldLibrary;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.WorldBuilding;

namespace Roguelike.Common.RoguelikeMode.RoguelikeBiome;
internal class Subworld_Corruption : Subworld {
	public override int Width => 800;
	public override int Height => 2000;
	public override List<GenPass> Tasks =>
		new() {
			new GeneralGenPass_PlayerSpawnLocaltion(.5f,.1f),
			new GenPass_CorruptionSW("Generating Corruption",0),
			new GenPass_CorruptionSurface("Generating corrupted surface", 0),
			new GenPass_ApplyingCorruptedStone("Placing stone", 0)
		};

	public override void OnExit() {
		RoguelikeWorldProperty.Set_PlayerLocation(Main.LocalPlayer);
	}
}
public class GenPass_CorruptionSW : GenPass {
	public GenPass_CorruptionSW(string name, double loadWeight) : base(name, loadWeight) {
	}

	protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration) {
		GenerationHelper.Create_WorldBiome(1000, 2500, RogueLikeWorldGen.dict_BiomeBundle[Bid.Corruption]);
	}
}
public class GenPass_CorruptionSurface : GenPass {
	public GenPass_CorruptionSurface(string name, double loadWeight) : base(name, loadWeight) {
	}
	protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration) {
		int PlayerSpawnYPos = Main.spawnTileY;
		for (int i = 0; i < PlayerSpawnYPos; i++) {
			for (int j = 0; j < Main.maxTilesX; j++) {
				if (!WorldGen.InWorld(j, i)) {
					continue;
				}
				Main.tile[j, i].ClearEverything();
			}
		}
		int existingAmount = 200 + WorldGen.genRand.Next(50, 200);
		int attemptToRise = 0;
		for (int x = 0; x < Main.maxTilesX; x++) {
			int amount = existingAmount + PlayerSpawnYPos;
			for (int i = PlayerSpawnYPos; i < amount; i++) {
				if (!WorldGen.InWorld(x, i)) {
					continue;
				}
				if (WorldGen.TileEmpty(x, i - 1)) {
					GenerationHelper.FastPlaceTile(x, i, TileID.CorruptGrass);
				}
				else {
					GenerationHelper.FastPlaceTile(x, i, WorldGen.genRand.NextBool(100) ? TileID.CorruptGrass : TileID.Dirt);
				}
			}
			if (attemptToRise > 0) {
				existingAmount += WorldGen.genRand.Next(0, 5);
				attemptToRise--;
			}
			else {
				existingAmount += WorldGen.genRand.Next(-4, 5);
			}
			if (existingAmount < 50) {
				attemptToRise = 10;
				existingAmount = 50;
			}
		}
	}
}
public class GenPass_ApplyingCorruptedStone : GenPass {
	public GenPass_ApplyingCorruptedStone(string name, double loadWeight) : base(name, loadWeight) {
	}
	protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration) {
		int PlayerSpawnYPos = Main.spawnTileY + 50;
		int FarOffSet = PlayerSpawnYPos + 200;
		int amount = RogueLikeWorldGen.Rand.Next(30, 50);
		for (int i = 0; i < amount; i++) {

			WorldGen.TileRunner(Main.rand.Next(20, Main.maxTilesX - 19), Main.rand.Next(PlayerSpawnYPos, FarOffSet),
				WorldGen.genRand.NextFloat(50), Main.rand.Next(10, 50), TileID.Ebonstone,
				speedX: WorldGen.genRand.NextFloat(),
				speedY: WorldGen.genRand.NextFloat());
		}
	}
}
