using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using Roguelike.Common.Systems;
using Roguelike.Texture;
using Roguelike.Common.Utils;

namespace Roguelike.Contents.Items.aDebugItem.UIdebug;
class ReInitializeUI : ModItem {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.BossRushDefaultToConsume(1, 1);
		Item.Set_DebugItem(true);
	}
	public override void ModifyTooltips(List<TooltipLine> tooltips) {
		tooltips.Add(new TooltipLine(Mod, "DebugUIInitializer", "Current UI to reintialize :  UI"));
	}
	public override bool? UseItem(Player player) {
		if (player.ItemAnimationJustStarted) {
			//ModContent.GetInstance<UniversalSystem>().artifactUI.RemoveAllChildren();
			//ModContent.GetInstance<UniversalSystem>().artifactUI.OnInitialize();
			//ModContent.GetInstance<UniversalSystem>().artifactUI.Activate();
		}
		return false;
	}
}

