using Roguelike.Common.Mode.BossRushMode;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.Toggle;
internal class BossRushModeActivation : ModItem {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.height = 60;
		Item.width = 56;
		Item.value = 0;
		Item.rare = ItemRarityID.Purple;
		Item.useAnimation = 30;
		Item.useTime = 30;
		Item.useStyle = ItemUseStyleID.HoldUp;
		Item.scale = .5f;
	}
	public override bool CanUseItem(Player player) {
		return !ModUtils.IsAnyVanillaBossAlive();
	}
	public override bool? UseItem(Player player) {
		if (player.whoAmI == Main.myPlayer) {
			SoundEngine.PlaySound(SoundID.Roar, player.position);
			Main.NewText("Boss rush mode start !");
			ModContent.GetInstance<BossRushStructureHandler>().Start_BossRush();
		}
		return true;
	}
}
