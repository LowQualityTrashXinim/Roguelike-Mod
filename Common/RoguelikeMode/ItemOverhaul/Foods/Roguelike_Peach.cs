using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Roguelike.Common.Utils;
using Roguelike.Contents.Transfixion.Skill;

namespace Roguelike.Common.RoguelikeMode.ItemOverhaul.Foods;
internal class Roguelike_Peach : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.Peach;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(4);
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(2);
		SetBuff(item, ModContent.BuffType<Roguelike_Peach_Buff>(), ModUtils.ToMinute(2.5f));
	}
	public override void OnConsumeItem(Item item, Player player) {
		Player_FoodPlayer(player).SetFoodBuff(item.type, 0);
		Player_SkillPlayer(player).Modify_EnergyAmount(57);
	}
}
public class Roguelike_Peach_Buff : FoodItemTier1 {
	public override int TypeID => ItemID.Peach;
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<SkillHandlePlayer>().skilldamage += .23f;
		player.ModPlayerStats().EnergyRecharge += .14f;
	}
}
