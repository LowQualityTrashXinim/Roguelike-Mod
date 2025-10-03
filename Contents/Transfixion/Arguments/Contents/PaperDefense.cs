using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Transfixion.Arguments.Contents;
internal class PaperDefense : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Red;
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, int index, Item item) {
		int counter = acc.Check_ChargeConvertToStackAmount(index);
		PlayerStatsHandle handle = player.ModPlayerStats();
		handle.UpdateDefenseBase.Base += 1;
		PaperDefense_ModPlayer paper = player.GetModPlayer<PaperDefense_ModPlayer>();
		if (counter >= 1) {
			paper.PaperDefenseII = true;
			if (paper.Trinket6_Stack <= 0) {
				return;
			}
			if (--paper.Trinket6_StackDecay <= 0) {
				paper.Trinket6_Stack--;
				if (counter >= 2) {
					paper.Trinket6_StackLossses++;
					paper.Trinket6_StackDecay = ModUtils.ToSecond(5);
					player.AddBuff(ModContent.BuffType<PaperDefense_DefensesBonus_Buff>(), ModUtils.ToMinute(1));
				}
			}
			else {
				player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.Defense, -.05f * paper.Trinket6_Stack);
			}
		}
	}
	public override TooltipLine ModifyDescription(Player player, AugmentsWeapon acc, int index, Item item, int stack) {
		string desc = Description;
		for (int i = 0; i < stack; i++) {
			switch (stack) {
				case 1:
					desc += "\n" + Description2("1");
					break;
				case 2:
					desc += "\n" + Description2("2");
					break;
				case 3:
				case 4:
				case 5:
					break;
			}
		}
		TooltipLine line = new(Mod, Name, desc);
		return line;
	}
	public override string ModifyName(Player player, AugmentsWeapon acc, int index, Item item, int stack) {
		string name = DisplayName;
		switch (stack) {
			case 1:
				name = DisplayName2("1");
				break;
			case 2:
				name = DisplayName2("2");
				break;
			case 3:
			case 4:
			case 5:
				break;
		}
		return ColorWrapper(name);
	}
}
public class PaperDefense_ModPlayer : ModPlayer {
	public bool PaperDefenseII = false;
	public int Trinket6_Stack = 0;
	public int Trinket6_StackDecay = 0;
	public int Trinket6_StackLossses = 0;
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		if (PaperDefenseII) {
			Trinket6_Stack = Math.Clamp(Trinket6_Stack + 1, 0, 10);
			Trinket6_StackDecay = ModUtils.ToSecond(5);
		}
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		if (PaperDefenseII) {
			Trinket6_Stack = Math.Clamp(Trinket6_Stack + 1, 0, 10);
			Trinket6_StackDecay = ModUtils.ToSecond(5);
		}
	}

}
public class PaperDefense_DefensesBonus_Buff : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare) {
		PaperDefense_ModPlayer trinketplayer = Main.LocalPlayer.GetModPlayer<PaperDefense_ModPlayer>();
		tip = string.Format(tip,new string[] { trinketplayer.Trinket6_StackLossses.ToString(), trinketplayer.Trinket6_Stack.ToString() });
	}
	public override bool ReApply(Player player, int time, int buffIndex) {
		player.buffTime[buffIndex] = time;
		return true;
	}
	public override void Update(Player player, ref int buffIndex) {
		PaperDefense_ModPlayer trinketplayer = player.GetModPlayer<PaperDefense_ModPlayer>();
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.Defense, Base: 5 * trinketplayer.Trinket6_StackLossses);
		if (player.buffTime[buffIndex] <= 0) {
			trinketplayer.Trinket6_StackLossses = 0;
		}
	}
}
