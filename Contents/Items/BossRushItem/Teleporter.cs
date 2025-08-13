using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Roguelike.Common.Systems;
using Roguelike.Common.Utils;

namespace Roguelike.Contents.Items.BossRushItem;
class ExoticTeleporter : ModItem {
	public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.CellPhone);
	public override void SetDefaults() {
		Item.BossRushDefaultToConsume(32, 32);
	}
	public override bool? UseItem(Player player) {
		if (player.itemAnimation == player.itemAnimationMax && !ModUtils.IsAnyVanillaBossAlive()) {
			ModContent.GetInstance<UniversalSystem>().ActivateTeleportUI();
		}
		return base.UseItem(player);
	}
}
