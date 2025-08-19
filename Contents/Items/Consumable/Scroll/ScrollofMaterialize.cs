using Terraria;
using Terraria.ID;
using Roguelike.Texture;
using Terraria.ModLoader;
using Roguelike.Common.Utils;

namespace Roguelike.Contents.Items.Consumable.Scroll;
internal class ScrollOfMaterialize : ModItem {
	public override void SetStaticDefaults() {
		ModItemLib.LootboxPotion.Add(Item);
	}
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.BossRushDefaultPotion(32, 32, ModContent.BuffType<MaterializeSpell>(), ModUtils.ToMinute(4));
	}
}
public class MaterializeSpell : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
}
public class ScrollOfMaterializePlayer : ModPlayer {
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (Player.HasBuff<MaterializeSpell>()) {
			if (Main.rand.NextBool(15)) {
				Item.NewItem(Player.GetSource_OnHit(target), target.Hitbox, Main.rand.NextBool() ? ItemID.Heart : ItemID.Star);
			}
		}
	}
}

