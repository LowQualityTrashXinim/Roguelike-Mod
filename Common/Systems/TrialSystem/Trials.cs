using Microsoft.Xna.Framework;
using Roguelike.Contents.Items.Lootbox.MiscLootbox;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.Systems.TrialSystem;

public class TestTrial : ModTrial {
	public override void TrialReward(IEntitySource source, Player player) {
		player.QuickSpawnItem(source, ModContent.ItemType<WeaponLootBox>());
	}
}
