using Roguelike.Common.Utils;
using Roguelike.Contents.Items.Consumable.SpecialReward;
using Roguelike.Texture;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.aDebugItem.Resetter
{
	internal class ClearAllNoHit : ModItem {
		public override string Texture => ModTexture.MissingTexture_Default;
		public override void SetDefaults() {
			Item.BossRushDefaultToConsume(1, 1);
			Item.Set_DebugItem(true);
		}
		public override bool? UseItem(Player player) {
			player.GetModPlayer<NoHitPlayerHandle>().BossNoHitNumber.Clear();
			player.GetModPlayer<NoHitPlayerHandle>().DontHitBossNumber.Clear();
			return true;
		}
	}
}
