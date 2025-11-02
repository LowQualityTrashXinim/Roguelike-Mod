using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Roguelike.Contents.Items.Weapon;
using Roguelike.Common.Systems.ArtifactSystem;
using Roguelike.Contents.Transfixion.Artifacts;

using Roguelike.Texture;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;

namespace Roguelike.Contents.Transfixion.Arguments;
public class BerserkI : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.OrangeRed;
	}
	public override void ModifyHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		float percentage = player.statLife / (float)player.statLifeMax2;
		modifiers.SourceDamage += .5f * percentage;
	}
	public override void ModifyHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type && !proj.minion) {
			float percentage = player.statLife / (float)player.statLifeMax2;
			modifiers.SourceDamage += .5f * percentage;
		}
	}
}

public class TrueStatus : ModAugments {
	public override bool ConditionToBeApplied(Player player, Item item, out float Chance) {
		Chance = 0;
		if (player.HeldItem.type == ItemID.TrueExcalibur || player.HeldItem.type == ItemID.TrueNightsEdge) {
			Chance = .2f;
		}
		return true;
	}
	public override void SetStaticDefaults() {
		tooltipColor = Color.Yellow;
	}
	public override void ModifyHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		int damage = (int)(player.GetWeaponDamage(item) * .1f);
		modifiers.FinalDamage.Flat += damage;
	}
	public override void ModifyHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type) {
			int damage = (int)(player.GetWeaponDamage(player.HeldItem) * .1f);
			modifiers.FinalDamage.Flat += damage;
		}
	}
}

public class Terra : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Green;
	}
	public override void OnHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC npc, NPC.HitInfo hitInfo) {
		TerraStrike(player, npc, player.HeldItem, hitInfo);
	}
	public override void OnHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC npc, NPC.HitInfo hitInfo) {
		if (proj.Check_ItemTypeSource(player.HeldItem.type) && !proj.minion) {
			TerraStrike(player, npc, player.HeldItem, hitInfo);
		}
	}
	private static void TerraStrike(Player player, NPC npc, Item item, NPC.HitInfo hitInfo) {
		if (Main.rand.NextFloat() > .05f) {
			return;
		}
		float randomrotation = Main.rand.NextFloat(90);
		Vector2 randomPosOffset = Main.rand.NextVector2Circular(20f, 20f);
		for (int i = 0; i < 4; i++) {
			Vector2 Toward = Vector2.UnitX.RotatedBy(MathHelper.ToRadians(90 * i + randomrotation)) * (3 + Main.rand.NextFloat());
			for (int l = 0; l < 8; l++) {
				float multiplier = Main.rand.NextFloat();
				float scale = MathHelper.Lerp(1.5f, .1f, multiplier);
				int dust = Dust.NewDust(npc.Center + randomPosOffset, 0, 0, DustID.Terra, 0, 0, 0, default, scale);
				Main.dust[dust].velocity = Toward * multiplier;
				Main.dust[dust].noGravity = true;
			}
		}
		NPC.HitModifiers modifier = new NPC.HitModifiers();
		modifier.FinalDamage.Flat = player.GetWeaponDamage(item) * (hitInfo.Crit ? player.GetModPlayer<PlayerStatsHandle>().UpdateCritDamage.ApplyTo(2) : 1);
		modifier.FinalDamage *= 0;
		player.StrikeNPCDirect(npc, modifier.ToHitInfo(1, hitInfo.Crit, hitInfo.Knockback, true));
	}
}

public class TitanI : ModAugments {
	public override bool ConditionToBeApplied(Player player, Item item, out float Chance) {
		Chance = 0;
		if (player.HeldItem.knockBack > 10) {
			Chance = .2f;
		}
		return true;
	}
	public override void SetStaticDefaults() {
		tooltipColor = Color.Blue;
	}
	public override void ModifyHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		int damage = (int)player.GetWeaponKnockback(item);
		modifiers.SourceDamage.Base += damage;
	}
	public override void ModifyHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type) {
			int damage = (int)player.GetWeaponKnockback(player.HeldItem);
			modifiers.SourceDamage.Base += damage;
		}
	}
}

public class TitanII : ModAugments {
	public override bool ConditionToBeApplied(Player player, Item item, out float Chance) {
		Chance = 0;
		if (player.HeldItem.knockBack > 10) {
			Chance = .2f;
		}
		return true;
	}
	public override void SetStaticDefaults() {

		tooltipColor = Color.Blue;
	}
	public override void ModifyHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		int knockbackStrength = (int)(player.GetWeaponDamage(item) * .05f);
		modifiers.Knockback += knockbackStrength;
	}
	public override void ModifyHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type) {
			int knockbackStrength = (int)(player.GetWeaponDamage(player.HeldItem) * .05f);
			modifiers.Knockback += knockbackStrength;
		}
	}
}


