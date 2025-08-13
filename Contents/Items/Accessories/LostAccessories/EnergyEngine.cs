using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Contents.Skill;
using Roguelike.Texture;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.Accessories.LostAccessories;
internal class EnergyEngine : ModItem {
	public override string Texture => ModTexture.Get_MissingTexture("LostAcc");
	public override void SetDefaults() {
		Item.Set_LostAccessory(32, 32);
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<EnergyEngine_ModPlayer>().LostAcc1 = true;
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.EnergyCap, Base: -100);
		modplayer.AddStatsToPlayer(PlayerStats.EnergyRecharge, 1.11f);
		modplayer.EnergyRegen.Base += 1;
	}
}
public class EnergyEngine_ModPlayer : ModPlayer {
	public bool LostAcc1 = false;
	public override void ResetEffects() {
		LostAcc1 = false;
	}
	public override void UpdateEquips() {
		if (LostAcc1) {
			SkillHandlePlayer modplayer = Player.GetModPlayer<SkillHandlePlayer>();
			if (modplayer.Activate) {
				PlayerStatsHandle statplayer = Player.GetModPlayer<PlayerStatsHandle>();
				statplayer.AddStatsToPlayer(PlayerStats.PureDamage, 1.22f);
				statplayer.AddStatsToPlayer(PlayerStats.Defense, Base: 12);
			}
		}
	}
}
