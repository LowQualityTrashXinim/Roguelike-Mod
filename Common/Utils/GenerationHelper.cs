using Microsoft.Xna.Framework;
using Roguelike.Common.RoguelikeMode;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace Roguelike.Common.Utils;

internal static partial class GenerationHelper {
	public static bool CoordinatesOutOfBounds(int i, int j) => i >= Main.maxTilesX || j >= Main.maxTilesY || i < 0 || j < 0;
	public static void FastPlaceTile(int i, int j, ushort tileType) {
		if (CoordinatesOutOfBounds(i, j) || tileType < 0 || tileType >= TileID.Count) {
			return;
		}

		Tile tile = Main.tile[i, j];
		tile.TileType = tileType;
		tile.Get<TileWallWireStateData>().HasTile = true;
	}

	public static void FastPlaceTile(int i, int j, ushort tileType, byte color) {
		if (CoordinatesOutOfBounds(i, j) || tileType < 0 || tileType >= TileID.Count) {
			return;
		}

		Tile tile = Main.tile[i, j];
		tile.TileType = tileType;
		tile.TileColor = color;
		tile.Get<TileWallWireStateData>().HasTile = true;
	}
	public static void FastPlaceTile(int i, int j, int tileType) {
		FastPlaceTile(i, j, (ushort)tileType);
	}

	public static void FastRemoveTile(int i, int j) {
		if (CoordinatesOutOfBounds(i, j)) {
			return;
		}
		Main.tile[i, j].ClearTile();
		Main.tile[i, j].Get<TileWallWireStateData>().HasTile = false;
	}

	public static void FastPlaceWall(int i, int j, int wallType) {
		if (CoordinatesOutOfBounds(i, j)) {
			return;
		}

		Main.tile[i, j].WallType = (ushort)wallType;
	}

	public static void FastRemoveWall(int i, int j) {
		if (CoordinatesOutOfBounds(i, j)) {
			return;
		}

		Main.tile[i, j].WallType = WallID.None;
	}

	public static void ForEachInRectangle(Rectangle rectangle, Action<int, int> action) {
		for (int i = rectangle.X; i < rectangle.X + rectangle.Width; i++) {
			for (int j = rectangle.Y; j < rectangle.Y + rectangle.Height; j++) {
				action(i, j);
			}
		}
	}

