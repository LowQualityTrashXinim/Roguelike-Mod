using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Roguelike.Common.Systems;

using Roguelike.Contents.Items.Weapon;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Common.Global.Mechanic.OutroEffect;

namespace Roguelike.Contents.Items {
	internal class SynergyEnergy : ModItem {
		public override void SetDefaults() {
			Item.rare = ItemRarityID.Red;
			Item.width = 54;
			Item.height = 20;
			Item.material = true;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetModPlayer<SynergyModPlayer>().acc_SynergyEnergy = true;
			PlayerStatsHandle handle = player.GetModPlayer<PlayerStatsHandle>();
			handle.AddStatsToPlayer(PlayerStats.PureDamage, Multiplicative: 1.01f);
			handle.AddStatsToPlayer(PlayerStats.Defense, Multiplicative: 1.01f);
			handle.AddStatsToPlayer(PlayerStats.MovementSpeed, Multiplicative: 1.01f);
			handle.AddStatsToPlayer(PlayerStats.JumpBoost, Multiplicative: 1.01f);
			handle.Iframe += 1.1f;
			handle.BuffTime *= 1.01f;
			handle.DebuffTime *= 1.01f;
			handle.DebuffBuffTime *= .99f;
		}
	}
	public class SynergyModPlayer : ModPlayer {
		public bool acc_SynergyEnergy = false;
		public override void ResetEffects() {
			acc_SynergyEnergy = false;
		}
		public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
			if (acc_SynergyEnergy) {
				damage.Base += 5;
			}
		}
	}
}
