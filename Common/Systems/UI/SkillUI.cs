using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Roguelike.Common.Utils;
using Roguelike.Contents.Transfixion.Skill;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace Roguelike.Common.Systems.UI;
public class SkillUI : UIState {
	public ExitUI exitUI;
	public UIPanel panel;
	public UIText energyCostText;
	public UIText durationText;
	public Roguelike_UIPanel panel_ActiveSkill;
	public List<btn_SkillActive> list_activeskill = new();
	public Roguelike_UIPanel panel_Inventory;
	public List<btn_SkillSlotHolder> list_inventory = new();
	public Roguelike_TextBox txb_Search;
	public const int Row = 8;
	public const int Column = 12;
	public override void OnInitialize() {
		panel = new UIPanel();
		panel.UISetWidthHeight(820, 80);
		panel.HAlign = .5f;
		panel.VAlign = .5f;
		panel.MarginBottom = 700;
		Append(panel);

		energyCostText = new UIText("");
		energyCostText.VAlign = 0;
		panel.Append(energyCostText);
		durationText = new UIText("");
		durationText.VAlign = 1f;
		panel.Append(durationText);

		exitUI = new ExitUI(TextureAssets.InventoryBack10);
		exitUI.UISetWidthHeight(52, 52);
		exitUI.HAlign = 1f;
		exitUI.VAlign = .5f;
		panel.Append(exitUI);

		txb_Search = new("");
		txb_Search.VAlign = .5f;
		txb_Search.HAlign = 1f;
		txb_Search.UISetWidthHeight(150, 80);
		txb_Search.MarginRight = exitUI.Width.Pixels + 10;
		txb_Search.OnHoverText = "Search";
		panel.Append(txb_Search);

		panel_ActiveSkill = new();
		panel_ActiveSkill.UISetWidthHeight(400, 600);
		panel_ActiveSkill.MarginRight = panel_ActiveSkill.Width.Pixels + 10;
		panel_ActiveSkill.HAlign = .5f;
		panel_ActiveSkill.VAlign = .5f;
		Append(panel_ActiveSkill);

		panel_Inventory = new();
		panel_Inventory.UISetWidthHeight(400, 600);
		panel_Inventory.MarginLeft = panel_Inventory.Width.Pixels + 10;
		panel_Inventory.HAlign = .5f;
		panel_Inventory.VAlign = .5f;
		Append(panel_Inventory);
	}
	public void Add_SkillToInventory(int SkillID, int stack = 1) {
		int count = list_inventory.Count;
		for (int i = 0; i < count; i++) {
			if (list_inventory[i].sKillID == SkillID) {
				list_inventory[i].Stack++;
				return;
			}
		}
		if (panel_Inventory == null) {
			ModContent.GetInstance<UniversalSystem>().ActivateSkillUI();
		}
		btn_SkillSlotHolder btn = new(TextureAssets.InventoryBack10, SkillID);
		btn.OverflowHidden = true;
		btn.Stack = stack;
		btn.HAlign = (count % Row) / (Row - 1f);
		btn.VAlign = (count / Row) / (Column - 1f);
		btn.UISetWidthHeight(52, 52);
		btn.OnLeftClick += OnLeftClick_Inventory;
		panel_Inventory.Append(btn);
		list_inventory.Add(btn);
	}

	private void OnLeftClick_Inventory(UIMouseEvent evt, UIElement listeningElement) {
		Player player = Main.LocalPlayer;
		var modplayer = player.GetModPlayer<SkillHandlePlayer>();
		btn_SkillSlotHolder btn = (btn_SkillSlotHolder)listeningElement;
		if (btn.Stack > 0) {
			btn.Stack--;
			modplayer.SkillInventory[btn.sKillID]--;
			Add_ActiveSkill(btn.sKillID);
			modplayer.ActiveSkill.Add(btn.sKillID);
		}
		SoundEngine.PlaySound(SoundID.Grab);
	}
	public void Add_ActiveSkill(int skillID) {
		int count = list_activeskill.Count;
		btn_SkillActive btn = new(TextureAssets.InventoryBack10, skillID, count);
		btn.OverflowHidden = true;
		btn.HAlign = (count % Row) / (Row - 1f);
		btn.VAlign = (count / Row) / (Column - 1f);
		btn.UISetWidthHeight(52, 52);
		btn.OnLeftClick += OnLeftClick_ActiveSkill;
		panel_ActiveSkill.Append(btn);
		list_activeskill.Add(btn);
	}

	private void OnLeftClick_ActiveSkill(UIMouseEvent evt, UIElement listeningElement) {
		Player player = Main.LocalPlayer;
		var modplayer = player.GetModPlayer<SkillHandlePlayer>();

		if (listeningElement is btn_SkillActive btn) {
			Add_SkillToInventory(btn.SKillID);
			modplayer.SkillInventory[btn.SKillID]++;
			modplayer.ActiveSkill.RemoveAt(btn.WhoAmI);
			Reflesh_ActiveSkill();
		}
		SoundEngine.PlaySound(SoundID.Grab);
	}
	/// <summary>
	/// This will reflesh the skill UI automatically<br/>
	/// No need to manually remove the UI yourself
	/// </summary>
	public void Reflesh_ActiveSkill() {
		Player player = Main.LocalPlayer;
		var modplayer = player.GetModPlayer<SkillHandlePlayer>();
		for (int i = list_activeskill.Count - 1; i >= 0; i--) {
			list_activeskill[i].Remove();
		}
		list_activeskill.Clear();
		foreach (var item in modplayer.ActiveSkill) {
			Add_ActiveSkill(item);
		}
	}
	/// <summary>
	/// This will reflesh the skill UI automatically<br/>
	/// No need to manually remove the UI yourself
	/// </summary>
	public void Reflesh_InventorySkill() {
		Player player = Main.LocalPlayer;
		var modplayer = player.GetModPlayer<SkillHandlePlayer>();
		for (int i = list_inventory.Count - 1; i >= 0; i--) {
			list_inventory[i].Remove();
		}
		list_inventory.Clear();
		foreach (var item in modplayer.SkillInventory) {
			Add_SkillToInventory(item.Key, item.Value);
		}
	}

	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		Player player = Main.LocalPlayer;
		var modplayer = player.GetModPlayer<SkillHandlePlayer>();

