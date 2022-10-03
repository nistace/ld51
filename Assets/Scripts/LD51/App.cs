using System.Collections;
using LD51.Data;
using LD51.Inputs;
using UnityEngine;
using UnityEngine.SceneManagement;

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
			GameInput.controls.App.Volume.Enable();
			GameInput.controls.App.VolumeDown.Enable();
		}

		private void Update() {
			if (GameInput.controls.App.Volume.inProgress) GameAudio.volume = Mathf.Clamp01(GameAudio.volume + Time.deltaTime);
			if (GameInput.controls.App.VolumeDown.inProgress) GameAudio.volume = Mathf.Clamp01(GameAudio.volume - Time.deltaTime);
		}

		private IEnumerator DoLoad() {
			SpriteAtlasLibrary.instance = _spriteAtlasLibrary;
			yield return StartCoroutine(_spriteAtlasLibrary.BuildAll());
			SceneManager.LoadSceneAsync("Menu");
		}
	}
}