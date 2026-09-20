using Microsoft.Xna.Framework;
using Mono.Cecil;
using Newtonsoft.Json.Linq;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Transfixion.WeaponEnchantment;

public class BabyBirdStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.BabyBirdStaff;
		ForcedCleanCounter = true;
	}
	public override void PreCleanCounter(int index, Player player, EnchantmentGlobalItem globalItem, Item item) {
		player.ClearBuff(BuffID.BabyBird);
		if (globalItem.Item_Counter1[index] < 0 || globalItem.Item_Counter1[index] >= 1000) {
			return;
		}
		Main.projectile[globalItem.Item_Counter1[index]].Kill();
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.SummonDMG, 1.18f);
		int damage = 14 + (int)(player.GetWeaponDamage(item) * 1.5f);
		if (globalItem.Item_Counter1[index] < 0 || globalItem.Item_Counter1[index] >= 1000) {
			int proj1 = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ProjectileID.BabyBird, damage, 0, player.whoAmI);
			Main.projectile[proj1].minionSlots = 0;
			globalItem.Item_Counter1[index] = proj1;
		}
		else {
			Projectile projectile = Main.projectile[globalItem.Item_Counter1[index]];
			if (projectile == null || !projectile.active || projectile.timeLeft <= 0) {
				globalItem.Item_Counter1[index] = -1;
			}
		}
		player.AddBuff(BuffID.BabyBird, 60);
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (!ContentSamples.ProjectilesByType[type].minion) {
			Projectile projectile = Main.projectile[globalItem.Item_Counter1[index]];
			if (projectile == null || !projectile.active || projectile.timeLeft <= 0) {
				globalItem.Item_Counter1[index] = -1;
				return;
			}
			Projectile.NewProjectile(source, projectile.Center, (Main.MouseWorld - projectile.Center).SafeNormalize(Vector2.Zero) * velocity.Length(), type, (int)(damage * .25f), knockback * .25f, player.whoAmI);
		}
	}
}
public class BabySlimeStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.SlimeStaff;
		ForcedCleanCounter = true;
	}
	public override void PreCleanCounter(int index, Player player, EnchantmentGlobalItem globalItem, Item item) {
		player.ClearBuff(BuffID.BabySlime);
		if (globalItem.Item_Counter1[index] < 0 || globalItem.Item_Counter1[index] >= 1000) {
			return;
		}
		Main.projectile[globalItem.Item_Counter1[index]].Kill();
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.SummonDMG, 1.12f);
		int damage = 30 + player.GetWeaponDamage(item);
		if (globalItem.Item_Counter1[index] < 0 || globalItem.Item_Counter1[index] >= 1000) {
			int proj1 = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ProjectileID.BabySlime, damage, 0, player.whoAmI);
			Main.projectile[proj1].minionSlots = 0;
			globalItem.Item_Counter1[index] = proj1;
		}
		else {
			Projectile projectile = Main.projectile[globalItem.Item_Counter1[index]];
			if (projectile == null || !projectile.active || projectile.timeLeft <= 0) {
				globalItem.Item_Counter1[index] = -1;
			}
		}
		player.AddBuff(BuffID.BabySlime, 60);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.type == ProjectileID.BabySlime) {
			float Amount = Main.rand.Next(1, 3);
			for (int i = 0; i < Amount; i++) {
				Vector2 vel = Main.rand.NextVector2Unit(-(MathHelper.PiOver2 + MathHelper.PiOver4 * .5f), MathHelper.PiOver4) * Main.rand.NextFloat(5, 7);
				int projec = Projectile.NewProjectile(proj.GetSource_FromAI(), proj.Center, vel, ProjectileID.SpikedSlimeSpike, proj.damage, 0, player.whoAmI);
				Main.projectile[projec].friendly = true;
				Main.projectile[projec].hostile = false;
			}
		}
	}
}
public class FlinxStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.FlinxStaff;
		ForcedCleanCounter = true;
	}
	public override void PreCleanCounter(int index, Player player, EnchantmentGlobalItem globalItem, Item item) {
		player.ClearBuff(BuffID.FlinxMinion);
		if (globalItem.Item_Counter1[index] >= 0 && globalItem.Item_Counter1[index] < 1000) {
			Main.projectile[globalItem.Item_Counter1[index]].Kill();
		}
		if (globalItem.Item_Counter2[index] >= 0 && globalItem.Item_Counter2[index] < 1000) {
			Main.projectile[globalItem.Item_Counter2[index]].Kill();
		}
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		int damage = 18 + player.GetWeaponDamage(item) / 2;
		if (globalItem.Item_Counter1[index] < 0 || globalItem.Item_Counter1[index] >= 1000) {
			int proj1 = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ProjectileID.FlinxMinion, damage, 0, player.whoAmI);
			Main.projectile[proj1].minionSlots = 0;
			globalItem.Item_Counter1[index] = proj1;
		}
		else {
			Projectile projectile = Main.projectile[globalItem.Item_Counter1[index]];
			if (projectile == null || !projectile.active || projectile.timeLeft <= 0) {
				globalItem.Item_Counter1[index] = -1;
			}
		}
		if (globalItem.Item_Counter2[index] < 0 || globalItem.Item_Counter2[index] >= 1000) {
			int proj2 = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ProjectileID.FlinxMinion, damage, 0, player.whoAmI);
			Main.projectile[proj2].minionSlots = 0;
			globalItem.Item_Counter2[index] = proj2;
		}
		else {
			Projectile projectile = Main.projectile[globalItem.Item_Counter2[index]];
			if (projectile == null || !projectile.active || projectile.timeLeft <= 0) {
				globalItem.Item_Counter2[index] = -1;
			}
		}
		player.AddBuff(BuffID.FlinxMinion, 60);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.type == ProjectileID.FlinxMinion) {
			target.AddBuff(BuffID.Frostburn, 120);
		}
	}
}
public class VampireFrogStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.VampireFrogStaff;
		ForcedCleanCounter = true;
	}
	public override void PreCleanCounter(int index, Player player, EnchantmentGlobalItem globalItem, Item item) {
		player.ClearBuff(BuffID.VampireFrog);
		if (globalItem.Item_Counter1[index] < 0 || globalItem.Item_Counter1[index] >= 1000) {
			return;
		}
		if (Main.projectile[globalItem.Item_Counter1[index]] != null) {
			Main.projectile[globalItem.Item_Counter1[index]].Kill();
		}
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.ModPlayerStats().LifeSteal += .1f;
		if (globalItem.Item_Counter1[index] < 0 || globalItem.Item_Counter1[index] >= 1000) {
			int proj1 = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ProjectileID.VampireFrog,
				20 + player.GetWeaponDamage(item) / 4, 0, player.whoAmI);
			Main.projectile[proj1].minionSlots = 0;
			globalItem.Item_Counter1[index] = proj1;
		}
		else {
			Projectile projectile = Main.projectile[globalItem.Item_Counter1[index]];
			if (projectile == null || !projectile.active || projectile.timeLeft <= 0) {
				globalItem.Item_Counter1[index] = -1;
			}
		}
		player.AddBuff(BuffID.VampireFrog, 60);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.type == ProjectileID.VampireFrog) {
			player.Heal(1);
		}
	}
}
public class AbigailsFlower : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.AbigailsFlower;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.GetDamage(DamageClass.Summon) += .5f;
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void ModifyHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.minion) {
			modifiers.SourceDamage += 1;
		}
	}
	public override void ModifyHitByNPC(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, ref Player.HurtModifiers modifiers) {
		if (globalItem.Item_Counter1[index] <= 0) {
			modifiers.SetMaxDamage(50);
		}
	}
	public override void ModifyHitByProj(int index, Player player, EnchantmentGlobalItem globalItem, Item item, Projectile proj, ref Player.HurtModifiers modifiers) {
		if (globalItem.Item_Counter1[index] <= 0) {
			modifiers.SetMaxDamage(50);
		}
	}
	public override void OnHitByNPC(int index, EnchantmentGlobalItem globalItem, Player player, NPC npc, Player.HurtInfo hurtInfo) {
		if (hurtInfo.Damage >= 50) {
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, ModUtils.ToSecond(30));
		}
	}
	public override void OnHitByProjectile(int index, EnchantmentGlobalItem globalItem, Player player, Projectile proj, Player.HurtInfo hurtInfo) {
		if (hurtInfo.Damage >= 50) {
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, ModUtils.ToSecond(30));
		}
	}
}
public class BladeStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.Smolstar; ForcedCleanCounter = true;
	}
	public override void PreCleanCounter(int index, Player player, EnchantmentGlobalItem globalItem, Item item) {
		player.ClearBuff(BuffID.Smolstar);
		if (globalItem.Item_Counter1[index] < 0 || globalItem.Item_Counter1[index] >= 1000) {
			return;
		}
		if (Main.projectile[globalItem.Item_Counter1[index]] != null) {
			Main.projectile[globalItem.Item_Counter1[index]].Kill();
		}
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		if (globalItem.Item_Counter1[index] < 0 || globalItem.Item_Counter1[index] >= 1000) {
			int proj1 = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ProjectileID.Smolstar,
				10 + player.GetWeaponDamage(item) / 10, 0, player.whoAmI);
			Main.projectile[proj1].minionSlots = 0;
			globalItem.Item_Counter1[index] = proj1;
		}
		else {
			Projectile projectile = Main.projectile[globalItem.Item_Counter1[index]];
			if (projectile == null || !projectile.active || projectile.timeLeft <= 0) {
				globalItem.Item_Counter1[index] = -1;
			}
		}
		player.AddBuff(BuffID.Smolstar, 60);
	}
}
public class SpiderStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.SpiderStaff; ForcedCleanCounter = true;
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		if (item.DamageType == DamageClass.Summon) {
			damage += .2f;
		}
	}
	public override void PreCleanCounter(int index, Player player, EnchantmentGlobalItem globalItem, Item item) {
		player.ClearBuff(BuffID.SpiderMinion);
		if (globalItem.Item_Counter1[index] < 0 || globalItem.Item_Counter1[index] >= 1000) {
			return;
		}
		if (Main.projectile[globalItem.Item_Counter1[index]] != null) {
			Main.projectile[globalItem.Item_Counter1[index]].Kill();
		}
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		if (globalItem.Item_Counter1[index] < 0 || globalItem.Item_Counter1[index] >= 1000) {
			int proj1 = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ProjectileID.DangerousSpider,
				30 + player.GetWeaponDamage(item) / 10, 0, player.whoAmI);
			Main.projectile[proj1].minionSlots = 0;
			globalItem.Item_Counter1[index] = proj1;
		}
		else {
			Projectile projectile = Main.projectile[globalItem.Item_Counter1[index]];
			if (projectile == null || !projectile.active || projectile.timeLeft <= 0) {
				globalItem.Item_Counter1[index] = -1;
			}
		}
		player.AddBuff(BuffID.SpiderMinion, 60);
	}
}
public class PirateStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.PirateStaff; ForcedCleanCounter = true;
	}
	public override void PreCleanCounter(int index, Player player, EnchantmentGlobalItem globalItem, Item item) {
		player.ClearBuff(BuffID.PirateMinion);
		if (globalItem.Item_Counter1[index] < 0 || globalItem.Item_Counter1[index] >= 1000) {
			return;
		}
		if (Main.projectile[globalItem.Item_Counter1[index]] != null) {
			Main.projectile[globalItem.Item_Counter1[index]].Kill();
		}
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		if (globalItem.Item_Counter1[index] < 0 || globalItem.Item_Counter1[index] >= 1000) {
			int proj1 = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ProjectileID.OneEyedPirate,
				30 + player.GetWeaponDamage(item) / 10, 0, player.whoAmI);
			Main.projectile[proj1].minionSlots = 0;
			globalItem.Item_Counter1[index] = proj1;
		}
		else {
			Projectile projectile = Main.projectile[globalItem.Item_Counter1[index]];
			if (projectile == null || !projectile.active || projectile.timeLeft <= 0) {
				globalItem.Item_Counter1[index] = -1;
			}
		}
		player.AddBuff(BuffID.PirateMinion, 60);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.type == ProjectileID.OneEyedPirate
			|| proj.type == ProjectileID.PirateCaptain
			|| proj.type == ProjectileID.SoulscourgePirate) {
			player.QuickSpawnItem(player.GetSource_FromThis(), ItemID.CopperCoin);
		}
	}
}
public class SanguineStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.SanguineStaff; ForcedCleanCounter = true;
	}
	public override void PreCleanCounter(int index, Player player, EnchantmentGlobalItem globalItem, Item item) {
		player.ClearBuff(BuffID.BatOfLight);
		if (globalItem.Item_Counter1[index] < 0 || globalItem.Item_Counter1[index] >= 1000) {
			return;
		}
		if (Main.projectile[globalItem.Item_Counter1[index]] != null) {
			Main.projectile[globalItem.Item_Counter1[index]].Kill();
		}
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.ModPlayerStats().UpdateHPRegen.Base += 2;
		if (globalItem.Item_Counter1[index] < 0 || globalItem.Item_Counter1[index] >= 1000) {
			int proj1 = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ProjectileID.BatOfLight,
				15 + player.GetWeaponDamage(item) / 2, 0, player.whoAmI);
			Main.projectile[proj1].minionSlots = 0;
			globalItem.Item_Counter1[index] = proj1;
		}
		else {
			Projectile projectile = Main.projectile[globalItem.Item_Counter1[index]];
			if (projectile == null || !projectile.active || projectile.timeLeft <= 0) {
				globalItem.Item_Counter1[index] = -1;
			}
		}
		player.AddBuff(BuffID.BatOfLight, 60);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.type == ProjectileID.BatOfLight) {
			player.Heal(1);
		}
	}
}
public class OpticStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.OpticStaff;
		ForcedCleanCounter = true;
	}
	public override void PreCleanCounter(int index, Player player, EnchantmentGlobalItem globalItem, Item item) {
		player.ClearBuff(BuffID.TwinEyesMinion);
		if (globalItem.Item_Counter1[index] >= 0 && globalItem.Item_Counter1[index] < 1000) {
			Main.projectile[globalItem.Item_Counter1[index]].Kill();
		}
		if (globalItem.Item_Counter2[index] >= 0 && globalItem.Item_Counter2[index] < 1000) {
			Main.projectile[globalItem.Item_Counter2[index]].Kill();
		}
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		int damage = 28 + player.GetWeaponDamage(item) / 3;
		if (globalItem.Item_Counter1[index] < 0 || globalItem.Item_Counter1[index] >= 1000) {
			int proj1 = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ProjectileID.Spazmamini, damage, 0, player.whoAmI);
			Main.projectile[proj1].minionSlots = 0;
			globalItem.Item_Counter1[index] = proj1;
		}
		else {
			Projectile projectile = Main.projectile[globalItem.Item_Counter1[index]];
			if (projectile == null || !projectile.active || projectile.timeLeft <= 0) {
				globalItem.Item_Counter1[index] = -1;
			}
		}
		if (globalItem.Item_Counter2[index] < 0 || globalItem.Item_Counter2[index] >= 1000) {
			int proj2 = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Vector2.Zero, ProjectileID.Retanimini, damage, 0, player.whoAmI);
			Main.projectile[proj2].minionSlots = 0;
			globalItem.Item_Counter2[index] = proj2;
		}
		else {
			Projectile projectile = Main.projectile[globalItem.Item_Counter2[index]];
			if (projectile == null || !projectile.active || projectile.timeLeft <= 0) {
				globalItem.Item_Counter2[index] = -1;
			}
		}
		player.AddBuff(BuffID.TwinEyesMinion, 60);
	}
	public override void ModifyHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.type == ProjectileID.Spazmamini && target.HasBuff<Enchantment_Marked>()) {
			modifiers.SourceDamage += 2;
			target.RequestBuffRemoval(ModContent.BuffType<Enchantment_Marked>());
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.type == ProjectileID.Spazmamini) {
			target.AddBuff(BuffID.CursedInferno, 120);
		}
		if (proj.type == ProjectileID.MiniRetinaLaser) {
			target.AddBuff<Enchantment_Marked>(ModUtils.ToSecond(5));
		}
	}
	public class Enchantment_Marked : ModBuff {
		public override string Texture => ModTexture.EMPTYDEBUFF;
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultDeBuff();
		}
		public override void Update(NPC npc, ref int buffIndex) {
			npc.GetGlobalNPC<RoguelikeGlobalNPC>().StatDefense -= .1f;
		}
	}
}
public class LeatherWhip : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.BlandWhip;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.whipRangeMultiplier += .2f;
		player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += .2f;
		player.GetDamage(DamageClass.Summon).Base += 1;
	}
}

