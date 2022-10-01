using UnityEngine;

namespace LD51.Data.Misc {
	public static class GameTime {
		private const  float loopTime = 10;
		private static float time                  => Time.time;
		public static  float progressInCurrentLoop => (time % 10) / 10;
		private static int   currentLoop           => GetLoop(time);
		public static  bool  justStartedNewLoop    => currentLoop != GetLoop(time - Time.deltaTime);

		private static int GetLoop(float time) => Mathf.FloorToInt(time / loopTime);
	}
}