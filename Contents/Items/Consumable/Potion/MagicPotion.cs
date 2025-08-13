using Terraria;
using Terraria.ModLoader;
using Roguelike.Texture;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;

namespace Roguelike.Contents.Items.Consumable.Potion;

public class MagicPotion : ModItem {
	public override void SetStaticDefaults() {
		ModItemLib.LootboxPotion.Add(Item);
	}
	public override string Texture => ModTexture.MISSINGTEXTUREPOTION;
	public override void SetDefaults() {
		Item.BossRushDefaultPotion(32, 32, ModContent.BuffType<Magic_Buff>(), ModUtils.ToMinute(10));
	}
}
public class Magic_Buff : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handle = player.GetModPlayer<PlayerStatsHandle>();
		handle.AddStatsToPlayer(PlayerStats.MagicDMG, Multiplicative: 1.1f);
		handle.AddStatsToPlayer(PlayerStats.MagicCritChance, Base: 10);
		handle.AddStatsToPlayer(PlayerStats.MagicAtkSpeed, 1.1f);
	}
}
