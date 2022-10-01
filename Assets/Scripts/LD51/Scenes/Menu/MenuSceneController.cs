using System.Collections;
using LD51.Data;
using LD51.Menu.Ui;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Extensions;

namespace LD51.Menu {
	public class MenuSceneController : MonoBehaviour {
		private void Start() {
			MenuUi.barUi.color = Color.white;
			MenuUi.onStartClicked.AddListenerOnce(StartGame);
		}

		private void StartGame() => StartCoroutine(LoadGameScene());

		private static IEnumerator LoadGameScene() {
			MenuUi.barUi.color = Color.green;
			while (GameTime.progressInCurrentLoop > .01f) yield return null;
			yield return SceneManager.LoadSceneAsync("Game");
		}
	}
}