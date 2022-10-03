using System.Collections;
using LD51.Data;
using LD51.Data.World;
using LD51.Game.Ui;
using LD51.Menu.Ui;
using UnityEngine;

namespace LD51.Game {
	public class GameSceneController : MonoBehaviour {
		public void Start() {
			StartCoroutine(DoInitialize());
		}

		private IEnumerator DoInitialize() {
			while (!SpriteAtlasLibrary.loaded) yield return null;
			WorldRenderer.instance.Build();
			WorldObjectManager.Initialize();
			GameUi.Initialize();
			yield return StartCoroutine(LoadingScreenUi.FadeOut());
			AbstractStateBehaviour.ChangeState(OverviewGameState.state);
		}
	}
}