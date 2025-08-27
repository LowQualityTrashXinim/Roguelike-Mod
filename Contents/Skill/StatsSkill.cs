using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.Systems.Mutation;
using Roguelike.Common.Utils;
using Roguelike.Contents.Projectiles;
using Roguelike.Texture;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Skill;
public class DamageUp : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 50;
		Skill_Duration = 0;
		Skill_CoolDown = ModUtils.ToSecond(2);
		Skill_Type = SkillTypeID.Skill_Stats;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		skillplayer.SkillDamageWhileActive += 1f;
	}
}
public class GreaterDamageUp : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 200;
		Skill_Duration = 0;
		Skill_CoolDown = ModUtils.ToSecond(2);
		Skill_Type = SkillTypeID.Skill_Stats;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		skillplayer.SkillDamageWhileActive += 3f;
	}
}
public class Increases_3xDamage : ModSkill {
	public override string Texture => ModUtils.GetTheSameTextureAs<Increases_3xDamage>("PowerBank");
	public override void SetDefault() {
		Skill_EnergyRequire = 530;
		Skill_Duration = 8;
		Skill_CoolDown = ModUtils.ToSecond(15);
		Skill_Type = SkillTypeID.Skill_Stats;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.PureDamage, Additive: 4);
	}
	public override void OnEnded(Player player) {
		player.AddBuff(ModContent.BuffType<PowerBankDebuff>(), ModUtils.ToMinute(1));
	}
	public class PowerBankDebuff : ModBuff {
		public override string Texture => ModTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultDeBuff();
		}
		public override void Update(Player player, ref int buffIndex) {
			player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.EnergyRecharge, -.5f);
		}
	}
}
public class PowerSaver : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 560;
		Skill_Duration = 0;
		Skill_CoolDown = ModUtils.ToMinute(1);
		Skill_EnergyRequirePercentage = -.5f;
		Skill_Type = SkillTypeID.Skill_Stats;
	}
}
public class FastForward : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 200;
		Skill_Duration = 0;
		Skill_CoolDown = ModUtils.ToSecond(20);
		Skill_Type = SkillTypeID.Skill_Stats;
	}
	public override void ModifyNextSkillStats(out StatModifier energy, out StatModifier duration, out StatModifier cooldown) {
		energy = new();
		duration = new();
		cooldown = new();
		duration -= .5f;
		cooldown -= .5f;
	}
}
public class Skip1 : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 0;
		Skill_Duration = 0;
		Skill_CoolDown = ModUtils.ToSecond(15);
		Skill_Type = SkillTypeID.Skill_Stats;
	}
	public override void ModifySkillSet(Player player, SkillHandlePlayer modplayer, ref int index, ref StatModifier energy, ref StatModifier duration, ref StatModifier cooldown) {
		int[] currentskillset = modplayer.GetCurrentActiveSkillHolder();
		for (int i = index + 1; i < currentskillset.Length; i++) {
			ModSkill skill = SkillModSystem.GetSkill(currentskillset[i]);
			if (skill == null) {
				continue;
			}
			index = index + 1;
			energy.Base -= 300;
			break;
		}
	}
}