public class Snapthorn : ModEnchantment {

	public override void SetDefaults() {
		ItemIDType = ItemID.ThornWhip;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.whipRangeMultiplier += 0.15f;
		player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.15f;
		player.GetDamage(DamageClass.Summon).Base += 2;
	}

	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		player.AddBuff(BuffID.ThornWhipPlayerBuff, ModUtils.ToSecond(1.5f));
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		player.AddBuff(BuffID.ThornWhipPlayerBuff, ModUtils.ToSecond(1.5f));
	}


}

public class SpinalTap : ModEnchantment {

	public override void SetDefaults() {
		ItemIDType = ItemID.BoneWhip;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.whipRangeMultiplier += 0.2f;
		player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.2f;
		player.GetDamage(DamageClass.Summon).Base += 3;
	}
}

public class ImpStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.ImpStaff;
		ForcedCleanCounter = true;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index]++;
		foreach (Projectile minion in Main.ActiveProjectiles) {
			NPC target = minion.FindTargetWithinRange(200);
			if (minion.minion && minion.owner == player.whoAmI && globalItem.Item_Counter1[index] >= 30 && target != null) {
				Projectile.NewProjectile(player.GetSource_FromAI(), minion.position, (target.Center - minion.Center).SafeNormalize(Vector2.UnitY) * 15, ProjectileID.ImpFireball, 15, 0, player.whoAmI);
				globalItem.Item_Counter1[index] = 0;
			}
		}
	}
}


