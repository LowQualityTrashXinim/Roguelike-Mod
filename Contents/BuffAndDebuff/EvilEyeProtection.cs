using Roguelike.Contents.Items.Accessories.EnragedBossAccessories.EvilEye;
using Roguelike.Texture;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.BuffAndDebuff;
internal class EvilEyeProtection : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		Main.debuff[Type] = false;
		Main.buffNoSave[Type] = true;
	}

	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<EvilEyePlayer>().EyeProtection = false;
	}
}
