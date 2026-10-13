using Roguelike.Common.Utils;
using Roguelike.Texture;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.Consumable.Potion.UnnamedPotion;
public abstract class UnnamedPotionBase : ModItem {
	public override void SetStaticDefaults() {
		ModItemLib.LootboxPotion.Add(Item);
	}
	public override string Texture => ModTexture.MISSINGTEXTUREPOTION;
	public virtual int ModBuffType => 0;
	public override void SetDefaults() {
		Item.BossRushDefaultPotion(32, 32, ModBuffType, ModUtils.ToMinute(10));
	}
}
public abstract class UnnamedBuff : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void Update(Player player, ref int buffIndex) {
		UpdatePlayer(player);
		if (player.buffTime[buffIndex] == 0) {
			OnEnded(player);
		}
	}
	public virtual void UpdatePlayer(Player player) { }
	public virtual void OnEnded(Player player) { }
}
