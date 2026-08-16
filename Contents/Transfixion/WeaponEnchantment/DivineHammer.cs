using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Roguelike.Common.Systems;
using Roguelike.Common.Utils;
using System.Collections.Generic;
using Roguelike.Common.Global;
using System.Linq;

namespace Roguelike.Contents.Transfixion.WeaponEnchantment;
internal class DivineHammer : ModItem {
	public override void SetDefaults() {
		Item.width = Item.height = 32;
		Item.useTime = Item.useAnimation = 15;
		Item.rare = ItemRarityID.Red;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.UseSound = SoundID.Item37;
		Item.Set_InfoItem();
	}
	public override void ModifyTooltips(List<TooltipLine> tooltips) {
		string keybind = "";
		List<string> keybindList = ProcessTriggerSystem_Roguelike.Open_DivineUI.GetAssignedKeys();
		keybind = keybindList.FirstOrDefault();
		tooltips.Add(new TooltipLine(Mod, "Keybind", string.Format(ModUtils.LocalizationText("Items.DivineHammer", "Keybind"), $"[c/{Color.Yellow.Hex3()}:{keybind}]")));
	}
	public override bool? UseItem(Player player) {
		if (!UniversalSystem.CanEnchantmentBeAccess()) {
			ModUtils.CombatTextRevamp(player.Hitbox, Color.Red, "Can't access enchantment ui");
			return false;
		}
		if (player.ItemAnimationJustStarted) {
			ModContent.GetInstance<UniversalSystem>().ActivateEnchantmentUI();
		}
		return false;
	}
}
