using Microsoft.Xna.Framework;
using Roguelike.Texture;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.Weapon.SummonerSynergyWeapon.StickySlime;

internal class StickyFlower : SynergyModItem {
	public override void Synergy_SetStaticDefaults() {
		ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true;
		ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
	}
	public override void SetDefaults() {
		Item.damage = 15;
		Item.mana = 10;
		Item.width = 32;
		Item.height = 32;
		Item.useTime = Item.useAnimation = 20;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.value = Item.sellPrice(gold: 30);
		Item.rare = ItemRarityID.Cyan;
		Item.UseSound = SoundID.Item44;

		Item.noMelee = true;
		Item.DamageType = DamageClass.Summon;
		Item.buffType = ModContent.BuffType<StickyFriend>();

		Item.shoot = ModContent.ProjectileType<GhostSlime>();
	}
	public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		position = Main.MouseWorld;
	}
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		player.AddBuff(Item.buffType, 2);
		var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
		projectile.originalDamage = Item.damage;
		CanShootItem = false;
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.SlimeStaff)
			.AddIngredient(ItemID.AbigailsFlower)
			.Register();
	}
}
public class StickyFriend : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		// DisplayName.SetDefault("slimy ghost friend");
		// Description.SetDefault("Will tries his best");

		Main.buffNoSave[Type] = true; // This buff won't save when you exit the world
		Main.buffNoTimeDisplay[Type] = true; // The time remaining won't display on this buff
	}

	public override void Update(Player player, ref int buffIndex) {
		// If the minions exist reset the buff time, otherwise remove the buff from the player
		if (player.ownedProjectileCounts[ModContent.ProjectileType<GhostSlime>()] > 0) {
			player.buffTime[buffIndex] = 18000;
		}
		else {
			player.DelBuff(buffIndex);
			buffIndex--;
		}
	}
}
