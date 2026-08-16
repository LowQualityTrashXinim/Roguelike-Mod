using Terraria;
using Terraria.ModLoader;
using Terraria.GameInput;
using Roguelike.Common.General;
using Roguelike.Common.Systems;
using Microsoft.Xna.Framework.Input;
using Roguelike.Contents.Transfixion.Skill;
using Roguelike.Contents.Items.Toggle.Transmutation;
using Roguelike.Contents.Transfixion.WeaponEnchantment;

namespace Roguelike.Common.Global;
public class ProcessTriggerSystem_Roguelike : ModSystem {
	public static ModKeybind SkillActivation { get; private set; }
	public static ModKeybind Open_SkillUI { get; private set; }
	public static ModKeybind Open_DivineUI { get; private set; }
	public static ModKeybind Open_TransmutateUI { get; private set; }
	public override void Load() {
		SkillActivation = KeybindLoader.RegisterKeybind(Mod, "Skill activation", Keys.F);
		Open_SkillUI = KeybindLoader.RegisterKeybind(Mod, "Open skill interface", Keys.J);
		Open_DivineUI = KeybindLoader.RegisterKeybind(Mod, "Open divine hammer interface", Keys.L);
		Open_TransmutateUI = KeybindLoader.RegisterKeybind(Mod, "Open transmutation interface", Keys.K);
	}
	public override void Unload() {
		SkillActivation = null;
		Open_SkillUI = null;
		Open_DivineUI = null;
		Open_TransmutateUI = null;
	}
}
internal class ProcessTriggerPlayer : ModPlayer {
	public bool Hold_Shift = false;
	public bool Press_Shift = false;
	public bool Pressed_Shift = false;
	public bool Shift_Option() {
		if (ModContent.GetInstance<RogueLikeConfig>().HoldShift) {
			return Hold_Shift;
		}
		else {
			return Press_Shift;
		}
	}
	public override void ProcessTriggers(TriggersSet triggersSet) {
		if (Main.playerInventory) {
			Hold_Shift = triggersSet.SmartSelect;
			if (triggersSet.SmartSelect) {
				if (!Pressed_Shift) {
					Press_Shift = !Press_Shift;
				}
				Pressed_Shift = true;
			}
			else {
				Pressed_Shift = false;
			}
		}
		UniversalSystem system = ModContent.GetInstance<UniversalSystem>();
		if (ProcessTriggerSystem_Roguelike.Open_DivineUI.JustReleased
			&& Player.HasItem(ModContent.ItemType<DivineHammer>())) {
			if (system.user2ndInterface.CurrentState == system.DivineHammer_uiState) {
				system.DeactivateUI();
			}
			else {
				system.ActivateEnchantmentUI();
			}
		}
		if (ProcessTriggerSystem_Roguelike.Open_SkillUI.JustReleased
		&& Player.HasItem(ModContent.ItemType<SkillOrb>())) {
			if (system.user2ndInterface.CurrentState == system.skillUIstate) {
				system.DeactivateUI();
			}
			else {
				system.ActivateSkillUI();
			}
		}
		if (ProcessTriggerSystem_Roguelike.Open_TransmutateUI.JustReleased
		&& Player.HasItem(ModContent.ItemType<TransmuteTablet>())) {
			if (system.user2ndInterface.CurrentState == system.transmutationUI) {
				system.DeactivateUI();
			}
			else {
				system.ActivateTransmutationUI();
			}
		}
	}
}
