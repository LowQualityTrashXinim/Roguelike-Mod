using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.Systems;
using Roguelike.Common.Utils;
using Roguelike.Contents.BuffAndDebuff;
using Roguelike.Contents.Items;
using Roguelike.Contents.Items.BuilderItem;
using Roguelike.Contents.Items.Weapon;
using Roguelike.Contents.Projectiles;
using Roguelike.Contents.Skill;
using Roguelike.Texture;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System.Collections.Generic;
using Roguelike.Contents.Perks.BlessingPerk;

namespace Roguelike.Contents.Perks;
public class MarkOfSpectre : Perk {
	public override void SetDefaults() {
		textureString = ModUtils.GetTheSameTextureAsEntity<MarkOfSpectre>();
		CanBeStack = false;
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.MovementSpeed, 1.35f);
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.JumpBoost, 1.65f);
	}
	public override void ModifyHitByNPC(Player player, NPC npc, ref Player.HurtModifiers modifiers) {
		ModifyHit(ref modifiers);
	}
	public override void ModifyHitByProjectile(Player player, Projectile proj, ref Player.HurtModifiers modifiers) {
		ModifyHit(ref modifiers);
	}
	private static void ModifyHit(ref Player.HurtModifiers modifiers) {
		modifiers.FinalDamage += .25f;
		modifiers.Knockback *= .35f;
	}
	public override bool FreeDodge(Player player, Player.HurtInfo hurtInfo) {
		if (!player.immune && Main.rand.NextFloat() <= .6f) {
			player.AddImmuneTime(hurtInfo.CooldownCounter, 60);
			player.immune = true;
			return true;
		}
		return base.FreeDodge(player, hurtInfo);
	}
}
public class LethalKnockBack : Perk {
	public override void SetDefaults() {
		textureString = ModUtils.GetTheSameTextureAsEntity<LethalKnockBack>();
		list_category.Add(PerkCategory.WeaponUpgrade);
		CanBeStack = false;
	}
	public override void ModifyKnockBack(Player player, Item item, ref StatModifier knockback) {
		if (item.DamageType == DamageClass.Melee) {
			knockback += .15f;
		}
	}
	public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
		damage -= .11f;
	}
	public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		modifiers.SourceDamage += item.knockBack * .1f * Math.Clamp(Math.Abs(target.knockBackResist - 1), 0, 3f);
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		modifiers.SourceDamage += proj.knockBack * .1f * Math.Clamp(Math.Abs(target.knockBackResist - 1), 0, 3f);
	}
}
public class PowerUp : Perk {
	public override void SetDefaults() {
		textureString = ModUtils.GetTheSameTextureAsEntity<PowerUp>();
		CanBeStack = true;
		StackLimit = 3;
	}
	public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
		damage += .25f * StackAmount(player);
	}
	public override void ModifyCriticalStrikeChance(Player player, Item item, ref float crit) {
		crit += 10 * StackAmount(player);
	}
	public override void ModifyUseSpeed(Player player, Item item, ref float useSpeed) {
		useSpeed -= .25f + .1f * StackAmount(player);
	}
}
public class LifeForceOrb : Perk {
	public override void SetDefaults() {
		textureString = ModUtils.GetTheSameTextureAsEntity<LifeForceOrb>();
		CanBeStack = false;
	}
	public override void OnHitNPCWithItem(Player player, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		LifeForceSpawn(player, target);
	}
	public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		LifeForceSpawn(player, target);
	}
	private static void LifeForceSpawn(Player player, NPC target) {
		if (Main.rand.NextBool(10))
			Projectile.NewProjectile(player.GetSource_FromThis(), target.Center + Main.rand.NextVector2Circular(target.width + 100, target.height + 100), Vector2.Zero, ModContent.ProjectileType<LifeOrb>(), 0, 0, player.whoAmI);
	}
}
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
}
public class BackUpMana_CoolDown : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff(true);
	}
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.RegenMana, -.5f);
	}
}
public class Dirt : Perk {
	public override void SetDefaults() {
		CanBeStack = false;
		textureString = ModUtils.GetTheSameTextureAsEntity<Dirt>();
	}
	public override void UpdateEquip(Player player) {
		PlayerStatsHandle handle = player.GetModPlayer<PlayerStatsHandle>();
		handle.AugmentationChance += .1f * StackAmount(player);
		handle.RandomizeChanceEnchantment += .1f * StackAmount(player);
		handle.ChanceDropModifier += .1f * StackAmount(player);
		handle.DropModifier.Base += 1;
	}
}
public class SelfExplosion : Perk {
	public override void SetDefaults() {
		textureString = ModUtils.GetTheSameTextureAsEntity<SelfExplosion>();
		CanBeStack = true;
		StackLimit = 2;
	}
	public override void OnHitByAnything(Player player) {
		player.Center.LookForHostileNPC(out List<NPC> npclist, 500);
		foreach (NPC npc in npclist) {
			int direction = player.Center.X - npc.Center.X > 0 ? -1 : 1;
			npc.StrikeNPC(npc.CalculateHitInfo((120 + player.statLife) * StackAmount(player), direction, false, 10));
		}
		for (int i = 0; i < 150; i++) {
			int smokedust = Dust.NewDust(player.Center, 0, 0, DustID.Smoke);
			Main.dust[smokedust].noGravity = true;
			Main.dust[smokedust].velocity = Main.rand.NextVector2Circular(500 / 12f, 500 / 12f);
			Main.dust[smokedust].scale = Main.rand.NextFloat(.75f, 2f);
			int dust = Dust.NewDust(player.Center, 0, 0, DustID.Torch);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = Main.rand.NextVector2Circular(500 / 12f, 500 / 12f);
			Main.dust[dust].scale = Main.rand.NextFloat(.75f, 2f);
		}
		player.AddBuff(ModContent.BuffType<ExplosionHealing>(), ModUtils.ToSecond(5 + StackAmount(player)));
	}
	class ExplosionHealing : ModBuff {
		public override string Texture => ModTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultBuff();
		}
		public override void Update(Player player, ref int buffIndex) {
			player.lifeRegen += 22;
		}
	}
}
public class ProjectileProtection : Perk {
	public override void SetDefaults() {
		textureString = ModUtils.GetTheSameTextureAsEntity<ProjectileProtection>();
		CanBeStack = true;
		StackLimit = 3;
	}
	public override void UpdateEquip(Player player) {
		player.endurance += .05f * StackAmount(player);
	}
	public override void ModifyHitByProjectile(Player player, Projectile proj, ref Player.HurtModifiers modifiers) {
		modifiers.SourceDamage += -.3f * StackAmount(player);
	}
}
public class ProjectileDuplication : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 3;
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		modifiers.SourceDamage -= (StackLimit - StackAmount(player) + 1) * .15f;
	}
	public override void Shoot(Player player, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (type != ModContent.ProjectileType<ArenaMakerProj>()
			|| type == ModContent.ProjectileType<NeoDynamiteExplosion>()
			|| type == ModContent.ProjectileType<TowerDestructionProjectile>()
			|| !ContentSamples.ProjectilesByType[type].minion) {
			player.GetModPlayer<PlayerStatsHandle>().Request_ShootExtra(StackAmount(player), 5 + 5 * StackAmount(player));
		}
	}
}
public class ScatterShot : Perk {
	public override void SetDefaults() {
		textureString = ModUtils.GetTheSameTextureAsEntity<ScatterShot>();
		CanBeStack = false;
	}
	public override void ResetEffect(Player player) {
		player.GetModPlayer<PerkPlayer>().perk_ScatterShot = true;
	}
}
public class BloodStrike : Perk {
	public override void SetDefaults() {
		textureString = ModUtils.GetTheSameTextureAsEntity<BloodStrike>();
		CanBeStack = false;
	}
	public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
		damage *= 1.36f;
		damage.Flat += 7;
	}
	public override void OnUseItem(Player player, Item item) {
		if (item.IsAWeapon() && player.itemAnimation == player.itemAnimationMax && player.ItemAnimationActive) {
			int damage = (int)Math.Round(player.GetWeaponDamage(player.HeldItem) * .05f);
			player.statLife = Math.Clamp(player.statLife - damage, 0, player.statLifeMax2);
			ModUtils.CombatTextRevamp(player.Hitbox, Color.Red, "-" + damage, Main.rand.Next(-10, 40));
		}
	}
}
public class SpeedArmor : Perk {
	public override void SetDefaults() {
		textureString = ModUtils.GetTheSameTextureAsEntity<SpeedArmor>();
		CanBeStack = true;
		StackLimit = 2;
	}
	public override void UpdateEquip(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.MovementSpeed, Additive: 1 + .45f * StackAmount(player));
		modplayer.AddStatsToPlayer(PlayerStats.JumpBoost, Additive: 1 + .2f * StackAmount(player));
		modplayer.AddStatsToPlayer(PlayerStats.Defense, Base: (int)Math.Round(player.velocity.Length()) * StackAmount(player));
	}
	public override void PostUpdateRun(Player player) {
		player.runAcceleration += .5f;
		player.runSlowdown += .25f;
	}
}
public class CelestialRage : Perk {
	public override void SetDefaults() {
		CanBeStack = false;
	}
	public override void OnChoose(Player player) {
		player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<CelestialWrath>());
	}
	public override void UpdateEquip(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.CritDamage, 2f);
		modplayer.AddStatsToPlayer(PlayerStats.CritChance, Base: 5);
		modplayer.AddStatsToPlayer(PlayerStats.PureDamage, Multiplicative: 1.1f);
	}
}
public class ArenaBlessing : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 4;
	}
	public override string ModifyToolTip() {
		int stack = StackAmount(Main.LocalPlayer);
		if (stack > 0) {
			return ModUtils.LocalizationText("ModPerk", $"{Name}.Description{stack}");
		}
		return ModUtils.LocalizationText("ModPerk", $"{Name}.Description");
	}
	public override void Update(Player player) {
		if (player.ownedProjectileCounts[ModContent.ProjectileType<AdventureSpirit>()] < 1) {
			Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<AdventureSpirit>(), 0, 0, player.whoAmI);
		}
	}
}
public class StellarRetirement : Perk {
	public override void SetDefaults() {
		CanBeStack = false;
		textureString = ModTexture.ACCESSORIESSLOT;
	}
	public override void Update(Player player) {
		if (Main.rand.NextBool(200)) {
			int damage = (int)player.GetDamage(DamageClass.Generic).ApplyTo(1000);
			int proj = Projectile.NewProjectile(Entity.GetSource_NaturalSpawn(), player.Center + new Vector2(Main.rand.NextFloat(-1000, 1000), -1500), (Vector2.UnitY * 15).Vector2RotateByRandom(25), ProjectileID.SuperStar, damage, 5);
			Main.projectile[proj].tileCollide = false;
			Main.projectile[proj].timeLeft = ModUtils.ToSecond(3);
		}
	}
}
public class OverchargedMana : Perk {
	public override void SetDefaults() {
		CanBeStack = false;
		textureString = ModTexture.ACCESSORIESSLOT;
	}
	public override void UpdateEquip(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.RegenMana, Base: 20);
		if (player.statMana <= player.statManaMax2 / 2) {
			modplayer.AddStatsToPlayer(PlayerStats.RegenMana, Base: 40);
		}
		if (player.statMana > player.statLife) {
			modplayer.AddStatsToPlayer(PlayerStats.MagicDMG, 1.25f);
		}
	}
	public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
		if (player.statMana >= player.statManaMax2 * .9f && item.DamageType == DamageClass.Magic) {
			damage += 0.77f;
		}
	}
	public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (hit.DamageType == DamageClass.Magic && player.statMana == player.statLifeMax2) {
			target.Center.LookForHostileNPC(out List<NPC> npclist, 200);
			for (int i = 0; i < 65; i++) {
				var d = Dust.NewDust(target.Center + Main.rand.NextVector2CircularEdge(64, 64), 0, 0, DustID.BlueTorch);
				Main.dust[d].noGravity = true;
			}
			float damage = .15f;
			if (player.statMana > player.statLife) {
				damage += .3f;
			}
			foreach (var i in npclist) {
				player.StrikeNPCDirect(target, i.CalculateHitInfo(5 + (int)(proj.damage * damage), 1, Main.rand.NextBool(10)));
			}
		}
	}
}
public class BeyondCritcal : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 3;
	}
	public override void Update(Player player) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.CritChance, Base: 5 * StackAmount(player));
	}
	public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (Main.rand.NextFloat() <= .04f * StackAmount(player)) {
			modifiers.FinalDamage *= 2.5f;
			modifiers.ScalingArmorPenetration += 0.9f;
		}
		else if (player.GetModPlayer<PlayerStatsHandle>().ModifyHit_Before_Crit && Main.rand.NextFloat() <= .15f * StackAmount(player)) {
			modifiers.FinalDamage *= 2.5f;
			modifiers.ScalingArmorPenetration += 0.9f;
		}
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (Main.rand.NextFloat() <= .04f * StackAmount(player)) {
			modifiers.FinalDamage *= 2.5f;
			modifiers.ScalingArmorPenetration += 0.9f;
		}
		else if (player.GetModPlayer<PlayerStatsHandle>().ModifyHit_Before_Crit && Main.rand.NextFloat() <= .15f * StackAmount(player)) {
			modifiers.FinalDamage *= 2.5f;
			modifiers.ScalingArmorPenetration += 0.9f;
		}
	}
}
public class SummonBuffPerk : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 4;
	}
	public override void Update(Player player) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.MaxMinion, Base: 2 * StackAmount(player));
	}
	public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (hit.DamageType != DamageClass.Summon) {
			hit.SourceDamage -= (int)(hit.SourceDamage * 0.05f) * StackAmount(player);
		}
	}
}
public class TrueMeleeBuffPerk : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 2;
		list_category.Add(PerkCategory.WeaponUpgrade);
	}
	public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (modifiers.DamageType == DamageClass.Melee && item.shoot == ProjectileID.None) {
			modifiers.DamageVariationScale *= 0f;
			modifiers.SourceDamage += .25f * StackAmount(player);
			if (!player.immune) {
				modifiers.SourceDamage += .44f;
			}
		}
	}
}
public class KnockpackPerk : Perk {
	public override void SetDefaults() {
		CanBeStack = false;
	}
	public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		modifiers.HitDirectionOverride = 0;
		modifiers.Knockback *= 2;
		if (target.knockBackResist >= 1f) {
			modifiers.Defense.Base -= player.GetWeaponKnockback(item);
		}
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		modifiers.HitDirectionOverride = 0;
		modifiers.Knockback *= 2;
		if (target.knockBackResist >= 1f) {
			modifiers.Defense.Base -= proj.knockBack;
		}
	}
}
public class ImprovedPotion : Perk {
	public override void SetDefaults() {
		CanBeStack = false;
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().BuffTime += .45f;
		player.GetModPlayer<PerkPlayer>().perk_ImprovedPotion = true;
	}
}
public class WeaponExpert : Perk {
	public override void SetDefaults() {
		CanBeStack = false;
		textureString = ModTexture.ACCESSORIESSLOT;
		list_category.Add(PerkCategory.WeaponUpgrade);
	}
	public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
		if (item.damage <= 20 || Main.hardMode && item.damage <= 42) {
			damage += 1;
		}
	}
}
public class AspectOfTheUnderworld : Perk {
	public override void SetDefaults() {
		CanBeStack = false;
		textureString = ModTexture.ACCESSORIESSLOT;
		DataStorer.AddContext("Perk_RingOfFire", new(
			300,
			Vector2.Zero,
			false,
			Color.DarkRed
			));
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.Defense, 1.15f, 1, 10);
		if (player.IsHealthAbovePercentage(.66f)) {
			player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.RegenHP, Flat: -32);
		}
		player.buffImmune[BuffID.OnFire] = true;
	}
	public override void Update(Player player) {
		DataStorer.ActivateContext(player, "Perk_RingOfFire");
		player.Center.LookForHostileNPC(out List<NPC> npclist, 300);
		for (int i = 0; i < 4; i++) {
			int dust = Dust.NewDust(player.Center + Main.rand.NextVector2Circular(300, 300), 0, 0, DustID.Torch);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = -Vector2.UnitY * 4f;
			Main.dust[dust].scale = Main.rand.NextFloat(.75f, 2f);
		}
		if (npclist.Count > 0) {
			foreach (NPC npc in npclist) {
				npc.AddBuff(BuffID.OnFire, 180);
				if (player.IsHealthAbovePercentage(.67f)) {
					npc.AddBuff(BuffID.OnFire3, 180);
					npc.AddBuff(ModContent.BuffType<TheUnderworldWrath>(), 180);
				}
			}
		}
	}
	public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.OnFire) || target.HasBuff(BuffID.OnFire3)) {
			modifiers.CritDamage *= 1.15f;
		}
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.OnFire) || target.HasBuff(BuffID.OnFire3)) {
			modifiers.CritDamage *= 1.15f;
		}
	}
}
public class AspectOfFirstChaos : Perk {
	public override bool SelectChoosing() {
		return Main.LocalPlayer.HasPerk<ChaosProtection>();
	}
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 2;
		textureString = ModTexture.ACCESSORIESSLOT;
	}
	public override string ModifyToolTip() {
		if (StackAmount(Main.LocalPlayer) >= 1) {
			return ModUtils.LocalizationText("ModPerk", $"{Name}.Description1");
		}
		return base.ModifyToolTip();
	}
	public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (StackAmount(player) >= 1) {
			var globalproj = proj.GetGlobalProjectile<RoguelikeGlobalProjectile>();
			if (globalproj.Source_CustomContextInfo == "AspectOfFirstChaos") {
				if (!Main.rand.NextBool(4)) {
					Vector2 newPos = ModUtils.SpawnRanPositionThatIsNotIntoTile(target.Center, 300, 300);
					Projectile.NewProjectile(proj.GetSource_FromAI(), newPos, (target.Center - newPos).SafeNormalize(Vector2.Zero) * Main.rand.Next(10, 15), Main.rand.Next(TerrariaArrayID.UltimateProjPack), proj.damage, proj.knockBack, player.whoAmI);
				}
			}
		}
		int randcount = 1 + StackAmount(player);
		for (int i = 0; i < randcount; i++) {
			if (Main.rand.NextBool(5)) {
				randcount++;
				player.StrikeNPCDirect(target, target.CalculateHitInfo(proj.damage, 0));
			}
		}
		bool Opportunity = Main.rand.NextBool(10);
		int[] debuffArray =
			{ BuffID.OnFire, BuffID.OnFire3, BuffID.Bleeding, BuffID.Frostburn, BuffID.Frostburn2, BuffID.ShadowFlame,
				BuffID.CursedInferno, BuffID.Ichor, BuffID.Venom, BuffID.Poisoned, BuffID.Confused, BuffID.Midas };
		if (debuffArray.Where(d => !target.HasBuff(d)).Count() >= debuffArray.Length)
			return;
		for (int i = 0; i < debuffArray.Length; i++) {
			if (Opportunity && !target.HasBuff(debuffArray[i])) {
				target.AddBuff(debuffArray[i], 1800);
				break;
			}
			else {
				if (!Opportunity)
					Opportunity = Main.rand.NextBool(10);
			}
			if (i == debuffArray.Length - 1 && Opportunity)
				i = 0;
		}
	}
	public override void OnHitNPCWithItem(Player player, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		int randcount = 1 + StackAmount(player);
		for (int i = 0; i < randcount; i++) {
			if (Main.rand.NextBool(5)) {
				randcount++;
				player.StrikeNPCDirect(target, target.CalculateHitInfo(item.damage, 0));
			}
		}
		if (!Main.rand.NextBool(4)) {
			Projectile.NewProjectile(player.GetSource_ItemUse(item, "AspectOfFirstChaos"), player.Center, (target.Center - player.Center).SafeNormalize(Vector2.Zero) * 4, Main.rand.Next(TerrariaArrayID.UltimateProjPack), item.damage, item.knockBack, player.whoAmI);
		}
		bool Opportunity = Main.rand.NextBool(10);
		int[] debuffArray =
			{ BuffID.OnFire, BuffID.OnFire3, BuffID.Bleeding, BuffID.Frostburn, BuffID.Frostburn2, BuffID.ShadowFlame,
				BuffID.CursedInferno, BuffID.Ichor, BuffID.Venom, BuffID.Poisoned, BuffID.Confused, BuffID.Midas };
		if (debuffArray.Where(d => !target.HasBuff(d)).Count() >= debuffArray.Length)
			return;
		for (int i = 0; i < debuffArray.Length; i++) {
			if (Opportunity && !target.HasBuff(debuffArray[i])) {
				target.AddBuff(debuffArray[i], 1800);
				break;
			}
			else {
				if (!Opportunity)
					Opportunity = Main.rand.NextBool(10);
			}
			if (i == debuffArray.Length - 1 && Opportunity)
				i = 0;
		}
	}
	public override void Shoot(Player player, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (!Main.rand.NextBool(Math.Clamp(4 - StackAmount(player), 1, 4))) {
			return;
		}
		EntitySource_ItemUse_WithAmmo newsource = new EntitySource_ItemUse_WithAmmo(source.Player, source.Item, source.AmmoItemIdUsed, "AspectOfFirstChaos");
		Projectile.NewProjectile(newsource, position, velocity, Main.rand.Next(TerrariaArrayID.UltimateProjPack), damage, knockback, player.whoAmI);
	}
}
public class EnergyAbsorption : Perk {
	public override void SetDefaults() {
		CanBeStack = false;
	}
	public override void UpdateEquip(Player player) {
		PlayerStatsHandle statplayer = player.GetModPlayer<PlayerStatsHandle>();
		statplayer.EnergyRegen.Base += 1;
		statplayer.AddStatsToPlayer(PlayerStats.EnergyCap, 1.2f);
		SkillHandlePlayer modplayer = player.GetModPlayer<SkillHandlePlayer>();
		if (modplayer.Activate) {
			player.endurance += .1f;
			statplayer.AddStatsToPlayer(PlayerStats.PureDamage, 1.11f);
		}
	}
	public override void OnHitByNPC(Player player, NPC npc, Player.HurtInfo hurtInfo) {
		player.GetModPlayer<SkillHandlePlayer>().Modify_EnergyAmount((int)(hurtInfo.Damage * .5f));
	}
}
public class HybridRanger : Perk {
	public override void SetDefaults() {
		CanBeStack = false;
	}
	public override void UpdateEquip(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.RangeDMG, Additive: 1.1f);
		modplayer.AddStatsToPlayer(PlayerStats.MaxMinion, Base: 1);
		modplayer.AddStatsToPlayer(PlayerStats.MaxSentry, Base: 1);
	}
	public override void Shoot(Player player, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (item.DamageType != DamageClass.Ranged) {
			return;
		}
		int amount = (player.maxMinions + player.maxTurrets) / 2;
		for (int i = 0; i < amount; i++) {
			if (!Main.rand.NextBool(10)) {
				continue;
			}
			Vector2 pos = position + Main.rand.NextVector2CircularEdge(Main.rand.NextFloat(300, 400), Main.rand.NextFloat(300, 400));
			for (int l = 0; l < 20; l++) {
				Dust dust = Dust.NewDustDirect(pos, 0, 0, DustID.SpectreStaff);
				dust.noGravity = true;
				dust.velocity = Main.rand.NextVector2Circular(5, 5);
				dust.scale = Main.rand.NextFloat(.9f, 2.25f);
			}
			Projectile proj = Projectile.NewProjectileDirect(source, pos,
				Vector2.One.Vector2RotateByRandom(180), ProjectileID.SpectreWrath, damage, knockback, player.whoAmI);
			proj.timeLeft = 1800;
			proj.extraUpdates = 6;
		}
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.minion || proj.DamageType == DamageClass.Summon) {
			if (Main.rand.Next(1, 101) <= player.GetTotalCritChance<RangedDamageClass>()) {
				modifiers.SetCrit();
			}
		}
	}
}
public class OathOfSword : Perk {
	public override void SetDefaults() {
		CanBeStack = false;
	}
	public override void UpdateEquip(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		if (ModUtils.IsAVanillaSword(player.HeldItem.type)
			|| player.HeldItem.CheckUseStyleMelee(ModUtils.MeleeStyle.CheckVanillaSwingWithModded)
			&& player.HeldItem.DamageType == DamageClass.Melee) {
			modplayer.AddStatsToPlayer(PlayerStats.MeleeDMG, 2f, 1.11f);
		}
		else {
			modplayer.AddStatsToPlayer(PlayerStats.MagicDMG, .45f);
			modplayer.AddStatsToPlayer(PlayerStats.RangeDMG, .45f);
			modplayer.AddStatsToPlayer(PlayerStats.SummonDMG, .45f);
		}
	}
}
public class TitanPower : Perk {
	public override void SetDefaults() {
		CanBeStack = false;
	}
	public override bool SelectChoosing() {
		Player player = Main.LocalPlayer;
		PerkPlayer perkplayer = player.GetModPlayer<PerkPlayer>();
		if (perkplayer.perks.ContainsKey(GetPerkType<BlessingOfTitan>())
			&& perkplayer.perks.ContainsKey(GetPerkType<ProjectileProtection>())) {
			return true;
		}
		return false;
	}
	public override void ResetEffect(Player player) {
		PlayerStatsHandle.SetSecondLifeCondition(player, "P_TP", !player.HasBuff(ModContent.BuffType<TitanPowerBuff>()));
	}
	public override void UpdateEquip(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.MaxHP, Base: 200);
		modplayer.AddStatsToPlayer(PlayerStats.Thorn, Base: 1.5f);
		modplayer.AddStatsToPlayer(PlayerStats.Defense, Additive: 1.25f, Flat: 15);
		player.endurance += .4f;
	}
	public override bool PreKill(Player player) {
		if (PlayerStatsHandle.GetSecondLife(player, "P_TP")) {
			player.AddBuff(ModContent.BuffType<TitanPowerBuff>(), ModUtils.ToMinute(4));
			player.Heal(player.statLifeMax2);
			return true;
		}
		return false;
	}
}
public class TitanPowerBuff : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff(true);
	}
}
public class DemolitionistGunner : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 3;
	}
	public override string ModifyToolTip() {
		if (StackAmount(Main.LocalPlayer) >= 2) {
			return DescriptionIndex(1);
		}
		return Description;
	}
	public override void ModifyShootStat(Player player, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		float chance = 0.25f * StackAmount(player);
		if (item.useAmmo == AmmoID.Bullet && type == ProjectileID.Bullet && Main.rand.NextFloat() <= chance) {
			type = ProjectileID.ExplosiveBullet;
		}
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		float chance = 0.1f * StackAmount(player);
		if (proj.type == ProjectileID.ExplosiveBullet && Main.rand.NextFloat() <= chance) {
			modifiers.SourceDamage += .55f;
		}
	}
	public override void Shoot(Player player, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (StackAmount(player) >= 2) {
			if (Main.rand.NextFloat() <= .05f) {
				Vector2 vel = -Vector2.UnitY.RotatedBy(MathHelper.ToRadians(30 * player.direction)) * 15;
				Projectile.NewProjectile(source, position, vel, ModContent.ProjectileType<FriendlyGrenadeProjectile>(), damage * 3, knockback, player.whoAmI);
			}
		}
	}
	public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		float chance = 0.05f * StackAmount(player);
		if (proj.type != ProjectileID.ExplosiveBullet || Main.rand.NextFloat() > chance) {
			return;
		}
		for (int i = 0; i < 25; i++) {
			int smokedust = Dust.NewDust(target.Center, 0, 0, DustID.Smoke);
			Main.dust[smokedust].noGravity = true;
			Main.dust[smokedust].velocity = Main.rand.NextVector2Circular(15f, 15f);
			Main.dust[smokedust].scale = Main.rand.NextFloat(.75f, 2f);
			int dust = Dust.NewDust(target.Center, 0, 0, DustID.Torch);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = Main.rand.NextVector2Circular(15f, 15f);
			Main.dust[dust].scale = Main.rand.NextFloat(.75f, 2f);
		}
		target.Center.LookForHostileNPC(out List<NPC> npclist, 150f);
		foreach (NPC npc in npclist) {
			player.StrikeNPCDirect(npc, hit);
		}
	}
}

