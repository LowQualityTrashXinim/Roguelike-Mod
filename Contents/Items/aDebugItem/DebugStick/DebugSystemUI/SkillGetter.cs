using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Roguelike.Common.Systems.Skill;
using Roguelike.Common.Systems.UI;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace Roguelike.Contents.Items.aDebugItem.DebugStick.DebugSystemUI;
class btn_Skill : UIImageButton {
	public int ModSkillID = -1;
	Texture2D texture = null;
	public void ChangeModSKillID(int newID) {
		ModSkillID = Math.Clamp(newID, 0, SkillModSystem.TotalCount);
	}
	public btn_Skill(Asset<Texture2D> texture) : base(texture) {
		this.texture = texture.Value;
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		this.Disable_MouseItemUsesWhenHoverOverAUI();
	}
	public override void Draw(SpriteBatch spriteBatch) {
		base.Draw(spriteBatch);
		var drawpos = GetInnerDimensions().Position() + texture.Size() * .5f;
		if (ModSkillID < 0 || ModSkillID >= SkillModSystem.TotalCount) {
			return;
		}
		ModSkill skill = SkillModSystem.GetSkill(ModSkillID);
		var skilltexture = ModContent.Request<Texture2D>(skill.Texture).Value;
		var origin = skilltexture.Size() * .5f;
		float scaling = ScaleCalculation(texture.Size(), skilltexture.Size());
		spriteBatch.Draw(skilltexture, drawpos, null, new Color(255, 255, 255), 0, origin, scaling, SpriteEffects.None, 0);
		if (IsMouseHovering) {
			string tooltipText = "";
			string Name = "";
			if (skill != null) {
				Name = skill.DisplayName;
				tooltipText = skill.Description;
				tooltipText +=
					$"\n[c/{Color.Yellow.Hex3()}:Skill duration] : {Math.Round(skill.Duration / 60f, 2)}s" +
					$"\n[c/{Color.DodgerBlue.Hex3()}:Energy require] : {skill.EnergyRequire}";
				if (skill.Skill_Type == SkillTypeID.Projectile) {
					tooltipText +=
						$"\n[c/{Color.Red.Hex3()}:Damage] : {skill.Damage}" +
						$"\n[c/{Color.Purple.Hex3()}:Knockback] : {skill.Knockback}" +
						$"\n[c/{Color.Gray.Hex3()}:Cool down] : {Math.Round(skill.Cooldown / 60f, 2)}s";
				}
			}
			UICommon.TooltipMouseText(Name + "\n" + tooltipText);
		}
	}
	public override void LeftClick(UIMouseEvent evt) {
		Main.LocalPlayer.GetModPlayer<SkillHandlePlayer>().RequestAddSkill_Inventory(ModSkillID);
		Main.NewText($"Added skill: {SkillModSystem.GetSkill(ModSkillID).DisplayName}");
	}
	private float ScaleCalculation(Vector2 originalTexture, Vector2 textureSize) => originalTexture.Length() / (textureSize.Length() * 1.5f);
}
class SkillGetterUI : UIState {
	ExitUI btn_select;
	UIPanel panel;
	Roguelike_UIText Set_AllSkill;
	Roguelike_UIText Set_NoSkill;

	private int linePosition;
	List<btn_Skill> btn_list;
	public const int SKILL_MAXLINE = 10;
	public override void OnInitialize() {
		btn_select = new();
		btn_select.HAlign = .65f;
		btn_select.VAlign = .7f;
		Append(btn_select);

		panel = new UIPanel();
		panel.HAlign = .5f;
		panel.VAlign = .5f;
		panel.UISetWidthHeight(500, 500);
		Append(panel);

		btn_list = new();

		Set_AllSkill = new("Add all skills");
		Set_AllSkill.HAlign = .65f;
		Set_AllSkill.VAlign = .4f;
		Set_AllSkill.OnLeftClick += Set_AllSkill_OnLeftClick;
		Append(Set_AllSkill);

		Set_NoSkill = new("Remove all skills");
		Set_NoSkill.HAlign = .65f;
		Set_NoSkill.VAlign = .5f;
		Set_NoSkill.OnLeftClick += Set_NoSkill_OnLeftClick; ;
		Append(Set_NoSkill);
	}

	private void Set_NoSkill_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		SkillHandlePlayer modplayer = Main.LocalPlayer.GetModPlayer<SkillHandlePlayer>();
		modplayer.ActiveSkill.Clear();
		modplayer.SkillInventory.Clear();
		Main.NewText("Removed all of the skill from player inventory");
	}

	private void Set_AllSkill_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		for (int i = 0; i < SkillModSystem.TotalCount; i++) {
			int skilltype = SkillModSystem.GetSkill(i).Type;
			Main.LocalPlayer.GetModPlayer<SkillHandlePlayer>().RequestAddSkill_Inventory(skilltype, count: 999, silent: true);
		}
		Main.NewText("added all of the skill to player inventory at max stack of 999");
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if (Set_AllSkill.IsMouseHovering) {
			Set_AllSkill.TextColor = Color.Yellow;
		}
		else {
			Set_AllSkill.TextColor = Color.White;
		}
		if (Set_NoSkill.IsMouseHovering) {
			Set_NoSkill.TextColor = Color.Yellow;
		}
		else {
			Set_NoSkill.TextColor = Color.White;
		}
	}

	public override void OnActivate() {
		btn_list.Clear();
		panel.RemoveAllChildren();
		int length = SkillModSystem.TotalCount;
		int lineCounter = 0;
		for (int i = 0; i < length; i++) {
			if (i % SKILL_MAXLINE == 0) {
				lineCounter++;
			}
			btn_Skill button = new(ModUtils.Get_SkillbackgroundTexture(i)) {
				Width = StyleDimension.FromPixels(44f),
				Height = StyleDimension.FromPixels(44f),
				Left = StyleDimension.FromPixels(i % SKILL_MAXLINE * 46.0f + 6.0f),
				Top = StyleDimension.FromPixels(i / SKILL_MAXLINE * 48.0f + 1.0f)
			};
			button.ModSkillID = i;
			btn_list.Add(button);
			panel.Append(button);

		}
	}
	public override void ScrollWheel(UIScrollWheelEvent evt) {
		linePosition -= MathF.Sign(evt.ScrollWheelValue);
		int offsetvalue = linePosition * SKILL_MAXLINE;
		int length = SkillModSystem.TotalCount;
		int offsetlength = length - offsetvalue;
		for (int i = 0; i < length; i++) {
			if (i < 0 || i >= btn_list.Count) {
				continue;
			}
			int arty = Math.Clamp(i + offsetvalue, 0, length - 1);
			btn_list[i].ChangeModSKillID(-1);
			if (i > offsetlength) {
				continue;
			}
			btn_list[i].ChangeModSKillID(arty);
		}
	}
}
