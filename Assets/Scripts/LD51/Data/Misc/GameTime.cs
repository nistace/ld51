using UnityEngine;

namespace LD51.Data.Misc {
	public static class GameTime {
		private const  float loopTime       = 10;
		public const   float animationSpeed = 3f;
		private static float time                  => Time.time;
		public static  float progressInCurrentLoop => (time % 10) / 10;
		private static int   currentLoop           => GetLoop(time);
		public static  bool  justStartedNewLoop    => currentLoop != GetLoop(time - Time.deltaTime);
		public static  int   animationFrame        => Mathf.FloorToInt(Time.time * animationSpeed);

		private static int GetLoop(float time) => Mathf.FloorToInt(time / loopTime);
	}
}