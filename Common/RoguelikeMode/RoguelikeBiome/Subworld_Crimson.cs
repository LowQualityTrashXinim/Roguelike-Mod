using Roguelike.Common.RoguelikeMode;
using Roguelike.Common.Utils;
using SubworldLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.IO;
using Terraria.WorldBuilding;

namespace Roguelike.Common.RoguelikeMode.RoguelikeBiome;
internal class Subworld_Crimson : Subworld {
	public override int Width => 800;

	public override int Height => 2000;

	public override List<GenPass> Tasks =>
		new() {
			new GenPass_CrimsonSW("Generating Crimson",0)
		};
}
public class GenPass_CrimsonSW : GenPass {
	public GenPass_CrimsonSW(string name, double loadWeight) : base(name, loadWeight) {
	}

	protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration) {
		GenerationHelper.Create_WorldBiome(1000, 2500, RogueLikeWorldGen.dict_BiomeBundle[Bid.Crimson]);
	}
}
