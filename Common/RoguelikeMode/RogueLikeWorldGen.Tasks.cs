using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Roguelike.Common.Systems;
using Roguelike.Common.Systems.ObjectSystem;
using Roguelike.Common.Utils;
using Roguelike.Common.Wrapper;
using Roguelike.Contents.Items.Lootbox;
using Roguelike.Contents.Items.RelicItem;
using Roguelike.Contents.Items.RelicItem.RelicTemplateContent;
using Roguelike.Texture;
using StructureHelper.API;
using StructureHelper.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Roguelike.Common.RoguelikeMode;
/// <summary>
/// short for Biome Arena ID 
/// </summary>
public class Bid {
	public const short None = 0;
	public const short Forest = 1;
	public const short Jungle = 2;
	public const short Tundra = 3;
	public const short Desert = 4;
	public const short Crimson = 5;
	public const short Corruption = 6;
	public const short Dungeon = 7;
	public const short BlueShroom = 8;
	public const short Granite = 9;
	public const short Marble = 10;
	public const short Slime = 11;
	public const short FleshRealm = 12;
	public const short Beaches = 13;
	public const short Underworld = 14;
	public const short BeeNest = 15;
	public const short Hallow = 16;
	public const short Ocean = 17;
	public const short JungleTemple = 18;
	public const short Space = 19;
	public const short Caven = 20;
	public const short CorruptedTundra = 21;
	public const short CorruptedDesert = 22;
	public const short CrimsonTundra = 23;
	public const short CrimsonDesert = 24;
	//These are biome ignore char value, do not uses this in general biome mapping
	public const short Structure_Dungeon = 996;
	public const short Structure_JungleTemple = 997;
	public const short ShrineOfOffering = 998;
	public const short Advanced = 999;
}
public struct BiomeDataBundle {
	public ushort outlineTile = 0;
	/// <summary>
	/// The main tile that will be used in the tile format
	/// </summary>
	public ushort tile = 0;
	/// <summary>
	/// This is for sub tile in the file format
	/// </summary>
	public ushort tile2 = ushort.MaxValue;
	/// <summary>
	/// Determines the chance of tile2 appearing
	/// </summary>
	public float weight2 = 1;
	public ushort wall = 0;
	public string FormatFile = "";
	public short Range = -1;
	public BiomeDataBundle() {
	}
	public BiomeDataBundle(ushort t, ushort w, string file, short r = -1) {
		outlineTile = t;
		tile = t;
		tile2 = t;
		wall = w;
		FormatFile = file;
		Range = r;
	}
}
public struct MapData {
	public bool ignored = false;
	public short bid = Bid.None;
	public static MapData Default = new(false, 0);
	public MapData() {
	}
	public MapData(bool Ignore) {
		ignored = Ignore;
	}
	public MapData(short BiomeId) {
		bid = BiomeId;
	}
	public MapData(bool Ignore, short BiomeId) {
		ignored = Ignore;
		bid = BiomeId;
	}
}
public partial class RogueLikeWorldGen : ModSystem {
	public static Dictionary<short, string> BiomeID;
	public static TimeSpan WatchTracker = TimeSpan.Zero;
	public static Dictionary<short, BiomeDataBundle> dict_BiomeBundle = new();
	public static Dictionary<short, List<Rectangle>> BiomeZone = new();
	public static Dictionary<short, List<short>> BiomeGroup = new();
	public static bool[] StaticNoise255x255 = new bool[65025];
	public override void OnModLoad() {
		Asset<Texture2D> sprite = ModContent.Request<Texture2D>(ModTexture.CommonTextureStringPattern + "StaticNoise255x255", AssetRequestMode.ImmediateLoad);
		Color[] color = new Color[65025];
		StaticNoise255x255 = new bool[65025];
		Main.RunOnMainThread(() => {
			sprite.Value.GetData(color);
		}).Wait();

		for (int i = 0; i < color.Length; i++) {
			//This is just extra careful
			if (color[i].R == 0 && color[i].G == 0 && color[i].B == 0) {
				StaticNoise255x255[i] = false;
			}
			else {
				StaticNoise255x255[i] = true;
			}
		}
		Main.RunOnMainThread(sprite.Dispose).Wait();

		BiomeID = new();
		FieldInfo[] field = typeof(Bid).GetFields();
		for (int i = 0; i < field.Length; i++) {
			var obj = field[i].GetValue(null);
			if (obj == null) {
				continue;
			}
			if (obj.GetType() != typeof(short)) {
				continue;
			}
			short objvalue = (short)obj;
			BiomeID.Add(objvalue, field[i].Name);
		}
		Biome = new();
		TrialArea = new();
		BiomeZone = new();

		dict_BiomeBundle = new() {
			{ Bid.Forest, new BiomeDataBundle(TileID.Dirt, WallID.Dirt, "") with { outlineTile = TileID.Grass,tile2 = TileID.Stone, weight2 = .67f} },
			{ Bid.Jungle, new BiomeDataBundle(TileID.JungleGrass, WallID.Jungle, "") with { outlineTile = TileID.JungleGrass,tile2 = TileID.Mud, weight2 = .34f} },
			{ Bid.Tundra, new BiomeDataBundle(TileID.SnowBlock, WallID.SnowWallUnsafe, "") with { outlineTile = TileID.SnowBlock,tile2 = TileID.IceBlock, weight2 = .44f} },
			{ Bid.Desert, new(TileID.Sandstone, WallID.Sandstone, "") },
			{ Bid.Corruption, new BiomeDataBundle(TileID.Ebonstone, WallID.EbonstoneUnsafe, "") with { outlineTile = TileID.CorruptGrass } },
			{ Bid.Crimson, new BiomeDataBundle(TileID.Crimstone, WallID.CrimstoneUnsafe, "") with { outlineTile = TileID.CrimsonGrass} },
			{ Bid.Dungeon, new BiomeDataBundle(TileID.BlueDungeonBrick, WallID.BlueDungeon, "Dungeon") },
			{ Bid.Hallow, new  BiomeDataBundle(TileID.HallowedGrass, WallID.HallowedGrassUnsafe, "") with { outlineTile = TileID.HallowedGrass, tile2 = TileID.Dirt} },
			{ Bid.Ocean, new BiomeDataBundle(TileID.Coralstone, WallID.Sandstone, "") },
			{ Bid.Space, new BiomeDataBundle(TileID.Stone, WallID.None, "Space", 18) },
			{ Bid.Caven, new BiomeDataBundle(TileID.Stone, WallID.Stone, "") },
			{ Bid.Underworld, new BiomeDataBundle(TileID.Ash, WallID.None, "") },
			{ Bid.JungleTemple, new BiomeDataBundle(TileID.LihzahrdBrick, WallID.LihzahrdBrickUnsafe, "")},
			{ Bid.Slime, new BiomeDataBundle(TileID.SlimeBlock, WallID.Slime,"")},
			{ Bid.Marble, new BiomeDataBundle(TileID.Marble, WallID.Marble,"")},
			{ Bid.Granite, new BiomeDataBundle(TileID.Granite, WallID.Granite,"")},
			{ Bid.BlueShroom, new BiomeDataBundle(TileID.MushroomGrass, WallID.Mushroom,"") with { outlineTile = TileID.MushroomGrass,tile2 = TileID.Mud, weight2 = .44f } },
			{ Bid.FleshRealm, new BiomeDataBundle(TileID.FleshBlock, WallID.Flesh,"")},
			{ Bid.CorruptedTundra, new BiomeDataBundle(TileID.CorruptIce, WallID.IceUnsafe,"") with { tile2 = TileID.SnowBlock, weight2 = .64f } },
			{ Bid.CorruptedDesert, new BiomeDataBundle(TileID.CorruptHardenedSand, WallID.CorruptHardenedSand,"") with { tile2 = TileID.Sandstone, weight2 = .64f } },
			{ Bid.CrimsonTundra, new BiomeDataBundle(TileID.FleshIce, WallID.IceUnsafe,"") with { tile2 = TileID.SnowBlock, weight2 = .64f } },
			{ Bid.CrimsonDesert, new BiomeDataBundle(TileID.CrimsonHardenedSand, WallID.CrimsonHardenedSand,"") with { tile2 = TileID.Sandstone, weight2 = .64f } },
			{ Bid.None, new BiomeDataBundle()},
		};

		BiomeGroup = new();
	}
	public override void OnModUnload() {
		BiomeID = null;
		TrialArea = null;
		dict_BiomeBundle = null;
		BiomeZone = null;
		BiomeGroup = null;
		StaticNoise255x255 = null;
	}
	public override void PreSaveAndQuit() {
		PlayerPos_WorldCood = Main.LocalPlayer.Center;
	}
	public override void Load() {
	}
	public bool AlreadyGenerated = false;
	public static int GridPart_X = Main.maxTilesX / 24;
	public static int GridPart_Y = Main.maxTilesY / 24;
	public static float WorldWidthHeight_Ratio = Main.maxTilesX / (float)Main.maxTilesY;
	public static float WorldHeightWidth_Ratio = Main.maxTilesX / (float)Main.maxTilesX;
	public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
		if (UniversalSystem.CanAccessContent(UniversalSystem.BOSSRUSH_MODE)) {
			return;
		}
		tasks.ForEach(g => g.Disable());
		tasks.AddRange(((ITaskCollection)this).Tasks);
	}
	public static Dictionary<short, List<Rectangle>> Biome;
	public static List<Rectangle> TrialArea = new();
	public Vector2 PlayerPos_WorldCood = Vector2.Zero;
	public override void SaveWorldData(TagCompound tag) {
		tag["PlayerPos_WorldCood"] = PlayerPos_WorldCood;
		if (Biome == null) {
			return;
		}
		tag["BiomeType"] = Biome.Keys.ToList();
		tag["BiomeArea"] = Biome.Values.ToList();
		tag["TrialArea"] = TrialArea;
		tag["CursedKingdomArea"] = CursedKingdomArea;
		tag["ForestZone"] = ForestZone;
		tag["AlreadyGenerated"] = AlreadyGenerated;
		tag["CrimsonEntrance"] = CrimsonEntrance;
		tag["CorruptionEntrance"] = CorruptionEntrance;
		tag["FleshRealmEntrance"] = FleshRealmEntrance;
		tag["SlimeWorldEntrance"] = SlimeWorldEntrance;
		tag["JungleTempleEntrance"] = JungleTempleEntrance;
		tag["KingSlimeStructure"] = KingSlimeStructure;
		tag["CrimsonStructure"] = CrimsonStructure;
		tag["DesertPyramid"] = Pyramid;
	}
	public override void LoadWorldData(TagCompound tag) {
		PlayerPos_WorldCood = tag.Get<Vector2>("PlayerPos_WorldCood");
		AlreadyGenerated = tag.Get<bool>("AlreadyGenerated");
		var Type = tag.Get<List<short>>("BiomeType");
		var Area = tag.Get<List<List<Rectangle>>>("BiomeArea");
		//Entrance
		SlimeWorldEntrance = tag.Get<Rectangle>("SlimeWorldEntrance");
		CorruptionEntrance = tag.Get<Rectangle>("CorruptionEntrance");
		CrimsonEntrance = tag.Get<Rectangle>("CrimsonEntrance");
		FleshRealmEntrance = tag.Get<Rectangle>("FleshRealmEntrance");
		CursedKingdomArea = tag.Get<Rectangle>("CursedKingdomArea");
		JungleTempleEntrance = tag.Get<Rectangle>("JungleTempleEntrance");
		ForestZone = tag.Get<List<Rectangle>>("ForestZone");
		Pyramid = tag.Get<Rectangle>("DesertPyramid");
		KingSlimeStructure = tag.Get<Rectangle>("KingSlimeStructure");
		CrimsonStructure = tag.Get<Rectangle>("CrimsonStructure");
		if (Type == null || Area == null) {
			return;
		}
		Biome = Type.Zip(Area, (k, v) => new { Key = k, Value = v }).ToDictionary(x => x.Key, x => x.Value);
		TrialArea = tag.Get<List<Rectangle>>("TrialArea");
	}
	public override void PostUpdateEverything() {
		if (!Main.LocalPlayer.active) {
			return;
		}
	}
}
public class EyeCreature : ModObject {
	public override void SetDefaults() {
		timeLeft = 9999;
	}
	public override void AI() {
		timeLeft = 9999;
	}
	public override void Draw(SpriteBatch spritebatch) {
		Texture2D texture = ModContent.Request<Texture2D>(ModTexture.Eye).Value;
		Vector2 origin = texture.Size() * .5f;
		Vector2 drawpos = position - Main.screenPosition;
		spritebatch.Draw(texture, drawpos, null, Color.White, 0, origin, 2, SpriteEffects.None, 1);
		Texture2D texture2 = ModContent.Request<Texture2D>(ModTexture.EyePupil).Value;
		Vector2 origin2 = texture2.Size() * .5f;
		Vector2 drawpos2 = position - Main.screenPosition;
		spritebatch.Draw(texture2, drawpos2, null, Color.White, 0, origin2, 2, SpriteEffects.None, 1);
	}
}
//Full of utils shit here so that the world gen code look less of a cluster fuck
public partial class RogueLikeWorldGen {
	public static readonly GenerateStyle[] styles = [GenerateStyle.None, GenerateStyle.FlipHorizon, GenerateStyle.FlipVertical, GenerateStyle.FlipBoth];
	/// <summary>
	/// Convert from <see cref="Bid"/> biome area id to char type
	/// </summary>
	/// <param name="num"></param>
	/// <returns></returns>
	public static string ToC(int num) {
		if (num < 0 || num >= char.MaxValue) {
			return "" + char.MinValue;
		}
		return "" + (char)num;
	}
	public static short CharToBid(string c, int index = 0) {
		if (string.IsNullOrEmpty(c)) {
			return Bid.None;
		}
		if (c[index] < 0 || c[index] >= char.MaxValue) {
			return Bid.None;
		}
		return (short)c[index];
	}
	public static string GetStringDataBiomeMapping(int x, int y) {
		x = Math.Clamp(x, 0, 23);
		y = Math.Clamp(y, 0, 23);
		string assign = ModContent.GetInstance<RogueLikeWorldGen>().BiomeMapping[x + y * 24];
		if (assign == null) {
			return " ";
		}
		return assign;
	}
	public static StructureData Get_RandomizeAbandonStructure(Mod mod, short b_id = Bid.Forest) {
		StructureData data = Generator.GetStructureData("Assets/ShrineOfOffering", mod);
		string[] arr_stringpath = null;
		switch (b_id) {
			case Bid.Forest:
				arr_stringpath = new string[] {
					"AbandonHouse1",
					"AbandonHouse2",
					"AbandonHouse3",
					"AbandonHouse4",
					"AbandonHouse5",
					"AbandonHouse6",
					"AbandonHouse7",
					"AbandonHouse8",
				};
				data = Generator.GetStructureData($"Assets/{Main.rand.Next(arr_stringpath)}", mod);
				break;
		}
		return data;
	}
	public void ResetTemplate_GenerationValue() {
		rect = new();
		counter = OffSetPoint;
		count = -1;
		IsUsingHorizontal = false;
		offsetcount = 0;
		additionaloffset = -1;
		BiomeZone.Clear();
	}
	public static int MapIndex(int x, int y) => x + y * 24;
	public bool Get_BiomeData(Point positions, int BiomeIndex, out BiomeDataBundle bundle) {
		if (dict_BiomeBundle.TryGetValue(Convert.ToInt16(GetStringDataBiomeMapping(positions.X / GridPart_X, positions.Y / GridPart_Y)[BiomeIndex]), out BiomeDataBundle value1)) {
			bundle = value1;
			return true;
		}
		bundle = new();
		return false;
	}
	private void Place_Tile_CreateBiome(int holdX, int holdY, int noiseCounter, ref TileData data) {
		int noiseCounter2nd = ModUtils.Safe_SwitchValue(noiseCounter + 200, StaticNoise255x255.Length - 1);
		var bundle = dict_BiomeBundle[Arr_ZoneIgnored[holdY * Main.maxTilesX + holdX].bid];
		if (bundle.tile2 != ushort.MaxValue && StaticNoise255x255[noiseCounter] && Rand.NextFloat() <= bundle.weight2) {
			data.Tile_Type = bundle.tile2;
		}
		else {
			data.Tile_Type = bundle.tile;
		}
		if (StaticNoise255x255[noiseCounter2nd]) {
			data.Tile_WallData = bundle.wall;
		}
		else {
			data.Tile_WallData = WallID.None;
		}
		GenerationHelper.Structure_SimplePlaceTile(holdX, holdY, ref data);
	}
	/// <summary>
	/// The position here is tile position not world position
	/// </summary>
	/// <param name="position"></param>
	/// <param name="WorldBiomeIndex"></param>
	/// <returns></returns>
	public static short Get_BiomeIDViaPos(Point position, int WorldBiomeIndex) {
		RogueLikeWorldGen gen = ModContent.GetInstance<RogueLikeWorldGen>();
		Point positionInWorld = new(position.X / GridPart_X, position.Y / GridPart_Y);
		int WorldIndex = MapIndex(positionInWorld.X, positionInWorld.Y);
		if (WorldIndex >= gen.BiomeMapping.Length) {
			return Bid.None;
		}
		string zone = gen.BiomeMapping[WorldIndex];
		return CharToBid(zone, WorldBiomeIndex);
	}
	public static Rectangle Rect_CentralizeRect(int X, int Y, int W, int H) => new(X - W / 2, Y - H / 2, W, H);
	public MapData[] Arr_ZoneIgnored = { };
	public void Set_MapIgnoredZoneIntoWorldGen(int X, int Y, int width, int height) {
		for (int i = 0; i < height; i++) {
			int bound = (Y + i) * Main.maxTilesX + X;
			if (Arr_ZoneIgnored.Length <= bound) {
				continue;
			}
			Array.Fill(Arr_ZoneIgnored, new(true), bound, width);
		}
	}
	public bool Get_MapIgnoredZoneInWorldGen(int X, int Y) => Arr_ZoneIgnored[Y * Main.maxTilesX + X].ignored;
	public void Set_MapIgnoredZoneIntoWorldGen(Rectangle rect) {
		Set_MapIgnoredZoneIntoWorldGen(rect.X, rect.Y, rect.Width, rect.Height);
	}
	public static Rectangle Rect_CentralizeRect(Rectangle rect) {
		rect.X -= rect.Width / 2;
		rect.Y -= rect.Height / 2;
		return rect;
	}
	private void SpawnTownNPC(int type, int X, int Y) {
		X = X * 16;
		Y = Y * 16;
		int whoAmI = NPC.NewNPC(NPC.GetSource_NaturalSpawn(), X, Y, type);
	}
	private bool ValidBiome(int ID) =>
		ID != Bid.Space
		&& ID != Bid.Ocean
		&& ID != Bid.Desert
		&& ID != Bid.Caven
		&& ID != Bid.Underworld
		&& ID != Bid.Marble
		&& ID != Bid.Granite
		&& ID != Bid.Slime
		&& ID != Bid.FleshRealm
		&& ID != Bid.None;
}
//This one is to hold variable that used by the world gen code
public partial class RogueLikeWorldGen {
	//Common used code
	List<Structure_XinimVer> slightlyMoreOptimized = new();
	Structure_XinimVer[] HorizontalTemplate = null;
	Structure_XinimVer[] VerticalTemplate = null;
	private static UnifiedRandom Rand => WorldGen.genRand;
	public static readonly Point OffSetPoint = new Point(-64, -64);
	Rectangle rect = new();
	Point counter = OffSetPoint;
	int count = -1;
	bool IsUsingHorizontal = false;
	int offsetcount = 0;
	int additionaloffset = -1;
	public string[] BiomeMapping = new string[24 * 24];

