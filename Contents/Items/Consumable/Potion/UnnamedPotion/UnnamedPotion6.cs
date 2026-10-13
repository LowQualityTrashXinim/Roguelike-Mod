using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.Consumable.Potion.UnnamedPotion;
internal class UnnamedPotion6 : UnnamedPotionBase {
	public override int ModBuffType => ModContent.BuffType<UnnamedPotion6_Buff>();
}
public class UnnamedPotion6_Buff : UnnamedBuff {
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void UpdatePlayer(Player player) {
		player.endurance += .15f;
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.MovementSpeed, Additive: 1 - .14f);
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.AttackSpeed, Additive: 1 - .06f);
	}
}
