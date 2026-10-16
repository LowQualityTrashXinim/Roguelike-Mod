using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria;
using System;
using System.Linq;
using Roguelike.Common.Systems.ArtifactSystem;
using Roguelike.Common.Systems.Achievement;

using Roguelike.Texture;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Contents.Transfixion.Perks;

namespace Roguelike.Contents.Transfixion.Artifacts;
internal class TokenOfGluttonyArtifact : Artifact {
	public override string TexturePath => ModTexture.Get_MissingTexture("Artifact");
	public override Color DisplayNameColor => Color.LimeGreen;
}
public class TokenOfGluttonyPlayer : ModPlayer {
	bool TokenOfGluttony = false;
	public override void SetStaticDefaults() {
		DataStorer.AddContext("Artifact_ToG", new(300, Vector2.Zero, false, Color.Brown));
	}
	public override void ResetEffects() {
		TokenOfGluttony = Player.HasArtifact<TokenOfGluttonyArtifact>();
		if (TokenOfGluttony) {
			DataStorer.ActivateContext(Player, "Artifact_ToG");
		}
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (!TokenOfGluttony) {
			return;
		}
		Player.Heal(Main.rand.Next(1, 5));
	}
	public static bool IsWithinTheRadius(Vector2 player, Vector2 entity) => player.DistanceSQ(entity) <= 300;
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		if (!TokenOfGluttony) {
			return;
		}
		if (!IsWithinTheRadius(Player.Center, target.Center)) {
			modifiers.SourceDamage -= .45f;
		}
	}
	public override void UpdateEquips() {
		if (TokenOfGluttony) {
			Player.endurance += (1 - PercentageRatio) / 2f;
			Player.GetModPlayer<PlayerStatsHandle>().BuffTime -= .9f;
			if (Player.buffType.Where(b => b != 0 && !Main.buffNoTimeDisplay[b]).Any()) {
				Player.endurance += .1f;
			}
		}
	}
	public float PercentageRatio => Player.statLife / (float)Player.statLifeMax2;
	public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
		if (!TokenOfGluttony) {
			return;
		}
		if (Main.rand.NextBool(5)) {
			Player.Heal((int)modifiers.FinalDamage.ApplyTo(proj.damage * .5f));
			modifiers.SetMaxDamage(1);
		}
	}
}
