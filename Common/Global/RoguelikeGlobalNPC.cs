using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Common.General;
using Roguelike.Common.Systems.WorldSettingSystem;
using Roguelike.Common.Utils;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace Roguelike.Common.Global;
/// <summary>
/// Common word prefix need to know : <br/><br/>
/// If the value is a stats<br/>
/// "no prefix" - assumed this value always update and reset <br/>
/// "static" - This is a value that won't be changed or update, instead developer must handle or change this value themselves, useful for adding custom fixed stats like mutation or weapon that reduce said stats on hit permanently.<br/>
/// </summary>
internal class RoguelikeGlobalNPC : GlobalNPC {
	public override bool InstancePerEntity => true;

	public bool DRFromFatalAttack = false;
	public bool OneTimeDR = false;
	public int DRTimer = 0;
	public int ExtraUpdate = 0;
	//The below should be synced with netcode
	/// <summary>
	/// Have 3 slots much like Projectile.ai<br/>
	/// </summary>
	public float[] Global_AI = [4];
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
	/// Uses this along with <see cref="BelongToWho"/> to make it so that this NPC will die when the parent NPC is killed <br/>
	/// if you are making a undying minion. <br/>
	/// Otherwise make sure you handle this NPC properly/
	/// </summary>
	public bool IsAGhostEnemy = false;
	/// <summary>
	/// The whoAmI but to owner NPC, by default it is set to -1 meaning this NPC is not a minion<br/>
	/// The moment this value is set to a NPC to follow, this NPC will automatically be handled<br/>
	/// This NPC will automatically die upon the owner NPC die.
	/// </summary>
	public int BelongToWho = -1;

	public bool Deny_YouFromLoot = false;
	/// <summary>
	/// This doesn't reset anywhere, it is a static value that allow npc to regenerate hp
	/// </summary>
	public int Static_PositiveLifeRegen = 0;
	public int PositiveLifeRegenCount = 0;
	/// <summary>
	/// Set this to true if you don't want the mod to apply boss NPC fixed boss's stats
	/// </summary>
	public bool NPC_SpecialException = false;

	//Add value to this will grant NPC i-frame
	//Unlike player where you have to set both value
	public int InvincibilityFrame = 0;

	public float PercentageDamage = 0;

	/// <summary>
	/// This is a percentage damage block value, by default it is set to 1<br/>
	/// Setting it to 0.15f will mean that this NPC can't take more than 15% of their health as damage
	/// </summary>
	public float MaxDamageTaken = 1;
	/// <summary>
	/// This is a always reset value, will increases damage NPC via ModifyHit hook and can be applied to projectile
	/// </summary>
	public StatModifier DamageIncrease = new();
	/// <summary>
	/// This is always reset back to 0, if you are looking for static damage reduciton, use <see cref="Static_Endurance"/> instead
	/// </summary>
	public float Endurance = 0;
	/// <summary>
	/// NPC dodge chance, can only range from 1 to 100%<br/>
	/// This value is a always update reset value
	/// </summary>
	public float DodgeChance = 0;
	/// <summary>
	/// Static prefix mean that this won't be change during the update
	/// </summary>
	public float Static_Endurance = 0;
	/// <summary>
	/// NPC dodge chance, can only range from 1 to 100%<br/>
	/// This value is a non-update value
	/// </summary>
	public float Static_DodgeChance = 0;
	public float Static_PercentageDamage = 0;

	public StatModifier Static_DamageIncrease = new();

