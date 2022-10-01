using System.Collections;
using LD51.Data;
using LD51.Data.World;
using UnityEngine;

namespace LD51.Game {
	public class GameSceneController : MonoBehaviour {
		public void Start() {
			StartCoroutine(DoInitialize());
		}

		private static IEnumerator DoInitialize() {
			while (!SpriteAtlasLibrary.loaded) yield return null;
			WorldRenderer.instance.Build();
			WorldObjectManager.Initialize();
			AbstractStateBehaviour.ChangeState(OverviewGameState.state);
		}
	}
}