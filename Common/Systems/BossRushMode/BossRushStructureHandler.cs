using Microsoft.Xna.Framework;
using Roguelike.Common.General;
using Roguelike.Common.Global;
using Roguelike.Common.Mode.BossRushMode;
using Roguelike.Common.Systems;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.Toggle;
using Roguelike.Texture;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.IO;

namespace Roguelike.Common.Systems.BossRushMode;
public class BossRushStructureHandler : ModSystem {
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
	public TimeSpan Get_Timer => _timer.Elapsed;

	bool Initialize = true;
	int SpawnTime = 0;
	int SpawnTimeLimit = 600;
	int SpawnAmount = 0;
	bool NGplus = false;
	public void Start_BossRush(bool NGplus = false) {
		Active = true;
		_timer.Start();
		BossList = [.. Arr_BossID];
		this.NGplus = NGplus;
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
		NPCID.Tim,
		];
	public int[] MiniBossspawnPool = [
		NPCID.RuneWizard,
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
	public readonly int[] Arr_BossID = [
		NPCID.KingSlime,
		NPCID.EyeofCthulhu,
		NPCID.EaterofWorldsHead,
		NPCID.BrainofCthulhu,
		NPCID.QueenBee,
		NPCID.Deerclops,
		NPCID.SkeletronHead,
		NPCID.QueenSlimeBoss,
		NPCID.Retinazer,
		NPCID.SkeletronPrime,
		NPCID.TheDestroyer,
		NPCID.Plantera,
		NPCID.Golem,
		NPCID.DukeFishron,
		NPCID.HallowBoss,
		NPCID.CultistBoss,
	];
	List<int> BossList = new();
	public int BossSpawnCD = 0;
	public override void PostUpdateEverything() {
		if (!Main.LocalPlayer.active || Main.LocalPlayer.dead || !Active || !ModContent.GetInstance<BossRushWorldGen>().BossRushWorld) {
			return;
		}
		if (BossSpawnCD > 0 && BossSpawnCD <= ModUtils.ToSecond(10))
			if (BossSpawnCD % 60 == 0) {
				Main.NewText($"{BossSpawnCD / 60}", Color.Red);
			}
		if (Initialize) {
			if (BossList == null || BossList.Count < 1) {
				BossList = [.. Arr_BossID];
			}
			Initialize = false;
		}
		SpawnTimeLimit = Math.Clamp(600 - SpawnAmount / 100, 60, 900);
		if (++SpawnTime >= SpawnTimeLimit) {
			SpawnTime = 0;
			SpawnAmount++;
			var zone = Rect_BossRushStructure();
			var pos = (zone.Location + Main.rand.NextFromHashSet(MobsSpawningPos())).ToWorldCoordinates();
			var npc = NPC.NewNPCDirect(new EntitySource_SpawnNPC(), (int)pos.X, (int)pos.Y, Main.rand.Next(NPCspawnPool));
			npc.GetGlobalNPC<RoguelikeGlobalNPC>().CanDenyYouFromLoot = true;
			npc.timeLeft = 9999999;
		}
		if (!Setting_SpawnOnPlayerCommand) {
			SpawnBoss();
		}
	}
	public void SpawnBoss(bool ForcedSpawn = false) {
		if (!ModContent.GetInstance<BossRushWorldGen>().IsABossAlive) {
			if (--BossSpawnCD > 0 && !ForcedSpawn) {
				return;
			}
			if (BossList == null || BossList.Count < 1) {
				if (!NGplus) {
					NGplus = true;
					Active = false;
					Main.NewText("Congratulation, you beaten boss rush mode");
					Main.NewText("If you want to continue to see how far you can push yourself, activate the boss rush item again");
					_timer.Stop();
					return;
				}
				else {
					BossList = [.. Arr_BossID];
				}
			}
			int type;
			if (Setting_FightBossInProgression) {
				type = BossList[0];
			}
			else {
				type = Main.rand.Next(BossList);
			}
			BossList.Remove(type);
			BossSpawnCD = ModUtils.ToSecond(30);
			var pos = Rect_BossRushStructure().Center.ToWorldCoordinates();
			var self = Main.LocalPlayer;
			NPC.SpawnBoss((int)pos.X, (int)pos.Y, type, self.whoAmI);
			if (type == NPCID.Spazmatism) {
				NPC.SpawnBoss((int)pos.X, (int)pos.Y, NPCID.Retinazer, self.whoAmI);
			}
			else if (type == NPCID.Retinazer) {
				NPC.SpawnBoss((int)pos.X, (int)pos.Y, NPCID.Spazmatism, self.whoAmI);
			}
			if (type == NPCID.DukeFishron) {
				self.ZoneBeach = true;
			}
			if (type == NPCID.BrainofCthulhu) {
				self.ZoneCrimson = true;
			}
			if (type == NPCID.EaterofWorldsBody || type == NPCID.EaterofWorldsHead || type == NPCID.EaterofWorldsTail) {
				self.ZoneCorrupt = true;
			}
			if (type == NPCID.SkeletronHead || type == NPCID.SkeletronPrime || type == NPCID.Retinazer || type == NPCID.Spazmatism || type == NPCID.HallowBoss) {
				Main.dayTime = false;
			}
			if (type == NPCID.QueenBee || type == NPCID.Plantera) {
				self.ZoneJungle = true;
			}
		}
	}
	public override void PreSaveAndQuit() {
		Active = false;
		_timer.Reset();
		_timer.Stop();
		BossList = [.. Arr_BossID];
	}
	public override void SaveWorldData(TagCompound tag) {
		base.SaveWorldData(tag);
	}
	public override void LoadWorldData(TagCompound tag) {
		base.LoadWorldData(tag);
	}
	public static bool Setting_FightBossInProgression => ModContent.GetInstance<RogueLikeConfig>().BossRushMode_Setting_FightBossInProgression;
	public static bool Setting_SpawnOnPlayerCommand => ModContent.GetInstance<RogueLikeConfig>().BossRushMode_Setting_SpawnOnPlayerCommand;
}
public class BossRushModeGlobalNPC : GlobalNPC {
	public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo) {
		if (!ModContent.GetInstance<BossRushWorldGen>().BossRushWorld) {
			return;
		}
		pool.Clear();
	}
	public override void OnKill(NPC npc) {
		if (!ModContent.GetInstance<BossRushWorldGen>().BossRushWorld) {
			return;
		}
		if (npc.boss && !BossRushStructureHandler.Setting_SpawnOnPlayerCommand) {
			ModContent.GetInstance<BossRushStructureHandler>().BossSpawnCD = ModUtils.ToSecond(30);
			Main.NewText("A boss have been killed, 30s until a new boss is spawn", Color.Red);
		}
	}
}
internal class BossRushModeActivation : ModItem {
	public override void SetStaticDefaults() {
		Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(3, 8));
		ItemID.Sets.AnimatesAsSoul[Item.type] = true;
	}
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<CursedSkull>();
	public override void SetDefaults() {
		Item.height = 60;
		Item.width = 56;
		Item.value = 0;
		Item.rare = ItemRarityID.Purple;
		Item.useAnimation = 30;
		Item.useTime = 30;
		Item.useStyle = ItemUseStyleID.HoldUp;
		Item.scale = .5f;
	}
	public override bool CanUseItem(Player player) {
		return !ModUtils.IsAnyVanillaBossAlive();
	}
	public override bool AltFunctionUse(Player player) => true;
	public override bool? UseItem(Player player) {
		if (player.whoAmI == Main.myPlayer) {
			SoundEngine.PlaySound(SoundID.Roar, player.position);
			if (BossRushStructureHandler.Setting_SpawnOnPlayerCommand && player.ItemAnimationJustStarted) {
				if (!ModContent.GetInstance<BossRushStructureHandler>().Active) {
					ModContent.GetInstance<BossRushStructureHandler>().Start_BossRush();
				}
				ModContent.GetInstance<BossRushStructureHandler>().SpawnBoss(true);
				ModContent.GetInstance<BossRushStructureHandler>().BossSpawnCD = ModUtils.ToSecond(30);
				return true;
			}
			if (ModContent.GetInstance<UniversalSystem>().Count_BossKill > 1) {
				Main.NewText("Restart boss rush !");
				ModContent.GetInstance<BossRushStructureHandler>().Start_BossRush();
			}
			else {
				Main.NewText("Boss rush mode start !");
				ModContent.GetInstance<BossRushStructureHandler>().Start_BossRush();
			}
		}
		return true;
	}
}
