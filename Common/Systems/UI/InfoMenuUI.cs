using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Roguelike.Common.Systems.UI;
internal class InfoMenuUI : UIState {
	public Roguelike_UIPanel mainPanel;
	public Roguelike_UIPanel headerPanel;
	public InfoMenuPanelThatCanScroll textpanel;
	public Roguelike_UIPanel SectionPanel;
	public List<InfoMenuContent_TextPanel> list_contentPanel;
	public ExitUI exit;
	public const int Row = 15;
	public int OffSetScroll = 0;
	public override void OnInitialize() {
		mainPanel = new Roguelike_UIPanel();
		mainPanel.UISetWidthHeight(600, 600);
		mainPanel.HAlign = .5f;
		mainPanel.VAlign = .5f;
		mainPanel.MarginLeft = mainPanel.Width.Pixels * .5f;
		Append(mainPanel);

		headerPanel = new Roguelike_UIPanel();
		headerPanel.UISetWidthHeight(600, 80);
		mainPanel.Append(headerPanel);

		exit = new(ModContent.Request<Texture2D>(ModTexture.ACCESSORIESSLOT));
		exit.HAlign = 1f;
		exit.VAlign = 1f;
		exit.UISetWidthHeight(52, 52);
		headerPanel.Append(exit);

		textpanel = new InfoMenuPanelThatCanScroll();
		textpanel.UISetWidthHeight(600, 500);
		textpanel.HAlign = .5f;
		textpanel.VAlign = 1f;
		mainPanel.Append(textpanel);

		SectionPanel = new();
		SectionPanel.UISetWidthHeight(300, 600);
		SectionPanel.HAlign = .5f;
		SectionPanel.VAlign = .5f;
		SectionPanel.MarginRight = mainPanel.Width.Pixels + 10;
		SectionPanel.OnScrollWheel += SectionPanel_OnScrollWheel;
		Append(SectionPanel);

		list_contentPanel = new();
		for (int i = 0; i < ModInformation_System.listinformation.Count; i++) {
			var item = ModInformation_System.listinformation[i];
			InfoMenuContent_TextPanel txpanel = new();
			if (i < Row) {
				txpanel.SetInformation(ModInformation_System.listinformation[i]);
				txpanel.OnLeftClick += Txpanel_OnLeftClick;
				txpanel.HAlign = .5f;
				txpanel.Width.Percent = 1f;
				txpanel.VAlign = i / (Row - 1f);
				list_contentPanel.Add(txpanel);
				SectionPanel.Append(list_contentPanel[i]);
			}
		}
	}

	private void Txpanel_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		if (listeningElement is InfoMenuContent_TextPanel text) {
			if (string.IsNullOrEmpty(text.Text)) {
				return;
			}
			textpanel.InfoText = text.info.Localization;
		}
	}

	private void SectionPanel_OnScrollWheel(UIScrollWheelEvent evt, UIElement listeningElement) {
		if (!listeningElement.IsMouseHovering) {
			return;
		}
		OffSetScroll -= MathF.Sign(evt.ScrollWheelValue);
		OffSetScroll = Math.Clamp(OffSetScroll, 0, ModInformation_System.listinformation.Count - 1);
		for (int i = 0; i < list_contentPanel.Count; i++) {
			InfoMenuContent_TextPanel panel = list_contentPanel[i];
			int realIndex = OffSetScroll + i;
			panel.SetText("");
			if (realIndex > ModInformation_System.listinformation.Count - 1) {
				continue;
			}
			panel.SetInformation(ModInformation_System.listinformation[OffSetScroll + i]);
		}
	}
}
public class InfoMenuContent_TextPanel : Roguelike_UITextPanel {
	public InfoMenuContent_TextPanel(string text, float textScale = 1, bool large = false) : base(text, textScale, large) {
	}
	public InfoMenuContent_TextPanel() : base("", 1, false) {

	}
	public ModInformation info { get; private set; } = null;
	public void SetInformation(ModInformation information) {
		if (information != null) {
			info = information;
			SetText(information.LocalizationName);
		}
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		this.Disable_MouseItemUsesWhenHoverOverAUI();
	}
}
public class InfoMenuPanelThatCanScroll : Roguelike_UIPanel {
	public override void OnInitialize() {
		OverrideDefaultDraw = true;
	}
	public string InfoText = "";
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		this.Disable_MouseItemUsesWhenHoverOverAUI();
	}
	public override void PostDraw(SpriteBatch spriteBatch) {
		Info_Draw(spriteBatch);
	}
	private int linePosition;
	private int maxLinePosition;
	protected void Info_Draw(SpriteBatch spriteBatch) {
		string text = InfoText;

		DynamicSpriteFont font = FontAssets.MouseText.Value;
		float scale = 1f;
		string[] lines = Terraria.Utils.WordwrapString(
		   text,
			font,
			(int)(GetInnerDimensions().Width / scale),
			9999,
			out int lineCount
		).Where(line => line is not null).ToArray();

		maxLinePosition = Math.Max(lines.Length - 15, 0);
		linePosition = Math.Clamp(linePosition, 0, maxLinePosition);

		float yOffset = 0f;
		for (int i = linePosition; i < Math.Min(linePosition + 15, lines.Length); i++) {
			string text2 = lines[i];
			ChatManager.DrawColorCodedStringWithShadow(
				spriteBatch,
				font,
				text2,
				GetInnerDimensions().Position() + Vector2.UnitY * yOffset,
				Color.White,
				0f,
				Vector2.Zero,
				Vector2.One * scale
			);

			yOffset += scale * 25f;
		}
	}

	public override void ScrollWheel(UIScrollWheelEvent evt) {
		linePosition -= MathF.Sign(evt.ScrollWheelValue);
	}
}
public class ModInformation_System : ModSystem {
	public static List<ModInformation> listinformation { get; private set; } = new List<ModInformation>();
	public static int Register(ModInformation info) {
		ModTypeLookup<ModInformation>.Register(info);
		listinformation.Add(info);
		return listinformation.Count - 1;
	}
}
public abstract class ModInformation : ModType {
	public int Type = 0;
	protected override void Register() {
		Type = ModInformation_System.Register(this);
	}
	public virtual string LocalizationName => "";
	public string Localization => ModUtils.LocalizationText("Info", LocalizationName);
}
public class HallowedGaze_ModInfo : ModInformation {
	public override string LocalizationName => "HallowedGaze";
}
public class WrathofBlueMoon_ModInfo : ModInformation {
	public override string LocalizationName => "WrathofBlueMoon";
}
public class FuryOfTheSun_ModInfo : ModInformation {
	public override string LocalizationName => "FuryOfTheSun";
}
public class Avarice_ModInfo : ModInformation {
	public override string LocalizationName => "Avarice";
}
public class ElectricConductor_ModInfo : ModInformation {
	public override string LocalizationName => "ElectricConductor";
}