public class MindOfBattlefield : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 3;
	}
	public override void UpdateEquip(Player player) {
		PlayerStatsHandle statplayer = player.GetModPlayer<PlayerStatsHandle>();
		int stack = StackAmount(player);
		if (player.HeldItem.useAmmo == AmmoID.Bullet) {
			statplayer.AddStatsToPlayer(PlayerStats.AttackSpeed, 1 + .14f * stack);
		}
		else if (player.HeldItem.useAmmo == AmmoID.Arrow) {
			statplayer.AddStatsToPlayer(PlayerStats.CritChance, Base: 7 * stack);
		}
		else if (player.HeldItem.CheckUseStyleMelee(ModUtils.MeleeStyle.CheckVanillaSwingWithModded)) {
			statplayer.AddStatsToPlayer(PlayerStats.Iframe, 1 + .11f * stack);
		}
		statplayer.AddStatsToPlayer(PlayerStats.MovementSpeed, 1 + .2f * stack);
		statplayer.AddStatsToPlayer(PlayerStats.JumpBoost, 1 + .15f * stack);
		statplayer.AddStatsToPlayer(PlayerStats.StaticDefense, Base: 10 * stack);
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		//Hope this work exactly what I expect it to do
		Projectile sample = ContentSamples.ProjectilesByType[proj.type];
		Rectangle defaultProjSize = new((int)proj.position.X, (int)proj.position.Y, sample.width, sample.height);
		if (ProjectileID.Sets.Explosive[proj.type] && defaultProjSize.Intersects(target.Hitbox)) {
			modifiers.SourceDamage += .5f * StackAmount(player);
		}
	}
}

