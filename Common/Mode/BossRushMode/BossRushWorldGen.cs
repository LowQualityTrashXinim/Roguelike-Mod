using Roguelike.Common.Utils;
using Microsoft.Xna.Framework;
using Roguelike.Common.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using Terraria.WorldBuilding;
using Roguelike.Common.RoguelikeMode;
using StructureHelper.Models;
using StructureHelper.API;

namespace Roguelike.Common.Mode.BossRushMode {
	public partial class BossRushWorldGen : ModSystem {
		public override void Load() {
			On_Player.UpdateBiomes += On_Player_UpdateBiomes;
			On_Player.ItemCheck_UseBossSpawners += On_Player_ItemCheck_UseBossSpawners;
		}
		private void On_Player_ItemCheck_UseBossSpawners(On_Player.orig_ItemCheck_UseBossSpawners orig, Player self, int onWhichPlayer, Item sItem) {
			if (BossRushWorld) {
				if (sItem.type == ItemID.SlimeCrown) {
					return;
				}
			}
			orig(self, onWhichPlayer, sItem);
		}
		private void On_Player_UpdateBiomes(On_Player.orig_UpdateBiomes orig, Player self) {
			if (!UniversalSystem.CanAccessContent(self, UniversalSystem.BOSSRUSH_MODE) || self.difficulty == PlayerDifficultyID.Creative) {
				orig(self);
				return;
			}
			if (ModContent.GetInstance<UniversalSystem>().ListOfBossKilled.Contains(NPCID.WallofFlesh)) {
				Main.hardMode = true;
			}
		}
		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
			if (!UniversalSystem.CanAccessContent(UniversalSystem.BOSSRUSH_MODE)) {
				return;
			}
			tasks.ForEach(g => g.Disable());
			tasks.AddRange(((ITaskCollection)this).Tasks);
			BossRushWorld = true;
		}
		public bool BossRushWorld = false;
		public override void SaveWorldData(TagCompound tag) {
			tag["BossRushWorld"] = BossRushWorld;
			tag["BossRushStructure"] = BossRushStructure;
		}
		public override void LoadWorldData(TagCompound tag) {
			if (tag.TryGet("BossRushWorld", out bool BossRushMode)) {
				BossRushWorld = BossRushMode;

			}
			if (tag.TryGet("BossRushStructure", out Rectangle BRstructure)) {
				BossRushStructure = BRstructure;
			}
		}
	}
	public partial class BossRushWorldGen : ITaskCollection {
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
		public Rectangle BossRushStructure = new();
		[Task]
		public void Create_Arena() {
			StructureData data = Generator.GetStructureData("Assets/BossRushStructure", Mod);
			Rectangle rect = new Rectangle(Main.spawnTileX - data.width / 2, Main.spawnTileY - data.height / 2, data.width, data.height);
			Generator.GenerateFromData(data, rect.TopLeft().ToPoint16());
			BossRushStructure = rect;
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
}
