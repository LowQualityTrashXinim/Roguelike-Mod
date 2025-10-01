using Microsoft.Xna.Framework;
using ModJam2.Common.Subworlds;
using ModJam2.Common.Wrapper;
using Roguelike.Common.RoguelikeMode;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.Consumable.Scroll;
using SubworldLibrary;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace ModJam2.Common.Subworlds {
	public class CursedKingdomSubworld : Subworld {
		public override int Width => 800;

		public override int Height => 420;

		public override List<GenPass> Tasks => new()
		{
			new GenPass_Seed(),
			new GenPass_CursedKingdom("Cursed Kingdom", 0.01f)
		};
		public override void OnEnter() {
			Main.NewText("Cursed Kingdom Domain: Disable natural life regeneration", Color.DarkRed);
		}
	}
}
public class GenPass_Seed : GenPass {
	public GenPass_Seed() : base("Set Seed", 0.01f) { }

	protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration) {
		Main.ActiveWorldFileData.SetSeedToRandom();
	}
}
public class GenPass_CursedKingdom : GenPass {
	public GenPass_CursedKingdom(string name, double loadWeight) : base(name, loadWeight) {
	}

	protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration) {
		int length = 800 * 420;
		short counterH = 0;
		Main.worldSurface = 0;
		Main.rockLayer = 0;
		for (int i = 0; i < length; i++) {
			int x = i % 800;
			WorldGen.PlaceTile(x, counterH, TileID.Stone);
			if (i % 800 == 0 && i != 0) {
				counterH += 1;
			}
		}
		CursedKingdom_GenSystem system = ModContent.GetInstance<CursedKingdom_GenSystem>();
		if (system.Place_CursedKingdom()) {
			system.Place_ChestWithLoot();
			int spawnX, spawnY = 0;
			do {
				spawnX = Main.rand.Next(2, 800);
				spawnY = Main.rand.Next(2, 420);
			} while (!ModUtils.Check_PositionValid(spawnX, spawnY));
			Main.spawnTileX = spawnX;
			Main.spawnTileY = spawnY;
		}
	}
}
public class CursedKingdom_GenSystem : ModSystem {
	public void Place_CursedKingdomEntrance(int X, int Y) {
		var data = ModWrapper.Get_StructureData("Assets/CK_Entrance", Mod);
		int Width = data.width / 2;
		int Height = data.height / 2;
		Point16 point = new(X - Width, Y - Height);
		if (ModWrapper.IsInBound(data, point)) {
			ModContent.GetInstance<RogueLikeWorldGen>().CursedKingdomArea = new(point.X, point.Y, data.width, data.height);
			ModWrapper.GenerateFromData(data, point);
		}
	}
	public bool Place_CursedKingdom() {
		var data = ModWrapper.Get_StructureData("Assets/CursedKingdom", Mod);
		int X = Main.maxTilesX / 2;
		int Y = Main.maxTilesY / 2;
		int Width = data.width / 2;
		int Height = data.height / 2;
		Point16 point = new(X - Width, Y - Height);
		if (ModWrapper.IsInBound(data, point)) {
			ModWrapper.GenerateFromData(data, point);
			return true;
		}
		return false;
	}
	public void Place_ChestWithLoot() {
		UnifiedRandom rand = WorldGen.genRand;
		int chestAmount = 125 + rand.Next(0, 25);
		for (int i = 0; i < chestAmount; i++) {
			int X = rand.Next(0, Main.maxTilesX);
			int Y = rand.Next(0, Main.maxTilesY);
			int chestI = WorldGen.PlaceChest(X, Y);
			if (chestI < 0 || chestI >= Main.maxChests) {
				i--;
				continue;
			}
			Chest chest = Main.chest[chestI];
			for (int a = 0; a < chest.item.Length; a++) {
				if (Main.rand.NextFloat() >= .1f) {
					continue;
				}
				int item = ChestLoot(Main.rand.NextFloat());
				chest.item[a] = new Item(item);
			}
		}
	}
	/// <summary>
	/// Use this to set up chest loot !
	/// </summary>
	/// <param name="luck">Recommend us Main.rand.NextFloat as it is</param>
	/// <returns></returns>
	public static int ChestLoot(float luck) {
		if (luck >= .9f) {
			return ModContent.ItemType<ScrollOfInvincibility>();
		}
		else if (luck >= .8f) {
			return Main.rand.Next([ModContent.ItemType<ScrollofStrike>(), ModContent.ItemType<ScrollOfTeleportation>()]);
		}
		else if (luck >= .85f) {
			return Main.rand.Next([ModContent.ItemType<ScrollOfMaterialize>(), ModContent.ItemType<ScrollofEvasive>()]);
		}
		else if (luck >= .8f) {
			return Main.rand.Next([ModContent.ItemType<ScrollOfFire>(), ModContent.ItemType<ScrollOfWater>()]);
		}
		else if (luck >= .7f) {
			return Main.rand.Next(new int[]
			{
				ItemID.CobaltHelmet,ItemID.CobaltHat, ItemID.CobaltMask,ItemID.MythrilHelmet,ItemID.MythrilHat,ItemID.MythrilHood,ItemID.AdamantiteHelmet,ItemID.AdamantiteHeadgear,ItemID.AdamantiteMask,ItemID.PalladiumHelmet,ItemID.PalladiumHeadgear,ItemID.PalladiumMask,ItemID.OrichalcumHelmet,ItemID.OrichalcumHeadgear,ItemID.OrichalcumMask,ItemID.TitaniumHelmet,ItemID.TitaniumHeadgear, ItemID.TitaniumMask,ItemID.CobaltBreastplate, ItemID.MythrilChainmail,ItemID.AdamantiteBreastplate, ItemID.PalladiumBreastplate,ItemID.OrichalcumBreastplate,ItemID.TitaniumBreastplate,ItemID.CobaltLeggings, ItemID.MythrilGreaves,ItemID.AdamantiteLeggings, ItemID.PalladiumLeggings,ItemID.OrichalcumLeggings,ItemID.TitaniumLeggings,

			});
		}
		else if (luck >= .65f) {
			return Main.rand.Next(
				new int[] {
					ItemID.CobaltSword, ItemID.MythrilSword, ItemID.AdamantiteSword, ItemID.PalladiumSword, ItemID.OrichalcumSword, ItemID.TitaniumSword,
				ItemID.CobaltRepeater, ItemID.MythrilRepeater, ItemID.AdamantiteRepeater, ItemID.PalladiumRepeater, ItemID.OrichalcumRepeater, ItemID.TitaniumRepeater});
		}
		else if (luck >= .15f) {
			return Main.rand.Next(new int[] { ItemID.HealingPotion, ItemID.RegenerationPotion, ItemID.IronskinPotion, ItemID.SwiftnessPotion, ItemID.EndurancePotion, ItemID.WrathPotion, ItemID.RagePotion, ItemID.LifeforcePotion, ItemID.TeleportationPotion });
		}
		else if (luck >= .05f) {
			return Main.rand.Next(new int[] { ItemID.ThrowingKnife, ItemID.PoisonedKnife, ItemID.Shuriken, ItemID.HealingPotion });
		}
		return ItemID.ThrowingKnife;
	}
}
public class CursedKingdomPlayer : ModPlayer {
	public override void UpdateLifeRegen() {
		if (SubworldSystem.IsActive<CursedKingdomSubworld>()) {
			Player.lifeRegenTime = 0;
		}
	}
	public override void UpdateEquips() {
		if (SubworldSystem.IsActive<CursedKingdomSubworld>()) {
			Player.AddBuff(BuffID.NoBuilding, 2);
		}
	}
	public override bool CanUseItem(Item item) {
		if (SubworldSystem.IsActive<CursedKingdomSubworld>()) {
			return !ProjectileID.Sets.Explosive[item.shoot];
		}
		return base.CanUseItem(item);
	}
}
