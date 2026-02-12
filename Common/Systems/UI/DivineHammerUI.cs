using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Roguelike.Common.Utils;
using Roguelike.Contents.Transfixion.Arguments;
using Roguelike.Contents.Transfixion.WeaponEnchantment;
using Roguelike.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
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
		HeaderPanel.BackgroundColor = Color.Black;
		HeaderPanel.BorderColor = Color.Black;
		HeaderPanel.BackgroundColor.A = 0;
		HeaderPanel.BorderColor.A = 0;
		Mainpanel.Append(HeaderPanel);

		BodyPanel = new();
		BodyPanel.Width.Percent = 1f;
		BodyPanel.Height.Pixels = Mainpanel.Height.Pixels - HeaderPanel.Height.Pixels - 30;
		BodyPanel.VAlign = 1f;
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
	Roguelike_UITextPanel AugmentationSelection_Text;
	Roguelike_UIPanel AugmentationSelection_Container;
	Roguelike_UIPanel AugmentationSelection_Head;
	Roguelike_UIPanel AugmentationSelection_Body;
	Roguelike_UIImageButton AugmentationSelection_BackIcon;
	Roguelike_UIImageButton AugmentationSelection_Forward;
	Roguelike_UIImageButton AugmentationSelection_Backward;
	public void AugmentationInit() {
		textlist.Clear();
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

		AugmentationSelection_Text = new("click on this panel to select", .8f);
		AugmentationSelection_Text.Width.Percent = 1f;
		AugmentationSelection_Text.Height.Pixels = 30;
		AugmentationSelection_Text.TextHAlign = .5f;
		AugmentationSelection_Text.Hide = true;
		AugmentationSelection_Text.OnLeftClick += AugmentationSelection_Text_OnLeftClick;
		BodyPanel.Append(AugmentationSelection_Text);

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

		auglist.Clear();
		auglist.AddRange(AugmentsLoader.ReturnListOfAugment());
		float num = 4;
		for (int x = 0; x < num; x++) {
			for (int y = 0; y < num + 2; y++) {
				int counter = (int)(x * (num + 2)) + y + 1;
				ModAugments aug = auglist[counter];
				string augText = "";
				Color color = Color.White;
				if (aug != null) {
					augText = aug.DisplayName;
					color = aug.tooltipColor;
				}
				AugmentationText btn = new(aug.Type, augText, .7f);
				btn.BorderColor = color;
				btn.BackgroundColor = btn.BackgroundColor with { A = 255 };
				btn.VAlign = MathHelper.Lerp(0f, 1f, y / (num + 1));
				btn.HAlign = MathHelper.Lerp(0f, 1f, x / (num - 1f));
				btn.UISetWidthHeight(110, 10);
				btn.OnLeftClick += Btn_OnLeftClick;
				btn.OnUpdate += Btn_OnUpdate;
				btn.TextHAlign = .5f;
				btn.PaddingBottom = 15;
				btn.PaddingTop = 15;
				AugmentationSelection_Body.Append(btn);
				textlist.Add(btn);
			}
		}
		maxPage = AugmentsLoader.TotalCount / 24;
		if (AugmentsLoader.TotalCount % 24 != 0) {
			maxPage++;
		}

		AugmentationSelection_BackIcon = new(ModContent.Request<Texture2D>(ModTexture.ACCESSORIESSLOT));
		AugmentationSelection_BackIcon.SetPostTex(ModContent.Request<Texture2D>(ModTexture.BackIcon));
		AugmentationSelection_BackIcon.UISetWidthHeight(52, 52);
		AugmentationSelection_BackIcon.HAlign = 1f;
		AugmentationSelection_BackIcon.VAlign = .5f;
		AugmentationSelection_BackIcon.OnLeftClick += AugmentationSelection_OnLeftClick;
		AugmentationSelection_Head.Append(AugmentationSelection_BackIcon);

		AugmentationSelection_Backward = new(ModContent.Request<Texture2D>(ModTexture.ACCESSORIESSLOT));
		AugmentationSelection_Backward.SetPostTex(ModContent.Request<Texture2D>(ModTexture.Arrow_Left));
		AugmentationSelection_Backward.UISetWidthHeight(52, 52);
		AugmentationSelection_Backward.VAlign = .5f;
		AugmentationSelection_Backward.OnLeftClick += AugmentationSelection_OnLeftClick;
		AugmentationSelection_Head.Append(AugmentationSelection_Backward);

		AugmentationSelection_Forward = new(ModContent.Request<Texture2D>(ModTexture.ACCESSORIESSLOT));
		AugmentationSelection_Forward.SetPostTex(ModContent.Request<Texture2D>(ModTexture.Arrow_Right));
		AugmentationSelection_Forward.UISetWidthHeight(52, 52);
		AugmentationSelection_Forward.VAlign = .5f;
		AugmentationSelection_Forward.MarginLeft = AugmentationSelection_Backward.Width.Pixels + 10;
		AugmentationSelection_Forward.OnLeftClick += AugmentationSelection_OnLeftClick;
		AugmentationSelection_Head.Append(AugmentationSelection_Forward);
	}

	private void AugmentationSelection_Text_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		Visual_AugmentationSelection();
	}

	List<ModAugments> auglist = new();
	int maxPage = 0;
	int currentPage = 0;
	private void AugmentationSelection_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		if (listeningElement.UniqueId == AugmentationSelection_BackIcon.UniqueId) {
			Visual_Augmentation(false);
		}
		else if (listeningElement.UniqueId == AugmentationSelection_Forward.UniqueId) {
			currentPage++;
			Reflesh_VisibleAugList();
		}
		else if (listeningElement.UniqueId == AugmentationSelection_Backward.UniqueId) {
			currentPage--;
			Reflesh_VisibleAugList();
		}
		currentPage = Math.Clamp(currentPage, 0, maxPage - 1);
	}
	private void Reflesh_VisibleAugList() {
		if (currentPage >= maxPage || currentPage < 0 || maxPage <= 1) {
			return;
		}
		int startingPoint = 24 * currentPage;
		for (int i = 0; i < textlist.Count; i++) {
			AugmentationText btn = textlist[i];
			int indexChecker = startingPoint + i + 1;
			if (textlist.Count - 1 < i || indexChecker >= auglist.Count) {
				btn.BorderColor = Color.White;
				btn.SetAug(0);
				btn.SetText(" ");
				continue;
			}
			ModAugments aug = auglist[indexChecker];
			btn.SetAug(aug.Type);
			btn.SetText(aug.DisplayName);
			btn.BorderColor = aug.tooltipColor;
		}
	}
	private void Btn_OnUpdate(UIElement affectedElement) {
		if (affectedElement is AugmentationText btn) {
			if (btn.AugmentationType != SelectedAugmentationType) {
				btn.TextColor = Color.White;
			}
		}
	}

	int SelectedAugmentationType = 0;
	private void Btn_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		if (listeningElement is AugmentationText btn) {
			SelectedAugmentationType = btn.AugmentationType;
			btn.TextColor = Color.Yellow;
			ModAugments aug = AugmentsLoader.GetAugments(SelectedAugmentationType);
			if (aug == null) {
				return;
			}
			AugmentationSelection_Text.SetText(aug.DisplayName);
			AugmentationSelection_Text.TextColor = aug.tooltipColor;
		}
	}

	List<AugmentationText> textlist = new();
	private void ConfirmButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		if (SelectedAugmentationType == 0) {
			return;
		}
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
		if (!item.GetGlobalItem<AugmentsWeapon>().AugmentsSlots.Contains(0)) {
			return;
		}
		int[] slots = item.GetGlobalItem<AugmentsWeapon>().AugmentsSlots;
		int counter = 0;
		for (int i = 0; i < slots.Length; i++) {
			if (slots[i] == 0) {
				continue;
			}
			counter++;
		}
		if (counter >= 1) {
			return;
		}
		if (AugmentsLoader.GetAugments(SelectedAugmentationType) == null) {
			return;
		}
		SelectedAugmentationType = 0;
		AugmentsWeapon.AddAugments(ref item, SelectedAugmentationType);
		AccSacrificeAugmentSlot.item.TurnToAir();
		AccAugmentResult.item = item.Clone();
		AccAugmentSlot.item.TurnToAir();
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
			if (!item.accessory && item.type != ItemID.None) {
				return;
			}
			ModUtils.SimpleItemMouseExchange(player, ref AccSacrificeAugmentSlot.item);
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
		AugmentationSelection_Text.Hide = hide;
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
public class AugmentationText : Roguelike_UITextPanel {
	public int AugmentationType { get; private set; }
	public void SetAug(int type) {
		AugmentationType = type;
	}
	public AugmentationText(int type, string text, float textScale = 1, bool large = false) : base(text, textScale, large) {
		if (AugmentsLoader.GetAugments(type) == null) {
			return;
		}
		AugmentationType = type;
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
	List<int> textUqID = new List<int>();
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
		Vector2 drawpos = GetInnerDimensions().Position() + texture.Size() * .5f;
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
				Vector2 drawpos = GetInnerDimensions().Position() + texture.Size() * .5f;
				spriteBatch.Draw(lockTexture, drawpos, null, Color.White, 0, origin, .87f, SpriteEffects.None, 0);
				return;
			}
			if (itemOwner == null)
				return;
			if (itemType != 0) {
				Vector2 drawpos = GetInnerDimensions().Position() + texture.Size() * .5f;
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
