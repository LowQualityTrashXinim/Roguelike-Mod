using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Roguelike.Common.Utils;
using Roguelike.Common.Global;

namespace Roguelike.Common.RoguelikeMode.ItemOverhaul.Foods;
internal class Roguelike_Coconut : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.Coconut;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(3);
	public override int LifeAmount() => 32;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(1.5f);
		SetBuff(item, ModContent.BuffType<Roguelike_Coconut_Buff>(), ModUtils.ToMinute(4));
	}
	public override void OnConsumeFood(Item item, Player player) {
		Player_FoodPlayer(player).SetFoodBuff(item.type, 0);
	}
}
public class Roguelike_Coconut_Buff : FoodItemTier1 {
	public override int TypeID => ItemID.Coconut;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.UpdateFullHPDamage += .2f;
		handler.UpdateDefenseBase.Base += 4;
		player.endurance += .07f;
	}
}
