using Terraria.ID;

namespace Roguelike.Contents.Items.Consumable.Spawner
{
	public class PocketPortal : BaseSpawnerItem {
		public override int[] NPCtypeToSpawn => new int[] { NPCID.DD2Betsy };
		public override void SetSpawnerDefault(out int width, out int height) {
			height = 55;
			width = 53;
		}
	}
}
