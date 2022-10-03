using System.Linq;
using LD51.Data.Misc;
using UnityEngine;
using UnityEngine.Events;

namespace LD51.Data.GameResources {
	public class GameInventory : MonoBehaviour {
		private static GameInventory instance { get; set; }

		[SerializeField] protected Inventory            _inventory = new Inventory();
		[SerializeField] protected GameResourceAmount[] _resourcesOnStart;
		[SerializeField] protected GameResource[]       _resourceOrder;
		[SerializeField] protected int                  _foodConsumption;

		public static UnityEvent onNoFoodLeft { get; } = new UnityEvent();

		public static int foodConsumption {
			get => instance._foodConsumption;
			set => instance._foodConsumption = value;
		}

		public static Inventory inventory {
			get => instance._inventory;
			set => instance._inventory = value;
		}

		public static GameResource[] resourceOrder => instance._resourceOrder;

		private void Awake() {
			instance = this;
			foreach (var resourceOnStart in _resourcesOnStart) _inventory.Add(resourceOnStart.resource, resourceOnStart.amount);
		}

		private static void ConsumeFood() {
			var remainingToTake = foodConsumption;
			foreach (var foodResource in instance._resourceOrder.Where(t => t.food)) {
				var amountToTake = Mathf.Min(inventory.Count(foodResource), remainingToTake);
				inventory.Take(foodResource, amountToTake);
				remainingToTake -= amountToTake;
			}
			if (remainingToTake > 0) onNoFoodLeft.Invoke();
		}

		private void Update() {
			if (GameTime.justStartedNewLoop) ConsumeFood();
		}
	}
}