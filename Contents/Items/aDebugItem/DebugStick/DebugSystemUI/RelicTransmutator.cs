using Microsoft.CodeAnalysis.Differencing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Roguelike.Common.Systems;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.RelicItem;
using Roguelike.Contents.Items.RelicItem.RelicSetContent;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;
using Terraria.WorldBuilding;

namespace Roguelike.Contents.Items.aDebugItem.DebugStick.DebugSystemUI;
class RelicTransmuteUI : UIState {
	UIImageButton btn_select;
	Roguelike_UIPanel panel_Main;
	List<UIText> textlist;
	int current_Template = -1;
	int current_Prefix = -1;
	int current_Set = -1;
	PlayerStats current_PlayerStats;

	Roguelike_UIPanel container_AutoContainer;
	Roguelike_UIText text_container;
	Roguelike_UIPanel container_ExtraContainer;
	Roguelike_UITextPanel txp_AutoTemplate;
	bool automatic_template = true;

	Roguelike_UITextPanel txp_AutoValue;
	bool automatic_value = true;

	Roguelike_UITextPanel txp_AutoPrefix;
	bool automatic_prefix = true;

	Roguelike_UITextPanel txp_AutoSet;
	bool automatic_set = true;

	Roguelike_UITextPanel txp_AutoPlayerStats;
	bool automatic_PlayerStats = true;

	Roguelike_UIPanel panel_sidePanel;
	ExitUI exit;

	/// <summary>
	/// After tester is done with selecting, clicking this will add in a completed relic into UI storage<br/>
	/// </summary>
	Roguelike_UIImageButton btn_AddRelic;
	/// <summary>
	/// After tester is done with adjusting their relic, clicking confirmation will drop that relic;
	/// </summary>
	Roguelike_UIImageButton btn_confirmation;
	/// <summary>
	/// After tester is done with adjusting their relic, clicking confirmation will drop that relic;
	/// </summary>
	Roguelike_UIImageButton btn_ClearSetting;

	Roguelike_UIPanel container_ManualSetting;
	Roguelike_UIText text_container2;
	Roguelike_UIPanel container_ExtraContainer2;

	Roguelike_UITextPanel txp_Template;
	Roguelike_UITextPanel txp_Set;
	Roguelike_UITextPanel txp_Prefix;
	Roguelike_UITextPanel txp_PlayerStats;
	bool finish_Init = false;

	Roguelike_UIPanel container_ManualSetting2;
	Roguelike_UIText text_container22;
	Roguelike_UIPanel container_ExtraContainer22;
	Roguelike_TextBox txb_valueSetterAdd;
	Roguelike_TextBox txb_valueSetterMult;
	Roguelike_TextBox txb_valueSetterBase;
	Roguelike_TextBox txb_valueSetterFlat;
	Relic relic = null;

