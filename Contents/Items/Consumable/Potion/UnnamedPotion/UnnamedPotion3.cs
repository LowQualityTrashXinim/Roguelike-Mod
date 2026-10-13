using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Contents.Transfixion.Artifacts;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.Consumable.Potion.UnnamedPotion;
internal class UnnamedPotion3 : UnnamedPotionBase {
	public override int ModBuffType => ModContent.BuffType<UnnamedPotion3_Buff>();
}
public class UnnamedPotion3_Buff : EssenceBuff {
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void UpdatePlayer(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.RegenHP, Additive: 1.15f, Base: 1);
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.PureDamage, Additive: 1 - .12f);
	}
	public override void OnEnded(Player player) {
		player.Heal(25);
	}
}
