
using Roguelike.Common.RoguelikeMode;
using Roguelike.Common.Utils;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.NoneSynergy.FailWeapon;
internal class FailGoldBroadsword : ModItem {
	public override void SetDefaults() {
		Item.BossRushSetDefault(56, 56, 25, 4, 20, 20, ItemUseStyleID.Swing, true);
		Item.DamageType = DamageClass.Melee;
		if (Item.TryGetGlobalItem(out MeleeWeaponOverhaul global)) {
			global.SwingType = BossRushUseStyle.Swipe;
		}
	}
}
internal class FailPlatinumSword : ModItem {
	public override void SetDefaults() {
		Item.BossRushSetDefault(48, 48, 27, 4, 20, 20, ItemUseStyleID.Swing, true);
		Item.DamageType = DamageClass.Melee;
		if (Item.TryGetGlobalItem(out MeleeWeaponOverhaul global)) {
			global.SwingType = BossRushUseStyle.Swipe;
		}
	}
}
internal class FailMuramasa : ModItem {
	public override void SetDefaults() {
		Item.BossRushSetDefault(61, 64, 25, 4, 12, 12, ItemUseStyleID.Swing, true);
		Item.DamageType = DamageClass.Melee;
		if (Item.TryGetGlobalItem(out MeleeWeaponOverhaul global)) {
			global.SwingType = BossRushUseStyle.Swipe;
		}
	}
}
