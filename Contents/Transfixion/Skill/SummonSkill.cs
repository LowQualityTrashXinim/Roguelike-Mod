using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Contents.Projectiles;
using Roguelike.Texture;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Transfixion.Skill;
public class BroadSwordSpirit : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 545;
		Skill_Duration = ModUtils.ToSecond(1);
		Skill_Type = SkillTypeID.Skill_Summon;
	}
	public override void OnTrigger(Player player, SkillHandlePlayer skillplayer, int duration, int energy) {
		if (player.ownedProjectileCounts[ModContent.ProjectileType<SwordProjectile3>()] < 1) {
			for (int i = 0; i < 3; i++) {
				int damage = (int)player.GetTotalDamage(DamageClass.Melee).ApplyTo(skillplayer.SkillDamage(34));
				float knockback = (int)player.GetTotalKnockback(DamageClass.Melee).ApplyTo(3);
				int proj = Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<SwordProjectile3>(), damage, knockback, player.whoAmI, 0, 0, i);
				if (Main.projectile[proj].ModProjectile is SwordProjectile3 woodproj)
					woodproj.ItemIDtextureValue = Main.rand.Next(TerrariaArrayID.AllOreBroadSword);
			}
		}
	}
}
public class WoodSwordSpirit : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 645;
		Skill_Duration = ModUtils.ToSecond(3);
		Skill_Type = SkillTypeID.Skill_Summon;
	}
	public override void OnTrigger(Player player, SkillHandlePlayer skillplayer, int duration, int energy) {
		if (player.ownedProjectileCounts[ModContent.ProjectileType<SoulWoodSword>()] < 1) {
			int damage = (int)player.GetTotalDamage(DamageClass.Melee).ApplyTo(skillplayer.SkillDamage(24));
			float knockback = (int)player.GetTotalKnockback(DamageClass.Melee).ApplyTo(5);
			int proj = Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<SoulWoodSword>(), damage, knockback, player.whoAmI);
			if (Main.projectile[proj].ModProjectile is SwordProjectile2 woodproj)
				woodproj.ItemIDtextureValue = Main.rand.Next(TerrariaArrayID.AllWoodSword);
		}
	}
}

public class WilloFreeze : ModSkill {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<WilloFreeze>();
	public override void SetDefault() {
		Skill_EnergyRequire = 685;
		Skill_Duration = ModUtils.ToSecond(4);
		Skill_Type = SkillTypeID.Skill_Summon;
	}
	public override void OnTrigger(Player player, SkillHandlePlayer skillplayer, int duration, int energy) {
		if (player.ownedProjectileCounts[ModContent.ProjectileType<WilloFreezeProjectile>()] < 1) {
			int damage = (int)player.GetTotalDamage(DamageClass.Magic).ApplyTo(skillplayer.SkillDamage(36));
			float knockback = (int)player.GetTotalKnockback(DamageClass.Magic).ApplyTo(5);
			for (int i = 0; i < 4; i++) {
				Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<WilloFreezeProjectile>(), damage, knockback, player.whoAmI, 75, i, 4);
			}
		}
	}
}

public class PowerPlant : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 525;
		Skill_Duration = ModUtils.ToSecond(4);
		Skill_Type = SkillTypeID.Skill_Summon;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		if (player.ownedProjectileCounts[ModContent.ProjectileType<PowerPlantProjectile>()] < 1) {
			Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<PowerPlantProjectile>(), 0, 0, player.whoAmI);
		}
	}
}
public class TransferStation : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 525;
		Skill_Duration = ModUtils.ToSecond(4);
		Skill_Type = SkillTypeID.Skill_Summon;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		if (player.ownedProjectileCounts[ModContent.ProjectileType<TransferStationProjectile>()] < 1) {
			Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<TransferStationProjectile>(), 0, 0, player.whoAmI);
		}
	}
}
public class OrbOfPurity : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 325;
		Skill_Duration = ModUtils.ToSecond(4);
		Skill_Type = SkillTypeID.Skill_Summon;
	}
	public override void OnTrigger(Player player, SkillHandlePlayer skillplayer, int duration, int energy) {
		if (player.ownedProjectileCounts[ModContent.ProjectileType<DiamondSwotaffOrb>()] < 1) {
			int damage = skillplayer.SkillDamage(10);
			Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<DiamondSwotaffOrb>(), damage, 0, player.whoAmI);
		}
	}
}

