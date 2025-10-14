using Microsoft.Xna.Framework;
using Roguelike.Common.Systems;
using Roguelike.Common.Utils;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace Roguelike.Contents.Items.aDebugItem.DebugStick;
internal class MainDebugUI : UIState {
	UIPanel panel;
	UIPanel Roguelike_Panel;
	UIText RoguelikeText;
	UIText open_SpoilUI;
	UIText open_SkillUI;
	UIText open_RelicUI;
	UIText exit_Menu;
	public override void OnInitialize() {
		Roguelike_Panel = new();
		Roguelike_Panel.HAlign = .5f;
		Roguelike_Panel.VAlign = .5f;
		Roguelike_Panel.UISetWidthHeight(400, 400);
		Append(Roguelike_Panel);

		RoguelikeText = new("Roguelike debug menu");
		RoguelikeText.HAlign = .5f;
		Roguelike_Panel.Append(RoguelikeText);

		panel = new UIPanel();
		panel.HAlign = .5f;
		panel.VAlign = 1;
		panel.Height.Percent = .9f;
		panel.Width.Percent = 1f;
		panel.MarginTop = RoguelikeText.Height.Pixels + 20;
		Roguelike_Panel.Append(panel);

		open_SpoilUI = new("Select spoil", 1.5f);
		open_SpoilUI.OnLeftClick += Open_SpoilUI_OnLeftClick;
		open_SpoilUI.OnUpdate += Universal_OnUpdate;
		open_SpoilUI.OnMouseOver += Universal_MouseOver;
		panel.Append(open_SpoilUI);

		open_SkillUI = new("Select skill", 1.5f);
		open_SkillUI.OnLeftClick += Open_SkillUI_OnLeftClick;
		open_SkillUI.OnUpdate += Universal_OnUpdate;
		open_SkillUI.OnMouseOver += Universal_MouseOver;
		open_SkillUI.MarginTop = open_SpoilUI.Height.Pixels + 40;
		panel.Append(open_SkillUI);

		open_RelicUI = new("Relic creative menu", 1.5f);
		open_RelicUI.OnLeftClick += open_RelicUI_OnLeftClick;
		open_RelicUI.OnUpdate += Universal_OnUpdate;
		open_RelicUI.OnMouseOver += Universal_MouseOver;
		open_RelicUI.MarginTop = open_SkillUI.MarginTop + open_SkillUI.Height.Pixels + 40;
		panel.Append(open_RelicUI);

		exit_Menu = new("Back", 1.5f);
		exit_Menu.OnLeftClick += Exit_Menu_OnLeftClick;
		exit_Menu.OnUpdate += Exit_Menu_OnUpdate;
		exit_Menu.OnMouseOver += Exit_Menu_OnMouseOver;
		exit_Menu.HAlign = 1f;
		exit_Menu.VAlign = 1f;
		panel.Append(exit_Menu);
	}


	private void open_RelicUI_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		SoundEngine.PlaySound(SoundID.MenuOpen);
		ModContent.GetInstance<UniversalSystem>().ActivateDebugUI();
	}

	private void Open_SpoilUI_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		SoundEngine.PlaySound(SoundID.MenuOpen);
		ModContent.GetInstance<UniversalSystem>().ActivateDebugUI("spoil");
	}

	private void Open_SkillUI_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		SoundEngine.PlaySound(SoundID.MenuOpen);
		ModContent.GetInstance<UniversalSystem>().ActivateDebugUI("skill");
	}
	private void Universal_OnUpdate(UIElement affectedElement) {
		affectedElement.Disable_MouseItemUsesWhenHoverOverAUI();
		if (affectedElement.UniqueId == open_SpoilUI.UniqueId) {
			if (affectedElement.IsMouseHovering) {
				open_SpoilUI.TextColor = Color.Yellow;
			}
			else {
				open_SpoilUI.TextColor = Color.White;
			}
		}
		else if (affectedElement.UniqueId == open_RelicUI.UniqueId) {
			if (affectedElement.IsMouseHovering) {
				open_RelicUI.TextColor = Color.Yellow;
			}
			else {
				open_RelicUI.TextColor = Color.White;
			}
		}
		else if (affectedElement.UniqueId == open_SkillUI.UniqueId) {
			if (affectedElement.IsMouseHovering) {
				open_SkillUI.TextColor = Color.Yellow;
			}
			else {
				open_SkillUI.TextColor = Color.White;
			}
		}
	}
	private void Universal_MouseOver(UIMouseEvent evt, UIElement listeningElement) {
		SoundEngine.PlaySound(SoundID.MenuTick);
	}
	private void Exit_Menu_OnMouseOver(UIMouseEvent evt, UIElement listeningElement) {
		SoundEngine.PlaySound(SoundID.MenuTick);
	}

	private void Exit_Menu_OnUpdate(UIElement affectedElement) {
		affectedElement.Disable_MouseItemUsesWhenHoverOverAUI();
		if (affectedElement.IsMouseHovering) {
			exit_Menu.TextColor = Color.Yellow;
		}
		else {
			exit_Menu.TextColor = Color.White;
		}
	}

	private void Exit_Menu_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		SoundEngine.PlaySound(SoundID.MenuClose);
		ModContent.GetInstance<UniversalSystem>().DeactivateUI();
	}
}
