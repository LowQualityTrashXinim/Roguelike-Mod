using Roguelike.Common.Utils;
using System;
using Terraria;
using Terraria.ID;

namespace Roguelike.Common.Systems.ArcadeMode;
/// <summary>
/// This class only contain information and handle getting NPC type to spawn<br/>
/// The rest of the logic should be on the system side to handle as that is the best practice<br/>
/// When this class is created, you should make the system remember the information <br/>
/// rather than constantly creating the same information over and over again.
/// </summary>
public class ArcadeMode_Wave {
	public int[] NPCpool = Array.Empty<int>();
	/// <summary>
	/// This is just how frequent should a ModObject be created to spawn NPC<br/>
	/// </summary>
	public int SpawnTime_Frequency = 0;
	/// <summary>
	/// To make it easy to find, this variable used SpawnNPC prefixed.<br/>
	/// This variable represent wave duration.
	/// </summary>
	public int SpawnNPC_Duration = 0;
	public int SpawnNPC_Amount = 1;
	public int SpawnNPC_Limit = 0;
	/// <summary>
	/// When using this, remember to use <see cref="RareValue"/> to get the chance to spawn the rare NPC<br/>
	/// </summary>
	public int[] RareNPCpool = Array.Empty<int>();
	public float RareValue = 0;
	/// <summary>
	/// Since a wave don't always have a boss NPC, it is fine to leave this empty and there are no boss NPCID start with 0.
	/// </summary>
	public int BossNPC = 0;
	public bool RandomizeChoose = false;
	int index = -1;

