using Roguelike.Common.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.Lootbox;
internal class SpoilBag : ModItem {
	public override void SetDefaults() {
		Item.height = Item.width = 52;
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
