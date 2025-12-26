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
		};
}
public class GenPass_SlimeWorldSW : GenPass {
	public GenPass_SlimeWorldSW(string name, double loadWeight) : base(name, loadWeight) {
	}

	protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration) {
		GenerationHelper.Create_WorldBiome(2000, 800, RogueLikeWorldGen.dict_BiomeBundle[Bid.Slime]);
	}
}
