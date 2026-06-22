using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Roguelike.Common.Systems.Achievement;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace Roguelike.Common.Systems.UI;
public class SynergyButton : Roguelike_UIImageButton {
	public string SynergyInternalName = "";
	public int InteralItemID = 0;
	public SynergyButton(Asset<Texture2D> texture) : base(texture) {
		SetVisibility(.67f, 1f);
		//InteralItemID = ModItemLib.SynergyItem.FirstOrDefault(s => s.ModItem.Name == SynergyInternalName).type;
	}
	public void SetSynergyItem(string synergyName) {
		SynergyInternalName = synergyName;
		if (!string.IsNullOrEmpty(synergyName)) {
			InteralItemID = ModItemLib.SynergyItem.Where(s => s.ModItem.Name == synergyName).FirstOrDefault().type;
		}
	}
	public override void DrawImage(SpriteBatch spriteBatch) {
		if (InteralItemID >= TextureAssets.Item.Length || string.IsNullOrEmpty(SynergyInternalName)) {
			return;
		}
		if (IsMouseHovering) {
			Main.instance.MouseText(SynergyInternalName);
		}
		if (InteralItemID == 0 && !string.IsNullOrEmpty(SynergyInternalName)) {
			InteralItemID = ModItemLib.SynergyItem.Where(s => s.ModItem.Name == SynergyInternalName).FirstOrDefault().type;
		}
		Main.instance.LoadItem(InteralItemID);
		Texture2D item = TextureAssets.Item[InteralItemID].Value;
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
public class SynergyMenuWikiUI : UIState {
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
	int Row = 5;
	int Line = 5;
	List<SynergyButton> synegybuttonList = new();
	public string CurrentlySelectedSynergyWeapon = "";
	public void SetPageIndex(int index) {
		pageIndex = Math.Clamp(index, 0, maxPage);
	}
	public override void OnInitialize() {
		synegybuttonList = new();

		holderPanel = new();
		holderPanel.UISetWidthHeight(500, 500);
		holderPanel.HAlign = .5f;
		holderPanel.VAlign = .5f;
		Append(holderPanel);

		mainPanel = new();
		mainPanel.Width.Percent = 1;
		mainPanel.Height.Percent = .7f;
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

		maxPage = (int)Math.Ceiling(ModItemLib.SynergyItem.Count / (float)(Line * Row));

		for (int i = 0; i < Line; i++) {
			for (int j = 0; j < Row; j++) {
				SynergyButton btn = new(TextureAssets.InventoryBack);
				int index = Line * i + j;
				if (index < ModItemLib.SynergyItem.Count) {
					btn.SynergyInternalName = ModItemLib.SynergyItem[index].ModItem.Name;
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

	private void Img_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		SetPageIndex(pagnitation.Select(el => el.UniqueId).ToList().IndexOf(listeningElement.UniqueId));
		RefleshSelectionUIBaseOnPageIndex();
	}

	private void ButtonRight_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		if (pageIndex < maxPage - 1) {
			pageIndex++;
		}
		RefleshSelectionUIBaseOnPageIndex();
	}

	private void ButtonLeft_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		if (pageIndex > 0) {
			pageIndex--;
		}
		RefleshSelectionUIBaseOnPageIndex();
	}

	public void RefleshSelectionUIBaseOnPageIndex() {
		if (pageIndex > maxPage || pageIndex < 0 || maxPage <= 1) {
			return;
		}
		int maxcount = ModItemLib.SynergyItem.Count;
		int startingPoint = Line * Row * pageIndex;
		for (int i = 0; i < Line; i++) {
			for (int j = 0; j < Row; j++) {
				string name = "";
				int index = Line * i + j + startingPoint;
				if (index < ModItemLib.SynergyItem.Count) {
					name = ModItemLib.SynergyItem[index].ModItem.Name;
				}
				SynergyButton btn = synegybuttonList[Line * i + j];
				if (index >= maxcount) {
					btn.SetSynergyItem(string.Empty);
					continue;
				}
				btn.SetSynergyItem(name);
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