public class AlchemistI : ModAugments {
	public override bool ConditionToBeApplied(Player player, Item item, out float Chance) {
		Chance = 0;
		if (Artifact.PlayerCurrentArtifact<AlchemistKnowledgeArtifact>()) {
			Chance += .1f;
		}
		return true;
	}
	public override void SetStaticDefaults() {
		tooltipColor = Color.BlueViolet;
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, int index, Item item) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.DebuffDamage, 1.06f);
	}
}

public class AlchemistII : ModAugments {
	public override bool ConditionToBeApplied(Player player, Item item, out float Chance) {
		Chance = 0;
		if (Artifact.PlayerCurrentArtifact<AlchemistKnowledgeArtifact>()) {
			Chance += .1f;
		}
		return true;
	}
	public override void SetStaticDefaults() {
		tooltipColor = Color.BlueViolet;
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, int index, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.RegenHP, Base: player.BuffAmount());
	}
}

public class Light : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Pink;
	}
	public override void ModifyHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.GetLifePercent() > .8f)
			modifiers.SourceDamage += 1.5f;
	}
	public override void ModifyHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.GetLifePercent() > .8f)
			modifiers.SourceDamage += 1.5f;
	}
}

public class Dark : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Purple;
	}
	public override void ModifyHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.GetLifePercent() < .4f)
			modifiers.SourceDamage += 1.5f;
	}
	public override void ModifyHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.GetLifePercent() < .4f)
			modifiers.SourceDamage += 1.5f;
	}
}

public class Union : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Bisque;
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, int index, Item item) {
		float damageIncreasement = 0;
		for (int i = 0; player.inventory.Length > 0; i++) {
			if (i > 50) {
				break;
			}
			Item invitem = player.inventory[i];
			if (!invitem.IsAWeapon() || invitem == item || item.ModItem is SynergyModItem) {
				continue;
			}
			damageIncreasement += .5f;
		}
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.PureDamage, Flat: damageIncreasement);
	}
}
public class Strengthen : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.IndianRed;
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, int index, Item item) {
		PlayerStatsHandle stathandle = player.ModPlayerStats();
		stathandle.AddStatsToPlayer(PlayerStats.PureDamage, 1.03f);
		stathandle.AddStatsToPlayer(PlayerStats.CritDamage, 1.06f);
		stathandle.AddStatsToPlayer(PlayerStats.Defense, Base: 2);
		stathandle.AddStatsToPlayer(PlayerStats.MaxHP, Base: 5);
		stathandle.AddStatsToPlayer(PlayerStats.MaxMana, Base: 5);
		stathandle.AddStatsToPlayer(PlayerStats.RegenHP, Base: 1);
		stathandle.AddStatsToPlayer(PlayerStats.CritChance, Base: 1);
	}
}

public class Ghost : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Wheat;
	}
	public override void OnHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC npc, NPC.HitInfo hitInfo) {
		if (!player.immune) {
			player.immune = true;
			player.AddImmuneTime(-1, 8);
		}
	}
}

public class ExtraLife : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.White;
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, int index, Item item) {
		player.GetModPlayer<PlayerStatsHandle>().Add_ExtraLifeWeapon(item);
	}
}

public class IntoxicateI : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.GreenYellow;
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, int index, Item item) {
		for (int i = 0; i < player.buffType.Length; i++) {
			if (player.buffType[i] == 0) continue;
			if (Main.debuff[player.buffType[i]]) {
				player.endurance += .1f;
			}
		}
	}
}

