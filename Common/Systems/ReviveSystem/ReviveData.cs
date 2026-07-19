namespace Roguelike.Common.Systems.ReviveSystem;

public class ReviveData {
	/// <summary>
	/// Whether this revive has been activated on the player.
	/// This value is reset during player ResetEffects and
	/// needs to be set between ResetEffects and PreKill.
	/// </summary>
	public bool Active;

	/// <summary>
	/// The result of the chance calculation for this revive.
	/// </summary>
	public bool Chance;

	/// <summary>
	/// Whether this revive has already been used by
	/// the player.
	/// </summary>
	public bool Used;

	/// <summary>
	/// Clones this data object. Use memberwise clone
	/// for all the value types.
	/// </summary>
	public ReviveData Clone() {
		return (ReviveData)MemberwiseClone();
	}
}
