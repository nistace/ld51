using System.Collections;
using LD51.Data;
using UnityEngine;

namespace LD51 {
	public class App : MonoBehaviour {
		[SerializeField] protected SpriteAtlasLibrary _spriteAtlasLibrary;

		private void Start() {
			StartCoroutine(DoLoad());
		}

		private IEnumerator DoLoad() {
			SpriteAtlasLibrary.instance = _spriteAtlasLibrary;
			yield return StartCoroutine(_spriteAtlasLibrary.BuildAll());
		}
	}
}