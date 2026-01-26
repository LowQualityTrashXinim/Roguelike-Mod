using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Roguelike.Common.Mode.BossRushMode;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.Lootbox.Lootpool;
using Roguelike.Contents.Transfixion.Perks.BlessingPerk;
using Roguelike.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace Roguelike.Common.Systems.SpoilSystem;
public class ModSpoilSystem : ModSystem {
	private static Dictionary<string, ModSpoil> _spoils = new();
	public static int TotalCount => _spoils.Count;
	public override void Load() {
		foreach (var type in Mod.Code.GetTypes().Where(type => !type.IsAbstract && type.IsAssignableTo(typeof(ModSpoil)))) {
			var spoil = (ModSpoil)Activator.CreateInstance(type);
			spoil.SetStaticDefault();
			_spoils.Add(spoil.Name, spoil);
		}
	}
	public static List<ModSpoil> GetSpoilsList() => new(_spoils.Values);

	public override void Unload() {
		_spoils = null;
	}
	public static ModSpoil GetSpoils(string name) {
		return _spoils.ContainsKey(name) ? _spoils[name] : null;
	}
}
public static class SpoilDropRarity {
	public readonly static int Common = ItemRarityID.White;
	public readonly static int Uncommon = ItemRarityID.Blue;
	public readonly static int Rare = ItemRarityID.Yellow;
	public readonly static int SuperRare = ItemRarityID.Purple;
	public readonly static int SSR = ItemRarityID.Red;
	/// <summary>
	/// Use this to make sure chance applying effect for spoil work
	/// </summary>
	/// <param name="chance"></param>
	/// <returns></returns>
	public static bool ChanceWrapper(float chance) {
		if (!UniversalSystem.LuckDepartment(UniversalSystem.CHECK_RARESPOILS)) {
			return false;
		}
		if (Main.LocalPlayer.HasPerk<BlessingOfPerk>()) {
			chance *= 1.5f;
		}
		return Main.rand.NextFloat() <= chance;
	}
	public static bool UncommonDrop() => ChanceWrapper(.44f);
	public static bool RareDrop() => ChanceWrapper(.10f);
	public static bool SuperRareDrop() => ChanceWrapper(.025f);
	public static bool SSRDrop() => ChanceWrapper(.001f);
}
public abstract class ModSpoil {
	public string Name => GetType().Name;
	public int RareValue = 0;
	public string DisplayName => $"- {ModUtils.LocalizationText("Spoils", $"{Name}.DisplayName")} -";
	public string Description => ModUtils.LocalizationText("Spoils", $"{Name}.Description");
	public virtual void SetStaticDefault() { }
	public virtual string FinalDisplayName() => DisplayName;
	public virtual string FinalDescription() => Description;
	public virtual bool IsSelectable(Player player) {
		return true;
	}
	public virtual void OnChoose(Player player) { }
	public sealed override string ToString() {
		return base.ToString();
	}
}
public class SpoilsPlayer : ModPlayer {
	public List<string> SpoilsGift = new();
	public override void Initialize() {
		SpoilsGift = new();
	}
}
public class SpoilsUIState : UIState {
	public int Limit_Spoils = 5;
	public List<SpoilsUIButton> btn_List;
	public UITextPanel<string> panel;
	public override void OnInitialize() {
		panel = new UITextPanel<string>(Language.GetTextValue($"Mods.Roguelike.SystemTooltip.Spoil.Header"));
		panel.HAlign = .5f;
		panel.VAlign = .3f;
		panel.UISetWidthHeight(150, 53);
		Append(panel);
		Limit_Spoils = 5;
		btn_List = new List<SpoilsUIButton>();
	}
	public override void OnActivate() {
		btn_List.Clear();
		SpoilsPlayer modplayer = Main.LocalPlayer.GetModPlayer<SpoilsPlayer>();
		Player player = Main.LocalPlayer;
		List<ModSpoil> SpoilList = ModSpoilSystem.GetSpoilsList();
		if (modplayer.SpoilsGift.Count > Limit_Spoils - 1) {
			SpoilList.Clear();
			SpoilList = modplayer.SpoilsGift.Select(ModSpoilSystem.GetSpoils).ToList();
			modplayer.SpoilsGift.Clear();
		}
		else {
			modplayer.SpoilsGift.Clear();
			for (int i = SpoilList.Count - 1; i >= 0; i--) {
				ModSpoil spoil = SpoilList[i];
				if (!spoil.IsSelectable(player)) {
					SpoilList.Remove(spoil);
				}
			}
		}
		if (SpoilList.Count < 1) {
			SpoilList = ModSpoilSystem.GetSpoilsList();
		}
		//prioritize rarer spoil
		int spoilPriortize = 1;
		for (int i = 0; i < Limit_Spoils; i++) {
			ModSpoil spoil = Main.rand.Next(SpoilList);
			if (spoilPriortize > 0) {
				spoilPriortize--;
				foreach (var item in SpoilList) {
					if (item.RareValue > SpoilDropRarity.Rare) {
						spoil = item;
					}
				}
			}
			float Hvalue = MathHelper.Lerp(.3f, .7f, i / (float)(Limit_Spoils - 1));
			SpoilsUIButton btn = new SpoilsUIButton(TextureAssets.InventoryBack, spoil);
			modplayer.SpoilsGift.Add(spoil.Name);
			SpoilList.Remove(spoil);
			btn.HAlign = Hvalue;
			btn.VAlign = .4f;
			btn_List.Add(btn);
			Append(btn);
		}
		//SpoilsUIButton btna = new SpoilsUIButton(TextureAssets.InventoryBack10, null);
		//btna.HAlign = .7f;
		//btna.VAlign = .4f;
		//btn_List.Add(btna);
		//Append(btna);
	}
}
public class SpoilsUIButton : UIImageButton {
	public ModSpoil spoil;
	int LootboxItem = 0;
	public SpoilsUIButton(Asset<Texture2D> texture, ModSpoil Spoil) : base(texture) {
		spoil = Spoil;
	}
	public override void LeftClick(UIMouseEvent evt) {
		Player player = Main.LocalPlayer;
		SpoilsPlayer modplayer = player.GetModPlayer<SpoilsPlayer>();
		if (spoil == null) {
			List<ModSpoil> SpoilList = ModSpoilSystem.GetSpoilsList();
			for (int i = SpoilList.Count - 1; i >= 0; i--) {
				ModSpoil spoil = SpoilList[i];
				if (!spoil.IsSelectable(player)) {
					SpoilList.Remove(spoil);
				}
			}
			Main.rand.Next(SpoilList).OnChoose(player);
			modplayer.SpoilsGift.Clear();
			ModContent.GetInstance<UniversalSystem>().DeactivateUI();
			return;
		}
		spoil.OnChoose(player);
		modplayer.SpoilsGift.Clear();
		ModContent.GetInstance<UniversalSystem>().DeactivateUI();
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if (ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
		if (IsMouseHovering) {
			if (LootboxSystem.GetItemPool(LootboxItem) == null && !Main.LocalPlayer.IsDebugPlayer()) {
				return;
			}
			if (spoil == null) {
				Main.instance.MouseText(Language.GetTextValue($"Mods.Roguelike.SystemTooltip.Spoil.Randomize"));
			}
			else {
				Main.instance.MouseText(spoil.FinalDisplayName(), spoil.FinalDescription(), spoil.RareValue);
			}
		}
		else {
			if (!Parent.Children.Where(e => e.IsMouseHovering).Any()) {
				Main.instance.MouseText("");
			}
		}
	}
}
internal class SpoilBag : ModItem {
	public override string Texture => ModTexture.PLACEHOLDERCHEST;
	public override void SetDefaults() {
		Item.height = 60;
		Item.width = 56;
		Item.value = 0;
		Item.rare = ItemRarityID.Purple;
		Item.useAnimation = 30;
		Item.useTime = 30;
		Item.useStyle = ItemUseStyleID.HoldUp;
		Item.scale = .5f;
		Item.maxStack = 9999;
	}
	public override bool CanRightClick() => true;
	public override void RightClick(Player player) {
		if (player.whoAmI == Main.myPlayer) {
			ModContent.GetInstance<UniversalSystem>().ActivateSpoilsUI();
		}
	}
}
