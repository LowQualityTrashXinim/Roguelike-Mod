using Roguelike.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.RoguelikeMode.ItemOverhaul.Foods;
internal class Roguelike_ElderBerry : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.Elderberry;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(3);
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = 60;
		SetBuff(item, ModContent.BuffType<Roguelike_ElderBerry_Buff>(), ModUtils.ToMinute(8));
	}
	public override void OnConsumeItem(Item item, Player player) {
		player.ManaHeal(21);
		Player_FoodPlayer(player).SetFoodBuff(item.type, 0);
	}
}
public class Roguelike_ElderBerry_Buff : FoodItemTier1 {
	public override int TypeID => ItemID.Elderberry;
	public override void Update(Player player, ref int buffIndex) {
		player.GetDamage(DamageClass.Ranged) += .09f;
		player.ModPlayerStats().Range_CritDamage += .21f;
		player.GetModPlayer<Roguelike_ElderBerry_ModPlayer>().ElderBerry = true;
	}
}
public class Roguelike_ElderBerry_ModPlayer : ModPlayer {
	public bool ElderBerry = false;
	public override void ResetEffects() {
		ElderBerry = false;
	}
	public override bool CanConsumeAmmo(Item weapon, Item ammo) {
		if (ElderBerry) {
			return Main.rand.NextFloat() <= .17f;
		}
		return base.CanConsumeAmmo(weapon, ammo);
	}
}
