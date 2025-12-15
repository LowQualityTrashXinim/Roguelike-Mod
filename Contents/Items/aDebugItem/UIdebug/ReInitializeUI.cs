using Roguelike.Common.Systems;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.aDebugItem.InGameEditor;
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
		tooltips.Add(new TooltipLine(Mod, "DebugUIInitializer", "Current UI to reintialize : position UI"));
	}
	public override bool? UseItem(Player player) {
		if (player.ItemAnimationJustStarted) {
			ModContent.GetInstance<PositionWandSystem>().PosWandUI.RemoveAllChildren();
			ModContent.GetInstance<PositionWandSystem>().PosWandUI.OnInitialize();
			ModContent.GetInstance<PositionWandSystem>().PosWandUI.Activate();
		}
		return false;
	}
}

