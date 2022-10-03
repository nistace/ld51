using UnityEngine;

namespace LD51.Data.Misc {
	public static class GameTime {
		private const  float loopTime       = 10;
		public const   float animationSpeed = 3f;
		public static  float time                  => Application.isPlaying ? Time.time : 0;
		public static  float progressInCurrentLoop => timeInCurrentLoop / loopTime;
		public static  float timeInCurrentLoop     => time % loopTime;
		private static int   currentLoop           => GetLoop(time);
		public static  bool  justStartedNewLoop    => currentLoop != GetLoop(time - Time.deltaTime);
		public static  int   animationFrame        => Mathf.FloorToInt(time * animationSpeed);

		private static int GetLoop(float time) => Mathf.FloorToInt(time / loopTime);
	}
}