public class HornetStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.HornetStaff;
		ForcedCleanCounter = true;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		globalItem.Item_Counter1[index]++;
		foreach (Projectile minion in Main.ActiveProjectiles) {
			NPC target = minion.FindTargetWithinRange(200);
			if (minion.minion && minion.owner == player.whoAmI && globalItem.Item_Counter1[index] >= 40 && target != null) {
				Projectile.NewProjectile(player.GetSource_FromAI(), minion.position, (target.Center - minion.Center).SafeNormalize(Vector2.UnitY) * 5, ProjectileID.HornetStinger, 12, 0, player.whoAmI);
				globalItem.Item_Counter1[index] = 0;
			}
		}
	}
}

public class HoundiusShootius : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.HoundiusShootius;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.GetDamage(DamageClass.Summon) += .3f;
		if (--globalItem.Item_Counter2[index] > 0) {
			return;
		}
		globalItem.Item_Counter2[index] = 60;
		if (player.Center.LookForHostileNPC(out NPC npc, 800)) {
			Vector2 pos = player.Center + Main.rand.NextVector2Circular(50, 50);
			Vector2 vel = (npc.Center - pos).SafeNormalize(Vector2.Zero) * 15;
			int damage = player.GetWeaponDamage(item) + 22;
			Projectile.NewProjectile(player.GetSource_ItemUse(item), pos, vel, ProjectileID.HoundiusShootiusFireball, damage, 1, player.whoAmI);
		}
	}
	public override void Shoot(int index, Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (++globalItem.Item_Counter1[index] < 3) {
			return;
		}
		globalItem.Item_Counter1[index] = 0;
		Projectile.NewProjectile(source, position, velocity, ProjectileID.HoundiusShootiusFireball, (int)(damage * .74f), knockback, player.whoAmI);
	}
}
public class LightningAuraRod : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.DD2LightningAuraT1Popper;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.GetDamage(DamageClass.Summon) += .3f;
		for (int i = 0; i < 30; i++) {
			Dust dust = Dust.NewDustDirect(player.Center + Main.rand.NextVector2CircularEdge(600, 600), 0, 0, DustID.Electric);
			dust.noGravity = true;
			dust.scale = .2f;
			dust.velocity = Vector2.Zero;
		}
		if (--globalItem.Item_Counter2[index] > 0) {
			return;
		}
		globalItem.Item_Counter2[index] = PlayerStatsHandle.WE_CoolDown(player, 10);
		player.Center.LookForHostileNPC(out List<NPC> npclist, 600);
		foreach (NPC npc in npclist) {
			int damage = (int)(player.GetWeaponDamage(item) * .25f) + 5;
			player.StrikeNPCDirect(npc, npc.CalculateHitInfo(damage, -1));
			npc.AddBuff(BuffID.Electrified, 90);
		}
	}
}

