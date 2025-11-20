using Terraria.ID;
using Terraria.ModLoader;
using Roguelike.Common.Utils;
using System.Collections.Generic;

namespace Roguelike.Contents.Items.Lootbox.Lootpool;

public class CorruptionPool : ItemPool {
	public override HashSet<int> MeleeLoot() => [ItemID.WarAxeoftheNight, ItemID.LightsBane, ItemID.BallOHurt, ItemID.DarkLance, ItemID.CorruptYoyo, ItemID.TentacleSpike];
	public override HashSet<int> RangeLoot() => [ItemID.DemonBow, ItemID.EbonwoodBow];
	public override HashSet<int> MagicLoot() => [ItemID.Vilethorn];
}
public class HoneyPool : ItemPool {
	public override HashSet<int> MeleeLoot() => [ItemID.BeeKeeper, ItemID.HiveFive];
	public override HashSet<int> RangeLoot() => [ItemID.BeesKnees];
	public override HashSet<int> MagicLoot() => [ItemID.WaspGun, ItemID.BeeGun];
	public override HashSet<int> SummonLoot() => [ItemID.HornetStaff];
}

public class CrimsonPool : ItemPool {
	public override HashSet<int> MeleeLoot() => [ItemID.BloodLustCluster, ItemID.BloodButcherer, ItemID.TheMeatball, ItemID.TheRottedFork, ItemID.CrimsonYoyo];
	public override HashSet<int> RangeLoot() => [ItemID.TendonBow, ItemID.TheUndertaker, ItemID.ShadewoodBow];
	public override HashSet<int> MagicLoot() => [ItemID.CrimsonRod];
}
public class SpacePool : ItemPool {
	public override HashSet<int> MeleeLoot() => [ItemID.BluePhasesaber, ItemID.GreenPhasesaber, ItemID.PurplePhaseblade, ItemID.OrangePhaseblade, ItemID.RedPhaseblade, ItemID.WhitePhaseblade, ItemID.YellowPhaseblade];
	public override HashSet<int> RangeLoot() => [ItemID.StarCannon];
	public override HashSet<int> MagicLoot() => [ItemID.SpaceGun, ItemID.LaserRifle, ItemID.LaserMachinegun];
	public override HashSet<int> SummonLoot() => [ItemID.DeadlySphereStaff];
}
public class CrystalPool : ItemPool {
	public override HashSet<int> MeleeLoot() => [ItemID.Chik];
	public override HashSet<int> MagicLoot() => [ItemID.CrystalVileShard, ItemID.CrystalStorm, ItemID.CrystalStorm];
	public override HashSet<int> SummonLoot() => [ItemID.Smolstar, ItemID.RainbowCrystalStaff];
}
public class IcePool : ItemPool {
	public override HashSet<int> MeleeLoot() => [ItemID.IceBlade, ItemID.Amarok];
	public override HashSet<int> RangeLoot() => [ItemID.IceBoomerang, ItemID.IceBow];
	public override HashSet<int> MagicLoot() => [ItemID.FrostStaff, ItemID.FlowerofFrost, ItemID.WandofFrosting, ItemID.IceRod];
	public override HashSet<int> SummonLoot() => [ItemID.FlinxStaff, ItemID.CoolWhip];
}
public class ShadowPool : ItemPool {
	public override HashSet<int> MeleeLoot() => [ItemID.FieryGreatsword, ItemID.HelFire, ItemID.Sunfury];
	public override HashSet<int> RangeLoot() => [ItemID.Flamarang, ItemID.HellwingBow, ItemID.MoltenFury, ItemID.PhoenixBlaster];
	public override HashSet<int> MagicLoot() => [ItemID.Flamelash, ItemID.FlowerofFire, ItemID.DemonScythe];
	public override HashSet<int> SummonLoot() => [ItemID.ImpStaff, ItemID.FireWhip];
}
public class BloodPool : ItemPool {
	public override HashSet<int> MeleeLoot() => [ItemID.Bladetongue, ItemID.DripplerFlail];
	public override HashSet<int> RangeLoot() => [ItemID.BloodRainBow];
	public override HashSet<int> MagicLoot() => [ItemID.SharpTears];
	public override HashSet<int> SummonLoot() => [ItemID.VampireFrogStaff, ItemID.SanguineStaff];
}
/// <summary>
/// Overworld lootbox<br/>
/// Belong in forest, jungle and any variantion of those
/// </summary>
public class WoodPool : ItemPool {
	public override HashSet<int> MeleeLoot() => [.. TerrariaArrayID.AllWoodSword];
	public override HashSet<int> RangeLoot() => [.. TerrariaArrayID.AllWoodBowPHM];
	public override HashSet<int> MagicLoot() => [ItemID.WandofFrosting, ItemID.WandofSparking];
	public override HashSet<int> SummonLoot() => [ItemID.SlimeStaff, ItemID.BabyBirdStaff];
}
public class UniversalPool : ItemPool {
	public override HashSet<int> MeleeLoot() => [.. TerrariaArrayID.AllOreBroadSword, .. TerrariaArrayID.CommonAxe, .. TerrariaArrayID.AllOreShortSword,
	ItemID.Mace, ItemID.FlamingMace, ItemID.Katana, ItemID.Rally, ItemID.Spear, ItemID.WoodenBoomerang, ItemID.ChainKnife, ItemID.BladedGlove, ItemID.FalconBlade
	];
	public override HashSet<int> RangeLoot() => [.. TerrariaArrayID.AllOreBowPHM, ItemID.FlintlockPistol, ItemID.Musket, ItemID.Revolver, ItemID.Boomstick, ItemID.Minishark,
		ItemID.StylistKilLaKillScissorsIWish
		];
	public override HashSet<int> MagicLoot() => [.. TerrariaArrayID.AllGemStaffPHM];
	public override HashSet<int> SummonLoot() => [ItemID.SlimeStaff, ItemID.BabyBirdStaff, ItemID.BlandWhip];
	public override HashSet<int> PotionPool() => [ItemID.SwiftnessPotion, ItemID.RegenerationPotion, ItemID.IronskinPotion];
	public override HashSet<int> ArmorLoot() => [ItemID.CopperHelmet, ItemID.TinHelmet, ItemID.IronHelmet, ItemID.LeadHelmet, ItemID.SilverHelmet, ItemID.TungstenHelmet, ItemID.GoldHelmet, ItemID.PlatinumHelmet, ItemID.CopperChainmail, ItemID.TinChainmail, ItemID.IronChainmail, ItemID.LeadChainmail, ItemID.SilverChainmail, ItemID.TungstenChainmail, ItemID.GoldChainmail, ItemID.PlatinumChainmail, ItemID.CopperGreaves, ItemID.TinGreaves, ItemID.IronGreaves, ItemID.LeadGreaves, ItemID.SilverGreaves, ItemID.TungstenGreaves, ItemID.GoldGreaves, ItemID.PlatinumGreaves];
	public override HashSet<int> AccessoryLoot() => [ItemID.Aglet, ItemID.ClimbingClaws, ItemID.ShoeSpikes, ItemID.ShinyRedBalloon, ItemID.HermesBoots];
}
