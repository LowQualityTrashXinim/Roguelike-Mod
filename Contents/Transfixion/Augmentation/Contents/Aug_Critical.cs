using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;

namespace Roguelike.Contents.Transfixion.Augmentation.Contents;
public class Critical : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Microsoft.Xna.Framework.Color.Orange;
		ItemTypeID = ItemID.EyeoftheGolem;
	}
	public override int[] UpgradeAvailable() => [ItemID.SharkToothNecklace, ItemID.VampireKnives, ItemID.RedCape];
	public override string Description2(Player player, AugmentsWeapon acc, Item item, string Extra) {
		string desc = base.Description2(player, acc, item, Extra);
		switch (Extra) {
			case "1":
				return Get_FormattedDescription(acc, desc, ItemID.SharkToothNecklace);
			case "2":
				return Get_FormattedDescription(acc, desc, ItemID.VampireKnives);
			case "3":
				return Get_FormattedDescription(acc, desc, ItemID.RedCape);
			default:
				return desc;
		}
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, Item item) {
		var modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.CritChance, Base: 5);
		modplayer.AddStatsToPlayer(PlayerStats.CritDamage, 1.1f);
	}
	public override void OnHitNPCWithItem(Player player, AugmentsWeapon acc, Item item, NPC npc, NPC.HitInfo hitInfo) {
		if (acc.AugmentUpgrade.Contains(ItemID.VampireKnives)) {
			if (hitInfo.Crit) {
				player.Heal(Math.Clamp((int)Math.Ceiling(player.statLifeMax2 * 0.01f), 1, player.statLifeMax2));
			}
		}
	}
	public override void OnHitNPCWithProj(Player player, AugmentsWeapon acc, Projectile proj, NPC npc, NPC.HitInfo hitInfo) {
		if (acc.AugmentUpgrade.Contains(ItemID.VampireKnives)) {
			if (hitInfo.Crit && !proj.minion && proj.Check_ItemTypeSource(player.HeldItem.type)) {
				player.Heal(Math.Clamp((int)Math.Ceiling(player.statLifeMax2 * 0.01f), 1, player.statLifeMax2));
			}
		}
	}
	public override void ModifyHitNPCWithItem(Player player, AugmentsWeapon acc, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (acc.AugmentUpgrade.Contains(ItemID.SharkToothNecklace)) {
			if (player.GetModPlayer<PlayerStatsHandle>().ModifyHit_Before_Crit) {
				modifiers.ScalingArmorPenetration += .5f;
			}
		}
		if (acc.AugmentUpgrade.Contains(ItemID.RedCape)) {
			int critchanceReroll = player.GetWeaponCrit(item);
			if (Main.rand.Next(1, 101) < critchanceReroll) {
				modifiers.CritDamage += 1;
			}
		}
	}
	public override void ModifyHitNPCWithProj(Player player, AugmentsWeapon acc, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (acc.AugmentUpgrade.Contains(ItemID.SharkToothNecklace)) {
			if (player.GetModPlayer<PlayerStatsHandle>().ModifyHit_Before_Crit && !proj.minion && proj.Check_ItemTypeSource(player.HeldItem.type)) {
				modifiers.ScalingArmorPenetration += .5f;
			}
		}
		if (acc.AugmentUpgrade.Contains(ItemID.RedCape)) {
			int critchanceReroll = proj.CritChance;
			if (Main.rand.Next(1, 101) < critchanceReroll) {
				modifiers.CritDamage += 1;
			}
		}
	}
}
