using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.RoguelikeMode;
using Roguelike.Common.Utils;
using StructureHelper.API;
using StructureHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Roguelike.Common.Systems.ArcadeMode;
public partial class ArcadeModeStructure : ModSystem {
	public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
		if (ModContent.GetInstance<RoguelikeWorldProperty>().ArcadeMode) {
			tasks.ForEach(g => g.Disable());
			tasks.AddRange(((ITaskCollection)this).Tasks);
		}
	}
	public override void SaveWorldData(TagCompound tag) {
		tag["ArcadeStructure"] = ArcadeStructure;
	}
	public override void LoadWorldData(TagCompound tag) {
		if (tag.TryGet("ArcadeStructure", out Rectangle structure)) {
			ArcadeStructure = structure;
		}
	}
}
//Just a straight direct copy of boss rush structure world gen
public partial class ArcadeModeStructure : ITaskCollection {
	[Task]
	public void SetUp() {
		RogueLikeWorldGen.GridPart_X = Main.maxTilesX / 24;//small world : 175
		RogueLikeWorldGen.GridPart_Y = Main.maxTilesY / 24;//small world : 50
		RogueLikeWorldGen.WorldWidthHeight_Ratio = Main.maxTilesX / (float)Main.maxTilesY;
		RogueLikeWorldGen.WorldHeightWidth_Ratio = Main.maxTilesX / (float)Main.maxTilesX;
		Main.worldSurface = (int)(Main.maxTilesY * .22f);
		Main.rockLayer = (int)(Main.maxTilesY * .34f);
		GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(0, 5, 24, 15),
			(i, j) => { GenerationHelper.FastPlaceTile(i, j, TileID.GraniteBlock); });
		WorldGen._genRand = new UnifiedRandom(WorldGen._genRandSeed);
		Main.spawnTileX = Main.maxTilesX / 2;
		Main.spawnTileY = Main.maxTilesY / 2;
	}
	public Rectangle ArcadeStructure = new();
	[Task]
	public void Create_Arena() {
		StructureData data = Generator.GetStructureData("Assets/BossRushStructureV2", Mod);
		Rectangle rect = new Rectangle(Main.spawnTileX - data.width / 2, Main.spawnTileY - data.height / 2, data.width, data.height);
		Generator.GenerateFromData(data, rect.TopLeft().ToPoint16());
		ArcadeStructure = rect;
	}
	[Task]
	public void Create_Hell() {
		GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(0, 20, 24, 4),
		(i, j) => {
			if (j == RogueLikeWorldGen.GridPart_Y * 21f
			|| j == RogueLikeWorldGen.GridPart_Y * 20.5f) {
				GenerationHelper.FastPlaceTile(i, j, TileID.Platforms);
			}
			if (j < RogueLikeWorldGen.GridPart_Y * 21.4f) {
				return;
			}
			if (j == RogueLikeWorldGen.GridPart_Y * 21.4f) {
				GenerationHelper.FastPlaceTile(i, j, TileID.AshGrass);
			}
			else {
				GenerationHelper.FastPlaceTile(i, j, TileID.Ash);
			}
		});
	}
	[Task]
	public void Readjust_Final() {
	}
}
