using Microsoft.Xna.Framework;
using Roguelike.Common.Systems;
using Roguelike.Common.Utils;
using Roguelike.Contents.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.RoguelikeChange.ItemOverhaul.ItemOverhaul;
public class Roguelike_MagicGemStaff : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return UniversalSystem.Check_RLOH();
	}
	public override void SetDefaults(Item entity) {
		switch (entity.type) {
			case ItemID.AmethystStaff:
				entity.shoot = ModContent.ProjectileType<AmethystMagicalBolt>();
				entity.useTime = 3;
				entity.useAnimation = 15;
				entity.reuseDelay = 30;
				entity.mana = 25;
				entity.shootSpeed = 1;
				entity.damage += 5;
				break;
			case ItemID.TopazStaff:
				entity.shoot = ModContent.ProjectileType<TopazMagicalBolt>();
				entity.useTime = 3;
				entity.useAnimation = 18;
				entity.reuseDelay = 33;
				entity.mana = 26;
				entity.shootSpeed = 1;
				entity.damage += 5;
				break;
			case ItemID.SapphireStaff:
				entity.shoot = ModContent.ProjectileType<SapphireMagicalBolt>();
				entity.useTime = 3;
				entity.useAnimation = 18;
				entity.mana = 12;
				entity.damage -= 3;
				entity.shootSpeed = 1;
				break;
			case ItemID.EmeraldStaff:
				entity.shoot = ModContent.ProjectileType<EmeraldMagicalBolt>();
				entity.useTime = 10;
				entity.useAnimation = 20;
				entity.mana = 16;
				entity.shootSpeed = 1;
				break;
			case ItemID.RubyStaff:
				entity.damage += 5;
				entity.shoot = ModContent.ProjectileType<RubyMagicalBolt>();
				entity.shootSpeed = 1;
				entity.mana = 40;
				break;
			case ItemID.DiamondStaff:
				entity.damage += 5;
				entity.shoot = ModContent.ProjectileType<DiamondMagicalBolt>();
				entity.shootSpeed = 1;
				entity.mana = 40;
				break;
		}
	}
	public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		switch (item.type) {
			case ItemID.AmethystStaff:
				velocity = velocity.Vector2RotateByRandom(10);
				position = position.PositionOFFSET(velocity, 50);
				break;
			case ItemID.TopazStaff:
				velocity = velocity.Vector2RotateByRandom(15) * Main.rand.NextFloat(.75f, 1.25f);
				position = position.PositionOFFSET(velocity, 50);
				break;
			case ItemID.SapphireStaff:
				velocity = velocity.Vector2RotateByRandom(6) * Main.rand.NextFloat(.75f, 1.25f);
				position = position.PositionOFFSET(velocity, 50);
				break;
			case ItemID.EmeraldStaff:
				velocity *= Main.rand.NextFloat(1, 1.5f);
				position = position.PositionOFFSET(velocity, 50);
				break;
			case ItemID.DiamondStaff:
			case ItemID.RubyStaff:
				position = position.PositionOFFSET(velocity, 50);
				break;
		}
	}
}
