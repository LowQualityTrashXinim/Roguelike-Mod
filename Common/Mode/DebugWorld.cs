using Roguelike.Common.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Roguelike.Common;
using Roguelike.Common.General;

namespace Roguelike.Common.Mode;
public partial class DebugWorld : ModSystem {
	public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
		if (ModContent.GetInstance<RogueLikeConfig>().TemplateTest) {
			tasks.ForEach(g => g.Disable());
			tasks.AddRange(((ITaskCollection)this).Tasks);
		}
	}
}
public partial class DebugWorld : ITaskCollection {
	[Task]
	public void SettingUpPlayerSpawn() {
		Main.spawnTileX = 100;
		Main.spawnTileY = 100;
	}
	[Task]
	public void GenerateHorizonTemplate() {
		Rectangle re = GenerationHelper.GridPositionInTheWorld24x24(new(1, 1, 64, 32));
		int X = 0;
		for (int i = 1; i < 10; i++) {
			X = re.Width * i + 10 * i;
			GenerationHelper.PlaceStructure("Template/WG_Template" + "Horizontal" + i, new(re.X + X, re.Y, re.Width, re.Height));
		}
	}

	[Task]
	public void GenerateVerticalTemplate() {
		Rectangle re = GenerationHelper.GridPositionInTheWorld24x24(new(1, 2, 32, 64));
		int X = 0;
		for (int i = 1; i < 10; i++) {
			X = re.Width * i + 10 * i;
			GenerationHelper.PlaceStructure("Template/WG_Template" + "Vertical" + i, new(re.X + X, re.Y, re.Width, re.Height));
		}
	}

	[Task]
	public void GenerateDungeonTemplate_Horizontal() {
		Rectangle re = GenerationHelper.GridPositionInTheWorld24x24(new(1, 3, 64, 32));
		int X = 0;
		for (int i = 1; i < 10; i++) {
			X = re.Width * i + 10 * i;
			GenerationHelper.PlaceStructure("Template/WG_Dungeon_Template" + "Horizontal" + i, new(re.X + X, re.Y, re.Width, re.Height));
		}
	}
	[Task]
	public void GenerateDungeonTemplate_Vertical() {
		Rectangle re = GenerationHelper.GridPositionInTheWorld24x24(new(1, 4, 32, 64));
		int X = 0;
		for (int i = 1; i < 10; i++) {
			X = re.Width * i + 10 * i;
			GenerationHelper.PlaceStructure("Template/WG_Dungeon_Template" + "Vertical" + i, new(re.X + X, re.Y, re.Width, re.Height));
		}
	}
	[Task]
	public void GenerateSpaceTemplate_Horizontal() {
		Rectangle re = GenerationHelper.GridPositionInTheWorld24x24(new(1, 6, 64, 32));
		int X = 0;
		for (int i = 1; i < 10; i++) {
			X = re.Width * i + 10 * i;
			GenerationHelper.PlaceStructure("Template/WG_Space_Template" + "Horizontal" + i, new(re.X + X, re.Y, re.Width, re.Height));
		}
	}
	[Task]
	public void GenerateSpaceTemplate_Vertical() {
		Rectangle re = GenerationHelper.GridPositionInTheWorld24x24(new(1, 7, 32, 64));
		int X = 0;
		for (int i = 1; i < 10; i++) {
			X = re.Width * i + 10 * i;
			GenerationHelper.PlaceStructure("Template/WG_Space_Template" + "Vertical" + i, new(re.X + X, re.Y, re.Width, re.Height));
		}
	}
	[Task]
	public void GenerateTestStructure() {
		Rectangle re = GenerationHelper.GridPositionInTheWorld24x24(new(1, 8, 18, 8));
		int X = 0;
		for (int i = 1; i < 10; i++) {
			X = re.Width * i + 10 * i;
			GenerationHelper.PlaceStructure("Detailed_TestSave", new(re.X + X, re.Y, re.Width, re.Height));
		}
	}
}
