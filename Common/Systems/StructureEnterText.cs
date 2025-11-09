using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.UI;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria.ModLoader.UI;
using Roguelike.Contents.Transfixion.Artifacts;

namespace Roguelike.Common.Systems;
/// <summary>
/// This is a custom text box, this text box allow user to input text into them.
/// </summary>
public class Roguelike_TextBox : UITextBox {
	public Roguelike_TextBox(string text, float textScale = 1, bool large = false) : base(text, textScale, large) {
	}
	public bool Hide = false;
	public bool focus = false;
	public bool mousePressed = false;
	public string OnHoverText = "";
	public override void LeftClick(UIMouseEvent evt) {
	}
	public override void LeftMouseUp(UIMouseEvent evt) {
		mousePressed = false;
	}
	public override void LeftMouseDown(UIMouseEvent evt) {
		focus = true;
		mousePressed = true;
	}
	public override void OnDeactivate() {
		focus = false;
		Main.blockInput = false;
		PlayerInput.WritingText = false;
		SetText("");
	}
	public override void Update(GameTime gameTime) {
		ShowInputTicker = focus;
		if (IgnoresMouseInteraction || Hide || !focus)
			return;
		SetTextMaxLength(999);
		if (ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}

		if (Main.mouseLeft && !IsMouseHovering || Main.inputText.IsKeyDown(Keys.Escape))
			focus = false;

		Main.blockInput = focus;
	}
	public bool focused = false;
	// must be inside this drawself method for it to write text like this...
	protected override void DrawSelf(SpriteBatch spriteBatch) {
		if (Hide) {
			return;
		}
		if (focus) {
			PlayerInput.WritingText = focus;
			string text = Main.GetInputText(Text);
			if (!Text.Equals(text)) {
				SetText(text);
			}
		}

		_color = focus ? Color.Yellow : Color.White;
		base.DrawSelf(spriteBatch);
		if (!string.IsNullOrEmpty(OnHoverText)) {
			if (IsMouseHovering) {
				UICommon.TooltipMouseText(OnHoverText);
			}
		}
	}
}
