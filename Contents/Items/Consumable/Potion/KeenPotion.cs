using Terraria;
using Terraria.ModLoader;
using Roguelike.Texture;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;

namespace Roguelike.Contents.Items.Consumable.Potion;
internal class KeenPotion : ModItem {
	public override string Texture => ModTexture.MISSINGTEXTUREPOTION;
	public override void SetStaticDefaults() {
		ModItemLib.LootboxPotion.Add(Item);
	}
	public override void SetDefaults() {
		Item.BossRushDefaultPotion(32, 32, ModContent.BuffType<KeenBuff>(), ModUtils.ToMinute(2));
		Item.Set_AdvancedBuffItem();
	}
}
public class KeenBuff : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.CritDamage, 1f);
	}
}
public class KeenPlayer : ModPlayer {
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		if(Player.HasBuff<KeenBuff>()) {
			modifiers.SetCrit();
			int buffindex = Player.FindBuffIndex(ModContent.BuffType<KeenBuff>());
			Player.buffTime[buffindex] -= ModUtils.ToSecond(30);
		}
	}
}
