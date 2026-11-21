using Mono.Cecil;
using Roguelike.Common.Utils;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using Terraria;
using Terraria.DataStructures;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Roguelike.Common.Global;
using Microsoft.Xna.Framework;

namespace Roguelike.Contents.Transfixion.Mutations;
internal class MutationSystem : ModSystem {
	private static List<ModMutation> list_mutation = new();
	public override void Load() {
		On_NPC.NewNPC += On_NPC_NewNPC;
	}
	private int On_NPC_NewNPC(On_NPC.orig_NewNPC orig, IEntitySource source, int X, int Y, int Type, int Start, float ai0, float ai1, float ai2, float ai3, int Target) {
		int whoAmI = orig(source, X, Y, Type, Start, ai0, ai1, ai2, ai3, Target);





		return whoAmI;
	}

	public static short Register(ModMutation item) {
		ModTypeLookup<ModMutation>.Register(item);
		list_mutation.Add(item);
		return (short)(list_mutation.Count - 1);
	}
	public static ModMutation GetMutation(int ID) => ID > -1 && ID < list_mutation.Count ? list_mutation[ID] : null;
}
public abstract class ModMutation : ModType {
	public short Type;
	public bool Super = false;
	#region never to be touched
	protected override sealed void Register() {
		SetStaticDefaults();
		Type = MutationSystem.Register(this);
	}
	public override sealed bool Equals(object obj) {
		return base.Equals(obj);
	}
	public sealed override int GetHashCode() {
		return base.GetHashCode();
	}
	public sealed override bool IsLoadingEnabled(Mod mod) {
		return base.IsLoadingEnabled(mod);
	}
	public sealed override void SetupContent() {
		base.SetupContent();
	}
	public sealed override string ToString() {
		return base.ToString();
	}
	public sealed override void Unload() {
		base.Unload();
	}
	public sealed override void Load() {
	}
	#endregion
	//The purpose of localization here is for both player to know the name and also to later develop a in-game database that player can view.
	//The data base will be merged with the info tab section in the mod
	public string DisplayName => ModUtils.LocalizationText("Mutation", "Name");
	public string Description => ModUtils.LocalizationText("Mutation", "Desc");
	/// <summary>
	/// Use this to modify stats. However don't do flat modification as it may result in unexpected behavior.<br/>
	/// Instead <see cref="RoguelikeGlobalNPC.Extra_Health"/> to do modification or adjustment
	/// </summary>
	/// <param name="npc"></param>
	public virtual void ApplyAdjustment(NPC npc) {

	}
	/// <summary>
	/// Last ditch modify, use this if you want to do final modification to stats
	/// </summary>
	/// <param name="npc"></param>
	/// <param name="source"></param>
	public virtual void OnSpawn(NPC npc, IEntitySource source) {

	}
	public virtual void PreAI(NPC npc) {

	}
	public virtual void PostAI(NPC npc) {

	}
	public virtual void OnHitPlayer(NPC npc, Player player, Player.HurtInfo hurtInfo) {

	}
	public virtual void ModifyHitPlayer(NPC npc, Player player, ref Player.HurtModifiers modifiers) {

	}
	/// <summary>
	/// Whether or not an NPC should be killed when it reaches 0 health.<br/> 
	/// Return false to stop the NPC from being killed. <br/>
	/// Returns true by default.
	/// </summary>
	/// <param name="npc"></param>
	/// <returns></returns>
	public virtual bool CheckDead(NPC npc) {
		return true;
	}
	public virtual void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers) {

	}
	public virtual void ModifyHitByProj(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers) {

	}
	public virtual void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit) {

	}
	public virtual void OnHitByProj(NPC npc, Projectile proj, NPC.HitInfo hit) {

	}
	/// <summary>
	/// This will handle npc dodge, the chance here are multiplicative in gameplay sense<br/>
	/// The code is run in the collision check before NPC get damage<br/>
	/// When the condition is met, the developer should use the variable/field <see cref="RoguelikeGlobalNPC.InvincibilityFrame"/> to add
	/// i-frame
	/// </summary>
	/// <param name="npc"></param>
	/// <returns>
	/// <b>True</b> to make this NPC dodge any attack<br/>
	/// return false by default
	/// </returns>
	public virtual bool DodgeCondition(NPC npc) {
		return false;
	}
	public ModMutation Clone() {
		ModMutation clone = (ModMutation)this.MemberwiseClone();
		clone.SetDefault();
		return clone;
	}
	/// <summary>
	/// When creating a field in the mod mutation<br/>
	/// Use this hook to create a entirely new instance of that variable so that they aren't share between each copies.
	/// </summary>
	public virtual void SetDefault() {

	}
}

