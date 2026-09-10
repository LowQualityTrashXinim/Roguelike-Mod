using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Transfixion.Augmentation.Contents;
public class ReactiveDefenses : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Microsoft.Xna.Framework.Color.MediumBlue;
		ItemTypeID = ItemID.TurtleShell;
	}
	public override int[] UpgradeAvailable() => [ItemID.FrozenTurtleShell, ItemID.CobaltShield, ItemID.BandofRegeneration];
	public override string Description2(Player player, AugmentsWeapon acc, Item item, string Extra) {
		string desc = base.Description2(player, acc, item, Extra);
		switch (Extra) {
			case "1":
				return Get_FormattedDescription(acc, desc, ItemID.BandofRegeneration);
			case "2":
				return Get_FormattedDescription(acc, desc, ItemID.FrozenTurtleShell);
			case "3":
				return Get_FormattedDescription(acc, desc, ItemID.CobaltShield);
			default:
				return desc;
		}
	}
	public override void OnHitByNPC(Player player, AugmentsWeapon acc, NPC npc, Player.HurtInfo info) {
		if (Main.rand.NextBool(3)) {
			player.Heal((int)Math.Ceiling(player.statLifeMax2 * .05f));
		}
		if (acc.AugmentUpgrade.Contains(ItemID.BandofRegeneration) && Main.rand.NextFloat() <= .15f && !player.HasBuff<ReactiveHealingBuff>()) {
			player.AddBuff(ModContent.BuffType<ReactiveHealingBuff>(), ModUtils.ToSecond(Main.rand.Next(4, 11)));
		}
		if (acc.AugmentUpgrade.Contains(ItemID.FrozenTurtleShell) && Main.rand.NextBool(4) && !player.HasBuff<ReactiveDefenseBuff>()) {
			player.AddBuff(ModContent.BuffType<ReactiveDefenseBuff>(), ModUtils.ToSecond(Main.rand.Next(4, 11)));
		}
		if (acc.AugmentUpgrade.Contains(ItemID.CobaltShield) && Main.rand.NextBool(10) && !player.HasBuff<ReactiveDefenseIIBuff>()) {
			player.AddBuff(ModContent.BuffType<ReactiveDefenseIIBuff>(), ModUtils.ToSecond(Main.rand.Next(4, 11)));
		}
	}
	public override void OnHitByProj(Player player, AugmentsWeapon acc, Projectile projectile, Player.HurtInfo info) {
		if (Main.rand.NextBool(3)) {
			player.Heal((int)Math.Ceiling(player.statLifeMax2 * .05f));
		}
		if (acc.AugmentUpgrade.Contains(ItemID.BandofRegeneration) && Main.rand.NextFloat() <= .15f && !player.HasBuff<ReactiveHealingBuff>()) {
			player.AddBuff(ModContent.BuffType<ReactiveHealingBuff>(), ModUtils.ToSecond(Main.rand.Next(4, 11)));
		}
		if (acc.AugmentUpgrade.Contains(ItemID.FrozenTurtleShell) && Main.rand.NextBool(4) && !player.HasBuff<ReactiveDefenseBuff>()) {
			player.AddBuff(ModContent.BuffType<ReactiveDefenseBuff>(), ModUtils.ToSecond(Main.rand.Next(4, 11)));
		}
		if (acc.AugmentUpgrade.Contains(ItemID.CobaltShield) && Main.rand.NextBool(10) && !player.HasBuff<ReactiveDefenseIIBuff>()) {
			player.AddBuff(ModContent.BuffType<ReactiveDefenseIIBuff>(), ModUtils.ToSecond(Main.rand.Next(4, 11)));
		}
	}
}
public class ReactiveHealingBuff : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.RegenHP, Base: 10);
	}
}

public class ReactiveDefenseBuff : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		player.endurance += .1f;
	}
}
public class ReactiveDefenseIIBuff : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.Defense, 1.1f, Flat: 6);
	}
}
