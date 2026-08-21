using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Common.General;
using Roguelike.Common.Utils;
using System;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace Roguelike.Common.Global;
internal class RoguelikeGlobalNPC : GlobalNPC {
	public override bool InstancePerEntity => true;

	public bool DRFromFatalAttack = false;
	public bool OneTimeDR = false;
	public int DRTimer = 0;
	public int ExtraUpdate = 0;
	/// <summary>
	/// Use this for always update velocity
	/// </summary>
	public float VelocityMultiplier = 1;
	/// <summary>
	/// Use this for permanent effect
	/// </summary>
	public float static_velocityMultiplier = 1;
	/// <summary>
	/// Set this to true if your NPC is a ghost NPC which can't be kill<br/>
	/// Uses this along with <see cref="BelongToWho"/> to make it so that this NPC will die when the parent NPC is killed
	/// </summary>
	public bool IsAGhostEnemy = false;
	public int BelongToWho = -1;
	/// <summary>
	/// Currently this is broken, highly not recommand to use it.
	/// </summary>
	public bool CanDenyYouFromLoot = false;
	/// <summary>
	/// This doesn't reset anywhere, it is a static value that allow npc to regenerate hp
	/// </summary>
	public int PositiveLifeRegen = 0;
	public int PositiveLifeRegenCount = 0;
	/// <summary>
	/// Set this to true if you don't want the mod to apply boss NPC fixed boss's stats
	/// </summary>
	public bool NPC_SpecialException = false;
	public int InvincibilityFrame = 0;

	public StatModifier Static_PercentageDamage = new();

	public float MaxDamageTaken = 1;
	/// <summary>
	/// This is always reset back to 0, if you are looking for static damage reduciton, use <see cref="Static_Endurance"/> instead
	/// </summary>
	public float Endurance = 0;
	public float Static_Endurance = 0;

