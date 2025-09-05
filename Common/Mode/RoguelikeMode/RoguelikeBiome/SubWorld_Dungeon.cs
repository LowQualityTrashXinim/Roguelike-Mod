using Roguelike.Common.RoguelikeMode;
using Roguelike.Common.Utils;
using SubworldLibrary;
using System.Collections.Generic;
using Terraria.IO;
using Terraria.WorldBuilding;

namespace Roguelike.Common.Mode.RoguelikeMode.RoguelikeBiome;
internal class SubWorld_Dungeon : Subworld {
	public override int Width => 1000;

	public override int Height => 2500;

	public override List<GenPass> Tasks =>
		new() {
			new GenPass_DungeonSW("Generating dungeon",0)
		};
}
public class GenPass_DungeonSW : GenPass {
	public GenPass_DungeonSW(string name, double loadWeight) : base(name, loadWeight) {
	}

	protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration) {
		GenerationHelper.Create_WorldBiome(1000, 2500, RogueLikeWorldGen.dict_BiomeBundle[Bid.Dungeon]);
	}
}
