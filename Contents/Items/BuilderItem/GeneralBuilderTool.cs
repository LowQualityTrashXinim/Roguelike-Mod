using Microsoft.Build.Tasks.Deployment.ManifestUtilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Roguelike.Common.Systems;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace Roguelike.Contents.Items.BuilderItem;
public class GeneralBuilderToolSystem : ModSystem {
	public const byte None = 0;
	public const byte Fill = 1;
	public const byte Draw = 2;
	public const byte Create = 3;
	public static byte CurrentMode = None;
	public static bool DeleteMode = false;
	public static bool OverrideMode = false;
	public static bool OutlineMode = false;
	public static bool MirrorMode = false;
	public static bool Tile = false;
	public static bool Wall = false;
	public Vector2 createPosition = Vector2.Zero;
	public GeneralBuilderToolUI GeneralBuilderToolState;
	internal UserInterface userInterface;
	public override void Load() {
		if (!Main.dedServ) {
			userInterface = new();
			GeneralBuilderToolState = new();
		}
	}
	public override void Unload() {
		userInterface = null;
		GeneralBuilderToolState = null;
	}
	public override void UpdateUI(GameTime gameTime) {
		base.UpdateUI(gameTime);
		userInterface?.Update(gameTime);
	}
	public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
		int InventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
		if (InventoryIndex != -1)
			layers.Insert(InventoryIndex, new LegacyGameInterfaceLayer(
				"Roguelike: UI",
				delegate {
					GameTime gametime = new GameTime();
					userInterface.Draw(Main.spriteBatch, gametime);
					return true;
				},
				InterfaceScaleType.UI)
			);
	}
	public void ToggleUI() {
		if (userInterface.CurrentState != null) {
			DeactivateUI();
		}
		else {
			ActivateUI();
		}
	}
	public void DeactivateUI() {
		userInterface.SetState(null);
	}
	public void ActivateUI() {
		userInterface.SetState(GeneralBuilderToolState);
	}
}
public class GeneralBuilderToolUI : UIState {
	public UIPanel Panel;
	public Roguelike_UIImage FillMode;
	public Roguelike_UIImage DrawMode;
	public Roguelike_UIImage DeleteMode;
	public Roguelike_UIImage OverrideMode;
	public Roguelike_UIImage TileMode;
	public Roguelike_UIImage WallMode;
	public Roguelike_UIPanel Main_CreateMode_Panel;
	public Roguelike_UIImage CreateMode;
	public Roguelike_TextBox txb_width;
	public Roguelike_TextBox txb_height;
	public Roguelike_UIImage OutlineMode;
	public override void OnInitialize() {
		Asset<Texture2D> thesameuitextureasvanilla = TextureAssets.InventoryBack7;

		Panel = new();
		Panel.Width.Pixels = 350;
		Panel.Height.Pixels = 400;
		Panel.Left.Percent = .5f;
		Panel.Top.Percent = .5f;
		Panel.BackgroundColor.A = 150;
		Panel.OnLeftMouseDown += Panel_OnLeftMouseDown;
		Panel.OnLeftMouseUp += Panel_OnLeftMouseUp;
		Panel.OnUpdate += Panel_OnUpdate;
		Append(Panel);

		DrawMode = new(thesameuitextureasvanilla);
		DrawMode.SetPostTex(ModContent.Request<Texture2D>(ModTexture.DrawBrush));
		DrawMode.HighlightColor = DrawMode.OriginalColor * .5f;
		DrawMode.SwapHightlightColorWithOriginalColor();
		DrawMode.OnLeftClick += DrawMode_OnLeftClick;
		DrawMode.HoverText = "Draw mode";
		Panel.Append(DrawMode);

		FillMode = new(thesameuitextureasvanilla);
		FillMode.SetPostTex(ModContent.Request<Texture2D>(ModTexture.FillBucket));
		FillMode.HAlign = MathHelper.Lerp(0, 1f, 1 / 5f);
		FillMode.HighlightColor = FillMode.OriginalColor * .5f;
		FillMode.SwapHightlightColorWithOriginalColor();
		FillMode.OnLeftClick += FillMode_OnLeftClick;
		FillMode.HoverText = "Fill mode";
		Panel.Append(FillMode);

		DeleteMode = new(thesameuitextureasvanilla);
		DeleteMode.SetPostTex(TextureAssets.Trash);
		DeleteMode.HAlign = MathHelper.Lerp(0, 1f, 2 / 5f);
		DeleteMode.HighlightColor = DeleteMode.OriginalColor * .5f;
		DeleteMode.OnLeftClick += DeleteMode_OnLeftClick;
		DeleteMode.SwapHightlightColorWithOriginalColor();
		DeleteMode.HoverText = "Delete mode";
		Panel.Append(DeleteMode);

		OverrideMode = new(thesameuitextureasvanilla);
		OverrideMode.HAlign = MathHelper.Lerp(0, 1f, 3 / 5f);
		OverrideMode.HighlightColor = OverrideMode.OriginalColor * .5f;
		OverrideMode.OnLeftClick += OverrideMode_OnLeftClick;
		OverrideMode.SwapHightlightColorWithOriginalColor();
		OverrideMode.HoverText = "Override mode";
		Panel.Append(OverrideMode);

		TileMode = new(thesameuitextureasvanilla);
		TileMode.SetPostTex(TextureAssets.Item[ItemID.StoneBlock], attemptToLoad: true);
		TileMode.HAlign = MathHelper.Lerp(0, 1f, 4 / 5f);
		TileMode.HighlightColor = TileMode.OriginalColor * .5f;
		TileMode.OnLeftClick += TileMode_OnLeftClick;
		TileMode.SwapHightlightColorWithOriginalColor();
		TileMode.HoverText = "Tile";
		Panel.Append(TileMode);

		WallMode = new(thesameuitextureasvanilla);
		WallMode.SetPostTex(TextureAssets.Item[ItemID.StoneWall], attemptToLoad: true);
		WallMode.HAlign = 1;
		WallMode.HighlightColor = WallMode.OriginalColor * .5f;
		WallMode.OnLeftClick += WallMode_OnLeftClick;
		WallMode.SwapHightlightColorWithOriginalColor();
		WallMode.HoverText = "Wall";
		Panel.Append(WallMode);

		Main_CreateMode_Panel = new();
		Main_CreateMode_Panel.MaxWidth = Panel.Width;
		Main_CreateMode_Panel.Width.Pixels = 352;
		Main_CreateMode_Panel.Height.Pixels = 110;
		Main_CreateMode_Panel.MarginLeft = -11;
		Main_CreateMode_Panel.MarginRight = -11;
		Main_CreateMode_Panel.BackgroundColor.A = 100;
		Main_CreateMode_Panel.MarginTop = DrawMode.GetInnerDimensions().Height + 10;
		Panel.Append(Main_CreateMode_Panel);

		CreateMode = new(thesameuitextureasvanilla);
		CreateMode.SetPostTex(ModContent.Request<Texture2D>(ModTexture.AddSprite));
		CreateMode.HighlightColor = DrawMode.OriginalColor * .5f;
		CreateMode.SwapHightlightColorWithOriginalColor();
		CreateMode.OnLeftClick += CreateMode_OnLeftClick;
		CreateMode.HoverText = "Create mode";
		Main_CreateMode_Panel.Append(CreateMode);

		OutlineMode = new(thesameuitextureasvanilla);
		OutlineMode.SetPostTex(ModContent.Request<Texture2D>(ModTexture.Ring));
		OutlineMode.HighlightColor = DrawMode.OriginalColor * .5f;
		OutlineMode.MarginLeft = CreateMode.GetInnerDimensions().Width + 10;
		OutlineMode.SwapHightlightColorWithOriginalColor();
		OutlineMode.OnLeftClick += OutlineMode_OnLeftClick;
		OutlineMode.HoverText = "Outline only";
		Main_CreateMode_Panel.Append(OutlineMode);

		txb_width = new("");
		txb_width.OnHoverText = "Width";
		txb_width.HAlign = 1f;
		txb_width.Width.Percent = .49f;
		Main_CreateMode_Panel.Append(txb_width);

		txb_height = new("");
		txb_height.OnHoverText = "Height";
		txb_height.HAlign = 1f;
		txb_height.Width.Percent = .49f;
		txb_height.MarginTop = txb_width.GetOuterDimensions().Height + 5;
		Main_CreateMode_Panel.Append(txb_height);
	}