public class LightningAuraCane : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.DD2LightningAuraT2Popper;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.GetDamage(DamageClass.Summon) += .3f;
		for (int i = 0; i < 100; i++) {
			Dust dust = Dust.NewDustDirect(player.Center + Main.rand.NextVector2CircularEdge(1000, 1000), 0, 0, DustID.Electric);
			dust.noGravity = true;
			dust.scale = .2f;
			dust.velocity = Vector2.Zero;
		}
		if (--globalItem.Item_Counter2[index] > 0) {
			return;
		}
		globalItem.Item_Counter2[index] = PlayerStatsHandle.WE_CoolDown(player, 60);
		if (player.Center.LookForHostileNPC(out NPC npc, 1000)) {
			float value = 1.25f;
			if (npc.HasBuff(BuffID.Electrified)) {
				value += 2;
			}
			int damage = (int)(player.GetWeaponDamage(item) * value) + 5;
			player.StrikeNPCDirect(npc, npc.CalculateHitInfo(damage, -1));
			for (int i = 0; i < 30; i++) {
				Dust dust = Dust.NewDustDirect(npc.Center, 0, 0, DustID.Electric);
				dust.noGravity = true;
				dust.scale = 1.2f + Main.rand.NextFloat(.2f, .4f);
				dust.velocity = Main.rand.NextVector2Circular(5, 5);
			}
			for (int i = 0; i < 30; i++) {
				Dust dust = Dust.NewDustDirect(player.Center, 0, 0, DustID.Electric);
				dust.noGravity = true;
				dust.scale = 1.2f + Main.rand.NextFloat(.2f, .4f);
				dust.velocity = Main.rand.NextVector2Circular(5, 5);
			}
		}
	}
}
public class LightningAuraStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.DD2LightningAuraT3Popper;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.GetDamage(DamageClass.Summon) += .3f;
	}
	public override void ModifyHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.Electrified)) {
			modifiers.SourceDamage += 1;
		}
	}
	public override void ModifyHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.Electrified)) {
			modifiers.SourceDamage += 1;
		}
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		if (item.type == ItemID.DD2LightningAuraT1Popper
			|| item.type == ItemID.DD2LightningAuraT2Popper
			|| item.type == ItemID.DD2LightningAuraT3Popper) {
			damage += .4f;
		}
	}
}
public class FlameburstRod : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.DD2FlameburstTowerT1Popper;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.GetDamage(DamageClass.Summon) += .3f;
		if (--globalItem.Item_Counter2[index] > 0) {
			return;
		}
		globalItem.Item_Counter2[index] = PlayerStatsHandle.WE_CoolDown(player, 30);
		if (player.Center.LookForHostileNPC(out NPC npc, 800)) {
			Vector2 pos = player.Center + Main.rand.NextVector2Circular(50, 50);
			Vector2 vel = (npc.Center - pos).SafeNormalize(Vector2.Zero) * 17;
			int damage = (int)(player.GetWeaponDamage(item) * 1.5f) + 5;
			Projectile.NewProjectile(player.GetSource_ItemUse(item), pos, vel, ProjectileID.DD2FlameBurstTowerT1Shot, damage, 1, player.whoAmI);
		}
	}
	public override void ModifyHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.OnFire) || target.HasBuff(BuffID.OnFire3)) {
			modifiers.SourceDamage += .5f;
		}
	}
	public override void ModifyHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.OnFire) || target.HasBuff(BuffID.OnFire3)) {
			modifiers.SourceDamage += .5f;
		}
	}
}
public class FlameburstCane : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.DD2FlameburstTowerT2Popper;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.GetDamage(DamageClass.Summon) += .3f;
		if (--globalItem.Item_Counter2[index] > 0) {
			return;
		}
		globalItem.Item_Counter2[index] = PlayerStatsHandle.WE_CoolDown(player, 60);
		int damage = (int)(player.GetWeaponDamage(item) * .75f) + 25;
		for (int i = 0; i < 4; i++) {
			Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, Main.rand.NextVector2CircularEdge(17, 17), ProjectileID.DD2FlameBurstTowerT2Shot, damage, 1, player.whoAmI);
		}
		Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 17, ProjectileID.DD2FlameBurstTowerT2Shot, damage, 1, player.whoAmI);
	}
}
public class FlameburstStaff : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.DD2FlameburstTowerT3Popper;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		player.GetDamage(DamageClass.Summon) += .3f;
	}
	public override void ModifyHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.OnFire) || target.HasBuff(BuffID.OnFire3)) {
			modifiers.SourceDamage += 1;
		}
	}
	public override void ModifyHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.OnFire) || target.HasBuff(BuffID.OnFire3)) {
			modifiers.SourceDamage += 1;
		}
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		if (item.type == ItemID.DD2FlameburstTowerT1Popper
			|| item.type == ItemID.DD2FlameburstTowerT2Popper
			|| item.type == ItemID.DD2FlameburstTowerT3Popper) {
			damage += .4f;
		}
	}
}