public class SoulShatter : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 3;
	}
	public override void UpdateEquip(Player player) {
		PlayerStatsHandle handle = player.GetModPlayer<PlayerStatsHandle>();
		handle.AddStatsToPlayer(PlayerStats.FullHPDamage, 6f);
		handle.AddStatsToPlayer(PlayerStats.CritDamage, 1.4f);
		SoulShatter_ModPlayer charged = player.GetModPlayer<SoulShatter_ModPlayer>();
		charged.Perk = true;
		if (charged.Charged) {
			handle.AddStatsToPlayer(PlayerStats.PureDamage, 1.4f);
			handle.AddStatsToPlayer(PlayerStats.CritDamage, 2);
		}
	}
	public override void OnHitNPCWithItem(Player player, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		Chance_InstantKill(player, target, hit);
	}
	public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		Chance_InstantKill(player, target, hit);
	}
	private static void Chance_InstantKill(Player player, NPC target, NPC.HitInfo info) {
		if (player.GetModPlayer<PlayerStatsHandle>().NPC_HitCount == 1) {
			target.Center.LookForHostileNPC(out List<NPC> npclist, 400);
			foreach (NPC npc in npclist) {
				npc.AddBuff(ModContent.BuffType<Shatter>(), ModUtils.ToSecond(Main.rand.Next(10, 17)));
			}
		}
		if (Main.rand.NextFloat() <= 0.0001f) {
			info.InstantKill = true;
			player.StrikeNPCDirect(target, info);
		}
	}
}
public class SoulShatter_ModPlayer : ModPlayer {
	public bool Charged = false;
	public int counter = 0;
	public const int ChargeTime = 150;
	public bool Perk = false;
	public override void ResetEffects() {
		Perk = false;
		Charged = false;
	}
	public override void UpdateEquips() {
		if (!Perk) {
			return;
		}
		if (++counter > ChargeTime) {
			Charged = true;
		}
	}
	public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		Charged = false;
		counter = 0;
		return base.Shoot(item, source, position, velocity, type, damage, knockback);
	}
}
public class UntappedPotential : Perk {
	public override void SetDefaults() {
		CanBeStack = false;
	}
	public override bool SelectChoosing() {
		return Main.LocalPlayer.inventory.Where(i => i.ModItem != null && i.ModItem is SynergyModItem).Any();
	}
}
public class GlassCannon : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 999;
	}
	public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
		damage *= 1 + StackAmount(player) * .25f;
	}
	public override void UpdateEquip(Player player) {
		player.ModPlayerStats().CappedHealthAmount = 50;
		player.GetModPlayer<GlassCannonPlayer>().GlassCannon = true;
	}
}
public class GlassCannonPlayer : ModPlayer {
	public bool GlassCannon = false;
	public int cooldown = 0;
	public override void ResetEffects() {
		GlassCannon = false;
		cooldown = ModUtils.CountDown(cooldown);
	}
	public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (GlassCannon && cooldown == 0) {
			cooldown = Player.itemAnimationMax * 5;
			Vector2 reVelocity = velocity.SafeNormalize(Vector2.Zero) * 17;
			Projectile.NewProjectile(source, position, reVelocity.Vector2RotateByRandom(5), ModContent.ProjectileType<GlassProjectile>(), damage * 5, knockback, Player.whoAmI);
		}
		return base.Shoot(item, source, position, velocity, type, damage, knockback);
	}
}

