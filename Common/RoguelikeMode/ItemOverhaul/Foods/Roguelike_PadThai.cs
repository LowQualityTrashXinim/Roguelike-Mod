using Roguelike.Common.Global;
using Roguelike.Common.Systems.Skill;
using Roguelike.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.RoguelikeMode.ItemOverhaul.Foods;
internal class Roguelike_PadThai : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.PadThai;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(3.5f);
	public override int EnergyAmount() => 485;
	public override byte Tier() => 1;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(4.5f);
		SetBuff(item, ModContent.BuffType<Roguelike_PadThai_ModBuff>(), ModUtils.ToMinute(21));
	}
}
public class Roguelike_PadThai_ModBuff : FoodItemTier2 {
	public override int TypeID => ItemID.PadThai;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		player.GetModPlayer<SkillHandlePlayer>().EnergyCapacity.Base += 420;
		player.GetModPlayer<SkillHandlePlayer>().EnergyRecharge += .25f;
		handler.SkillDuration += .34f;
		player.GetModPlayer<SkillHandlePlayer>().EnergyRegen += .32f;
	}
}