public class IntoxicateII : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.GreenYellow;
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, int index, Item item) {
		for (int i = 0; i < player.buffType.Length; i++) {
			if (player.buffType[i] == 0) continue;
			if (Main.debuff[player.buffType[i]]) {
				PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.RegenHP, Additive: 1.15f, Flat: 5);
			}
		}
	}
}
public class ReactiveHealingI : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.ForestGreen;
	}
	public override void OnHitByNPC(Player player, AugmentsWeapon acc, int index, NPC npc, Player.HurtInfo info) {
		if (Main.rand.NextBool(3)) {
			player.Heal((int)Math.Ceiling(player.statLifeMax2 * .05f));
		}
	}
	public override void OnHitByProj(Player player, AugmentsWeapon acc, int index, Projectile projectile, Player.HurtInfo info) {
		if (Main.rand.NextBool(3)) {
			player.Heal((int)Math.Ceiling(player.statLifeMax2 * .05f));
		}
	}
}
public class ReactiveHealingII : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.ForestGreen;
	}
	public override void OnHitByNPC(Player player, AugmentsWeapon acc, int index, NPC npc, Player.HurtInfo info) {
		if (Main.rand.NextBool(3) && !player.HasBuff<ReactiveHealingBuff>()) {
			player.AddBuff(ModContent.BuffType<ReactiveHealingBuff>(), ModUtils.ToSecond(Main.rand.Next(4, 11)));
		}
	}
	public override void OnHitByProj(Player player, AugmentsWeapon acc, int index, Projectile projectile, Player.HurtInfo info) {
		if (Main.rand.NextBool(3) && !player.HasBuff<ReactiveHealingBuff>()) {
			player.AddBuff(ModContent.BuffType<ReactiveHealingBuff>(), ModUtils.ToSecond(Main.rand.Next(4, 11)));
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

public class ReactiveDefenseI : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.MediumPurple;
	}
	public override void OnHitByNPC(Player player, AugmentsWeapon acc, int index, NPC npc, Player.HurtInfo info) {
		if (Main.rand.NextBool(4) && !player.HasBuff<ReactiveDefenseBuff>()) {
			player.AddBuff(ModContent.BuffType<ReactiveDefenseBuff>(), ModUtils.ToSecond(Main.rand.Next(4, 11)));
		}
	}
	public override void OnHitByProj(Player player, AugmentsWeapon acc, int index, Projectile projectile, Player.HurtInfo info) {
		if (Main.rand.NextBool(4) && !player.HasBuff<ReactiveDefenseBuff>()) {
			player.AddBuff(ModContent.BuffType<ReactiveDefenseBuff>(), ModUtils.ToSecond(Main.rand.Next(4, 11)));
		}
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
public class ReactiveDefenseII : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.MediumPurple;
	}
	public override void OnHitByNPC(Player player, AugmentsWeapon acc, int index, NPC npc, Player.HurtInfo info) {
		if (Main.rand.NextBool(4) && !player.HasBuff<ReactiveDefenseIIBuff>()) {
			player.AddBuff(ModContent.BuffType<ReactiveDefenseIIBuff>(), ModUtils.ToSecond(Main.rand.Next(4, 11)));
		}
	}
	public override void OnHitByProj(Player player, AugmentsWeapon acc, int index, Projectile projectile, Player.HurtInfo info) {
		if (Main.rand.NextBool(4) && !player.HasBuff<ReactiveDefenseIIBuff>()) {
			player.AddBuff(ModContent.BuffType<ReactiveDefenseIIBuff>(), ModUtils.ToSecond(Main.rand.Next(4, 11)));
		}
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

public class VitalityStrikeI : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.PaleVioletRed;
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, int index, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.PureDamage, 1 + player.statLifeMax2 * .0005f);
	}
}
public class VitalityStrikeII : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.PaleVioletRed;
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, int index, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.CritChance, Base: player.statLifeMax2 * .01f);
	}
}

public class ArcaneStrikeI : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.DarkBlue;
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, int index, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.PureDamage, 1 + player.statManaMax2 * .0005f);
	}
}
public class ArcaneStrikeII : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.DarkBlue;
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, int index, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.CritChance, Base: player.statManaMax2 * .01f);
	}
}

public class StealthStrikeI : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.DarkGray;
	}
	public override void ModifyHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (player.invis)
			modifiers.SourceDamage += .25f;
	}
	public override void ModifyHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.Check_ItemTypeSource(player.HeldItem.type)) {
			if (player.invis)
				modifiers.SourceDamage += .25f;
		}
	}
}
public class StealthStrikeII : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.DarkGray;
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, int index, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.FullHPDamage, 2f);
	}
}
public class DryadBlessing : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.LimeGreen;
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, int index, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.RegenHP, Base: 3);
	}
	public override void OnHitByProj(Player player, AugmentsWeapon acc, int index, Projectile projectile, Player.HurtInfo info) {
		if (Main.rand.NextFloat() <= Main.rand.NextFloat(.1f, .4f) && !player.HasBuff<DryadBlessing_Buff>()) {
			player.AddBuff(ModContent.BuffType<DryadBlessing_Buff>(), ModUtils.ToSecond(Main.rand.Next(3, 8)));
		}
	}
	public override void OnHitByNPC(Player player, AugmentsWeapon acc, int index, NPC npc, Player.HurtInfo info) {
		if (Main.rand.NextFloat() <= Main.rand.NextFloat(.1f, .4f) && !player.HasBuff<DryadBlessing_Buff>()) {
			player.AddBuff(ModContent.BuffType<DryadBlessing_Buff>(), ModUtils.ToSecond(Main.rand.Next(3, 8)));
		}
	}
}
public class DryadBlessing_Buff : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle statplayer = player.GetModPlayer<PlayerStatsHandle>();
		statplayer.AddStatsToPlayer(PlayerStats.Defense, Base: 8);
		statplayer.AddStatsToPlayer(PlayerStats.RegenHP, Base: 5);
	}
}
