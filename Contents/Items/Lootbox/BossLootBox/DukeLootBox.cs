using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Terraria;
using Terraria.ID;

namespace Roguelike.Contents.Items.Lootbox.BossLootBox {
	internal class DukeLootBox : LootBoxBase {
		public override void SetDefaults() {
			Item.width = 38;
			Item.height = 30;
			Item.rare = ItemRarityID.Red;
		}
		public override void AbsoluteRightClick(Player player) {
			base.AbsoluteRightClick(player);
		}
	}
}
