using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Contents.Projectiles;
using Roguelike.Texture;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Transfixion.Skill;
public class BloodToPower : ModSkill {
	public override string Texture => ModUtils.GetTheSameTextureAs<BloodToPower>("SacrificialPact");
	public override void SetDefault() {
		Skill_EnergyRequire = 570;
		Skill_Duration = ModUtils.ToSecond(2);
		Skill_Type = SkillTypeID.Skill_Empowered;
	}
	public override void OnTrigger(Player player, SkillHandlePlayer skillplayer, int duration, int energy) {
		int blood = player.statLife / 2;
		player.statLife -= blood;
		player.GetModPlayer<SkillHandlePlayer>().BloodToPower = blood;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.PureDamage, Additive: 1 + player.GetModPlayer<SkillHandlePlayer>().BloodToPower * .01f);
	}
}

public class GuaranteedCrit : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 425;
		Skill_Duration = ModUtils.ToSecond(.5f);
		Skill_Type = SkillTypeID.Skill_Empowered;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		player.GetModPlayer<PlayerStatsHandle>().AlwaysCritValue++;
	}
}
public class AdAstra : ModSkill {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<AdAstra>();
	public override void SetDefault() {
		Skill_EnergyRequire = 450;
		Skill_Duration = ModUtils.ToSecond(3);
		Skill_Type = SkillTypeID.Skill_Empowered;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		var modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.PureDamage, Additive: 6f);
	}
	public override void OnEnded(Player player) {
		player.AddBuff(ModContent.BuffType<AdAstraDebuff>(), ModUtils.ToSecond(15));
	}
	public class AdAstraDebuff : ModBuff {
		public override string Texture => ModTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultDeBuff();
		}
		public override void Update(Player player, ref int buffIndex) {
			var modplayer = player.GetModPlayer<PlayerStatsHandle>();
			modplayer.AddStatsToPlayer(PlayerStats.AttackSpeed, Multiplicative: .1f);
			modplayer.AddStatsToPlayer(PlayerStats.PureDamage, Multiplicative: .1f);
			modplayer.AddStatsToPlayer(PlayerStats.Defense, Multiplicative: .1f);
		}
	}
}
public class Procrastination : ModSkill {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<Procrastination>();
	public override void SetDefault() {
		Skill_EnergyRequire = 350;
		Skill_Duration = ModUtils.ToSecond(4);
		Skill_Type = SkillTypeID.Skill_Empowered;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		player.AddBuff(BuffID.Stoned, 2);
		var modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.PureDamage, 2.25f);
		player.endurance += .75f;
	}
}
public class Increases_3xDamage : ModSkill {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<Increases_3xDamage>();
	public override void SetDefault() {
		Skill_EnergyRequire = 400;
		Skill_Duration = 120;
		Skill_Type = SkillTypeID.Skill_Empowered;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.PureDamage, Multiplicative: 3);
	}
}
public class SpeedDemon : ModSkill {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<SpeedDemon>();
	public override void SetDefault() {
		Skill_EnergyRequire = 330;
		Skill_Duration = 30;
		Skill_Type = SkillTypeID.Skill_Empowered;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		var modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.AttackSpeed, 2.5f);
		modplayer.AddStatsToPlayer(PlayerStats.MovementSpeed, 1.5f);
	}
}
public class InfiniteManaSupply : ModSkill {
	public override string Texture => ModUtils.GetTheSameTextureAs<InfiniteManaSupply>("AstralOvercharge");
	public override void SetDefault() {
		Skill_EnergyRequire = 220;
		Skill_Duration = ModUtils.ToSecond(.5f);
		Skill_Type = SkillTypeID.Skill_Empowered;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		if (player.statMana < player.statManaMax2) {
			player.statMana += 10;
		}
	}
}
public class RapidHealing : ModSkill {
	public override string Texture => ModUtils.GetTheSameTextureAs<RapidHealing>("RapidOverflow");
	public override void SetDefault() {
		Skill_EnergyRequire = 345;
		Skill_Duration = ModUtils.ToSecond(2);
		Skill_Type = SkillTypeID.Skill_Empowered;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		player.ModPlayerStats().Rapid_LifeRegen += 1;
	}
}
public class Overclock : ModSkill {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<Overclock>();
	public override void SetDefault() {
		Skill_EnergyRequire = 635;
		Skill_Duration = ModUtils.ToSecond(1);
		Skill_Type = SkillTypeID.Skill_Empowered;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.AttackSpeed, 1 + Math.Clamp(.5f * (skillplayer.MaximumDuration - skillplayer.Duration) / 50, 0, 7.5f));
	}
}

