using Roguelike.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.aDebugItem
{
	internal class DayTimeCycle : ModItem {
		public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.FastClock);
		public override void SetStaticDefaults() {
			ItemID.Sets.ItemIconPulse[Item.type] = true;
		}
		public override void SetDefaults() {
			Item.BossRushSetDefault(20, 20, 0, 0, 1, 1, ItemUseStyleID.HoldUp, false);
			Item.Set_DebugItem(true);
		}
		public override bool? UseItem(Player player) {
			Main.time = Main.dayTime ? Main.dayLength : Main.nightLength;
			return false;
		}
	}
}
