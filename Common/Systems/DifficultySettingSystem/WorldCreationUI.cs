using Humanizer;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using System;
using System.Net.Sockets;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using static System.Net.Mime.MediaTypeNames;

namespace Roguelike.Common.Systems.DifficultySettingSystem;
public class DifficultySettingSystem : ModSystem {
	public override void Load() {
		On_UIWorldCreation.BuildPage += On_UIWorldCreation_BuildPage;
		if (!Main.dedServ) {
			difficulty = new();
		}
	}
	WorldDifficultyUI difficulty;
	Roguelike_UIImageButton difficulty_button;
	private void On_UIWorldCreation_BuildPage(On_UIWorldCreation.orig_BuildPage orig, UIWorldCreation self) {
		orig(self);
		difficulty_button = new(ModContent.Request<Texture2D>(ModTexture.Mod_icon));
		difficulty_button.UISetWidthHeight(80, 80);
		difficulty_button.OnLeftClick += Difficulty_button_OnLeftClick;
		difficulty_button.HAlign = .68f;
		difficulty_button.VAlign = .25f;
		self.Append(difficulty_button);
	}

	private void Difficulty_button_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		Main.MenuUI.SetState(difficulty);
	}
	public byte Enemy_HP = 0;//max 10
	public byte Enemy_DMG = 0;//max 10
	public byte Boss_DMG = 0;//max 10
	public byte Boss_HP = 0;//max 10
	public byte Boss_DMGPercentage = 0;//max 10
	public byte Boss_ProgressionLock = 0;//max vanilla boss

	public bool Player_ReviveCurse = false;
	public bool Player_HitTakenEffectiveness = false;
	public bool Player_TimeRestriction = false;
	public byte Player_TimeRestriction_Scale = 0;

	public byte World_ReduceHouseLoot = 0;//max 10
	public byte World_IncreasesSpawnRate = 0;//max 10
	public byte World_EnemyToElite = 0;
	public bool World_EnemyRevive = false;
	public bool World_BiomeModifier = false;

	public static Asset<Texture2D> defaultTextureAsset = ModContent.Request<Texture2D>(ModTexture.ACCESSORIESSLOT);
}
public class WorldDifficultyUI : UIState {
	public Roguelike_UIPanel main_panel;
	public DifficultyOption[] Arr_difficulty;
	public int SettingPage1_DifficultyValue = 0;
	public int SettingPage2_DifficultyValue = 0;
	public Special_ExitUI exit;

