using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Roguelike.Common.Systems;
using Roguelike.Common.Systems.ArtifactSystem;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.RelicItem;
using Roguelike.Contents.Transfixion.Perks;
using Roguelike.Texture;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace Roguelike.Contents.Items.aDebugItem.DebugStick.DebugSystemUI;
internal class PerkDebugUI : UIState {
	public Roguelike_UIPanel panel_Main;
	public Roguelike_UIPanel panel_Head;
	public Roguelike_UIPanel panel_Body;
	public Roguelike_UIImageButton btn_ResetPerk;
	public Roguelike_UIImageButton btn_ResetPerkSelection;
	public Roguelike_UIImageButton btn_ConfirmSelectino;
	public ExitUI exit;

	public List<PerkDebugBtn> list_perk = new();
	public List<int> list_perkSelection = new();
	public override void OnInitialize() {
		list_perk = new();
		list_perkSelection = new();

		panel_Main = new();
		panel_Main.UISetWidthHeight(600, 600);
		panel_Main.HAlign = .5f;
		panel_Main.VAlign = .5f;
		Append(panel_Main);

		panel_Head = new();
		panel_Head.Width.Percent = 1f;
		panel_Head.Height.Pixels = 80;
		panel_Main.Append(panel_Head);

		panel_Body = new();
		panel_Body.Width.Percent = 1f;
		panel_Body.Height.Pixels = 500;
		panel_Body.VAlign = 1f;
		panel_Main.Append(panel_Body);

		btn_ResetPerk = new(TextureAssets.InventoryBack7);
		btn_ResetPerk.HoverText = "Reset perk";
		btn_ResetPerk.SetVisibility(.6f, 1f);
		btn_ResetPerk.UISetWidthHeight(52, 52);
		btn_ResetPerk.OnLeftClick += Btn_ResetPerk_OnLeftClick;
		panel_Head.Append(btn_ResetPerk);

		btn_ResetPerkSelection = new(TextureAssets.InventoryBack7);
		btn_ResetPerkSelection.HoverText = "Reset perk selection";
		btn_ResetPerkSelection.UISetWidthHeight(52, 52);
		btn_ResetPerkSelection.SetVisibility(.6f, 1f);
		btn_ResetPerkSelection.MarginLeft += 52 + 10;
		btn_ResetPerkSelection.OnLeftClick += Btn_ResetPerkSelection_OnLeftClick;
		panel_Head.Append(btn_ResetPerkSelection);

		btn_ConfirmSelectino = new(TextureAssets.InventoryBack7);
		btn_ConfirmSelectino.HoverText = "Confirm perk selection";
		btn_ConfirmSelectino.SetVisibility(.6f, 1f);
		btn_ConfirmSelectino.UISetWidthHeight(52, 52);
		btn_ConfirmSelectino.MarginLeft += (52 + 10) * 2;
		btn_ConfirmSelectino.OnLeftClick += Btn_ConfirmSelectino_OnLeftClick;
		panel_Head.Append(btn_ConfirmSelectino);

		int row = 6;
		int col = 6;
		for (int i = 0; i < row; i++) {
			float Yalign = MathHelper.Lerp(0, 1f, i / (float)(row - 1));
			for (int l = 0; l < col; l++) {
				float Xalign = MathHelper.Lerp(0, 1f, l / (float)(col - 1));
				PerkDebugBtn btn = new(TextureAssets.InventoryBack);
				int type = i * row + l;
				if (type >= ModPerkLoader.TotalCount) {
					continue;
				}
				btn.ChangePerkType(type);
				btn.HAlign = Xalign;
				btn.VAlign = Yalign;
				btn.SetVisibility(.4f, .4f);
				btn.OnLeftClick += Btn_OnLeftClick;
				list_perk.Add(btn);
				panel_Body.Append(btn);
			}
		}

		exit = new(TextureAssets.InventoryBack);
		exit.UISetWidthHeight(52, 52);
		exit.HAlign = 1f;
		panel_Head.Append(exit);
	}
	int currentStarterIndex = 0;
	public override void ScrollWheel(UIScrollWheelEvent evt) {
		currentStarterIndex -= MathF.Sign(evt.ScrollWheelValue);
		currentStarterIndex = Math.Clamp(currentStarterIndex, 0, (int)Math.Ceiling(ModPerkLoader.TotalCount / 6f) - 6);
		int offsett = currentStarterIndex * 6;
		for (int i = 0; i < list_perk.Count; i++) {
			int arty = offsett + i;
			list_perk[i].ChangePerkType(-1);
			if (arty >= ModPerkLoader.TotalCount) {
				continue;
			}
			list_perk[i].ChangePerkType(arty);
		}
	}
	private void Btn_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		foreach (var item in list_perk) {
			if (item.UniqueId == listeningElement.UniqueId) {
				SoundEngine.PlaySound(SoundID.Item35 with { Pitch = -1 });
				if (list_perkSelection.Contains(item.perkType)) {
					item.Info = "";
					list_perkSelection.Remove(item.perkType);
				}
				else {
					item.SetVisibility(1f, 1f);
					item.Info = "selected";
					list_perkSelection.Add(item.perkType);
				}
				break;
			}
		}
	}

	private void Btn_ConfirmSelectino_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		PerkPlayer perkplayer = Main.LocalPlayer.GetModPlayer<PerkPlayer>();
		for (int i = 0; i < list_perkSelection.Count; i++) {
			int perkType = list_perkSelection[i];
			Perk perk = ModPerkLoader.GetPerk(perkType);
			if (perk != null) {
				if (perk.StackLimit == -1 && perk.CanBeStack) {
					perk.OnChoose(perkplayer.Player);
					continue;
				}
			}
			if (perkplayer.perks.Count < 0 || !perkplayer.perks.ContainsKey(perkType))
				perkplayer.perks.Add(perkType, 1);
			else
				if (perkplayer.perks.ContainsKey(perkType) && perk.CanBeStack)
				perkplayer.perks[perkType]++;
			perk.OnChoose(perkplayer.Player);
		}
		list_perkSelection.Clear();
		foreach (var item in list_perk) {
			item.Info = "";
		}
	}
	private void Btn_ResetPerkSelection_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		list_perkSelection.Clear();
		foreach (var item in list_perk) {
			item.Info = "";
		}
	}
	private void Btn_ResetPerk_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		Main.LocalPlayer.GetModPlayer<PerkPlayer>().perks.Clear();
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if (btn_ConfirmSelectino.IsMouseHovering) {
			string text = "Confirm perk selection";
			foreach (var item in list_perkSelection) {
				Perk perk = ModPerkLoader.GetPerk(item);
				if (perk == null) {
					continue;
				}
				text += "\n - " + perk.DisplayName;
			}
			btn_ConfirmSelectino.HoverText = text;
		}
	}
}
class PerkDebugBtn : Roguelike_UIImageButton {
	public int perkType;
	public string Info = "";
	private Asset<Texture2D> texture;
	private Asset<Texture2D> ahhlookingassdefaultbgsperktexture = ModContent.Request<Texture2D>(ModTexture.ACCESSORIESSLOT);
	public PerkDebugBtn(Asset<Texture2D> texture) : base(ModContent.Request<Texture2D>(ModTexture.ACCESSORIESSLOT)) {
		this.texture = texture;
	}
	public void ChangePerkType(int type) {
		perkType = type;
		Perk perk = ModPerkLoader.GetPerk(perkType);
		if (perk != null && perk.textureString != null) {
			texture = ModContent.Request<Texture2D>(ModPerkLoader.GetPerk(perkType).textureString);
		}
		else {
			texture = ModContent.Request<Texture2D>(ModTexture.ACCESSORIESSLOT);
		}
		SetImage(texture);
		this.UISetWidthHeight(52, 52);
	}
	public override void UpdateOuter(GameTime gametime) {
		if (ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
	}
	public override void DrawImage(SpriteBatch spriteBatch) {
		base.DrawImage(spriteBatch);
		if (IsMouseHovering && ModPerkLoader.GetPerk(perkType) != null) {
			UICommon.TooltipMouseText(ModPerkLoader.GetPerk(perkType).DisplayName + "\n" + ModPerkLoader.GetPerk(perkType).ModifyToolTip());
		}
		Color color = Color.White;
		if (Info == "selected") {
			float size = 1.2f;
			Vector2 origin = ahhlookingassdefaultbgsperktexture.Value.Size() * .5f;
			Vector2 adjustment = origin - origin * size;
			spriteBatch.Draw(ahhlookingassdefaultbgsperktexture.Value, GetInnerDimensions().Position() + adjustment, null, color * .9f, 0, Vector2.Zero, size, SpriteEffects.None, 0f);
		}
		spriteBatch.Draw(ahhlookingassdefaultbgsperktexture.Value, GetInnerDimensions().Position(), null, color, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
		Vector2 size1 = texture.Size();
		Vector2 size2 = ahhlookingassdefaultbgsperktexture.Size();
		if (size1.X <= size2.X && size1.Y <= size2.Y) {
			spriteBatch.Draw(texture.Value, GetInnerDimensions().Position() + ahhlookingassdefaultbgsperktexture.Size() * .5f, null, Color.White, 0, texture.Size() * .5f, 1f, SpriteEffects.None, 0);
		}
		else {
			spriteBatch.Draw(texture.Value, GetInnerDimensions().Position() + ahhlookingassdefaultbgsperktexture.Size() * .5f, null, Color.White, 0, texture.Size() * .5f, ModUtils.Scale_OuterTextureWithInnerTexture(size1, size2, .8f), SpriteEffects.None, 0);
		}
	}
}