public class PowerCord : ModSkill {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<PowerCord>();
	public override void SetDefault() {
		Skill_EnergyRequire = 100;
		Skill_EnergyRequirePercentage = .25f;
		Skill_Duration = 0;
		Skill_CoolDown = 0;
		Skill_Type = SkillTypeID.Skill_Stats;
	}
	public override void OnEnded(Player player) {
		player.AddBuff(ModContent.BuffType<PowerCordBuff>(), ModUtils.ToSecond(12));
	}
	public class PowerCordBuff : ModBuff {
		public override string Texture => ModTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			Main.debuff[Type] = false;
		}
		public override void Update(Player player, ref int buffIndex) {
			player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.EnergyRecharge, 2.5f);
		}
	}
}
public class Procrastination : ModSkill {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<Procrastination>();
	public override void SetDefault() {
		Skill_EnergyRequire = 350;
		Skill_Duration = ModUtils.ToSecond(4);
		Skill_CoolDown = ModUtils.ToSecond(17);
		Skill_Type = SkillTypeID.Skill_Stats;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		player.AddBuff(BuffID.Stoned, 2);
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.PureDamage, 2.25f);
		player.endurance += .75f;
	}
}
public class SpeedDemon : ModSkill {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<SpeedDemon>();
	public override void SetDefault() {
		Skill_EnergyRequire = 330;
		Skill_Duration = 30;
		Skill_CoolDown = ModUtils.ToSecond(9);
		Skill_Type = SkillTypeID.Skill_Stats;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.AttackSpeed, 2.5f);
		modplayer.AddStatsToPlayer(PlayerStats.MovementSpeed, 1.5f);
	}
}
public class TranquilMind : ModSkill {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<TranquilMind>();
	public override void SetDefault() {
		Skill_EnergyRequire = 400;
		Skill_Duration = ModUtils.ToSecond(5);
		Skill_CoolDown = ModUtils.ToSecond(60);
		Skill_Type = SkillTypeID.Skill_Stats;
	}
}
public class InfiniteManaSupply : ModSkill {
	public override string Texture => ModUtils.GetTheSameTextureAs<InfiniteManaSupply>("AstralOvercharge");
	public override void SetDefault() {
		Skill_EnergyRequire = 220;
		Skill_Duration = ModUtils.ToSecond(.5f);
		Skill_CoolDown = ModUtils.ToSecond(6);
		Skill_Type = SkillTypeID.Skill_Stats;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		if (player.statMana < player.statManaMax2) {
			player.statMana += 10;
		}
	}
}
public class GuaranteedCrit : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 425;
		Skill_Duration = ModUtils.ToSecond(.5f);
		Skill_CoolDown = ModUtils.ToSecond(2.5f);
		Skill_Type = SkillTypeID.Skill_Stats;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		player.GetModPlayer<PlayerStatsHandle>().AlwaysCritValue++;
	}
}
public class RapidHealing : ModSkill {
	public override string Texture => ModUtils.GetTheSameTextureAs<RapidHealing>("RapidOverflow");
	public override void SetDefault() {
		Skill_EnergyRequire = 345;
		Skill_Duration = ModUtils.ToSecond(2);
		Skill_CoolDown = ModUtils.ToSecond(30);
		Skill_Type = SkillTypeID.Skill_Stats;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		SkillHandlePlayer modplayer = player.GetModPlayer<SkillHandlePlayer>();
		if (modplayer.Duration % 6 != 0) {
			return;
		}
		player.Heal(6);
	}
}
public class AdAstra : ModSkill {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<AdAstra>();
	public override void SetDefault() {
		Skill_EnergyRequire = 450;
		Skill_Duration = ModUtils.ToSecond(3);
		Skill_CoolDown = ModUtils.ToSecond(5);
		Skill_Type = SkillTypeID.Skill_Stats;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
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
			PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
			modplayer.AddStatsToPlayer(PlayerStats.AttackSpeed, Multiplicative: .1f);
			modplayer.AddStatsToPlayer(PlayerStats.PureDamage, Multiplicative: .1f);
			modplayer.AddStatsToPlayer(PlayerStats.Defense, Multiplicative: .1f);
		}
	}
}
public class BloodToPower : ModSkill {
	public override string Texture => ModUtils.GetTheSameTextureAs<BloodToPower>("SacrificialPact");
	public override void SetDefault() {
		Skill_EnergyRequire = 570;
		Skill_Duration = ModUtils.ToSecond(2);
		Skill_CoolDown = ModUtils.ToSecond(9);
		Skill_Type = SkillTypeID.Skill_Stats;
	}
	public override void OnTrigger(Player player, SkillHandlePlayer skillplayer, int duration, int cooldown, int energy) {
		int blood = player.statLife / 2;
		player.statLife -= blood;
		player.GetModPlayer<SkillHandlePlayer>().BloodToPower = blood;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.PureDamage, Additive: 1 + player.GetModPlayer<SkillHandlePlayer>().BloodToPower * .01f);
	}
}
public class Overclock : ModSkill {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<Overclock>();
	public override void SetDefault() {
		Skill_EnergyRequire = 635;
		Skill_Duration = ModUtils.ToSecond(1);
		Skill_CoolDown = ModUtils.ToSecond(9);
		Skill_Type = SkillTypeID.Skill_Stats;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.AttackSpeed, 1 + Math.Clamp(.5f * (skillplayer.MaximumDuration - skillplayer.Duration) / 50, 0, 7.5f));
	}
}
public class TerrorForm : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 900;
		Skill_Duration = ModUtils.ToSecond(4);
		Skill_CoolDown = ModUtils.ToSecond(12);
		Skill_Type = SkillTypeID.Skill_Stats;
	}
	public override void Update(Player player, SkillHandlePlayer skillplayer) {
		for (int i = 0; i < 2; i++) {
			Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, Main.rand.Next(new int[] { DustID.Shadowflame, DustID.Wraith, DustID.UltraBrightTorch }));
			dust.noGravity = true;
			dust.velocity = Vector2.UnitY * -Main.rand.NextFloat(3);
			dust.scale = Main.rand.NextFloat(0.75f, 1.25f);
		}
		player.statLife = Math.Clamp(player.statLife - 1, 1, player.statLifeMax2);
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		float percentage = 1 - player.statLife / (float)player.statLifeMax2;
		modplayer.AddStatsToPlayer(PlayerStats.PureDamage, 1.5f, Multiplicative: 1 + percentage);
		modplayer.AddStatsToPlayer(PlayerStats.CritChance, Multiplicative: 1 + percentage, Base: 25);
		modplayer.AddStatsToPlayer(PlayerStats.CritDamage, 2f, Multiplicative: 1 + percentage);
		modplayer.AddStatsToPlayer(PlayerStats.AttackSpeed, 1.5f, Multiplicative: 1 + percentage);
		modplayer.AddStatsToPlayer(PlayerStats.MovementSpeed, 1.35f, Multiplicative: 1 + percentage);
		modplayer.AddStatsToPlayer(PlayerStats.JumpBoost, 1.35f, Multiplicative: 1 + percentage);
	}
}
public class AllOrNothing : ModSkill {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<AllOrNothing>();
	public override void SetDefault() {
		Skill_EnergyRequirePercentage = 1;
		Skill_Duration = 1;
		Skill_CoolDown = ModUtils.ToMinute(15);
		Skill_CanBeSelect = false;
		Skill_Type = SkillTypeID.Skill_Stats;
	}
	public override void OnTrigger(Player player, SkillHandlePlayer skillplayer, int duration, int cooldown, int energy) {
		player.AddBuff(ModContent.BuffType<AllOrNothingBuff>(), ModUtils.ToSecond(5));
	}
	public class AllOrNothingBuff : ModBuff {
		public override string Texture => ModTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultDeBuff();
		}
		public override void Update(Player player, ref int buffIndex) {
			if (player.buffTime[buffIndex] <= 0) {
				player.Center.LookForHostileNPC(out List<NPC> npclist, 1000);
				foreach (NPC npc in npclist) {
					int direction = player.Center.X > npc.Center.X ? 1 : -1;
					int originDmg = (int)(npc.lifeMax * .1f);
					int dmg = originDmg;
					for (int i = 2; i < 17; i++) {
						if (Main.rand.NextBool(i)) {
							dmg += (int)(originDmg * Main.rand.NextFloat(.85f, 1.15f));
						}
					}
					player.StrikeNPCDirect(npc, npc.CalculateHitInfo(dmg, direction));
				}
			}
		}
	}
}
public class CoinFlip : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 0;
		Skill_Duration = 600;
		Skill_CoolDown = ModUtils.ToMinute(.1f);
		Skill_CanBeSelect = false;
		Skill_Type = SkillTypeID.Skill_Stats;
	}
	public override void OnTrigger(Player player, SkillHandlePlayer skillplayer, int duration, int cooldown, int energy) {
		if (Main.rand.NextBool()) {
			int chanceDecider = Main.rand.Next(10);
			//Positive effect
		}
		else {
			int chanceDecider = Main.rand.Next(10);
			//Negative effect
		}
	}
}
public class DiceRoll : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 0;
		Skill_Duration = 600;
		Skill_CoolDown = ModUtils.ToMinute(.1f);
		Skill_CanBeSelect = false;
		Skill_Type = SkillTypeID.Skill_Stats;
	}
	public override void OnTrigger(Player player, SkillHandlePlayer skillplayer, int duration, int cooldown, int energy) {
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
					Vector2 spawnPositionRandom = player.Center + Main.rand.NextVector2CircularEdge(400, 400) * Main.rand.NextFloat(.5f, 1.5f);
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
					MutationSystem.AddMutation(ModMutation.GetMutationType<Tanky>());
					NPC npc = NPC.NewNPCDirect(player.GetSource_FromThis("Skill"), player.Center + Vector2.One.Vector2DistributeEvenlyPlus(10, 360, i) * 600, NPCID.Ghost);
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
			PlayerStatsHandle handle = player.GetModPlayer<PlayerStatsHandle>();
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
			PlayerStatsHandle handle = player.GetModPlayer<PlayerStatsHandle>();
			handle.AddStatsToPlayer(PlayerStats.PureDamage, .85f);
			handle.AddStatsToPlayer(PlayerStats.AttackSpeed, .85f);
			handle.AddStatsToPlayer(PlayerStats.CritChance, .5f);
			handle.AddStatsToPlayer(PlayerStats.Defense, .5f);
			handle.AddStatsToPlayer(PlayerStats.MovementSpeed, .85f);
		}
	}
}
public class ProtectiveOnslaught : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 1000;
		Skill_Duration = 10;
		Skill_CoolDown = ModUtils.ToSecond(30);
		Skill_Type = SkillTypeID.Skill_Stats;
	}
	public override void OnTrigger(Player player, SkillHandlePlayer skillplayer, int duration, int cooldown, int energy) {
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
				player.Center.LookForHostileNPC(out List<NPC> npclist, 500);
				foreach (NPC npc in npclist) {
					player.StrikeNPCDirect(npc, npc.CalculateHitInfo(leftover * 100, player.Center.X > npc.Center.X ? -1 : 1, true, 20, damageVariation: true));
				}
			}
			player.Heal(player.statDefense);
		}
	}

}
