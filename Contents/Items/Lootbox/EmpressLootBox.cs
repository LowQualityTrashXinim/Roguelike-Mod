using Terraria;
using Terraria.ID;
using Roguelike.Common.Utils;
using Roguelike.Common.Global;

namespace Roguelike.Contents.Items.Lootbox {
	internal class EmpressLootBox : LootBoxBase {
		public override void SetDefaults() {
			Item.width = 37;
			Item.height = 35;
			Item.rare = ItemRarityID.Red;
		}
		public override void OnRightClick(Player player, PlayerStatsHandle modplayer) {
			var entitySource = player.GetSource_OpenItem(Type);
			modplayer.GetAmount();
			GetWeapon(entitySource, player, modplayer.weaponAmount);
			for (int i = 0; i < 3; i++) {
				player.QuickSpawnItem(entitySource, GetAccessory());
			}
		}
	}
}
