using Roguelike.Common.Mode.RoguelikeMode.RoguelikeChange.ItemOverhaul;
using Roguelike.Common.Utils;
using Terraria.ID;

namespace Roguelike.Contents.Items.Weapon.UnfinishedItem;
internal class MasterSword : SynergyModItem {
	public override void SetDefaults() {
		Item.BossRushDefaultMeleeShootCustomProjectile(62, 62, 56, 7f, 27, 27, ItemUseStyleID.Swing, 1, 1, true);
		MeleeWeaponOverhaul global = Item.GetGlobalItem<MeleeWeaponOverhaul>();
		global.SwingType = BossRushUseStyle.Swipe;
		global.SwingStrength = 11f;
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.WoodenSword)
			.AddIngredient(ItemID.EnchantedSword)
			.Register();
	}
}
