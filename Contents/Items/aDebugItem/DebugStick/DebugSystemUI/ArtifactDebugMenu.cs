using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Roguelike.Common.Systems.ArtifactSystem;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.Toggle;
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

namespace Roguelike.Contents.Items.aDebugItem.DebugStick.DebugSystemUI;
internal class ArtifactDebugMenu : UIState {
	public Roguelike_UIPanel panel_MainPanel;
	public Roguelike_UIPanel panel_artifactselection;
	public Roguelike_UIImage panel_MainArtifactPanel;
	public Roguelike_UIPanel panel_ArtifactHeader;
	public Roguelike_UIPanel panel_ArtifactBody;
	public Info_ArtifactImageV2 img_ArtifactIcon;
	public Roguelike_UIText text_ArtifactName;
	public Roguelike_WrapTextUIPanel textpanel_ArtifactDesc;
	public Btn_Artifact[] arr_artifactbtn = new Btn_Artifact[10];
	ExitUI exit;
	public override void OnInitialize() {
		panel_MainPanel = new();
		panel_MainPanel.UISetWidthHeight(600, 600);
		panel_MainPanel.HAlign = .5f;
		panel_MainPanel.VAlign = .5f;
		panel_MainPanel.BackgroundColor = new Color(0, 0, 0, 0);
		panel_MainPanel.BorderColor = new Color(0, 0, 0, 0);
		Append(panel_MainPanel);

		panel_artifactselection = new();
		panel_artifactselection.Width.Pixels = 80;
		panel_artifactselection.Height.Percent = 1f;
		panel_MainPanel.Append(panel_artifactselection);

		arr_artifactbtn = new Btn_Artifact[10];
		int totalArtifact = Artifact.ArtifactCount;
		for (int i = 0; i < arr_artifactbtn.Length; i++) {
			arr_artifactbtn[i] = new(TextureAssets.InventoryBack);
			arr_artifactbtn[i].HAlign = .5f;
			arr_artifactbtn[i].VAlign = MathHelper.Lerp(0, 1f, i / 9f);
			if (i >= totalArtifact) {
				continue;
			}
			Artifact artifact = Artifact.GetArtifact(i);
			if (artifact == null) {
				continue;
			}
			arr_artifactbtn[i].SetArtifactType(i);
			arr_artifactbtn[i].OnLeftClick += ArtifactDebugMenu_OnLeftClick;
			panel_artifactselection.Append(arr_artifactbtn[i]);
		}

		panel_ArtifactBody = new();
		panel_ArtifactBody.Width.Pixels = 490;
		panel_ArtifactBody.Height.Percent = 1f;
		panel_ArtifactBody.HAlign = 1f;
		panel_MainPanel.Append(panel_ArtifactBody);

		panel_ArtifactHeader = new();
		panel_ArtifactHeader.Width.Percent = 1f;
		panel_ArtifactHeader.Height.Pixels = 110;
		panel_ArtifactBody.Append(panel_ArtifactHeader);

		img_ArtifactIcon = new Info_ArtifactImageV2(TextureAssets.InventoryBack);
		panel_ArtifactHeader.Append(img_ArtifactIcon);

		text_ArtifactName = new("");
		text_ArtifactName.VAlign = 1f;
		panel_ArtifactHeader.Append(text_ArtifactName);

		textpanel_ArtifactDesc = new Roguelike_WrapTextUIPanel("", .8f);
		textpanel_ArtifactDesc.VAlign = 1;
		textpanel_ArtifactDesc.Width.Percent = 1;
		textpanel_ArtifactDesc.Height.Precent = 1;
		textpanel_ArtifactDesc.Height.Pixels -= panel_ArtifactHeader.Height.Pixels + 10;
		textpanel_ArtifactDesc.DrawPanel = false;
		panel_ArtifactBody.Append(textpanel_ArtifactDesc);

		exit = new(TextureAssets.InventoryBack);
		exit.UISetWidthHeight(52, 52);
		exit.HAlign = 1f;
		panel_ArtifactHeader.Append(exit);
	}
	int currentStarterIndex = 0;
	public override void ScrollWheel(UIScrollWheelEvent evt) {
		currentStarterIndex -= MathF.Sign(evt.ScrollWheelValue);
		currentStarterIndex = Math.Clamp(currentStarterIndex, 0, Artifact.ArtifactCount - 10);
		for (int i = 0; i < arr_artifactbtn.Length; i++) {
			int arty = currentStarterIndex + i;
			arr_artifactbtn[i].SetArtifactType(-1);
			if (arty >= Artifact.ArtifactCount) {
				continue;
			}
			arr_artifactbtn[i].SetArtifactType(arty);
		}
	}
	private void ArtifactDebugMenu_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		for (int i = 0; i < arr_artifactbtn.Length; i++) {
			if (arr_artifactbtn[i].UniqueId == listeningElement.UniqueId) {
				Artifact artifact = Artifact.GetArtifact(arr_artifactbtn[i].artifactType);
				if (artifact == null) {
					return;
				}
				Main.NewText($"You have just changed to [c/{artifact.DisplayNameColor.Hex3()}:{artifact.DisplayName}]");
				img_ArtifactIcon.SetArtifactType(artifact.Type);
				SetArtifactInfo(Main.LocalPlayer, artifact.Type);
				//Main.LocalPlayer.GetModPlayer<ArtifactPlayer>().ActiveArtifact = artifact.Type;
				break;
			}
		}
	}

	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
	}
	public void SetArtifactInfo(Player player, int type) {
		string line = $"{Artifact.GetArtifact(type).ModifyDesc(player)}";
		text_ArtifactName.SetText(Artifact.GetArtifact(type).DisplayName);
		textpanel_ArtifactDesc.SetText(line);
	}
}
class Btn_Artifact : Roguelike_UIImageButton {
	public Btn_Artifact(Asset<Texture2D> texture) : base(texture) {
	}
	public int artifactType = 0;
	public void SetArtifactType(int type) {
		artifactType = type;
	}
	public override void UpdateOuter(GameTime gametime) {
		if (Main.LocalPlayer.GetModPlayer<ArtifactPlayer>().ActiveArtifact == artifactType) {
			SetVisibility(1f, .9f);
		}
		else {
			SetVisibility(1f, .4f);
		}
	}
	public override void DrawImage(SpriteBatch spriteBatch) {
		Artifact artifact = Artifact.GetArtifact(artifactType);
		if (artifact == null) {
			return;
		}
		CalculatedStyle style = GetInnerDimensions();
		artifact.DrawInUI(spriteBatch, style);
	}
}

class Info_ArtifactImageV2 : Roguelike_UIImage {
	public Info_ArtifactImageV2(Asset<Texture2D> texture) : base(texture) {
	}
	public int artifactType = 0;
	public void SetArtifactType(int type) {
		artifactType = type;
	}
	public override void DrawImage(SpriteBatch spriteBatch) {
		Artifact artifact = Artifact.GetArtifact(artifactType);
		if (artifact == null) {
			return;
		}
		CalculatedStyle style = GetInnerDimensions();
		artifact.DrawInUI(spriteBatch, style);
	}
}