	Roguelike_UIPanel panel_bodypanel;
	ItemHolderSlot slot;
	public override void OnDeactivate() {
		finish_Init = false;
	}
	public override void OnInitialize() {
		finish_Init = false;
		panel_Main = new();
		panel_Main.HAlign = .5f;
		panel_Main.VAlign = .5f;
		panel_Main.UISetWidthHeight(655, 305);
		Append(panel_Main);

		automatic_PlayerStats = automatic_prefix = automatic_set = automatic_template = automatic_value = true;

		panel_sidePanel = new();
		panel_sidePanel.HAlign = 1f;
		panel_sidePanel.Height.Percent = 1f;
		panel_sidePanel.Width.Pixels = 72;
		panel_Main.Append(panel_sidePanel);

		exit = new(TextureAssets.InventoryBack7);
		exit.UISetWidthHeight(52, 52);
		panel_sidePanel.Append(exit);

		Init_AutoContainer();

		Init_ManualSetting();

		Init_ManualSetting2();

		Init_Button();

		panel_bodypanel = new();
		panel_bodypanel.Height.Pixels = 75;
		panel_bodypanel.Width.Percent = .875f;
		panel_bodypanel.VAlign = 1f;
		panel_Main.Append(panel_bodypanel);

		slot = new(TextureAssets.InventoryBack11);
		slot.UISetWidthHeight(52, 52);
		panel_bodypanel.Append(slot);

		finish_Init = true;
	}
	public override void OnActivate() {
		relic = new Item(ModContent.ItemType<Relic>()).ModItem as Relic;
		slot.item = relic.Item;
	}
	private void Init_AutoContainer() {
		container_AutoContainer = new();
		container_AutoContainer.UISetWidthHeight(130, 200);
		container_AutoContainer.SetPadding(10);
		panel_Main.Append(container_AutoContainer);

		text_container = new("Automatic setting", .8f);
		text_container.HAlign = .5f;
		text_container.DynamicallyScaleDownToWidth = true;
		container_AutoContainer.Append(text_container);

		container_ExtraContainer = new();
		container_ExtraContainer.Width.Percent = 1f;
		container_ExtraContainer.Height.Percent = .9f;
		container_ExtraContainer.VAlign = 1;
		container_ExtraContainer.BorderColor = Color.Black with { A = 0 };
		container_ExtraContainer.BackgroundColor = Color.Black with { A = 0 };
		container_ExtraContainer.PaddingBottom = 2;
		container_AutoContainer.Append(container_ExtraContainer);

		txp_AutoTemplate = new("Template", .77f);
		txp_AutoTemplate.Width.Percent = 1f;
		txp_AutoTemplate.TextHAlign = 0;
		txp_AutoTemplate.OnLeftClick += Txp_AutoTemplate_OnLeftClick;
		txp_AutoTemplate.BorderColor = Color.Black with { A = 0 };
		txp_AutoTemplate.BackgroundColor = Color.Black with { A = 0 };
		txp_AutoTemplate.SetPadding(0);
		txp_AutoTemplate.TextColor = Color.Yellow;
		container_ExtraContainer.Append(txp_AutoTemplate);

		txp_AutoValue = new("Value", .77f);
		txp_AutoValue.VAlign = .25f;
		txp_AutoValue.TextHAlign = 0;
		txp_AutoValue.Width.Percent = 1f;
		txp_AutoValue.OnLeftClick += Txp_AutoValue_OnLeftClick;
		txp_AutoValue.BorderColor = Color.Black with { A = 0 };
		txp_AutoValue.BackgroundColor = Color.Black with { A = 0 };
		txp_AutoValue.SetPadding(0);
		txp_AutoValue.TextColor = Color.Yellow;
		container_ExtraContainer.Append(txp_AutoValue);

		txp_AutoPrefix = new("Prefix", .77f);
		txp_AutoPrefix.VAlign = .5f;
		txp_AutoPrefix.Width.Percent = 1f;
		txp_AutoPrefix.TextHAlign = 0;
		txp_AutoPrefix.OnLeftClick += Txp_AutoPrefix_OnLeftClick;
		txp_AutoPrefix.BorderColor = Color.Black with { A = 0 };
		txp_AutoPrefix.BackgroundColor = Color.Black with { A = 0 };
		txp_AutoPrefix.SetPadding(0);
		txp_AutoPrefix.TextColor = Color.Yellow;
		container_ExtraContainer.Append(txp_AutoPrefix);

		txp_AutoSet = new("Set", .77f);
		txp_AutoSet.VAlign = .75f;
		txp_AutoSet.Width.Percent = 1f;
		txp_AutoSet.TextHAlign = 0;
		txp_AutoSet.OnLeftClick += Txp_AutoSet_OnLeftClick;
		txp_AutoSet.BorderColor = Color.Black with { A = 0 };
		txp_AutoSet.BackgroundColor = Color.Black with { A = 0 };
		txp_AutoSet.SetPadding(0);
		txp_AutoSet.TextColor = Color.Yellow;
		container_ExtraContainer.Append(txp_AutoSet);

		txp_AutoPlayerStats = new("PlayerStats", .77f);
		txp_AutoPlayerStats.VAlign = 1f;
		txp_AutoPlayerStats.Width.Percent = 1f;
		txp_AutoPlayerStats.TextHAlign = 0;
		txp_AutoPlayerStats.OnLeftClick += Txp_AutoPlayerStats_OnLeftClick;
		txp_AutoPlayerStats.BorderColor = Color.Black with { A = 0 };
		txp_AutoPlayerStats.BackgroundColor = Color.Black with { A = 0 };
		txp_AutoPlayerStats.SetPadding(0);
		txp_AutoPlayerStats.TextColor = Color.Yellow;
		container_ExtraContainer.Append(txp_AutoPlayerStats);
	}
	private void Init_ManualSetting() {
		container_ManualSetting = new();
		container_ManualSetting.UISetWidthHeight(200, 200);
		container_ManualSetting.MarginLeft = container_AutoContainer.Width.Pixels + 10;
		panel_Main.Append(container_ManualSetting);

		text_container2 = new("Manual setting", .8f);
		text_container2.HAlign = .5f;
		text_container2.DynamicallyScaleDownToWidth = true;
		container_ManualSetting.Append(text_container2);

		container_ExtraContainer2 = new();
		container_ExtraContainer2.Width.Percent = 1f;
		container_ExtraContainer2.Height.Percent = .9f;
		container_ExtraContainer2.VAlign = 1f;
		container_ExtraContainer2.BorderColor = Color.Black with { A = 0 };
		container_ExtraContainer2.PaddingBottom = 0;
		container_ExtraContainer2.PaddingTop = 0;
		container_ManualSetting.Append(container_ExtraContainer2);

		txp_Template = new("", .77f);
		txp_Template.HAlign = .5f;
		txp_Template.VAlign = 0;
		txp_Template.Width.Percent = 1f;
		container_ExtraContainer2.Append(txp_Template);

		txp_Set = new("", .77f);
		txp_Set.HAlign = .5f;
		txp_Set.VAlign = 1 / 3f;
		txp_Set.Width.Percent = 1f;
		container_ExtraContainer2.Append(txp_Set);

		txp_Prefix = new("", .77f);
		txp_Prefix.HAlign = .5f;
		txp_Prefix.VAlign = 2 / 3f;
		txp_Prefix.Width.Percent = 1f;
		container_ExtraContainer2.Append(txp_Prefix);

		txp_PlayerStats = new("", .77f);
		txp_PlayerStats.HAlign = .5f;
		txp_PlayerStats.VAlign = 1;
		txp_PlayerStats.Width.Percent = 1f;
		container_ExtraContainer2.Append(txp_PlayerStats);
	}
	private void Init_ManualSetting2() {
		container_ManualSetting2 = new();
		container_ManualSetting2.UISetWidthHeight(200, 200);
		container_ManualSetting2.MarginLeft = container_AutoContainer.Width.Pixels + container_ManualSetting2.Width.Pixels + 20;
		panel_Main.Append(container_ManualSetting2);

		text_container22 = new("Setting value", .8f);
		text_container22.HAlign = .5f;
		text_container22.DynamicallyScaleDownToWidth = true;
		container_ManualSetting2.Append(text_container22);

		container_ExtraContainer22 = new();
		container_ExtraContainer22.Width.Percent = 1f;
		container_ExtraContainer22.Height.Percent = .9f;
		container_ExtraContainer22.VAlign = 1f;
		container_ExtraContainer22.PaddingTop = 0;
		container_ExtraContainer22.PaddingBottom = 0;
		container_ExtraContainer22.BorderColor = Color.Black with { A = 0 };
		container_ManualSetting2.Append(container_ExtraContainer22);

		txb_valueSetterAdd = new("1.0", .77f);
		txb_valueSetterAdd.Width.Precent = 1f;
		txb_valueSetterAdd.TextHAlign = 0;
		txb_valueSetterAdd.OnHoverText = "Additive";
		container_ExtraContainer22.Append(txb_valueSetterAdd);

		txb_valueSetterMult = new("1.0", .77f);
		txb_valueSetterMult.Width.Precent = 1f;
		txb_valueSetterMult.TextHAlign = 0;
		txb_valueSetterMult.VAlign = 1 / 3f;
		txb_valueSetterMult.OnHoverText = "Multiplicative";
		container_ExtraContainer22.Append(txb_valueSetterMult);

		txb_valueSetterBase = new("0", .77f);
		txb_valueSetterBase.Width.Precent = 1f;
		txb_valueSetterBase.TextHAlign = 0;
		txb_valueSetterBase.VAlign = 2 / 3f;
		txb_valueSetterBase.OnHoverText = "Base";
		container_ExtraContainer22.Append(txb_valueSetterBase);

		txb_valueSetterFlat = new("0", .77f);
		txb_valueSetterFlat.Width.Precent = 1f;
		txb_valueSetterFlat.TextHAlign = 0;
		txb_valueSetterFlat.VAlign = 1f;
		txb_valueSetterFlat.OnHoverText = "Flat";
		container_ExtraContainer22.Append(txb_valueSetterFlat);

	}
	private void Init_Button() {
		btn_AddRelic = new(TextureAssets.InventoryBack7);
		btn_AddRelic.UISetWidthHeight(52, 52);
		btn_AddRelic.MarginTop = 62;
		btn_AddRelic.OnLeftClick += Btn_AddRelic_OnLeftClick;
		btn_AddRelic.SetVisibility(.5f, 1f);
		btn_AddRelic.HoverText = "Add relic stats";
		panel_sidePanel.Append(btn_AddRelic);

		btn_confirmation = new(TextureAssets.InventoryBack7);
		btn_confirmation.UISetWidthHeight(52, 52);
		btn_confirmation.MarginTop = 124;
		btn_confirmation.OnLeftClick += Btn_confirmation_OnLeftClick;
		btn_confirmation.SetVisibility(.5f, 1f);
		btn_confirmation.HoverText = "Created relic";
		panel_sidePanel.Append(btn_confirmation);

		btn_ClearSetting = new(TextureAssets.InventoryBack7);
		btn_ClearSetting.UISetWidthHeight(52, 52);
		btn_ClearSetting.MarginTop = 186;
		btn_ClearSetting.OnLeftClick += Btn_ClearSetting_OnLeftClick; ;
		btn_ClearSetting.SetVisibility(.5f, 1f);
		btn_ClearSetting.HoverText = "Clear relic";
		panel_sidePanel.Append(btn_ClearSetting);

	}

