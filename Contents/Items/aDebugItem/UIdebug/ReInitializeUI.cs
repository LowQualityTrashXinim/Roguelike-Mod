using Roguelike.Common.Systems;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.aDebugItem.UIdebug;
class ReInitializeUI : ModItem {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.BossRushDefaultToConsume(1, 1);
		Item.Set_DebugItem(true);
	}
	public override void ModifyTooltips(List<TooltipLine> tooltips) {
		tooltips.Add(new TooltipLine(Mod, "DebugUIInitializer", "Current UI to reintialize : default UI"));
	}
	public override bool? UseItem(Player player) {
		if (player.ItemAnimationJustStarted) {
			ModContent.GetInstance<UniversalSystem>().shopUI.RemoveAllChildren();
			ModContent.GetInstance<UniversalSystem>().shopUI.OnInitialize();
			ModContent.GetInstance<UniversalSystem>().shopUI.Activate();
		}
		return false;
	}
}

