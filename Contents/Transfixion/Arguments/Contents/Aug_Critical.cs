using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using System;
using Terraria;

namespace Roguelike.Contents.Transfixion.Arguments.Contents;
public class Critical : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Microsoft.Xna.Framework.Color.Orange;
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, int index, Item item) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.CritChance, Base: 5);
		modplayer.AddStatsToPlayer(PlayerStats.CritDamage, 1.1f);
	}
	public override void OnHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC npc, NPC.HitInfo hitInfo) {
		if (acc.Check_ChargeConvertToStackAmount(index) >= 2) {
			if (hitInfo.Crit) {
				player.Heal(Math.Clamp((int)Math.Ceiling(player.statLifeMax2 * 0.01f), 1, player.statLifeMax2));
			}
		}
	}
	public override void OnHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC npc, NPC.HitInfo hitInfo) {
		if (acc.Check_ChargeConvertToStackAmount(index) >= 2) {
			if (hitInfo.Crit && !proj.minion && proj.Check_ItemTypeSource(player.HeldItem.type)) {
				player.Heal(Math.Clamp((int)Math.Ceiling(player.statLifeMax2 * 0.01f), 1, player.statLifeMax2));
			}
		}
	}
	public override void ModifyHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (acc.Check_ChargeConvertToStackAmount(index) >= 1) {
			if (player.GetModPlayer<PlayerStatsHandle>().ModifyHit_Before_Crit) {
				modifiers.ScalingArmorPenetration += .5f;
			}
		}
		if (acc.Check_ChargeConvertToStackAmount(index) >= 3) {
			int critchanceReroll = player.GetWeaponCrit(item);
			if (Main.rand.Next(1, 101) < critchanceReroll) {
				modifiers.CritDamage += 1;
			}
		}
	}

	public override void ModifyHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (acc.Check_ChargeConvertToStackAmount(index) >= 1) {
			if (player.GetModPlayer<PlayerStatsHandle>().ModifyHit_Before_Crit && !proj.minion && proj.Check_ItemTypeSource(player.HeldItem.type)) {
				modifiers.ScalingArmorPenetration += .5f;
			}
		}
		if (acc.Check_ChargeConvertToStackAmount(index) >= 3) {
			int critchanceReroll = proj.CritChance;
			if (Main.rand.Next(1, 101) < critchanceReroll) {
				modifiers.CritDamage += 1;
			}
		}
	}
}
