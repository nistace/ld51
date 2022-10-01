using UnityEngine;

namespace LD51.Data {
	public static class GameTime {
		private const float loopTime = 10;
		public static float time                => Time.time;
		public static float progressInCurrentLoop => (time % 10) / 10;
		public static int   currentLoop         => GetLoop(time);
		public static bool  justStartedNewLoop  => currentLoop != GetLoop(time - Time.deltaTime);

		private static int GetLoop(float time) => Mathf.FloorToInt(time / loopTime);
	}
}