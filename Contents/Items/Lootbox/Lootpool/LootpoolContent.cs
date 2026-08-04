using Terraria.ID;
using Roguelike.Common.Utils;
using System.Collections.Generic;
using Terraria;

namespace Roguelike.Contents.Items.Lootbox.Lootpool;
public class LunarPool : ItemPool {

}
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
	public override HashSet<int> AccessoryLoot() => [
		ItemID.Aglet, ItemID.ClimbingClaws, ItemID.ShoeSpikes, ItemID.ShinyRedBalloon, ItemID.HermesBoots,
		ItemID.ShinyRedBalloon, ItemID.LuckyHorseshoe, ItemID.LuckyCoin, ItemID.ObsidianSkull, ItemID.ObsidianRose,
		ItemID.CloudinaBottle, ItemID.FartInABalloon, ItemID.Flipper, ItemID.BandofRegeneration, ItemID.BandofStarpower,
		ItemID.AdhesiveBandage, ItemID.Bezoar, ItemID.HandWarmer, ItemID.Blindfold, ItemID.ArmorPolish, ItemID.Megaphone,
		ItemID.Nazar, ItemID.TrifoldMap, ItemID.FastClock, ItemID.Vitamins
		];
}
public class Tier2Pool : ItemPool {
	public override HashSet<int> MeleeLoot()
		=> [
			ItemID.ZombieArm, ItemID.CandyCaneSword, ItemID.Katana, ItemID.IceBlade, ItemID.EnchantedSword, ItemID.PurpleClubberfish,
			ItemID.LightsBane, ItemID.BloodButcherer, ItemID.Starfury, ItemID.BeeKeeper, ItemID.BladeofGrass, ItemID.Muramasa, ItemID.BoneSword, ItemID.BatBat,
			..TerrariaArrayID.Phaseblade, ItemID.TentacleSpike, ItemID.SlapHand,
			ItemID.CorruptYoyo, ItemID.CrimsonYoyo, ItemID.JungleYoyo, ItemID.Code1, ItemID.HiveFive, ItemID.Valor, ItemID.Cascade,
			ItemID.Trident, ItemID.ThunderSpear, ItemID.TheRottedFork, ItemID.Swordfish,
			ItemID.ThornChakram, ItemID.Trimarang, ItemID.CombatWrench, ItemID.Flamarang,
			ItemID.BlueMoon, ItemID.Sunfury, ItemID.Anchor,
			ItemID.Terragrim, ItemID.Arkhalis, ItemID.JoustingLance,
		];