	/// <param name="Time">Time frequency to spawn NPC</param>
	/// <param name="duration">Wave duration</param>
	/// <param name="NPCAmount">Amount of NPC can be spawn per frequency</param>
	/// <param name="NPCAmount">The NPC limit that can be spawned</param>
	public ArcadeMode_Wave(int Time, int duration, int NPCAmount, int NPCLimit) {
		SpawnTime_Frequency = Time;
		SpawnNPC_Duration = duration;
		if (NPCAmount > 1) {
			SpawnNPC_Amount = NPCAmount;
		}
		SpawnNPC_Limit += NPCLimit;
		BossNPC = 0;
		RareNPCpool = Array.Empty<int>();
		NPCpool = Array.Empty<int>();
		index = -1;
	}
	public void Set_NPCpool(int[] array) {
		Array.Resize(ref NPCpool, array.Length);
		Array.Copy(array, NPCpool, array.Length);
	}
	public void Set_RareNPCpool(int[] array, float chance) {
		Array.Resize(ref RareNPCpool, array.Length);
		Array.Copy(array, RareNPCpool, array.Length);
		RareValue = chance;
	}
	public void Set_BossNPC(int ID) {
		BossNPC = ID;
	}
	public int Get_NPCtoSpawn() {
		if (NPCpool.Length < 2) {
			return NPCpool[0];
		}
		if (RandomizeChoose) {
			return Main.rand.Next(NPCpool);
		}
		else {
			index = ModUtils.Safe_SwitchValue(index, NPCpool.Length - 1);
			return NPCpool[index];
		}
	}
	public int Get_RareNPCtoSpawn() {
		if (RareNPCpool.Length < 2) {
			return RareNPCpool[0];
		}
		return Main.rand.Next(RareNPCpool);
	}
}
public static class ArcadeMode_Database {
	public static ArcadeMode_Wave Get_Wave(int waveNumber) {
		int cappedfornow = 45;
		if (waveNumber >= cappedfornow) {
			waveNumber = waveNumber - cappedfornow * waveNumber / cappedfornow;
		}
		ArcadeMode_Wave Wave = new(0, 0, 0, 0);
		switch (waveNumber) {
			case 1:
				Wave = new ArcadeMode_Wave(30, ModUtils.ToSecond(15), 1, 10);
				Wave.RandomizeChoose = true;
				Wave.Set_NPCpool([NPCID.BlueSlime, NPCID.GreenSlime, NPCID.PurpleSlime, NPCID.RedSlime, NPCID.YellowSlime]);
				Wave.Set_RareNPCpool([NPCID.GoldenSlime], .001f);
				break;
			case 2:
				Wave = new ArcadeMode_Wave(20, ModUtils.ToSecond(15), 1, 15);
				Wave.RandomizeChoose = true;
				Wave.Set_NPCpool([NPCID.BlueSlime, NPCID.GreenSlime, NPCID.PurpleSlime, NPCID.RedSlime, NPCID.YellowSlime]);
				Wave.Set_RareNPCpool([NPCID.GoldenSlime], .0011f);
				break;
			case 3:
				Wave = new ArcadeMode_Wave(20, ModUtils.ToSecond(15), 1, 20);
				Wave.RandomizeChoose = true;
				Wave.Set_NPCpool([NPCID.BlueSlime, NPCID.GreenSlime, NPCID.PurpleSlime, NPCID.RedSlime, NPCID.YellowSlime]);
				Wave.Set_RareNPCpool([NPCID.GoldenSlime], .0012f);
				break;
			case 4:
				Wave = new ArcadeMode_Wave(20, ModUtils.ToSecond(15), 1, 25);
				Wave.RandomizeChoose = true;
				Wave.Set_NPCpool([NPCID.BlueSlime, NPCID.GreenSlime, NPCID.PurpleSlime, NPCID.RedSlime, NPCID.YellowSlime]);
				Wave.Set_RareNPCpool([NPCID.GoldenSlime], .0013f);
				break;
			case 5:
				Wave = new ArcadeMode_Wave(20, ModUtils.ToSecond(60), 2, 30);
				Wave.RandomizeChoose = false;
				Wave.Set_NPCpool([NPCID.BlueSlime, NPCID.SlimeSpiked]);
				Wave.Set_RareNPCpool([NPCID.GoldenSlime], .0014f);
				Wave.Set_BossNPC(NPCID.KingSlime);
				break;
			case 6:
				Wave = new ArcadeMode_Wave(25, ModUtils.ToSecond(15), 1, 30);
				Wave.RandomizeChoose = true;
				Wave.Set_NPCpool([NPCID.DemonEye, NPCID.DemonEye2, NPCID.CataractEye, NPCID.CataractEye2, NPCID.SleepyEye, NPCID.SleepyEye2, NPCID.DialatedEye, NPCID.DialatedEye2, NPCID.GreenEye, NPCID.GreenEye2, NPCID.PurpleEye, NPCID.PurpleEye2]);
				Wave.Set_RareNPCpool([NPCID.WanderingEye], .001f);
				break;
			case 7:
				Wave = new ArcadeMode_Wave(23, ModUtils.ToSecond(15), 1, 35);
				Wave.RandomizeChoose = true;
				Wave.Set_NPCpool([NPCID.DemonEye, NPCID.DemonEye2, NPCID.CataractEye, NPCID.CataractEye2, NPCID.SleepyEye, NPCID.SleepyEye2, NPCID.DialatedEye, NPCID.DialatedEye2, NPCID.GreenEye, NPCID.GreenEye2, NPCID.PurpleEye, NPCID.PurpleEye2]);
				Wave.Set_RareNPCpool([NPCID.WanderingEye], .002f);
				break;
			case 8:
				Wave = new ArcadeMode_Wave(22, ModUtils.ToSecond(15), 2, 35);
				Wave.RandomizeChoose = true;
				Wave.Set_NPCpool([NPCID.DemonEye, NPCID.DemonEye2, NPCID.CataractEye, NPCID.CataractEye2, NPCID.SleepyEye, NPCID.SleepyEye2, NPCID.DialatedEye, NPCID.DialatedEye2, NPCID.GreenEye, NPCID.GreenEye2, NPCID.PurpleEye, NPCID.PurpleEye2]);
				Wave.Set_RareNPCpool([NPCID.WanderingEye], .002f);
				break;
			case 9:
				Wave = new ArcadeMode_Wave(21, ModUtils.ToSecond(15), 2, 40);
				Wave.RandomizeChoose = true;
				Wave.Set_NPCpool([NPCID.DemonEye, NPCID.DemonEye2, NPCID.CataractEye, NPCID.CataractEye2, NPCID.SleepyEye, NPCID.SleepyEye2, NPCID.DialatedEye, NPCID.DialatedEye2, NPCID.GreenEye, NPCID.GreenEye2, NPCID.PurpleEye, NPCID.PurpleEye2]);
				Wave.Set_RareNPCpool([NPCID.WanderingEye], .002f);
				break;
			case 10:
				Wave = new ArcadeMode_Wave(20, ModUtils.ToSecond(60), 2, 40);
				Wave.Set_NPCpool([NPCID.DemonEye, NPCID.DemonEye2, NPCID.CataractEye, NPCID.CataractEye2, NPCID.SleepyEye, NPCID.SleepyEye2, NPCID.DialatedEye, NPCID.DialatedEye2, NPCID.GreenEye, NPCID.GreenEye2, NPCID.PurpleEye, NPCID.PurpleEye2, NPCID.ServantofCthulhu]);
				Wave.Set_BossNPC(NPCID.EyeofCthulhu);
				Wave.Set_RareNPCpool([NPCID.WanderingEye], .005f);
				break;
			case 11:
				Wave = new ArcadeMode_Wave(20, ModUtils.ToSecond(15), 1, 50);
				Wave.RandomizeChoose = true;
				Wave.Set_NPCpool([NPCID.GoblinArcher, NPCID.GoblinThief, NPCID.GoblinPeon, NPCID.GoblinScout, NPCID.GoblinWarrior, NPCID.GoblinSorcerer]);
				break;
			case 12:
				Wave = new ArcadeMode_Wave(18, ModUtils.ToSecond(15), 1, 60);
				Wave.RandomizeChoose = true;
				Wave.Set_NPCpool([NPCID.GoblinArcher, NPCID.GoblinThief, NPCID.GoblinPeon, NPCID.GoblinScout, NPCID.GoblinWarrior, NPCID.GoblinSorcerer]);
				break;
			case 13:
				Wave = new ArcadeMode_Wave(16, ModUtils.ToSecond(15), 2, 70);
				Wave.RandomizeChoose = true;
				Wave.Set_NPCpool([NPCID.GoblinArcher, NPCID.GoblinThief, NPCID.GoblinPeon, NPCID.GoblinScout, NPCID.GoblinWarrior, NPCID.GoblinSorcerer]);
				break;
			case 14:
				Wave = new ArcadeMode_Wave(14, ModUtils.ToSecond(15), 2, 80);
				Wave.RandomizeChoose = true;
				Wave.Set_NPCpool([NPCID.GoblinArcher, NPCID.GoblinThief, NPCID.GoblinPeon, NPCID.GoblinScout, NPCID.GoblinWarrior, NPCID.GoblinSorcerer]);
				break;
			case 15:
				Wave = new ArcadeMode_Wave(20, ModUtils.ToSecond(60), 2, 90);
				Wave.RandomizeChoose = true;
				Wave.Set_NPCpool([NPCID.GoblinArcher, NPCID.GoblinThief, NPCID.GoblinPeon, NPCID.GoblinScout, NPCID.GoblinWarrior, NPCID.GoblinSorcerer]);
				Wave.Set_BossNPC(NPCID.GoblinSummoner);
				break;
			case 16:
				Wave = new ArcadeMode_Wave(20, ModUtils.ToSecond(15), 2, 90);
				Wave.RandomizeChoose = true;
				Wave.Set_NPCpool([NPCID.EaterofSouls, NPCID.DevourerHead, NPCID.CorruptSlime, NPCID.Slimeling, NPCID.DarkMummy]);
				Wave.Set_RareNPCpool([NPCID.Corruptor, NPCID.Slimer, NPCID.CursedHammer], .1f);
				break;
			case 17:
				Wave = new ArcadeMode_Wave(18, ModUtils.ToSecond(15), 2, 90);
				Wave.RandomizeChoose = true;
				Wave.Set_NPCpool([NPCID.EaterofSouls, NPCID.DevourerHead, NPCID.CorruptSlime, NPCID.Slimeling, NPCID.DarkMummy]);
				Wave.Set_RareNPCpool([NPCID.Corruptor, NPCID.Slimer, NPCID.CursedHammer], .15f);
				break;
			case 18:
				Wave = new ArcadeMode_Wave(17, ModUtils.ToSecond(15), 3, 90);
				Wave.RandomizeChoose = true;
				Wave.Set_NPCpool([NPCID.EaterofSouls, NPCID.DevourerHead, NPCID.CorruptSlime, NPCID.Slimeling, NPCID.DarkMummy]);
				Wave.Set_RareNPCpool([NPCID.Corruptor, NPCID.Slimer, NPCID.CursedHammer], .2f);
				break;
			case 19:
				Wave = new ArcadeMode_Wave(16, ModUtils.ToSecond(15), 3, 90);
				Wave.RandomizeChoose = true;
				Wave.Set_NPCpool([NPCID.EaterofSouls, NPCID.DevourerHead, NPCID.CorruptSlime, NPCID.Slimeling, NPCID.DarkMummy]);
				Wave.Set_RareNPCpool([NPCID.Corruptor, NPCID.Slimer, NPCID.CursedHammer], .25f);
				break;
			case 20:
				Wave = new ArcadeMode_Wave(15, ModUtils.ToSecond(15), 3, 90);
				Wave.RandomizeChoose = true;
				Wave.Set_NPCpool([NPCID.EaterofSouls, NPCID.DevourerHead, NPCID.CorruptSlime, NPCID.Slimeling, NPCID.DarkMummy]);
				Wave.Set_RareNPCpool([NPCID.Corruptor, NPCID.Slimer, NPCID.CursedHammer], .25f);
				Wave.Set_BossNPC(NPCID.EaterofWorldsHead);
				break;
			case 21:
				Wave = new ArcadeMode_Wave(15, ModUtils.ToSecond(15), 2, 90);
				Wave.Set_NPCpool([NPCID.Crimera, NPCID.FaceMonster, NPCID.LittleCrimera, NPCID.BigCrimera, NPCID.BloodCrawler, NPCID.Crimslime, NPCID.BloodMummy]);
				Wave.Set_RareNPCpool([NPCID.Herpling, NPCID.Crimslime, NPCID.FloatyGross, NPCID.IchorSticker, NPCID.CrimsonAxe], .15f);
				break;
			case 22:
				Wave = new ArcadeMode_Wave(15, ModUtils.ToSecond(15), 2, 90);
				Wave.Set_NPCpool([NPCID.Crimera, NPCID.FaceMonster, NPCID.LittleCrimera, NPCID.BigCrimera, NPCID.BloodCrawler, NPCID.Crimslime, NPCID.BloodMummy]);
				Wave.Set_RareNPCpool([NPCID.Herpling, NPCID.Crimslime, NPCID.FloatyGross, NPCID.IchorSticker, NPCID.CrimsonAxe], .2f);
				break;
			case 23:
				Wave = new ArcadeMode_Wave(15, ModUtils.ToSecond(15), 2, 90);
				Wave.Set_NPCpool([NPCID.Crimera, NPCID.FaceMonster, NPCID.LittleCrimera, NPCID.BigCrimera, NPCID.BloodCrawler, NPCID.Crimslime, NPCID.BloodMummy]);
				Wave.Set_RareNPCpool([NPCID.Herpling, NPCID.Crimslime, NPCID.FloatyGross, NPCID.IchorSticker, NPCID.CrimsonAxe], .25f);
				break;
			case 24:
				Wave = new ArcadeMode_Wave(15, ModUtils.ToSecond(15), 3, 90);
				Wave.Set_NPCpool([NPCID.Crimera, NPCID.FaceMonster, NPCID.LittleCrimera, NPCID.BigCrimera, NPCID.BloodCrawler, NPCID.Crimslime, NPCID.BloodMummy]);
				Wave.Set_RareNPCpool([NPCID.Herpling, NPCID.Crimslime, NPCID.FloatyGross, NPCID.IchorSticker, NPCID.CrimsonAxe], .25f);
				break;
			case 25:
				Wave = new ArcadeMode_Wave(15, ModUtils.ToSecond(60), 3, 90);
				Wave.RandomizeChoose = false;
				Wave.Set_NPCpool([NPCID.Crimera, NPCID.FaceMonster, NPCID.LittleCrimera, NPCID.BigCrimera, NPCID.BloodCrawler, NPCID.Crimslime, NPCID.BloodMummy]);
				Wave.Set_RareNPCpool([NPCID.Herpling, NPCID.FloatyGross, NPCID.IchorSticker, NPCID.CrimsonAxe], .05f);
				Wave.Set_BossNPC(NPCID.BrainofCthulhu);
				break;
			case 26:
				Wave = new ArcadeMode_Wave(15, ModUtils.ToSecond(15), 2, 90);
				Wave.RandomizeChoose = false;
				Wave.Set_NPCpool([NPCID.Bee, NPCID.BeeSmall, NPCID.Hornet, NPCID.HornetFatty, NPCID.HornetHoney, NPCID.HornetLeafy, NPCID.HornetSpikey, NPCID.HornetStingy, NPCID.BigHornetFatty, NPCID.BigHornetHoney, NPCID.BigHornetLeafy, NPCID.BigHornetSpikey, NPCID.BigHornetStingy, NPCID.SpikedJungleSlime, NPCID.JungleBat, NPCID.JungleSlime, NPCID.LacBeetle]);
				Wave.Set_RareNPCpool([NPCID.DoctorBones], .01f);
				break;
			case 27:
				Wave = new ArcadeMode_Wave(15, ModUtils.ToSecond(15), 2, 90);
				Wave.RandomizeChoose = false;
				Wave.Set_NPCpool([NPCID.Bee, NPCID.BeeSmall, NPCID.Hornet, NPCID.HornetFatty, NPCID.HornetHoney, NPCID.HornetLeafy, NPCID.HornetSpikey, NPCID.HornetStingy, NPCID.BigHornetFatty, NPCID.BigHornetHoney, NPCID.BigHornetLeafy, NPCID.BigHornetSpikey, NPCID.BigHornetStingy, NPCID.SpikedJungleSlime, NPCID.JungleBat, NPCID.JungleSlime, NPCID.LacBeetle]);
				Wave.Set_RareNPCpool([NPCID.DoctorBones], .01f);
				break;
			case 28:
				Wave = new ArcadeMode_Wave(15, ModUtils.ToSecond(30), 2, 90);
				Wave.RandomizeChoose = false;
				Wave.Set_NPCpool([NPCID.Bee, NPCID.BeeSmall, NPCID.Hornet, NPCID.HornetFatty, NPCID.HornetHoney, NPCID.HornetLeafy, NPCID.HornetSpikey, NPCID.HornetStingy, NPCID.BigHornetFatty, NPCID.BigHornetHoney, NPCID.BigHornetLeafy, NPCID.BigHornetSpikey, NPCID.BigHornetStingy, NPCID.SpikedJungleSlime, NPCID.JungleBat, NPCID.JungleSlime, NPCID.LacBeetle]);
				Wave.Set_RareNPCpool([NPCID.DoctorBones], .01f);
				break;
			case 29:
				Wave = new ArcadeMode_Wave(10, ModUtils.ToSecond(30), 3, 90);
				Wave.RandomizeChoose = false;
				Wave.Set_NPCpool([NPCID.Bee, NPCID.BeeSmall, NPCID.Hornet, NPCID.HornetFatty, NPCID.HornetHoney, NPCID.HornetLeafy, NPCID.HornetSpikey, NPCID.HornetStingy, NPCID.BigHornetFatty, NPCID.BigHornetHoney, NPCID.BigHornetLeafy, NPCID.BigHornetSpikey, NPCID.BigHornetStingy, NPCID.SpikedJungleSlime, NPCID.JungleBat, NPCID.JungleSlime, NPCID.LacBeetle]);
				Wave.Set_RareNPCpool([NPCID.DoctorBones], .01f);
				break;
			case 30:
				Wave = new ArcadeMode_Wave(15, ModUtils.ToSecond(90), 3, 90);
				Wave.RandomizeChoose = true;
				Wave.Set_NPCpool([NPCID.Bee, NPCID.BeeSmall, NPCID.Hornet, NPCID.HornetFatty, NPCID.HornetHoney, NPCID.HornetLeafy, NPCID.HornetSpikey, NPCID.HornetStingy, NPCID.BigHornetFatty, NPCID.BigHornetHoney, NPCID.BigHornetLeafy, NPCID.BigHornetSpikey, NPCID.BigHornetStingy, NPCID.SpikedJungleSlime, NPCID.JungleBat, NPCID.JungleSlime, NPCID.LacBeetle]);
				Wave.Set_RareNPCpool([NPCID.DoctorBones], .05f);
				Wave.Set_BossNPC(NPCID.QueenBee);
				break;
			case 31:
				Wave = new ArcadeMode_Wave(30, ModUtils.ToSecond(15), 5, 90);
				Wave.RandomizeChoose = false;
				Wave.Set_NPCpool([NPCID.IceSlime, NPCID.SpikedIceSlime, NPCID.Wolf, NPCID.IceElemental, NPCID.IceBat, NPCID.SnowFlinx, NPCID.UndeadViking, NPCID.IcyMerman]);
				Wave.Set_RareNPCpool([NPCID.IceGolem], .001f);
				break;
			case 32:
				Wave = new ArcadeMode_Wave(30, ModUtils.ToSecond(15), 6, 90);
				Wave.RandomizeChoose = false;
				Wave.Set_NPCpool([NPCID.IceSlime, NPCID.SpikedIceSlime, NPCID.Wolf, NPCID.IceElemental, NPCID.IceBat, NPCID.SnowFlinx, NPCID.UndeadViking, NPCID.IcyMerman]);
				Wave.Set_RareNPCpool([NPCID.IceGolem], .001f);
				break;
			case 33:
				Wave = new ArcadeMode_Wave(60, ModUtils.ToSecond(15), 9, 90);
				Wave.RandomizeChoose = false;
				Wave.Set_NPCpool([NPCID.IceSlime, NPCID.SpikedIceSlime, NPCID.Wolf, NPCID.IceElemental, NPCID.IceBat, NPCID.SnowFlinx, NPCID.UndeadViking, NPCID.IcyMerman]);
				Wave.Set_RareNPCpool([NPCID.IceGolem], .001f);
				break;
			case 34:
				Wave = new ArcadeMode_Wave(180, ModUtils.ToSecond(40), 15, 90);
				Wave.RandomizeChoose = false;
				Wave.Set_NPCpool([NPCID.IceSlime, NPCID.SpikedIceSlime, NPCID.Wolf, NPCID.IceElemental, NPCID.IceBat, NPCID.SnowFlinx, NPCID.UndeadViking, NPCID.IcyMerman]);
				Wave.Set_RareNPCpool([NPCID.IceGolem], .001f);
				break;
			case 35:
				Wave = new ArcadeMode_Wave(180, ModUtils.ToSecond(90), 15, 90);
				Wave.RandomizeChoose = true;
				Wave.Set_NPCpool([NPCID.IceSlime, NPCID.SpikedIceSlime, NPCID.Wolf, NPCID.IceElemental, NPCID.IceBat, NPCID.SnowFlinx, NPCID.UndeadViking, NPCID.IcyMerman]);
				Wave.Set_RareNPCpool([NPCID.IceGolem], .005f);
				Wave.Set_BossNPC(NPCID.Deerclops);
				break;
			case 36:
				Wave = new ArcadeMode_Wave(30, ModUtils.ToSecond(90), 5, 90);
				Wave.Set_NPCpool([NPCID.MushiLadybug, NPCID.SporeBat, NPCID.SporeSkeleton, NPCID.ZombieMushroomHat, NPCID.ZombieMushroom, NPCID.AnomuraFungus]);
				//Wave.Set_RareNPCpool([NPCID.IceGolem], .005f);
				break;
			case 37:
				Wave = new ArcadeMode_Wave(20, ModUtils.ToSecond(90), 5, 90);
				Wave.Set_NPCpool([NPCID.MushiLadybug, NPCID.SporeBat, NPCID.SporeSkeleton, NPCID.ZombieMushroomHat, NPCID.ZombieMushroom, NPCID.AnomuraFungus]);
				break;
			case 38:
				Wave = new ArcadeMode_Wave(15, ModUtils.ToSecond(90), 5, 90);
				Wave.Set_NPCpool([NPCID.MushiLadybug, NPCID.SporeBat, NPCID.SporeSkeleton, NPCID.ZombieMushroomHat, NPCID.ZombieMushroom, NPCID.AnomuraFungus]);
				break;
			case 39:
				Wave = new ArcadeMode_Wave(15, ModUtils.ToSecond(90), 7, 90);
				Wave.Set_NPCpool([NPCID.MushiLadybug, NPCID.SporeBat, NPCID.SporeSkeleton, NPCID.ZombieMushroomHat, NPCID.ZombieMushroom, NPCID.AnomuraFungus]);
				break;
			case 40:
				Wave = new ArcadeMode_Wave(30, ModUtils.ToSecond(90), 10, 90);
				Wave.Set_NPCpool([NPCID.MushiLadybug, NPCID.SporeBat, NPCID.SporeSkeleton, NPCID.ZombieMushroomHat, NPCID.ZombieMushroom, NPCID.AnomuraFungus]);
				break;
			case 41:
				Wave = new ArcadeMode_Wave(30, ModUtils.ToSecond(30), 3, 100);
				Wave.Set_NPCpool([NPCID.Skeleton, NPCID.AngryBones, NPCID.AngryBonesBig, NPCID.AngryBonesBigHelmet, NPCID.AngryBonesBigMuscle, NPCID.DarkCaster, NPCID.CursedSkull, NPCID.BlueArmoredBones, NPCID.BlueArmoredBonesMace, NPCID.BlueArmoredBonesNoPants, NPCID.BlueArmoredBonesSword, NPCID.RustyArmoredBonesAxe, NPCID.RustyArmoredBonesFlail, NPCID.RustyArmoredBonesSword, NPCID.RustyArmoredBonesSwordNoArmor, NPCID.HellArmoredBones, NPCID.HellArmoredBonesMace, NPCID.HellArmoredBonesSpikeShield, NPCID.HellArmoredBonesSword, NPCID.SkeletonArcher, NPCID.ArmoredSkeleton]);
				Wave.Set_RareNPCpool([NPCID.RuneWizard, NPCID.GiantCursedSkull, NPCID.BoneLee, NPCID.SkeletonSniper, NPCID.Necromancer, NPCID.NecromancerArmored, NPCID.RaggedCaster, NPCID.RaggedCasterOpenCoat, NPCID.DiabolistRed, NPCID.DiabolistWhite, NPCID.TacticalSkeleton], .05f);
				break;
			case 42:
				Wave = new ArcadeMode_Wave(30, ModUtils.ToSecond(30), 4, 100);
				Wave.Set_NPCpool([NPCID.Skeleton, NPCID.AngryBones, NPCID.AngryBonesBig, NPCID.AngryBonesBigHelmet, NPCID.AngryBonesBigMuscle, NPCID.DarkCaster, NPCID.CursedSkull, NPCID.BlueArmoredBones, NPCID.BlueArmoredBonesMace, NPCID.BlueArmoredBonesNoPants, NPCID.BlueArmoredBonesSword, NPCID.RustyArmoredBonesAxe, NPCID.RustyArmoredBonesFlail, NPCID.RustyArmoredBonesSword, NPCID.RustyArmoredBonesSwordNoArmor, NPCID.HellArmoredBones, NPCID.HellArmoredBonesMace, NPCID.HellArmoredBonesSpikeShield, NPCID.HellArmoredBonesSword, NPCID.SkeletonArcher, NPCID.ArmoredSkeleton]);
				Wave.Set_RareNPCpool([NPCID.RuneWizard, NPCID.GiantCursedSkull, NPCID.BoneLee, NPCID.SkeletonSniper, NPCID.Necromancer, NPCID.NecromancerArmored, NPCID.RaggedCaster, NPCID.RaggedCasterOpenCoat, NPCID.DiabolistRed, NPCID.DiabolistWhite, NPCID.TacticalSkeleton], .05f);
				break;
			case 43:
				Wave = new ArcadeMode_Wave(20, ModUtils.ToSecond(30), 5, 100);
				Wave.Set_NPCpool([NPCID.Skeleton, NPCID.AngryBones, NPCID.AngryBonesBig, NPCID.AngryBonesBigHelmet, NPCID.AngryBonesBigMuscle, NPCID.DarkCaster, NPCID.CursedSkull, NPCID.BlueArmoredBones, NPCID.BlueArmoredBonesMace, NPCID.BlueArmoredBonesNoPants, NPCID.BlueArmoredBonesSword, NPCID.RustyArmoredBonesAxe, NPCID.RustyArmoredBonesFlail, NPCID.RustyArmoredBonesSword, NPCID.RustyArmoredBonesSwordNoArmor, NPCID.HellArmoredBones, NPCID.HellArmoredBonesMace, NPCID.HellArmoredBonesSpikeShield, NPCID.HellArmoredBonesSword, NPCID.SkeletonArcher, NPCID.ArmoredSkeleton]);
				Wave.Set_RareNPCpool([NPCID.RuneWizard, NPCID.GiantCursedSkull, NPCID.BoneLee, NPCID.SkeletonSniper, NPCID.Necromancer, NPCID.NecromancerArmored, NPCID.RaggedCaster, NPCID.RaggedCasterOpenCoat, NPCID.DiabolistRed, NPCID.DiabolistWhite, NPCID.TacticalSkeleton], .05f);
				break;
			case 44:
				Wave = new ArcadeMode_Wave(30, ModUtils.ToSecond(40), 10, 100);
				Wave.Set_NPCpool([NPCID.Skeleton, NPCID.AngryBones, NPCID.AngryBonesBig, NPCID.AngryBonesBigHelmet, NPCID.AngryBonesBigMuscle, NPCID.DarkCaster, NPCID.CursedSkull, NPCID.BlueArmoredBones, NPCID.BlueArmoredBonesMace, NPCID.BlueArmoredBonesNoPants, NPCID.BlueArmoredBonesSword, NPCID.RustyArmoredBonesAxe, NPCID.RustyArmoredBonesFlail, NPCID.RustyArmoredBonesSword, NPCID.RustyArmoredBonesSwordNoArmor, NPCID.HellArmoredBones, NPCID.HellArmoredBonesMace, NPCID.HellArmoredBonesSpikeShield, NPCID.HellArmoredBonesSword, NPCID.SkeletonArcher, NPCID.ArmoredSkeleton]);
				Wave.Set_RareNPCpool([NPCID.RuneWizard, NPCID.GiantCursedSkull, NPCID.BoneLee, NPCID.SkeletonSniper, NPCID.Necromancer, NPCID.NecromancerArmored, NPCID.RaggedCaster, NPCID.RaggedCasterOpenCoat, NPCID.DiabolistRed, NPCID.DiabolistWhite, NPCID.TacticalSkeleton], .1f);
				break;
			case 45:
				Wave = new ArcadeMode_Wave(210, ModUtils.ToSecond(40), 40, 40);
				Wave.RandomizeChoose = true;
				Wave.Set_NPCpool([NPCID.Skeleton, NPCID.AngryBones, NPCID.AngryBonesBig, NPCID.AngryBonesBigHelmet, NPCID.AngryBonesBigMuscle, NPCID.DarkCaster, NPCID.CursedSkull, NPCID.BlueArmoredBones, NPCID.BlueArmoredBonesMace, NPCID.BlueArmoredBonesNoPants, NPCID.BlueArmoredBonesSword, NPCID.RustyArmoredBonesAxe, NPCID.RustyArmoredBonesFlail, NPCID.RustyArmoredBonesSword, NPCID.RustyArmoredBonesSwordNoArmor, NPCID.HellArmoredBones, NPCID.HellArmoredBonesMace, NPCID.HellArmoredBonesSpikeShield, NPCID.HellArmoredBonesSword, NPCID.SkeletonArcher, NPCID.ArmoredSkeleton]);
				Wave.Set_RareNPCpool([NPCID.RuneWizard, NPCID.GiantCursedSkull, NPCID.BoneLee, NPCID.SkeletonSniper, NPCID.Necromancer, NPCID.NecromancerArmored, NPCID.RaggedCaster, NPCID.RaggedCasterOpenCoat, NPCID.DiabolistRed, NPCID.DiabolistWhite, NPCID.TacticalSkeleton], .05f);
				Wave.Set_BossNPC(NPCID.SkeletronHead);
				break;
			case 46:
				Wave = new ArcadeMode_Wave(120, ModUtils.ToSecond(15), 3, 100);
				Wave.Set_NPCpool([NPCID.BloodZombie, NPCID.Drippler, NPCID.WanderingEye]);
				Wave.Set_RareNPCpool([NPCID.BloodEelHead, NPCID.GoblinShark, NPCID.EyeballFlyingFish], .35f);
				break;
			case 47:
				Wave = new ArcadeMode_Wave(120, ModUtils.ToSecond(15), 4, 100);
				Wave.Set_NPCpool([NPCID.BloodZombie, NPCID.Drippler, NPCID.WanderingEye]);
				Wave.Set_RareNPCpool([NPCID.BloodEelHead, NPCID.GoblinShark, NPCID.EyeballFlyingFish], .35f);
				break;
			case 48:
				Wave = new ArcadeMode_Wave(60, ModUtils.ToSecond(25), 4, 100);
				Wave.Set_NPCpool([NPCID.BloodZombie, NPCID.Drippler, NPCID.WanderingEye]);
				Wave.Set_RareNPCpool([NPCID.BloodEelHead, NPCID.GoblinShark, NPCID.EyeballFlyingFish], .35f);
				break;
			case 49:
				Wave = new ArcadeMode_Wave(60, ModUtils.ToSecond(40), 5, 100);
				Wave.Set_NPCpool([NPCID.BloodZombie, NPCID.Drippler, NPCID.WanderingEye]);
				Wave.Set_RareNPCpool([NPCID.BloodEelHead, NPCID.GoblinShark, NPCID.EyeballFlyingFish], .35f);
				break;
			case 50:
				Wave = new ArcadeMode_Wave(60, ModUtils.ToSecond(40), 5, 100);
				Wave.Set_NPCpool([NPCID.BloodZombie, NPCID.Drippler, NPCID.WanderingEye]);
				Wave.Set_RareNPCpool([NPCID.BloodEelHead, NPCID.GoblinShark, NPCID.EyeballFlyingFish], .35f);
				Wave.Set_BossNPC(NPCID.BloodNautilus);
				break;
				//Underworld raid
			case 51:
				break;
			case 52:
				break;
			case 53:
				break;
			case 54:
				break;
			case 55:
				break;
				//
			case 56:
				break;
			case 57:
				break;
			case 58:
				break;
			case 59:
				break;
			case 60:
				break;
			case 61:
				break;
			case 62:
				break;
			case 63:
				break;
			case 64:
				break;
			case 65:
				break;
			case 66:
				break;
			case 67:
				break;
			case 68:
				break;
			case 69:
				break;
			case 70:
				break;
			case 71:
				break;
			case 72:
				break;
			case 73:
				break;
			case 74:
				break;
			case 75:
				break;
			case 76:
				break;
			case 77:
				break;
			case 78:
				break;
			case 79:
				break;
			case 80:
				break;
			case 81:
				break;
			case 82:
				break;
			case 83:
				break;
			case 84:
				break;
			case 85:
				break;
		}
		return Wave;
	}
}