public class PhoenixBlazingTornado : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 325;
		Skill_Duration = ModUtils.ToSecond(5);
		Skill_Type = SkillTypeID.Skill_Summon;
	}
	public override void OnTrigger(Player player, SkillHandlePlayer skillplayer, int duration, int energy) {
		if (player.ownedProjectileCounts[ModContent.ProjectileType<BlazingTornado>()] < 1) {
			int damage = skillplayer.SkillDamage(120);
			Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<BlazingTornado>(), damage, 0, player.whoAmI);
		}
	}
}

public class DebugCommand : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 777;
		Skill_Duration = ModUtils.ToSecond(10);
		Skill_Type = SkillTypeID.Skill_Summon;
	}
	public override void OnTrigger(Player player, SkillHandlePlayer skillplayer, int duration, int energy) {
		int damage = skillplayer.SkillDamage(2000);
		player.Center.LookForHostileNPC(out var npclist, 2500);
		foreach (var npc in npclist) {
			player.StrikeNPCDirect(npc, npc.CalculateHitInfo(damage, ModUtils.DirectionFromPlayerToNPC(player.Center.X, npc.Center.X)));
		}
		player.AddImmuneTime(-1, duration / 10);
		player.immune = true;
		player.AddBuff<DebugStatus>(ModUtils.ToSecond(duration / 10));
	}
}
public class DebugStatus : ModBuff {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		for (int i = 0; i < player.buffImmune.Length; i++) {
			int buffType = i;
			if (Main.debuff[buffType] && !ModItemLib.TrueDebuff.Contains(buffType)) {
				player.buffImmune[buffIndex] = true;
			}
		}
		player.potionDelayTime = 0;
	}
}

public class LucidNightmares : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 850;
		Skill_Duration = ModUtils.ToSecond(4);
		Skill_Type = SkillTypeID.Skill_Summon;
	}
	public override void OnTrigger(Player player, SkillHandlePlayer skillplayer, int duration, int energy) {
		int damage = skillplayer.SkillDamage(53);
		for (int i = 0; i < 3; i++) {
			Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.UnitX.Vector2DistributeEvenly(3, 360, i) * Main.rand.NextFloat(4, 7), ModContent.ProjectileType<NightmaresProjectile>(), damage, 0, player.whoAmI);
		}
		player.AddBuff<AbyssalAbsorption>(duration);

	}
	public class AbyssalAbsorption : ModBuff {
		public override string Texture => ModTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultBuff();
		}
		public override void Update(Player player, ref int buffIndex) {
			player.ModPlayerStats().TrueDamage.Flat += 10;
		}
	}
}

public class SacrificialWormhole : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 950;
		Skill_Duration = ModUtils.ToSecond(14);
		Skill_Type = SkillTypeID.Skill_Summon;
	}
	public override void OnTrigger(Player player, SkillHandlePlayer skillplayer, int duration, int energy) {
		int damage = skillplayer.SkillDamage(50);
		Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero,
			ModContent.ProjectileType<SacrificialWormholeProjectile>(), damage, 0, player.whoAmI);
		player.AddBuff<LifeLoss>(ModUtils.ToSecond(60));
	}
	public class LifeLoss : ModBuff {
		public override string Texture => ModTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultDeBuff();
		}
		public override void Update(Player player, ref int buffIndex) {
			PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.MaxHP, .5f);
		}
	}
}
