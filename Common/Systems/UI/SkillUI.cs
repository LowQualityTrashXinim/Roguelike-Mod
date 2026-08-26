using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Roguelike.Common.Systems.Skill;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using System;
using System.Collections.Generic;
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

	public Roguelike_TextBox txb_Loadout;
	public Roguelike_UIImageButton btn_Loadout_Add;
	public Roguelike_UIImageButton btn_Loadout_Remove;
	public Roguelike_UIImageButton btn_Loadout_Modify;
	public Roguelike_UIPanel panel_Loadout;
	public Loadout_TextPanel[] btn_Loadout = new Loadout_TextPanel[10];
	public const int Row = 15;
	public const int Column = 5;
	public int CurrentSelect_btn_Loadout = -1;
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

		panel_ActiveSkill = new();
		panel_ActiveSkill.UISetWidthHeight(820, 300);
		panel_ActiveSkill.MarginBottom = panel_ActiveSkill.Height.Pixels + 10;
		panel_ActiveSkill.HAlign = .5f;
		panel_ActiveSkill.VAlign = .5f;
		Append(panel_ActiveSkill);

		panel_Inventory = new();
		panel_Inventory.UISetWidthHeight(820, 300);
		panel_Inventory.MarginTop = panel_Inventory.Height.Pixels + 10;
		panel_Inventory.HAlign = .5f;
		panel_Inventory.VAlign = .5f;
		Append(panel_Inventory);

		Initialize_Loadout();
	}

	private void Initialize_Loadout() {
		panel_Loadout = new();
		panel_Loadout.HAlign = .5f;
		panel_Loadout.VAlign = .5f;
		panel_Loadout.UISetWidthHeight(200, 610);
		panel_Loadout.MarginLeft = panel.GetOuterDimensions().Width + panel_Loadout.Width.Pixels + 10;
		Append(panel_Loadout);

		txb_Loadout = new("");
		txb_Loadout.VAlign = .5f;
		txb_Loadout.HAlign = 1f;
		txb_Loadout.UISetWidthHeight(150, 80);
		txb_Loadout.MarginRight = exitUI.Width.Pixels + 10;
		txb_Loadout.OnHoverText = "Loadout name";
		txb_Loadout.MaxText = 50;
		panel.Append(txb_Loadout);

		Asset<Texture2D> ass = TextureAssets.InventoryBack10;
		btn_Loadout_Add = new(ass);
		btn_Loadout_Add.SetPostTex(ModContent.Request<Texture2D>(ModTexture.AddSprite));
		btn_Loadout_Add.SetVisibility(.6f, 1);
		btn_Loadout_Add.MarginRight = txb_Loadout.GetOuterDimensions().Width + 10;
		btn_Loadout_Add.HAlign = 1;
		btn_Loadout_Add.OnLeftClick += Btn_Loadout_Add_OnLeftClick;
		panel.Append(btn_Loadout_Add);

		btn_Loadout_Remove = new(ass);
		btn_Loadout_Remove.SetPostTex(ModContent.Request<Texture2D>(ModTexture.CrossSprite));
		btn_Loadout_Remove.SetVisibility(.6f, 1);
		btn_Loadout_Remove.HAlign = 1;

		btn_Loadout_Remove.MarginRight =
			+btn_Loadout_Add.GetOuterDimensions().Width + 10;

		btn_Loadout_Remove.OnLeftClick += Btn_Loadout_Remove_OnLeftClick;
		btn_Loadout_Remove.HoverText = "Remove";
		panel.Append(btn_Loadout_Remove);


		btn_Loadout_Modify = new(ass);
		btn_Loadout_Modify.SetPostTex(ModContent.Request<Texture2D>(ModTexture.ACCESSORIESSLOT));
		btn_Loadout_Modify.SetVisibility(.6f, 1);
		btn_Loadout_Modify.HAlign = 1;

		btn_Loadout_Modify.MarginRight =
			+btn_Loadout_Remove.GetOuterDimensions().Width + 10;

		btn_Loadout_Modify.OnLeftClick += Btn_Loadout_Modify_OnLeftClick;
		btn_Loadout_Modify.HoverText = "Modify";
		panel.Append(btn_Loadout_Modify);
		for (int i = 0; i < btn_Loadout.Length; i++) {
			var item = btn_Loadout[i];
			if (item == null) {
				continue;
			}
			item.Remove();
		}
		Player player = Main.LocalPlayer;
		SkillHandlePlayer modplayer = player.GetModPlayer<SkillHandlePlayer>();
		Array.Fill(btn_Loadout, new Loadout_TextPanel(new()));
		for (int i = 0; i < btn_Loadout.Length; i++) {
			SkillLoadOut load = new();
			if (i <= modplayer.loadout.Count - 1) {
				load = modplayer.loadout[i];
			}
			var item = new Loadout_TextPanel(load);
			item.Width.Percent = 1;
			item.Height.Pixels = 80;
			item.HAlign = .5f;
			item.VAlign = i / (float)(btn_Loadout.Length - 1);
			panel_Loadout.Append(item);
			btn_Loadout[i] = item;
		}
	}

	private void Btn_Loadout_Modify_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		foreach (var item in btn_Loadout) {
			if (item.UniqueId == CurrentSelect_btn_Loadout) {
				Player player = Main.LocalPlayer;
				SkillHandlePlayer modplayer = player.GetModPlayer<SkillHandlePlayer>();
				item.Set_Loadout(new(modplayer.ActiveSkill, txb_Loadout.Text));
				break;
			}
		}
	}

	private void Btn_Loadout_Remove_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		foreach (var item in btn_Loadout) {
			if (item.UniqueId == CurrentSelect_btn_Loadout) {
				item.Set_Loadout(new());
				break;
			}
		}
	}

	private void Btn_Loadout_Add_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		foreach (var item in btn_Loadout) {
			if (item.Get_Loadout().list_SkillLoadOut.Count < 1) {
				Player player = Main.LocalPlayer;
				SkillHandlePlayer modplayer = player.GetModPlayer<SkillHandlePlayer>();
				item.Set_Loadout(new(modplayer.ActiveSkill, txb_Loadout.Text));
				break;
			}
		}
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
			if (!SkillModSystem.GetSkill(btn.sKillID).CanBeActive()) {
				return;
			}
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
			Refresh_ActiveSkill();
		}
		SoundEngine.PlaySound(SoundID.Grab);
	}
	/// <summary>
	/// This will refresh the skill UI automatically<br/>
	/// No need to manually remove the UI yourself
	/// </summary>
	public void Refresh_ActiveSkill() {
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
	/// This will refresh the skill UI automatically<br/>
	/// No need to manually remove the UI yourself
	/// </summary>
	public void Refresh_InventorySkill() {
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
		int energy = modplayer.SimulateSkillCost();
		int duration = modplayer.SimulateSkillDuration();
		var color = energy <= modplayer.EnergyCap ? Color.Green : Color.Red;
		energyCostText.SetText($"[c/{color.Hex3()}:Energy cost = {energy}]");
		durationText.SetText($"Duration = {MathF.Round(duration / 60f, 2)}s");

		txb_Loadout.Disable_MouseItemUsesWhenHoverOverAUI();

		foreach (var item in btn_Loadout) {
			if (item.UniqueId == CurrentSelect_btn_Loadout) {
				item.BorderColor = Color.Yellow;
				item.TextColor = Color.Yellow;
			}
			else {
				item.BorderColor = Color.White;
				item.TextColor = Color.White;
			}
		}
	}
	public override void OnActivate() {
		var player = Main.LocalPlayer;
		Refresh_ActiveSkill();
		Refresh_InventorySkill();
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
public class Loadout_TextPanel : Roguelike_UITextPanel {
	SkillLoadOut Loadout = new();
	public Loadout_TextPanel(SkillLoadOut loadout, float textScale = 1, bool large = false) : base("Empty loadout", textScale, large) {
		Loadout = loadout;
	}
	public void Set_Loadout(SkillLoadOut loadout) {
		Loadout.list_SkillLoadOut.Clear();
		Loadout.Change_Name(loadout.Name);
		Loadout.Change_LoadOut(loadout.list_SkillLoadOut);
	}
	public SkillLoadOut Get_Loadout() => Loadout;
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if (Loadout != null) {
			SetText(Loadout.Name);
		}
		timer = ModUtils.CountDown(timer);
		this.Disable_MouseItemUsesWhenHoverOverAUI();
	}
	int timer = 0;
	public override void LeftClick(UIMouseEvent evt) {
		SkillUI ui = ModContent.GetInstance<UniversalSystem>().skillUIstate;
		ui.CurrentSelect_btn_Loadout = this.UniqueId;
		ui.txb_Loadout.SetText(Loadout.Name);
		if (timer <= 0) {
			timer = 60;
			return;
		}
		Player player = Main.LocalPlayer;
		SkillHandlePlayer modplayer = player.GetModPlayer<SkillHandlePlayer>();
		foreach (int item in modplayer.ActiveSkill) {
			if (modplayer.SkillInventory.ContainsKey(item)) {
				modplayer.SkillInventory[item]++;
			}
			else {
				modplayer.SkillInventory.Add(item, 1);
			}
		}
		modplayer.ActiveSkill.Clear();
		modplayer.ActiveSkill.AddRange(Loadout.list_SkillLoadOut);
		foreach (int item in modplayer.ActiveSkill) {
			if (modplayer.SkillInventory.ContainsKey(item)) {
				modplayer.SkillInventory[item]--;
			}
		}
		ui.Refresh_ActiveSkill();
		ui.Refresh_InventorySkill();
	}
}