		modplayer.SkillStatTotal(true, out int energy, out int duration);
		var color = energy <= modplayer.EnergyCap ? Color.Green : Color.Red;
		energyCostText.SetText($"[c/{color.Hex3()}:Energy cost = {energy}]");
		durationText.SetText($"Duration = {MathF.Round(duration / 60f, 2)}s");
	}
	public override void OnActivate() {
		var player = Main.LocalPlayer;
		Reflesh_ActiveSkill();
		Reflesh_InventorySkill();
	}
}
/// <summary>
/// This is a UI button for active skill in skill window<br/>
/// </summary>
public class btn_SkillActive : Roguelike_UIImageButton {
	public int SKillID = -1;
	public int WhoAmI = -1;
	Asset<Texture2D> Texture;
	public btn_SkillActive(Asset<Texture2D> texture, int skillID, int WhoAmI) : base(texture) {
		SetVisibility(1, .67f);
		SKillID = skillID;
		Texture = texture;
		this.WhoAmI = WhoAmI;
	}
	public override void DrawImage(SpriteBatch spriteBatch) {
		base.DrawImage(spriteBatch);
		var drawpos = GetInnerDimensions().Position() + Texture.Size() * .5f;
		if (SKillID < 0 || SKillID >= SkillModSystem.TotalCount) {
			return;
		}
		string texturestring = SkillModSystem.GetSkill(SKillID).Texture;
		var skilltexture = ModContent.Request<Texture2D>(texturestring).Value;
		var origin = skilltexture.Size() * .5f;
		float scaling = ScaleCalculation(Texture.Size(), skilltexture.Size());
		spriteBatch.Draw(skilltexture, drawpos, null, new Color(255, 255, 255), 0, origin, scaling, SpriteEffects.None, 0);
		if (IsMouseHovering) {
			string tooltipText = "";
			string Name = "";
			ModSkill skill = SkillModSystem.GetSkill(SKillID);
			if (skill != null) {
				Name = skill.DisplayName;
				tooltipText = skill.Description;
				tooltipText +=
					$"\n[c/{Color.Yellow.Hex3()}:Skill duration] : {Math.Round(skill.Duration / 60f, 2)}s" +
					$"\n[c/{new Color(243, 171, 77).Hex3()}:Energy require] : {skill.EnergyRequire}";
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
	private float ScaleCalculation(Vector2 originalTexture, Vector2 textureSize) => originalTexture.Length() / (textureSize.Length() * 1.5f);
}
public class btn_SkillSlotHolder : UIImageButton {
	public int sKillID = -1;
	public int Stack = 0;
	Texture2D Texture;
	public btn_SkillSlotHolder(Asset<Texture2D> texture, int SkillID) : base(texture) {
		sKillID = SkillID;
		Texture = texture.Value;
		SetVisibility(1, .67f);
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if (ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
	}
	public override void Draw(SpriteBatch spriteBatch) {
		base.Draw(spriteBatch);
		var drawpos = GetInnerDimensions().Position() + Texture.Size() * .5f;
		if (sKillID < 0 || sKillID >= SkillModSystem.TotalCount) {
			return;
		}
		string texturestring = SkillModSystem.GetSkill(sKillID).Texture;
		var skilltexture = ModContent.Request<Texture2D>(texturestring).Value;
		var origin = skilltexture.Size() * .5f;
		float scaling = ScaleCalculation(Texture.Size(), skilltexture.Size());
		spriteBatch.Draw(skilltexture, drawpos, null, new Color(255, 255, 255), 0, origin, scaling, SpriteEffects.None, 0);
		if (IsMouseHovering) {
			string tooltipText = "";
			string Name = "";
			ModSkill skill = SkillModSystem.GetSkill(sKillID);
			if (skill != null) {
				Name = skill.DisplayName;
				tooltipText = skill.Description;
				tooltipText +=
					$"\n[c/{Color.Yellow.Hex3()}:Skill duration] : {Math.Round(skill.Duration / 60f, 2)}s" +
					$"\n[c/{new Color(243, 171, 77).Hex3()}:Energy require] : {skill.EnergyRequire}";
				if (skill.Skill_Type == SkillTypeID.Projectile) {
					tooltipText +=
						$"\n[c/{Color.Red.Hex3()}:Damage] : {skill.Damage}" +
						$"\n[c/{Color.Purple.Hex3()}:Knockback] : {skill.Knockback}" +
						$"\n[c/{Color.Gray.Hex3()}:Cool down] : {Math.Round(skill.Cooldown / 60f, 2)}s";
				}
			}
			UICommon.TooltipMouseText(Name + "\n" + tooltipText);
		}
		Terraria.Utils.DrawBorderString(spriteBatch, Stack.ToString(), drawpos + origin * .5f, Color.White, 1f);
	}
	private float ScaleCalculation(Vector2 originalTexture, Vector2 textureSize) => originalTexture.Length() / (textureSize.Length() * 1.5f);
}
