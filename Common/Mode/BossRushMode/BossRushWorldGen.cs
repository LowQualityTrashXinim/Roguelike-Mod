using Roguelike.Common.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Common.Systems;
using Roguelike.Common.Systems.ObjectSystem;
using Roguelike.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
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
			self.ZoneCorrupt = false;
			self.ZoneCrimson = false;
			self.ZoneJungle = false;
			self.ZoneSnow = false;
			self.ZoneHallow = false;
			self.ZoneUnderworldHeight = false;
			self.ZoneBeach = false;
			Tile tileSafely = Framing.GetTileSafely(self.Center);
			if (tileSafely != null)
				self.behindBackWall = tileSafely.WallType > WallID.None;

			if (IsInBiome(self, Bid.Corruption, Room)) {
				self.ZoneCorrupt = true;
			}
			if (IsInBiome(self, Bid.Crimson, Room)) {
				self.ZoneCrimson = true;
			}
			if (IsInBiome(self, Bid.BeeNest, Room)) {
				self.ZoneJungle = true;
				self.ZoneRockLayerHeight = true;
			}
			if (IsInBiome(self, Bid.Tundra, Room)) {
				self.ZoneSnow = true;
			}
			if (IsInBiome(self, Bid.Underworld, Room)) {
				self.ZoneUnderworldHeight = true;
			}
			if (IsInBiome(self, Bid.Jungle, Room)) {
				self.ZoneJungle = true;
				self.ZoneRockLayerHeight = true;
			}
			if (IsInBiome(self, Bid.Hallow, Room)) {
				self.ZoneHallow = true;
			}
			else if (IsInBiome(self, Bid.Ocean, Room)) {
				self.ZoneBeach = true;
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
		public Dictionary<short, List<Rectangle>> Room;
		public static bool IsInBiome(Player player, short BiomeID, Dictionary<short, List<Rectangle>> Room) {
			if (Room == null) {
				return false;
			}
			if (!Room.ContainsKey(BiomeID)) {
				return false;
			}
			List<Rectangle> rectList = Room[BiomeID];
			foreach (Rectangle rect in rectList) {
				if (rect.Contains((int)player.Center.X / 16, (int)player.Center.Y / 16)) {
					return true;
				}
			}
			return false;
		}
		public static bool FindSuitablePlaceToTeleport(Mod mod, Player player, short BiomeID, Dictionary<short, List<Rectangle>> Room) {
			if (Room == null) {
				mod.Logger.Error("Room return null");
				return false;
			}
			if (!Room.ContainsKey(BiomeID)) {
				mod.Logger.Error("Biome id doesn't exist in the dictionary");
				return false;
			}
			if (BiomeID == Bid.Underworld) {
				player.Teleport(new Vector2(RogueLikeWorldGen.GridPart_X * 12f, RogueLikeWorldGen.GridPart_Y * 21.3f).ToWorldCoordinates());
				player.AddBuff(BuffID.Featherfall, ModUtils.ToSecond(2.5f));
				return true;
			}
			List<Rectangle> rect = Room[BiomeID];
			int failsafe = 0;
			while (failsafe <= 9999) {
				Rectangle roomPosition = Main.rand.Next(rect);
				Point position = new Point(
					Main.rand.Next(roomPosition.Left, roomPosition.Right + 1),
					Main.rand.Next(roomPosition.Top, roomPosition.Bottom));
				if (WorldGen.TileEmpty(position.X, position.Y)) {
					int pass = 0;
					for (int offsetX = -1; offsetX <= 1; offsetX++) {
						for (int offsetY = -1; offsetY <= 1; offsetY++) {
							if (offsetX == 0 && offsetY == 0) continue;
							if (WorldGen.TileEmpty(position.X + offsetX, position.Y + offsetY)) {
								pass++;
							}
						}
					}
					if (pass >= 8) {
						player.Teleport(position.ToVector2().ToWorldCoordinates());
						player.AddBuff(BuffID.Featherfall, ModUtils.ToSecond(2.5f));
						return true;
					}
					failsafe++;
				}
			}
			mod.Logger.Error("Fail to find a suitable spot to teleport");
			return false;
		}
		public static bool FindSuitablePlaceToTeleport(Player player, short BiomeID, Dictionary<short, List<Rectangle>> Room) {
			if (Room == null) {
				return false;
			}
			if (!Room.ContainsKey(BiomeID)) {
				return false;
			}
			List<Rectangle> rect = Room[BiomeID];
			if (BiomeID == Bid.Underworld) {
				player.Teleport(new Vector2(RogueLikeWorldGen.GridPart_X * 12f, RogueLikeWorldGen.GridPart_Y * 21.3f).ToWorldCoordinates());
				player.AddBuff(BuffID.Featherfall, ModUtils.ToSecond(2.5f));
				return true;
			}
			int failsafe = 0;
			while (failsafe <= 9999) {
				Rectangle roomPosition = Main.rand.Next(rect);
				Point position = new Point(
					Main.rand.Next(roomPosition.Left, roomPosition.Right + 1),
					Main.rand.Next(roomPosition.Top, roomPosition.Bottom));
				if (WorldGen.TileEmpty(position.X, position.Y)) {
					int pass = 0;
					for (int offsetX = -1; offsetX <= 1; offsetX++) {
						for (int offsetY = -1; offsetY <= 1; offsetY++) {
							if (offsetX == 0 && offsetY == 0) continue;
							if (WorldGen.TileEmpty(position.X + offsetX, position.Y + offsetY)) {
								pass++;
							}
						}
					}
					if (pass >= 8) {
						player.Teleport(position.ToVector2().ToWorldCoordinates());
						player.AddBuff(BuffID.Featherfall, ModUtils.ToSecond(2.5f));
						return true;
					}
					failsafe++;
				}
			}
			return false;
		}
		public bool BossRushWorld = false;
		public override void SaveWorldData(TagCompound tag) {
			tag["BossRushWorld"] = BossRushWorld;
			if (Room == null) {
				return;
			}
			tag["BiomeType"] = Room.Keys.ToList();
			tag["BiomeArea"] = Room.Values.ToList();
		}
		public override void LoadWorldData(TagCompound tag) {
			if (tag.TryGet("BossRushWorld", out bool BossRushMode)) {
				BossRushWorld = BossRushMode;
			}
			var Type = tag.Get<List<short>>("BiomeType");
			var Area = tag.Get<List<List<Rectangle>>>("BiomeArea");
			if (Type == null || Area == null) {
				return;
			}
			Room = Type.Zip(Area, (k, v) => new { Key = k, Value = v }).ToDictionary(x => x.Key, x => x.Value);
		}
	}
	public partial class BossRushWorldGen : ITaskCollection {
		[Task]
		public void SetUp() {
			Room = new Dictionary<short, List<Rectangle>>();
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
		[Task]
		public void Create_Arena() {
			Rectangle rect = GenerationHelper.GridPositionInTheWorld24x24(3, 3, 2, 2);
			StructureData data = Generator.GetStructureData("Assets/StarterArena", Mod);
			Generator.GenerateFromData(data, rect.TopLeft().ToPoint16());
			Main.spawnTileX = rect.X + ((rect.Right - rect.X) / 2);
			Main.spawnTileY = rect.Y + ((rect.Bottom - rect.Y) / 2);
			Room.Add(Bid.Forest, new() { rect });
		}
		[Task]
		public void Create_JungleArena() {
			//StructureData data = Generator.GetStructureData("Assets/StarterArena", Mod);
			//Generator.GenerateFromData(data, rect.TopLeft().ToPoint16());
			//Rectangle rect = GenerationHelper.GridPositionInTheWorld24x24(15, 8, 3, 3);
			//ImageData flesharena = ImageStructureLoader.Get(
			//	ImageStructureLoader.StringBuilder(ImageStructureLoader.JungleArenaVar, 1)
			//	);
			//flesharena.EnumeratePixels((a, b, color) => {
			//	a += rect.X;
			//	b += rect.Y;
			//	if (a > rect.Right || b > rect.Bottom) {
			//		return;
			//	}
			//	GenerationHelper.FastRemoveTile(a, b);
			//	if (color.R == 255 && color.G == 255) {
			//		GenerationHelper.FastPlaceTile(a, b, TileID.Torches);
			//	}
			//	else if (color.R == 254) {
			//		GenerationHelper.FastPlaceTile(a, b, TileID.JungleGrass);
			//	}
			//	else if (color.R == 255) {
			//		GenerationHelper.FastPlaceTile(a, b, TileID.Mud);
			//		GenerationHelper.FastPlaceWall(a, b, WallID.Jungle);
			//	}
			//	else if (color.B == 255) {
			//		GenerationHelper.FastPlaceTile(a, b, TileID.Platforms);
			//	}
			//	GenerationHelper.FastPlaceWall(a, b, WallID.Jungle);
			//});
			//Room.Add(Bid.Jungle, new List<Rectangle> { rect });
		}
		[Task]
		public void Create_BeeNest() {
			Rectangle rect = GenerationHelper.GridPositionInTheWorld24x24(new(15, 12, 150, 100));
			StructureData data = Generator.GetStructureData("Assets/BeeNestArena" + WorldGen.genRand.Next(1, 3), Mod);
			Generator.GenerateFromData(data, rect.TopLeft().ToPoint16());
			Room.Add(Bid.BeeNest, new List<Rectangle> { rect });
		}
		[Task]
		public void Create_TundraArena() {
			Rectangle rect = GenerationHelper.GridPositionInTheWorld24x24(new(11, 10, 150, 100));
			StructureData data = Generator.GetStructureData("Assets/TundraArena1", Mod);
			Generator.GenerateFromData(data, rect.TopLeft().ToPoint16());
			Room.Add(Bid.Tundra, new List<Rectangle> { rect });
		}
		[Task]
		public void Create_CrimsonArena() {
			Rectangle rect = GenerationHelper.GridPositionInTheWorld24x24(new(6, 5, 150, 100));
			StructureData data = Generator.GetStructureData("Assets/CrimsonArena" + WorldGen.genRand.Next(1, 3), Mod);
			Generator.GenerateFromData(data, rect.TopLeft().ToPoint16());
			Room.Add(Bid.Crimson, new List<Rectangle> { rect });
		}
		[Task]
		public void Create_CorruptionArena() {
			Rectangle rect = GenerationHelper.GridPositionInTheWorld24x24(new(10, 5, 150, 100));
			StructureData data = Generator.GetStructureData("Assets/CorruptionArena" + WorldGen.genRand.Next(1, 3), Mod);
			Generator.GenerateFromData(data, rect.TopLeft().ToPoint16());
			Room.Add(Bid.Corruption, new List<Rectangle> { rect });
		}
		//[Task]
		//public void Create_HallowArena() {
		//	Rectangle rect = GenerationHelper.GridPositionInTheWorld24x24(9, 15, 3, 3);
		//	//Generator.GenerateStructure(StringBuilder($"HallowArenaVar{WorldGen.genRand.Next(1, 3)}"), rect.TopLeft().ToPoint16(), Mod);
		//	ImageData arena = ImageStructureLoader.Get(
		//		ImageStructureLoader.StringBuilder(ImageStructureLoader.HallowArena, 1)
		//		);
		//	arena.EnumeratePixels((a, b, color) => {
		//		a += rect.X;
		//		b += rect.Y;
		//		if (a > rect.Right || b > rect.Bottom) {
		//			return;
		//		}
		//		GenerationHelper.FastRemoveTile(a, b);
		//		if (color.R == 255 && color.G == 255 && color.B == 0) {
		//			GenerationHelper.FastPlaceTile(a, b, TileID.Torches);
		//		}
		//		else if (color.R == 255) {
		//			GenerationHelper.FastPlaceTile(a, b, TileID.Pearlstone);
		//		}
		//		else if (color.R == 200 && color.G == 10) {
		//			GenerationHelper.FastPlaceTile(a, b, TileID.HallowedGrass);
		//		}
		//		else if (color.R == 200 && color.G == 100) {
		//			GenerationHelper.FastPlaceTile(a, b, TileID.Dirt);
		//		}
		//		else if (color.B == 255) {
		//			GenerationHelper.FastPlaceTile(a, b, TileID.Platforms);
		//		}
		//		GenerationHelper.FastPlaceWall(a, b, WallID.HallowUnsafe1);
		//	});
		//	Room.Add(Bid.Hallow, new List<Rectangle> { rect });
		//}
		[Task]
		public void Create_DungeonArena() {
			Rectangle rect = GenerationHelper.GridPositionInTheWorld24x24(new(13, 5, 150, 100));
			//Generator.GenerateStructure(StringBuilder($"DungeonArenaVar{WorldGen.genRand.Next(1, 4)}"), rect.TopLeft().ToPoint16(), Mod);
			StructureData data = Generator.GetStructureData("Assets/DungeonArena" + WorldGen.genRand.Next(1, 3), Mod);
			Generator.GenerateFromData(data, rect.TopLeft().ToPoint16());
			Room.Add(Bid.Dungeon, new List<Rectangle> { rect });
		}
		[Task]
		public void Create_SlimeArena() {
			Rectangle rect = GenerationHelper.GridPositionInTheWorld24x24(new(4, 10, 150, 100));
			StructureData data = Generator.GetStructureData("Assets/SlimeArena" + WorldGen.genRand.Next(1, 3), Mod);
			Generator.GenerateFromData(data, rect.TopLeft().ToPoint16());
			Room.Add(Bid.Slime, new List<Rectangle> { rect });
		}
		[Task]
		public void Create_FleshArena() {
			Rectangle rect = GenerationHelper.GridPositionInTheWorld24x24(new(7, 10, 150, 100));
			StructureData data = Generator.GetStructureData("Assets/FleshArena" + WorldGen.genRand.Next(1, 3), Mod);
			Generator.GenerateFromData(data, rect.TopLeft().ToPoint16());
			Room.Add(Bid.FleshRealm, new List<Rectangle> { rect });
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
			Room.Add(Bid.Underworld, new List<Rectangle> { GenerationHelper.GridPositionInTheWorld24x24(0, 20, 24, 3) });
		}
		[Task]
		public void Readjust_Final() {
		}
	}
}
