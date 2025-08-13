using Roguelike.Common.Systems.Achievement;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.aDebugItem.Resetter;

class AchievementResetter : ModItem {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.width = Item.height = 32;
		Item.useTime = Item.useAnimation = 15;
		Item.useStyle = ItemUseStyleID.HoldUp;
		Item.Set_DebugItem(true);
	}
	public override bool? UseItem(Player player) {
		if (player.ItemAnimationJustStarted) {
			int achievementCount = AchievementSystem.Achievements.Count;
			for (int i = 0; i < achievementCount; i++) {
				AchievementSystem.SafeGetAchievement(i).Achieved = false;
			}
		}
		return base.UseItem(player);
	}
}
