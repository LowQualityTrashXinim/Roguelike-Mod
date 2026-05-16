using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;
using static System.Net.Mime.MediaTypeNames;

namespace Roguelike.Common.Systems.ShopSystem;
public class OpenShopItem : ModItem {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.width = Item.height = 32;
		Item.useTime = Item.useAnimation = 15;
		Item.useStyle = ItemUseStyleID.HoldUp;
		Item.noUseGraphic = true;
	}
	public override bool? UseItem(Player player) {
		if (player.ItemAnimationJustStarted) {
			ModContent.GetInstance<UniversalSystem>().ActivateShopUI();
		}
		return base.UseItem(player);
	}
}
public class ShopUI : UIState {
	public Roguelike_UIPanel panel_Shop;
	//Header
	public Roguelike_UITextPanel txpanel_ShopHeaderText;
	public ExitUI exitButton;
	//Body
	public Roguelike_UIPanel panel_Shop_BodyMain;

	public Roguelike_UITextPanel txpanel_ShoptextRow1;
	public Roguelike_UIPanel panel_Shop_ShopRow1;
	public ShopUIButton[] Arr_ShopUIButtonRow1;

	public Roguelike_UITextPanel txpanel_ShoptextRow2;
	public Roguelike_UIPanel panel_Shop_ShopRow2;
	public ShopUIButton[] Arr_ShopUIButtonRow2;
	//Footer
	public override void OnInitialize() {
		panel_Shop = new Roguelike_UIPanel();
		panel_Shop.UISetWidthHeight(600, 350);
		panel_Shop.HAlign = .5f;
		panel_Shop.VAlign = .5f;
		Append(panel_Shop);

		txpanel_ShopHeaderText = new($"[i:{ItemID.FallenStar}] Shop [i:{ItemID.FallenStar}]", 2);
		txpanel_ShopHeaderText.Width.Percent = 1;
		txpanel_ShopHeaderText.Height.Percent = .1f;
		panel_Shop.Append(txpanel_ShopHeaderText);

		exitButton = new(TextureAssets.InventoryBack);
		exitButton.HAlign = 1f;
		exitButton.VAlign = .5f;
		exitButton.UISetWidthHeight(52, 52);
		txpanel_ShopHeaderText.Append(exitButton);

		panel_Shop_BodyMain = new();
		panel_Shop_BodyMain.Width.Percent = 1f;
		panel_Shop_BodyMain.Height.Percent = .8f;
		panel_Shop_BodyMain.VAlign = 1;
		panel_Shop_BodyMain.SetPadding(0);
		panel_Shop_BodyMain.PaddingTop = 10;
		panel_Shop_BodyMain.BorderColor = new(0, 0, 0, 0);
		panel_Shop_BodyMain.BackgroundColor = new(0, 0, 0, 0);
		panel_Shop.Append(panel_Shop_BodyMain);

		txpanel_ShoptextRow1 = new($"[i:{ItemID.FallenStar}] Shop 1 [i:{ItemID.FallenStar}]");
		txpanel_ShoptextRow1.Width.Percent = 1;
		txpanel_ShoptextRow1.Height.Pixels = 80;
		txpanel_ShoptextRow1.MarginTop = 5;
		txpanel_ShoptextRow1.TextHAlign = 0;
		panel_Shop_BodyMain.Append(txpanel_ShoptextRow1);

		panel_Shop_ShopRow1 = new();
		panel_Shop_ShopRow1.Width.Percent = 1f;
		panel_Shop_ShopRow1.Height.Pixels = 80;
		panel_Shop_ShopRow1.BorderColor = new(0, 0, 0, 0);
		panel_Shop_ShopRow1.BackgroundColor = new(0, 0, 0, 0);
		panel_Shop_ShopRow1.SetPadding(0);
		panel_Shop_ShopRow1.MarginTop = txpanel_ShoptextRow1.GetHeight();
		panel_Shop_BodyMain.Append(panel_Shop_ShopRow1);

		Arr_ShopUIButtonRow1 = new ShopUIButton[10];
		for (int i = 0; i < Arr_ShopUIButtonRow1.Length; i++) {
			ShopUIButton shop = new ShopUIButton();
			shop.HAlign = i / 9f;
			Arr_ShopUIButtonRow1[i] = shop;
			panel_Shop_ShopRow1.Append(Arr_ShopUIButtonRow1[i]);
		}


		txpanel_ShoptextRow2 = new($"[i:{ItemID.FallenStar}] Shop 2 [i:{ItemID.FallenStar}]");
		txpanel_ShoptextRow2.Width.Percent = 1;
		txpanel_ShoptextRow2.Height.Pixels = 80;
		txpanel_ShoptextRow2.TextHAlign = 0;
		txpanel_ShoptextRow2.MarginTop = panel_Shop_ShopRow1.MarginTop + panel_Shop_ShopRow1.GetHeight() + 5;
		panel_Shop_BodyMain.Append(txpanel_ShoptextRow2);

		panel_Shop_ShopRow2 = new();
		panel_Shop_ShopRow2.Width.Percent = 1f;
		panel_Shop_ShopRow2.Height.Pixels = 80;
		panel_Shop_ShopRow2.BorderColor = new(0, 0, 0, 0);
		panel_Shop_ShopRow2.BackgroundColor = new(0, 0, 0, 0);
		panel_Shop_ShopRow2.SetPadding(0);
		panel_Shop_ShopRow2.MarginTop = txpanel_ShoptextRow2.MarginTop + txpanel_ShoptextRow2.GetHeight();
		panel_Shop_BodyMain.Append(panel_Shop_ShopRow2);

		Arr_ShopUIButtonRow2 = new ShopUIButton[10];
		for (int i = 0; i < Arr_ShopUIButtonRow2.Length; i++) {
			ShopUIButton shop = new ShopUIButton();
			shop.HAlign = i / 9f;
			Arr_ShopUIButtonRow2[i] = shop;
			panel_Shop_ShopRow2.Append(Arr_ShopUIButtonRow2[i]);
		}
	}
	public override void OnActivate() {
		for (int i = 0; i < Arr_ShopUIButtonRow1.Length; i++) {
			Arr_ShopUIButtonRow1[i].SetItem(new Item(ItemID.FieryGreatsword));
		}
		for (int i = 0; i < Arr_ShopUIButtonRow2.Length; i++) {
			Arr_ShopUIButtonRow2[i].SetItem(new Item(ItemID.LaserRifle));
		}
	}
}
public class ShopUIButton : Roguelike_UIImageButton {
	public Asset<Texture2D> baseAsset;
	private Item item = new Item();
	public ShopUIButton() : base(TextureAssets.InventoryBack) {
		baseAsset = TextureAssets.InventoryBack;
		Width.Pixels = baseAsset.Width();
		Height.Pixels = baseAsset.Height();
		SetVisibility(.6f, 1f);
	}
	public void SetItem(Item sellitem) {
		this.item = sellitem;
	}
	public override void LeftClick(UIMouseEvent evt) {

	}
	public override void DrawImage(SpriteBatch spriteBatch) {
		if (this.item.type >= TextureAssets.Item.Length) {
			return;
		}
		Main.instance.LoadItem(this.item.type);
		Texture2D item = TextureAssets.Item[this.item.type].Value;
		Vector2 origin = item.Size() * .5f;
		Vector2 drawPos = GetInnerDimensions().Position() + new Vector2(26, 26);
		float scale;
		if (origin.X < 27 && origin.Y < 27) {
			scale = .8f;
		}
		else {
			scale = ModUtils.Scale_OuterTextureWithInnerTexture(new(52, 52), item.Size() * 2f, 1.5f);
		}
		spriteBatch.Draw(item, drawPos, null, Color.White, 0, origin, scale, SpriteEffects.None, 0);
		if (IsMouseHovering) {
			Main.HoverItem = this.item.Clone();
			Main.hoverItemName = this.item.HoverName;
		}
	}
}