	public Roguelike_UIPanel header_panel;
	public Roguelike_UITextPanel terrariaMode;
	public Roguelike_UITextPanel bossrushMode;
	public Roguelike_UITextPanel nightmareMode;
	public WorldDifficultyUI() {
		main_panel = new();
		main_panel.UISetWidthHeight(1000, 800);
		main_panel.HAlign = .5f;
		main_panel.VAlign = .5f;
		Append(main_panel);

		header_panel = new();
		header_panel.Width.Percent = 1f;
		header_panel.Height.Pixels = 60;
		header_panel.HAlign = .5f;
		header_panel.PaddingTop = 0;
		header_panel.PaddingBottom = 0;
		main_panel.Append(header_panel);

		terrariaMode = new("Terraria mode");
		terrariaMode.Width.Pixels = 150;
		terrariaMode.HighlightColor = Color.Yellow;
		terrariaMode.OnLeftClick += TerrariaMode_OnLeftClick;
		terrariaMode.VAlign = .5f;
		header_panel.Append(terrariaMode);

		bossrushMode = new("Boss rush mode");
		bossrushMode.Width.Pixels = 150;
		bossrushMode.HighlightColor = Color.Yellow;
		bossrushMode.OnLeftClick += BossrushMode_OnLeftClick;
		bossrushMode.VAlign = .5f;
		bossrushMode.MarginLeft = 160;
		header_panel.Append(bossrushMode);

		nightmareMode = new("Nightmare mode");
		nightmareMode.Width.Pixels = 150;
		nightmareMode.HighlightColor = Color.Yellow;
		nightmareMode.OnLeftClick += NightmareMode_OnLeftClick;
		nightmareMode.VAlign = .5f;
		nightmareMode.MarginLeft = 320;
		header_panel.Append(nightmareMode);

		Arr_difficulty = new DifficultyOption[10];
		for (int i = 0; i < Arr_difficulty.Length; i++) {
			Arr_difficulty[i] = new(0);
		}
		SettingPage1();
		exit = new(DifficultySettingSystem.defaultTextureAsset);
		exit.UISetWidthHeight(52, 52);
		exit.HAlign = 1f;
		exit.VAlign = .5f;
		header_panel.Append(exit);
	}
	public override void OnInitialize() {
		string text = ModUtils.LocalizationText("SystemTooltip", "TerrariaMode");
		terrariaMode.HoverText = text;
		text = ModUtils.LocalizationText("SystemTooltip", "BossRushMode");
		bossrushMode.HoverText = text;
		text = ModUtils.LocalizationText("SystemTooltip", "NightmareMode");
		nightmareMode.HoverText = text;
	}
	private void NightmareMode_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		nightmareMode.Highlight = !nightmareMode.Highlight;
		RoguelikeWorldProperty.NightmareWorld = nightmareMode.Highlight;
	}

	private void BossrushMode_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		bossrushMode.Highlight = !bossrushMode.Highlight;
		RoguelikeWorldProperty.BossRushWorld = bossrushMode.Highlight;
		RoguelikeWorldProperty.RoguelikeWorld = !RoguelikeWorldProperty.BossRushWorld;
	}

	private void TerrariaMode_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		terrariaMode.Highlight = !terrariaMode.Highlight;
		bossrushMode.Highlight = false;
		RoguelikeWorldProperty.BossRushWorld = false;
		RoguelikeWorldProperty.RoguelikeWorld = !terrariaMode.Highlight;
	}

	/// <summary>
	/// Remember to force clear before setting up new page
	/// </summary>
	private void SettingPage1() {
		int index = 0;
		//1
		Arr_difficulty[index].KillYourSelf();
		Arr_difficulty[index].ScaleCap = 10;
		Arr_difficulty[index].ScalerValue = 10;
		Arr_difficulty[index].ScaleDifficultyOption = true;
		Arr_difficulty[index].ToggleOption = false;
		Arr_difficulty[index].SetUp("Enemy max health", "Increases enemy's maximum health");
		for (int i = 0; i < Arr_difficulty[index].ScaleCap; i++) {
			Arr_difficulty[index].scaler[i].OnLeftClick += Enemy_HP_scale;
		}
		Arr_difficulty[index].Finish(main_panel, .1f);

		//2
		index++;
		Arr_difficulty[index].KillYourSelf();
		Arr_difficulty[index].ScaleCap = 10;
		Arr_difficulty[index].ScalerValue = 10;
		Arr_difficulty[index].ScaleDifficultyOption = true;
		Arr_difficulty[index].ToggleOption = false;
		Arr_difficulty[index].SetUp("Enemy damage", "Increases enemy's damage output");
		for (int i = 0; i < Arr_difficulty[index].ScaleCap; i++) {
			Arr_difficulty[index].scaler[i].OnLeftClick += Enemy_Damage_Scale;
		}
		Arr_difficulty[index].Finish(main_panel, .2f);

		//3
		index++;
		Arr_difficulty[index].KillYourSelf();
		Arr_difficulty[index].ScaleCap = 10;
		Arr_difficulty[index].ScalerValue = 10;
		Arr_difficulty[index].ScaleDifficultyOption = true;
		Arr_difficulty[index].ToggleOption = false;
		Arr_difficulty[index].SetUp("Boss maximum health", "Increases boss's maximum health");
		for (int i = 0; i < Arr_difficulty[index].ScaleCap; i++) {
			Arr_difficulty[index].scaler[i].OnLeftClick += Boss_Health_Scale;
		}
		Arr_difficulty[index].Finish(main_panel, .3f);

		//4
		index++;
		Arr_difficulty[index].KillYourSelf();
		Arr_difficulty[index].ScaleCap = 10;
		Arr_difficulty[index].ScalerValue = 10;
		Arr_difficulty[index].ScaleDifficultyOption = true;
		Arr_difficulty[index].ToggleOption = false;
		Arr_difficulty[index].SetUp("Boss damage", "Increases boss's damage output");
		for (int i = 0; i < Arr_difficulty[index].ScaleCap; i++) {
			Arr_difficulty[index].scaler[i].OnLeftClick += Boss_Damage_Scale;
		}
		Arr_difficulty[index].Finish(main_panel, .4f);

		//5
		index++;
		Arr_difficulty[index].KillYourSelf();
		Arr_difficulty[index].ScaleCap = 10;
		Arr_difficulty[index].ScalerValue = 5;
		Arr_difficulty[index].ScaleDifficultyOption = true;
		Arr_difficulty[index].ToggleOption = false;
		Arr_difficulty[index].SetUp("Boss %damage", "Boss can deal percentage damage\nPercentage damage deal damage based on your maximum health");
		for (int i = 0; i < Arr_difficulty[index].ScaleCap; i++) {
			Arr_difficulty[index].scaler[i].OnLeftClick += Boss_PercentageDamage_Scale;
		}
		Arr_difficulty[index].Finish(main_panel, .5f);

		//6
		index++;
		Arr_difficulty[index].KillYourSelf();
		Arr_difficulty[index].ScaleCap = 17;
		Arr_difficulty[index].ScalerValue = 10;
		Arr_difficulty[index].ScaleDifficultyOption = true;
		Arr_difficulty[index].ToggleOption = false;
		Arr_difficulty[index].SetUp("Boss progression lock", "Moon lord can only be beaten after certain amount of boss is defeated");
		for (int i = 0; i < Arr_difficulty[index].ScaleCap; i++) {
			Arr_difficulty[index].scaler[i].OnLeftClick += Boss_Progression_Scale;
		}
		Arr_difficulty[index].Finish(main_panel, .6f);

		//7
		index++;
		Arr_difficulty[index].KillYourSelf();
		Arr_difficulty[index].ScaleDifficultyOption = false;
		Arr_difficulty[index].ToggleOption = true;
		Arr_difficulty[index].ToggleValue = 20;
		Arr_difficulty[index].SetUp("Cursed of revival", "Getting revived will reduce you maximum health by 20%, this will only occur once");
		Arr_difficulty[index].toggler.OnLeftClick += Player_Revival_Scale;
		Arr_difficulty[index].Finish(main_panel, .7f);

		//8
		index++;
		Arr_difficulty[index].KillYourSelf();
		Arr_difficulty[index].ScaleDifficultyOption = false;
		Arr_difficulty[index].ToggleOption = true;
		Arr_difficulty[index].ToggleValue = 30;
		Arr_difficulty[index].SetUp("Hit taken effectiveness", "Taking multiple hit will increases the damage of next taken hit by 10% which is stackable and have 2s duration for every decrease");
		Arr_difficulty[index].toggler.OnLeftClick += Player_HitTakenEffective_Scale;
		Arr_difficulty[index].Finish(main_panel, .8f);

		//9
		index++;
		Arr_difficulty[index].KillYourSelf();
		Arr_difficulty[index].ScaleCap = 10;
		Arr_difficulty[index].ScaleDifficultyOption = true;
		Arr_difficulty[index].ToggleOption = true;
		Arr_difficulty[index].ToggleValue = 30;
		Arr_difficulty[index].ScalerValue = 10;
		Arr_difficulty[index].SetUp("Time restriction", " Reduce the time that is available to the player to beat the mod, by default player have unlimited time, and once player activated this option, they will be left with 3 hours to beat the mod.");
		Arr_difficulty[index].toggler.OnLeftClick += Player_TimeRestriction_Toggle;
		for (int i = 0; i < Arr_difficulty[index].ScaleCap; i++) {
			Arr_difficulty[index].scaler[i].OnLeftClick += Player_TimeRestriction_Scale;
		}
		Arr_difficulty[index].Finish(main_panel, .9f);

		//10
		index++;
		Arr_difficulty[index].KillYourSelf();
		Arr_difficulty[index].ScaleCap = 10;
		Arr_difficulty[index].ScalerValue = 5;
		Arr_difficulty[index].ScaleDifficultyOption = true;
		Arr_difficulty[index].ToggleOption = false;
		Arr_difficulty[index].SetUp("Reduce abandon house WIP", "reduce amount of house created during world generation");
		for (int i = 0; i < Arr_difficulty[index].ScaleCap; i++) {
			Arr_difficulty[index].scaler[i].OnLeftClick += World_ReduceHouse_Scale;
		}
		Arr_difficulty[index].Finish(main_panel, 1);
	}
	/// <summary>
	/// Remember to force clear before setting up new page
	/// </summary>
	private void SettingPage2() {
		int index = 0;
		Arr_difficulty[index].KillYourSelf();
		Arr_difficulty[index].ScaleCap = 10;
		Arr_difficulty[index].ScalerValue = 5;
		Arr_difficulty[index].ScaleDifficultyOption = true;
		Arr_difficulty[index].ToggleOption = false;
		Arr_difficulty[index].SetUp("Increase spawn rate", "Increases enemy's spawn rate in the world");
		for (int i = 0; i < Arr_difficulty[index].ScaleCap; i++) {
			Arr_difficulty[index].scaler[i].OnLeftClick += World_SpawnRate_Scale;
		}
		Arr_difficulty[index].Finish(main_panel, index * .1f);

		index++;
		Arr_difficulty[index].KillYourSelf();
		Arr_difficulty[index].ScaleCap = 10;
		Arr_difficulty[index].ScalerValue = 10;
		Arr_difficulty[index].ScaleDifficultyOption = true;
		Arr_difficulty[index].ToggleOption = false;
		Arr_difficulty[index].SetUp("Elite status", "Increases chance of enemy spawn with elite status");
		for (int i = 0; i < Arr_difficulty[index].ScaleCap; i++) {
			Arr_difficulty[index].scaler[i].OnLeftClick += World_EnemyElite_Scale;
		}
		Arr_difficulty[index].Finish(main_panel, index * .1f);

		index++;
		Arr_difficulty[index].KillYourSelf();
		Arr_difficulty[index].ToggleValue = 30;
		Arr_difficulty[index].ScaleDifficultyOption = false;
		Arr_difficulty[index].ToggleOption = true;
		Arr_difficulty[index].SetUp("Revive status", "Normal enemy have 1 in 3 chance to be revived");
		Arr_difficulty[index].toggler.OnLeftClick += World_EnemyRevive_Toggle;
		Arr_difficulty[index].Finish(main_panel, index * .1f);

		index++;
		Arr_difficulty[index].KillYourSelf();
		Arr_difficulty[index].ToggleValue = 20;
		Arr_difficulty[index].ScaleDifficultyOption = false;
		Arr_difficulty[index].ToggleOption = true;
		Arr_difficulty[index].SetUp("Biome modifier", "Biome now have their default modifier will always active");
		Arr_difficulty[index].toggler.OnLeftClick += World_BiomeModifier_Toggle;
		Arr_difficulty[index].Finish(main_panel, index * .1f);
	}
	private void Enemy_HP_scale(UIMouseEvent evt, UIElement listeningElement) {
		DifficultySettingSystem system = ModContent.GetInstance<DifficultySettingSystem>();
		CommonScale(0, listeningElement.UniqueId,
			(i) => { system.Enemy_HP = i; },
			() => { system.Enemy_HP = 0; });
	}
	private void Enemy_Damage_Scale(UIMouseEvent evt, UIElement listeningElement) {
		DifficultySettingSystem system = ModContent.GetInstance<DifficultySettingSystem>();
		CommonScale(1, listeningElement.UniqueId, (i) => { system.Enemy_DMG = i; }, () => { system.Enemy_DMG = 0; });
	}
	private void Boss_Health_Scale(UIMouseEvent evt, UIElement listeningElement) {
		DifficultySettingSystem system = ModContent.GetInstance<DifficultySettingSystem>();
		CommonScale(2, listeningElement.UniqueId, (i) => { system.Boss_HP = i; }, () => { system.Boss_HP = 0; });
	}
	private void Boss_Damage_Scale(UIMouseEvent evt, UIElement listeningElement) {
		DifficultySettingSystem system = ModContent.GetInstance<DifficultySettingSystem>();
		CommonScale(3, listeningElement.UniqueId, (i) => { system.Boss_DMG = i; }, () => { system.Boss_DMG = 0; });
	}
	private void Boss_PercentageDamage_Scale(UIMouseEvent evt, UIElement listeningElement) {
		DifficultySettingSystem system = ModContent.GetInstance<DifficultySettingSystem>();
		CommonScale(4, listeningElement.UniqueId, (i) => { system.Boss_DMGPercentage = i; }, () => { system.Boss_DMGPercentage = 0; });
	}
	private void Boss_Progression_Scale(UIMouseEvent evt, UIElement listeningElement) {
		DifficultySettingSystem system = ModContent.GetInstance<DifficultySettingSystem>();
		CommonScale(5, listeningElement.UniqueId, (i) => { system.Boss_ProgressionLock = i; }, () => { system.Boss_ProgressionLock = 0; });
	}
	private void Player_Revival_Scale(UIMouseEvent evt, UIElement listeningElement) {
		DifficultySettingSystem system = ModContent.GetInstance<DifficultySettingSystem>();
		Arr_difficulty[6].toggler.Highlight = !Arr_difficulty[6].toggler.Highlight;
		system.Player_ReviveCurse = Arr_difficulty[6].toggler.Highlight;
	}
	private void Player_HitTakenEffective_Scale(UIMouseEvent evt, UIElement listeningElement) {
		DifficultySettingSystem system = ModContent.GetInstance<DifficultySettingSystem>();
		Arr_difficulty[7].toggler.Highlight = !Arr_difficulty[7].toggler.Highlight;
		system.Player_HitTakenEffectiveness = Arr_difficulty[7].toggler.Highlight;
	}
	private void Player_TimeRestriction_Toggle(UIMouseEvent evt, UIElement listeningElement) {
		DifficultySettingSystem system = ModContent.GetInstance<DifficultySettingSystem>();
		Arr_difficulty[8].toggler.Highlight = !Arr_difficulty[8].toggler.Highlight;
		system.Player_TimeRestriction = Arr_difficulty[8].toggler.Highlight;
	}
	private void Player_TimeRestriction_Scale(UIMouseEvent evt, UIElement listeningElement) {
		DifficultySettingSystem system = ModContent.GetInstance<DifficultySettingSystem>();
		CommonScale(8, listeningElement.UniqueId,
			(i) => { system.Player_TimeRestriction_Scale = i; },
			() => { system.Player_TimeRestriction_Scale = 0; });
	}
	private void World_ReduceHouse_Scale(UIMouseEvent evt, UIElement listeningElement) {
		DifficultySettingSystem system = ModContent.GetInstance<DifficultySettingSystem>();
		CommonScale(9, listeningElement.UniqueId,
			(i) => { system.World_ReduceHouseLoot = i; },
			() => { system.World_ReduceHouseLoot = 0; });
	}
	private void World_SpawnRate_Scale(UIMouseEvent evt, UIElement listeningElement) {
		DifficultySettingSystem system = ModContent.GetInstance<DifficultySettingSystem>();
		CommonScale(0, listeningElement.UniqueId,
			(i) => { system.World_IncreasesSpawnRate = i; },
			() => { system.World_IncreasesSpawnRate = 0; });
	}
	private void World_EnemyElite_Scale(UIMouseEvent evt, UIElement listeningElement) {
		DifficultySettingSystem system = ModContent.GetInstance<DifficultySettingSystem>();
		CommonScale(1, listeningElement.UniqueId,
			(i) => { system.World_EnemyToElite = i; },
			() => { system.World_EnemyToElite = 0; });
	}
	private void World_EnemyRevive_Toggle(UIMouseEvent evt, UIElement listeningElement) {
		DifficultySettingSystem system = ModContent.GetInstance<DifficultySettingSystem>();
		Arr_difficulty[2].toggler.Highlight = !Arr_difficulty[2].toggler.Highlight;
		system.World_EnemyRevive = Arr_difficulty[2].toggler.Highlight;
	}
	private void World_BiomeModifier_Toggle(UIMouseEvent evt, UIElement listeningElement) {
		DifficultySettingSystem system = ModContent.GetInstance<DifficultySettingSystem>();
		Arr_difficulty[3].toggler.Highlight = !Arr_difficulty[3].toggler.Highlight;
		system.World_BiomeModifier = Arr_difficulty[3].toggler.Highlight;
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		for (int i = 0; i < Arr_difficulty.Length; i++) {
			if (Arr_difficulty[i] != null) {
				CommonValueText(i);
			}
		}
	}
	private void CommonValueText(int index) {
		int totalValue = 0;
		if (Arr_difficulty[index].ToggleOption && Arr_difficulty[index].toggler.Highlight) {
			totalValue += Arr_difficulty[index].ToggleValue;
		}
		if (Arr_difficulty[index].ToggleOption && Arr_difficulty[index].ScaleDifficultyOption) {
			if (Arr_difficulty[index].CurrentActivateIndex != -1 && Arr_difficulty[index].toggler.Highlight) {
				totalValue += Arr_difficulty[index].ScalerValue * Arr_difficulty[index].CurrentActivateIndex;
			}
		}
		else if (Arr_difficulty[index].ScaleDifficultyOption && Arr_difficulty[index].CurrentActivateIndex != -1) {
			totalValue += Arr_difficulty[index].ScalerValue * Arr_difficulty[index].CurrentActivateIndex;
		}
		Arr_difficulty[index].value.SetText(totalValue + "%");
	}
	private void CommonScale(int scaleIndex, int UniqueID, Action<byte> action, Action reset) {
		for (byte i = 1; i <= Arr_difficulty[scaleIndex].ScaleCap; i++) {
			int index = i - 1;
			if (Arr_difficulty[scaleIndex].scaler[index].UniqueId == UniqueID) {
				if (Arr_difficulty[scaleIndex].scaler[index].Highlight) {
					Arr_difficulty[scaleIndex].scaler[index].Highlight = false;
					reset();
					Arr_difficulty[scaleIndex].CurrentActivateIndex = -1;
				}
				else {
					Arr_difficulty[scaleIndex].scaler[index].Highlight = true;
					action(i);
					Arr_difficulty[scaleIndex].CurrentActivateIndex = i;
				}
			}
			else {
				Arr_difficulty[scaleIndex].scaler[index].Highlight = false;
			}
		}
	}
}
public class Special_ExitUI : UIImageButton {
	Texture2D textureInner;
	public Special_ExitUI(Asset<Texture2D> texture) : base(texture) {
		SetVisibility(.7f, 1f);
		textureInner = texture.Value;
	}

