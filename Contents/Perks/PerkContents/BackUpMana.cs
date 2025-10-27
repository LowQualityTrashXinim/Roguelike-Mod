using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Perks.PerkContents;
public class BackUpMana : Perk {
	public override void SetDefaults() {
		textureString = ModUtils.GetTheSameTextureAsEntity<BackUpMana>();
		CanBeStack = true;
		StackLimit = 3;
	}
	public override void ModifyMaxStats(Player player, ref StatModifier health, ref StatModifier mana) {
		mana.Base += 67 * StackAmount(player);
	}
	public override void OnMissingMana(Player player, Item item, int neededMana) {
		if (!player.HasBuff<BackUpMana_CoolDown>()) {
			player.statMana = player.statManaMax2;
			player.AddBuff(ModContent.BuffType<BackUpMana_CoolDown>(), ModUtils.ToSecond(Math.Clamp(37 - 7 * StackAmount(player), 1, 9999)));
		}
	}
	class BackUpMana_CoolDown : ModBuff {
		public override string Texture => ModTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultDeBuff(true);
		}
		public override void Update(Player player, ref int buffIndex) {
			PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.RegenMana, -.5f);
		}
	}
}
