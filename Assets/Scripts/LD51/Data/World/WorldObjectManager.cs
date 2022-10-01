using System.Collections.Generic;
using System.Linq;
using LD51.Data.GameResources;
using UnityEngine;
using Utils.Extensions;

namespace LD51.Data.World {
	public static class WorldObjectManager {
		private static HashSet<WorldObject> buildings { get; } = new HashSet<WorldObject>();

		public static void Initialize() {
			buildings.Clear();
			buildings.AddAll(Object.FindObjectsOfType<WorldObject>());
			buildings.ForEach(t => t.SetSelected(false));
			GameInventory.foodConsumption = buildings.SelectNotNullComponents<TensieSpawnerModule>().Count();
		}

		public static void SetHoverWithMouseEnabled(bool enabled) {
			WorldObject.hoverWithMouse = enabled;
			if (!enabled) buildings.ForEach(t => t.SetHovered(false));
		}
	}
}