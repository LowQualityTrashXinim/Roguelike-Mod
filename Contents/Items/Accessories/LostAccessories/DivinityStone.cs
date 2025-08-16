using System;
using Terraria;
using Terraria.ModLoader;
using Roguelike.Texture;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;

namespace Roguelike.Contents.Items.Accessories.LostAccessories;
internal class DivinityStone : ModItem {
	public override string Texture => ModTexture.Get_MissingTexture("LostAcc");
	public override void SetDefaults() {
		Item.Set_LostAccessory(32, 32);
		Item.Set_InfoItem(true);
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.CritChance, Base: 5);
		player.GetModPlayer<DivinityStonePlayer>().DivinityStone = true;
	}
}
public class DivinityStonePlayer : ModPlayer {
	public bool DivinityStone = false;
	public int Booster = 0;
	public int Booster_Decay = 0;
	public override void ResetEffects() {
		DivinityStone = false;
		if (Booster > 0) {
			if (--Booster_Decay <= 0) {
				Booster = 0;
				Booster_Decay = ModUtils.ToSecond(8);
			}
		}
	}
	public override void UpdateEquips() {
		if(DivinityStone) {
			PlayerStatsHandle modplayer = Player.GetModPlayer<PlayerStatsHandle>();
			modplayer.AddStatsToPlayer(PlayerStats.RegenHP, Additive: 1 + .1f * Booster, Flat: 2 * Booster);
			modplayer.AddStatsToPlayer(PlayerStats.RegenMana, Additive: 1 + .25f * Booster, Flat: 2 * Booster);
		}
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (DivinityStone) {
			if(hit.Crit) {
				Booster = Math.Clamp(Booster + 1, 0, 7);
				Booster_Decay = ModUtils.ToSecond(8);
			}
		}
	}
}
