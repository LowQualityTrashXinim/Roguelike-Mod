using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Contents.Transfixion.Artifacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.Consumable.Potion.UnnamedPotion;
internal class UnnamedPotion1 : UnnamedPotionBase {
	public override int ModBuffType => ModContent.BuffType<UnnamedPotion1_Buff>();
}
public class UnnamedPotion1_Buff : UnnamedBuff {
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void UpdatePlayer(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.PureDamage, Additive: 1.12f);
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.Defense, Base: -10);
	}
}
