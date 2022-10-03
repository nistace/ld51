using LD51.Data.Tutorial;
using LD51.Menu.Ui;
using UnityEngine;

namespace LD51.Menu {
	public class MenuSceneController : MonoBehaviour {
		[SerializeField] protected int _audioLevel = 2;

		private void Start() {
			GameAudio.desiredLevel = _audioLevel;
			StartCoroutine(LoadingScreenUi.FadeOut());
			GameScript.Restart();
		}
	}
}