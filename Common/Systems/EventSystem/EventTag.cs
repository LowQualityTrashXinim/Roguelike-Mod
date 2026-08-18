using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Common.Systems.EventSystem;
/// <summary>
/// Use this class in concurrence with <see cref="ModSystem"/> class
/// </summary>
public abstract class Event : ModType {
	public virtual bool IsActive() {
		return false;
	}
	/// <summary>
	/// This is for conditioning the event whenever if it can be triggered<br/>
	/// It is note that this is not always checking trigger as this code only run when <see cref="IsActive"/> return true<br/>
	/// </summary>
	/// <param name="player"></param>
	/// <returns>
	/// True to run <see cref="OnTrigger(Player)"/><br/>
	/// False to run <see cref="OnFailTrigger(Player)"/>
	/// </returns>
	public virtual bool CanTrigger(Player player) {
		return false;
	}
	/// <summary>
	/// What happen when <see cref="CanTrigger(Player)"/> return true
	/// </summary>
	/// <param name="player"></param>
	public virtual void OnTrigger(Player player) {

	}
	/// <summary>
	/// What happen when <see cref="CanTrigger(Player)"/> return false
	/// </summary>
	/// <param name="player"></param>
	public virtual void OnFailTrigger(Player player	) {

	}
}
