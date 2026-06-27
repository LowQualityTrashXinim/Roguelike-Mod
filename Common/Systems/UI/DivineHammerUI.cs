using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Roguelike.Common.Utils;
using Roguelike.Contents.Transfixion.Augmentation;
using Roguelike.Contents.Transfixion.WeaponEnchantment;
using Roguelike.Texture;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace Roguelike.Common.Systems.UI;
public class DivineHammerUIState : UIState {
	UIPanel Mainpanel;
	Roguelike_UIPanel HeaderPanel;
	UIPanel BodyPanel;
	Roguelike_UIImage enchantment;
	Roguelike_UIImage augmentation;
	public WeaponEnchantmentUIslot weaponEnchantmentUIslot;
	ExitUI exit;

	public ItemHolderSlot AccAugmentSlot;
	public ItemHolderSlot AccSacrificeAugmentSlot;
	public ConfirmButton confirmButton;
	public ItemHolderSlot AccAugmentResult;

	public EnchantmentUIslot slot1, slot2, slot3;
	Asset<Texture2D> tex = TextureAssets.InventoryBack;
	public void GeneralInit() {
		Mainpanel = new UIPanel();
		Mainpanel.HAlign = .5f;
		Mainpanel.VAlign = .5f;
		Mainpanel.UISetWidthHeight(300, 250);
		Append(Mainpanel);

		HeaderPanel = new();
		HeaderPanel.Width.Percent = 1f;
		HeaderPanel.Height.Pixels = 80;
		HeaderPanel.BackgroundColor = Color.Black with { A = 0 };
		HeaderPanel.BorderColor = Color.Black with { A = 0 };
		Mainpanel.Append(HeaderPanel);

		BodyPanel = new();
		BodyPanel.Width.Percent = 1f;
		BodyPanel.Height.Pixels = 160;
		BodyPanel.VAlign = 1f;
		BodyPanel.HAlign = 0;
		Mainpanel.Append(BodyPanel);

		exit = new ExitUI(tex);
		exit.UISetWidthHeight(52, 52);
		exit.HAlign = 1f;
		HeaderPanel.Append(exit);
	}
	public void EnchantmentInit() {
		enchantment = new(tex);
		enchantment.SetPostTex(ModContent.Request<Texture2D>(ModUtils.GetTheSameTextureAs<DivineHammerUIState>("WeaponEnchantment")), true);
		enchantment.UISetWidthHeight(52, 52);
		enchantment.OnLeftClick += Universal_OnLeftClick;
		enchantment.HighlightColor = enchantment.Color.ScaleRGB(0.5f);
		enchantment.Highlight = true;
		HeaderPanel.Append(enchantment);

		weaponEnchantmentUIslot = new WeaponEnchantmentUIslot(tex);
		weaponEnchantmentUIslot.UISetWidthHeight(52, 52);
		BodyPanel.Append(weaponEnchantmentUIslot);

		slot1 = new EnchantmentUIslot(tex);
		slot1.HAlign = 0;
		slot1.VAlign = 1f;
		slot1.WhoAmI = 0;
		slot1.Hide = true;
		BodyPanel.Append(slot1);

		slot2 = new EnchantmentUIslot(tex);
		slot2.HAlign = .5f;
		slot2.VAlign = 1f;
		slot2.WhoAmI = 1;
		slot2.Hide = true;
		BodyPanel.Append(slot2);

		slot3 = new EnchantmentUIslot(tex);
		slot3.HAlign = 1;
		slot3.VAlign = 1f;
		slot3.WhoAmI = 2;
		slot3.Hide = true;
		BodyPanel.Append(slot3);
	}
	public void Visual_Enchantment(bool hide) {
		weaponEnchantmentUIslot.Hide = hide;
	}
	Roguelike_UIPanel AugmentationSelection_Container;
	Roguelike_UIPanel AugmentationSelection_Head;
	Roguelike_UIPanel AugmentationSelection_Body;
	public void AugmentationInit() {
		augmentation = new(tex);
		augmentation.SetPostTex(ModContent.Request<Texture2D>(ModUtils.GetTheSameTextureAs<DivineHammerUIState>("Augmentation")), true);
		augmentation.UISetWidthHeight(52, 52);
		augmentation.MarginLeft = enchantment.Width.Pixels + 10;
		augmentation.OnLeftClick += Universal_OnLeftClick;
		augmentation.HighlightColor = augmentation.Color.ScaleRGB(0.5f);
		HeaderPanel.Append(augmentation);

		AccAugmentSlot = new(tex);
		AccAugmentSlot.HAlign = 0;
		AccAugmentSlot.VAlign = 1;
		AccAugmentSlot.Hide = true;
		AccAugmentSlot.SetPostTex(TextureAssets.Item[ItemID.AvengerEmblem], attemptToLoad: true);
		AccAugmentSlot.drawInfo.Opacity = .3f;
		AccAugmentSlot.OnLeftClick += AccAugmentSlot_OnLeftClick;
		BodyPanel.Append(AccAugmentSlot);

		AccSacrificeAugmentSlot = new(tex);
		AccSacrificeAugmentSlot.HAlign = .33f;
		AccSacrificeAugmentSlot.VAlign = 1;
		AccSacrificeAugmentSlot.Hide = true;
		AccSacrificeAugmentSlot.OnLeftClick += AccAugmentSlot_OnLeftClick;
		BodyPanel.Append(AccSacrificeAugmentSlot);

		confirmButton = new(tex);
		confirmButton.HAlign = .66f;
		confirmButton.VAlign = 1;
		confirmButton.Hide = true;
		confirmButton.OnLeftClick += ConfirmButton_OnLeftClick;
		confirmButton.SetPostTex(ModContent.Request<Texture2D>(ModUtils.GetTheSameTextureAs<DivineHammerUIState>("Augmentation")), true);
		confirmButton.SetVisibility(.7f, 1f);
		BodyPanel.Append(confirmButton);

		AccAugmentResult = new(tex);
		AccAugmentResult.HAlign = 1f;
		AccAugmentResult.VAlign = 1;
		AccAugmentResult.Hide = true;
		AccAugmentResult.OnLeftClick += AccAugmentSlot_OnLeftClick;
		BodyPanel.Append(AccAugmentResult);

		Vector2 position = Mainpanel.GetOuterDimensions().Position();
		AugmentationSelection_Container = new();
		AugmentationSelection_Container.UISetWidthHeight(480, 450);
		AugmentationSelection_Container.Hide = true;
		AugmentationSelection_Container.HAlign = .5f;
		AugmentationSelection_Container.VAlign = .5f;
		AugmentationSelection_Container.BackgroundColor = AugmentationSelection_Container.BackgroundColor with { A = 200 };
		AugmentationSelection_Container.SetPadding(5);
		Append(AugmentationSelection_Container);

		AugmentationSelection_Head = new();
		AugmentationSelection_Head.Width.Percent = 1;
		AugmentationSelection_Head.Height.Pixels = 70;
		AugmentationSelection_Head.SetPadding(5);
		AugmentationSelection_Container.Append(AugmentationSelection_Head);

		AugmentationSelection_Body = new();
		AugmentationSelection_Body.Width.Percent = 1;
		AugmentationSelection_Body.Height.Pixels = AugmentationSelection_Container.Height.Pixels - 90;
		AugmentationSelection_Body.VAlign = 1f;
		AugmentationSelection_Container.Append(AugmentationSelection_Body);
	}
	private void ConfirmButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		if (AccAugmentSlot.item == null || AccAugmentSlot.item.type == ItemID.None) {
			return;
		}
		if (AccSacrificeAugmentSlot.item == null || AccSacrificeAugmentSlot.item.type == ItemID.None) {
			return;
		}
		if (AccAugmentResult.item != null && AccAugmentResult.item.type != ItemID.None) {
			return;
		}
		Item item = AccAugmentSlot.item;
		if (AccSacrificeAugmentSlot.item.ModItem is Augmentation aug) {
			if (AugmentsLoader.GetAugments(aug.Aug_Type) == null) {
				return;
			}
			if (item.GetGlobalItem<AugmentsWeapon>().Augment == aug.Type) {
				item.GetGlobalItem<AugmentsWeapon>().Modify_Charge(50);
			}
			else {
				AugmentsWeapon.AddAugments(ref item, aug.Aug_Type);
			}
			AccSacrificeAugmentSlot.item.TurnToAir();
			AccAugmentResult.item = item.Clone();
			AccAugmentSlot.item.TurnToAir();
		}
	}

	private void AccAugmentSlot_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		Player player = Main.LocalPlayer;
		if (listeningElement.UniqueId == AccAugmentSlot.UniqueId) {
			Item item = Main.mouseItem;
			if (!item.accessory && item.type != ItemID.None) {
				return;
			}
			ModUtils.SimpleItemMouseExchange(player, ref AccAugmentSlot.item);
			if (AccAugmentSlot.item.type == ItemID.None) {
				AccAugmentSlot.drawInfo.Hide = true;
			}
			else {
				if (Main.mouseItem.type == ItemID.None) {
					AccAugmentSlot.drawInfo.Hide = false;
				}
			}
		}
		else if (listeningElement.UniqueId == AccSacrificeAugmentSlot.UniqueId) {
			Item item = Main.mouseItem;
			if (item.type != ItemID.None && item.ModItem is Augmentation) {
				ModUtils.SimpleItemMouseExchange(player, ref AccSacrificeAugmentSlot.item);
			}
		}
		else if (listeningElement.UniqueId == AccAugmentResult.UniqueId) {
			Item item = Main.mouseItem;
			if (item.type != ItemID.None) {
				return;
			}
			if (AccAugmentResult.item.type == ItemID.None) {
				return;
			}
			Main.mouseItem = AccAugmentResult.item.Clone();
			player.inventory[58] = AccAugmentResult.item.Clone();
			AccAugmentResult.item.TurnToAir();
		}
	}

	public void Visual_Augmentation(bool hide) {
		AccAugmentSlot.Hide = hide;
		AccSacrificeAugmentSlot.Hide = hide;
		confirmButton.Hide = hide;
		AccAugmentResult.Hide = hide;
		AugmentationSelection_Container.Hide = true;
		AugmentationSelection_Head.Hide = true;
		AugmentationSelection_Body.Hide = true;
	}
	public void Visual_AugmentationSelection() {
		AugmentationSelection_Container.Hide = false;
		AugmentationSelection_Head.Hide = false;
		AugmentationSelection_Body.Hide = false;
	}
	public override void OnInitialize() {
		GeneralInit();

		EnchantmentInit();

		AugmentationInit();
	}
	private void Universal_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		enchantment.Highlight = false;
		augmentation.Highlight = false;
		Visual_Augmentation(true);
		Visual_Enchantment(true);
		if (listeningElement.UniqueId == enchantment.UniqueId) {
			enchantment.Highlight = true;
			Visual_Enchantment(false);
		}
		else if (listeningElement.UniqueId == augmentation.UniqueId) {
			augmentation.Highlight = true;
			Visual_Augmentation(false);
		}
	}

	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if (ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
		if (enchantment.IsMouseHovering) {
			Main.instance.MouseText("Weapon Enchantment");
		}
	}
	public override void OnDeactivate() {
		slot3.Hide = true;
		slot2.Hide = true;
		slot1.Hide = true;
	}
}
public class WeaponEnchantmentUIslot : Roguelike_UIImage {
	public int WhoAmI = -1;
	public Texture2D textureDraw;
	public Item item;
	private Texture2D texture;
	public WeaponEnchantmentUIslot(Asset<Texture2D> texture) : base(texture) {
		this.texture = texture.Value;
	}
	public override void LeftClick(UIMouseEvent evt) {
		Player player = Main.LocalPlayer;
		//checking whenever or not if the mouse item is a actual weapon
		if (Main.mouseItem.type != ItemID.None) {
			if (!Main.mouseItem.IsAWeapon())
				return;
			Item itemcached;
			//Checking whenever or not the slot is empty or not
			if (item != null && item.type != ItemID.None) {
				//The slot is not empty and the the mouse have a item, we exchange item
				itemcached = item.Clone();
				item.TurnToAir();
				item = Main.mouseItem.Clone();
				Main.mouseItem = itemcached;
				player.inventory[58] = itemcached;
			}
			else {
				//The slot is empty, we clone the item into the slot
				item = Main.mouseItem.Clone();
				Main.mouseItem.TurnToAir();
				player.inventory[58].TurnToAir();
				UniversalSystem.EnchantingState = true;
			}
			//we are setting up the slot
			if (item.TryGetGlobalItem(out EnchantmentGlobalItem globalItem)) {
				DivineHammerUIState state = ModContent.GetInstance<UniversalSystem>().DivineHammer_uiState;
				state.slot1.WhoAmI = 0;
				state.slot1.text.SetText("1");
				state.slot1.itemOwner = item;
				state.slot1.itemType = globalItem.EnchantmenStlot[0];
				state.slot1.Hide = false;

				state.slot2.WhoAmI = 1;
				state.slot2.text.SetText("2");
				state.slot2.itemOwner = item;
				state.slot2.itemType = globalItem.EnchantmenStlot[1];
				state.slot2.Hide = false;

				state.slot3.WhoAmI = 2;
				state.slot3.text.SetText("3");
				state.slot3.itemOwner = item;
				state.slot3.itemType = globalItem.EnchantmenStlot[2];
				state.slot3.Hide = false;
			}
		}
		//In this case, it mean that the mouse item is empty
		else {
			//Checking whenver or not if the item holder is empty or not, if it is then do return and do nothing
			if (item == null)
				return;
			//It seem that it is not empty, we disable general stuff
			UniversalSystem.EnchantingState = false;
			Main.mouseItem = item;
			item = null;
			DivineHammerUIState state = ModContent.GetInstance<UniversalSystem>().DivineHammer_uiState;
			state.slot1.Hide = true;
			state.slot2.Hide = true;
			state.slot3.Hide = true;
		}
	}
	public void DropItem(Player player) {
		if (item == null)
			return;
		for (int i = 0; i < 50; i++) {
			if (player.CanItemSlotAccept(player.inventory[i], item)) {
				if (ModContent.GetInstance<UniversalSystem>().WorldState == "Exited") {
					ModContent.GetInstance<UniversalSystem>().IsAttemptingToBringItemToNewPlayer = true;
					return;
				}
				player.inventory[i] = item.Clone();
				item = null;
				return;
			}
		}
		player.TryDroppingSingleItem(player.GetSource_DropAsItem(), item);
		item = null;
	}
	public override void OnDeactivate() {
		Player player = Main.LocalPlayer;
		UniversalSystem.EnchantingState = false;
		DropItem(player);
	}
	public override void DrawImage(SpriteBatch spriteBatch) {
		Vector2 drawpos = GetInnerDimensions().Position() * Main.UIScale + texture.Size() * .5f;
		if (item != null) {
			if (IsMouseHovering) {
				UniversalSystem.EnchantingState = false;
				Main.HoverItem = item.Clone();
				Main.hoverItemName = item.HoverName;
			}
			else {
				UniversalSystem.EnchantingState = true;
			}
			Main.instance.LoadItem(item.type);
			Texture2D texture = TextureAssets.Item[item.type].Value;
			Vector2 origin = texture.Size() * .5f;
			float scaling = 1;
			if (texture.Width > this.texture.Width || texture.Height > this.texture.Height) {
				scaling = ScaleCalculation(texture.Size()) * .68f;
			}
			spriteBatch.Draw(texture, drawpos, null, Color.White, 0, origin, scaling, SpriteEffects.None, 0);
		}
		else {
			Texture2D backgroundtexture = TextureAssets.Item[ItemID.SilverBroadsword].Value;
			spriteBatch.Draw(backgroundtexture, drawpos, null, new Color(0, 0, 0, 80), 0, texture.Size() * .35f, ScaleCalculation(backgroundtexture.Size()) * .78f, SpriteEffects.None, 0);
		}
	}
	private float ScaleCalculation(Vector2 textureSize) => texture.Size().Length() / textureSize.Length();
}
public class ConfirmButton : Roguelike_UIImageButton {
	public ConfirmButton(Asset<Texture2D> texture) : base(texture) {
	}
}
public class EnchantmentUIslot : Roguelike_UIImage {
	public int itemType = 0;
	public int WhoAmI = -1;