	private void OutlineMode_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		GeneralBuilderToolSystem.OutlineMode = !GeneralBuilderToolSystem.OutlineMode;
		OutlineMode.Highlight = GeneralBuilderToolSystem.OutlineMode;
	}

	public Vector2 createPosition = Vector2.Zero;
	private void CreateMode_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		GeneralBuilderToolSystem.CurrentMode = GeneralBuilderToolSystem.Create;
		VisualUpdateBaseOnMode();
		if (createPosition == Vector2.Zero) {
			return;
		}
		else {
			PlaceTile(new Point((int)point1.X, (int)point1.Y), new Point((int)point2.X, (int)point2.Y));
			point1 = Vector2.Zero;
			point2 = Vector2.Zero;
			createPosition = Vector2.Zero;
			ModContent.GetInstance<GeneralBuilderToolSystem>().createPosition = Vector2.Zero;
		}
	}
	private void PlaceTile(Point position1, Point position2) {
		Player player = Main.LocalPlayer;
		int minX = Math.Min(position1.X, position2.X);
		int maxX = Math.Max(position1.X, position2.X);
		int minY = Math.Min(position1.Y, position2.Y);
		int maxY = Math.Max(position1.Y, position2.Y);
		bool SearchedForTile, SearchedForWall = SearchedForTile = false;
		for (int i = 0; i < player.inventory.Length; i++) {
			if (SearchedForTile && GeneralBuilderToolSystem.Tile && !GeneralBuilderToolSystem.Wall) {
				break;
			}
			else if (SearchedForWall && GeneralBuilderToolSystem.Wall && !GeneralBuilderToolSystem.Tile) {
				break;
			}
			else if (!GeneralBuilderToolSystem.Tile && !GeneralBuilderToolSystem.Wall) {
				break;
			}
			else if (SearchedForTile && SearchedForWall) {
				break;
			}
			Item item = player.inventory[i];
			if (GeneralBuilderToolSystem.Tile && !SearchedForTile && (item.favorited && item.createTile != -1 || GeneralBuilderToolSystem.DeleteMode)) {
				for (int x = minX; x <= maxX; x++) {
					for (int y = minY; y <= maxY; y++) {
						if ((x == minX || x == maxX || y == minY || y == maxY) && GeneralBuilderToolSystem.OutlineMode || !GeneralBuilderToolSystem.OutlineMode) {
							if (GeneralBuilderToolSystem.DeleteMode) {
								WorldGen.KillTile(x, y, noItem: true);
							}
							else if (GeneralBuilderToolSystem.OverrideMode) {
								if (Main.tile[x, y].TileType != item.createTile) {
									WorldGen.KillTile(x, y, noItem: true);
									WorldGen.PlaceTile(x, y, item.createTile, true, style: item.placeStyle);
								}
							}
							else {
								WorldGen.PlaceTile(x, y, item.createTile, true, style: item.placeStyle);
							}
						}
					}
				}
				SearchedForTile = true;
				continue;
			}
			if (GeneralBuilderToolSystem.Wall && !SearchedForWall && (item.favorited && item.createWall != -1 || GeneralBuilderToolSystem.DeleteMode)) {
				for (int x = minX; x <= maxX; x++) {
					for (int y = minY; y <= maxY; y++) {
						if (GeneralBuilderToolSystem.DeleteMode) {
							Main.tile[x, y].Get<WallTypeData>().Type = WallID.None;
							WorldGen.SquareWallFrame(x, y);
						}
						else if (GeneralBuilderToolSystem.OverrideMode) {
							if (Main.tile[x, y].WallType != item.createWall) {
								Main.tile[x, y].Get<WallTypeData>().Type = WallID.None;
								WorldGen.PlaceWall(x, y, item.createWall, true);
							}
						}
						else {
							WorldGen.PlaceWall(x, y, item.createWall, true);
						}
					}
				}
				SearchedForWall = true;
				continue;
			}
		}
	}
	private void TileMode_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		GeneralBuilderToolSystem.Tile = !GeneralBuilderToolSystem.Tile;
		TileMode.Highlight = GeneralBuilderToolSystem.Tile;
	}

	private void WallMode_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		GeneralBuilderToolSystem.Wall = !GeneralBuilderToolSystem.Wall;
		WallMode.Highlight = GeneralBuilderToolSystem.Wall;
	}

	private void OverrideMode_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		GeneralBuilderToolSystem.OverrideMode = !GeneralBuilderToolSystem.OverrideMode;
		GeneralBuilderToolSystem.DeleteMode = false;
		VisualUpdateBaseOnMode();
	}

	private void DeleteMode_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		GeneralBuilderToolSystem.DeleteMode = !GeneralBuilderToolSystem.DeleteMode;
		GeneralBuilderToolSystem.OverrideMode = false;
		VisualUpdateBaseOnMode();
	}

	private void DrawMode_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		GeneralBuilderToolSystem.CurrentMode = GeneralBuilderToolSystem.Draw;
		VisualUpdateBaseOnMode();
	}

	private void FillMode_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		GeneralBuilderToolSystem.CurrentMode = GeneralBuilderToolSystem.Fill;
		VisualUpdateBaseOnMode();
	}
	/// <summary>
	/// Doesn't support tile and wall mode due to it being too basic
	/// </summary>
	private void VisualUpdateBaseOnMode() {
		DeleteMode.Highlight = GeneralBuilderToolSystem.DeleteMode;
		OverrideMode.Highlight = GeneralBuilderToolSystem.OverrideMode;
		DrawMode.Highlight = false;
		FillMode.Highlight = false;
		CreateMode.Highlight = false;
		switch (GeneralBuilderToolSystem.CurrentMode) {
			case GeneralBuilderToolSystem.None:
				break;
			case GeneralBuilderToolSystem.Draw:
				DrawMode.Highlight = true;
				break;
			case GeneralBuilderToolSystem.Fill:
				FillMode.Highlight = true;
				break;
			case GeneralBuilderToolSystem.Create:
				CreateMode.Highlight = true;
				break;
		}
	}
	public override void LeftClick(UIMouseEvent evt) {
	}
	Vector2 offsetExtra = Vector2.Zero;
	Vector2 lastPressedPositionDistance = Vector2.Zero;
	private void Panel_OnLeftMouseUp(UIMouseEvent evt, UIElement listeningElement) {
		if (evt.Target != listeningElement) {
			return;
		}
		dragging = false;
	}

	private void Panel_OnLeftMouseDown(UIMouseEvent evt, UIElement listeningElement) {
		if (evt.Target != listeningElement) {
			return;
		}
		if (!dragging) {
			offsetExtra = lastPressedPositionDistance - evt.MousePosition;
		}
		dragging = true;
		Vector2 pos = Panel.GetInnerDimensions().Position();
		Vector2 distanceFromWhereTopLeftToMouse = pos - evt.MousePosition;
		offset = distanceFromWhereTopLeftToMouse;
	}

	private void Panel_OnUpdate(UIElement affectedElement) {
		if (dragging) {
			Panel.Left.Set(Main.mouseX - offset.X + offsetExtra.X * 2, 0f); // Main.MouseScreen.X and Main.mouseX are the same
			Panel.Top.Set(Main.mouseY - offset.Y + offsetExtra.Y * 2, 0f);
			Panel.Recalculate();
		}
		else {
			lastPressedPositionDistance = Panel.GetOuterDimensions().Position();
		}
		affectedElement.Disable_MouseItemUsesWhenHoverOverAUI();
	}

	private Vector2 offset;
	private bool dragging;
	private Vector2 point1, point2;
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
	}
	public override void Draw(SpriteBatch spriteBatch) {
		base.Draw(spriteBatch);
		int num1, num2;
		if (!int.TryParse(txb_width.Text, out num1)) {
			return;
		}
		if (!int.TryParse(txb_height.Text, out num2)) {
			return;
		}
		createPosition = ModContent.GetInstance<GeneralBuilderToolSystem>().createPosition;
		if (createPosition == Vector2.Zero || !createPosition.IsCloseToPosition(Main.LocalPlayer.Center, 2000)) {
			return;
		}
		createPosition = createPosition.ToTileCoordinates16().ToVector2();
		Texture2D tex = ModContent.Request<Texture2D>("Roguelike/Texture/StructureHelper_corner").Value;
		point1 = createPosition;
		point2 = createPosition + new Vector2(num1, num2);

		Texture2D tex2 = ModContent.Request<Texture2D>("Roguelike/Texture/StructureHelper_Box").Value;

		Point16 topLeft = new Point16(
			(int)(point1.X < point2.X ? point1.X : point2.X),
			(int)(point1.Y < point2.Y ? point1.Y : point2.Y));

		Point16 bottomRight = new Point16(
			(int)(point1.X > point2.X ? point1.X : point2.X),
			(int)(point1.Y > point2.Y ? point1.Y : point2.Y)
			);

		int Width = bottomRight.X - topLeft.X;
		int Height = bottomRight.Y - topLeft.Y;

		var target = new Rectangle((int)(topLeft.X * 16 - Main.screenPosition.X), (int)(topLeft.Y * 16 - Main.screenPosition.Y), Width * 16 + 16, Height * 16 + 16);
		ModUtils.DrawOutline(spriteBatch, target, Color.Lerp(Color.Gold, Color.White, 0.5f + 0.5f * (float)Math.Sin(Main.GameUpdateCount * 0.2f)));
		spriteBatch.Draw(tex2, target, tex2.Frame(), Color.White * 0.15f);

		spriteBatch.Draw(tex, createPosition * 16 - Main.screenPosition, null, Color.Yellow, 0, tex.Size() * .5f, 1, SpriteEffects.None, 0);
	}
}
internal class GeneralBuilderTool : ModItem {
	public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.CelestialWand);
	public override void SetDefaults() {
		Item.width = Item.height = 32;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.useAnimation = Item.useTime = 1;
		Item.noUseGraphic = true;
		Item.consumable = false;
		Item.autoReuse = true;
	}
	public Point position1 = new();
	public Point position2 = new();
	public Point oldMousePosition = new();
	public override void ModifyTooltips(List<TooltipLine> tooltips) {
		string text = "";
		if (GeneralBuilderToolSystem.CurrentMode == GeneralBuilderToolSystem.Fill) {
			text = "Current Mode : Fill";
		}
		else if (GeneralBuilderToolSystem.CurrentMode == GeneralBuilderToolSystem.Draw) {
			text = "Current Mode : Draw";
		}
		else if (GeneralBuilderToolSystem.CurrentMode == GeneralBuilderToolSystem.Create) {
			text = "Current Mode : Create";
		}
		else {
			text = "Alt click to select Fill or Draw mode";
		}
		if (GeneralBuilderToolSystem.DeleteMode) {
			text += "\nDelete Mode";
		}
		else if (GeneralBuilderToolSystem.OverrideMode) {
			text += "\nOverride Mode";
		}
		if (GeneralBuilderToolSystem.Tile) {
			text += "\nPlace tile";
		}
		if (GeneralBuilderToolSystem.Wall) {
			text += "\nPlace wall";
		}
		TooltipLine line = new(Mod, "Text", text);
		tooltips.Add(line);
	}
	public override bool AltFunctionUse(Player player) => true;
	public override void HoldItem(Player player) {
		GeneralBuilderToolSystem system = ModContent.GetInstance<GeneralBuilderToolSystem>();
		if (GeneralBuilderToolSystem.CurrentMode == GeneralBuilderToolSystem.Fill) {
			Item.autoReuse = false;
		}
		else {
			Item.autoReuse = true;
		}
	}
	public override bool? UseItem(Player player) {
		GeneralBuilderToolSystem system = ModContent.GetInstance<GeneralBuilderToolSystem>();
		if (player.altFunctionUse == 2 && player.ItemAnimationJustStarted) {
			system.ToggleUI();
		}
		else if (GeneralBuilderToolSystem.CurrentMode == GeneralBuilderToolSystem.Fill) {
			return FillFunction(player);
		}
		else if (GeneralBuilderToolSystem.CurrentMode == GeneralBuilderToolSystem.Draw) {
			return DrawFunction(player);
		}
		else if (GeneralBuilderToolSystem.CurrentMode == GeneralBuilderToolSystem.Create) {
			return CreateSelection();
		}
		return base.UseItem(player);
	}
	public bool DrawFunction(Player player) {
		Point point = Main.MouseWorld.ToTileCoordinates();
		if (Main.mouseLeftRelease) {
			oldMousePosition = new();
			position1 = new();
			position2 = new();
		}
		if (Main.mouseLeft) {
			if (oldMousePosition.X == 0 && oldMousePosition.Y == 0) {
				oldMousePosition = point;
			}
			GeneralPlaceTileFunction(player, point, oldMousePosition);
		}
		oldMousePosition = point;
		return false;
	}
	public bool FillFunction(Player player) {
		if (Main.mouseLeft) {
			if (position1.X == 0 && position1.Y == 0) {
				position1 = Main.MouseWorld.ToTileCoordinates();
				Main.NewText("First position selected");
				return false;
			}
			if (position2.X == 0 && position2.Y == 0) {
				position2 = Main.MouseWorld.ToTileCoordinates();
				Main.NewText("Second position selected, ready to fill tile");
				return false;
			}
			GeneralPlaceTileFunction(player, position1, position2);
			position1 = new();
			position2 = new();
			Main.NewText("Resetted position");
			return false;
		}
		Main.NewText("Fail to find nearest favorited tile");
		return false;
	}
	public bool CreateSelection() {
		if (Main.mouseLeft) {
			ModContent.GetInstance<GeneralBuilderToolSystem>().createPosition = Main.MouseWorld;
		}
		return false;
	}
	public static void GeneralPlaceTileFunction(Player player, Point position1, Point position2) {
		int minX = Math.Min(position1.X, position2.X);
		int maxX = Math.Max(position1.X, position2.X);
		int minY = Math.Min(position1.Y, position2.Y);
		int maxY = Math.Max(position1.Y, position2.Y);
		bool SearchedForTile, SearchedForWall = SearchedForTile = false;
		for (int i = 0; i < player.inventory.Length; i++) {
			if (SearchedForTile && GeneralBuilderToolSystem.Tile && !GeneralBuilderToolSystem.Wall) {
				break;
			}
			else if (SearchedForWall && GeneralBuilderToolSystem.Wall && !GeneralBuilderToolSystem.Tile) {
				break;
			}
			else if (!GeneralBuilderToolSystem.Tile && !GeneralBuilderToolSystem.Wall) {
				break;
			}
			else if (SearchedForTile && SearchedForWall) {
				break;
			}
			Item item = player.inventory[i];
			if (GeneralBuilderToolSystem.Tile && !SearchedForTile && (item.favorited && item.createTile != -1 || GeneralBuilderToolSystem.DeleteMode)) {
				for (int x = minX; x <= maxX; x++) {
					for (int y = minY; y <= maxY; y++) {
						if (GeneralBuilderToolSystem.DeleteMode) {
							WorldGen.KillTile(x, y, noItem: true);
						}
						else if (GeneralBuilderToolSystem.OverrideMode) {
							if (Main.tile[x, y].TileType != item.createTile) {
								WorldGen.KillTile(x, y, noItem: true);
								WorldGen.PlaceTile(x, y, item.createTile, true, style: item.placeStyle);
							}
						}
						else {
							WorldGen.PlaceTile(x, y, item.createTile, true, style: item.placeStyle);
						}
					}
				}
				SearchedForTile = true;
				continue;
			}
			if (GeneralBuilderToolSystem.Wall && !SearchedForWall && (item.favorited && item.createWall != -1 || GeneralBuilderToolSystem.DeleteMode)) {
				for (int x = minX; x <= maxX; x++) {
					for (int y = minY; y <= maxY; y++) {
						if (GeneralBuilderToolSystem.DeleteMode) {
							Main.tile[x, y].Get<WallTypeData>().Type = WallID.None;
							WorldGen.SquareWallFrame(x, y);
						}
						else if (GeneralBuilderToolSystem.OverrideMode) {
							if (Main.tile[x, y].WallType != item.createWall) {
								Main.tile[x, y].Get<WallTypeData>().Type = WallID.None;
								WorldGen.PlaceWall(x, y, item.createWall, true);
							}
						}
						else {
							WorldGen.PlaceWall(x, y, item.createWall, true);
						}
					}
				}
				SearchedForWall = true;
				continue;
			}
		}
	}
}
