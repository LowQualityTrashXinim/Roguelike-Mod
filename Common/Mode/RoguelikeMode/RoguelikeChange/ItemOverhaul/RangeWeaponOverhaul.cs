using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Roguelike.Common.Systems;
using Roguelike.Common.Utils;

namespace Roguelike.Common.Mode.RoguelikeMode.RoguelikeChange.ItemOverhaul {
	//public readonly static int[] GunType = {
	//    ItemID.RedRyder,
	//    ItemID.Minishark,
	//    ItemID.Gatligator,
	//    ItemID.Handgun,
	//    ItemID.PhoenixBlaster,
	//    ItemID.Musket,
	//    ItemID.TheUndertaker,
	//    ItemID.FlintlockPistol,
	//    ItemID.Revolver,
	//    ItemID.ClockworkAssaultRifle,
	//    ItemID.Megashark,
	//    ItemID.Uzi,
	//    ItemID.VenusMagnum,
	//    ItemID.SniperRifle,
	//    ItemID.ChainGun,
	//    ItemID.SDMG,
	//    ItemID.Boomstick,
	//    ItemID.QuadBarrelShotgun,
	//    ItemID.Shotgun,
	//    ItemID.OnyxBlaster,
	//    ItemID.TacticalShotgun
	//};
	public class RangeWeaponOverhaul : GlobalItem {
		public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
			if (entity.damage > 0 && !entity.accessory) {
				return true;
			}
			else {
				return false;
			}
		}
		public override bool InstancePerEntity => true;
		public float OffSetPost = 0;
		public float SpreadAmount = 0;
		public float AdditionalSpread = 0;
		public float AdditionalMulti = 1;
		public int NumOfProjectile = 1;
		public int VariableBulletAmount = 0;
		public bool itemIsAShotgun = false;
		public override void SetDefaults(Item entity) {
		}
		/// <summary>
		/// Use this if your weapon have spread or not
		/// </summary>
		/// <param name="modplayer"></param>
		/// <param name="velocity"></param>
		/// <returns></returns>
		public Vector2 RoguelikeGunVelocity(Vector2 velocity) {
			return velocity
				.Vector2RotateByRandom(SpreadAmount)
				.Vector2RandomSpread(AdditionalSpread, AdditionalMulti);
		}
		public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if (item.ModItem == null) {
				return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
			}
			if (itemIsAShotgun && item.ModItem == null) {
				return true;
			}
			int amount = NumOfProjectile;
			if (VariableBulletAmount > 0) {
				amount += Main.rand.Next(VariableBulletAmount + 1);
			}
			if (amount >= 2) {
				amount--;
				for (int i = 0; i < amount; i++) {
					var velocity2 = RoguelikeGunVelocity(velocity);
					Projectile.NewProjectile(source, position, velocity2, type, damage, knockback, player.whoAmI);
				}
			}
			return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
		}
		public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (item.ModItem == null) {
				return;
			}
			position = position.PositionOFFSET(velocity, OffSetPost);
			if (!itemIsAShotgun) {
				velocity = RoguelikeGunVelocity(velocity);
			}
		}
	}
}
