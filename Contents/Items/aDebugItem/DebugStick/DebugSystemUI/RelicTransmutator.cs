using Microsoft.Xna.Framework;
using Roguelike.Common.Systems;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.RelicItem;
using Roguelike.Texture;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace Roguelike.Contents.Items.aDebugItem.DebugStick.DebugSystemUI;
class RelicTransmuteUI : UIState {
	UIImageButton btn_select;
	UIPanel panel;
	List<UIText> textlist;
	int currentSelectTemplate = -1;
	public override void OnInitialize() {
		btn_select = new UIImageButton(TextureAssets.InventoryBack);
		btn_select.HAlign = .65f;
		btn_select.VAlign = .5f;
		btn_select.OnLeftClick += Btn_select_OnLeftClick;
		btn_select.OnUpdate += Btn_select_OnUpdate;
		Append(btn_select);

		panel = new UIPanel();
		panel.HAlign = .5f;
		panel.VAlign = .5f;
		panel.UISetWidthHeight(400, 600);
		Append(panel);

		textlist = new List<UIText>();
	}
	private void Btn_select_OnUpdate(UIElement affectedElement) {
		if (affectedElement.ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
	}
	private void Btn_select_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		var text = textlist.Where(i => i.UniqueId == currentSelectTemplate).FirstOrDefault();
		if (text == null) {
			return;
		}
		var template = RelicTemplateLoader.GetTemplate(textlist.IndexOf(text));
		if (template == null) {
			return;
		}
		var item = Main.LocalPlayer.QuickSpawnItemDirect(Main.LocalPlayer.GetSource_FromThis(), ModContent.ItemType<Relic>());
		if (item.ModItem is Relic relic) {
			relic.AddRelicTemplate(Main.LocalPlayer, template.Type);
		}
		ModContent.GetInstance<UniversalSystem>().DeactivateUI();
	}
	public override void OnActivate() {
		textlist.Clear();
		panel.RemoveAllChildren();
		for (int i = 0; i < RelicTemplateLoader.TotalCount; i++) {
			var text = new UIText(RelicTemplateLoader.GetTemplate(i).Name);
			text.OnLeftClick += Text_OnLeftClick;
			text.OnUpdate += Text_OnUpdate;
			text.VAlign = i / (RelicTemplateLoader.TotalCount - 1f);
			panel.Append(text);
			textlist.Add(text);
		}
	}
	private void Text_OnUpdate(UIElement affectedElement) {
		if (currentSelectTemplate == affectedElement.UniqueId) {
			var text = textlist.Where(i => i.UniqueId == currentSelectTemplate).FirstOrDefault();
			if (text == null) {
				return;
			}
			var template = RelicTemplateLoader.GetTemplate(textlist.IndexOf(text));
			if (template == null) {
				return;
			}
			text.SetText($"[c/{Color.Yellow.Hex3()}:{template.Name}]");
		}
		else {
			var text = textlist.Where(i => i.UniqueId == affectedElement.UniqueId).FirstOrDefault();
			if (text == null) {
				return;
			}
			var template = RelicTemplateLoader.GetTemplate(textlist.IndexOf(text));
			if (template == null) {
				return;
			}
			text.SetText(template.Name);
		}
		if (affectedElement.ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
	}
	private void Text_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		currentSelectTemplate = listeningElement.UniqueId;
	}
}