	public StatModifier DamageIncrease = new();
	public StatModifier StatDefense = new StatModifier();
	public override void SetDefaults(NPC entity) {
		StatDefense = new();
		DamageIncrease = new();
	}
	public override void ResetEffects(NPC npc) {
		//var player = Main.player[npc.target];
		//if (npc.Center.IsCloseToPosition(player.Center, 1500)) {
		//	npc.timeLeft = 600;
		//}
		//npc.buffImmune[ModContent.BuffType<Anti_Immunity>()] = false;
		StatDefense = new();
		DamageIncrease = new();
		MaxDamageTaken = 1;
		if (IsAGhostEnemy) {
			npc.dontTakeDamage = true;
			npc.chaseable = false;
		}
		if (--DRTimer <= 0) {
			DRFromFatalAttack = false;
		}
		else {
			DRFromFatalAttack = true;
		}
		Endurance = 0;
		//if (npc.boss) {
		//	if (npc.life <= npc.lifeMax / 2) {
		//		DamageIncrease += 1;
		//		MaxDamageTaken -= .99f;
		//	}
		//}
	}
	public int Grapefruit = 0;
	public override bool? CanBeHitByItem(NPC npc, Player player, Item item) {
		if (IsAGhostEnemy || InvincibilityFrame > 0) {
			return false;
		}
		return base.CanBeHitByItem(npc, player, item);
	}
	public override bool CanBeHitByNPC(NPC npc, NPC attacker) {
		if (IsAGhostEnemy || InvincibilityFrame > 0) {
			return false;
		}
		return base.CanBeHitByNPC(npc, attacker);
	}
	public override bool? CanBeHitByProjectile(NPC npc, Projectile projectile) {
		if (IsAGhostEnemy || InvincibilityFrame > 0) {
			return false;
		}
		return base.CanBeHitByProjectile(npc, projectile);
	}
	public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
		LeadingConditionRule rule = new(new DenyYouFromLoot());
		foreach (var item in npcLoot.Get()) {
			item.OnSuccess(rule);
		}
	}
	public override Color? GetAlpha(NPC npc, Color drawColor) {
		if (IsAGhostEnemy) {
			drawColor.A = 0;
			drawColor.ScaleRGB(.25f);
			drawColor.B = 255;
			return drawColor;
		}
		return base.GetAlpha(npc, drawColor);
	}
	public override bool PreAI(NPC npc) {
		if (VelocityMultiplier != 0) {
			npc.velocity /= VelocityMultiplier + static_velocityMultiplier - 1;
		}
		else {
			npc.velocity /= .001f;
		}
		return base.PreAI(npc);
	}
	public override void PostAI(NPC npc) {
		if (VelocityMultiplier != 0) {
			npc.velocity *= VelocityMultiplier + static_velocityMultiplier - 1;
		}
		else {
			npc.velocity *= .001f;
		}
		VelocityMultiplier = 1;
		if (BelongToWho >= 0 && BelongToWho < Main.maxNPCs) {
			var parent = Main.npc[BelongToWho];
			if (parent != null) {
				if (!parent.active || parent.life <= 0) {
					npc.life = 0;
					npc.realLife = 0;
					npc.active = false;
					return;
				}
			}
			else {
				BelongToWho = -1;
			}
		}
		if (++PositiveLifeRegenCount >= 60) {
			PositiveLifeRegenCount = 0;
			npc.life = Math.Clamp(npc.life + PositiveLifeRegen, 0, npc.lifeMax);
		}
		InvincibilityFrame = ModUtils.CountDown(InvincibilityFrame);
	}
	public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers) {
		modifiers.FinalDamage.Flat += (int)(target.statLifeMax2 * .1f);
		modifiers.SourceDamage = modifiers.SourceDamage.CombineWith(DamageIncrease);
	}
	public int ResistHitCount = 0;
	public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers) {
		NPC_Debuff(npc, ref modifiers);
	}
	public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers) {
		NPC_Debuff(npc, ref modifiers);
	}
	private void NPC_Debuff(NPC npc, ref NPC.HitModifiers modifiers) {
		if (MaxDamageTaken >= 1) {
			return;
		}
		modifiers.SetMaxDamage((int)(npc.lifeMax * MaxDamageTaken));
		modifiers.Defense = modifiers.Defense.CombineWith(StatDefense);
		modifiers.SourceDamage *= Math.Clamp(1 - Endurance, 0, 1f);
		modifiers.SourceDamage *= Math.Clamp(1 - Static_Endurance, 0, 1f);
		if (--ResistHitCount > 0) {
			modifiers.SetMaxDamage(1);
		}
	}
	public int HitCount = 0;
	public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone) {
		HitCount++;
	}
	public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) {
		HitCount++;
	}
	public override void OnKill(NPC npc) {
		int playerIndex = npc.lastInteraction;
		if (!Main.player[playerIndex].active || Main.player[playerIndex].dead) {
			playerIndex = npc.FindClosestPlayer();
		}
		var player = Main.player[playerIndex];
		player.GetModPlayer<PlayerStatsHandle>().successfullyKillNPCcount++;
		player.GetModPlayer<PlayerStatsHandle>().NPC_HitCount = HitCount;
	}
	public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
		if (InvincibilityFrame > 0 && InvincibilityFrame % 5 == 0) {
			return false;
		}
		//TODO : this is very broken, I couldn't get the outline to work so I gave up
		//if (npc.boss) {
		//	Main.instance.LoadNPC(npc.type);
		//	Texture2D texture = TextureAssets.Npc[npc.type].Value;
		//	SpriteEffects effect = SpriteEffects.None;
		//	Vector2 origin = npc.frame.Size() * .5f;
		//	Vector2 drawpos = npc.position - Main.screenPosition;
		//	spriteBatch.Draw(texture, drawpos + Vector2.One * 3, npc.frame, Color.Red * .25f, npc.rotation, origin, npc.scale, effect, 0);
		//	spriteBatch.Draw(texture, drawpos - Vector2.One * 3, npc.frame, Color.Red * .25f, npc.rotation, origin, npc.scale, effect, 0);
		//	spriteBatch.Draw(texture, drawpos + Vector2.One.Add(-2, 0) * 3, npc.frame, Color.Red * .25f, npc.rotation, origin, npc.scale, effect, 0);
		//	spriteBatch.Draw(texture, drawpos + Vector2.One.Add(0, -2) * 3, npc.frame, Color.Red * .25f, npc.rotation, origin, npc.scale, effect, 0);
		//}
		return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
	}
}
public class RoguelikeNPCModSystem : ModSystem {
	public override void Load() {
		On_NPC.UpdateNPC += On_NPC_UpdateNPC;
	}

	private void On_NPC_UpdateNPC(On_NPC.orig_UpdateNPC orig, NPC self, int i) {
		if (self.TryGetGlobalNPC(out RoguelikeGlobalNPC global)) {
			int amount = global.ExtraUpdate;
			for (int l = 0; l < amount; l++) {
				orig(self, i);
			}
		}
		orig(self, i);
	}
}
