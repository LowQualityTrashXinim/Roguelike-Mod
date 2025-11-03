using Terraria;
using Terraria.ID;
using Roguelike.Common.Utils;

using Roguelike.Common.Global;

namespace Roguelike.Contents.Items.Lootbox.BossLootBox {
	class HoneyLootBox : LootBoxBase {
		public override void LootPoolSetStaticDefaults() {
		}
		public override void SetDefaults() {
			Item.width = 54;
			Item.height = 38;
			Item.rare = ItemRarityID.LightRed;
		}
		public override void AbsoluteRightClick(Player player) {
			var entitySource = player.GetSource_OpenItem(Type);
			player.QuickSpawnItem(entitySource, ItemID.Honeyfin, 10);
		}
	}
}
