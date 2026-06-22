using JetBrains.Annotations;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.Consumable;
internal class WeaponEssence : ModItem {
	public override string Texture => ModTexture.WHITEBALL;
	public override void SetDefaults() {
		Item.BossRushDefaultPotion(32, 32, ModContent.BuffType<PowerEssence>(), ModUtils.ToSecond(30));
	}
	public override bool? UseItem(Player player) {
		if (player.ItemAnimationJustStarted) {
			player.GetModPlayer<WeaponEssence_ModPlayer>().EssencePower += Item.damage;
		}
		return base.UseItem(player);
	}
}
public class PowerEssence : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<WeaponEssence_ModPlayer>().PowerEssence = true;
	}
}
public class WeaponEssence_ModPlayer : ModPlayer {
	public bool PowerEssence = false;
	public int EssencePower = 0;
	public int EssenceCounter = 0;
	public override void ResetEffects() {
		PowerEssence = false;
		if (EssencePower > 0) {
			if (++EssenceCounter >= 60) {
				EssenceCounter = 0;
				EssencePower--;
			}
		}
	}
}
