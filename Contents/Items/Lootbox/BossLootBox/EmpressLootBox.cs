using Terraria;
using Terraria.ID;
using Roguelike.Common.Utils;
using Roguelike.Common.Global;

namespace Roguelike.Contents.Items.Lootbox.BossLootBox {
	internal class EmpressLootBox : LootBoxBase {
		public override void SetDefaults() {
			Item.width = 37;
			Item.height = 35;
			Item.rare = ItemRarityID.Red;
		}
		public override void AbsoluteRightClick(Player player) {
			base.AbsoluteRightClick(player);
		}
	}
}
