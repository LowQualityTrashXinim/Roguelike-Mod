using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Roguelike.Common.Systems.Achievement;
using Roguelike.Common.Systems.IOhandle;
using Roguelike.Common.Utils;
using Roguelike.Contents.Transfixion.WeaponEnchantment;
using Roguelike.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace Roguelike.Common.Systems.UI;
public class EnchantmentButton : Roguelike_UIImageButton {
	public int InteralItemID = 0;
	public EnchantmentButton(Asset<Texture2D> texture, int enchantmentItemID) : base(texture) {
		SetVisibility(.67f, 1f);
		InteralItemID = enchantmentItemID;
		//InteralItemID = ModItemLib.SynergyItem.FirstOrDefault(s => s.ModItem.Name == SynergyInternalName).type;
	}
	public override void DrawImage(SpriteBatch spriteBatch) {
		if (InteralItemID < 0 || InteralItemID >= TextureAssets.Item.Length) {
			return;
		}
		Main.instance.LoadItem(InteralItemID);
		Texture2D item = TextureAssets.Item[InteralItemID].Value;
		if (IsMouseHovering) {
			Main.instance.MouseText(ContentSamples.ItemsByType[InteralItemID].Name);
		}
		Vector2 origin = item.Size() * .5f;
		Vector2 drawPos = GetInnerDimensions().Position() + new Vector2(26, 26);
		float scale;
		if (origin.X < 27 && origin.Y < 27) {
			scale = .8f;
		}
		else {
			scale = ScaleCalculation(new(52, 52), item.Size() * 2f);
		}
		spriteBatch.Draw(item, drawPos, null, Color.White, 0, origin, scale, SpriteEffects.None, 0);
	}
	private static float ScaleCalculation(Vector2 originalTexture, Vector2 textureSize) => originalTexture.Length() / textureSize.Length();
}
public enum FilterOption : byte {
	None,
	Melee,
	Range,
	Magic,
	Summon,
	Misc
}
public class EnchantmentMenuWikiUI : UIState {
	public UIPanel holderPanel;
	public UIPanel mainPanel;
	public UIPanel headerPanel;
	public UIPanel footerPanel;
	public ExitUI exit;
	Roguelike_UIImageButton buttonLeft;
	Roguelike_UIImageButton buttonRight;
	List<PageImage> pagnitation = new();
	int pageIndex = 0;
	int maxPage = 1;
	int Row = 10;
	int Line = 10;

