using System.Linq;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace Roguelike.Contents.Items.Lootbox.Lootpool;
public class LootboxSystem : ModSystem {
	public static List<ItemPool> LootBoxDropPool { get; private set; } = new List<ItemPool>();
	/// <summary>
	/// Direct modify maybe unstable, unsure how this will work <br/>
	/// </summary>
	/// <param name="type"></param>
	/// <returns></returns>
	public static ItemPool GetItemPool(int type) => LootBoxDropPool.Where(i => i.PoolID == type).FirstOrDefault();
	public static ItemPool GetItemPool<T>() where T : ItemPool {
		foreach (var item in LootBoxDropPool) {
			if (item.GetType() is T) {
				return item;
			}
		}
		return null;
	}
	public static int Register(ItemPool pool) {
		ModTypeLookup<ItemPool>.Register(pool);
		LootBoxDropPool.Add(pool);
		return LootBoxDropPool.Count - 1;
	}
	public override void Unload() {
		LootBoxDropPool = null;
	}
}
/// <summary>
/// This class serves as a common loot that can be found in lootbox that met the condition of the loot<br/>
/// It provide a easy and a lot faster way to register or runtime loot assignment.
/// </summary>
public abstract class ItemPool : ModType {
	public static int GetPoolType<T>() where T : ItemPool => ModContent.GetInstance<T>().Type;
	public ItemPool() {
	}
	protected sealed override void Register() {
		Type = LootboxSystem.Register(this);
	}
	protected int _poolid = 0;
	/// <summary>
	/// Return the Item ID that own this <see cref="ItemPool"/>
	/// </summary>
	public int PoolID { get => _poolid; }
	/// <summary>
	/// Return the index of <see cref="ItemPool"/> that current at
	/// </summary>
	public int Type = -1;
	public virtual HashSet<int> MeleeLoot() => new();
	public virtual HashSet<int> RangeLoot() => new();
	public virtual HashSet<int> MagicLoot() => new();
	public virtual HashSet<int> SummonLoot() => new();

	private HashSet<int> _cachedAllItems = null;
	/// <summary>
	/// Call this when you know it will get update 
	/// </summary>
	public void UpdateAllItemPool() {
		_cachedAllItems = new HashSet<int>();
		_cachedAllItems.UnionWith(MeleeLoot());
		_cachedAllItems.UnionWith(RangeLoot());
		_cachedAllItems.UnionWith(MagicLoot());
		_cachedAllItems.UnionWith(SummonLoot());
	}
	public HashSet<int> AllItemPool() {
		if (_cachedAllItems == null) {
			UpdateAllItemPool();
		}
		return _cachedAllItems;
	}
}