	public static int PointOnRectX(this Rectangle rect, float scale) => (int)(rect.X + rect.Width * scale);
	public static int PointOnRectY(this Rectangle rect, float scale) => (int)(rect.Y + rect.Height * scale);
	public static void ForEachInRectangle(int i, int j, int width, int height, Action<int, int> action) {
		ForEachInRectangle(new Rectangle(i, j, width, height), action);
	}
	public static void ForEachInCircle(int i, int j, int width, int height, Action<int, int> action) {
		ForEachInRectangle(
			i - width / 2,
			j - height / 2,
			width,
			height,
			(iLocal, jLocal) => {
				if (MathF.Pow((iLocal - i) / (width * 0.5f), 2) + MathF.Pow((jLocal - j) / (height * 0.5f), 2) - 1 >= 0) {
					return;
				}

				action(iLocal, jLocal);
			}
		);
	}
	/// <summary>
	/// Use this for easy place tile in the world in 24x24 grid like
	/// </summary>
	/// <param name="x">The starting X position</param>
	/// <param name="y">The Starting Y position</param>
	/// <param name="dragX">Select multiple grid on X axis</param>
	/// <param name="dragY">Select multiple grid on Y axis</param>
	/// <returns></returns>
	public static Rectangle GridPositionInTheWorld24x24(int x, int y, int dragX = 1, int dragY = 1)
		=> new Rectangle(RogueLikeWorldGen.GridPart_X * x, RogueLikeWorldGen.GridPart_Y * y, RogueLikeWorldGen.GridPart_X * dragX, RogueLikeWorldGen.GridPart_Y * dragY);
	/// <summary>
	/// Use this for easy place structure in the world in 24x24 grid like
	/// </summary>
	/// <param name="x">The starting X position</param>
	/// <param name="y">The Starting Y position</param>
	/// <param name="dragX">Select multiple grid on X axis</param>
	/// <param name="dragY">Select multiple grid on Y axis</param>
	/// <returns></returns>
	public static Rectangle GridPositionInTheWorld24x24(Rectangle rect)
		=> new Rectangle(RogueLikeWorldGen.GridPart_X * rect.X, RogueLikeWorldGen.GridPart_Y * rect.Y, rect.Width, rect.Height);
	/// <summary>
	/// Use this for easy place tile in the world in 48x48 grid like
	/// </summary>
	/// <param name="x">The starting X position</param>
	/// <param name="y">The Starting Y position</param>
	/// <param name="dragX">Select multiple grid on X axis</param>
	/// <param name="dragY">Select multiple grid on Y axis</param>
	/// <returns></returns>
	public static Rectangle GridPositionInTheWorld48x48(int x, int y, int dragX = 1, int dragY = 1)
		=> new Rectangle(RogueLikeWorldGen.GridPart_X / 2 * x, RogueLikeWorldGen.GridPart_Y / 2 * y, RogueLikeWorldGen.GridPart_X / 2 * dragX, RogueLikeWorldGen.GridPart_Y / 2 * dragY);
	public static void ForEachInCircle(int i, int j, int radius, Action<int, int> action) {
		ForEachInCircle(i, j, radius * 2, radius * 2, action);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void PlaceStructure(string FileName, Rectangle rect, GenerateStyle style = GenerateStyle.None) {
		List<GenPassData> datalist;
		RogueLikeWorldGenSystem modsystem = ModContent.GetInstance<RogueLikeWorldGenSystem>();
		if (modsystem.dict_Struture.ContainsKey(FileName)) {
			datalist = modsystem.dict_Struture[FileName];
		}
		else {
			Console.WriteLine("Structure not found !");
			return;
		}
		int X = rect.X, Y = rect.Y, offsetY = 0, offsetX = 0, holdX, holdY;

		var collection = CollectionsMarshal.AsSpan(datalist);
		int counter;
		switch (style) {
			case GenerateStyle.None:
				for (int i = 0; i < collection.Length; i++) {
					TileData tiledata = collection[i].tileData;
					counter = collection[i].Count;
					for (int l = 0; l < counter; l++) {
						if (offsetY >= rect.Height) {
							offsetY = 0;
							offsetX++;
						}
						holdX = X + offsetX; holdY = Y + offsetY;
						Structure_PlaceTileXY(holdX, holdY, tiledata);
						offsetY++;
					}
				}
				break;
			case GenerateStyle.FlipHorizon:
				for (int i = 0; i < collection.Length; i++) {
					TileData tiledata = collection[i].tileData;
					counter = collection[i].Count;
					for (int l = counter; l > 0; l--) {
						if (offsetY >= rect.Height) {
							offsetY = 0;
							offsetX++;
						}
						holdX = X + offsetX; holdY = Y + offsetY;
						Structure_PlaceTileXY(holdX, holdY, tiledata);
						offsetY++;
					}
				}
				break;
			case GenerateStyle.FlipVertical:
				for (int i = datalist.Count - 1; i >= 0; i--) {
					TileData tiledata = collection[i].tileData;
					counter = collection[i].Count;
					for (int l = 0; l < counter; l++) {
						if (offsetY >= rect.Height) {
							offsetY = 0;
							offsetX++;
						}
						holdX = X + offsetX; holdY = Y + offsetY;
						Structure_PlaceTileXY(holdX, holdY, tiledata);
						offsetY++;
					}
				}
				break;
			case GenerateStyle.FlipBoth:
				for (int i = datalist.Count - 1; i >= 0; i--) {
					TileData tiledata = collection[i].tileData;
					counter = collection[i].Count;
					for (int l = counter; l > 0; l--) {
						if (offsetY >= rect.Height) {
							offsetY = 0;
							offsetX++;
						}
						holdX = X + offsetX; holdY = Y + offsetY;
						Structure_PlaceTileXY(holdX, holdY, tiledata);
						offsetY++;
					}
				}
				break;
		}
	}
	public static void Structure_PlaceTileXY(int holdX, int holdY, TileData data) {
		if (!data.Tile_Air) {
			data.PlaceTile(holdX, holdY);
		}
		else {
			FastRemoveTile(holdX, holdY);
			Main.tile[holdX, holdY].WallType = data.Tile_WallData;
		}
	}
	public static void Structure_PlaceTile(int holdX, int holdY, ref TileData data) {
		Tile tile = Main.tile[holdX, holdY];
		if (!data.Tile_Air) {
			data.PlaceTile(tile);
		}
		else {
			FastRemoveTile(holdX, holdY);
			tile.WallType = data.Tile_WallData;
		}
	}
	/// <summary>
	/// Offer slightly slower structure placing, but ensure safety<br/>
	/// Use this to place the structure in world gen code
	/// </summary>
	/// <param name="method">the method that was uses to optimize the file</param>
	public static void Safe_PlaceStructure(string FileName, Rectangle rect) {
		List<GenPassData> datalist;
		RogueLikeWorldGenSystem modsystem = ModContent.GetInstance<RogueLikeWorldGenSystem>();
		if (modsystem.dict_Struture.ContainsKey(FileName)) {
			datalist = modsystem.dict_Struture[FileName];
		}
		else {
			Console.WriteLine("Structure not found !");
			return;
		}
		int X = rect.X, Y = rect.Y, offsetY = 0, offsetX = 0, holdX, holdY;

		for (int i = 0; i < datalist.Count; i++) {
			GenPassData gdata = datalist[i];
			for (int l = 0; l < gdata.Count; l++) {
				if (offsetY >= rect.Height) {
					offsetY = 0;
					offsetX++;
				}
				holdX = X + offsetX; holdY = Y + offsetY;
				if (WorldGen.InWorld(holdX, holdY)) {
					Tile tile = Main.tile[holdX, holdY];
					TileData data = gdata.tileData;
					if (!data.Tile_Air) {
						data.PlaceTile(tile);
					}
					else {
						FastRemoveTile(holdX, holdY);
						tile.WallType = data.Tile_WallData;
						if (data.Tile_WireData != 1) {
							data.PlaceWire(holdX, holdY);
						}
					}
				}
				offsetY++;
			}
		}
	}
	/// <summary>
	/// Saves a given region of the world as a structure file
	/// </summary>
	/// <param name="target">The region of the world to save, in tile coordinates</param>
	/// <param name="targetPath">The name of the file to save. Automatically defaults to a file named after the date in the SavedStructures folder</param>
	public static void SaveToFile(Rectangle target, string name = "unnamed structure", SaverOptimizedMethod method = SaverOptimizedMethod.Default) {
		if (name == "")
			name = "unnamed structure";

		string path = Path.Join(Program.SavePathShared, "RogueLikeData");

		if (!Directory.Exists(path))
			Directory.CreateDirectory(path);
		if (method == SaverOptimizedMethod.Default) {
			SaveStructureV2(target, path, name);
		}
		else if (method == SaverOptimizedMethod.Template) {
			SaveTemplateStructure(target, path, name);
		}
		else if (method == SaverOptimizedMethod.MultiStructure) {
			Detailed_SaveStructure(target, path, name);
		}
		Main.NewText("Structure saved as " + Path.Combine(path, name), Color.Yellow);
	}
	private static Dictionary<TileData, char> dict_TileData = new();
	private static readonly HashSet<char> Character = new() { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
	/// <summary>
	/// Attempt to save a structure into a file
	/// </summary>
	/// <param name="target">The region to transform</param>
	/// <param name="path">Path to save</param>
	/// <param name="name">File's name</param>
	public static void SaveStructureV2(Rectangle target, string path, string name) {
		try {
			dict_TileData.Clear();
			int indexCounter = 0;

			Tile outSideLoop = new();
			outSideLoop.TileType = ushort.MaxValue;
			int distance = 0;
			string td = string.Empty;
			string structureData = string.Empty;
			for (int x = target.X; x <= target.X + target.Width; x++) {
				for (int y = target.Y; y <= target.Y + target.Height; y++) {
					//Since this just saving, it is completely fine to be slow
					Tile tile = Framing.GetTileSafely(x, y);
					TileData tileA = new(tile);
					TileData tileB = new(outSideLoop);
					if (distance == 0) {
						outSideLoop = tile;
						distance = 1;
						char c;
						c = Character.ElementAt(indexCounter);
						indexCounter++;
						td += c + tileA.ToString();
						dict_TileData.Add(tileA, c);
						continue;
					}
					if (!tileA.Equals(tileB)) {
						if (dict_TileData.ContainsKey(tileB)) {
							structureData += $"{dict_TileData[tileB]}{distance.ToString()}";
							distance = 1;
						}
						outSideLoop = tile;
						if (indexCounter >= Character.Count) {
							throw new Exception("GenerationHelper.SaveStructureV2.ExceedTileVarityAllowing");
						}
						char c;
						if (dict_TileData.ContainsKey(tileA)) {
							c = dict_TileData[tileA];
						}
						else {
							c = Character.ElementAt(indexCounter);
							td += c + tileA.ToString();
							indexCounter++;
							dict_TileData.Add(tileA, c);
						}
						//string extra = "{" + c.ToString() + 'X' + td.Tile_FrameX + 'Y' + td.Tile_FrameY + "}";
					}
					else {
						distance++;
					}
				}
			}
			if (distance != 0) {
				structureData += $"{dict_TileData[new(outSideLoop)]}{distance}";
			}
			string finalWrapper = $"2{td}>{structureData}";
			using FileStream file = File.Create(Path.Combine(path, name));
			Span<byte> info = new UTF8Encoding(true).GetBytes(finalWrapper);
			file.Write(info);
		}
		catch (Exception ex) {
			Console.WriteLine(ex.ToString());
			throw;
		}
	}
	/// <summary>
	/// Attempt to save a structure into a file
	/// </summary>
	/// <param name="target">The region to transform</param>
	/// <param name="path">Path to save</param>
	/// <param name="name">File's name</param>
	public static void SaveTemplateStructure(Rectangle target, string path, string name) {
		try {
			using FileStream file = File.Create(Path.Combine(path, name));
			using StreamWriter m = new(file);

			Tile outSideLoop = new();
			outSideLoop.TileType = ushort.MaxValue;
			int distance = 0;
			for (int x = target.X; x <= target.X + target.Width; x++) {
				for (int y = target.Y; y <= target.Y + target.Height; y++) {
					//Since this just saving, it is completely fine to be slow
					Tile tile = Framing.GetTileSafely(x, y);
					if (tile.TileType != outSideLoop.TileType && tile.TileType >= TileID.Count) {
						if (distance != 0) {
							m.Write(distance);
						}
						outSideLoop = tile;
						TileData td = new(tile);
						if (td.Tile_Type != 0) {
							td.Tile_Type = TileID.Dirt;
						}
						if (td.Tile_WallData != 0) {
							td.Tile_WallData = WallID.Stone;
						}
						m.Write(td.ToString());
						distance = 1;
					}
					else {
						distance++;
					}
				}
			}
			if (distance != 0) {
				m.Write(distance);
			}
			m.Close();
			file.Close();
		}
		catch (Exception ex) {
			Console.WriteLine(ex.ToString());
			throw;
		}
	}
	public static void Detailed_SaveStructure(Rectangle target, string path, string name) {
		try {
			using FileStream file = File.Create(Path.Combine(path, name));
			using StreamWriter m = new(file);

			for (int x = target.X; x <= target.X + target.Width; x++) {
				for (int y = target.Y; y <= target.Y + target.Height; y++) {
					//Since this just saving, it is completely fine to be slow
					Tile tile = Framing.GetTileSafely(x, y);
					TileData td = new(tile);
					m.Write(td.ToString());
				}
			}
		}
		catch (Exception ex) {
			Console.WriteLine(ex.ToString());
			throw;
		}
	}
	public static void Create_WorldBiome(int width, int height, BiomeDataBundle bundle) {
		Rectangle rect = new(0, 0, width, height);
		string TemplatePath = "Template/WG_Template";
		string horizontal = TemplatePath + "Horizontal";
		string vertical = TemplatePath + "Vertical";
		string file = "";
		Point counter = new Point();
		int additionaloffset = 0;
		bool IsUsingHorizontal;
		int count = 0;
		int offsetcount = 0;
		while (counter.X < rect.Width || counter.Y < rect.Height) {
			if (++additionaloffset >= 2) {
				counter.X += 32;
				additionaloffset = 0;
			}
			IsUsingHorizontal = ++count % 2 == 0;
			Rectangle re = new Rectangle(rect.X + counter.X, rect.Y + counter.Y, 0, 0);
			int X = re.X, Y = re.Y, offsetY = 0, offsetX = 0, holdX, holdY;
			Structure_XinimVer structure = null;
			if (IsUsingHorizontal) {
				re.Width = 64;
				re.Height = 32;
				if (bundle.FormatFile == "") {
					file = horizontal + WorldGen.genRand.Next(1, 10);
				}
				else {
					if (bundle.Range == -1) {
						file = "Template/WG_" + bundle.FormatFile + "_TemplateHorizontal" + WorldGen.genRand.Next(1, 10);
					}
					else {
						file = "Template/WG_" + bundle.FormatFile + "_TemplateHorizontal" + WorldGen.genRand.Next(1, bundle.Range);
					}
				}
				RogueLikeWorldGenSystem modsystem = ModContent.GetInstance<RogueLikeWorldGenSystem>();
				foreach (var item in modsystem.list_Structure) {
					if (item.Get_FilePath == file) {
						structure = item;
					}
				}
				if (structure == null) {
					continue;
				}
			}
			else {
				re.Width = 32;
				re.Height = 64;
				RogueLikeWorldGenSystem modsystem = ModContent.GetInstance<RogueLikeWorldGenSystem>();
				if (bundle.FormatFile == "") {
					file = vertical + WorldGen.genRand.Next(1, 10);
				}
				else {
					if (bundle.Range == -1) {
						file = "Template/WG_" + bundle.FormatFile + "_TemplateVertical" + WorldGen.genRand.Next(1, 10);
					}
					else {
						file = "Template/WG_" + bundle.FormatFile + "_TemplateVertical" + WorldGen.genRand.Next(1, bundle.Range);
					}
				}
				foreach (var item in modsystem.list_Structure) {
					if (item.Get_FilePath == file) {
						structure = item;
					}
				}
				if (structure == null) {
					continue;
				}
			}
			int length;
			int noiseCounter = 0;
			switch (Main.rand.Next(RogueLikeWorldGen.styles)) {
				case GenerateStyle.None:
					length = structure.Get_TotalLength();
					for (int i = 0; i < length; i++) {
						TileData data = structure.Get_CurrentTileData(i);
						if (offsetY >= re.Height) {
							offsetY = 0;
							offsetX++;
						}
						holdX = X + offsetX; holdY = Y + offsetY;
						if (WorldGen.InWorld(holdX, holdY)) {
							Place_Tile_CreateBiome(holdX, holdY, noiseCounter, bundle, ref data);
						}
						offsetY++;
						noiseCounter = ModUtils.Safe_SwitchValue(noiseCounter, RogueLikeWorldGen.StaticNoise255x255.Length - 1);
					}
					break;
				case GenerateStyle.FlipHorizon:
					for (int i = 0; i < structure.Get_Data.Length; i++) {
						GenPassData gdata = structure.Get_Data[i];
						TileData data = gdata.tileData;
						for (int l = gdata.Count; l > 0; l--) {
							if (offsetY >= re.Height) {
								offsetY = 0;
								offsetX++;
							}
							holdX = X + offsetX; holdY = Y + offsetY;
							if (WorldGen.InWorld(holdX, holdY)) {
								Place_Tile_CreateBiome(holdX, holdY, noiseCounter, bundle, ref data);
							}
							offsetY++;
							noiseCounter = ModUtils.Safe_SwitchValue(noiseCounter, RogueLikeWorldGen.StaticNoise255x255.Length - 1);
						}
					}
					break;
				case GenerateStyle.FlipVertical:
					for (int i = structure.Get_Data.Length - 1; i >= 0; i--) {
						GenPassData gdata = structure.Get_Data[i];
						TileData data = gdata.tileData;
						for (int l = 0; l < gdata.Count; l++) {
							if (offsetY >= re.Height) {
								offsetY = 0;
								offsetX++;
							}
							holdX = X + offsetX; holdY = Y + offsetY;
							if (WorldGen.InWorld(holdX, holdY)) {
								Place_Tile_CreateBiome(holdX, holdY, noiseCounter, bundle, ref data);
							}
							offsetY++;
							noiseCounter = ModUtils.Safe_SwitchValue(noiseCounter, RogueLikeWorldGen.StaticNoise255x255.Length - 1);
						}
					}
					break;
				case GenerateStyle.FlipBoth:
					length = structure.Get_TotalLength();
					for (int i = length; i >= 0; i--) {
						TileData data = structure.Get_CurrentTileData(i);
						if (offsetY >= re.Height) {
							offsetY = 0;
							offsetX++;
						}
						holdX = X + offsetX; holdY = Y + offsetY;
						if (WorldGen.InWorld(holdX, holdY)) {
							Place_Tile_CreateBiome(holdX, holdY, noiseCounter, bundle, ref data);
						}
						offsetY++;
						noiseCounter = ModUtils.Safe_SwitchValue(noiseCounter, RogueLikeWorldGen.StaticNoise255x255.Length - 1);
					}
					break;
			}
			if (counter.X < rect.Width) {
				counter.X += re.Width;
			}
			else {
				offsetcount++;
				counter.X = 0 - 32 * offsetcount;
				counter.Y += 32;
				count = 1;
				additionaloffset = -1;
			}
		}
	}
	private static void Place_Tile_CreateBiome(int holdX, int holdY, int noiseCounter, BiomeDataBundle val, ref TileData data) {
		int noiseCounter2nd = ModUtils.Safe_SwitchValue(noiseCounter + 200, RogueLikeWorldGen.StaticNoise255x255.Length - 1);
		if (val.tile2 != ushort.MaxValue && RogueLikeWorldGen.StaticNoise255x255[noiseCounter] && Main.rand.NextFloat() <= val.weight2) {
			data.Tile_Type = val.tile2;
		}
		else {
			data.Tile_Type = val.tile;
		}
		if (RogueLikeWorldGen.StaticNoise255x255[noiseCounter2nd] || Main.rand.NextBool(5)) {
			data.Tile_WallData = val.wall;
		}
		else {
			data.Tile_WallData = WallID.None;
		}
		Structure_PlaceTile(holdX, holdY, ref data);
	}
}
public enum GenerateStyle : byte {
	None,
	FlipHorizon,
	FlipVertical,
	FlipBoth
}
/// <summary>
/// TODO : Implement the below optimize saving method
/// All of these act differently so it is as expected
/// </summary>
public enum SaverOptimizedMethod : byte {
	/// <summary>
	/// The default optimization, it is highly recommend to use this method when you are saving a small building
	/// </summary>
	Default,
	/// <summary>
	/// The default optimization but instead horizontal
	/// </summary>
	HorizontalDefault,
	/// <summary>
	/// This optimization method will save the following structure in rectangle-like<br/>
	/// useful for wanting to save a massive arena structure that have a lot of open space<br/>
	/// Extremely not recommend for massive structure that is very complex
	/// </summary>
	MultiStructure,
	/// <summary>
	/// Default optimization but seperate wall and tile into 2 different field
	/// </summary>
	WallTileSeperate,
	/// <summary>
	/// This is for template kind, which will choose the most less consuming tile data and save it
	/// </summary>
	Template
}
public struct TileData : ICloneable {
	public ushort Tile_Type = 0;
	public short Tile_FrameX = 0;
	public short Tile_FrameY = 0;
	public ushort Tile_WallData = 0;
	public short Tile_WireData = 1;
	public byte Tile_LiquidData = byte.MaxValue;
	public byte Tile_Liquid = 0;
	public bool Tile_HasActuator = false;
	public bool Tile_Air = false;
	public bool Tile_Echo = false;
	public bool Tile_WallEcho = false;
	public SlopeType Tile_Slope = SlopeType.Solid;
	public bool IsATileObject = false;
	public static TileData Default => new();
	public TileData() {
		Tile_Type = 0;
		Tile_FrameX = 0;
		Tile_FrameY = 0;
		Tile_WallData = 0;
		Tile_WireData = 1;
		Tile_LiquidData = byte.MaxValue;
		Tile_Liquid = 0;
		Tile_HasActuator = false;
		Tile_Air = false;
		Tile_Slope = SlopeType.Solid;
		Tile_WallEcho = false;
		Tile_Echo = false;
		IsATileObject = false;
	}
	public char Slope_Parser(SlopeType type) {
		switch (type) {
			case SlopeType.SlopeDownLeft:
				return 'x';
			case SlopeType.SlopeDownRight:
				return 'c';
			case SlopeType.SlopeUpLeft:
				return 'd';
			case SlopeType.SlopeUpRight:
				return 'e';
			default:
				return ' ';
		}
	}
	public SlopeType Slope_Parser(char type) {
		switch (type) {
			case 'x':
				return SlopeType.SlopeDownLeft;
			case 'c':
				return SlopeType.SlopeDownRight;
			case 'd':
				return SlopeType.SlopeUpLeft;
			case 'e':
				return SlopeType.SlopeUpRight;
			default:
				return SlopeType.Solid;
		}
	}
	public TileData(ushort tileType, short tileFrameX, short tileFrameY, ushort tile_WallData, short tile_WireData, bool tile_HasActuator) {
		Tile_Type = tileType;
		Tile_FrameX = tileFrameX;
		Tile_FrameY = tileFrameY;
		Tile_WallData = tile_WallData;
		Tile_WireData = tile_WireData;
		Tile_HasActuator = tile_HasActuator;
	}
	public TileData(Tile tile) {
		if (tile.Get<TileWallWireStateData>().HasTile) {
			Tile_Type = tile.TileType;
			if (tile.TileFrameX != -1) {
				Tile_FrameX = tile.TileFrameX;
			}
			if (tile.TileFrameY != -1) {
				Tile_FrameY = tile.TileFrameY;
			}
		}
		else {
			Tile_Air = true;
		}
		if (tile.WallType != 0) {
			Tile_WallData = tile.WallType;
		}
		if (tile.RedWire) {
			Tile_WireData *= 3;
		}
		if (tile.BlueWire) {
			Tile_WireData *= 5;
		}
		if (tile.YellowWire) {
			Tile_WireData *= 7;
		}
		if (tile.GreenWire) {
			Tile_WireData *= 11;
		}
		if (tile.LiquidAmount != 0) {
			Tile_Liquid = (byte)tile.LiquidType;
			Tile_LiquidData = tile.LiquidAmount;
		}
		if (tile.IsTileInvisible) {
			Tile_Echo = true;
		}
		if (tile.IsWallInvisible) {
			Tile_WallEcho = true;
		}
		if (tile.Slope != SlopeType.Solid) {
			Tile_Slope = tile.Slope;
		}
	}
	/// <summary>
	/// Reverse the ToString back into a tile format
	/// </summary>
	/// <param name="Tile"></param>
	public TileData(string Tile) {
		string NumberString = "";
		char c = ' ';
		for (int i = 0; i < Tile.Length; i++) {
			if (char.IsNumber(Tile[i])) {
				NumberString += Tile[i];
			}
			else {
				if (c != ' ') {
					ParseStringData(c, NumberString);
					NumberString = "";
				}
				c = Tile[i];
			}
		}
		if (c != ' ')
			ParseStringData(c, NumberString);
	}

	private void ParseStringData(char c, string str) {
		switch (c) {
			case 'T':
				Tile_Type = ushort.Parse(str); break;
			case 'X':
				Tile_FrameX = short.Parse(str); break;
			case 'Y':
				Tile_FrameY = short.Parse(str); break;
			case 'W':
				Tile_WallData = ushort.Parse(str); break;
			case 'L':
				Tile_Liquid = byte.Parse(str); break;
			case 'D':
				Tile_LiquidData = byte.Parse(str); break;
			case 'A':
				Tile_HasActuator = true; break;
			case 'N':
				Tile_Air = true; break;
			case 'E':
				Tile_Echo = true; break;
			case 'O':
				Tile_WallEcho = true; break;
		}
		Tile_Slope = Slope_Parser(c);
		Tile_WireData *= ParseStringToWireData(c);

	}
	/// <summary>
	/// Use this method during world gen is advised<br/>
	/// But only if there are actually anything implemented into this method
	/// </summary>
	public void PlaceTile(Tile tile) {
		if (!Tile_Air) {
			tile.TileType = Tile_Type;
			tile.TileFrameX = Tile_FrameX;
			tile.TileFrameY = Tile_FrameY;
			tile.Get<TileWallWireStateData>().HasTile = true;
			tile.IsTileInvisible = Tile_Echo;
			tile.Slope = Tile_Slope;
		}
		tile.WallType = Tile_WallData;
		tile.IsWallInvisible = Tile_WallEcho;
		if (Tile_WireData != 1) {
			if (Tile_WireData % 3 == 0) {
				tile.Get<TileWallWireStateData>().RedWire = true;
			}
			if (Tile_WireData % 5 == 0) {
				tile.Get<TileWallWireStateData>().BlueWire = true;
			}
			if (Tile_WireData % 7 == 0) {
				tile.Get<TileWallWireStateData>().YellowWire = true;
			}
			if (Tile_WireData % 11 == 0) {
				tile.Get<TileWallWireStateData>().GreenWire = true;
			}
		}
	}
	/// <summary>
	/// Use this method during world gen is advised<br/>
	/// But only if there are actually anything implemented into this method
	/// </summary>
	public void PlaceTile(int X, int Y) {
		Tile tile = Main.tile[X, Y];
		if (!Tile_Air) {
			tile.TileType = Tile_Type;
			tile.TileFrameX = Tile_FrameX;
			tile.TileFrameY = Tile_FrameY;
			tile.Get<TileWallWireStateData>().HasTile = true;
			tile.IsTileInvisible = Tile_Echo;
			tile.Slope = Tile_Slope;
		}
		tile.WallType = Tile_WallData;
		tile.IsWallInvisible = Tile_WallEcho;
		if (Tile_WireData != 1) {
			if (Tile_WireData % 3 == 0) {
				WorldGen.PlaceWire(X, Y);
			}
			if (Tile_WireData % 5 == 0) {
				WorldGen.PlaceWire2(X, Y);
			}
			if (Tile_WireData % 7 == 0) {
				WorldGen.PlaceWire3(X, Y);
			}
			if (Tile_WireData % 11 == 0) {
				WorldGen.PlaceWire4(X, Y);
			}
		}
	}
	public void PlaceWire(int X, int Y) {
		if (Tile_WireData != 0) {
			if (Tile_WireData % 3 == 0) {
				WorldGen.PlaceWire(X, Y);
			}
			if (Tile_WireData % 5 == 0) {
				WorldGen.PlaceWire2(X, Y);
			}
			if (Tile_WireData % 7 == 0) {
				WorldGen.PlaceWire3(X, Y);
			}
			if (Tile_WireData % 11 == 0) {
				WorldGen.PlaceWire4(X, Y);
			}
		}
	}
	public override string ToString() {
		StringBuilder sb = new StringBuilder();
		if (Tile_Type < TileID.Count && !Tile_Air) {
			sb.Append('T').Append(Tile_Type);
			sb.Append('X').Append(Tile_FrameX);
			sb.Append('Y').Append(Tile_FrameY);
		}
		else {
			sb.Append('N');
		}
		if (Tile_WallData < WallID.Count) {
			sb.Append('W').Append(Tile_WallData);
		}
		if (Tile_LiquidData < LiquidID.Count && Tile_Liquid > 0) {
			sb.Append('L').Append(Tile_Liquid);
			sb.Append('D').Append(Tile_LiquidData);
		}
		if (Tile_HasActuator) {
			sb.Append('A');
		}
		if (Tile_Echo) {
			sb.Append('E');
		}
		if (Tile_WallEcho) {
			sb.Append('O');
		}
		if (Tile_Slope != SlopeType.Solid) {
			sb.Append(Slope_Parser(Tile_Slope));
		}
		if (Tile_WireData != 0) {
			sb.Append(TileWireToString(Tile_WireData));
		}
		return "{" + sb.ToString() + "}";
	}
	public string TileWireToString(short num) {
		string str = string.Empty;
		if (num % 3 == 0) {
			str += 'r';
		}
		if (num % 5 == 0) {
			str += 'b';
		}
		if (num % 7 == 0) {
			str += 'y';
		}
		if (num % 11 == 0) {
			str += 'g';
		}
		return str;
	}
	public short ParseStringToWireData(char c) {
		return (short)(c == 'r' ? 3 : c == 'b' ? 5 : c == 'y' ? 7 : c == 'g' ? 11 : 1);
	}
	public object Clone() {
		return this.MemberwiseClone();
	}
	public override bool Equals([NotNullWhen(true)] object obj) {
		var TD = (TileData)obj;
		return TD.Tile_Type == this.Tile_Type
			&& TD.Tile_Liquid == this.Tile_Liquid
			&& TD.Tile_LiquidData == this.Tile_LiquidData
			&& TD.Tile_Air == this.Tile_Air
			&& TD.Tile_HasActuator == this.Tile_HasActuator
			&& TD.Tile_WallData == this.Tile_WallData
			&& TD.Tile_WireData == this.Tile_WireData
			&& TD.Tile_Echo == this.Tile_Echo
			&& TD.Tile_Slope == this.Tile_Slope;
	}

	public static bool operator ==(TileData left, TileData right) {
		return left.Equals(right);
	}

	public static bool operator !=(TileData left, TileData right) {
		return !(left == right);
	}

	public override int GetHashCode() {
		return base.GetHashCode();
	}
}