	public Item itemOwner = null;
	private Texture2D texture;
	public UIText text;
	public EnchantmentUIslot(Asset<Texture2D> texture) : base(texture) {
		this.texture = texture.Value;
		text = new((WhoAmI + 1).ToString());
		text.HAlign = .5f;
		text.VAlign = .5f;
		Append(text);
	}
	public override void LeftClick(UIMouseEvent evt) {
		if (Main.LocalPlayer.GetModPlayer<EnchantmentModplayer>().SlotUnlock < WhoAmI) {
			return;
		}
		if (itemOwner == null)
			return;
		if (Main.mouseItem.type != ItemID.None) {
			if (Main.mouseItem.consumable)
				return;
			if (itemType != 0)
				return;
			if (EnchantmentLoader.GetEnchantmentItemID(Main.mouseItem.type) == null)
				return;
			itemType = Main.mouseItem.type;
			Main.mouseItem.TurnToAir();
			Main.LocalPlayer.inventory[58].TurnToAir();
			EnchantmentSystem.EnchantItem(ref itemOwner, WhoAmI, itemType);
		}
	}
	public override void DrawImage(SpriteBatch spriteBatch) {
		try {
			if (Main.LocalPlayer.GetModPlayer<EnchantmentModplayer>().SlotUnlock < WhoAmI) {
				Texture2D lockTexture = ModContent.Request<Texture2D>(ModTexture.Lock).Value;
				Vector2 origin = lockTexture.Size() * .5f;
				Vector2 drawpos = GetInnerDimensions().Position() * Main.UIScale + texture.Size() * .5f;
				spriteBatch.Draw(lockTexture, drawpos, null, Color.White, 0, origin, .87f, SpriteEffects.None, 0);
				return;
			}
			if (itemOwner == null)
				return;
			if (itemType != 0) {
				Vector2 drawpos = GetInnerDimensions().Position() * Main.UIScale + texture.Size() * .5f;
				Main.instance.LoadItem(itemType);
				Texture2D texture1 = TextureAssets.Item[itemType].Value;
				Vector2 origin = texture1.Size() * .5f;
				spriteBatch.Draw(texture1, drawpos, null, Color.White, 0, origin, .87f, SpriteEffects.None, 0);
				if (IsMouseHovering) {
					string tooltipText = "No enchantment can be found";
					if (EnchantmentLoader.GetEnchantmentItemID(itemType) != null) {
						tooltipText = EnchantmentLoader.GetEnchantmentItemID(itemType).Description;
					}
					Item fakeItem = new Item(itemType, 1, 0);
					fakeItem.SetNameOverride(tooltipText);
					Main.HoverItem = fakeItem;
					Main.hoverItemName = tooltipText;
					Main.instance.MouseText("");
					Main.mouseText = true;
				}
			}
		}
		catch (Exception ex) {
			Main.NewText(ex.Message);
		}
	}
	public override void UpdateImage(GameTime gameTime) {
		if (itemType == ItemID.None)
			return;
		text.SetText("");
	}
}
