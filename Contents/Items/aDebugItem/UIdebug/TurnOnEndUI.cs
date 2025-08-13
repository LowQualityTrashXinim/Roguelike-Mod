using Terraria;
using Terraria.ModLoader;
using Roguelike.Common.Systems;
using Roguelike.Texture;
using Roguelike.Common.Utils;

namespace Roguelike.Contents.Items.aDebugItem.UIdebug;

public class TurnOnEndUI : ModItem {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.BossRushDefaultToConsume(1, 1);
		Item.Set_DebugItem(true);
	}
	public override bool? UseItem(Player player) {
		if (player.ItemAnimationJustStarted) {
			ModContent.GetInstance<UniversalSystem>().defaultUI.TurnOnEndOfDemoMessage();
		}
		return false;
	}
}

