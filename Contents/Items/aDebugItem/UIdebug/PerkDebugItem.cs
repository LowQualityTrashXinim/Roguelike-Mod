using Terraria;
using Terraria.ModLoader;
using Roguelike.Common.Systems;
using Roguelike.Contents.Perks;
 
using Roguelike.Texture;
using Roguelike.Common.Utils;

namespace Roguelike.Contents.Items.aDebugItem.UIdebug;
internal class PerkDebugItem : ModItem {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.BossRushDefaultToConsume(32, 32);
		Item.Set_DebugItem(true);
		Item.maxStack = 999;
	}
	public override bool AltFunctionUse(Player player) => true;
	public override bool? UseItem(Player player) {
		var modplayer = player.GetModPlayer<PerkPlayer>();
		if (player.altFunctionUse != 2) {
			var uiSystemInstance = ModContent.GetInstance<UniversalSystem>();
			uiSystemInstance.ActivatePerkUI(PerkUIState.DebugState);
		}
		else if (player.IsDebugPlayer()) {
			modplayer.perks.Clear();
		}
		return true;
	}
}
