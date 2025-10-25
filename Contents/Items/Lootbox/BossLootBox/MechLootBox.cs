using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.Consumable.Spawner;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.Lootbox.BossLootBox {
	class MechLootBox : LootBoxBase {
		public override void SetDefaults() {
			Item.width = 54;
			Item.height = 38;
			Item.rare = ItemRarityID.Lime;
		}
		public override List<int> FlagNumAcc() => new List<int> { 8, 9, 10 };
		public override void OnRightClick(Player player, PlayerStatsHandle modplayer) {
			var entitySource = player.GetSource_OpenItem(Type);
			modplayer.GetAmount();
			if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3) {
				GetArmorForPlayer(entitySource, player, Main.rand.NextBool(5));
			}
			GetWeapon(entitySource, player, modplayer.weaponAmount);
			for (int i = 0; i < 2; i++) {
				player.QuickSpawnItem(entitySource, GetAccessory());
			}
			for (int i = 0; i < modplayer.potionTypeAmount; i++) {
				player.QuickSpawnItem(entitySource, GetPotion(), modplayer.potionNumAmount);
			}
		}
		public override void AbsoluteRightClick(Player player) {
			var entitySource = player.GetSource_OpenItem(Type);
			int wing = Main.rand.Next(new int[] { ItemID.ButterflyWings, ItemID.FlameWings, ItemID.FrozenWings, ItemID.SteampunkWings, ItemID.Jetpack });
			player.QuickSpawnItem(entitySource, wing);
			if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3) {
				player.QuickSpawnItem(entitySource, ItemID.ChlorophytePickaxe);
			}
			player.QuickSpawnItem(entitySource, ItemID.LifeFruit, 5);
			player.QuickSpawnItem(entitySource, ItemID.DD2ElderCrystal);
		}
	}
}
