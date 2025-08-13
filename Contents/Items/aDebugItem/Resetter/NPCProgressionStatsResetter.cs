using Terraria;
using Roguelike.Texture;
using Terraria.ModLoader;
using Roguelike.Common.Systems;
using Roguelike.Common.Utils;

namespace Roguelike.Contents.Items.aDebugItem.Resetter
{
	class NPCProgressionStatsResetter : ModItem {
		public override string Texture => ModTexture.MissingTexture_Default;
		public override void SetDefaults() {
			Item.BossRushDefaultToConsume(32, 32);
			Item.Set_DebugItem(true);
		}
		public override bool? UseItem(Player player) {
			if (player.ItemAnimationJustStarted) {
				ModContent.GetInstance<UniversalSystem>().ListOfBossKilled.Clear();
			}
			return base.UseItem(player);
		}
	}
}