	/// <summary>
	/// This will make it so that the biome handler system can tell if player are in forest or not
	/// </summary>
	public List<Rectangle> ForestZone = new();

	/// <summary>
	/// Contain list of rectangle for ignoring special zone that should be ingored for place structure<br/>
	/// This serves as another optimization method for placing structure using rectangle aka zone instead of a point<br/>
	/// This method is ideal if you are using StructureHelper.<br/>
	/// Otherwise using <see cref="Arr_ZoneIgnored"/> is much better for optimization
	/// </summary>
	public List<Rectangle> ZoneToBeIgnored = new();

	//Zone Code
	public Rectangle CursedKingdomArea = new Rectangle();
	Rectangle MainForestZone = new Rectangle();
	Rectangle MainTundraForestZone = new Rectangle();
	Rectangle MainJungleForestZone = new Rectangle();
	public Rectangle JungleTempleEntrance = new();

	Rectangle merchantHouse = new();
	Rectangle mechanicHouse = new();
	public Rectangle CrimsonEntrance = new();
	public Rectangle CorruptionEntrance = new();
	public Rectangle FleshRealmEntrance = new();
	public Rectangle SlimeWorldEntrance = new();
	public Rectangle Pyramid = new Rectangle();
	public Rectangle KingSlimeStructure = new Rectangle();
	public Rectangle CrimsonStructure = new Rectangle();
	public Rectangle CorruptStructure = new Rectangle();
	public Rectangle FleshStructure = new Rectangle();
}
public partial class RogueLikeWorldGen : ITaskCollection {
	private void InitializeForestWorld() {
		//Forest spawn zone
		MainForestZone = new(Main.spawnTileX - 200, Main.spawnTileY - 200, 400, 200);
		Set_MapIgnoredZoneIntoWorldGen(MainForestZone);
		ZoneToBeIgnored.Add(MainForestZone);
		//Snow forest zone
		//Finding Suitable index in snow forest
		int xdex = Main.rand.Next(4, 6);
		int ydex = Main.rand.Next(9, 11);
		Point zonecachedPoint = new(GridPart_X * xdex + Main.rand.Next(0, GridPart_X), GridPart_Y * ydex);
		MainTundraForestZone = Rect_CentralizeRect(zonecachedPoint.X, zonecachedPoint.Y, 400, 150);
		ZoneToBeIgnored.Add(MainTundraForestZone);
		Set_MapIgnoredZoneIntoWorldGen(MainTundraForestZone);

		zonecachedPoint = new(GridPart_X * 9, GridPart_Y * 5);
		MainJungleForestZone = Rect_CentralizeRect(zonecachedPoint.X, zonecachedPoint.Y, 400, 150);
		ZoneToBeIgnored.Add(MainJungleForestZone);
		Set_MapIgnoredZoneIntoWorldGen(MainJungleForestZone);

		merchantHouse = new Rectangle(
			GridPart_X * 13 + Main.rand.Next(0, GridPart_X - 100),
			GridPart_Y * 13 + Main.rand.Next(0, GridPart_Y - 50),
			100, 50);
		ZoneToBeIgnored.Add(merchantHouse);
		Set_MapIgnoredZoneIntoWorldGen(merchantHouse);

		mechanicHouse = new Rectangle(
			GridPart_X * 5 + Main.rand.Next(0, GridPart_X - 100),
			GridPart_Y * 7 + Main.rand.Next(0, GridPart_Y - 50),
			100, 50);
		ZoneToBeIgnored.Add(mechanicHouse);
		Set_MapIgnoredZoneIntoWorldGen(mechanicHouse);
		//Set small forest area
		for (int i = 0; i < 60; i++) {
			xdex = Main.rand.Next(1, 23);
			ydex = Main.rand.Next(1, 22);
			short ID = CharToBid(GetStringDataBiomeMapping(xdex, ydex));
			if (!ValidBiome(ID)) {
				i--;
				continue;
			}
			zonecachedPoint = new(GridPart_X * xdex + Main.rand.Next(0, GridPart_X), GridPart_Y * ydex);
			Rectangle re = Rect_CentralizeRect(zonecachedPoint.X, zonecachedPoint.Y, 80, 40);
			bool canBeAdded = true;
			foreach (var item in ZoneToBeIgnored) {
				if (item.Intersects(re)) {
					canBeAdded = false;
					break;
				}
			}
			short centralID = Get_BiomeIDViaPos(re.Center, 0);
			short topleftID = Get_BiomeIDViaPos(re.TopLeft().ToPoint(), 0);
			short toprightID = Get_BiomeIDViaPos(re.TopRight().ToPoint(), 0);
			short bottomleftID = Get_BiomeIDViaPos(re.BottomLeft().ToPoint(), 0);
			short bottomrightID = Get_BiomeIDViaPos(re.BottomRight().ToPoint(), 0);
			if (!canBeAdded || centralID != topleftID || centralID != toprightID || centralID != bottomleftID || centralID != bottomrightID) {
				i--;
				continue;
			}
			ForestZone.Add(re);
			ZoneToBeIgnored.Add(re);
			Set_MapIgnoredZoneIntoWorldGen(re);
		}

	}
	/// <summary>
	/// This code here is pure definition of evil for the sake of "optimization", idfk if this is fast or not<br/>
	/// But don't touch this code<br/>
	/// <br/>
	/// For brief explanation of the code, the <see cref="BiomeMapping"/> is a array of string that store world biome data<br/>
	/// The array of string have a size of 24x24, because we are using grid of 24x24 system<br/>
	/// The biome ID <see cref="Bid"/> is stored as a char data
	/// </summary>
	public void InitializeBiomeWorld() {
		Array.Fill(BiomeMapping, ToC(Bid.None), 0, 24);
		Array.Fill(BiomeMapping, ToC(Bid.None), MapIndex(0, 1), 24);

		//Initialize Space biome
		Array.Fill(BiomeMapping, ToC(Bid.None), MapIndex(0, 2), 6);
		Array.Fill(BiomeMapping, ToC(Bid.None), MapIndex(0, 3), 6);
		Array.Fill(BiomeMapping, ToC(Bid.None), MapIndex(0, 4), 6);
		Array.Fill(BiomeMapping, ToC(Bid.None), MapIndex(0, 5), 4);
		Array.Fill(BiomeMapping, ToC(Bid.None), MapIndex(0, 6), 2);
		Array.Fill(BiomeMapping, ToC(Bid.None), MapIndex(0, 7), 1);
		Array.Fill(BiomeMapping, ToC(Bid.None), MapIndex(11, 1), 5);
		Array.Fill(BiomeMapping, ToC(Bid.None), MapIndex(12, 2), 5);
		Array.Fill(BiomeMapping, ToC(Bid.None), MapIndex(13, 3), 3);
		Array.Fill(BiomeMapping, ToC(Bid.None), MapIndex(6, 2), 6);
		Array.Fill(BiomeMapping, ToC(Bid.None), MapIndex(17, 2), 7);

		//Initialize Hallow biome
		Array.Fill(BiomeMapping, ToC(Bid.Hallow), MapIndex(15, 3), 9);
		Array.Fill(BiomeMapping, ToC(Bid.Hallow), MapIndex(16, 4), 8);
		Array.Fill(BiomeMapping, ToC(Bid.Hallow), MapIndex(20, 5), 4);

		//Initialize Jungle biome
		Array.Fill(BiomeMapping, ToC(Bid.Jungle), MapIndex(6, 3), 7);
		Array.Fill(BiomeMapping, ToC(Bid.Jungle), MapIndex(4, 4), 9);
		Array.Fill(BiomeMapping, ToC(Bid.Jungle), MapIndex(4, 5), 10);
		Array.Fill(BiomeMapping, ToC(Bid.Jungle), MapIndex(7, 6), 9);
		Array.Fill(BiomeMapping, ToC(Bid.Jungle), MapIndex(7, 7), 8);
		Array.Fill(BiomeMapping, ToC(Bid.Jungle), MapIndex(8, 8), 6);
		Array.Fill(BiomeMapping, ToC(Bid.Jungle), MapIndex(8, 9), 7);

		//Initialize Desert biome
		Array.Fill(BiomeMapping, ToC(Bid.Desert), MapIndex(13, 4), 3);
		Array.Fill(BiomeMapping, ToC(Bid.Desert), MapIndex(14, 5), 6);
		Array.Fill(BiomeMapping, ToC(Bid.Desert), MapIndex(16, 6), 8);
		Array.Fill(BiomeMapping, ToC(Bid.Desert), MapIndex(15, 7), 9);
		Array.Fill(BiomeMapping, ToC(Bid.Desert), MapIndex(14, 8), 10);
		Array.Fill(BiomeMapping, ToC(Bid.Desert), MapIndex(15, 9), 3);
		Array.Fill(BiomeMapping, ToC(Bid.Desert), MapIndex(20, 9), 4);
		Array.Fill(BiomeMapping, ToC(Bid.Desert), MapIndex(22, 10), 2);

		//Initialize Crimson desert biome
		Array.Fill(BiomeMapping, ToC(Bid.CrimsonDesert), MapIndex(18, 9), 2);
		BiomeMapping[MapIndex(21, 10)] = ToC(Bid.CrimsonDesert);
		Array.Fill(BiomeMapping, ToC(Bid.CrimsonDesert), MapIndex(22, 11), 2);

		//Initialize Tundra biome
		Array.Fill(BiomeMapping, ToC(Bid.Tundra), MapIndex(2, 6), 5);
		Array.Fill(BiomeMapping, ToC(Bid.Tundra), MapIndex(2, 7), 5);
		Array.Fill(BiomeMapping, ToC(Bid.Tundra), MapIndex(2, 8), 6);
		Array.Fill(BiomeMapping, ToC(Bid.Tundra), MapIndex(2, 9), 6);
		Array.Fill(BiomeMapping, ToC(Bid.Tundra), MapIndex(2, 10), 6);
		Array.Fill(BiomeMapping, ToC(Bid.Tundra), MapIndex(3, 11), 5);
		Array.Fill(BiomeMapping, ToC(Bid.Tundra), MapIndex(3, 12), 4);
		Array.Fill(BiomeMapping, ToC(Bid.Tundra), MapIndex(4, 13), 4);
		Array.Fill(BiomeMapping, ToC(Bid.Tundra), MapIndex(6, 14), 2);
		Array.Fill(BiomeMapping, ToC(Bid.Tundra), MapIndex(5, 15), 4);
		BiomeMapping[MapIndex(8, 16)] = ToC(Bid.Tundra);

		//Initialize Corrupted Tundra
		BiomeMapping[MapIndex(2, 11)] = ToC(Bid.CorruptedTundra);
		BiomeMapping[MapIndex(2, 12)] = ToC(Bid.CorruptedTundra);
		BiomeMapping[MapIndex(3, 13)] = ToC(Bid.CorruptedTundra);
		Array.Fill(BiomeMapping, ToC(Bid.CorruptedTundra), MapIndex(4, 14), 2);
		BiomeMapping[MapIndex(5, 15)] = ToC(Bid.CorruptedTundra);
		BiomeMapping[MapIndex(7, 16)] = ToC(Bid.CorruptedTundra);

		//Initialize Forest biome
		Array.Fill(BiomeMapping, ToC(Bid.Forest), MapIndex(8, 10), 10);
		Array.Fill(BiomeMapping, ToC(Bid.Forest), MapIndex(7, 11), 12);
		Array.Fill(BiomeMapping, ToC(Bid.Forest), MapIndex(6, 12), 13);
		Array.Fill(BiomeMapping, ToC(Bid.Forest), MapIndex(7, 13), 13);
		Array.Fill(BiomeMapping, ToC(Bid.Forest), MapIndex(15, 14), 3);

		//Initialize Ocean biome
		Array.Fill(BiomeMapping, ToC(Bid.Ocean), MapIndex(0, 6), 2);
		Array.Fill(BiomeMapping, ToC(Bid.Ocean), MapIndex(0, 7), 2);
		Array.Fill(BiomeMapping, ToC(Bid.Ocean), MapIndex(0, 8), 2);
		Array.Fill(BiomeMapping, ToC(Bid.Ocean), MapIndex(0, 9), 2);
		Array.Fill(BiomeMapping, ToC(Bid.Ocean), MapIndex(0, 10), 2);
		Array.Fill(BiomeMapping, ToC(Bid.Ocean), MapIndex(0, 11), 2);
		Array.Fill(BiomeMapping, ToC(Bid.Ocean), MapIndex(0, 12), 2);
		Array.Fill(BiomeMapping, ToC(Bid.Ocean), MapIndex(0, 13), 2);
		Array.Fill(BiomeMapping, ToC(Bid.Ocean), MapIndex(0, 14), 2);
		Array.Fill(BiomeMapping, ToC(Bid.Ocean), MapIndex(0, 15), 2);
		Array.Fill(BiomeMapping, ToC(Bid.Ocean), MapIndex(0, 16), 2);
		Array.Fill(BiomeMapping, ToC(Bid.Ocean), MapIndex(0, 17), 2);
		Array.Fill(BiomeMapping, ToC(Bid.Ocean), MapIndex(0, 18), 2);
		Array.Fill(BiomeMapping, ToC(Bid.Ocean), MapIndex(0, 19), 2);
		Array.Fill(BiomeMapping, ToC(Bid.Ocean), MapIndex(1, 20), 1);

		//Initialize Corruption biome
		BiomeMapping[MapIndex(2, 13)] = ToC(Bid.Corruption);
		Array.Fill(BiomeMapping, ToC(Bid.Corruption), MapIndex(2, 14), 2);
		Array.Fill(BiomeMapping, ToC(Bid.Corruption), MapIndex(2, 15), 3);
		Array.Fill(BiomeMapping, ToC(Bid.Corruption), MapIndex(2, 16), 5);
		Array.Fill(BiomeMapping, ToC(Bid.Corruption), MapIndex(2, 17), 7);
		Array.Fill(BiomeMapping, ToC(Bid.Corruption), MapIndex(2, 18), 8);
		Array.Fill(BiomeMapping, ToC(Bid.Corruption), MapIndex(2, 19), 9);
		Array.Fill(BiomeMapping, ToC(Bid.Corruption), MapIndex(2, 20), 8);
		Array.Fill(BiomeMapping, ToC(Bid.Corruption), MapIndex(2, 21), 8);

		//Initialize Caven biome
		Array.Fill(BiomeMapping, ToC(Bid.Caven), MapIndex(8, 14), 7);
		Array.Fill(BiomeMapping, ToC(Bid.Caven), MapIndex(18, 14), 3);
		Array.Fill(BiomeMapping, ToC(Bid.Caven), MapIndex(9, 15), 13);
		Array.Fill(BiomeMapping, ToC(Bid.Caven), MapIndex(9, 16), 11);
		Array.Fill(BiomeMapping, ToC(Bid.Caven), MapIndex(9, 17), 10);
		Array.Fill(BiomeMapping, ToC(Bid.Caven), MapIndex(10, 18), 8);
		Array.Fill(BiomeMapping, ToC(Bid.Caven), MapIndex(11, 19), 7);
		Array.Fill(BiomeMapping, ToC(Bid.Caven), MapIndex(14, 20), 3);

		//Initialize crimson biome
		Array.Fill(BiomeMapping, ToC(Bid.Crimson), MapIndex(18, 10), 3);
		Array.Fill(BiomeMapping, ToC(Bid.Crimson), MapIndex(19, 11), 3);
		Array.Fill(BiomeMapping, ToC(Bid.Crimson), MapIndex(19, 12), 5);
		Array.Fill(BiomeMapping, ToC(Bid.Crimson), MapIndex(20, 13), 4);
		Array.Fill(BiomeMapping, ToC(Bid.Crimson), MapIndex(21, 14), 3);
		Array.Fill(BiomeMapping, ToC(Bid.Crimson), MapIndex(22, 15), 2);
		Array.Fill(BiomeMapping, ToC(Bid.Crimson), MapIndex(20, 16), 4);
		Array.Fill(BiomeMapping, ToC(Bid.Crimson), MapIndex(19, 17), 5);
		Array.Fill(BiomeMapping, ToC(Bid.Crimson), MapIndex(18, 18), 6);
		Array.Fill(BiomeMapping, ToC(Bid.Crimson), MapIndex(18, 19), 4);
		Array.Fill(BiomeMapping, ToC(Bid.Crimson), MapIndex(17, 20), 4);
		Array.Fill(BiomeMapping, ToC(Bid.Crimson), MapIndex(15, 21), 5);

		//Initialize Underworld biome
		BiomeMapping[MapIndex(0, 20)] = ToC(Bid.Underworld);
		Array.Fill(BiomeMapping, ToC(Bid.Underworld), MapIndex(0, 21), 4);
		Array.Fill(BiomeMapping, ToC(Bid.Underworld), MapIndex(9, 21), 6);
		Array.Fill(BiomeMapping, ToC(Bid.Underworld), MapIndex(20, 21), 52);

		//Inintialize Marble biome
		BiomeMapping[MapIndex(7, 6)] = ToC(Bid.Marble);
		Array.Fill(BiomeMapping, ToC(Bid.Marble), MapIndex(6, 7), 3);
		Array.Fill(BiomeMapping, ToC(Bid.Marble), MapIndex(5, 8), 4);
		BiomeMapping[MapIndex(7, 9)] = ToC(Bid.Marble);

		//Initialize Granite biome
		BiomeMapping[MapIndex(9, 7)] = ToC(Bid.Granite);
		Array.Fill(BiomeMapping, ToC(Bid.Granite), MapIndex(8, 8), 2);
		Array.Fill(BiomeMapping, ToC(Bid.Granite), MapIndex(8, 9), 3);
		Array.Fill(BiomeMapping, ToC(Bid.Granite), MapIndex(8, 10), 2);
		BiomeMapping[MapIndex(9, 11)] = ToC(Bid.Granite);

		//Initialize slime world biome
		BiomeMapping[MapIndex(12, 7)] = ToC(Bid.Slime);
		Array.Fill(BiomeMapping, ToC(Bid.Slime), MapIndex(12, 8), 2);
		Array.Fill(BiomeMapping, ToC(Bid.Slime), MapIndex(13, 9), 2);
		Array.Fill(BiomeMapping, ToC(Bid.Slime), MapIndex(13, 10), 2);
		Array.Fill(BiomeMapping, ToC(Bid.Slime), MapIndex(13, 11), 4);
		Array.Fill(BiomeMapping, ToC(Bid.Slime), MapIndex(14, 12), 4);
		Array.Fill(BiomeMapping, ToC(Bid.Slime), MapIndex(14, 13), 3);
		Array.Fill(BiomeMapping, ToC(Bid.Slime), MapIndex(13, 14), 3);
		BiomeMapping[MapIndex(14, 15)] = ToC(Bid.Slime);

		//Initialize glowing mushroom biome
		Array.Fill(BiomeMapping, ToC(Bid.BlueShroom), MapIndex(10, 17), 2);
		Array.Fill(BiomeMapping, ToC(Bid.BlueShroom), MapIndex(9, 18), 7);
		Array.Fill(BiomeMapping, ToC(Bid.BlueShroom), MapIndex(8, 19), 7);
		Array.Fill(BiomeMapping, ToC(Bid.BlueShroom), MapIndex(8, 20), 9);

		Array.Fill(BiomeMapping, ToC(Bid.FleshRealm), MapIndex(22, 19), 2);
		Array.Fill(BiomeMapping, ToC(Bid.FleshRealm), MapIndex(21, 20), 3);

		for (int i = 0; i < BiomeMapping.Length; i++) {
			for (int j = 0; j < GridPart_Y; j++) {
				int bound = i % 24 * GridPart_X + (i / 24 * GridPart_Y + j) * Main.maxTilesX;
				if (Arr_ZoneIgnored.Length <= bound) {
					continue;
				}
				if ((short)BiomeMapping[i][0] == Bid.Underworld
					|| (short)BiomeMapping[i][0] == Bid.None
					|| (short)BiomeMapping[i][0] == Bid.Ocean) {
					Array.Fill(Arr_ZoneIgnored, new MapData(true, CharToBid(BiomeMapping[i])), bound, GridPart_X);
				}
				else {
					Array.Fill(Arr_ZoneIgnored, new MapData(CharToBid(BiomeMapping[i])), bound, GridPart_X);
				}
			}
		}
	}
	private void SetUpCommonTemplate() {
		//This is a hardcoded value of 9 because we only have 9 variants, if in the future there are more to be added
		//Then we will update the below, this will still work regardless of future updated
		//Tho the future common template file won't be added to this optimization, but that is a future problem.
		HorizontalTemplate = new Structure_XinimVer[9];
		VerticalTemplate = new Structure_XinimVer[9];
		byte indexH = 0, indexV = 0;
		RogueLikeWorldGenSystem modsystem = ModContent.GetInstance<RogueLikeWorldGenSystem>();
		slightlyMoreOptimized.AddRange(modsystem.list_Structure);
		//Since this is a 1 time process, we don't want to cache the list below
		//unless somehow in the future this list below is used many time
		List<string> attemptHori = new(){
		"Template/WG_TemplateHorizontal1",
		"Template/WG_TemplateHorizontal2",
		"Template/WG_TemplateHorizontal3",
		"Template/WG_TemplateHorizontal4",
		"Template/WG_TemplateHorizontal5",
		"Template/WG_TemplateHorizontal6",
		"Template/WG_TemplateHorizontal7",
		"Template/WG_TemplateHorizontal8",
		"Template/WG_TemplateHorizontal9",
		};
		List<string> attemptVerti = new() {
		"Template/WG_TemplateVertical1",
		"Template/WG_TemplateVertical2",
		"Template/WG_TemplateVertical3",
		"Template/WG_TemplateVertical4",
		"Template/WG_TemplateVertical5",
		"Template/WG_TemplateVertical6",
		"Template/WG_TemplateVertical7",
		"Template/WG_TemplateVertical8",
		"Template/WG_TemplateVertical9",
		};
		int len = modsystem.list_Structure.Count;
		for (int i = 0; i < len; i++) {
			bool skip = false;
			var structure = modsystem.list_Structure[i];
			if (structure.Get_FilePath == null) {
				continue;
			}
			//We are 100% sure that the string is gonna be Template/WG_ because that is how we format our file
			//So we only need to check if at the position 12 of the file if the char = to "T"
			//because this is how we mark the file is a commonly used template
			if (structure.Get_FilePath[12] != 'T') {
				continue;
			}
			//This below are attempt of smart optimization, to reduce the possible need of iteration,
			//we don't need the array to be sort so as long it is good, we are all happy
			slightlyMoreOptimized.Remove(structure);
			for (int a = 0; a < attemptHori.Count; a++) {
				if (structure.Get_FilePath == attemptHori[a]) {
					HorizontalTemplate[indexH] = structure;
					indexH++;
					attemptHori.RemoveAt(a);
					skip = true;
					break;
				}
			}
			if (skip) continue;
			for (int a = 0; a < attemptVerti.Count; a++) {
				if (structure.Get_FilePath == attemptVerti[a]) {
					VerticalTemplate[indexV] = structure;
					indexV++;
					attemptVerti.RemoveAt(a);
					break;
				}
			}

		}
	}
	[Task]
	public void ResetValue() {
		ZoneToBeIgnored.Clear();
		ForestZone.Clear();
		CursedKingdomArea = new Rectangle();
		MainForestZone = new Rectangle();
		MainTundraForestZone = new Rectangle();
		WatchTracker = TimeSpan.Zero;
		Biome = new();
		GridPart_X = Main.maxTilesX / 24;
		GridPart_Y = Main.maxTilesY / 24;
		Array.Resize(ref Arr_ZoneIgnored, Main.maxTilesX * Main.maxTilesY);
		Array.Fill(Arr_ZoneIgnored, MapData.Default);
		FieldInfo[] field = typeof(Main).GetFields();
		foreach (var item in field) {
			if (item.IsStatic && item.Name == "UnderworldLayer") {
				item.SetValue(null, Main.maxTilesY);
			}
		}
		Main.spawnTileX = GridPart_X * 11;
		Main.spawnTileY = GridPart_Y * 14;
		WorldWidthHeight_Ratio = Main.maxTilesX / (float)Main.maxTilesY;
		WorldHeightWidth_Ratio = Main.maxTilesX / (float)Main.maxTilesX;
	}
	/// <summary>
	/// This is mainly for initialization, so it is safe to use in anywhere that need initialize of the world
	/// </summary>
	[Task]
	public void SetUp() {
		Stopwatch watch = new();
		watch.Start();
		ResetTemplate_GenerationValue();
		InitializeBiomeWorld();
		InitializeForestWorld();
		SetUpCommonTemplate();
		watch.Stop();
		Mod.Logger.Info("Setup step: " + watch.ToString());
	}
	[Task]
	public void Generate_Secret() {
		Stopwatch watch = new();
		watch.Start();
		List<Item> itemlist = new();
		int amount = Rand.Next(3, 6);
		for (int i = 0; i < amount; i++) {
			Item item_relic = new Item(ModContent.ItemType<Relic>());
			Relic relic = item_relic.ModItem as Relic;
			relic.AddRelicTemplate(Main.LocalPlayer, RelicTemplate.GetRelicType<GenericTemplate>(), 1.1f);
			relic.AddRelicTemplate(Main.LocalPlayer, RelicTemplate.GetRelicType<GenericTemplate>(), 1.1f);
			relic.AddRelicTemplate(Main.LocalPlayer, RelicTemplate.GetRelicType<GenericTemplate>(), 1.1f);
			itemlist.Add(item_relic);
		}
		amount = Rand.Next(1, 4);
		for (int i = 0; i < amount; i++) {
			Item potion = new Item(Rand.Next(TerrariaArrayID.SpecialPotion));
			itemlist.Add(potion);
		}
		int X = 22 * GridPart_X + Rand.Next(0, GridPart_X);
		int Y = 21 * GridPart_Y + Rand.Next(0, GridPart_Y);
		GeneralWorldGenTask.Generate_Container(Rand, Mod, X, Y, out Rectangle re, itemlist);
		ZoneToBeIgnored.Add(re);
		Set_MapIgnoredZoneIntoWorldGen(re);
		watch.Stop();
		Mod.Logger.Info("Secret step: " + watch.ToString());
	}
	[Task]
	public void Generate_KingSlimeStructure() {
		Stopwatch watch = new();
		watch.Start();
		int X = 15 * GridPart_X + Main.rand.Next(GridPart_X);
		int Y = 12 * GridPart_Y + Main.rand.Next(GridPart_Y);
		var data = ModWrapper.Get_StructureData("Assets/SlimeChamber", Mod);
		int Width = data.width / 2;
		int Height = data.height / 2;
		Point16 point = new(X - Width, Y - Height);
		KingSlimeStructure = new(point.X, point.Y, data.width, data.height);
		if (ModWrapper.IsInBound(data, point)) {
			ModWrapper.GenerateFromData(data, point);
			ZoneToBeIgnored.Add(KingSlimeStructure);
			Set_MapIgnoredZoneIntoWorldGen(KingSlimeStructure);
		}
		watch.Stop();
		Mod.Logger.Info("King Slime structure realm step: " + watch.ToString());
	}
	[Task]
	public void Generate_BoCStructure() {
		Stopwatch watch = new();
		watch.Start();
		int X = 22 * GridPart_X + Main.rand.Next(GridPart_X);
		int Y = 12 * GridPart_Y + Main.rand.Next(GridPart_Y);
		var data = ModWrapper.Get_StructureData("Assets/CrimsonChamber", Mod);
		int Width = data.width / 2;
		int Height = data.height / 2;
		Point16 point = new(X - Width, Y - Height);
		CrimsonStructure = new(point.X, point.Y, data.width, data.height);
		if (ModWrapper.IsInBound(data, point)) {
			ModWrapper.GenerateFromData(data, point);
			ZoneToBeIgnored.Add(CrimsonStructure);
			Set_MapIgnoredZoneIntoWorldGen(CrimsonStructure);
		}
		watch.Stop();
		Mod.Logger.Info("Crimson structure realm step: " + watch.ToString());
	}
	[Task]
	public void Generate_EoWStructure() {
		Stopwatch watch = new();
		watch.Start();
		int X = 6 * GridPart_X + Main.rand.Next(GridPart_X);
		int Y = 19 * GridPart_Y + Main.rand.Next(GridPart_Y);
		var data = ModWrapper.Get_StructureData("Assets/CorruptChamber", Mod);
		int Width = data.width / 2;
		int Height = data.height / 2;
		Point16 point = new(X - Width, Y - Height);
		CorruptStructure = new(point.X, point.Y, data.width, data.height);
		if (ModWrapper.IsInBound(data, point)) {
			ModWrapper.GenerateFromData(data, point);
			ZoneToBeIgnored.Add(CorruptStructure);
			Set_MapIgnoredZoneIntoWorldGen(CorruptStructure);
		}
		watch.Stop();
		Mod.Logger.Info("Corrupt Chamber structure realm step: " + watch.ToString());
	}
	[Task]
	public void Generate_FleshStructure() {
		Stopwatch watch = new();
		watch.Start();
		int X = 22 * GridPart_X + Main.rand.Next(GridPart_X);
		int Y = 19 * GridPart_Y + Main.rand.Next(GridPart_Y);
		var data = ModWrapper.Get_StructureData("Assets/FleshChamber", Mod);
		int Width = data.width / 2;
		int Height = data.height / 2;
		Point16 point = new(X - Width, Y - Height);
		FleshStructure = new(point.X, point.Y, data.width, data.height);
		if (ModWrapper.IsInBound(data, point)) {
			ModWrapper.GenerateFromData(data, point);
			ZoneToBeIgnored.Add(FleshStructure);
			Set_MapIgnoredZoneIntoWorldGen(FleshStructure);
		}
		watch.Stop();
		Mod.Logger.Info("Flesh chamber structure realm step: " + watch.ToString());
	}
	[Task]
	public void Generate_StarterForest() {
		Stopwatch watch = new();
		watch.Start();
		Rectangle forestArea = MainForestZone;
		//We are using standard generation for this one
		int startingPoint = forestArea.Height - forestArea.Height / 8 + forestArea.Y;
		int offsetRaise = 0;
		bool MoveToNextX;
		float left = Rand.NextFloat(.25f, .35f);
		float right = Rand.NextFloat(.75f, .85f);
		int CurrentPosY;
		int LeftPos = forestArea.X + (int)(forestArea.Width * left);
		int RightPos = forestArea.X + (int)(forestArea.Width * right);
		int datawidth = 20;
		for (int i = forestArea.X; i < forestArea.Width + forestArea.X; i++) {
			MoveToNextX = true;
			if (Main.spawnTileX == i) {
				Main.spawnTileY = startingPoint - offsetRaise - 3;
				StructureData data = Generator.GetStructureData("Assets/ShrineOfOffering", Mod);
				Generator.GenerateStructure("Assets/ShrineOfOffering", new(i - data.width / 2, forestArea.Y + forestArea.Height / 2), Mod);
				Rectangle zone = new Rectangle(i - data.width / 2, forestArea.Y + forestArea.Height / 2, data.width, data.height);
				BiomeZone.TryAdd(Bid.ShrineOfOffering, new() { zone });
			}
			for (int j = startingPoint; j < forestArea.Height + forestArea.Y; j++) {
				if (MoveToNextX) {
					if (Rand.NextBool(5)) {
						offsetRaise += Rand.Next(-1, 2);
					}
					MoveToNextX = false;
					j = startingPoint - offsetRaise;
					GenerationHelper.FastPlaceTile(i, j, TileID.Grass);
					continue;
				}
				GenerationHelper.FastPlaceTile(i, j, TileID.Dirt);
			}
			CurrentPosY = startingPoint - offsetRaise;
			if (i == LeftPos) {
				StructureData data = Generator.GetStructureData("Assets/StarterHouse1", Mod);
				Generator.GenerateFromData(data, new(i - data.width / 2, CurrentPosY - data.height + 3));
			}
			else if (i == RightPos) {
				StructureData data = Generator.GetStructureData("Assets/StarterHouse2", Mod);
				Generator.GenerateFromData(data, new(i - data.width / 2, CurrentPosY - data.height + 3));
			}
			else if (Rand.NextBool(26)
				&& i < LeftPos - datawidth || i > LeftPos + datawidth
				&& i < RightPos - datawidth || i > RightPos + datawidth) {
				WorldGen.GrowTree(i, CurrentPosY);
			}
		}
		watch.Stop();
		Mod.Logger.Info("Forest step: " + watch.ToString());
	}
	[Task]
	public void Generate_SnowForest() {
		Stopwatch watch = new();
		watch.Start();
		Rectangle forestArea = MainTundraForestZone;
		//We are using standard generation for this one
		int startingPoint = forestArea.Height - forestArea.Height / 8 + forestArea.Y;
		int offsetRaise = 0;
		bool MoveToNextX;
		float left = Rand.NextFloat(.25f, .35f);
		float right = Rand.NextFloat(.75f, .85f);
		int CurrentPosY;
		int LeftPos = forestArea.X + (int)(forestArea.Width * left);
		int RightPos = forestArea.X + (int)(forestArea.Width * right);
		int datawidth = 20;
		for (int i = forestArea.X; i < forestArea.Width + forestArea.X; i++) {
			MoveToNextX = true;
			for (int j = startingPoint; j < forestArea.Height + forestArea.Y; j++) {
				if (MoveToNextX) {
					if (Rand.NextBool(5)) {
						offsetRaise += Rand.Next(-1, 2);
					}
					MoveToNextX = false;
					j = startingPoint - offsetRaise;
					GenerationHelper.FastPlaceTile(i, j, TileID.SnowBlock);
					continue;
				}
				if (Rand.NextFloat() <= .45f) {
					GenerationHelper.FastPlaceTile(i, j, TileID.IceBlock);
				}
				else {
					GenerationHelper.FastPlaceTile(i, j, TileID.SnowBlock);
				}
			}
			CurrentPosY = startingPoint - offsetRaise;
			if (Rand.NextBool(26)
				&& i < LeftPos - datawidth || i > LeftPos + datawidth
				&& i < RightPos - datawidth || i > RightPos + datawidth) {
				WorldGen.GrowTree(i, CurrentPosY);
			}
		}
		ForestZone.Add(MainTundraForestZone);
		watch.Stop();
		Mod.Logger.Info("Snow step: " + watch.ToString());
	}
	[Task]
	public void Generate_JungleForest() {
		Stopwatch watch = new();
		watch.Start();
		Rectangle forestArea = MainJungleForestZone;
		//We are using standard generation for this one
		int startingPoint = forestArea.Height - forestArea.Height / 8 + forestArea.Y;
		int offsetRaise = 0;
		bool MoveToNextX;
		float left = Rand.NextFloat(.25f, .35f);
		float right = Rand.NextFloat(.75f, .85f);
		int CurrentPosY;
		int LeftPos = forestArea.X + (int)(forestArea.Width * left);
		int RightPos = forestArea.X + (int)(forestArea.Width * right);
		int MiddlePos = forestArea.X + (int)(forestArea.Width * .5f);
		int datawidth = 20;
		for (int i = forestArea.X; i < forestArea.Width + forestArea.X; i++) {
			MoveToNextX = true;
			for (int j = startingPoint; j < forestArea.Height + forestArea.Y; j++) {
				if (MoveToNextX) {
					if (Rand.NextBool(5)) {
						offsetRaise += Rand.Next(-1, 2);
					}
					MoveToNextX = false;
					j = startingPoint - offsetRaise;
					GenerationHelper.FastPlaceTile(i, j, TileID.JungleGrass);
					continue;
				}
				if (Rand.NextFloat() <= .45f) {
					GenerationHelper.FastPlaceTile(i, j, TileID.Mud);
				}
				else {
					GenerationHelper.FastPlaceTile(i, j, TileID.JungleGrass);
				}
			}
			CurrentPosY = startingPoint - offsetRaise;
			if (i == MiddlePos) {
				SpawnTownNPC(NPCID.Dryad, i, CurrentPosY - 3);
				StructureData data = Generator.GetStructureData("Assets/Jungle_house", Mod);
				Generator.GenerateFromData(data, new(i - data.width / 2, CurrentPosY - data.height + 3));
			}
			else if (Rand.NextBool(26)
				&& i < LeftPos - datawidth || i > LeftPos + datawidth
				&& i < RightPos - datawidth || i > RightPos + datawidth) {
				WorldGen.GrowTree(i, CurrentPosY);
			}
		}
		ForestZone.Add(MainJungleForestZone);
		watch.Stop();
		Mod.Logger.Info("Snow step: " + watch.ToString());
	}
	[Task]
	public void Generate_MerchantHouse() {
		Stopwatch watch = new();
		watch.Start();
		Rectangle forestArea = merchantHouse;
		//We are using standard generation for this one
		int startingPoint = forestArea.Height - forestArea.Height / 8 + forestArea.Y;
		int offsetRaise = 0;
		bool MoveToNextX;
		float left = Rand.NextFloat(.25f, .35f);
		float right = Rand.NextFloat(.75f, .85f);
		int CurrentPosY = 0;
		int LeftPos = forestArea.X + (int)(forestArea.Width * left);
		int RightPos = forestArea.X + (int)(forestArea.Width * right);
		int Middle = (int)(forestArea.Width * .5f) + forestArea.X;
		int YoffsetByX = 0;
		for (int i = forestArea.X; i < forestArea.Width + forestArea.X; i++) {
			MoveToNextX = true;
			if (i == Middle) {
				var Merchant = ModWrapper.Get_StructureData("Assets/MerchantHouse", Mod);
				ModWrapper.GenerateFromData(Merchant, new(i - Merchant.width / 2, CurrentPosY - Merchant.height));
				SpawnTownNPC(NPCID.Merchant, i, CurrentPosY - 3);
				YoffsetByX = Merchant.width / 2;
				startingPoint = CurrentPosY;
			}
			for (int j = startingPoint; j < forestArea.Height + forestArea.Y; j++) {
				if (MoveToNextX) {
					if (YoffsetByX <= 0) {
						if (Rand.NextBool(5)) {
							offsetRaise += Rand.Next(-1, 2);
						}
						j = startingPoint - offsetRaise;
					}
					MoveToNextX = false;
					GenerationHelper.FastPlaceTile(i, j, TileID.Grass);
					continue;
				}
				GenerationHelper.FastPlaceTile(i, j, TileID.Dirt);
			}
			CurrentPosY = startingPoint - offsetRaise;
			YoffsetByX--;
			if (Rand.NextBool(15)
				&& i < LeftPos
				&& i > RightPos) {
				WorldGen.GrowTree(i, CurrentPosY);
			}
		}
		watch.Stop();
		Mod.Logger.Info("Merchant House structure realm step: " + watch.ToString());
	}
	[Task]
	public void Generate_AbandonIgloo() {
		Stopwatch watch = new();
		watch.Start();
		Rectangle forestArea = mechanicHouse;
		//We are using standard generation for this one
		int startingPoint = forestArea.Height - forestArea.Height / 8 + forestArea.Y;
		int offsetRaise = 0;
		bool MoveToNextX;
		float left = Rand.NextFloat(.25f, .35f);
		float right = Rand.NextFloat(.75f, .85f);
		int CurrentPosY = 0;
		int LeftPos = forestArea.X + (int)(forestArea.Width * left);
		int RightPos = forestArea.X + (int)(forestArea.Width * right);
		int Middle = (int)(forestArea.Width * .5f) + forestArea.X;
		int YoffsetByX = 0;
		for (int i = forestArea.X; i < forestArea.Width + forestArea.X; i++) {
			MoveToNextX = true;
			if (i == Middle) {
				var Merchant = ModWrapper.Get_StructureData("Assets/Abandon_Igloo", Mod);
				ModWrapper.GenerateFromData(Merchant, new(i - Merchant.width / 2, CurrentPosY - Merchant.height));
				SpawnTownNPC(NPCID.Mechanic, i, CurrentPosY - 3);
				YoffsetByX = Merchant.width / 2;
				startingPoint = CurrentPosY;
			}
			for (int j = startingPoint; j < forestArea.Height + forestArea.Y; j++) {
				if (MoveToNextX) {
					if (YoffsetByX <= 0) {
						if (Rand.NextBool(5)) {
							offsetRaise += Rand.Next(-1, 2);
						}
						j = startingPoint - offsetRaise;
					}
					MoveToNextX = false;
					GenerationHelper.FastPlaceTile(i, j, TileID.SnowBlock);
					continue;
				}
				if (Rand.NextFloat() <= .45f) {
					GenerationHelper.FastPlaceTile(i, j, TileID.IceBlock);
				}
				else {
					GenerationHelper.FastPlaceTile(i, j, TileID.SnowBlock);
				}
			}
			CurrentPosY = startingPoint - offsetRaise;
			YoffsetByX--;
			if (Rand.NextBool(15)
				&& i < LeftPos
				&& i > RightPos) {
				WorldGen.GrowTree(i, CurrentPosY);
			}
		}
		watch.Stop();
		Mod.Logger.Info("Abandon Igloo structure realm step: " + watch.ToString());
	}
	[Task]
	public void Generate_SmallForest() {
		Stopwatch watch = new();
		watch.Start();
		foreach (Rectangle zone in ForestZone) {
			//We are using standard generation for this one
			int startingPoint = zone.Height - zone.Height / 8 + zone.Y;
			int offsetRaise = 0;
			bool MoveToNextX;
			int CurrentPosY;
			Get_BiomeData(zone.Center, 0, out BiomeDataBundle bundle);
			for (int i = zone.X; i < zone.Width + zone.X; i++) {
				MoveToNextX = true;
				for (int j = startingPoint; j < zone.Height + zone.Y; j++) {
					if (MoveToNextX) {
						if (Rand.NextBool(5)) {
							offsetRaise += Rand.Next(-1, 2);
						}
						MoveToNextX = false;
						j = startingPoint - offsetRaise;
						GenerationHelper.FastPlaceTile(i, j, bundle.outlineTile);
						continue;
					}
					if (Rand.NextFloat() <= .45f) {
						GenerationHelper.FastPlaceTile(i, j, bundle.tile2);
					}
					else {
						GenerationHelper.FastPlaceTile(i, j, bundle.tile);
					}
				}
				CurrentPosY = startingPoint - offsetRaise;
				if (Rand.NextBool(11)) {
					WorldGen.GrowTree(i, CurrentPosY);
				}
			}
		}
		ForestZone.Add(merchantHouse);
		ForestZone.Add(mechanicHouse);
		ForestZone.Add(MainTundraForestZone);
		ForestZone.Add(MainJungleForestZone);
		ForestZone.Add(MainForestZone);
		watch.Stop();
		Mod.Logger.Info("Small forest step: " + watch.ToString());
	}
	[Task]
	public void Generate_JungleTempleEntrance() {
		Stopwatch watch = new();
		watch.Start();
		int X = 11 * GridPart_X + Rand.Next(0, GridPart_X);
		int Y = 5 * GridPart_Y + Rand.Next(0, GridPart_Y);
		var data = ModWrapper.Get_StructureData("Assets/JungleTempleEntrance", Mod);
		int Width = data.width / 2;
		int Height = data.height / 2;
		Point16 point = new(X - Width, Y - Height);
		if (ModWrapper.IsInBound(data, point)) {
			JungleTempleEntrance = new(point.X, point.Y, data.width, data.height);
			ModWrapper.GenerateFromData(data, point);
			ZoneToBeIgnored.Add(JungleTempleEntrance);
			Set_MapIgnoredZoneIntoWorldGen(JungleTempleEntrance);
		}
		watch.Stop();
		Mod.Logger.Info("Jungle temple entrance step: " + watch.ToString());
	}
	[Task]
	public void Generate_GoldRoom() {
		Stopwatch watch = new();
		watch.Start();
		Rectangle goldRoomSize = new(0, 0, 150, 150);
		for (int i = 0; i < Main.maxTilesX; i++) {
			for (int j = 0; j < Main.maxTilesY; j++) {
				if (i > 100 && i < Main.maxTilesX - 100 && j > 100 && j < Main.maxTilesY - 100) {
					//This is where we generate our gold room via code
					if (i == Main.maxTilesX * .9f && j == Main.maxTilesY * .1f) {
						goldRoomSize.X = i;
						goldRoomSize.Y = j;
					}
					else if (i == Main.maxTilesX * .1f && j == Main.maxTilesY * .1f) {
						goldRoomSize.X = i;
						goldRoomSize.Y = j;
					}
					if (goldRoomSize.X != 0 && goldRoomSize.Y != 0) {
						if (goldRoomSize.Contains(i, j)) {
							if (i == goldRoomSize.Left
							|| j == goldRoomSize.Top
							|| i == goldRoomSize.Right - 1
							|| j == goldRoomSize.Bottom - 1) {
								GenerationHelper.FastPlaceTile(i, j, TileID.Stone);
							}
							else {
								GenerationHelper.FastPlaceTile(i, j, TileID.Gold);
							}
							if (i == goldRoomSize.Right - 1 && j == goldRoomSize.Bottom - 1) {
								Set_MapIgnoredZoneIntoWorldGen(goldRoomSize);
								ZoneToBeIgnored.Add(goldRoomSize);
								goldRoomSize.X = 0;
								goldRoomSize.Y = 0;
							}
						}
					}
				}
			}
		}
		watch.Stop();
		Mod.Logger.Info("Gold room step: " + watch.ToString());
	}
	[Task]
	public void Generate_OceanBiome() {
		Stopwatch watch = new();
		watch.Start();
		rect = GenerationHelper.GridPositionInTheWorld24x24(0, 6, 2, 14);
		float halfWidth = rect.Width / 2f;
		float Height90 = rect.Height * .9f;
		float Height10 = rect.Height - Height90;
		float Height5 = Height10 / 2f;
		for (int y = rect.Y; y < rect.Y + rect.Height; y++) {
			for (int x = rect.X; x < rect.X + rect.Width; x++) {
				if (y >= rect.Y + rect.Height * .9f) {
					if (y == rect.Y + rect.Height - 1) {
						GenerationHelper.FastPlaceTile(x, y, TileID.Sandstone);
						continue;
					}
					float percentX = x / halfWidth;
					float percentY = (y - Height90 - rect.Y) / Height10;
					if (x > halfWidth) {
						percentX = (rect.Width - x) / halfWidth;
					}
					if (ModUtils.InOutExpo(percentX, 7) > percentY
						) {
						GenerationHelper.FastPlaceTile(x, y, TileID.Sand);
					}
					else {
						GenerationHelper.FastPlaceTile(x, y, TileID.Sandstone);
					}
				}
				else {
					if (x > rect.Width * .1f && x < rect.Width * .9f && y <= rect.Y + rect.Height * .85f) {
						x = (int)(rect.Width * .9f);
						continue;
					}
					if (y > rect.Y + rect.Height * .85f && x > rect.Width * .1f && x < rect.Width * .9f) {
						float percentX = x / halfWidth;
						float percentY = (y - Height90 + Height5 - rect.Y) / Height5;
						if (x > halfWidth) {
							percentX = (rect.Width - x) / halfWidth;
						}
						if (ModUtils.InOutExpo(percentX, 7) > percentY) {
							continue;
						}
					}
					GenerationHelper.FastPlaceTile(x, y, TileID.Sand);
				}
			}
		}
		watch.Stop();
		WatchTracker += watch.Elapsed;
		Mod.Logger.Info("Create ocean step :" + watch.ToString());
	}
	public static void AddLoot(Chest chest) {
		if (Main.rand.NextBool(50)) {
			chest.AddItemToShop(new Item(ModContent.ItemType<GoldLootBox>()));
		}
		else if (Main.rand.NextBool(25)) {
			chest.AddItemToShop(new Item(ModContent.ItemType<SilverLootBox>()));
		}
		else if (Main.rand.NextBool(10)) {
			chest.AddItemToShop(new Item(ModContent.ItemType<IronLootBox>()));
		}
		else {
			chest.AddItemToShop(new Item(ModContent.ItemType<WoodenLootBox>()));
		}
	}
	/// <summary>
	/// This is a note for future development, any structure that is not completely square should be generated after this<br/>
	/// So that the structure null wall/tile actually have a use
	/// </summary>
	[Task]
	public void Create_Biome() {
		Stopwatch watch = new();
		watch.Start();
		rect = GenerationHelper.GridPositionInTheWorld24x24(0, 0, 24, 24);
		//Since the length of the template won't changed since they all use the same size, it is completely fine to cached this 
		int length = HorizontalTemplate[0].Get_TotalLength();
		int noiseCounter = 0, Width, Height;
		Structure_XinimVer structure;
		while (counter.X < rect.Width || counter.Y < rect.Height) {
			if (++additionaloffset >= 2) {
				counter.X += 32;
				additionaloffset = 0;
			}
			IsUsingHorizontal = ++count % 2 == 0;
			int offsetY = 0, offsetX = 0, holdX, holdY;
			if (IsUsingHorizontal) {
				Width = 64;
				Height = 32;
				structure = Rand.Next(HorizontalTemplate);
			}
			else {
				Width = 32;
				Height = 64;
				structure = Rand.Next(VerticalTemplate);
			}
			switch (Rand.Next(styles)) {
				//Because implementing Flip Horizontal is very costly and we currently have no good way of doing it, it is better to just make it act the same as none
				case GenerateStyle.None:
				case GenerateStyle.FlipHorizon:
					for (int i = 0; i < length; i++) {
						if (offsetY >= Height) {
							offsetY = 0;
							offsetX++;
						}
						holdX = counter.X + offsetX; holdY = counter.Y + offsetY;
						if (WorldGen.InWorld(holdX, holdY) && !Get_MapIgnoredZoneInWorldGen(holdX, holdY)) {
							TileData data = structure.Get_CurrentTileData(i);
							Place_Tile_CreateBiome(holdX, holdY, noiseCounter, ref data);
						}
						offsetY++;
						noiseCounter = ModUtils.Safe_SwitchValue(noiseCounter, StaticNoise255x255.Length - 1);
					}
					break;
				case GenerateStyle.FlipVertical:
					for (int i = structure.Get_Data.Length - 1; i >= 0; i--) {
						GenPassData gdata = structure.Get_Data[i];
						for (int l = 0; l < gdata.Count; l++) {
							if (offsetY >= Height) {
								offsetY = 0;
								offsetX++;
							}
							holdX = counter.X + offsetX; holdY = counter.Y + offsetY;
							if (WorldGen.InWorld(holdX, holdY) && !Get_MapIgnoredZoneInWorldGen(holdX, holdY)) {
								TileData data = gdata.tileData;
								Place_Tile_CreateBiome(holdX, holdY, noiseCounter, ref data);
							}
							offsetY++;
							noiseCounter = ModUtils.Safe_SwitchValue(noiseCounter, StaticNoise255x255.Length - 1);
						}
					}
					break;
				case GenerateStyle.FlipBoth:
					for (int i = length - 1; i >= 0; i--) {
						if (offsetY >= Height) {
							offsetY = 0;
							offsetX++;
						}
						holdX = counter.X + offsetX; holdY = counter.Y + offsetY;
						if (WorldGen.InWorld(holdX, holdY) && !Get_MapIgnoredZoneInWorldGen(holdX, holdY)) {
							TileData data = structure.Get_CurrentTileData(i);
							Place_Tile_CreateBiome(holdX, holdY, noiseCounter, ref data);
						}
						offsetY++;
						noiseCounter = ModUtils.Safe_SwitchValue(noiseCounter, StaticNoise255x255.Length - 1);
					}
					break;
			}
			if (counter.X < rect.Width) {
				counter.X += Width;
			}
			else {
				//Due to fuckery of this code, I have no hand and I must comment.
				offsetcount = (offsetcount + 1) % 4;//Count from 0 -> 3
				counter.X = -32 * offsetcount; // preset offset
				counter.Y += 32;//this never change cause obviously
								//Setting standard variable.
				count = 1;
				additionaloffset = -1;

				while (counter.X < 0) {
					if (++additionaloffset >= 2) {
						counter.X += 32;
						additionaloffset = 0;
					}
					IsUsingHorizontal = ++count % 2 == 0;
					Width = IsUsingHorizontal ? 64 : 32;
					counter.X += Width;
				}
			}
		}
		watch.Stop();
		WatchTracker += watch.Elapsed;
		Mod.Logger.Info("Create biome step :" + watch.ToString());
	}
	[Task]
	public void Generate_SmallVillage() {
		Stopwatch watch = new();
		watch.Start();
		for (int b = 0; b < 24; b++) {
			List<Rectangle> placed = new();
			int amount = Rand.Next(10, 20);
			int xdex = Main.rand.Next(1, 23);
			int ydex = Main.rand.Next(1, 22);
			short ID = CharToBid(GetStringDataBiomeMapping(xdex, ydex));
			Rectangle villageZone = new(GridPart_X * xdex + Main.rand.Next(0, GridPart_X), GridPart_Y * ydex, 250, 100);
			short ID2 = Get_BiomeIDViaPos(villageZone.Location + new Point(0, 100), 0);
			short ID3 = Get_BiomeIDViaPos(villageZone.Location + new Point(250, 0), 0);
			short ID4 = Get_BiomeIDViaPos(villageZone.Location + new Point(250, 100), 0);
			while (!(ValidBiome(ID) && ValidBiome(ID2) && ValidBiome(ID3) && ValidBiome(ID4) && !ZoneToBeIgnored.Where(rect => rect.Intersects(villageZone)).Any())) {
				xdex = Main.rand.Next(1, 23);
				ydex = Main.rand.Next(1, 22);
				ID = CharToBid(GetStringDataBiomeMapping(xdex, ydex));
				villageZone.X = GridPart_X * xdex + Main.rand.Next(0, GridPart_X);
				villageZone.Y = GridPart_Y * ydex + Main.rand.Next(0, GridPart_Y);
				villageZone = Rect_CentralizeRect(villageZone);
				ID2 = Get_BiomeIDViaPos(villageZone.Location + new Point(0, 100), 0);
				ID3 = Get_BiomeIDViaPos(villageZone.Location + new Point(250, 0), 0);
				ID4 = Get_BiomeIDViaPos(villageZone.Location + new Point(250, 100), 0);
			}
			ZoneToBeIgnored.Add(villageZone);
			for (int i = 0; i < amount; i++) {
				StructureData data = Get_RandomizeAbandonStructure(Mod);
				Point zonecachedPoint = Rand.NextVector2FromRectangle(villageZone).ToPoint();
				Rectangle re = Rect_CentralizeRect(zonecachedPoint.X, zonecachedPoint.Y, data.width, data.height);
				bool canBeAdded = true;
				foreach (var item in placed) {
					if (item.Intersects(re)) {
						canBeAdded = false;
						break;
					}
				}
				if (!canBeAdded) {
					i--;
					continue;
				}
				Set_MapIgnoredZoneIntoWorldGen(re);
				ModWrapper.GenerateFromData(data, re.TopLeft().ToPoint16());
				bool chestplaced = false;
				int chestAttempt = 200;
				for (int a = 0; a < chestAttempt; a++) {
					Point randomPoint = new Point(Main.rand.Next(re.X, re.X + re.Width), Main.rand.Next(re.Y, re.Y + re.Height));
					int chest = WorldGen.PlaceChest(randomPoint.X, randomPoint.Y);
					if (chest == -1) {
						continue;
					}
					Set_MapIgnoredZoneIntoWorldGen(randomPoint.X, randomPoint.Y, 2, 2);
					chestplaced = true;
					AddLoot(Main.chest[chest]);
					//Add loot here
					break;
				}
				for (int x = re.X; x < re.X + re.Width; x++) {
					if (chestplaced) {
						break;
					}
					for (int y = re.Y; y < re.Y + re.Height; y++) {
						int chest = WorldGen.PlaceChest(x, y);
						if (chest == -1) {
							continue;
						}
						Set_MapIgnoredZoneIntoWorldGen(x, y, 2, 2);
						chestplaced = true;
						AddLoot(Main.chest[chest]);
						//Add loot here
						break;
					}
				}
				placed.Add(re);
			}
		}
		watch.Stop();
		Mod.Logger.Info("Small village step: " + watch.ToString());
	}
	[Task]
	public void Generate_AbandonStructureAllOverThePlace() {
		Stopwatch watch = new();
		watch.Start();
		List<Rectangle> placed = new();
		for (int i = 0; i < 300; i++) {
			StructureData data = Get_RandomizeAbandonStructure(Mod);
			int xdex = Main.rand.Next(1, 23);
			int ydex = Main.rand.Next(1, 22);
			short ID = CharToBid(GetStringDataBiomeMapping(xdex, ydex));
			if (!ValidBiome(ID)) {
				i--;
				continue;
			}
			Point zonecachedPoint = new(GridPart_X * xdex + Main.rand.Next(0, GridPart_X), GridPart_Y * ydex + Main.rand.Next(0, GridPart_Y));
			Rectangle re = Rect_CentralizeRect(zonecachedPoint.X, zonecachedPoint.Y, data.width, data.height);
			bool canBeAdded = true;
			foreach (var item in ZoneToBeIgnored) {
				if (item.Intersects(re)) {
					canBeAdded = false;
					break;
				}
			}
			foreach (var item in placed) {
				if (item.Intersects(re)) {
					canBeAdded = false;
					break;
				}
			}
			short centralID = Get_BiomeIDViaPos(re.Center, 0);
			short topleftID = Get_BiomeIDViaPos(re.TopLeft().ToPoint(), 0);
			short toprightID = Get_BiomeIDViaPos(re.TopRight().ToPoint(), 0);
			short bottomleftID = Get_BiomeIDViaPos(re.BottomLeft().ToPoint(), 0);
			short bottomrightID = Get_BiomeIDViaPos(re.BottomRight().ToPoint(), 0);
			if (!canBeAdded || centralID != topleftID || centralID != toprightID || centralID != bottomleftID || centralID != bottomrightID) {
				i--;
				continue;
			}
			Set_MapIgnoredZoneIntoWorldGen(re);
			ModWrapper.GenerateFromData(data, re.TopLeft().ToPoint16());
			bool chestplaced = false;
			int chestAttempt = 200;
			for (int a = 0; a < chestAttempt; a++) {
				Point randomPoint = new Point(Main.rand.Next(re.X, re.X + re.Width), Main.rand.Next(re.Y, re.Y + re.Height));
				int chest = WorldGen.PlaceChest(randomPoint.X, randomPoint.Y);
				if (chest == -1) {
					continue;
				}
				Set_MapIgnoredZoneIntoWorldGen(randomPoint.X, randomPoint.Y, 2, 2);
				chestplaced = true;
				AddLoot(Main.chest[chest]);
				//Add loot here
				break;
			}
			for (int x = re.X; x < re.X + re.Width; x++) {
				if (chestplaced) {
					break;
				}
				for (int y = re.Y; y < re.Y + re.Height; y++) {
					int chest = WorldGen.PlaceChest(x, y);
					if (chest == -1) {
						continue;
					}
					Set_MapIgnoredZoneIntoWorldGen(x, y, 2, 2);
					chestplaced = true;
					AddLoot(Main.chest[chest]);
					//Add loot here
					break;
				}
			}
			placed.Add(re);
		}
		watch.Stop();
		Mod.Logger.Info("Abandon structure step: " + watch.ToString());
	}
	[Task]
	public void Generate_CursedKingdomStructure() {
		Stopwatch watch = new();
		watch.Start();
		var data = ModWrapper.Get_StructureData("Assets/CK_Entrance", Mod);
		int X = 17 * GridPart_X + Rand.Next(GridPart_X - data.width);
		int Y = 17 * GridPart_Y + Rand.Next(GridPart_Y - data.height);
		int Width = data.width / 2;
		int Height = data.height / 2;
		Point16 point = new(X - Width, Y - Height);
		if (ModWrapper.IsInBound(data, point)) {
			CursedKingdomArea = new(point.X, point.Y, data.width, data.height);
			ModWrapper.GenerateFromData(data, point);
			ZoneToBeIgnored.Add(CursedKingdomArea);
			Set_MapIgnoredZoneIntoWorldGen(CursedKingdomArea);
		}
		watch.Stop();
		Mod.Logger.Info("CK_entrance step: " + watch.ToString());
	}
	[Task]
	public void Generate_CrimsonEntrance() {
		Stopwatch watch = new();
		watch.Start();
		var data = ModWrapper.Get_StructureData("Assets/Crimson_Entrance", Mod);
		int X = 21 * GridPart_X + Rand.Next(GridPart_X - data.width);
		int Y = 12 * GridPart_Y + Rand.Next(GridPart_Y - data.height);
		int Width = data.width / 2;
		int Height = data.height / 2;
		Point16 point = new(X - Width, Y - Height);
		if (ModWrapper.IsInBound(data, point)) {
			CrimsonEntrance = new(point.X, point.Y, data.width, data.height);
			ModWrapper.GenerateFromData(data, point);
			ZoneToBeIgnored.Add(CrimsonEntrance);
			Set_MapIgnoredZoneIntoWorldGen(CrimsonEntrance);
		}
		watch.Stop();
		Mod.Logger.Info("Crimson step: " + watch.ToString());
	}
	[Task]
	public void Generate_CorruptionEntrance() {
		Stopwatch watch = new();
		watch.Start();
		var data = ModWrapper.Get_StructureData("Assets/Corruption_Entrance", Mod);
		int X = 7 * GridPart_X + Rand.Next(GridPart_X - data.width);
		int Y = 19 * GridPart_Y + Rand.Next(GridPart_X - data.height);
		int Width = data.width / 2;
		int Height = data.height / 2;
		Point16 point = new(X - Width, Y - Height);
		if (ModWrapper.IsInBound(data, point)) {
			CorruptionEntrance = new(point.X, point.Y, data.width, data.height);
			ModWrapper.GenerateFromData(data, point);
			ZoneToBeIgnored.Add(CorruptionEntrance);
			Set_MapIgnoredZoneIntoWorldGen(CorruptionEntrance);
		}
		watch.Stop();
		Mod.Logger.Info("Corruption step: " + watch.ToString());
	}
	[Task]
	public void Generate_FleshRealmEntrance() {
		Stopwatch watch = new();
		watch.Start();
		var data = ModWrapper.Get_StructureData("Assets/FleshRealm_Entrance", Mod);
		int X = 22 * GridPart_X + Rand.Next(GridPart_X - data.width);
		int Y = 20 * GridPart_Y + Rand.Next(GridPart_Y - data.height);
		int Width = data.width / 2;
		int Height = data.height / 2;
		Point16 point = new(X - Width, Y - Height);
		if (ModWrapper.IsInBound(data, point)) {
			FleshRealmEntrance = new(point.X, point.Y, data.width, data.height);
			ModWrapper.GenerateFromData(data, point);
			ZoneToBeIgnored.Add(FleshRealmEntrance);
			Set_MapIgnoredZoneIntoWorldGen(FleshRealmEntrance);
		}
		watch.Stop();
		Mod.Logger.Info("Flesh realm step: " + watch.ToString());
	}
	[Task]
	public void Generate_SlimeWorldEntrance() {
		Stopwatch watch = new();
		watch.Start();
		int X = 16 * GridPart_X;
		int Y = 11 * GridPart_Y;
		var data = ModWrapper.Get_StructureData("Assets/SlimeWorld_Entrance", Mod);
		int Width = data.width / 2;
		int Height = data.height / 2;
		Point16 point = new(X - Width, Y - Height);
		if (ModWrapper.IsInBound(data, point)) {
			SlimeWorldEntrance = new(point.X, point.Y, data.width, data.height);
			ModWrapper.GenerateFromData(data, point);
			ZoneToBeIgnored.Add(SlimeWorldEntrance);
			Set_MapIgnoredZoneIntoWorldGen(SlimeWorldEntrance);
		}
		watch.Stop();
		Mod.Logger.Info("Slime world step: " + watch.ToString());
	}
	[Task]
	public void Generate_DesertPyramid() {
		Stopwatch watch = new();
		watch.Start();
		int X = 17 * GridPart_X + Main.rand.Next(GridPart_X);
		int Y = 7 * GridPart_Y + Main.rand.Next(GridPart_Y);
		var data = ModWrapper.Get_StructureData("Assets/DesertPyramid", Mod);
		int Width = data.width / 2;
		int Height = data.height / 2;
		Point16 point = new(X - Width, Y - Height);
		Pyramid = new(point.X, point.Y, data.width, data.height);
		if (ModWrapper.IsInBound(data, point)) {
			ModWrapper.GenerateFromData(data, point);
			ZoneToBeIgnored.Add(Pyramid);
			Set_MapIgnoredZoneIntoWorldGen(Pyramid);
		}
		watch.Stop();
		Mod.Logger.Info("Pyramid realm step: " + watch.ToString());
	}
	[Task]
	public void Generate_GuideHouse() {

	}
	[Task]
	public void Generate_UnderworldOptimized() {
		Stopwatch watch = new();
		watch.Start();
		int num838 = Main.maxTilesY - Rand.Next(150, 190);
		for (int num839 = 0; num839 < Main.maxTilesX; num839++) {
			num838 += Rand.Next(-3, 4);
			if (num838 < Main.maxTilesY - 190)
				num838 = Main.maxTilesY - 190;

			if (num838 > Main.maxTilesY - 160)
				num838 = Main.maxTilesY - 160;

			for (int num840 = num838 - 20 - Rand.Next(3); num840 < Main.maxTilesY; num840++) {
				Tile tile = Main.tile[num839, num840];
				if (num840 >= num838) {
					tile.HasTile = false;
					tile.LiquidType = LiquidID.Lava;
					tile.LiquidAmount = 0;
				}
				else {
					tile.TileType = TileID.Ash;
				}
			}
		}

		int num841 = Main.maxTilesY - Rand.Next(40, 70);
		for (int num842 = 10; num842 < Main.maxTilesX - 10; num842++) {
			num841 += Rand.Next(-10, 11);
			if (num841 > Main.maxTilesY - 60)
				num841 = Main.maxTilesY - 60;

			if (num841 < Main.maxTilesY - 100)
				num841 = Main.maxTilesY - 120;

			for (int i = num841; i < Main.maxTilesY - 10; i++) {
				Tile tile = Main.tile[num842, i];
				if (!tile.HasTile) {
					tile.LiquidType = LiquidID.Lava;
					tile.LiquidAmount = byte.MaxValue;
				}
			}
		}

		for (int num844 = 0; num844 < Main.maxTilesX; num844++) {
			if (Rand.Next(50) == 0) {
				int num845 = Main.maxTilesY - 65;
				while (!Main.tile[num844, num845].HasTile && num845 > Main.maxTilesY - 135) {
					num845--;
				}

				WorldGen.TileRunner(Rand.Next(0, Main.maxTilesX), num845 + Rand.Next(20, 50), Rand.Next(15, 20), 1000, 57, addTile: true, 0.0, Rand.Next(1, 3), noYChange: true);
			}
		}

		Liquid.QuickWater(-2);
		for (int num846 = 0; num846 < Main.maxTilesX; num846++) {
			if (Rand.NextBool(13)) {
				int num848 = Main.maxTilesY - 65;
				while ((Main.tile[num846, num848].LiquidAmount > 0 || Main.tile[num846, num848].HasTile) && num848 > Main.maxTilesY - 140) {
					num848--;
				}
				bool LeftRightCheck = !(num846 > Main.maxTilesX * 0.4) || !(num846 < Main.maxTilesX * 0.6);
				if (Rand.NextBool(3) || LeftRightCheck)
					WorldGen.TileRunner(num846, num848 - Rand.Next(2, 5), Rand.Next(5, 30), 1000, 57, addTile: true, 0.0, Rand.Next(1, 3), noYChange: true);

				float num849 = Rand.NextFloat(1, 3);
				if (Rand.NextBool(3))
					num849 *= 0.5f;

				if (Rand.NextBool(3) || LeftRightCheck) {
					if (Rand.NextBool(2)) {
						WorldGen.TileRunner(num846, num848 - Rand.Next(2, 5), (int)(Rand.Next(5, 15) * num849), (int)(Rand.Next(10, 15) * num849), 57, addTile: true, 1.0, 0.3);
					}

					if (Rand.NextBool(3)) {
						num849 = Rand.Next(1, 3);
						WorldGen.TileRunner(num846, num848 - Rand.Next(2, 5), (int)(Rand.Next(5, 15) * num849), (int)(Rand.Next(10, 15) * num849), 57, addTile: true, -1.0, 0.3);
					}
				}

				WorldGen.TileRunner(num846 + Rand.Next(-10, 10), num848 + Rand.Next(-10, 10), Rand.Next(5, 15), Rand.Next(5, 10), -2, addTile: false, Rand.Next(-1, 3), Rand.Next(-1, 3));
				if (Rand.NextBool(3))
					WorldGen.TileRunner(num846 + Rand.Next(-10, 10), num848 + Rand.Next(-10, 10), Rand.Next(10, 30), Rand.Next(10, 20), -2, addTile: false, Rand.Next(-1, 3), Rand.Next(-1, 3));

				if (Rand.NextBool(5))
					WorldGen.TileRunner(num846 + Rand.Next(-15, 15), num848 + Rand.Next(-15, 10), Rand.Next(15, 30), Rand.Next(5, 20), -2, addTile: false, Rand.Next(-1, 3), Rand.Next(-1, 3));
			}
		}

		for (int num850 = 0; num850 < Main.maxTilesX; num850++) {
			WorldGen.TileRunner(Rand.Next(20, Main.maxTilesX - 20), Rand.Next(Main.maxTilesY - 180, Main.maxTilesY - 10), Rand.Next(2, 7), Rand.Next(2, 7), -2);
		}

		for (int num853 = 0; num853 < (int)(Main.maxTilesX * Main.maxTilesY * 0.0008f); num853++) {
			WorldGen.TileRunner(Rand.Next(0, Main.maxTilesX), Rand.Next(Main.maxTilesY - 140, Main.maxTilesY), Rand.Next(2, 7), Rand.Next(3, 7), 58);
		}

		for (int num866 = 25; num866 < Main.maxTilesX - 25; num866++) {
			if (num866 < Main.maxTilesX * 0.17f || num866 > Main.maxTilesX * 0.83f) {
				for (int num867 = Main.maxTilesY - 300; num867 < Main.maxTilesY - 100; num867++) {
					if (Main.tile[num866, num867].TileType == TileID.Ash && Main.tile[num866, num867].HasTile
						&& (!Main.tile[num866 - 1, num867 - 1].HasTile
						|| !Main.tile[num866, num867 - 1].HasTile
						|| !Main.tile[num866 + 1, num867 - 1].HasTile
						|| !Main.tile[num866 - 1, num867].HasTile
						|| !Main.tile[num866 + 1, num867].HasTile
						|| !Main.tile[num866 - 1, num867 + 1].HasTile
						|| !Main.tile[num866, num867 + 1].HasTile
						|| !Main.tile[num866 + 1, num867 + 1].HasTile)) {
						Main.tile[num866, num867].TileType = TileID.AshGrass;
					}
				}
			}
			else {
				num866 = (int)(Main.maxTilesX * 0.83f + 1);
			}
		}
		watch.Stop();
		Mod.Logger.Info("Creating underworld step: " + watch.ToString());
	}
	[Task]
	public void Generate_PostWorld() {
	}
	[Task]
	public void FinalTask() {
		//Generate_TrialTest(Main.maxTilesX / 3, Main.maxTilesY / 2);
	}
}

