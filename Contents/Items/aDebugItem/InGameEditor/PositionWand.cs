using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Core.Platforms;
using ReLogic.Content;
using Roguelike.Common.Systems;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Roguelike.Contents.Items.aDebugItem.InGameEditor;
public enum PositionWandMode : byte {
	None,
	Selecting,
	Marking,
	Saving,
}
public class PositionWandSystem : ModSystem {
	public List<Point> list_Point = new List<Point>();
	public static PositionWandMode mode = PositionWandMode.None;
	//These 2 point below are the position that player select to as focus area to mark the pos.
	public Point position1 = new();
	public Point position2 = new();
	public PositionWandUI PosWandUI;
	internal UserInterface userInterface;
	public override void Load() {
		if (!Main.dedServ) {
			userInterface = new();
			PosWandUI = new();
		}
		list_Point = new();
	}
	public override void Unload() {
		list_Point = new();
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
		userInterface.SetState(PosWandUI);
	}
}
public class PositionWandUI : UIState {
	public UIPanel Panel;
	public Roguelike_UIImage btn_SelectingPos;
	public Roguelike_UIImage btn_MarkingPos;
	public Roguelike_UIImage btn_SavingPos;
	public Roguelike_UIImageButton btn_confirm;
	public Roguelike_UIImageButton btn_cancel;
	public Roguelike_UITextPanel textPanel;
	public Roguelike_TextBox txt_FileName;
	public Roguelike_UIPanel panel;
	public override void OnInitialize() {
		Asset<Texture2D> thesameuitextureasvanilla = TextureAssets.InventoryBack7;

		Panel = new();
		Panel.Width.Pixels = 250;
		Panel.Height.Pixels = 100;
		Panel.Left.Percent = .5f;
		Panel.Top.Percent = .5f;
		Panel.BackgroundColor.A = 255;
		Panel.OnLeftMouseDown += Panel_OnLeftMouseDown;
		Panel.OnLeftMouseUp += Panel_OnLeftMouseUp;
		Panel.OnUpdate += Panel_OnUpdate;
		Append(Panel);

		btn_SelectingPos = new(thesameuitextureasvanilla);
		//btn_SelectingPos.SetPostTex(ModContent.Request<Texture2D>(ModTexture.DrawBrush));
		btn_SelectingPos.VAlign = .5f;
		btn_SelectingPos.HighlightColor = btn_SelectingPos.OriginalColor * .5f;
		btn_SelectingPos.SwapHightlightColorWithOriginalColor();
		btn_SelectingPos.OnLeftClick += Btn_SelectingPos_OnLeftClick; ;
		btn_SelectingPos.HoverText = "Selecting mode";
		Panel.Append(btn_SelectingPos);

		btn_MarkingPos = new(thesameuitextureasvanilla);
		//btn_MarkingPos.SetPostTex(ModContent.Request<Texture2D>(ModTexture.DrawBrush));
		btn_MarkingPos.VAlign = .5f;
		btn_MarkingPos.HighlightColor = btn_MarkingPos.OriginalColor * .5f;
		btn_MarkingPos.HAlign = .5f;
		btn_MarkingPos.SwapHightlightColorWithOriginalColor();
		btn_MarkingPos.OnLeftClick += Btn_MarkingPos_OnLeftClick;
		btn_MarkingPos.HoverText = "Marking mode";
		Panel.Append(btn_MarkingPos);

		btn_SavingPos = new(thesameuitextureasvanilla);
		//btn_SavingPos.SetPostTex(ModContent.Request<Texture2D>(ModTexture.DrawBrush));
		btn_SavingPos.VAlign = .5f;
		btn_SavingPos.HighlightColor = btn_SavingPos.OriginalColor * .5f;
		btn_SavingPos.HAlign = 1f;
		btn_SavingPos.SwapHightlightColorWithOriginalColor();
		btn_SavingPos.OnLeftClick += Btn_SavingPos_OnLeftClick;
		btn_SavingPos.HoverText = "Saving mode";
		Panel.Append(btn_SavingPos);

		panel = new();
		panel.HAlign = .5f;
		panel.VAlign = .5f;
		panel.UISetWidthHeight(450, 200);
		panel.OnUpdate += panel_OnUpdate;
		panel.Hide = true;
		Append(panel);

		textPanel = new("Save this structure ? Please name the file");
		textPanel.UISetWidthHeight(400, 40);
		textPanel.HAlign = .5f;
		textPanel.VAlign = .1f;
		textPanel.Hide = true;
		panel.Append(textPanel);

		txt_FileName = new("");
		txt_FileName.HAlign = .5f;
		txt_FileName.VAlign = .45f;
		txt_FileName.Hide = true;
		panel.Append(txt_FileName);

		btn_cancel = new(ModContent.Request<Texture2D>(ModTexture.ACCESSORIESSLOT));
		btn_cancel.HAlign = 0f;
		btn_cancel.VAlign = 1f;
		btn_cancel.OnLeftClick += Btn_cancel_OnLeftClick;
		btn_cancel.UISetWidthHeight(52, 52);
		btn_cancel.Hide = true;
		panel.Append(btn_cancel);

		btn_confirm = new(ModContent.Request<Texture2D>(ModTexture.ACCESSORIESSLOT));
		btn_confirm.HAlign = 1f;
		btn_confirm.VAlign = 1f;
		btn_confirm.UISetWidthHeight(52, 52);
		btn_confirm.OnLeftClick += Btn_confirm_OnLeftClick;
		btn_confirm.Hide = true;
		panel.Append(btn_confirm);
	}
	private void panel_OnUpdate(UIElement affectedElement) {
		if (panel.ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
	}

	private void Btn_cancel_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		txt_FileName.SetText("");
		VisibilityUI(true);
	}
	public void toggle_NameTextUI() {
		panel.Hide = !panel.Hide;
		textPanel.Hide = !textPanel.Hide;
		txt_FileName.Hide = !txt_FileName.Hide;
		btn_cancel.Hide = !btn_cancel.Hide;
		btn_confirm.Hide = !btn_confirm.Hide;
	}
	public void VisibilityUI(bool hide) {
		panel.Hide = hide;
		textPanel.Hide = hide;
		txt_FileName.Hide = hide;
		btn_cancel.Hide = hide;
		btn_confirm.Hide = hide;
	}
	private void Btn_confirm_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		saving();
		VisibilityUI(true);
	}
	private void saving() {
		PositionWandSystem system = ModContent.GetInstance<PositionWandSystem>();
		try {
			Point pos1 = system.position1;
			Point pos2 = system.position2;
			int X = Math.Min(pos1.X, pos2.X);
			int Y = Math.Min(pos1.Y, pos2.Y);
			string path = Path.Join(Program.SavePathShared, "RogueLikeData");
			using FileStream file = File.Create(Path.Combine(path, txt_FileName.Text));
			using StreamWriter m = new(file);
			int count = 0;
			foreach (var item in system.list_Point) {
				m.WriteLine($"Point point{++count} = new Point({item.X - X},{item.Y - Y})");
			}
		}
		catch (Exception e) {
			Main.NewText("Error has occured: " + e.Message);
		}
		system.list_Point.Clear();
		system.position1 = new();
		system.position2 = new();
		Main.NewText("saved position");
	}
	private void Btn_SavingPos_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		PositionWandSystem.mode = PositionWandMode.Saving;
		btn_SavingPos.Highlight = true;
		btn_MarkingPos.Highlight = false;
		btn_SelectingPos.Highlight = false;
	}

	private void Btn_MarkingPos_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		PositionWandSystem.mode = PositionWandMode.Marking;
		btn_SavingPos.Highlight = false;
		btn_MarkingPos.Highlight = true;
		btn_SelectingPos.Highlight = false;
		VisibilityUI(true);
	}

	private void Btn_SelectingPos_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		PositionWandSystem.mode = PositionWandMode.Selecting;
		btn_SavingPos.Highlight = false;
		btn_MarkingPos.Highlight = false;
		btn_SelectingPos.Highlight = true;
		VisibilityUI(true);
	}
	public override void Draw(SpriteBatch spriteBatch) {
		base.Draw(spriteBatch);
		PositionWandSystem system = ModContent.GetInstance<PositionWandSystem>();
		Point emptyPoint = new Point(0, 0);
		Texture2D tex = ModContent.Request<Texture2D>("Roguelike/Texture/StructureHelper_corner").Value;
		if (system.list_Point != null) {
			foreach (Point pos in system.list_Point) {
				spriteBatch.Draw(tex, pos.ToWorldCoordinates() - Main.screenPosition, tex.Frame(), Color.Red, 0, tex.Frame().Size() / 2, 1, 0, 0);
			}
		}

		Vector2 pos1 = system.position1.ToWorldCoordinates();
		Vector2 pos2 = system.position2.ToWorldCoordinates();
		int X = (int)Math.Min(pos1.X, pos2.X);
		int Y = (int)Math.Min(pos1.Y, pos2.Y);
		int W = (int)Math.Max(pos1.X, pos2.X) - X;
		int H = (int)Math.Max(pos1.Y, pos2.Y) - Y;

		Vector2 topLeft = new Vector2(X, Y);
		if (system.position1 != emptyPoint) {
			spriteBatch.Draw(tex, pos1 - Main.screenPosition, tex.Frame(), Color.Black, 0, tex.Frame().Size() / 2, 1, 0, 0);
		}
		if (system.position2 != emptyPoint) {
			spriteBatch.Draw(tex, pos2 - Main.screenPosition, tex.Frame(), Color.White, 0, tex.Frame().Size() / 2, 1, 0, 0);
		}
		if (system.position1 == emptyPoint || system.position2 == emptyPoint) {
			return;
		}

		Texture2D tex2 = ModContent.Request<Texture2D>("Roguelike/Texture/StructureHelper_Box").Value;

		var target = new Rectangle((int)(topLeft.X - Main.screenPosition.X), (int)(topLeft.Y - Main.screenPosition.Y), W, H);
		ModUtils.DrawOutline(spriteBatch, target, Color.Lerp(Color.Gold, Color.White, 0.5f + 0.5f * (float)Math.Sin(Main.GameUpdateCount * 0.2f)));
		spriteBatch.Draw(tex2, target, tex2.Frame(), Color.White * 0.15f);
	}

	Vector2 offsetExtra = Vector2.Zero;
	Vector2 lastPressedPositionDistance = Vector2.Zero;
	bool dragging = false;
	Vector2 offset = Vector2.Zero;
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
	}
}
internal class PositionWand : ModItem {
	public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.CelestialWand);
	public override void SetDefaults() {
		Item.width = Item.height = 32;
		Item.useTime = Item.useAnimation = 15;
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.UseSound = SoundID.MaxMana with { Pitch = 1f };
		Item.noUseGraphic = true;
	}
	public override bool AltFunctionUse(Player player) => true;
	public override bool? UseItem(Player player) {
		PositionWandSystem system = ModContent.GetInstance<PositionWandSystem>();
		if (player.ItemAnimationJustStarted) {
			if (player.altFunctionUse == 2) {
				system.ToggleUI();
			}
			else if (PositionWandSystem.mode == PositionWandMode.Selecting) {
				return SelectFunction();
			}
			else if (PositionWandSystem.mode == PositionWandMode.Marking) {
				return MarkingFunction();
			}
			else if (PositionWandSystem.mode == PositionWandMode.Saving) {
				return SavingFunction();
			}
		}
		return base.UseItem(player);
	}
	public bool SelectFunction() {
		PositionWandSystem system = ModContent.GetInstance<PositionWandSystem>();
		if (Main.mouseLeft) {
			if (system.position1.X == 0 && system.position1.Y == 0) {
				system.position1 = Main.MouseWorld.ToTileCoordinates();
				Main.NewText("First position selected");
				return false;
			}
			if (system.position2.X == 0 && system.position2.Y == 0) {
				system.position2 = Main.MouseWorld.ToTileCoordinates();
				Main.NewText("Second position selected, ready to fill tile");
				return false;
			}
			system.position1 = new();
			system.position2 = new();
			system.list_Point.Clear();
			Main.NewText("Resetted position");
			return false;
		}
		return false;
	}
	public bool MarkingFunction() {
		PositionWandSystem system = ModContent.GetInstance<PositionWandSystem>();
		if (Main.mouseLeft) {
			system.list_Point.Add(Main.MouseWorld.ToTileCoordinates());
			Main.NewText("Added position");
			return false;
		}
		return false;
	}
	public bool SavingFunction() {
		PositionWandSystem system = ModContent.GetInstance<PositionWandSystem>();
		if (Main.mouseLeft) {
			system.PosWandUI.toggle_NameTextUI();
			return false;
		}
		return false;
	}
}
