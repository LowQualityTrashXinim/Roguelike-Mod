using Roguelike.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.RoguelikeMode;
internal class RoguelikeGlobalRecipe : ModSystem {
	public override void AddRecipes() {
		base.AddRecipes();
	}
	public override void PostAddRecipes() {
		foreach (var recipe in Main.recipe) {
			int type = recipe.createItem.type;
			if (//Torch
				type == ItemID.Torch
				|| type == ItemID.IceTorch
				|| type == ItemID.JungleTorch
				|| type == ItemID.DesertTorch
				|| type == ItemID.CorruptTorch
				|| type == ItemID.CrimsonTorch
				|| type == ItemID.DemonTorch
				|| type == ItemID.HallowedTorch
				|| type == ItemID.MushroomTorch
				//Campfire
				|| type == ItemID.Campfire
				|| type == ItemID.CorruptCampfire
				|| type == ItemID.CrimsonCampfire
				|| type == ItemID.DesertCampfire
				|| type == ItemID.FrozenCampfire
				|| type == ItemID.DesertCampfire
				|| type == ItemID.DemonCampfire
				|| type == ItemID.HallowedCampfire
				|| type == ItemID.JungleCampfire
				|| type == ItemID.MushroomCampfire
				//Arrow
				|| type == ItemID.WoodenArrow
				|| type == ItemID.BoneArrow
				|| type == ItemID.FlamingArrow
				|| type == ItemID.FrostburnArrow
				|| type == ItemID.CursedArrow
				|| type == ItemID.VenomArrow
				|| type == ItemID.IchorArrow
				|| type == ItemID.ChlorophyteArrow
				|| type == ItemID.HellfireArrow
				|| type == ItemID.JestersArrow
				) {
				continue;
			}
			recipe.AddCondition(new Condition(ModUtils.LocalizationText("Conditions", "RoguelikeModeCursed"), () => { return !ModContent.GetInstance<RogueLikeWorldGen>().RoguelikeWorld; }));
		}
	}
}
