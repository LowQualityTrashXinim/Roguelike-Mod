using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Roguelike.Common.Utils;

namespace Roguelike.Contents.Transfixion.Perks.PerkContents;
public class TripletShot : Perk {
	public override void SetDefaults() {
		CanBeStack = false;
		list_category.Add(PerkCategory.None);
	}
	public override void UpdateEquip(Player player) {
		var item = player.HeldItem;
		if (item.useAmmo == AmmoID.Arrow) {
			var stathandler = player.ModPlayerStats();
			stathandler.Request_ShootSpreadExtra(3, 25);
			stathandler.AddStatsToPlayer(PlayerStats.AttackSpeed, 1 - .15f);
			player.GetDamage(DamageClass.Ranged).Base -= 4;
		}
	}
}