public class TerrorForm : ModSkill {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<TerrorForm>();
	public override void SetDefault() {
		Skill_EnergyRequire = 900;
		Skill_Duration = ModUtils.ToSecond(4);
		Skill_Type = SkillTypeID.Skill_Empowered;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		for (int i = 0; i < 2; i++) {
			var dust = Dust.NewDustDirect(player.position, player.width, player.height, Main.rand.Next(new int[] { DustID.Shadowflame, DustID.Wraith, DustID.UltraBrightTorch }));
			dust.noGravity = true;
			dust.velocity = Vector2.UnitY * -Main.rand.NextFloat(3);
			dust.scale = Main.rand.NextFloat(0.75f, 1.25f);
		}
		player.statLife = Math.Clamp(player.statLife - 1, 1, player.statLifeMax2);
		var modplayer = player.GetModPlayer<PlayerStatsHandle>();
		float percentage = 1 - player.statLife / (float)player.statLifeMax2;
		modplayer.AddStatsToPlayer(PlayerStats.PureDamage, 1.5f, Multiplicative: 1 + percentage);
		modplayer.AddStatsToPlayer(PlayerStats.CritChance, Multiplicative: 1 + percentage, Base: 25);
		modplayer.AddStatsToPlayer(PlayerStats.CritDamage, 2f, Multiplicative: 1 + percentage);
		modplayer.AddStatsToPlayer(PlayerStats.AttackSpeed, 1.5f, Multiplicative: 1 + percentage);
		modplayer.AddStatsToPlayer(PlayerStats.MovementSpeed, 1.35f, Multiplicative: 1 + percentage);
		modplayer.AddStatsToPlayer(PlayerStats.JumpBoost, 1.35f, Multiplicative: 1 + percentage);
	}
}
public class ProtectiveOnslaught : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 1000;
		Skill_Duration = 10;
		Skill_Type = SkillTypeID.Skill_Empowered;
	}
	public override void OnTrigger(Player player, SkillHandlePlayer skillplayer, int duration, int energy) {
		player.AddBuff(ModContent.BuffType<ProtectiveOnslaught_Buff>(), ModUtils.ToSecond(10));
	}
	class ProtectiveOnslaught_ModPlayer : ModPlayer {
		public int HitCount = 0;
		public int Delay = 0;
		public override void ResetEffects() {
			if (!Player.HasBuff<ProtectiveOnslaught_Buff>()) {
				HitCount = 0;
			}
			Delay = ModUtils.CountDown(Delay);
		}
		public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
			if (Player.HasBuff<ProtectiveOnslaught_Buff>() && Delay <= 0) {
				HitCount++;
				Delay = 6;
			}
		}
		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			if (Player.HasBuff<ProtectiveOnslaught_Buff>() && Delay <= 0 && proj.Check_ItemTypeSource(Player.HeldItem.type) && !proj.minion) {
				HitCount++;
				Delay = 6;
			}
		}
	}
	class ProtectiveOnslaught_Buff : ModBuff {
		public override string Texture => ModTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultBuff();
		}
		public override void Update(Player player, ref int buffIndex) {
			player.statDefense += player.GetModPlayer<ProtectiveOnslaught_ModPlayer>().HitCount;
			player.ModPlayerStats().AddStatsToPlayer(PlayerStats.PureDamage, 1 + player.statDefense * .001f);
			if (player.buffTime[buffIndex] <= 0) {
				OnEnded(player);
			}
		}
		public void OnEnded(Player player) {
			player.GetModPlayer<ProtectiveOnslaught_ModPlayer>().HitCount = 0;
			if (player.statLifeMax2 <= player.statDefense) {
				for (int i = 0; i < 300; i++) {
					int smokedust = Dust.NewDust(player.Center, 0, 0, DustID.Smoke);
					Main.dust[smokedust].noGravity = true;
					Main.dust[smokedust].velocity = Main.rand.NextVector2Circular(25, 25);
					Main.dust[smokedust].scale = Main.rand.NextFloat(.75f, 2f);
					int dust = Dust.NewDust(player.Center, 0, 0, DustID.Torch);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity = Main.rand.NextVector2Circular(25, 25);
					Main.dust[dust].scale = Main.rand.NextFloat(.75f, 2f);
				}
				int leftover = player.statDefense - player.statLifeMax2;
				player.Center.LookForHostileNPC(out var npclist, 500);
				foreach (var npc in npclist) {
					player.StrikeNPCDirect(npc, npc.CalculateHitInfo(leftover * 100, player.Center.X > npc.Center.X ? -1 : 1, true, 20, damageVariation: true));
				}
			}
			player.Heal(player.statDefense);
		}
	}
}
public class CoinFlip : ModSkill {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<CoinFlip>();
	public override void SetDefault() {
		Skill_EnergyRequire = 200;
		Skill_Duration = 600;
		Skill_CanBeSelect = false;
		Skill_Type = SkillTypeID.Skill_Empowered;
	}
	public override void OnTrigger(Player player, SkillHandlePlayer skillplayer, int duration, int energy) {
		if (Main.rand.NextBool()) {
			player.AddBuff<CoinFlipBuff>(duration * 10);
		}
		else {
			player.AddBuff<CoinFlipDebuff>(duration * 10);
		}
	}
	public class CoinFlipBuff : ModBuff {
		public override string Texture => ModTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultBuff();
		}
		public override void Update(Player player, ref int buffIndex) {
			if (player.buffTime[buffIndex] <= 0) {
				player.Heal(400);
			}
			player.ModPlayerStats().AddStatsToPlayer(PlayerStats.PureDamage, Additive: 2);
		}
	}
	public class CoinFlipDebuff : ModBuff {
		public override string Texture => ModTexture.EMPTYDEBUFF;
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultBuff();
		}
		public override void Update(Player player, ref int buffIndex) {
			player.ModPlayerStats().AddStatsToPlayer(PlayerStats.PureDamage, Additive: .1f);
			player.ModPlayerStats().AddStatsToPlayer(PlayerStats.Defense, Additive: .1f);
		}
	}
}
public class DiceRoll : ModSkill {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<DiceRoll>();
	public override void SetDefault() {
		Skill_EnergyRequire = 0;
		Skill_Duration = 600;
		Skill_CanBeSelect = false;
		Skill_Type = SkillTypeID.Skill_Empowered;
	}
	public override void OnTrigger(Player player, SkillHandlePlayer skillplayer, int duration, int energy) {
		int chance = Main.rand.Next(7);//0 , 1 , 2, 3, 4, 5
		switch (chance) {
			case 0:
				Main.NewText("World smile upon your fate");
				player.AddBuff<DiceRollBuff>(ModUtils.ToMinute(1));
				break;
			case 1:
				Main.NewText("World smile upon your fate");
				for (int i = 0; i < 32; i++) {
					Projectile.NewProjectile(player.GetSource_FromThis("Skill"), player.Center, Vector2.One.Vector2DistributeEvenlyPlus(32, 360, i), ProjectileID.ShadowBeamFriendly, 100, 10, player.whoAmI);
				}
				break;
			case 2:
				Main.NewText("World smile upon your fate");
				int amount = player.statLifeMax2 / 20 + 1;
				for (int i = 0; i < amount; i++) {
					var spawnPositionRandom = player.Center + Main.rand.NextVector2CircularEdge(400, 400) * Main.rand.NextFloat(.5f, 1.5f);
					Item.NewItem(player.GetSource_FromThis("Skill"), spawnPositionRandom, ItemID.Heart);
				}
				break;
			case 3:
				Main.NewText("World smile with malice");
				for (int i = 0; i < 20; i++) {
					Projectile.NewProjectile(player.GetSource_FromThis("Skill"), player.Center + Vector2.One.Vector2DistributeEvenlyPlus(20, 360, i) * 600, Vector2.One.Vector2DistributeEvenlyPlus(32, 360, i), ModContent.ProjectileType<NegativeLifeProjectile>(), 0, 10, player.whoAmI);
				}
				break;
			case 4:
				Main.NewText("World smile with malice");
				for (int i = 0; i < 10; i++) {
					var npc = NPC.NewNPCDirect(player.GetSource_FromThis("Skill"), player.Center + Vector2.One.Vector2DistributeEvenlyPlus(10, 360, i) * 600, NPCID.Ghost);
					npc.GetGlobalNPC<RoguelikeGlobalNPC>().CanDenyYouFromLoot = true;
				}
				break;
			case 5:
				Main.NewText("World smile with malice");
				player.AddBuff<DiceRollDebuff>(ModUtils.ToMinute(1));
				break;
			default:
				Main.NewText("Error detection. Result to fall back method");
				break;
		}
	}
	public class DiceRollBuff : ModBuff {
		public override string Texture => ModTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultDeBuff(true);
		}
		public override void Update(Player player, ref int buffIndex) {
			var handle = player.GetModPlayer<PlayerStatsHandle>();
			handle.AddStatsToPlayer(PlayerStats.PureDamage, 1.15f);
			handle.AddStatsToPlayer(PlayerStats.AttackSpeed, 1.15f);
			handle.AddStatsToPlayer(PlayerStats.CritChance, 1.25f);
			handle.AddStatsToPlayer(PlayerStats.Defense, 1.25f);
			handle.AddStatsToPlayer(PlayerStats.MovementSpeed, 1.85f);
		}
	}
	public class DiceRollDebuff : ModBuff {
		public override string Texture => ModTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultDeBuff(true);
		}
		public override void Update(Player player, ref int buffIndex) {
			var handle = player.GetModPlayer<PlayerStatsHandle>();
			handle.AddStatsToPlayer(PlayerStats.PureDamage, .85f);
			handle.AddStatsToPlayer(PlayerStats.AttackSpeed, .85f);
			handle.AddStatsToPlayer(PlayerStats.CritChance, .5f);
			handle.AddStatsToPlayer(PlayerStats.Defense, .5f);
			handle.AddStatsToPlayer(PlayerStats.MovementSpeed, .85f);
		}
	}
}
