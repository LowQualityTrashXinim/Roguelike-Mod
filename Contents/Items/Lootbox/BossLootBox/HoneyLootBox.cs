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
		//public override List<int> FlagNumber() => new List<int>() { 0, 1, 2, 4, 5 };
		public override void OnRightClick(Player player, PlayerStatsHandle modplayer) {
			var entitySource = player.GetSource_OpenItem(Type);
			GetArmorForPlayer(entitySource, player, Main.rand.NextBool(5));
			GetWeapon(entitySource, player, 5);
			player.QuickSpawnItem(entitySource, GetPotion(), 3);
		}
		public override void AbsoluteRightClick(Player player) {
			var entitySource = player.GetSource_OpenItem(Type);
			player.QuickSpawnItem(entitySource, ItemID.Honeyfin, 10);
		}
	}
}
