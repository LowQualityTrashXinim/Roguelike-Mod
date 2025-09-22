using Roguelike.Common.Mode.RoguelikeMode.RoguelikeChange.ItemOverhaul;
using Roguelike.Common.Utils;
using Terraria.ID;

namespace Roguelike.Contents.Items.Weapon.UnfinishedItem;

class DrainingVeilBlade : SynergyModItem {
	public override void SetDefaults() {
		Item.BossRushDefaultMeleeShootCustomProjectile(80, 83, 100, 1, 50, 50, ItemUseStyleID.Swing, 1, 1, false);
		if (Item.TryGetGlobalItem(out MeleeWeaponOverhaul melee)) {
			melee.SwingType = BossRushUseStyle.SwipeDown;
		}
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.SoulDrain)
			.AddIngredient(ItemID.ClingerStaff)
			.AddRecipeGroup("Wood Sword")
			.Register();
	}
}
