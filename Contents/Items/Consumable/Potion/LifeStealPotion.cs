using Terraria;
using Terraria.ModLoader;
using Roguelike.Texture;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;

namespace Roguelike.Contents.Items.Consumable.Potion;
internal class LifeStealPotion : ModItem {
	public override string Texture => ModTexture.MISSINGTEXTUREPOTION;
	public override void SetStaticDefaults() {
		ModItemLib.LootboxPotion.Add(Item);
	}
	public override void SetDefaults() {
		Item.BossRushDefaultPotion(32, 32, ModContent.BuffType<LifeStealBuff>(), ModUtils.ToMinute(4));
		Item.Set_ItemIsRPG();
	}
}
public class LifeStealBuff : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<PlayerStatsHandle>().LifeSteal += .05f;
	}
}