public class MutationGlobalNPC : GlobalNPC {
	public override bool InstancePerEntity => true;
	public List<ModMutation> mutation_instance = new();
	public void Add_Mutation(short mutationID) {
		ModMutation mutation = MutationSystem.GetMutation(mutationID);
		if (mutation == null) {
			return;
		}
		mutation_instance.Add(mutation.Clone());
	}
	public void Mutation_Iteration(Action<ModMutation> func) {
		foreach (var item in mutation_instance) {
			ModMutation mutation = item;
			if (mutation == null) {
				continue;
			}
			func(mutation);
		}
	}

	public override void SetDefaults(NPC entity) {
		mutation_instance = new();
	}
	public override void ApplyDifficultyAndPlayerScaling(NPC npc, int numPlayers, float balance, float bossAdjustment) {
		Mutation_Iteration((m) => { m.ApplyAdjustment(npc); });
	}
	public override void OnSpawn(NPC npc, IEntitySource source) {
		Mutation_Iteration((m) => { m.OnSpawn(npc, source); });
	}
	public override bool PreAI(NPC npc) {
		Mutation_Iteration((m) => { m.PreAI(npc); });
		return base.PreAI(npc);
	}
	public override void PostAI(NPC npc) {
		Mutation_Iteration((m) => { m.PostAI(npc); });
	}
	public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo) {
		Mutation_Iteration((m) => { m.OnHitPlayer(npc, target, hurtInfo); });
	}
	public override bool ModifyCollisionData(NPC npc, Rectangle victimHitbox, ref int immunityCooldownSlot, ref MultipliableFloat damageMultiplier, ref Rectangle npcHitbox) {
		bool candodge = false;
		foreach (var item in mutation_instance) {
			ModMutation mutation = item;
			if (mutation == null) {
				continue;
			}
			if (mutation.DodgeCondition(npc)) {
				candodge = true;
			}
		}
		return !candodge;
	}
	public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers) {
		foreach (var item in mutation_instance) {
			ModMutation mutation = item;
			if (mutation == null) {
				continue;
			}
			mutation.ModifyHitPlayer(npc, target, ref modifiers);
		}
	}
	public override bool CheckDead(NPC npc) {
		bool candie = base.CheckDead(npc);
		foreach (var item in mutation_instance) {
			ModMutation mutation = item;
			if (mutation == null) {
				continue;
			}
			if (!mutation.CheckDead(npc)) {
				candie = false;
			}
		}
		return candie;
	}
	public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers) {
		foreach (var mut in mutation_instance) {
			ModMutation mutation = mut;
			if (mutation == null) {
				continue;
			}
			mutation.ModifyHitByItem(npc, player, item, ref modifiers);
		}
	}
	public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers) {
		foreach (var mut in mutation_instance) {
			ModMutation mutation = mut;
			if (mutation == null) {
				continue;
			}
			mutation.ModifyHitByProj(npc, projectile, ref modifiers);
		}
	}
	public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone) {
		Mutation_Iteration((m) => { m.OnHitByItem(npc, player, item, hit); });
	}
	public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) {
		Mutation_Iteration((m) => { m.OnHitByProj(npc, projectile, hit); });
	}
}