	public StatModifier StatDefense = new StatModifier();
	/// <summary>
	/// Use this for uniform debuff scaling<br/>
	/// Modify it as you would to any other statmodifier as the code within <see cref="RoguelikeGlobalNPC"/> will handle all
	/// </summary>
	public StatModifier Poison_Inner = StatModifier.Default;
	Dictionary<string, StatModifier> Poison_Global = new();
	/// <summary>
	/// Use this over <see cref="Poison_Inner"/> if the debuff change you made is global for your player
	/// </summary>
	/// <param name="key"></param>
	/// <param name="value"></param>
	public void Add_PoisonGlobal(string key, StatModifier value) {
		if (Poison_Global.ContainsKey(key)) {
			Poison_Global[key] = value;
		}
		else {
			Poison_Global.Add(key, value);
		}
	}
	/// <summary>
	/// Only run once in this class OnSpawn hook<br/>
	/// Use this if you want a easy way to add additive or flat health increases<br/>
	/// <br/>
	/// Best to use this in hook <see cref="GlobalNPC.ApplyDifficultyAndPlayerScaling"/><br/>
	/// Unexpected behavior may occur from trying to set in <see cref="GlobalNPC.SetDefaultsFromNetId(NPC)"/> hook<br/>
	/// This variable will only run once in <see cref="GlobalNPC.OnSpawn(NPC, IEntitySource)"/> hook
	/// </summary>
	public StatModifier Adjust_Extra_Health = StatModifier.Default;
	/// <summary>
	/// Only run once in this class OnSpawn hook<br/>
	/// Use this if you want a easy way to add additive or flat defenses increases<br/>
	/// <br/>
	/// Best to use this in hook <see cref="GlobalNPC.ApplyDifficultyAndPlayerScaling"/><br/>
	/// Unexpected behavior may occur from trying to set in <see cref="GlobalNPC.SetDefaultsFromNetId(NPC)"/> hook<br/>
	/// This variable will only run once in <see cref="GlobalNPC.OnSpawn(NPC, IEntitySource)"/> hook
	/// </summary>
	public StatModifier Adjust_Extra_Defense = StatModifier.Default;
	/// <summary>
	/// Only run once in this class OnSpawn hook<br/>
	/// Use this if you want a easy way to add additive or flat damage increases<br/>
	/// <br/>
	/// Best to use this in hook <see cref="GlobalNPC.ApplyDifficultyAndPlayerScaling"/><br/>
	/// Unexpected behavior may occur from trying to set in <see cref="GlobalNPC.SetDefaultsFromNetId(NPC)"/> hook<br/>
	/// This variable will only run once in <see cref="GlobalNPC.OnSpawn(NPC, IEntitySource)"/> hook
	/// </summary>
	public StatModifier Adjust_Extra_Damage = StatModifier.Default;
	public StatModifier Poison_Additional = StatModifier.Default - 1;
	public float Poison_Additional_Timer = 0;
	public float Poison_Additional_MaxTimer = 60;
	public int Amount_CurrentDebuffInflicted = 0;
	public override void SetDefaults(NPC entity) {
		StatDefense = new();
		DamageIncrease = new();
	}
	public override void OnSpawn(NPC npc, IEntitySource source) {
		npc.lifeMax = (int)Adjust_Extra_Health.ApplyTo(npc.lifeMax);
		npc.defense = (int)Adjust_Extra_Defense.ApplyTo(npc.defense);
		npc.damage = (int)Adjust_Extra_Damage.ApplyTo(npc.damage);
	}
	public int TimeLimit = -1;
	public override void ResetEffects(NPC npc) {
		//var player = Main.player[npc.target];
		//if (npc.Center.IsCloseToPosition(player.Center, 1500)) {
		//	npc.timeLeft = 600;
		//}
		StatDefense = new();
		DamageIncrease = new();
		MaxDamageTaken = 1;
		DodgeChance = 0;
		if (IsAGhostEnemy) {
			npc.dontTakeDamage = true;
			npc.chaseable = false;
			npc.life = npc.lifeMax;
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
	public override void UpdateLifeRegen(NPC npc, ref int damage) {
		foreach (var item in Poison_Global.Values) {
			Poison_Inner = Poison_Inner.CombineWith(item);
		}
		npc.lifeRegen = (int)((npc.lifeRegen - Poison_Inner.Base) * Poison_Inner.Additive * Poison_Inner.Multiplicative - Poison_Inner.Flat);
		Poison_Inner = StatModifier.Default;
		Amount_CurrentDebuffInflicted = 0;
		for (int i = 0; i < npc.buffType.Length; i++) {
			if (npc.buffType[i] <= 0) {
				continue;
			}
			if (Main.debuff[npc.buffType[i]]) {
				Amount_CurrentDebuffInflicted++;
			}
		}
		if (++Poison_Additional_Timer >= Poison_Additional_MaxTimer) {
			int additional = (int)((npc.lifeRegen - Poison_Additional.Base) * Poison_Additional.Additive * Poison_Additional.Multiplicative - Poison_Additional.Flat) * -1;
			npc.lifeRegen -= additional;
			Poison_Additional = StatModifier.Default - 1;
			Poison_Additional_Timer = 0;
		}
		damage = (int)Math.Ceiling(npc.lifeRegen * -.5f);
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
	public override Color? GetAlpha(NPC npc, Color drawColor) {
		if (IsAGhostEnemy) {
			drawColor.A = 0;
			drawColor.ScaleRGB(.25f);
			drawColor.B = 255;
			return drawColor;
		}
		return base.GetAlpha(npc, drawColor);
	}
	public override bool ModifyCollisionData(NPC npc, Rectangle victimHitbox, ref int immunityCooldownSlot, ref MultipliableFloat damageMultiplier, ref Rectangle npcHitbox) {
		if (Main.rand.NextFloat() <= DodgeChance) {
			InvincibilityFrame = 30;
			return false;
		}
		return base.ModifyCollisionData(npc, victimHitbox, ref immunityCooldownSlot, ref damageMultiplier, ref npcHitbox);
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
			npc.life = Math.Clamp(npc.life + Static_PositiveLifeRegen, 0, npc.lifeMax);
		}
		InvincibilityFrame = ModUtils.CountDown(InvincibilityFrame);
	}
	public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers) {
		modifiers.FinalDamage.Flat += (int)(target.statLifeMax2 * (PercentageDamage + Static_PercentageDamage));
		modifiers.SourceDamage = modifiers.SourceDamage.CombineWith(DamageIncrease.CombineWith(Static_DamageIncrease));
	}
	public int ResistHitCount = 0;
	public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers) {
		NPC_Debuff(npc, ref modifiers);
	}
	public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers) {
		NPC_Debuff(npc, ref modifiers);
	}
	private void NPC_Debuff(NPC npc, ref NPC.HitModifiers modifiers) {
		if (MaxDamageTaken < 1) {
			modifiers.SetMaxDamage((int)(npc.lifeMax * MaxDamageTaken));
		}
		modifiers.Defense = modifiers.Defense.CombineWith(StatDefense);
		modifiers.SourceDamage *= Math.Clamp(1 - (Endurance + Static_Endurance), 0, 1f);
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
		if (npc.boss)
			Main.NewText($"[c/{Color.Red.Hex3()}:<Everlasting> World progression has been increased by 1.]");
	}
	public override bool CheckDead(NPC npc) {
		if (ModContent.GetInstance<DifficultySettingSystem>().World_EnemyRevive) {
			bool check = Main.rand.NextBool(3) && !npc.boss;
			if (check) {
				npc.life = npc.lifeMax;
			}
			return check;
		}
		return base.CheckDead(npc);
	}
	public override bool PreKill(NPC npc) {
		return !Deny_YouFromLoot;
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
