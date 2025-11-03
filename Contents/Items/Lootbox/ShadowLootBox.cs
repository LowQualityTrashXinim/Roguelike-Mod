using Terraria;
using Terraria.ID;
using Roguelike.Common.Utils;
using Roguelike.Common.Global;
using Roguelike.Common.Systems;
using System.Collections.Generic;

namespace Roguelike.Contents.Items.Lootbox {
	class ShadowLootBox : LootBoxBase {
		public override void LootPoolSetStaticDefaults() {

		}
		public override void SetDefaults() {
			Item.width = 54;
			Item.height = 38;
			Item.rare = ItemRarityID.LightPurple;
		}
		public override void AbsoluteRightClick(Player player) {
			var entitySource = player.GetSource_OpenItem(Type);
			int wing = Main.rand.Next(new int[] { ItemID.AngelWings, ItemID.DemonWings, ItemID.LeafWings, ItemID.FairyWings, ItemID.HarpyWings });
			player.QuickSpawnItem(entitySource, wing);
			player.QuickSpawnItem(entitySource, ItemID.MythrilAnvil);
			player.QuickSpawnItem(entitySource, ItemID.AdamantiteForge);
			if (Main.rand.NextBool()) {
				player.QuickSpawnItem(entitySource, ItemID.AdamantitePickaxe);
			}
			else {
				player.QuickSpawnItem(entitySource, ItemID.TitaniumPickaxe);
			}
			if (Main.rand.NextBool(20)) {
				player.QuickSpawnItem(entitySource, ItemID.RodofDiscord);
			}
			int RandomModdedBuff = Main.rand.Next(TerrariaArrayID.SpecialPotion);
			player.QuickSpawnItem(entitySource, RandomModdedBuff, 1);

			GetArmorForPlayer(entitySource, player);
			GetWeapon(entitySource, player, 2);
			GetAccessories(Type, player);
			GetPotion(Type, player);
			GetSkillLootbox(Type, player, 1);
		}
	}
}
