using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Roguelike.Common.Global;
using Roguelike.Common.Systems.ArcaneMode;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.Lootbox.Lootpool;
using Roguelike.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace Roguelike.Common.Systems.ShopSystem;
public class OpenArcadeShopItem : ModItem {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.width = Item.height = 32;
		Item.useTime = Item.useAnimation = 15;
		Item.useStyle = ItemUseStyleID.HoldUp;
		Item.noUseGraphic = true;
		Item.consumable = true;
	}
	public override bool? UseItem(Player player) {
		if (player.ItemAnimationJustStarted) {
			ModContent.GetInstance<UniversalSystem>().ActivateArcadeShopUI();
			return true;
		}
		return base.UseItem(player);
	}
}
public class ArcadeShopUI : UIState {
	public Roguelike_UIPanel panel_Shop;
	//Header
	public Roguelike_UITextPanel txpanel_ShopHeaderText;
	public ExitUI exitButton;
	//Body
	public Roguelike_UIPanel panel_Shop_BodyMain;

	public Roguelike_UITextPanel txpanel_ShoptextRow1;
	public Roguelike_UIPanel panel_Shop_ShopRow1;
	public ArcadeShopUIButton[] Arr_ShopUIButtonRow1;

	public Roguelike_UITextPanel txpanel_ShoptextRow2;
	public Roguelike_UIPanel panel_Shop_ShopRow2;
	public ArcadeShopUIButton[] Arr_ShopUIButtonRow2;

	public Roguelike_UITextPanel txpanel_PointCounter;
	public Roguelike_UITextPanel txpanel_Price;

	//Footer
	public override void OnInitialize() {
		panel_Shop = new Roguelike_UIPanel();
		panel_Shop.UISetWidthHeight(600, 780);
		panel_Shop.HAlign = .5f;
		panel_Shop.VAlign = .5f;
		Append(panel_Shop);

		txpanel_ShopHeaderText = new($"[i:{ItemID.FallenStar}] Shop [i:{ItemID.FallenStar}]", 2);
		txpanel_ShopHeaderText.Width.Percent = 1;
		txpanel_ShopHeaderText.Height.Percent = .05f;
		panel_Shop.Append(txpanel_ShopHeaderText);

		exitButton = new();
		exitButton.HAlign = 1f;
		exitButton.VAlign = .5f;
		exitButton.UISetWidthHeight(52, 52);
		txpanel_ShopHeaderText.Append(exitButton);

		panel_Shop_BodyMain = new();
		panel_Shop_BodyMain.Width.Percent = 1f;
		panel_Shop_BodyMain.Height.Percent = .9f;
		panel_Shop_BodyMain.VAlign = 1;
		panel_Shop_BodyMain.SetPadding(0);
		panel_Shop_BodyMain.PaddingTop = 10;
		panel_Shop_BodyMain.BorderColor = new(0, 0, 0, 0);
		panel_Shop_BodyMain.BackgroundColor = new(0, 0, 0, 0);
		panel_Shop.Append(panel_Shop_BodyMain);

		txpanel_ShoptextRow1 = new($"[i:{ItemID.FallenStar}] Special offer ! [i:{ItemID.FallenStar}]");
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

		Arr_ShopUIButtonRow1 = new ArcadeShopUIButton[10];
		for (int i = 0; i < Arr_ShopUIButtonRow1.Length; i++) {
			ArcadeShopUIButton shop = new ArcadeShopUIButton();
			shop.HAlign = i / 9f;
			Arr_ShopUIButtonRow1[i] = shop;
			panel_Shop_ShopRow1.Append(Arr_ShopUIButtonRow1[i]);
		}


		txpanel_ShoptextRow2 = new($"[i:{ItemID.FallenStar}] Normal offer [i:{ItemID.FallenStar}]");
		txpanel_ShoptextRow2.Width.Percent = 1;
		txpanel_ShoptextRow2.Height.Pixels = 80;
		txpanel_ShoptextRow2.TextHAlign = 0;
		txpanel_ShoptextRow2.MarginTop = panel_Shop_ShopRow1.MarginTop + panel_Shop_ShopRow1.GetHeight() + 5;
		panel_Shop_BodyMain.Append(txpanel_ShoptextRow2);

		panel_Shop_ShopRow2 = new();
		panel_Shop_ShopRow2.Width.Percent = 1f;
		panel_Shop_ShopRow2.Height.Pixels = 520;
		panel_Shop_ShopRow2.BorderColor = new(0, 0, 0, 0);
		panel_Shop_ShopRow2.BackgroundColor = new(0, 0, 0, 0);
		panel_Shop_ShopRow2.SetPadding(0);
		panel_Shop_ShopRow2.MarginTop = txpanel_ShoptextRow2.MarginTop + txpanel_ShoptextRow2.GetHeight();
		panel_Shop_BodyMain.Append(panel_Shop_ShopRow2);

		Arr_ShopUIButtonRow2 = new ArcadeShopUIButton[60];
		int len = Arr_ShopUIButtonRow2.Length;
		for (int i = 0; i < len; i++) {
			ArcadeShopUIButton shop = new ArcadeShopUIButton();
			shop.HAlign = i % 10 / 9f;
			shop.VAlign = (i / 10) / (len / 10f);
			Arr_ShopUIButtonRow2[i] = shop;
			panel_Shop_ShopRow2.Append(Arr_ShopUIButtonRow2[i]);
		}

		txpanel_PointCounter = new("", 1.2f);
		txpanel_PointCounter.UISetWidthHeight(250, 50);
		txpanel_PointCounter.OnUpdate += Txpanel_PointCounter_OnUpdate;
		txpanel_PointCounter.HAlign = .5f;
		txpanel_PointCounter.TextHAlign = 0;
		txpanel_PointCounter.MarginLeft = panel_Shop.GetOuterDimensions().Width + 250 + 20;
		txpanel_PointCounter.Top.Set(panel_Shop.GetOuterDimensions().Y, 0);
		Append(txpanel_PointCounter);
	}

