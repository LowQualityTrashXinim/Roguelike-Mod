using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.RoguelikeMode.ItemOverhaul.Foods;
internal class Roguelike_Cherry : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.Cherry;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(6);
	public override int LifeAmount() => 20;
	public override int ManaAmount() => 40;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(1.25f);
		SetBuff(item, ModContent.BuffType<Roguelike_Cherry_Buff>(), ModUtils.ToMinute(1));
	}
	public override void OnConsumeFood(Item item, Player player) {
		Player_FoodPlayer(player).SetFoodBuff(item.type, 0);
	}
}
public class Roguelike_Cherry_Buff : FoodItemTier1 {
	public override int TypeID => ItemID.Cherry;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.UpdateThorn += 2;
		handler.UpdateThorn.Base += 3;
		handler.ThornAoE += 1;
		handler.ThornAoE.Base += 200;
		player.endurance += .1f;
	}
}
