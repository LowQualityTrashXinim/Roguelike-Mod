using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Roguelike.Common.Systems.Achievement;
using Roguelike.Common.Systems.IOhandle;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace Roguelike.Common.Systems.UI;
public class SynergyButton : Roguelike_UIImageButton {
	public bool ItemLocked = true;
	public int InteralItemID = 0;
	Asset<Texture2D> Lock;
	Texture2D _texture;
	public SynergyButton(Asset<Texture2D> texture) : base(texture) {
		SetVisibility(.67f, 1f);
		Lock = ModContent.Request<Texture2D>(ModTexture.Lock);
		_texture = texture.Value;
	}
	public override void UpdateOuter(GameTime gametime) {
		if (RoguelikeData.WeaponProgressTracker.ContainsKey(InteralItemID)) {
			ItemLocked = !RoguelikeData.WeaponProgressTracker[InteralItemID];
		}
	}
	public override void DrawImage(SpriteBatch spriteBatch) {
		if (InteralItemID >= TextureAssets.Item.Length || InteralItemID < 0) {
			return;
		}
		if (ItemLocked) {
			Texture2D locktex = Lock.Value;
			Vector2 origin2 = locktex.Size() * .5f;
			Vector2 drawpos2 = GetDimensions().Position() + _texture.Size() * .5f;
			spriteBatch.Draw(locktex, drawpos2, null, new Color(255, 255, 255), 0, origin2, .9f, SpriteEffects.None, 0);
			return;
		}
		if (IsMouseHovering) {
			Item item = ContentSamples.ItemsByType[InteralItemID];
			Main.HoverItem = item.Clone();
			Main.hoverItemName = item.HoverName;
		}
		//if (InteralItemID == 0 && !string.IsNullOrEmpty(SynergyInternalName)) {
		//	InteralItemID = ModItemLib.SynergyItem.Where(s => s.ModItem.Name == SynergyInternalName).FirstOrDefault().type;
		//}
		Main.instance.LoadItem(InteralItemID);
		Texture2D itemSprite = TextureAssets.Item[InteralItemID].Value;
		Vector2 origin = itemSprite.Size() * .5f;
		Vector2 drawPos = GetInnerDimensions().Position() + new Vector2(26, 26);
		float scale;
		if (origin.X < 27 && origin.Y < 27) {
			scale = .8f;
		}
		else {
			scale = ScaleCalculation(new(52, 52), itemSprite.Size() * 2f);
		}
		spriteBatch.Draw(itemSprite, drawPos, null, Color.White, 0, origin, scale, SpriteEffects.None, 0);
	}
	private static float ScaleCalculation(Vector2 originalTexture, Vector2 textureSize) => originalTexture.Length() / textureSize.Length();
}
public class SynergyMenuWikiUI : UIState {
	public UIPanel holderPanel;
	public UIPanel mainPanel;
	public UIPanel headerPanel;
	public UIPanel footerPanel;
	public ExitUI exit;
	Roguelike_UIImageButton buttonLeft;
	Roguelike_UIImageButton buttonRight;

	Roguelike_UIImage Filter_Melee;
	Roguelike_UIImage Filter_Range;
	Roguelike_UIImage Filter_Magic;
	Roguelike_UIImage Filter_Summon;

	List<PageImage> pagnitation = new();
	int pageIndex = 0;
	int maxPage = 1;
	int Row = 5;
	int Line = 5;
	List<SynergyButton> synegybuttonList = new();
	public string CurrentlySelectedSynergyWeapon = "";
	public void SetPageIndex(int index) {
		pageIndex = Math.Clamp(index, 0, maxPage);
	}
	public override void OnInitialize() {
		pagnitation.Clear();
		Row = 10;
		Line = 10;
		synegybuttonList = new();

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

		footerPanel = new();
		footerPanel.VAlign = 1;
		footerPanel.Width.Percent = 1;
		footerPanel.Height.Pixels = 60;
		footerPanel.PaddingTop = 5;
		footerPanel.PaddingBottom = 5;
		holderPanel.Append(footerPanel);

		exit = new(TextureAssets.InventoryBack);
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

		List<Item> list = [.. ModItemLib.List_Weapon, .. ModItemLib.SynergyItem];


		maxPage = (int)Math.Ceiling(list.Count / (float)(Line * Row));

		for (int i = 0; i < Line; i++) {
			for (int j = 0; j < Row; j++) {
				SynergyButton btn = new(TextureAssets.InventoryBack);
				int index = Line * i + j;
				if (index < list.Count) {
					btn.InteralItemID = list[index].type;
				}
				btn.HAlign = j / (Row - 1f);
				btn.VAlign = i / (Line - 1f);
				synegybuttonList.Add(btn);
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
		List<int> list = RoguelikeData.WeaponProgressTracker.Keys.Where(func).ToList();


		maxPage = (int)Math.Ceiling(list.Count / (float)(Line * Row));
		int maxcount = list.Count;
		int startingPoint = Line * Row * pageIndex;
		for (int i = 0; i < Line; i++) {
			for (int j = 0; j < Row; j++) {
				int index = Line * i + j + startingPoint;
				SynergyButton btn = synegybuttonList[Line * i + j];
				if (index >= maxcount) {
					btn.InteralItemID = -1;
					continue;
				}
				btn.InteralItemID = list[index];
			}
		}
	}
	public void RefleshSelectionUIBaseOnPageIndex() {
		List<int> list = RoguelikeData.WeaponProgressTracker.Keys.ToList();
		maxPage = (int)Math.Ceiling(list.Count / (float)(Line * Row));
		if (pageIndex > maxPage || pageIndex < 0 || maxPage <= 1) {
			return;
		}
		int maxcount = list.Count;
		int startingPoint = Line * Row * pageIndex;
		for (int i = 0; i < Line; i++) {
			for (int j = 0; j < Row; j++) {
				int index = Line * i + j + startingPoint;
				SynergyButton btn = synegybuttonList[Line * i + j];
				if (index >= maxcount) {
					btn.InteralItemID = -1;
					continue;
				}
				btn.InteralItemID = list[index];
			}
		}
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		for (int i = 0; i < pagnitation.Count; i++) {
			var item = pagnitation[i];
			item.toggled = i == pageIndex;
			if (item.IsMouseHovering) {
				Main.instance.MouseText("page " + (i + 1).ToString());
			}
		}
	}
}
