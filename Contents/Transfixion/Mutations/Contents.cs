using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Transfixion.Mutations;
public class Tanky : ModMutation {
	public override void ApplyAdjustment(NPC npc) {
		RoguelikeGlobalNPC global = npc.GetGlobalNPC<RoguelikeGlobalNPC>();
		global.Adjust_Extra_Health += 5;
		global.Adjust_Extra_Defense.Flat += 100;
		global.Static_Endurance += .2f;
	}
}
public class Ninja : ModMutation {
	public override void ApplyAdjustment(NPC npc) {
		RoguelikeGlobalNPC global = npc.GetGlobalNPC<RoguelikeGlobalNPC>();
		global.Static_DodgeChance += .5f;
	}
	public override void ModifyHitPlayer(NPC npc, Player player, ref Player.HurtModifiers modifiers) {
		if (Main.rand.NextBool()) {
			modifiers.SourceDamage *= 4;
		}
	}
}
public class Sandbag : ModMutation {
	public override void ApplyAdjustment(NPC npc) {
		npc.GetGlobalNPC<RoguelikeGlobalNPC>().Adjust_Extra_Health += 10;
	}
}
public class Super : ModMutation {
	public override void ApplyAdjustment(NPC npc) {
		npc.GetGlobalNPC<RoguelikeGlobalNPC>().Adjust_Extra_Damage += .75f;
		npc.GetGlobalNPC<RoguelikeGlobalNPC>().Static_Endurance += .5f;
	}
}
public class Deadly : ModMutation {
	public override void ApplyAdjustment(NPC npc) {
		npc.GetGlobalNPC<RoguelikeGlobalNPC>().Adjust_Extra_Damage += 1;
	}
}
public class ExtraDeadly : ModMutation {
	public override void ApplyAdjustment(NPC npc) {
		npc.GetGlobalNPC<RoguelikeGlobalNPC>().Adjust_Extra_Damage += 3;
	}
}
public class Vampiric : ModMutation {
	public override void OnHitPlayer(NPC npc, Player player, Player.HurtInfo hurtInfo) {
		npc.Heal((npc.lifeMax - npc.life) / 4);
	}
}
public class DeathTouch : ModMutation {
	public override void OnHitPlayer(NPC npc, Player player, Player.HurtInfo hurtInfo) {
		if (player.statLife > 100) {
			player.statLife = 1;
			player.AddBuff(ModContent.BuffType<DeathTouch_Debuff>(), 600);
		}
	}
	public class DeathTouch_Debuff : ModBuff {
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultDeBuff();
		}
		public override void Update(Player player, ref int buffIndex) {
			player.ModPlayerStats().TakenDamage += 10;
		}
	}
}
public class Empowered : ModMutation {
	public override void ApplyAdjustment(NPC npc) {
		RoguelikeGlobalNPC global = npc.GetGlobalNPC<RoguelikeGlobalNPC>();
		global.Adjust_Extra_Damage += .5f;
		global.Adjust_Extra_Defense += 1.5f;
		global.Adjust_Extra_Defense.Base += 50;
		global.Static_PercentageDamage += .1f;
		global.Static_PositiveLifeRegen = (int)Math.Max(1, npc.lifeMax * .01f);
	}
}
public class Weakening : ModMutation {
	public override void OnHitPlayer(NPC npc, Player player, Player.HurtInfo hurtInfo) {
		player.AddBuff<Weakening_Debuff>(ModUtils.ToSecond(20));
	}
	public class Weakening_Debuff : ModBuff {
		public override string Texture => ModTexture.EMPTYDEBUFF;
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultDeBuff();
		}
		public override void Update(NPC npc, ref int buffIndex) {

		}
		public override void Update(Player player, ref int buffIndex) {
			player.ModPlayerStats().TakenDamage += .5f;
			player.ModPlayerStats().DebuffBuffTime += 2;
		}
	}
}
public class Energeric : ModMutation {
	public override void ApplyAdjustment(NPC npc) {
		RoguelikeGlobalNPC global = npc.GetGlobalNPC<RoguelikeGlobalNPC>();
		global.ExtraUpdate++;
	}
}
public class Omega : ModMutation {
	public override void SetStaticDefaults() {
		Super = true;
	}
	float[] Omega_AI;
	public override void SetDefault() {
		Omega_AI = new float[4];
	}
	public override void ApplyAdjustment(NPC npc) {
		RoguelikeGlobalNPC global = npc.GetGlobalNPC<RoguelikeGlobalNPC>();
		global.Adjust_Extra_Health += 5;
		global.Static_PercentageDamage += .3f;
		npc.buffImmune[BuffID.Ichor] = true;
		npc.buffImmune[BuffID.OnFire] = true;
		npc.buffImmune[BuffID.OnFire3] = true;
		npc.buffImmune[BuffID.Frostburn] = true;
		npc.buffImmune[BuffID.Frostburn2] = true;
		npc.buffImmune[BuffID.Frozen] = true;
		npc.buffImmune[BuffID.CursedInferno] = true;
		npc.buffImmune[BuffID.Cursed] = true;
		npc.buffImmune[BuffID.Electrified] = true;
		npc.buffImmune[BuffID.Confused] = true;
		npc.buffImmune[BuffID.Bleeding] = true;
	}
	public override bool DodgeCondition(NPC npc) {
		bool dodge = false;
		if (Main.rand.NextFloat() <= .3f) {
			dodge = true;
			npc.TargetClosest();
			npc.GetGlobalNPC<RoguelikeGlobalNPC>().InvincibilityFrame = 30 * Main.rand.Next(1, 4);
			if (npc.target < 0 || npc.target >= Main.maxNPCs) {
				return dodge;
			}
			Vector2 player_pos = Main.player[npc.target].Center;
			Vector2 new_pos = player_pos;
			if (npc.noTileCollide) {
				new_pos += Main.rand.NextVector2CircularEdge(400, 400) * Main.rand.NextFloat(.85f, 1.15f);
			}
			else {
				new_pos += Main.rand.NextVector2CircularEdge(400, 400) * Main.rand.NextFloat(.85f, 1.15f);
				bool outofTile;
				do {
					new_pos.Y -= npc.height;
					Point tilePos = npc.position.ToTileCoordinates();
					outofTile = WorldGen.TileEmpty(tilePos.X, tilePos.Y);
				} while (!outofTile);
			}
			npc.TeleportCommon(new_pos);
			npc.velocity = (player_pos - new_pos).SafeNormalize(Vector2.Zero) * npc.velocity.Length();
		}
		return dodge;
	}
	public override void PreAI(NPC npc) {
		if (Omega_AI[2] > 0) {
			npc.life = Math.Min(npc.lifeMax, npc.life++);
		}
		//Instant heal ability
		switch (Omega_AI[0]) {
			case 0:
				if (npc.GetLifePercent() <= .3f) {
					npc.Heal((int)(npc.lifeMax * .9f));
					Omega_AI[0]++;
				}
				break;
			case 1:
				if (npc.GetLifePercent() <= .1f) {
					npc.Heal((int)(npc.lifeMax * .9f));
					Omega_AI[0]++;
				}
				break;
		}
		//Regeneration ability
		switch (Omega_AI[1]) {
			case 0:
				if (npc.GetLifePercent() <= .5f) {
					Omega_AI[2] = ModUtils.ToSecond(15);
					Omega_AI[1]++;
				}
				break;
			case 1:
				if (npc.GetLifePercent() <= .3f) {
					Omega_AI[2] = ModUtils.ToSecond(15);
					Omega_AI[1]++;
				}
				break;
			case 2:
				if (npc.GetLifePercent() <= .15f) {
					Omega_AI[2] = ModUtils.ToSecond(15);
					Omega_AI[1]++;
				}
				break;
		}
	}
	public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers) {
		modifiers.SourceDamage -= Main.rand.NextFloat(.4f, .9f);
		if (npc.HasBuff<Omega_Protection>()) {
			return;
		}
	}
	public override void ModifyHitByProj(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers) {
		modifiers.SourceDamage -= Main.rand.NextFloat(.4f, .9f);
		if (npc.HasBuff<Omega_Protection>()) {
			return;
		}
	}
	public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit) {
		if (Main.rand.NextBool(5)) {
			npc.GetGlobalNPC<RoguelikeGlobalNPC>().InvincibilityFrame = 30 * Main.rand.Next(2, 5);
		}
		if (npc.HasBuff<Omega_Protection>()) {
			return;
		}
		if (Main.rand.NextBool(10)) {
			npc.AddBuff<Omega_Protection>(ModUtils.ToSecond(10));
		}
	}
	public override void OnHitByProj(NPC npc, Projectile proj, NPC.HitInfo hit) {
		if (Main.rand.NextBool(5)) {
			npc.GetGlobalNPC<RoguelikeGlobalNPC>().InvincibilityFrame = 30 * Main.rand.Next(2, 5);
		}
		if (npc.HasBuff<Omega_Protection>()) {
			return;
		}
		if (Main.rand.NextBool(10)) {
			npc.AddBuff<Omega_Protection>(ModUtils.ToSecond(10));
		}
	}
	public override void OnHitPlayer(NPC npc, Player player, Player.HurtInfo hurtInfo) {
		npc.GetGlobalNPC<RoguelikeGlobalNPC>().InvincibilityFrame = 30 * Main.rand.Next(2, 5);
	}
	public override bool CheckDead(NPC npc) {
		RoguelikeGlobalNPC global = npc.GetGlobalNPC<RoguelikeGlobalNPC>();
		if (Omega_AI[3] <= 0) {
			Omega_AI[3]++;
			npc.Heal(npc.lifeMax);
			return false;
		}
		return true;
	}
	public class Omega_Protection : ModBuff {
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultBuff();
		}
		public override void Update(NPC npc, ref int buffIndex) {
			npc.GetGlobalNPC<RoguelikeGlobalNPC>().Endurance += .9f;
		}
	}
}
