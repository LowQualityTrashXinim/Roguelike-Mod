using Roguelike.Common.Global;
using Roguelike.Common.Systems.ReviveSystem;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.Consumable.Potion;

internal class TitanElixir : ModItem {
	public override void SetDefaults() {
		Item.BossRushDefaultPotion(20, 26, ModContent.BuffType<Protection>(), 12000);
		Item.rare = ItemRarityID.Orange;
		Item.value = Item.sellPrice(gold: 25);
	}
}
public class TitanElixir_Revive : Revive {
	public override bool IsActive(Player player) {
		return player.HasBuff(ModContent.BuffType<Protection>());
	}
	public override void OnRevive(Player player) {
		player.ClearBuff(ModContent.BuffType<Protection>());
		player.Heal(player.statLifeMax2);
		player.immune = true;
		player.AddImmuneTime(-1, 90);
	}
}
internal class Protection : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		Main.debuff[Type] = false;
		Main.buffNoSave[Type] = true;
	}

	public override void Update(Player player, ref int buffIndex) {
		player.endurance += 0.45f;
		player.statLifeMax2 += 400;
		player.statDefense += 45;

		player.GetDamage(DamageClass.Generic) -= 0.25f;

		player.moveSpeed *= .75f;
		player.maxRunSpeed = .75f;
		player.runAcceleration *= .75f;
		player.jumpSpeedBoost *= .75f;
		player.accRunSpeed *= .75f;
	}
}
