using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Roguelike.Texture;
using Roguelike.Common.Global;

namespace Roguelike.Contents.Items.NoneSynergy
{
	internal class PlusOneBullet : ModItem {
		public override string Texture => ModTexture.MissingTexture_Default;
		public override void SetDefaults() {
			Item.accessory = true;
			Item.height = 30;
			Item.width = 28;
			Item.rare = ItemRarityID.Green;
			Item.value = 1000000;
		}
		public override void UpdateEquip(Player player) {
			if (player.HeldItem.useAmmo == AmmoID.Bullet) {
				player.GetModPlayer<PlayerStatsHandle>().Request_ShootExtra(1, .35f);
			}
		}
	}
}
