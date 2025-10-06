using Roguelike.Common.Mode.RoguelikeMode.RoguelikeChange.ItemOverhaul;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;

namespace Roguelike.Contents.Items.Weapon.MeleeSynergyWeapon.RuleOfNature;
public class RuleOfNature : SynergyModItem {
	public override string Texture => ModTexture.Get_MissingTexture("Synergy");
	public override void SetDefaults() {
		Item.BossRushDefaultMeleeShootCustomProjectile(32, 32, 30, 9f, 21, 21, ItemUseStyleID.Swing, 1, 1, true);
		if (Item.TryGetGlobalItem(out MeleeWeaponOverhaul melee)) {
			melee.SwingType = BossRushUseStyle.Swipe;
		}
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.WoodenSword)
			.AddIngredient(ItemID.BorealWoodSword)
			.AddIngredient(ItemID.RichMahoganySword)
			.AddIngredient(ItemID.EbonwoodSword)
			.AddIngredient(ItemID.ShadewoodSword)
			.AddIngredient(ItemID.PalmWoodSword)
			.Register();
	}
}
