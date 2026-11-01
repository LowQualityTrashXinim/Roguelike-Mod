using Roguelike.Common.Global;
using Roguelike.Common.Systems.BossRushMode;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.Systems;
/// <summary>
/// Contain the basic handler for boss to properly work between each mode.<br/>
/// Avoid setting up new UpdateBiomes if all you need is here and just include a new variable check
/// </summary>
internal class BaseSystemHandler : ModSystem {
	public override void Load() {
		On_Player.UpdateBiomes += On_Player_UpdateBiomes;
		On_NPC.EncourageDespawn += On_NPC_EncourageDespawn;
	}
	private void On_NPC_EncourageDespawn(On_NPC.orig_EncourageDespawn orig, NPC self, int despawnTime) {
		if (ModContent.GetInstance<BossRushStructureHandler>().Active && ModContent.GetInstance<RoguelikeWorldProperty>().BossRushWorld
			|| ModContent.GetInstance<RoguelikeWorldProperty>().ArcadeMode) {
			orig(self, despawnTime);
		}
	}
	private void On_Player_UpdateBiomes(On_Player.orig_UpdateBiomes orig, Player self) {
		if (self.difficulty == PlayerDifficultyID.Creative) {
			orig(self);
			return;
		}
		var property = ModContent.GetInstance<RoguelikeWorldProperty>();
		if (!property.BossRushWorld && !property.ArcadeMode) {
			orig(self);
			return;
		}
		if (ModContent.GetInstance<UniversalSystem>().ListOfBossKilled.Contains(NPCID.WallofFlesh)) {
			Main.hardMode = true;
		}
		for (int i = 0; i < 200; i++) {
			NPC npc = Main.npc[i];
			if (!npc.active) {
				continue;
			}
			if (npc.type == NPCID.DukeFishron) {
				self.ZoneBeach = true;
			}
			if (npc.type == NPCID.BrainofCthulhu) {
				self.ZoneCrimson = true;
			}
			if (npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsTail) {
				self.ZoneCorrupt = true;
			}
			if (npc.type == NPCID.SkeletronHead || npc.type == NPCID.SkeletronPrime || npc.type == NPCID.Retinazer || npc.type == NPCID.Spazmatism) {
				Main.dayTime = false;
			}
			if (npc.type == NPCID.QueenBee || npc.type == NPCID.Plantera) {
				self.ZoneJungle = true;
			}
		}
	}
}
public class BaseSystemHandler_GlobalNPC : GlobalNPC {
	public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo) {
		if (ModContent.GetInstance<RoguelikeWorldProperty>().BossRushWorld
			|| ModContent.GetInstance<RoguelikeWorldProperty>().ArcadeMode) {
			pool.Clear();
		}
	}
	public override bool PreAI(NPC npc) {
		if (ModContent.GetInstance<RoguelikeWorldProperty>().TerrariaWorld) {
			return base.PreAI(npc);
		}
		if (npc.type == NPCID.OldMan) {
			return false;
		}
		if (npc.type == NPCID.TravellingMerchant) {
			return false;
		}
		return true;
	}
}