public class EnchantmentSmith : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 3;
	}
}
public class ArcaneMaster : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 2;
		textureString = ModUtils.GetTheSameTextureAsEntity<ArcaneMaster>();
	}
	public override void UpdateEquip(Player player) {
		player.ModPlayerStats().AddStatsToPlayer(PlayerStats.MagicDMG, Multiplicative: 1.1f);
		float addition = (StackAmount(player) - 1) * .05f;
		player.manaCost -= .15f + addition;
		player.GetModPlayer<ArcaneMasterPlayer>().ArcaneMaster = true;
	}
	public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
		ArcaneMasterPlayer modplayer = player.GetModPlayer<ArcaneMasterPlayer>();
		if (modplayer.ArcaneMaster && item.DamageType == DamageClass.Magic) {
			float addition = (StackAmount(player) - 1) * .005f;
			damage += modplayer.ManaCostIncreases * .01f + addition;
		}
	}
	class ArcaneMasterPlayer : ModPlayer {
		public bool ArcaneMaster = false;
		public int ManaCostIncreases = 0;
		public int Decay_ManaCostIncreases = 0;
		public override void ResetEffects() {
			ArcaneMaster = false;
			if (ManaCostIncreases <= 0 || Player.ItemAnimationActive) return;
			if (++Decay_ManaCostIncreases >= 10) {
				ManaCostIncreases--;
				Decay_ManaCostIncreases = 0;
			}
		}
		public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
			if (ArcaneMaster && item.DamageType == DamageClass.Magic) damage += ManaCostIncreases * .01f;
		}
		public override void ModifyManaCost(Item item, ref float reduce, ref float mult) {
			if (ArcaneMaster) mult += ManaCostIncreases * .02f;
		}
		public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if (ArcaneMaster) ManaCostIncreases = Math.Clamp(ManaCostIncreases + 1, 0, 60);
			return base.Shoot(item, source, position, velocity, type, damage, knockback);
		}
	}
}
public class Stimulation : Perk {
	public override void SetDefaults() {
		CanBeStack = false;
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<StimPackPlayer>().StimPack = true;
	}
	class StimPackPlayer : ModPlayer {
		public bool StimPack = false;
		public override void ResetEffects() {
			StimPack = false;
		}
		public override void UpdateEquips() {
			if (!Player.IsHealthAbovePercentage(.4f) && StimPack) {
				Player.AddBuff(ModContent.BuffType<StimPackBuff>(), ModUtils.ToSecond(10));
			}
		}
	}
	class StimPackBuff : ModBuff {
		public override string Texture => ModTexture.MissingTexture_Default;
		public override void SetStaticDefaults() {
			Main.debuff[Type] = false;
		}
		public override void Update(Player player, ref int buffIndex) {
			PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
			modplayer.AddStatsToPlayer(PlayerStats.RegenHP, 1.5f, Flat: 10);
			modplayer.AddStatsToPlayer(PlayerStats.AttackSpeed, 1.12f);
		}
	}
}
public class VampiricAura : Perk {
	public override void SetDefaults() {
		CanBeStack = false;
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<VitalityDrainTotemPlayer>().VitalityDrainTotem = true;
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.HealEffectiveness, Additive: 1.25f);
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.RegenHP, Base: 3);
	}
	class VitalityDrainTotemPlayer : ModPlayer {
		public bool VitalityDrainTotem = false;
		int cooldown = 0;
		public override void ResetEffects() {
			VitalityDrainTotem = false;
		}
		public override void PostUpdate() {
			if (!Player.immune || !VitalityDrainTotem) {
				return;
			}
			if (++cooldown <= 12) {
				return;
			}
			Player.Center.LookForHostileNPC(out List<NPC> npclist, 225f);
			int amount = 0;
			foreach (NPC npc in npclist) {
				Player.StrikeNPCDirect(npc, npc.CalculateHitInfo(Math.Clamp((int)(npc.lifeMax * .005f), 1, 1000), 0));
				amount++;
			}
			if (amount > 0) {
				Player.Heal(amount);
			}
			cooldown = 0;
		}
	}
}
public class ChaosProtection : Perk {
	public override void SetDefaults() {
		CanBeStack = false;
	}
	public override void ModifyHitByNPC(Player player, NPC npc, ref Player.HurtModifiers modifiers) {
		modifiers.SourceDamage -= Main.rand.NextFloat(.05f, .5f);
	}
	public override void ModifyHitByProjectile(Player player, Projectile proj, ref Player.HurtModifiers modifiers) {
		modifiers.SourceDamage -= Main.rand.NextFloat(.05f, .5f);
	}
	public override void OnHitByNPC(Player player, NPC npc, Player.HurtInfo hurtInfo) {
		if (Main.rand.NextBool(10)) {
			player.Heal(hurtInfo.Damage * 2);
		}
		SpawnProjectile(player, hurtInfo.Damage);
	}
	public override void OnHitByProjectile(Player player, Projectile proj, Player.HurtInfo hurtInfo) {
		if (Main.rand.NextBool(10)) {
			player.Heal(hurtInfo.Damage * 2);
		}
		SpawnProjectile(player, hurtInfo.Damage);
	}
	private void SpawnProjectile(Player player, int Damage) {
		if (Main.rand.NextBool(4)) {
			Vector2 pos = player.Center + Main.rand.NextVector2Circular(400, 400);
			Vector2 vel = Main.rand.NextVector2CircularEdge(10, 10);
			int min = Math.Min(10, Damage);
			int max = Math.Max(10, Damage);
			int damageraw = Main.rand.Next(min - 1, max);
			if (damageraw <= 0) {
				damageraw = 1;
			}
			Projectile.NewProjectile(player.GetSource_FromThis(), pos, vel, Main.rand.Next(TerrariaArrayID.UltimateProjPack), damageraw, Main.rand.NextFloat(2, 5), player.whoAmI);
		}
	}
	class ChaosTabletPlayer : ModPlayer {
		public bool ChaosTablet = false;
		public PlayerStats chaosstat = PlayerStats.None;
		public override void ResetEffects() {
			ChaosTablet = false;
		}
		public override void UpdateEquips() {
			if (!ChaosTablet) {
				return;
			}
			if (chaosstat == PlayerStats.None) {
				chaosstat = Main.rand.Next(MysteriousPotionBuff.lookupDictionary.Keys.ToList());
				Player.AddBuff(ModContent.BuffType<ChaosBuff>(), ModUtils.ToSecond(15));
			}
			if (!Player.Center.LookForAnyHostileNPC(600)) {
				return;
			}
			if (Main.rand.NextBool(100)) {
				int weapondmg = Player.GetWeaponDamage(Player.HeldItem);
				int dmg = Main.rand.Next(40, 180) + weapondmg;
				int buffid = Main.rand.Next(TerrariaArrayID.Debuff);
				Projectile.NewProjectile(Player.GetSource_Misc("ChaosTablet"), Player.Center + Main.rand.NextVector2Circular(600, 600), Vector2.Zero, ModContent.ProjectileType<ChaosExplosion>(), dmg, Main.rand.NextFloat(3, 9), Player.whoAmI, buffid);
			}
		}
	}
	class ChaosExplosion : ModProjectile {
		public override string Texture => ModTexture.MissingTexture_Default;
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 1;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 10;
			Projectile.hide = true;
		}
		public int BuffID { get => (int)Projectile.ai[0]; }
		public override bool? CanDamage() {
			return false;
		}
		public override void OnKill(int timeLeft) {
			for (int i = 0; i < 100; i++) {
				int firework = Main.rand.Next(new int[] { DustID.Firework_Blue, DustID.Firework_Green, DustID.Firework_Pink, DustID.Firework_Red, DustID.Firework_Yellow });
				int dust = Dust.NewDust(Projectile.Center, 0, 0, firework);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = Main.rand.NextVector2Circular(20, 20);
			}
			for (int i = 0; i < 50; i++) {
				int firework = Main.rand.Next(new int[] { DustID.Firework_Blue, DustID.Firework_Green, DustID.Firework_Pink, DustID.Firework_Red, DustID.Firework_Yellow });
				int dust = Dust.NewDust(Projectile.Center, 0, 0, firework);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = Main.rand.NextVector2CircularEdge(20, 20);
			}
			Projectile.Center.LookForHostileNPC(out List<NPC> npclist, 200);
			Player player = Main.player[Projectile.owner];
			NPC.HitInfo info = new();
			info.Damage = Projectile.damage;
			info.Knockback = Projectile.knockBack;
			foreach (NPC npc in npclist) {
				info.HitDirection = ModUtils.DirectionFromPlayerToNPC(Projectile.Center.X, npc.Center.X);
				info.Crit = Main.rand.NextBool(7);
				player.StrikeNPCDirect(npc, info);
				npc.AddBuff(BuffID, ModUtils.ToSecond(Main.rand.Next(5, 16)));
			}
		}
	}

	class ChaosBuff : ModBuff {
		public override string Texture => ModTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultBuff();
		}
		public override void Update(Player player, ref int buffIndex) {
			ChaosTabletPlayer chaosplayer = player.GetModPlayer<ChaosTabletPlayer>();
			PlayerStatsHandle statsplayer = player.GetModPlayer<PlayerStatsHandle>();
			var modplayer = player.GetModPlayer<MysteriousPotionPlayer>();
			switch (chaosplayer.chaosstat) {
				case PlayerStats.MaxHP:
				case PlayerStats.RegenHP:
				case PlayerStats.MaxMana:
				case PlayerStats.RegenMana:
				case PlayerStats.CritChance:
				case PlayerStats.MaxMinion:
				case PlayerStats.Defense:
				case PlayerStats.MaxSentry:
					statsplayer.AddStatsToPlayer(chaosplayer.chaosstat,
						Base: modplayer.ToStatsNumInt(chaosplayer.chaosstat, 2));
					break;
				default:
					statsplayer.AddStatsToPlayer(chaosplayer.chaosstat,
						Additive: 1 + modplayer.ToStatsNumFloat(chaosplayer.chaosstat, 2));
					break;
			}
			if (player.buffTime[buffIndex] <= 0) {
				player.GetModPlayer<ChaosTabletPlayer>().chaosstat = PlayerStats.None;
			}
		}
		public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare) {
			ChaosTabletPlayer chaosplayer = Main.LocalPlayer.GetModPlayer<ChaosTabletPlayer>();
			MysteriousPotionPlayer modplayer = Main.LocalPlayer.GetModPlayer<MysteriousPotionPlayer>();
			if (ModUtils.DoesStatsRequiredWholeNumber(chaosplayer.chaosstat)) {
				tip = $"+ {modplayer.ToStatsNumInt(chaosplayer.chaosstat, 1)} {chaosplayer.chaosstat}";
			}
			else {
				tip = $"+ {modplayer.ToStatsNumInt(chaosplayer.chaosstat, 1)}% {chaosplayer.chaosstat}";
			}
		}
		public override bool RightClick(int buffIndex) {
			return false;
		}
	}
}
public class BloodthornCore : Perk {
	public override void SetDefaults() {
		CanBeStack = false;
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.HealEffectiveness, Additive: 1.15f);
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.MaxHP, Base: 15);
	}
	public override void OnHitByAnything(Player player) {
		int damage = (int)player.GetDamage(DamageClass.Magic).ApplyTo(45);
		Vector2 vecR = Vector2.One.Vector2RotateByRandom(90);
		for (int i = 0; i < 6; i++) {
			Vector2 vec = vecR.Vector2DistributeEvenly(6, 360, i);
			Projectile.NewProjectile(player.GetSource_Misc("BloodthornCorePerk"), player.Center, vec, ProjectileID.SharpTears, damage, 3f, player.whoAmI, 0, Main.rand.NextFloat(.9f, 1.1f));
		}
	}
	public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_CustomContextInfo == "BloodthornCorePerk" && Main.rand.NextBool(3)) {
			player.Heal(Main.rand.Next(3, 7));
		}
	}
}
