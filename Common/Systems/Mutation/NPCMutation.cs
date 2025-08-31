using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace Roguelike.Common.Systems.Mutation;
internal class NPCMutation : GlobalNPC {
	public List<ModMutation> mutationList = new List<ModMutation>();
	public override bool InstancePerEntity => true;
	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) {
		if (entity.friendly) {
			return false;
		}
		return base.AppliesToEntity(entity, lateInstantiation);
	}
	public override void OnSpawn(NPC npc, IEntitySource source) {
		if (mutationList == null) {
			mutationList = new();
		}
		if (Main.rand.NextFloat() <= ModContent.GetInstance<MutationSystem>().MutationChance) {
			mutationList.Add(ModMutationLoader.GetMutation(Main.rand.Next(ModMutationLoader.TotalCount)));
		}
		if (mutationList != null) {
			foreach (var mutation in mutationList) {
				mutation.OnSpawn(npc, source);
			}
		}
	}
	public override void PostAI(NPC npc) {
		if (mutationList != null) {
			foreach (var mutation in mutationList) {
				mutation.PostAI(npc);
			}
		}
	}
	public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo) {
		if (mutationList != null) {
			foreach (var mutation in mutationList) {
				mutation.OnHitPlayer(npc, target, hurtInfo);
			}
		}
	}
	public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone) {
		if (mutationList != null) {
			foreach (var mutation in mutationList) {
				mutation.OnHitByItem(npc, player, item, hit, damageDone);
			}
		}
	}
	public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) {
		if (mutationList != null) {
			foreach (var mutation in mutationList) {
				mutation.OnHitByProjectile(npc, projectile, hit, damageDone);
			}
		}
	}
	public override void OnKill(NPC npc) {
		if (mutationList != null) {
			foreach (var mutation in mutationList) {
				mutation.OnKill(npc);
			}
		}
	}
	public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers) {
		if (npc.lastInteraction < 255 && npc.lastInteraction >= 0) {
			Player player = player = Main.player[npc.lastInteraction];
		}
		if (mutationList != null) {
			foreach (var mutation in mutationList) {
				mutation.ModifyHitByProjectile(npc, projectile, ref modifiers);
			}
		}
		if (!Main.masterMode) {
			return;
		}
	}
	public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers) {
		if (mutationList != null) {
			foreach (var mutation in mutationList) {
				mutation.ModifyHitByItem(npc, player, item, ref modifiers);
			}
		}
		if (!Main.masterMode) {
			return;
		}
	}
}
