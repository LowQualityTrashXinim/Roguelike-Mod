using Terraria;
using Terraria.ModLoader;
using Roguelike.Contents.Skill;
using Roguelike.Texture;
using Roguelike.Common.Utils;

namespace Roguelike.Contents.Items.Consumable;
internal class SkillSlotUnlock : ModItem {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.BossRushDefaultToConsume(32, 32);
	}
	public override bool? UseItem(Player player) {
		SkillHandlePlayer modplayer = player.GetModPlayer<SkillHandlePlayer>();
		return modplayer.IncreasesSkillSlot();
	}
}