	private void Txpanel_PointCounter_OnUpdate(UIElement affectedElement) {
		txpanel_PointCounter.SetText($"Point: {ModContent.GetInstance<ArcadeMode_System>().Point}");
	}

	public override void OnActivate() {
		for (int i = 0; i < Arr_ShopUIButtonRow1.Length; i++) {
			Arr_ShopUIButtonRow1[i].SetItem(ContentSamples.ItemsByType[Main.rand.Next(ModItemLib.ListLootboxType)]);
		}
		ItemPool pool = LootboxSystem.GetItemPool(ItemPool.GetPoolType<RainbowLootboxPool>());
		var accpool = pool.AccessoryLoot().ToArray();
		var armorpool = pool.ArmorLoot().ToArray();
		var potionpool = pool.PotionPool().ToArray();
		var itempool = pool.AllWeaponPool().ToArray();
		List<int> allPool = [.. itempool, .. potionpool, .. accpool, .. armorpool];
		for (int i = 0; i < Arr_ShopUIButtonRow2.Length; i++) {
			Arr_ShopUIButtonRow2[i].SetItem(new Item(Main.rand.Next(allPool)));
		}
	}
}
public class ArcadeShopUIButton : Roguelike_UIImageButton {
	public Asset<Texture2D> baseAsset;
	private Item item = new Item();
	public int Value = 0;
	Roguelike_UIText priceTag;
	public ArcadeShopUIButton() : base(TextureAssets.InventoryBack) {
		baseAsset = TextureAssets.InventoryBack;
		Width.Pixels = baseAsset.Width();
		Height.Pixels = baseAsset.Height();
		SetVisibility(.6f, 1f);
	}
	public override void OnInitialize() {
		priceTag = new($"{Value} p", .65f);
		priceTag.Top.Pixels = this.GetOuterDimensions().Height + 5;
		priceTag.HAlign = .5f;
		Append(priceTag);
	}
	public void SetItem(Item sellitem) {
		item = sellitem;
		SetVisibility(.6f, 1f);
		UnselectAble = false;
		int level = Main.rand.Next(0, 5);
		float chanceForRareHighLevel = .2f;
		while (Main.rand.NextFloat() <= chanceForRareHighLevel) {
			level += Main.rand.Next(0, 5);
		}
		if (level != 0) {
			item.GetGlobalItem<GlobalItemHandle>().SetItemLevel(level);
		}
		Value = Math.Clamp(item.value / 1000 + 100 * level, 0, 9999999);
		//priceTag.SetText($"{Value}p");
	}
	public override void UpdateOuter(GameTime gametime) {
		if (!UnselectAble) {
			priceTag.SetText($"{Value} p");
		}
	}
	public override void LeftClick(UIMouseEvent evt) {
		if (ModContent.GetInstance<ArcadeMode_System>().Point >= Value) {
			ModContent.GetInstance<ArcadeMode_System>().Point -= Value;
			Main.LocalPlayer.QuickSpawnItem(new EntitySource_Misc("Shop"), item);
			UnselectAble = true;
			SetVisibility(.4f, .4f);
			priceTag.SetText("Sold !");
		}
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
		Color color = Color.White;
		if (UnselectAble) {
			color.A = 100;
		}
		spriteBatch.Draw(item, drawPos, null, color, 0, origin, scale, SpriteEffects.None, 0);
		if (IsMouseHovering) {
			Main.HoverItem = this.item.Clone();
			Main.hoverItemName = "\n" + this.item.HoverName;
		}
	}
}
