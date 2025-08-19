using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
 
using Roguelike.Contents.Items.RelicItem;
using Roguelike.Contents.Items.Chest;
using Roguelike.Common.Utils;
using Roguelike.Contents.Skill;
using Roguelike.Texture;
using Roguelike.Common.Systems.TrialSystem;
using Roguelike;
public class AltarItem : ModItem {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.width = Item.height = 30;
		Item.useTime = Item.useAnimation = 30;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.createTile = ModContent.TileType<Altar>();
	}
}
public abstract class Altar : ModTile {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override string HighlightTexture => ModTexture.MissingTexture_Default;
	public bool Activated = false;
	public override void SetStaticDefaults() {
		Main.tileSolid[Type] = false;
		Main.tileMergeDirt[Type] = true;
		Main.tileBlockLight[Type] = true;
		Main.tileLighted[Type] = true;
		DustType = DustID.Stone;
		AddMapEntry(new Color(200, 200, 200));
	}
	public override bool RightClick(int i, int j) {
		Player player = Main.LocalPlayer;
		WorldGen.KillTile(i, j, noItem: true);
		On_RightClick(player, i, j);
		return base.RightClick(i, j);
	}
	public virtual void On_RightClick(Player player, int i, int j) { }
}
public class RelicAltar : Altar {
	public override void On_RightClick(Player player, int i, int j) {
		player.QuickSpawnItem(player.GetSource_TileInteraction(i, j), ModContent.ItemType<Relic>());
		for (int a = 0; a < 30; a++) {
			int dust = Dust.NewDust(new Vector2(i, j).ToWorldCoordinates(), 0, 0, DustID.Cloud, Scale: Main.rand.NextFloat(2, 3));
			Main.dust[dust].velocity = Main.rand.NextVector2Circular(4, 4);
		}
	}
}
public class WeaponAltar : Altar {
	public override void On_RightClick(Player player, int i, int j) {
		LootBoxBase.GetWeapon(out int weapon, out int amount);
		player.QuickSpawnItem(player.GetSource_TileInteraction(i, j), weapon, amount);

		for (int a = 0; a < 30; a++) {
			int dust = Dust.NewDust(new Vector2(i, j).ToWorldCoordinates(), 0, 0, DustID.Cloud, Scale: Main.rand.NextFloat(2, 3));
			Main.dust[dust].velocity = Main.rand.NextVector2Circular(4, 4);
		}
	}
}
public class ArmorAltar : Altar {
	public override void On_RightClick(Player player, int i, int j) {
		LootBoxBase.GetArmorPiece(ModContent.ItemType<WoodenLootBox>(), player, true);

		for (int a = 0; a < 30; a++) {
			int dust = Dust.NewDust(new Vector2(i, j).ToWorldCoordinates(), 0, 0, DustID.Cloud, Scale: Main.rand.NextFloat(2, 3));
			Main.dust[dust].velocity = Main.rand.NextVector2Circular(4, 4);
		}
	}
}
public class SkillAltar : Altar {
	public override void On_RightClick(Player player, int i, int j) {
		player.GetModPlayer<SkillHandlePlayer>().RequestAddSkill_Inventory(Main.rand.Next(SkillModSystem.TotalCount), false);

		for (int a = 0; a < 30; a++) {
			int dust = Dust.NewDust(new Vector2(i, j).ToWorldCoordinates(), 0, 0, DustID.Cloud, Scale: Main.rand.NextFloat(2, 3));
			Main.dust[dust].velocity = Main.rand.NextVector2Circular(4, 4);
		}
	}
}
public class PotionAltar : Altar {
	public override void On_RightClick(Player player, int i, int j) {
		LootBoxBase.GetPotion(ModContent.ItemType<WoodenLootBox>(), player);

		for (int a = 0; a < 30; a++) {
			int dust = Dust.NewDust(new Vector2(i, j).ToWorldCoordinates(), 0, 0, DustID.Cloud, Scale: Main.rand.NextFloat(2, 3));
			Main.dust[dust].velocity = Main.rand.NextVector2Circular(4, 4);
		}
	}
}
public class FoodAltar : Altar {
	public override void On_RightClick(Player player, int i, int j) {
		player.QuickSpawnItem(player.GetSource_TileInteraction(i, j), Main.rand.Next(TerrariaArrayID.AllFood), Main.rand.Next(5, 10));

		for (int a = 0; a < 30; a++) {
			int dust = Dust.NewDust(new Vector2(i, j).ToWorldCoordinates(), 0, 0, DustID.Cloud, Scale: Main.rand.NextFloat(2, 3));
			Main.dust[dust].velocity = Main.rand.NextVector2Circular(4, 4);
		}
	}
}

public abstract class BossItemAltar : Altar {
	protected virtual int BossItemID => ItemID.DirtBlock;
	public override void On_RightClick(Player player, int i, int j) {
		player.QuickSpawnItem(player.GetSource_TileInteraction(i, j), BossItemID);

		for (int a = 0; a < 30; a++) {
			int dust = Dust.NewDust(new Vector2(i, j).ToWorldCoordinates(), 0, 0, DustID.Cloud, Scale: Main.rand.NextFloat(2, 3));
			Main.dust[dust].velocity = Main.rand.NextVector2Circular(4, 4);
		}
	}
}
public class SlimeBossAltar : BossItemAltar {
	protected override int BossItemID => ItemID.SlimeCrown;
}
public class EoCBossAltar : BossItemAltar {
	protected override int BossItemID => ItemID.SuspiciousLookingEye;
}
public class StartTrialAltar_Template_1 : Altar {
	public override void On_RightClick(Player player, int i, int j) {
		TrialModSystem.SetTrial(ModTrial.GetTrialType<TestTrial>(), new Vector2(i, j).ToWorldCoordinates());
	}
}