	Roguelike_UIImage Filter_Melee;
	Roguelike_UIImage Filter_Range;
	Roguelike_UIImage Filter_Magic;
	Roguelike_UIImage Filter_Summon;
	List<EnchantmentButton> EnchantmentbuttonList = new();
	public string CurrentlySelectedSynergyWeapon = "";
	public void SetPageIndex(int index) {
		pageIndex = Math.Clamp(index, 0, maxPage);
	}
	public override void OnInitialize() {
		EnchantmentbuttonList = new();

		holderPanel = new();
		holderPanel.UISetWidthHeight(700, 800);
		holderPanel.HAlign = .5f;
		holderPanel.VAlign = .5f;
		Append(holderPanel);

		mainPanel = new();
		mainPanel.Width.Percent = 1;
		mainPanel.Height.Percent = .8f;
		mainPanel.HAlign = .5f;
		mainPanel.VAlign = .5f;
		holderPanel.Append(mainPanel);

		headerPanel = new();
		headerPanel.Width.Percent = 1;
		headerPanel.Height.Pixels = 60;
		headerPanel.PaddingTop = 5;
		headerPanel.PaddingBottom = 5;
		holderPanel.Append(headerPanel);

		footerPanel = new();
		footerPanel.VAlign = 1;
		footerPanel.Width.Percent = 1;
		footerPanel.Height.Pixels = 60;
		footerPanel.PaddingTop = 5;
		footerPanel.PaddingBottom = 5;
		holderPanel.Append(footerPanel);

		exit = new();
		exit.HAlign = 1f;
		exit.VAlign = .5f;
		headerPanel.Append(exit);

		buttonLeft = new(TextureAssets.InventoryBack);
		buttonLeft.SetVisibility(.67f, 1f);
		buttonLeft.VAlign = .5f;
		buttonLeft.postTex = ModContent.Request<Texture2D>(ModTexture.Arrow_Left);
		buttonLeft.OnLeftClick += ButtonLeft_OnLeftClick;
		footerPanel.Append(buttonLeft);

		buttonRight = new(TextureAssets.InventoryBack);
		buttonRight.SetVisibility(.67f, 1f);
		buttonRight.VAlign = .5f;
		buttonRight.HAlign = 1f;
		buttonRight.OnLeftClick += ButtonRight_OnLeftClick;
		buttonRight.postTex = ModContent.Request<Texture2D>(ModTexture.Arrow_Right);
		footerPanel.Append(buttonRight);

		Filter_Melee = new(TextureAssets.InventoryBack);
		Filter_Melee.SetPostTex(TextureAssets.Item[ItemID.WarriorEmblem], attemptToLoad: true);
		Filter_Melee.UISetWidthHeight(52, 52);
		Filter_Melee.VAlign = .5f;
		Filter_Melee.HighlightColor = Filter_Melee.OriginalColor.ScaleRGB(.4f);
		Filter_Melee.OnLeftClick += Filter_Melee_OnLeftClick;
		headerPanel.Append(Filter_Melee);

		Filter_Range = new(TextureAssets.InventoryBack);
		Filter_Range.SetPostTex(TextureAssets.Item[ItemID.RangerEmblem], attemptToLoad: true);
		Filter_Range.UISetWidthHeight(52, 52);
		Filter_Range.VAlign = .5f;
		Filter_Range.HighlightColor = Filter_Range.OriginalColor.ScaleRGB(.4f);
		Filter_Range.OnLeftClick += Filter_Range_OnLeftClick;
		Filter_Range.MarginLeft = Filter_Melee.GetInnerDimensions().Width + 10;
		headerPanel.Append(Filter_Range);

		Filter_Magic = new(TextureAssets.InventoryBack);
		Filter_Magic.SetPostTex(TextureAssets.Item[ItemID.SorcererEmblem], attemptToLoad: true);
		Filter_Magic.UISetWidthHeight(52, 52);
		Filter_Magic.VAlign = .5f;
		Filter_Magic.HighlightColor = Filter_Magic.OriginalColor.ScaleRGB(.4f);
		Filter_Magic.OnLeftClick += Filter_Magic_OnLeftClick;
		Filter_Magic.MarginLeft = (Filter_Melee.GetInnerDimensions().Width + 10) * 2;
		headerPanel.Append(Filter_Magic);

		Filter_Summon = new(TextureAssets.InventoryBack);
		Filter_Summon.SetPostTex(TextureAssets.Item[ItemID.SummonerEmblem], attemptToLoad: true);
		Filter_Summon.UISetWidthHeight(52, 52);
		Filter_Summon.VAlign = .5f;
		Filter_Summon.HighlightColor = Filter_Summon.OriginalColor.ScaleRGB(.4f);
		Filter_Summon.OnLeftClick += Filter_Summon_OnLeftClick;
		Filter_Summon.MarginLeft = (Filter_Melee.GetInnerDimensions().Width + 10) * 3;
		headerPanel.Append(Filter_Summon);


		maxPage = (int)Math.Ceiling(EnchantmentLoader.TotalCount / (float)(Line * Row));

		for (int i = 0; i < Line; i++) {
			for (int j = 0; j < Row; j++) {
				int index = Line * i + j;
				EnchantmentButton btn = new(TextureAssets.InventoryBack, EnchantmentLoader.EnchantmentcacheID[index]);
				btn.HAlign = j / (Row - 1f);
				btn.VAlign = i / (Line - 1f);
				btn.OnLeftClick += Btn_OnLeftClick;
				EnchantmentbuttonList.Add(btn);
				mainPanel.Append(btn);
			}
		}

		if (maxPage <= 1) {
			return;
		}
		for (int i = 0; i < maxPage; i++) {
			PageImage img = new(TextureAssets.InventoryBack);
			if (maxPage == 1) {
				img.HAlign = .5f;
			}
			else {
				img.HAlign = MathHelper.Lerp(.1f, .9f, i / (maxPage - 1f));
			}
			img.VAlign = .5f;
			img.OnLeftClick += Img_OnLeftClick;
			pagnitation.Add(img);
			footerPanel.Append(img);
		}
		WE_Panel = new();
		WE_Panel.Hide = true;
		WE_Panel.UISetWidthHeight(500, 500);
		WE_Panel.HAlign = .5f;
		WE_Panel.VAlign = .5f;
		WE_Panel.BackgroundColor = new(WE_Panel.BackgroundColor.R, WE_Panel.BackgroundColor.G, WE_Panel.BackgroundColor.B);
		Append(WE_Panel);

		WE_Header = new();
		WE_Header.Hide = true;
		WE_Header.Width.Percent = 1f;
		WE_Header.Height.Pixels = 80;
		WE_Panel.Append(WE_Header);

		WE_Img = new(TextureAssets.InventoryBack);
		WE_Img.Hide = true;
		WE_Img.VAlign = .5f;
		WE_Header.Append(WE_Img);

		WE_Name = new("");
		WE_Name.Hide = true;
		WE_Name.VAlign = .5f;
		WE_Name.MarginLeft = WE_Img.Width.Pixels + 30;
		WE_Header.Append(WE_Name);

		WE_Desc = new("");
		WE_Desc.Hide = true;
		WE_Desc.MarginTop = WE_Header.Height.Pixels + 30;
		WE_Desc.Height.Pixels = WE_Panel.Height.Pixels - WE_Header.Height.Pixels - WE_Desc.MarginTop + 30;
		WE_Desc.Width.Percent = 1f;
		WE_Panel.Append(WE_Desc);

		WE_Exit = new(TextureAssets.InventoryBack);
		WE_Exit.Hide = true;
		WE_Exit.HAlign = 1f;
		WE_Exit.VAlign = .5f;
		WE_Exit.OnLeftClick += WE_Exit_OnLeftClick;
		WE_Header.Append(WE_Exit);
	}
	private void Disable_HighlightForFilter() {
		Filter_Melee.Highlight = false;
		Filter_Range.Highlight = false;
		Filter_Magic.Highlight = false;
		Filter_Summon.Highlight = false;
	}
	private void Filter_Summon_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		if (!Filter_Summon.Highlight) {
			Disable_HighlightForFilter();
			Filter_Summon.Highlight = true;
			pageIndex = 0;
			RefleshSelectionUIBaseOnCondition(a => ContentSamples.ItemsByType[a].DamageType == DamageClass.Summon);
		}
		else {
			Filter_Summon.Highlight = false;
			RefleshSelectionUIBaseOnPageIndex();
		}
	}

	private void Filter_Magic_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		if (!Filter_Magic.Highlight) {
			Disable_HighlightForFilter();
			Filter_Magic.Highlight = true;
			pageIndex = 0;
			RefleshSelectionUIBaseOnCondition(a => ContentSamples.ItemsByType[a].DamageType == DamageClass.Magic);
		}
		else {
			Filter_Magic.Highlight = false;
			RefleshSelectionUIBaseOnPageIndex();
		}
	}

	private void Filter_Range_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		if (!Filter_Range.Highlight) {
			Disable_HighlightForFilter();
			Filter_Range.Highlight = true;
			pageIndex = 0;
			RefleshSelectionUIBaseOnCondition(a => ContentSamples.ItemsByType[a].DamageType == DamageClass.Ranged);
		}
		else {
			Filter_Range.Highlight = false;
			RefleshSelectionUIBaseOnPageIndex();
		}
	}

	private void Filter_Melee_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		if (!Filter_Melee.Highlight) {
			Disable_HighlightForFilter();
			Filter_Melee.Highlight = true;
			pageIndex = 0;
			RefleshSelectionUIBaseOnCondition(a => ContentSamples.ItemsByType[a].DamageType == DamageClass.Melee);
		}
		else {
			Filter_Melee.Highlight = false;
			RefleshSelectionUIBaseOnPageIndex();
		}
	}
	private void Reflesh() {
		if (Filter_Melee.Highlight) {
			RefleshSelectionUIBaseOnCondition(a => ContentSamples.ItemsByType[a].DamageType == DamageClass.Melee);
		}
		else if (Filter_Range.Highlight) {
			RefleshSelectionUIBaseOnCondition(a => ContentSamples.ItemsByType[a].DamageType == DamageClass.Ranged);
		}
		else if (Filter_Magic.Highlight) {
			RefleshSelectionUIBaseOnCondition(a => ContentSamples.ItemsByType[a].DamageType == DamageClass.Magic);
		}
		else if (Filter_Summon.Highlight) {
			RefleshSelectionUIBaseOnCondition(a => ContentSamples.ItemsByType[a].DamageType == DamageClass.Summon);
		}
		else {
			RefleshSelectionUIBaseOnPageIndex();
		}
	}
	public void RefleshSelectionUIBaseOnCondition(Func<int, bool> func) {
		List<int> list = EnchantmentLoader.EnchantmentcacheID.Where(func).ToList();
		maxPage = (int)Math.Ceiling(list.Count / (float)(Line * Row));
		int maxcount = list.Count;
		int startingPoint = Line * Row * pageIndex;
		for (int i = 0; i < Line; i++) {
			for (int j = 0; j < Row; j++) {
				int itemID = 0;
				int index = Line * i + j + startingPoint;
				if (index >= maxcount) {
					EnchantmentbuttonList[Line * i + j].InteralItemID = -1;
					continue;
				}
				if (index < EnchantmentLoader.TotalCount) {
					itemID = list[index];
				}
				EnchantmentButton btn = EnchantmentbuttonList[Line * i + j];
				btn.InteralItemID = itemID;
			}
		}
	}
	public void RefleshSelectionUIBaseOnPageIndex() {
		List<string> list = RoguelikeData.WeaponProgressTracker.Keys.ToList();
		maxPage = (int)Math.Ceiling(list.Count / (float)(Line * Row));
		if (pageIndex > maxPage || pageIndex < 0 || maxPage <= 1) {
			return;
		}
		int startingPoint = Line * Row * pageIndex;
		for (int i = 0; i < Line; i++) {
			for (int j = 0; j < Row; j++) {
				int itemID = 0;
				int index = Line * i + j + startingPoint;
				if (index < EnchantmentLoader.TotalCount) {
					itemID = EnchantmentLoader.EnchantmentcacheID[index];
				}
				EnchantmentButton btn = EnchantmentbuttonList[Line * i + j];
				btn.InteralItemID = itemID;
			}
		}
	}
	private void WE_Exit_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		WE_Panel.Hide = true;
	}

	Roguelike_UIPanel WE_Panel;
	Roguelike_UIPanel WE_Header;
	WeaponEnchantmentUIImg WE_Img;
	Roguelike_UIText WE_Name;
	Roguelike_WrapTextUIPanel WE_Desc;
	Roguelike_UIImage WE_Exit;
	int currentselectedItem = ItemID.None;
	private void Btn_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		WE_Panel.Hide = false;
		WE_Header.Hide = false;
		WE_Img.Hide = false;
		WE_Name.Hide = false;
		WE_Desc.Hide = false;
		WE_Exit.Hide = false;
		EnchantmentButton btn = EnchantmentbuttonList.Where(u => u.UniqueId == listeningElement.UniqueId).FirstOrDefault();
		if (btn == null) {
			return;
		}
		ModEnchantment enchantment = EnchantmentLoader.GetEnchantmentItemID(btn.InteralItemID);
		if (enchantment == null) {
			return;
		}
		currentselectedItem = btn.InteralItemID;
		WE_Img.weaponID = btn.InteralItemID;
		Item item = ContentSamples.ItemsByType[enchantment.ItemIDType];
		WE_Name.SetText(item.Name);
		WE_Desc.SetText(enchantment.Description);
	}

	private void Img_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		SetPageIndex(pagnitation.Select(el => el.UniqueId).ToList().IndexOf(listeningElement.UniqueId));
		Reflesh();
	}

	private void ButtonRight_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		if (pageIndex < maxPage - 1) {
			pageIndex++;
		}
		Reflesh();
	}

	private void ButtonLeft_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		if (pageIndex > 0) {
			pageIndex--;
		}
		Reflesh();
	}

	public override void Update(GameTime gameTime) {
		ModEnchantment enchantment = EnchantmentLoader.GetEnchantmentItemID(currentselectedItem);
		if (enchantment != null) {
			WE_Img.weaponID = enchantment.ItemIDType;
			Item item = ContentSamples.ItemsByType[enchantment.ItemIDType];
			WE_Name.SetText(item.Name);
			WE_Desc.SetText(enchantment.Description);
		}
		base.Update(gameTime);
		if (WE_Exit.IsMouseHovering) {
			Main.instance.MouseText("Exit");
		}
		for (int i = 0; i < pagnitation.Count; i++) {
			var item = pagnitation[i];
			item.toggled = i == pageIndex;
			if (item.IsMouseHovering) {
				Main.instance.MouseText("page " + (i + 1).ToString());
			}
		}
	}
}
public class WeaponEnchantmentUIImg : Roguelike_UIImage {
	public int weaponID = ItemID.None;
	public WeaponEnchantmentUIImg(Asset<Texture2D> texture) : base(texture) {
	}
	public override void DrawImage(SpriteBatch spriteBatch) {
		if (weaponID >= TextureAssets.Item.Length) {
			return;
		}
		Main.instance.LoadItem(weaponID);
		Texture2D item = TextureAssets.Item[weaponID].Value;
		Vector2 origin = item.Size() * .5f;
		Vector2 drawPos = GetInnerDimensions().Position() + new Vector2(26, 26);
		float scale;
		if (origin.X < 27 && origin.Y < 27) {
			scale = .8f;
		}
		else {
			scale = ScaleCalculation(new(52, 52), item.Size() * 2f);
		}
		spriteBatch.Draw(item, drawPos, null, Color.White, 0, origin, scale, SpriteEffects.None, 0);
	}
	private static float ScaleCalculation(Vector2 originalTexture, Vector2 textureSize) => originalTexture.Length() / textureSize.Length();
}
