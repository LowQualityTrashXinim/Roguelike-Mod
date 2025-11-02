using Terraria;
using Terraria.ModLoader;
using Roguelike.Texture;
using Roguelike.Common.Utils;
using Roguelike.Contents.Transfixion.Skill;

namespace Roguelike.Contents.Items.aDebugItem.SkillDebug;
internal class EnergyRechargeStation : ModItem {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.width = Item.height = 32;
		Item.Set_DebugItem(true);
	}
	public override void HoldItem(Player player) {
		var modplayer = player.GetModPlayer<SkillHandlePlayer>();
		modplayer.Modify_EnergyAmount(10);
	}
}
