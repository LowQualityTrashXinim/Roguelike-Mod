using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.RoguelikeMode.ItemOverhaul.Foods;
internal class Roguelike_Marshmallow : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.Marshmallow;
	public override int CoolDownBetweenUse() => 60;
	public override int EnergyAmount() => 45;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(.75f);
		SetBuff(item, ModContent.BuffType<Roguelike_Marshmallow_ModBuff>(), ModUtils.ToMinute(2));
	}
	public override void OnConsumeFood(Item item, Player player) {
		Player_FoodPlayer(player).SetFoodBuff(item.type, 0);
	}
}
public class Roguelike_Marshmallow_ModBuff : FoodItemTier1 {
	public override int TypeID => ItemID.Marshmallow;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.UpdateHPRegen *= 1.17f;
		handler.UpdateManaRegen *= 1.21f;
		handler.BuffTime *= .87f;
		handler.DebuffBuffTime *= 1.35f;
	}
}
