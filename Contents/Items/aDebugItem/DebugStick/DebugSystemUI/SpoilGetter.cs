using System;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Common.Systems;
using Roguelike.Common.Systems.SpoilSystem;

using Roguelike.Texture;
using Roguelike.Common.Utils;

namespace Roguelike.Contents.Items.aDebugItem.DebugStick.DebugSystemUI;
class SpoilGetterUI : UIState {
	UIPanel panel;
	int currentSelectTemplate = -1;
	private const int MAX_LINES = 6;

	List<SpoilsUIButton> btn_list;
	private List<ModSpoil> list_Spoil = new();
	public const int SPOIL_MAXLINE = 10;
	public override void OnInitialize() {
		panel = new UIPanel();
		panel.HAlign = .5f;
		panel.VAlign = .5f;
		panel.UISetWidthHeight(500, 500);
		Append(panel);

		btn_list = new();
	}
	public override void OnActivate() {
		btn_list.Clear();
		list_Spoil.Clear();
		panel.RemoveAllChildren();
		list_Spoil.AddRange(ModSpoilSystem.GetSpoilsList());
		int length = list_Spoil.Count;
		int lineCounter = 0;

		for (int i = 0; i < length; i++) {
			if (i % SPOIL_MAXLINE == 0) {
				lineCounter++;
			}
			if (lineCounter < SPOIL_MAXLINE) {

				SpoilsUIButton button = new(ModContent.Request<Texture2D>(ModTexture.ACCESSORIESSLOT), list_Spoil[i]) {
					Width = StyleDimension.FromPixels(44f),
					Height = StyleDimension.FromPixels(44f),
					Left = StyleDimension.FromPixels(i % SPOIL_MAXLINE * 46.0f + 6.0f),
					Top = StyleDimension.FromPixels(i / SPOIL_MAXLINE * 48.0f + 1.0f)
				};
				button.OnLeftClick += Text_OnLeftClick;
				btn_list.Add(button);
				panel.Append(button);
			}
		}
	}
	public override void ScrollWheel(UIScrollWheelEvent evt) {
		//linePosition -= MathF.Sign(evt.ScrollWheelValue);
		//int offsetvalue = linePosition * SPOIL_MAXLINE;
		//int length = list_Spoil.Count;
		//int offsetlength = length - offsetvalue;
		//for (int i = 0; i < length; i++) {
		//	int arty = Math.Clamp(i + offsetvalue, 0, length - 1);
		//	btn_list[i].spoil = null;
		//	if (i > offsetlength) {
		//		continue;
		//	}
		//	btn_list[i].spoil = list_Spoil[arty];
		//}
	}
	private void Text_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		currentSelectTemplate = listeningElement.UniqueId;
	}
}
