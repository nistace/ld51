using System.Collections;
using UnityEngine;
using Utils.Extensions;

namespace Utils.Coroutines {
	public class CoroutineRunner : MonoBehaviour {
		private static CoroutineRunner mInstance { get; set; }

		private static CoroutineRunner instance => mInstance ? mInstance : mInstance = new GameObject("CoroutineRunner").GetOrAddComponent<CoroutineRunner>();

		public static Coroutine Run(IEnumerator routine) => instance.StartCoroutine(routine);
		public static void Stop(Coroutine routine) => instance.StopCoroutine(routine);
	}
}