using Roguelike.Common.RoguelikeMode.RoguelikeBiome.GeneralGenPassess;
using Roguelike.Common.Utils;
using SubworldLibrary;
using System.Collections.Generic;
using Terraria.IO;
using Terraria.WorldBuilding;

namespace Roguelike.Common.RoguelikeMode.RoguelikeBiome;
internal class Subworld_SlimeWorld : Subworld {
	public override int Width => 2000;
	public override int Height => 800;
	public override List<GenPass> Tasks =>
		new() {
			new GeneralGenPass_PlayerSpawnLocaltion(.05f, .5f),
			new GenPass_SlimeWorldSW("Generating Slime",0),
		};
}
public class GenPass_SlimeWorldSW : GenPass {
	public GenPass_SlimeWorldSW(string name, double loadWeight) : base(name, loadWeight) {
	}

	protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration) {
		GenerationHelper.Create_WorldBiome(2000, 800, RogueLikeWorldGen.dict_BiomeBundle[Bid.Slime]);
	}
}
