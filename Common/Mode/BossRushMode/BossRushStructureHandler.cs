using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Roguelike.Common.Mode.BossRushMode;
internal class BossRushStructureHandler : ModSystem {
	public override void Load() {
		On_NPC.EncourageDespawn += On_NPC_EncourageDespawn;
	}

	private void On_NPC_EncourageDespawn(On_NPC.orig_EncourageDespawn orig, NPC self, int despawnTime) {
		if (!Active || !ModContent.GetInstance<BossRushWorldGen>().BossRushWorld) {
			return;
		}
		orig(self, despawnTime);
	}

	public HashSet<Point> MobsSpawningPos() {
		return new HashSet<Point>() {
		new Point(24,47),
		new Point(24,22),
		new Point(24,72),
		new Point(24,97),
		new Point(195,97),
		new Point(195,72),
		new Point(195,47),
		new Point(195,22)
		};
	}
	public Rectangle Rect_BossRushStructure() => ModContent.GetInstance<BossRushWorldGen>().BossRushStructure;
	Stopwatch _timer = new Stopwatch();
	bool Initialize = false;
	int SpawnTime = 0;
	int SpawnTimeLimit = 600;
	int SpawnAmount = 0;
	public void Start_BossRush() {
		Active = true;
		Initialize = true;
		_timer.Start();
	}
	public int[] NPCspawnPool = [
		NPCID.DemonEye,
		NPCID.Hornet,
		NPCID.FireImp,
		NPCID.BlueSlime,
		NPCID.SpikedIceSlime,
		NPCID.SpikedJungleSlime,
		NPCID.Crimera,
		NPCID.Corruptor,
		NPCID.Harpy,
		NPCID.GiantShelly,
		NPCID.GreekSkeleton,
		NPCID.GraniteFlyer,
		NPCID.GraniteGolem,
		NPCID.GiantWalkingAntlion,
		NPCID.GiantFlyingAntlion,
		NPCID.EaterofSouls,
		NPCID.GiantBat,
		NPCID.IceBat,
		NPCID.Demon,
		NPCID.CultistArcherWhite,
		NPCID.IceElemental,
		NPCID.IcyMerman,
		NPCID.Necromancer,
		NPCID.RuneWizard,
		NPCID.Tim,
		];
	public int[] MiniBossspawnPool = [
		NPCID.Paladin,
		NPCID.RockGolem,
		NPCID.Medusa,
		NPCID.BigMimicCrimson,
		NPCID.BigMimicCorruption,
		NPCID.BigMimicHallow,
		NPCID.BigMimicJungle,
		NPCID.BloodSquid,
		NPCID.RainbowSlime,
		NPCID.GoblinSummoner,
		];
	public bool Active { get; private set; } = false;
	List<int> BossList = [
		NPCID.KingSlime,
		NPCID.EyeofCthulhu,
		NPCID.SkeletronHead,
		NPCID.EaterofWorldsHead,
		NPCID.BrainofCthulhu,
		NPCID.QueenBee,
		NPCID.Deerclops,
		NPCID.TheDestroyer,
		NPCID.SkeletronPrime,
		NPCID.Retinazer,
		NPCID.Plantera,
		NPCID.Golem,
		NPCID.DukeFishron,
		NPCID.QueenSlimeBoss,
		NPCID.DD2Betsy,
		NPCID.HallowBoss,
		NPCID.CultistBoss,
		NPCID.Pumpking,
		NPCID.IceQueen,
		NPCID.MartianSaucer,
	];
	public override void PostUpdateEverything() {
		if (!Active || !ModContent.GetInstance<BossRushWorldGen>().BossRushWorld) {
			return;
		}
		if (Initialize) {
			Initialize = false;
		}

		SpawnTimeLimit = Math.Clamp(600 - SpawnAmount / 100, 60, 900);
		if (++SpawnTime >= SpawnTimeLimit) {
			SpawnAmount++;
			Rectangle zone = Rect_BossRushStructure();
			Vector2 pos = (zone.Location + Main.rand.NextFromHashSet(MobsSpawningPos())).ToWorldCoordinates();
			NPC npc = NPC.NewNPCDirect(new EntitySource_SpawnNPC(), (int)pos.X, (int)pos.Y, Main.rand.Next(NPCspawnPool));
			npc.GetGlobalNPC<RoguelikeGlobalNPC>().CanDenyYouFromLoot = true;
			npc.timeLeft = 99999;
		}
		if (ModUtils.IsAnyVanillaBossAlive()) {

		}


	}
	public override void SaveWorldData(TagCompound tag) {
		base.SaveWorldData(tag);
	}
	public override void LoadWorldData(TagCompound tag) {
		base.LoadWorldData(tag);
	}
}
