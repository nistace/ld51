using System.Collections.Generic;
using UnityEngine;
using Utils.Extensions;

namespace LD51.Data.World {
	public static class WorldObjectManager {
		private static HashSet<WorldObject> buildings { get; } = new HashSet<WorldObject>();

		public static void Initialize() {
			buildings.Clear();
			buildings.AddAll(Object.FindObjectsOfType<WorldObject>());
			buildings.ForEach(t => t.SetSelected(false));
		}

		public static void SetHoverWithMouseEnabled(bool enabled) {
			WorldObject.hoverWithMouse = enabled;
			if (!enabled) buildings.ForEach(t => t.SetHovered(false));
		}
	}
}