using Roguelike.Common.Systems;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.BuilderItem;
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
			ModContent.GetInstance<UniversalSystem>().UI_BRmodifier.RemoveAllChildren();
			ModContent.GetInstance<UniversalSystem>().UI_BRmodifier.OnInitialize();
			ModContent.GetInstance<UniversalSystem>().UI_BRmodifier.Activate();

			ModContent.GetInstance<GeneralBuilderToolSystem>().GeneralBuilderToolState.RemoveAllChildren();
			ModContent.GetInstance<GeneralBuilderToolSystem>().GeneralBuilderToolState.OnInitialize();
			ModContent.GetInstance<GeneralBuilderToolSystem>().GeneralBuilderToolState.Activate();
		}
		return false;
	}
}

