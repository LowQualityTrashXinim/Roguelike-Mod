using Roguelike.Common.Global;
using Terraria;
using Terraria.ID;

namespace Roguelike.Contents.Items.Consumable.Spawner {
	internal class LunaticTablet : BaseSpawnerItem {
		public override int[] NPCtypeToSpawn => new int[] { NPCID.CultistBoss };
		public override bool UseSpecialSpawningMethod => true;
		public override void SpecialSpawningLogic(Player player) {
			int spawnY = 250;
			NPC MainNPC = NPC.NewNPCDirect(player.GetSource_ItemUse(Item), (int)player.Center.X, (int)(player.Center.Y - spawnY), NPCtypeToSpawn[0]);
		}
	}
}
