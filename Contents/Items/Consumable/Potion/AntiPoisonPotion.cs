using Terraria.ModLoader;
using Terraria;
using Roguelike.Common.Utils;
 
using Roguelike.Texture;

namespace Roguelike.Contents.Items.Consumable.Potion;
internal class AntiPoisonPotion : ModItem {
	public override string Texture => ModTexture.MISSINGTEXTUREPOTION;
	public override void SetDefaults() {
		Item.BossRushDefaultPotion(32, 32, ModContent.BuffType<AntiPoisonBuff>(), ModUtils.ToMinute(1.5f));
		Item.Set_ItemIsRPG();
	}
}
public class AntiPoisonBuff : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		for (int i = 0; i < TerrariaArrayID.PoisonBuff.Length; i++) {
			player.buffImmune[TerrariaArrayID.PoisonBuff[i]] = true;
		}
	}
}
