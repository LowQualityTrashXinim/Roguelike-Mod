 
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.Weapon;
using Roguelike.Texture;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.Accessories.LostAccessories;
internal class StimPack : ModItem{
	public override string Texture => ModTexture.Get_MissingTexture("LostAcc");
	public override void SetDefaults() {
		Item.DefaultToAccessory(32, 32);
		Item.GetGlobalItem<GlobalItemHandle>().LostAccessories = true;
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<StimPackPlayer>().StimPack = true;
	}
}
public class StimPackPlayer : ModPlayer {
	public bool StimPack = false;
	public override void ResetEffects() {
		StimPack = false;
	}
	public override void UpdateEquips() {
		if(!Player.IsHealthAbovePercentage(.4f) && StimPack) {
			Player.AddBuff(ModContent.BuffType<StimPackBuff>(), ModUtils.ToSecond(10));
		}
	}
}
class StimPackBuff : ModBuff {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetStaticDefaults() {
		Main.debuff[Type] = false;
	}
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.RegenHP, 1.5f, Flat: 10);
		modplayer.AddStatsToPlayer(PlayerStats.AttackSpeed, 1.12f);
	}
}
