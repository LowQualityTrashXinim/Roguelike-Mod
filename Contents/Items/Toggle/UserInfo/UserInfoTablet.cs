using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;
using System;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent;
using System.Linq;
using Roguelike.Common.Systems;
using Roguelike.Common.Systems.ArtifactSystem;
using Roguelike.Contents.Items.RelicItem;
using Roguelike.Contents.Transfixion.WeaponEnchantment;
using Roguelike.Contents.Items.Lootbox;
using Roguelike.Contents.Items.Consumable.Potion;
using Roguelike.Contents.Items.Consumable.SpecialReward;
using Roguelike.Contents.Transfixion.Arguments;

using Roguelike.Texture;
using Roguelike.Common.Systems.IOhandle;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Contents.Transfixion.Perks;
using System.Collections;

namespace Roguelike.Contents.Items.Toggle.UserInfo {
	class UserInfoTablet : ModItem {
		public override void SetDefaults() {
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.autoReuse = false;
			Item.noUseGraphic = true;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			if (string.IsNullOrEmpty(InfoUI.InfoShowToItem)) {
				return;
			}
			tooltips.ForEach((t) => { if (t.Name != "ItemName") t.Hide(); });
			tooltips.Add(new(Mod, "Stats", InfoUI.InfoShowToItem.Substring(0, InfoUI.InfoShowToItem.Length - 1)));
		}
		public override bool? UseItem(Player player) {
			if (player.ItemAnimationJustStarted) {
				ModContent.GetInstance<UniversalSystem>().ActivateInfoUI();
			}
			return base.UseItem(player);
		}
	}
	class Info_ArtifactImage : Roguelike_UIImage {
		public Info_ArtifactImage(Asset<Texture2D> texture) : base(texture) {
		}
		public override void DrawImage(SpriteBatch spriteBatch) {
			var artifact = Artifact.GetArtifact(Main.LocalPlayer.GetModPlayer<ArtifactPlayer>().ActiveArtifact);
			var style = GetInnerDimensions();
			artifact.DrawInUI(spriteBatch, style);
		}
	}
	public class Roguelike_Info {
		public Roguelike_UIImageButton btn;
		public Roguelike_UIText text;
		public bool StatePressed = false;
		public InfoType info = null;
		public Roguelike_Info(UIElement state, InfoType type) {
			btn = new(ModContent.Request<Texture2D>(ModTexture.PinIcon));
			btn.UISetWidthHeight(18, 18);
			btn.OnLeftClick += Btn_OnLeftClick;
			btn.OnUpdate += Btn_OnUpdate;
			state.Append(btn);

			info = type;

			text = new(info.InfoText());
			text.MarginLeft = 24;
			state.Append(text);

		}

		public void Update() {
			text.SetText(info.InfoText());
		}
		private void Btn_OnUpdate(UIElement affectedElement) {
			affectedElement.Disable_MouseItemUsesWhenHoverOverAUI();
			if (info == null) {
				return;
			}
			if (StatePressed) {
				InfoUI.InfoShowToItem += info.InfoText() + "\n";
			}
		}

