using Roguelike.Contents.Items.Lootbox;
using Roguelike.Contents.Items.Lootbox.MiscLootbox;
using Roguelike.Contents.Items.RelicItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.Utils;
public static partial class ModUtils {
	/// <summary>
	/// Return weapon base on world/player progression
	/// </summary>
	/// <param name="ReturnWeapon"></param>
	/// <param name="Amount"></param>
	/// <param name="rng"></param>
	public static void GetWeapon(out int ReturnWeapon, out int Amount, int rng = 0) {
		if (rng > 5 || rng <= 0) {
			rng = Main.rand.Next(1, 5);
		}
		ReturnWeapon = 0;
		Amount = 1;
		var DropItemMelee = new List<int>();
		var DropItemRange = new List<int>();
		var DropItemMagic = new List<int>();
		var DropItemSummon = new List<int>();
		DropItemMelee.AddRange(TerrariaArrayID.MeleePreBoss);
		DropItemRange.AddRange(TerrariaArrayID.RangePreBoss);
		DropItemMagic.AddRange(TerrariaArrayID.MagicPreBoss);
		DropItemSummon.AddRange(TerrariaArrayID.SummonPreBoss);
		DropItemMelee.AddRange(TerrariaArrayID.MeleePreEoC);
		DropItemRange.AddRange(TerrariaArrayID.RangePreEoC);
		DropItemMagic.AddRange(TerrariaArrayID.MagicPreEoC);
		DropItemSummon.AddRange(TerrariaArrayID.SummonerPreEoC);
		if (NPC.downedBoss1) {
			DropItemMelee.Add(ItemID.Code1);
			DropItemMagic.Add(ItemID.ZapinatorGray);
		}
		if (NPC.downedBoss2) {
			DropItemMelee.AddRange(TerrariaArrayID.MeleeEvilBoss);
			DropItemRange.Add(ItemID.MoltenFury);
			DropItemRange.Add(ItemID.StarCannon);
			DropItemRange.Add(ItemID.AleThrowingGlove);
			DropItemRange.Add(ItemID.Harpoon);
			DropItemMagic.AddRange(TerrariaArrayID.MagicEvilBoss);
			DropItemSummon.Add(ItemID.ImpStaff);
		}
		if (NPC.downedBoss3) {
			DropItemMelee.AddRange(TerrariaArrayID.MeleeSkel);
			DropItemRange.AddRange(TerrariaArrayID.RangeSkele);
			DropItemMagic.AddRange(TerrariaArrayID.MagicSkele);
			DropItemSummon.AddRange(TerrariaArrayID.SummonSkele);
		}
		if (NPC.downedQueenBee) {
			DropItemMelee.Add(ItemID.BeeKeeper);
			DropItemRange.Add(ItemID.BeesKnees); DropItemRange.Add(ItemID.Blowgun);
			DropItemMagic.Add(ItemID.BeeGun);
			DropItemSummon.Add(ItemID.HornetStaff);
		}
		if (NPC.downedDeerclops) {
			DropItemRange.Add(ItemID.PewMaticHorn);
			DropItemMagic.Add(ItemID.WeatherPain);
			DropItemSummon.Add(ItemID.HoundiusShootius);
		}
		if (Main.hardMode) {
			DropItemMelee.AddRange(TerrariaArrayID.MeleeHM);
			DropItemRange.AddRange(TerrariaArrayID.RangeHM);
			DropItemMagic.AddRange(TerrariaArrayID.MagicHM);
			DropItemSummon.AddRange(TerrariaArrayID.SummonHM);
		}
		if (NPC.downedQueenSlime) {
			DropItemMelee.AddRange(TerrariaArrayID.MeleeQS);
			DropItemSummon.Add(ItemID.Smolstar);
		}
		if (NPC.downedMechBossAny) {
			DropItemMelee.AddRange(TerrariaArrayID.MeleeMech);
			DropItemRange.Add(ItemID.SuperStarCannon);
			DropItemRange.Add(ItemID.DD2PhoenixBow);
			DropItemMagic.Add(ItemID.UnholyTrident);
		}
		if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3) {
			DropItemMelee.AddRange(TerrariaArrayID.MeleePostAllMechs);
			DropItemRange.AddRange(TerrariaArrayID.RangePostAllMech);
			DropItemMagic.AddRange(TerrariaArrayID.MagicPostAllMech);
			DropItemSummon.AddRange(TerrariaArrayID.SummonPostAllMech);
		}
		if (NPC.downedPlantBoss) {
			DropItemMelee.AddRange(TerrariaArrayID.MeleePostPlant);
			DropItemRange.AddRange(TerrariaArrayID.RangePostPlant);
			DropItemMagic.AddRange(TerrariaArrayID.MagicPostPlant);
			DropItemSummon.AddRange(TerrariaArrayID.SummonPostPlant);
		}
		if (NPC.downedGolemBoss) {
			DropItemMelee.AddRange(TerrariaArrayID.MeleePostGolem);
			DropItemRange.AddRange(TerrariaArrayID.RangePostGolem);
			DropItemMagic.AddRange(TerrariaArrayID.MagicPostGolem);
			DropItemSummon.AddRange(TerrariaArrayID.SummonPostGolem);
		}
		if (NPC.downedEmpressOfLight) {
			DropItemMelee.AddRange(TerrariaArrayID.MeleePreLuna);
			DropItemRange.AddRange(TerrariaArrayID.RangePreLuna);
			DropItemMagic.AddRange(TerrariaArrayID.MagicPreLuna);
			DropItemSummon.AddRange(TerrariaArrayID.SummonPreLuna);
		}
		if (NPC.downedAncientCultist) {
			DropItemMelee.Add(ItemID.DayBreak);
			DropItemMelee.Add(ItemID.SolarEruption);
			DropItemRange.Add(ItemID.Phantasm);
			DropItemRange.Add(ItemID.VortexBeater);
			DropItemMagic.Add(ItemID.NebulaArcanum);
			DropItemMagic.Add(ItemID.NebulaBlaze);
			DropItemSummon.Add(ItemID.StardustCellStaff);
			DropItemSummon.Add(ItemID.StardustDragonStaff);
		}
		if (NPC.downedMoonlord) {
			DropItemMelee.Add(ItemID.StarWrath);
			DropItemMelee.Add(ItemID.Meowmere);
			DropItemMelee.Add(ItemID.Terrarian);
			DropItemRange.Add(ItemID.SDMG);
			DropItemRange.Add(ItemID.Celeb2);
			DropItemMagic.Add(ItemID.LunarFlareBook);
			DropItemMagic.Add(ItemID.LastPrism);
			DropItemSummon.Add(ItemID.RainbowCrystalStaff);
			DropItemSummon.Add(ItemID.MoonlordTurretStaff);
		}
		switch (rng) {
			case 0:
				ReturnWeapon = ItemID.None;
				break;
			case 1:
				ReturnWeapon = Main.rand.NextFromCollection(DropItemMelee);
				break;
			case 2:
				ReturnWeapon = Main.rand.NextFromCollection(DropItemRange);
				break;
			case 3:
				ReturnWeapon = Main.rand.NextFromCollection(DropItemMagic);
				break;
			case 4:
				ReturnWeapon = Main.rand.NextFromCollection(DropItemSummon);
				break;
		}
	}
	/// <summary>
	/// This is a static function version of GetWeapon in LootBoxBase<br/>
	/// Lootbox check is already presented in this function and as such it is not needed to check if the item a lootboxbase or not<br/>
	/// it is also important to remember that this function will automatically handle dropping item
	/// </summary>
	/// <param name="lootbox">The lootbox item</param>
	/// <param name="player">The player</param>
	/// <param name="rng">rng number</param>
	/// <param name="additiveModify">additive direct modify to amount of weapons can be given</param>
	public static void GetWeapon(Item lootbox, Player player, int rng = 0, float additiveModify = 1) {
		if (lootbox.ModItem is not LootBoxBase item) {
			return;
		}
		var modplayer = player.ModPlayerStats();
		modplayer.GetAmount();
		int weaponAmount = (int)Math.Clamp(MathF.Ceiling(modplayer.weaponAmount * additiveModify), 1, 999999);
		item.GetWeapon(player.GetSource_OpenItem(lootbox.type), player, weaponAmount);
	}
	/// <summary>
	/// Use this if you only care about getting ammo for weapon
	/// </summary>
	/// <param name="player"></param>
	/// <param name="weapon"></param>
	/// <param name="AmountModifier"></param>
	public static void AmmoForWeapon(Player player, int weapon, float AmountModifier = 1) {
		AmmoForWeapon(ModItemLib.ListLootboxType.FirstOrDefault(), player, weapon, AmountModifier);
	}
	/// <summary>
	/// Automatically quick drop player ammo item accordingly to weapon ammo type
	/// </summary>
	/// <param name="lootbox">The id of lootbox</param>
	/// <param name="player">The player</param>
	/// <param name="weapon">Weapon need to be checked</param>
	/// <param name="AmountModifier">Modify the ammount of ammo will be given</param>
	public static void AmmoForWeapon(int lootbox, Player player, int weapon, float AmountModifier = 1) {
		var entitySource = player.GetSource_OpenItem(lootbox);
		var weapontoCheck = ContentSamples.ItemsByType[weapon];
		if (weapontoCheck.consumable || weapontoCheck.useAmmo == AmmoID.None) {
			if (weapontoCheck.mana > 0) {
				player.QuickSpawnItem(entitySource, ItemID.LesserManaPotion, 5);
			}
			return;
		}
		//The most ugly code
		int Amount = (int)(350 * AmountModifier);
		int Ammo;
		if (Main.masterMode) {
			Amount += 150;
		}
		List<int> DropArrowAmmo = new();
		List<int> DropBulletAmmo = new();
		List<int> DropDartAmmo = new();

		DropArrowAmmo.AddRange(TerrariaArrayID.defaultArrow);
		DropBulletAmmo.AddRange(TerrariaArrayID.defaultBullet);
		DropDartAmmo.AddRange(TerrariaArrayID.defaultDart);

		if (Main.hardMode) {
			DropArrowAmmo.AddRange(TerrariaArrayID.ArrowHM);
			DropBulletAmmo.AddRange(TerrariaArrayID.BulletHM);
			DropDartAmmo.AddRange(TerrariaArrayID.DartHM);
		}
		if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3) {
			DropArrowAmmo.Add(ItemID.ChlorophyteArrow);
			DropBulletAmmo.Add(ItemID.ChlorophyteBullet);
		}
		if (NPC.downedPlantBoss) {
			DropArrowAmmo.Add(ItemID.VenomArrow);
			DropBulletAmmo.Add(ItemID.NanoBullet);
			DropBulletAmmo.Add(ItemID.VenomBullet);
		}
		if (weapontoCheck.useAmmo == AmmoID.Arrow) {
			Ammo = Main.rand.NextFromCollection(DropArrowAmmo);
		}
		else if (weapontoCheck.useAmmo == AmmoID.Bullet) {
			Ammo = Main.rand.NextFromCollection(DropBulletAmmo);
		}
		else if (weapontoCheck.useAmmo == AmmoID.Dart) {
			Ammo = Main.rand.NextFromCollection(DropDartAmmo);
		}
		else if (weapontoCheck.mana > 0) {
			Ammo = ItemID.LesserManaPotion;
			Amount = (int)(10 * AmountModifier);
		}
		else if (weapontoCheck.useAmmo == AmmoID.Rocket) {
			Ammo = Main.rand.Next(new int[] { ItemID.RocketI, ItemID.RocketII, ItemID.RocketIII, ItemID.RocketIV });
		}
		else if (weapontoCheck.useAmmo == AmmoID.Snowball) {
			Ammo = ItemID.Snowball;
		}
		else if (weapontoCheck.useAmmo == AmmoID.CandyCorn) {
			Ammo = ItemID.CandyCorn;
		}
		else if (weapontoCheck.useAmmo == AmmoID.JackOLantern) {
			Ammo = ItemID.JackOLantern;
		}
		else if (weapontoCheck.useAmmo == AmmoID.Flare) {
			Ammo = ItemID.Flare;
		}
		else if (weapontoCheck.useAmmo == AmmoID.Stake) {
			Ammo = ItemID.Stake;
		}
		else if (weapontoCheck.useAmmo == AmmoID.StyngerBolt) {
			Ammo = ItemID.StyngerBolt;
		}
		else if (weapontoCheck.useAmmo == AmmoID.NailFriendly) {
			Ammo = ItemID.Nail;
		}
		else if (weapontoCheck.useAmmo == AmmoID.Gel) {
			Ammo = ItemID.Gel;
		}
		else if (weapontoCheck.useAmmo == AmmoID.FallenStar) {
			Ammo = ModContent.ItemType<StarTreasureChest>();
			if (Amount < 1) {
				Amount = 1;
			}
			else if (Amount > 1) {
				Amount = (int)(1 * AmountModifier);
			}
		}
		else if (weapontoCheck.useAmmo == AmmoID.Sand) {
			Ammo = ItemID.SandBlock;
		}
		else {
			Ammo = ItemID.WoodenArrow;
		}
		player.QuickSpawnItem(entitySource, Ammo, Amount);
	}
	/// <summary>
	/// This function will automatically handle drop for you so no need to do it yourself
	/// </summary>
	/// <param name="type"></param>
	/// <param name="player"></param>
	public static void GetAccessories(int type, Player player) {
		var entitySource = player.GetSource_OpenItem(type);
		int acc = Main.rand.Next(TerrariaArrayID.EveryCombatHealtMovehAcc);
		player.QuickSpawnItem(entitySource, acc);
	}
	/// <summary>
	/// This function will automatically handle drop for you so no need to do it yourself
	/// </summary>
	/// <param name="type"></param>
	/// <param name="player"></param>
	public static void GetPotion(int type, Player player) {
		List<int> DropItemPotion = [.. TerrariaArrayID.NonMovementPotion, .. TerrariaArrayID.MovementPotion, .. ModItemLib.LootboxPotion.Select(i => i.type)];
		DropItemPotion.Add(ItemID.LifeforcePotion);
		DropItemPotion.Add(ItemID.InfernoPotion);
		var modplayer = player.ModPlayerStats();
		modplayer.GetAmount();
		var entitySource = player.GetSource_OpenItem(type);
		for (int i = 0; i < modplayer.potionTypeAmount; i++) {
			player.QuickSpawnItem(entitySource, Main.rand.Next(DropItemPotion), modplayer.potionNumAmount);
		}
	}
	public static void GetArmorPiece(int type, Player player, bool randomized = false) {
		var entitySource = player.GetSource_OpenItem(type);
		if (randomized) {
			player.QuickSpawnItem(entitySource, Main.rand.Next(TerrariaArrayID.EveryArmorPiece));
			return;
		}
		player.QuickSpawnItem(entitySource, Main.rand.Next(TerrariaArrayID.HeadAllPiece));
		player.QuickSpawnItem(entitySource, Main.rand.Next(TerrariaArrayID.BodyAllPiece));
		player.QuickSpawnItem(entitySource, Main.rand.Next(TerrariaArrayID.LegsAllPiece));
	}
	public static void GetRelic(int type, Player player, int amount = 1) {
		var entitySource = player.GetSource_OpenItem(type);
		amount = player.ModPlayerStats().ModifyGetAmount(amount);

		for (int i = 0; i < amount; i++) {
			player.QuickSpawnItem(entitySource, ModContent.ItemType<Relic>());
		}
	}
	public static void GetSkillLootbox(int type, Player player, int amount = 1) {
		var entitySource = player.GetSource_OpenItem(type);
		amount = player.ModPlayerStats().ModifyGetAmount(amount);

		for (int i = 0; i < amount; i++) {
			player.QuickSpawnItem(entitySource, ModContent.ItemType<SkillLootBox>());
		}
	}
	/// <summary>
	/// This method return a set of armor with randomize piece of armor accordingly to progression
	/// </summary>
	public static void GetArmorForPlayer(IEntitySource entitySource, Player player, bool returnOnlyPiece = false) {
		var HeadArmor = new List<int>();
		var BodyArmor = new List<int>();
		var LegArmor = new List<int>();
		HeadArmor.AddRange(TerrariaArrayID.HeadArmorPreBoss);
		BodyArmor.AddRange(TerrariaArrayID.BodyArmorPreBoss);
		LegArmor.AddRange(TerrariaArrayID.LegArmorPreBoss);
		if (NPC.downedBoss2) {
			HeadArmor.AddRange(TerrariaArrayID.HeadArmorPostEvil);
			BodyArmor.AddRange(TerrariaArrayID.BodyArmorPostEvil);
			LegArmor.AddRange(TerrariaArrayID.LegArmorPostEvil);
		}
		if (NPC.downedBoss3) {
			HeadArmor.Add(ItemID.NecroHelmet);
			BodyArmor.Add(ItemID.NecroBreastplate);
			LegArmor.Add(ItemID.NecroGreaves);
		}
		if (NPC.downedQueenBee) {
			HeadArmor.Add(ItemID.BeeHeadgear);
			BodyArmor.Add(ItemID.BeeBreastplate);
			LegArmor.Add(ItemID.BeeGreaves);
		}
		if (Main.hardMode) {
			HeadArmor.AddRange(TerrariaArrayID.HeadArmorHardMode);
			BodyArmor.AddRange(TerrariaArrayID.BodyArmorHardMode);
			LegArmor.AddRange(TerrariaArrayID.LegArmorHardMode);
		}
		if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3) {
			HeadArmor.AddRange(TerrariaArrayID.HeadArmorPostMech);
			BodyArmor.AddRange(TerrariaArrayID.BodyArmorPostMech);
			LegArmor.AddRange(TerrariaArrayID.LegArmorPostMech);
		}
		if (NPC.downedPlantBoss) {
			HeadArmor.AddRange(TerrariaArrayID.HeadArmorPostPlant);
			BodyArmor.AddRange(TerrariaArrayID.BodyArmorPostPlant);
			LegArmor.AddRange(TerrariaArrayID.LegArmorPostPlant);
		}
		if (NPC.downedGolemBoss) {
			HeadArmor.AddRange(TerrariaArrayID.HeadArmorPostGolem);
			BodyArmor.AddRange(TerrariaArrayID.BodyArmorPostGolem);
			LegArmor.AddRange(TerrariaArrayID.LegArmorPostGolem);
		}
		if (!returnOnlyPiece) {
			player.QuickSpawnItem(entitySource, Main.rand.Next(HeadArmor));
			player.QuickSpawnItem(entitySource, Main.rand.Next(BodyArmor));
			player.QuickSpawnItem(entitySource, Main.rand.Next(LegArmor));
		}
		else {
			player.QuickSpawnItem(entitySource, Main.rand.Next([.. LegArmor, .. BodyArmor, .. HeadArmor]));
		}
	}
}