	private void Btn_ClearSetting_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		relic.SetRelicData(new(), new(), new());
	}

	private void Btn_confirmation_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		Item.NewItem(Item.GetSource_None(), Main.LocalPlayer.Center, relic.Item);
	}

	private void Btn_AddRelic_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		if (automatic_prefix) {
			relic.RelicPrefixedType = (short)Main.rand.Next(RelicPrefixSystem.TotalCount);
		}
		else {
			relic.RelicPrefixedType = (short)current_Prefix;
		}
		if (automatic_set) {
			relic.RelicSetType = (short)Main.rand.Next(RelicSetSystem.TotalCount);
		}
		else {
			relic.RelicSetType = (short)current_Set;
		}
		if (automatic_template && automatic_PlayerStats && automatic_value) {
			relic.AddRelicTemplate(Main.LocalPlayer, Main.rand.Next(RelicTemplateLoader.TotalCount));
		}
		else if (automatic_template && automatic_value) {
			relic.AddRelicTemplate(Main.LocalPlayer, Main.rand.Next(RelicTemplateLoader.TotalCount), current_PlayerStats);
		}
		else if (automatic_template && automatic_PlayerStats) {
			GetValueFromTextBox(out StatModifier modifier);
			relic.AddRelicTemplate(Main.rand.Next(RelicTemplateLoader.TotalCount), modifier);
		}
		else if (automatic_value && automatic_PlayerStats) {
			relic.AddRelicTemplate(Main.LocalPlayer, current_Template);
		}
		else if (automatic_value) {
			relic.AddRelicTemplate(Main.LocalPlayer, current_Template, current_PlayerStats);
		}
		else if (automatic_PlayerStats) {
			GetValueFromTextBox(out StatModifier modifier);
			relic.AddRelicTemplate(current_Template, modifier);
		}
		else if (automatic_template) {
			GetValueFromTextBox(out StatModifier modifier);
			relic.AddRelicTemplate(Main.rand.Next(RelicTemplateLoader.TotalCount), modifier, current_PlayerStats);
		}
		else {
			GetValueFromTextBox(out StatModifier modifier);
			relic.AddRelicTemplate(current_Template, modifier, current_PlayerStats);
		}
	}
	private void GetValueFromTextBox(out StatModifier modifier) {
		float add, multi, baseA, flat;
		if (!float.TryParse(txb_valueSetterAdd.Text, out add)) {
			add = 1f;
		}
		if (!float.TryParse(txb_valueSetterMult.Text, out multi)) {
			multi = 1f;
		}
		if (!float.TryParse(txb_valueSetterBase.Text, out baseA)) {
			baseA = 0;
		}
		if (!float.TryParse(txb_valueSetterFlat.Text, out flat)) {
			flat = 0;
		}
		modifier = new StatModifier(add, multi, flat, baseA);
	}

	private void Txp_AutoTemplate_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		automatic_template = !automatic_template;
		if (!automatic_template) {
			txp_AutoTemplate.TextColor = Color.Gray;
		}
		else {
			txp_AutoTemplate.TextColor = Color.Yellow;
		}
	}

	private void Txp_AutoSet_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		automatic_set = !automatic_set;
		if (!automatic_set) {
			txp_AutoSet.TextColor = Color.Gray;
		}
		else {
			txp_AutoSet.TextColor = Color.Yellow;
		}
	}

	private void Txp_AutoPrefix_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		automatic_prefix = !automatic_prefix;
		if (!automatic_prefix) {
			txp_AutoPrefix.TextColor = Color.Gray;
		}
		else {
			txp_AutoPrefix.TextColor = Color.Yellow;
		}
	}

	private void Txp_AutoValue_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		automatic_value = !automatic_value;
		if (!automatic_value) {
			txp_AutoValue.TextColor = Color.Gray;
		}
		else {
			txp_AutoValue.TextColor = Color.Yellow;
		}
	}
	private void Txp_AutoPlayerStats_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		automatic_PlayerStats = !automatic_PlayerStats;
		if (!automatic_PlayerStats) {
			txp_AutoPlayerStats.TextColor = Color.Gray;
		}
		else {
			txp_AutoPlayerStats.TextColor = Color.Yellow;
		}
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
	}
	public override void Draw(SpriteBatch spriteBatch) {
		base.Draw(spriteBatch);
			Draw_Template();
			Draw_Prefix();
			Draw_Set();
			Draw_PlayerStats();
		
	}
	private void Draw_Template() {
		RelicTemplate val = RelicTemplateLoader.GetTemplate(scW_Template);
		if (val == null) {
			txp_Template.SetText("Error");
		}
		else {
			if (txp_Template.IsMouseHovering) {
				UICommon.TooltipMouseText(val.Description);
			}
			current_Template = val.Type;
		}
	}
	private void Draw_Prefix() {
		RelicPrefix val = RelicPrefixSystem.GetRelicPrefix(scW_Prefix);
		if (val == null) {
			txp_Prefix.SetText("Error");
		}
		else {
			txp_Prefix.SetText(val.DisplayName);
			if (txp_Prefix.IsMouseHovering) {
				UICommon.TooltipMouseText(val.Description);
			}
			current_Prefix = val.Type;
		}
	}
	private void Draw_Set() {
		RelicSet val = RelicSetSystem.GetSet(scW_Set);
		if (val == null) {
			txp_Set.SetText("Error");
		}
		else {
			txp_Set.SetText(val.DisplayName);
			if (txp_Set.IsMouseHovering) {
				UICommon.TooltipMouseText(val.Description);
			}
			current_Set = val.Type;
		}
	}
	private void Draw_PlayerStats() {
		PlayerStats stats = (PlayerStats)(byte)scW_PlayerStats;
		if (stats.ToString() == null) {
			txp_PlayerStats.SetText("Error");
		}
		else {
			txp_PlayerStats.SetText(stats.ToString());
			current_PlayerStats = stats;
		}
	}
	/// <summary>
	/// These scroll wheel value also serves as their ModType type selector
	/// </summary>
	int scW_Template = 0, scW_Prefix = 0, scW_Set = 0, scW_PlayerStats = 0;
	public override void ScrollWheel(UIScrollWheelEvent evt) {
		if (txp_Template.IsMouseHovering) {
			scW_Template -= MathF.Sign(evt.ScrollWheelValue);
			if (scW_Template >= RelicTemplateLoader.TotalCount) {
				scW_Template = 0;
			}
			if (scW_Template <= -1) {
				scW_Template = RelicTemplateLoader.TotalCount - 1;
			}
		}
		if (txp_Prefix.IsMouseHovering) {
			scW_Prefix -= MathF.Sign(evt.ScrollWheelValue);
			if (scW_Prefix >= RelicPrefixSystem.TotalCount) {
				scW_Prefix = 0;
			}
			if (scW_Prefix <= -1) {
				scW_Prefix = RelicPrefixSystem.TotalCount - 1;
			}
		}
		if (txp_Set.IsMouseHovering) {
			scW_Set -= MathF.Sign(evt.ScrollWheelValue);
			if (scW_Set >= RelicSetSystem.TotalCount) {
				scW_Set = 0;
			}
			if (scW_Set <= -1) {
				scW_Set = RelicSetSystem.TotalCount - 1;
			}
		}
		if (txp_PlayerStats.IsMouseHovering) {
			scW_PlayerStats -= MathF.Sign(evt.ScrollWheelValue);
			var max = Enum.GetValues(typeof(PlayerStats)).Cast<PlayerStats>().Max();
			if (scW_PlayerStats >= (int)max) {
				scW_PlayerStats = 0;
			}
			if (scW_PlayerStats <= -1) {
				scW_PlayerStats = (int)max;
			}
		}
	}
}
