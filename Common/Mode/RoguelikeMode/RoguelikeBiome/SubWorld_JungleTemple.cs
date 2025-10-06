﻿using Roguelike.Common.RoguelikeMode;
using Roguelike.Common.Utils;
using SubworldLibrary;
using System.Collections.Generic;
using Terraria.IO;
using Terraria.WorldBuilding;

namespace Roguelike.Common.Mode.RoguelikeMode.RoguelikeBiome;
internal class SubWorld_JungleTemple : Subworld {
	public override int Width => 800;

	public override int Height => 2000;

	public override List<GenPass> Tasks =>
		new() {
			new GenPass_JungleTemple("Generating jungle temple",0)
		};
}
public class GenPass_JungleTemple : GenPass {
	public GenPass_JungleTemple(string name, double loadWeight) : base(name, loadWeight) {
	}

	protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration) {
		GenerationHelper.Create_WorldBiome(1000, 2500, RogueLikeWorldGen.dict_BiomeBundle[Bid.JungleTemple]);
	}
}
