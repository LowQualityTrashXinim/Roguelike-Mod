using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Roguelike.Common.Global;
internal class CommonRecipeGroup : ModSystem {
	public override void AddRecipeGroups() {
		var WoodSword = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} Wood sword", new int[]
		{
				ItemID.WoodenSword,
				ItemID.BorealWoodSword,
				ItemID.RichMahoganySword,
				ItemID.ShadewoodSword,
				ItemID.EbonwoodSword,
				ItemID.PalmWoodSword,
				ItemID.PearlwoodSword,
		});
		RecipeGroup.RegisterGroup("Wood Sword", WoodSword);

		var WoodBow = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} Wood bow", new int[]
		{
				ItemID.WoodenBow,
				ItemID.BorealWoodBow,
				ItemID.RichMahoganyBow,
				ItemID.ShadewoodBow,
				ItemID.EbonwoodBow,
				ItemID.PalmWoodBow,
				ItemID.PearlwoodBow,
		});
		RecipeGroup.RegisterGroup("Wood Bow", WoodBow);

		var OreShortSword = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} Ore short sword", new int[]
		{
				ItemID.CopperShortsword,
				ItemID.TinShortsword,
				ItemID.IronShortsword,
				ItemID.LeadShortsword,
				ItemID.SilverShortsword,
				ItemID.TungstenShortsword,
				ItemID.GoldShortsword,
				ItemID.PlatinumShortsword,
		});
		RecipeGroup.RegisterGroup("Ore shortsword", OreShortSword);

		var OreBroadSword = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} Ore broad sword", new int[]
		{
				ItemID.CopperBroadsword,
				ItemID.TinBroadsword,
				ItemID.IronBroadsword,
				ItemID.LeadBroadsword,
				ItemID.SilverBroadsword,
				ItemID.TungstenBroadsword,
				ItemID.GoldBroadsword,
				ItemID.PlatinumBroadsword,
		});
		RecipeGroup.RegisterGroup("Ore broadsword", OreBroadSword);

		var OreBow = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} Ore Bow", new int[]
		{
				ItemID.CopperBow,
				ItemID.TinBow,
				ItemID.IronBow,
				ItemID.LeadBow,
				ItemID.SilverBow,
				ItemID.TungstenBow,
				ItemID.GoldBow,
				ItemID.PlatinumBow,
		});
		RecipeGroup.RegisterGroup("Ore bow", OreBow);

		var GemStaff = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} Gem staff", new int[]
{
				ItemID.AmethystStaff,
				ItemID.TopazStaff,
				ItemID.SapphireStaff,
				ItemID.EmeraldStaff,
				ItemID.RubyStaff,
				ItemID.DiamondStaff,
});
		RecipeGroup.RegisterGroup("Gem staff", OreBow);
	}
}
