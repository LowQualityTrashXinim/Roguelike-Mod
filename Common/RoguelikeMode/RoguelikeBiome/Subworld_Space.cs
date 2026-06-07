using Microsoft.Xna.Framework;
using Roguelike.Common.RoguelikeMode.RoguelikeBiome.GeneralGenPassess;
using Roguelike.Common.Utils;
using SubworldLibrary;
using System.Collections.Generic;
using Terraria;
using Terraria.IO;
using Terraria.WorldBuilding;

namespace Roguelike.Common.RoguelikeMode.RoguelikeBiome;
internal class Subworld_Space : Subworld {
	public override int Width => 2400;
	public override int Height => 1200;
	public override List<GenPass> Tasks =>
		new() {
			new GeneralGenPass_PlayerSpawnLocaltion(.5f, .8f),
			new GenPass_Space("Generating Space",0),
		};
}
public class GenPass_Space : GenPass {
	public GenPass_Space(string name, double loadWeight) : base(name, loadWeight) {
	}

	protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration) {
		GenerationHelper.Create_WorldBiome(2400, 1200, RogueLikeWorldGen.dict_BiomeBundle[Bid.Space]);
		for (int i = 0; i < 100; i++) {
			int radius = Main.rand.Next(3, 9) * 10;

		}
		int playerX = Main.spawnTileX;
		int playerY = Main.spawnTileY;
		int startX = playerX - 50;
		int startY = playerY - 50;
		Vector2 spawnPos = new Vector2(playerX, playerY);
		int endX = playerX + 100;
		int endY = playerY + 100;
		for (int i = startX; i < endX; i++) {
			for (int j = startY; j < endY; j++) {
				if (spawnPos.IsCloseToPosition(new(i, j), 25)) {
					GenerationHelper.FastRemoveTile(i, j);
				}
			}
		}
	}
}
