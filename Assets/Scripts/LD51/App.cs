using System;
using System.Collections;
using LD51.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Extensions;

namespace LD51 {
	public class App : MonoBehaviour {
		private static App instance { get; set; }

		[SerializeField] protected SpriteAtlasLibrary _spriteAtlasLibrary;

		private void Awake() {
			if (instance) {
				Destroy(gameObject);
			}
			else {
				instance = this;
				DontDestroyOnLoad(gameObject);
			}
		}

		private void Start() {
			StartCoroutine(DoLoad());
		}

		private IEnumerator DoLoad() {
			SpriteAtlasLibrary.instance = _spriteAtlasLibrary;
			yield return StartCoroutine(_spriteAtlasLibrary.BuildAll());
			SceneManager.LoadSceneAsync("Menu");
		}
	}
}