	public override void LeftClick(UIMouseEvent evt) {
		Main.MenuUI.GoBack();
		SoundEngine.PlaySound(SoundID.MenuClose);
	}
	public override void Draw(SpriteBatch spriteBatch) {
		base.Draw(spriteBatch);
		Texture2D texture = ModContent.Request<Texture2D>(ModTexture.CrossSprite).Value;
		Vector2 rect = this.GetDimensions().Position() + textureInner.Size() * .5f + Vector2.One;
		spriteBatch.Draw(texture, rect, null, Color.White, 0, textureInner.Size() * .5f, .7f, SpriteEffects.None, 0);
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		this.Disable_MouseItemUsesWhenHoverOverAUI();
		if (IsMouseHovering) {
			Main.instance.MouseText("Exit");
		}
	}
}
public class DifficultyOption {
	Roguelike_UITextPanel Difficulty_name;
	public Roguelike_UIImage[] scaler;
	public Roguelike_UIImage toggler;
	public Roguelike_UITextPanel value;
	Roguelike_UIPanel innerPanel;
	public int ToggleValue = 0;
	public int ScalerValue = 0;
	public int CurrentActivateIndex = -1;
	/// <summary>
	/// Enable this so that the game know this difficulty option can be toggle<br/>
	/// Setting it to <b>False</b> will disable the toggle difficulty UI
	/// </summary>
	public bool ToggleOption = false;
	/// <summary>
	/// Enable this so that the game know this difficulty option can be scaled<br/>
	/// Setting it to <b>False</b> will disable the scaler difficulty UI<br/>
	/// You must able adjust <see cref="ScaleCap"/> value to set the cap of this scaler
	/// </summary>
	public bool ScaleDifficultyOption = true;
	public int ScaleCap = 1;
	public DifficultyOption(int cap) {
		ScaleCap = cap;
		if (cap < 1) {
			return;
		}
		scaler = new Roguelike_UIImage[ScaleCap];
	}
	/// <summary>
	/// Only call this after you finish initializing in your own code sector<br/>
	/// This will not fully finish the UI, instead you must call <see cref="Finish"/><br/>
	/// This is so that you can add in your own event action for example like <see cref="toggler"/>
	/// </summary>
	public void SetUp(string name, string description) {
		Difficulty_name = new(name, .8f);
		Difficulty_name.Width.Set(200, 0);
		Difficulty_name.VAlign = .5f;
		Difficulty_name.HoverText = description;

		float Margin_Left_OffSet = 200 + 10;
		if (ToggleOption) {
			toggler = new(TextureAssets.InventoryBack6);
			toggler.HighlightColor = toggler.OriginalColor * .4f;
			toggler.VAlign = .5f;
			toggler.UISetWidthHeight(52, 52);
			toggler.MarginLeft += Margin_Left_OffSet;
			Margin_Left_OffSet += 52 + 10;
		}

		value = new("0%", .8f);
		value.HAlign = 1f;
		value.VAlign = .5f;
		value.Width.Pixels = 100;


		if (ScaleCap < 1 || !ScaleDifficultyOption) {
			return;
		}
		scaler = new Roguelike_UIImage[ScaleCap];
		for (int i = 0; i < ScaleCap; i++) {
			Roguelike_UIImage scale = new(DifficultySettingSystem.defaultTextureAsset);
			scale.VAlign = .5f;
			scale.HAlign = i / (float)ScaleCap;
			scale.MarginLeft = Margin_Left_OffSet + 10;
			scale.MarginRight = 70;
			scale.HighlightColor = scale.OriginalColor * .4f;
			scaler[i] = scale;
		}
	}
	/// <summary>
	/// Actually finish up the UI<br/>
	/// After calling this, you won't be able to modify the inner UI in this class
	/// </summary>
	public void Finish(UIElement parent, float Valign) {
		innerPanel = new();
		innerPanel.Width.Percent = 1;
		innerPanel.Height.Pixels = 60;
		innerPanel.HAlign = .5f;
		innerPanel.VAlign = Valign;
		innerPanel.PaddingTop = 0;
		innerPanel.PaddingBottom = 0;

		innerPanel.Append(Difficulty_name);
		innerPanel.Append(value);
		if (ToggleOption) {
			innerPanel.Append(toggler);
		}
		if (ScaleCap > 0) {
			for (int i = 0; i < scaler.Length; i++) {
				innerPanel.Append(scaler[i]);
			}
		}

		parent.Append(innerPanel);
	}
	public void KillYourSelf() {
		if (Difficulty_name != null)
			Difficulty_name.Remove();
		if (scaler != null) {
			if (ScaleCap > 1) {
				for (int i = 0; i < ScaleCap; i++) {
					if (scaler[i] != null)
						scaler[i].Remove();
				}
			}
			scaler = null;
		}
		if (toggler != null)
			toggler.Remove();
		if (innerPanel != null)
			innerPanel.Remove();
	}
}