		private void Btn_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
			StatePressed = !StatePressed;
			if (StatePressed) {
				btn.SetVisibility(1f, 1f);
				text.TextColor = Color.Yellow;
			}
			else {
				btn.SetVisibility(1f, .4f);
				text.TextColor = Color.White;
			}
		}
		public void Hide(bool Hide) {
			text.Hide = Hide;
			btn.Hide = Hide;
		}
		public void SetAlign(float x, float y) {
			btn.HAlign = x;
			text.HAlign = x;
			btn.VAlign = y;
			text.VAlign = y;
		}
		public void SetInfo() {
			if (info == null) {
				return;
			}
			text.SetText(info.InfoText());
		}
		public void Visibility(bool hide) {
			text.Hide = hide;
			btn.Hide = hide;
		}
	}
	class InfoUI : UIState {
		public static string InfoShowToItem = string.Empty;
		UIPanel mainPanel;
		UIPanel panel;
		Roguelike_WrapTextUIPanel textpanel;
		Roguelike_UITextPanel generalTextPanel;
		Dictionary<Roguelike_UIText, int> textlist;
		UIImageButton btn_Stats;
		UIImageButton btn_ModStats;
		UIImageButton btn_Perks;
		ExitUI btn_Exit;
		int CurrentState = 0;
		public List<Roguelike_Info> list_info = new();
		public override void OnInitialize() {
			GeneralPage_Initialization();

			float marginTopForButton = 0;

			//Normal stats
			BasicStats_Initialization(ref marginTopForButton);

			//Modded stats
			ModdedStats_Initialization(ref marginTopForButton);

			//Perk
			PerkPage_Initialization(ref marginTopForButton);

			//Artifact
			ArtifactPage_Initialization(ref marginTopForButton);

			btn_Exit = new ExitUI(TextureAssets.InventoryBack);
			btn_Exit.HAlign = .5f;
			btn_Exit.VAlign = 1;
			btn_Exit.UISetWidthHeight(52, 52);
			panel.Append(btn_Exit);
		}
		public void GeneralPage_Initialization() {
			mainPanel = new();
			mainPanel.HAlign = .5f;
			mainPanel.VAlign = .5f;
			mainPanel.UISetWidthHeight(800, 700);
			Append(mainPanel);

			textpanel = new Roguelike_WrapTextUIPanel("");
			textpanel.HAlign = 1;
			textpanel.VAlign = .5f;
			textpanel.Width.Pixels = 650;
			textpanel.Height.Precent = 1;
			mainPanel.Append(textpanel);

			panel = new UIPanel();
			panel.VAlign = .5f;
			panel.Width.Pixels = 100;
			panel.Height.Precent = 1;
			mainPanel.Append(panel);

			generalTextPanel = new Roguelike_UITextPanel("");
			generalTextPanel.UISetWidthHeight(10, 10);
			generalTextPanel.Hide = true;
			Append(generalTextPanel);
		}
		public void BasicStats_Initialization(ref float marginForBtn) {
			list_info.Clear();

			btn_Stats = new UIImageButton(TextureAssets.InventoryBack);
			btn_Stats.HAlign = .5f;
			btn_Stats.UISetWidthHeight(52, 52);
			marginForBtn += btn_Stats.Height.Pixels + 10;
			btn_Stats.OnLeftClick += Btn_Stats_OnLeftClick;
			btn_Stats.SetVisibility(1, 1);
			panel.Append(btn_Stats);
		}
		public void ModdedStats_Initialization(ref float marginForBtn) {
			btn_ModStats = new UIImageButton(TextureAssets.InventoryBack);
			btn_ModStats.UISetWidthHeight(52, 52);
			btn_ModStats.HAlign = .5f;
			btn_ModStats.MarginTop = marginForBtn;
			marginForBtn += btn_ModStats.Height.Pixels + 10;
			btn_ModStats.OnLeftClick += Btn_ModStats_OnLeftClick;
			panel.Append(btn_ModStats);
		}
		public void PerkPage_Initialization(ref float marginForBtn) {
			textlist = new Dictionary<Roguelike_UIText, int>();
			btn_Perks = new UIImageButton(TextureAssets.InventoryBack);
			btn_Perks.HAlign = .5f;
			btn_Perks.UISetWidthHeight(52, 52);
			btn_Perks.MarginTop = marginForBtn;
			marginForBtn += btn_Perks.Height.Pixels + 10;
			btn_Perks.OnLeftClick += Btn_Perks_OnLeftClick;
			panel.Append(btn_Perks);
		}
		UIImageButton btn_Artifact;
		Info_ArtifactImage Info_artifact;
		Roguelike_WrapTextUIPanel artifactcustomtextpanel;
		Roguelike_UIPanel artifactHeaderpanel;
		Roguelike_UIText artifact_text;
		Roguelike_UIImageButton artifact_upgradebtn;
		Roguelike_UIPanel artifactUpgradePanel;

		public void ArtifactUpgradePanelAppend(Roguelike_UIPanel panel) {
			artifactUpgradePanel = panel;
			Append(artifactUpgradePanel);
		}
		public void ArtifactPage_Initialization(ref float marginForBtn) {
			btn_Artifact = new UIImageButton(TextureAssets.InventoryBack);
			btn_Artifact.UISetWidthHeight(52, 52);
			btn_Artifact.HAlign = .5f;
			btn_Artifact.MarginTop = marginForBtn;
			marginForBtn += btn_Artifact.Height.Pixels + 10;
			btn_Artifact.OnLeftClick += Btn_Artifact_OnLeftClick;
			panel.Append(btn_Artifact);

			artifactHeaderpanel = new();
			artifactHeaderpanel.Width.Percent = 1f;
			artifactHeaderpanel.Height.Pixels = 80;
			artifactHeaderpanel.Hide = true;
			textpanel.Append(artifactHeaderpanel);

			artifactcustomtextpanel = new Roguelike_WrapTextUIPanel("");
			artifactcustomtextpanel.VAlign = 1;
			artifactcustomtextpanel.Width.Percent = 1;
			artifactcustomtextpanel.Height.Precent = 1;
			artifactcustomtextpanel.Height.Pixels -= artifactHeaderpanel.Height.Pixels + 10;
			artifactcustomtextpanel.Hide = true;
			artifactcustomtextpanel.DrawPanel = false;
			textpanel.Append(artifactcustomtextpanel);

			Info_artifact = new Info_ArtifactImage(TextureAssets.InventoryBack);
			Info_artifact.VAlign = .5f;
			Info_artifact.Hide = true;
			artifactHeaderpanel.Append(Info_artifact);

			artifact_text = new("");
			artifact_text.VAlign = .5f;
			artifact_text.MarginLeft = Info_artifact.Width.Pixels + 10;
			artifact_text.Hide = true;
			artifactHeaderpanel.Append(artifact_text);

			artifact_upgradebtn = new(TextureAssets.InventoryBack);
			artifact_upgradebtn.UISetWidthHeight(52, 52);
			artifact_upgradebtn.HAlign = 1f;
			artifact_upgradebtn.Hide = true;
			artifact_upgradebtn.SetVisibility(.76f, 1f);
			artifact_upgradebtn.OnLeftClick += Artifact_upgradebtn_OnLeftClick;
			artifactHeaderpanel.Append(artifact_upgradebtn);

			artifactUpgradePanel = new();
			artifactUpgradePanel.UISetWidthHeight(600, 600);
			artifactUpgradePanel.HAlign = .5f;
			artifactUpgradePanel.VAlign = .5f;
			artifactUpgradePanel.Hide = true;
			Append(artifactUpgradePanel);
		}

		private void Artifact_upgradebtn_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
			artifactUpgradePanel.Hide = !artifactUpgradePanel.Hide;
		}

		private void Text_OnUpdate(UIElement affectedElement) {
			if (affectedElement.IsMouseHovering) {
				var text = textlist.Keys.Where(e => e.UniqueId == affectedElement.UniqueId).FirstOrDefault();
				if (text == null || text.Hide) {
					return;
				}
				generalTextPanel.Hide = false;
				int perkType = textlist[text];
				generalTextPanel.SetText(ModPerkLoader.GetPerk(perkType).Description);
				generalTextPanel.Left.Set(Main.MouseScreen.X, 0);
				generalTextPanel.Top.Set(Main.MouseScreen.Y, 0);
			}
		}
		/// <summary>
		/// Set true to hide item, related to artifact, otherwise false to show them
		/// </summary>
		/// <param name="sett"></param>
		public void ArtifactPage_VisibilitySetting(bool sett = false) {
			artifactHeaderpanel.Hide = sett;
			Info_artifact.Hide = sett;
			artifactcustomtextpanel.Hide = sett;
			artifact_text.Hide = sett;
			artifact_upgradebtn.Hide = sett;
			if (artifactcustomtextpanel.Text == "") {
				string line = "";
				var artifactplayer = Main.LocalPlayer.GetModPlayer<ArtifactPlayer>();
				line = $"Current active artifact : {Artifact.GetArtifact(artifactplayer.ActiveArtifact).DisplayName}";
				line += $"\n{Artifact.GetArtifact(artifactplayer.ActiveArtifact).ModifyDesc(Main.CurrentPlayer)}";
				artifactcustomtextpanel.SetText(line);
			}
			if (!sett) {
				CurrentState = 2;
				btn_Artifact.SetVisibility(1, 1);
				textpanel.SetText("");
			}
			else {
				btn_Artifact.SetVisibility(.7f, .6f);
			}
		}
		private void Btn_Stats_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
			foreach (var item in textlist.Keys) {
				item.Hide = true;
			}
			ArtifactPage_VisibilitySetting(true);

			btn_Stats.SetVisibility(1, 1);
			btn_ModStats.SetVisibility(.7f, .6f);
			btn_Perks.SetVisibility(.7f, .6f);
			CurrentState = 0;
			generalTextPanel.Hide = true;
		}
		private void Btn_ModStats_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
			foreach (var item in textlist.Keys) {
				item.Hide = true;
			}
			ArtifactPage_VisibilitySetting(true);

			btn_ModStats.SetVisibility(1, 1);
			btn_Stats.SetVisibility(.7f, .6f);
			btn_Perks.SetVisibility(.7f, .6f);
			CurrentState = 1;
			generalTextPanel.Hide = true;
		}
		private void Btn_Artifact_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
			foreach (var item in textlist.Keys) {
				item.Hide = true;
			}
			ArtifactPage_VisibilitySetting(false);

			btn_ModStats.SetVisibility(.7f, .6f);
			btn_Perks.SetVisibility(.7f, .6f);
			btn_Stats.SetVisibility(.7f, .6f);
			generalTextPanel.Hide = true;
		}

		private void Btn_Perks_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
			ArtifactPage_VisibilitySetting(true);

			foreach (var item in textlist.Keys) {
				item.Hide = false;
			}
			btn_Perks.SetVisibility(1, 1);
			btn_ModStats.SetVisibility(.7f, .6f);
			btn_Stats.SetVisibility(.7f, .6f);
			CurrentState = 3;
		}
		public override void OnActivate() {
			foreach (var item in textlist.Keys) {
				textpanel.RemoveChild(item);
			}
			textlist.Clear();
			var player = Main.LocalPlayer;
			var perkplayer = player.GetModPlayer<PerkPlayer>();
			int counter = 0;
			foreach (var perkType in perkplayer.perks.Keys) {
				if (ModPerkLoader.GetPerk(perkType) != null) {
					var text = new Roguelike_UIText(ModPerkLoader.GetPerk(perkType).DisplayName + $" | current stack : [{perkplayer.perks[perkType]}]");
					text.OnUpdate += Text_OnUpdate;
					text.Top.Pixels += 25 * counter;
					text.Hide = true;
					textpanel.Append(text);
					textlist.Add(text, perkType);
					counter++;
				}
			}
		}
		public override void Update(GameTime gameTime) {
			base.Update(gameTime);
			InfoShowToItem = string.Empty;
			if (panel.ContainsPoint(Main.MouseScreen)) {
				Main.LocalPlayer.mouseInterface = true;
			}
			if (textpanel.ContainsPoint(Main.MouseScreen)) {
				Main.LocalPlayer.mouseInterface = true;
			}
			var player = Main.LocalPlayer;
			string line;
			var statshandle = player.GetModPlayer<PlayerStatsHandle>();
			var enchantplayer = player.GetModPlayer<EnchantmentModplayer>();
			var augmentation = player.GetModPlayer<AugmentsPlayer>();
			foreach (var item in list_info) {
				item.Hide(true);
			}
			textpanel.SetText("");
			switch (CurrentState) {
				case 0:
					foreach (var item in list_info) {
						item.Hide(false);
						item.Update();
					}
					// 0 to 17 is default stats
					if (list_info.Count < 1) {
						int len = InfoSystem._listInfo.Count;
						int i = 0;
						foreach (var item in InfoSystem._listInfo) {
							list_info.Add(new Roguelike_Info(textpanel, item));
							float Y = MathHelper.Lerp(0, 1f, i / (len - 1f));
							list_info[i].SetAlign(0, Y);
							i++;
						}
					}
					break;
				case 1:
					var drugplayer = player.GetModPlayer<WonderDrugPlayer>();
					var nohitPlayer = player.GetModPlayer<NoHitPlayerHandle>();
					statshandle.GetAmount();
					line =
						$"Amount drop chest final weapon : {statshandle.weaponAmount}" +
						$"\nAmount drop chest final potion type : {statshandle.potionTypeAmount}" +
						$"\nAmount drop chest final potion amount : {statshandle.potionNumAmount}" +
						$"\nWonder drug consumed rate : {drugplayer.DrugDealer}" +
						$"\nAmount boss no-hit : {nohitPlayer.BossNoHitNumber.Count}" +
						$"\nRun amount : {RoguelikeData.Run_Amount}" +
						$"\nLootbox opened: {RoguelikeData.Lootbox_AmountOpen}" +
						$"\nAmount boss don't-hit : {nohitPlayer.DontHitBossNumber.Count}";
					textpanel.SetText(line);
					break;
				case 2:
					var artifactplayer = Main.LocalPlayer.GetModPlayer<ArtifactPlayer>();
					artifact_text.SetText(Artifact.GetArtifact(artifactplayer.ActiveArtifact).DisplayName);
					line = $"{Artifact.GetArtifact(artifactplayer.ActiveArtifact).ModifyDesc(player)}";
					artifactcustomtextpanel.SetText(line);
					break;
				case 3:
					foreach (var item in textlist.Keys) {
						item.Hide = false;
					}
					if (textlist.Keys.Where(e => !e.IsMouseHovering).Count() == textlist.Keys.Count) {
						generalTextPanel.Hide = true;
					}
					break;
				default:
					line = "";
					textpanel.SetText(line);
					break;
			}
		}
	}
}
public class InfoSystem : ModSystem {
	public static List<InfoType> _listInfo = new();
	public static int Register(InfoType info) {
		ModTypeLookup<InfoType>.Register(info);
		_listInfo.Add(info);
		return _listInfo.Count - 1;
	}
	private static int CompareUsingPriority(InfoType type1, InfoType type2) {
		return type1.Priority.CompareTo(type2.Priority);
	}
	public static InfoType GetInfo(int type) => type < 0 || type >= _listInfo.Count ? null : _listInfo[type];
	public override void PostSetupContent() {
		_listInfo.Sort(CompareUsingPriority);
	}
	public override void Load() {
		if (_listInfo == null) {
			_listInfo = new List<InfoType>();
		}
	}
	public override void Unload() {
		_listInfo = null;
	}
}
public abstract class InfoType : ModType {
	public int Type = 0;
	public int Priority = 0;
	public static int GetInfoType<T>() where T : InfoType => ModContent.GetInstance<T>().Type;
	protected override void Register() {
		Type = InfoSystem.Register(this);
		SetStaticDefaults();
	}
	public virtual string InfoText() => "";
}