	public override HashSet<int> RangeLoot()
		=> [
			ItemID.DemonBow, ItemID.TendonBow, ItemID.BloodRainBow, ItemID.BeesKnees, ItemID.HellwingBow, ItemID.MoltenFury,
			ItemID.QuadBarrelShotgun, ItemID.Handgun, ItemID.PhoenixBlaster, ItemID.PewMaticHorn,
			ItemID.StarCannon, ItemID.Blowgun
		];
	public override HashSet<int> MagicLoot()
		=> [
			ItemID.ThunderStaff, ItemID.AmberStaff, ItemID.Vilethorn, ItemID.CrimsonRod,
			ItemID.WeatherPain, ItemID.MagicMissile, ItemID.AquaScepter, ItemID.FlowerofFire, ItemID.Flamelash,
			ItemID.ZapinatorGray, ItemID.SpaceGun, ItemID.BeeGun, 
			ItemID.WaterBolt, ItemID.BookofSkulls, ItemID.DemonScythe,
			];
	public override HashSet<int> SummonLoot() 
		=> [
			ItemID.HornetStaff, ItemID.VampireFrogStaff, ItemID.ImpStaff, ItemID.HoundiusShootius,
			ItemID.DD2BallistraTowerT1Popper, ItemID.DD2ExplosiveTrapT1Popper, ItemID.DD2FlameburstTowerT1Popper, ItemID.DD2LightningAuraT1Popper,
			];
	public override HashSet<int> PotionPool()
		=> [
			ItemID.ArcheryPotion, ItemID.EndurancePotion, ItemID.LifeforcePotion, ItemID.RagePotion, ItemID.WrathPotion, ItemID.SummoningPotion, ItemID.HeartreachPotion,
			];
	public override HashSet<int> AccessoryLoot() =>
		[
		ItemID.AdhesiveBandage, ItemID.Bezoar, ItemID.HandWarmer, ItemID.Blindfold, ItemID.ArmorPolish, ItemID.Megaphone,
		ItemID.Nazar, ItemID.TrifoldMap, ItemID.FastClock, ItemID.Vitamins,
		ItemID.FrogLeg, ItemID.BlizzardinaBottle, ItemID.SandstorminaBottle, ItemID.FlyingCarpet, ItemID.LavaCharm, ItemID.Magiluminescence, 
		ItemID.SpectreBoots
		];
	public override HashSet<int> ArmorLoot()
		=> [
			ItemID.NinjaHood, ItemID.NinjaShirt, ItemID.NinjaPants,
			ItemID.FossilHelm, ItemID.FossilShirt, ItemID.FossilPants,
			ItemID.ObsidianHelm, ItemID.ObsidianShirt, ItemID.ObsidianPants,
			ItemID.GladiatorHelmet, ItemID.GladiatorBreastplate, ItemID.GladiatorLeggings,
			ItemID.MeteorHelmet, ItemID.MeteorSuit, ItemID.MeteorLeggings,
			ItemID.JungleHat, ItemID.JungleShirt, ItemID.JunglePants,
			ItemID.AncientArmorHat, ItemID.AncientArmorShirt, ItemID.AncientArmorPants,
			ItemID.NecroHelmet, ItemID.NecroBreastplate, ItemID.NecroGreaves,
			ItemID.ShadowHelmet, ItemID.ShadowScalemail, ItemID.ShadowGreaves,
			ItemID.AncientShadowHelmet, ItemID.AncientShadowScalemail, ItemID.AncientShadowGreaves,
			ItemID.CrimsonHelmet, ItemID.CrimsonScalemail, ItemID.CrimsonGreaves,
			ItemID.BeeHeadgear, ItemID.BeeBreastplate, ItemID.BeeGreaves,
			ItemID.MoltenHelmet, ItemID.MoltenBreastplate, ItemID.MoltenGreaves,
			];
}
public class Tier1Pool : ItemPool {
	public override HashSet<int> MeleeLoot()
		=> [.. TerrariaArrayID.AllOreBroadSword,
			.. TerrariaArrayID.CommonAxe,
			.. TerrariaArrayID.AllOreShortSword,
			.. TerrariaArrayID.AllWoodSword, ItemID.CactusSword,
			ItemID.Mace, ItemID.FlamingMace, ItemID.BallOHurt, ItemID.TheMeatball,
			ItemID.WoodYoyo, ItemID.Rally,
			ItemID.Spear,
			ItemID.ChainKnife, ItemID.BladedGlove, ItemID.FalconBlade,ItemID.TaxCollectorsStickOfDoom,
			ItemID.EnchantedBoomerang, ItemID.WoodenBoomerang, ItemID.FruitcakeChakram, ItemID.BloodyMachete, ItemID.Shroomerang, ItemID.IceBoomerang,
			];
	public override HashSet<int> RangeLoot()
	=> [.. TerrariaArrayID.AllOreBowPHM, ..TerrariaArrayID.AllWoodBowPHM, ItemID.PearlwoodBow,
		ItemID.FlintlockPistol, ItemID.Musket, ItemID.RedRyder,ItemID.Revolver, ItemID.Boomstick, ItemID.Minishark,
		ItemID.StylistKilLaKillScissorsIWish, ItemID.Blowpipe, ItemID.Sandgun, ItemID.SnowballCannon, ItemID.PainterPaintballGun];
	public override HashSet<int> MagicLoot()
	=> [.. TerrariaArrayID.AllGemStaffPHM,
		ItemID.WandofFrosting, ItemID.WandofSparking
		];
	public override HashSet<int> SummonLoot()
	=> [ItemID.SlimeStaff, ItemID.BabyBirdStaff, ItemID.BlandWhip];
	public override HashSet<int> PotionPool()
	=> [ItemID.SwiftnessPotion, ItemID.RegenerationPotion, ItemID.IronskinPotion];
	public override HashSet<int> ArmorLoot()
	=> [ItemID.CopperHelmet, ItemID.CopperChainmail, ItemID.CopperGreaves,
		ItemID.TinHelmet, ItemID.TinChainmail,ItemID.TinGreaves,
		ItemID.IronHelmet,ItemID.IronChainmail,ItemID.IronGreaves,
		ItemID.LeadHelmet, ItemID.LeadChainmail,ItemID.LeadGreaves,
		ItemID.SilverHelmet,ItemID.SilverChainmail,ItemID.SilverGreaves,
		ItemID.TungstenHelmet, ItemID.TungstenChainmail,ItemID.TungstenGreaves,
		ItemID.GoldHelmet,ItemID.GoldChainmail,ItemID.GoldGreaves,
		ItemID.PlatinumHelmet,ItemID.PlatinumChainmail,ItemID.PlatinumGreaves,
		ItemID.WoodHelmet, ItemID.WoodBreastplate, ItemID.WoodGreaves,
		ItemID.AshWoodHelmet, ItemID.AshWoodBreastplate, ItemID.AshWoodGreaves,
		ItemID.EbonwoodHelmet, ItemID.EbonwoodBreastplate, ItemID.EbonwoodGreaves,
		ItemID.ShadewoodHelmet, ItemID.ShadewoodBreastplate, ItemID.ShadewoodGreaves,
		ItemID.PalmWoodHelmet, ItemID.PalmWoodBreastplate, ItemID.PalmWoodGreaves,
		ItemID.PearlwoodHelmet, ItemID.PearlwoodBreastplate, ItemID.PearlwoodGreaves,
		ItemID.RichMahoganyHelmet, ItemID.RichMahoganyBreastplate, ItemID.RichMahoganyGreaves,
		ItemID.BorealWoodHelmet, ItemID.BorealWoodBreastplate, ItemID.BorealWoodGreaves,
		ItemID.CactusHelmet, ItemID.CactusBreastplate, ItemID.CactusLeggings
		];
	public override HashSet<int> AccessoryLoot()
	=> [ItemID.Aglet, ItemID.ClimbingClaws, ItemID.ShoeSpikes, ItemID.ShinyRedBalloon, ItemID.HermesBoots,
		ItemID.ShinyRedBalloon, ItemID.LuckyHorseshoe, ItemID.LuckyCoin, ItemID.ObsidianSkull, ItemID.ObsidianRose,
		ItemID.CloudinaBottle, ItemID.FartInABalloon, ItemID.Flipper, ItemID.BandofRegeneration, ItemID.BandofStarpower,
		ItemID.WaterWalkingBoots, ItemID.FlurryBoots, ItemID.FairyBoots, ItemID.SandBoots,
		];
}
