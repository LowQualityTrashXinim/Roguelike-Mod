using Terraria;
using Terraria.ModLoader;
using Roguelike.Contents.Items.Weapon;
 
using Roguelike.Texture;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;

namespace Roguelike.Contents.Items.Accessories.LostAccessories
{
	class EnergeticOrb : ModItem {
		public override string Texture => ModTexture.Get_MissingTexture("LostAcc");
		public override void SetDefaults() {
			Item.DefaultToAccessory(28, 30);
			Item.GetGlobalItem<GlobalItemHandle>().LostAccessories = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.statManaMax2 += 30;
			player.manaRegen += 15;
			player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.MagicDMG, 1.06f);
			player.GetModPlayer<EnergeticOrbPlayer>().EnergeticOrb = true;
		}
	}
	public class EnergeticOrbPlayer : ModPlayer {
		public bool EnergeticOrb = false;
		public override void ResetEffects() {
			EnergeticOrb = false;
		}
		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			if (EnergeticOrb && proj.DamageType == DamageClass.Magic) {
				int value = Main.rand.Next(3, 10);
				Player.statMana += value;
				if (Player.statMana > Player.statManaMax2) Player.statMana = Player.statManaMax2;
				Player.ManaEffect(value);
			}
		}
	}
}
