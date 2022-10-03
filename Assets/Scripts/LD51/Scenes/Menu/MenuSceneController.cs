using System;
using LD51.Data.Tutorial;
using LD51.Inputs;
using LD51.Menu.Ui;
using UnityEngine;
using Utils.Extensions;

namespace LD51.Menu {
	public class MenuSceneController : MonoBehaviour {
		[SerializeField] protected int _audioLevel = 2;

		private void Start() {
			GameAudio.desiredLevel = _audioLevel;
			StartCoroutine(LoadingScreenUi.FadeOut());
			GameInput.controls.Menu.Quit.AddAnyListenerOnce(_ => Application.Quit());
			GameScript.Restart();
		}

		private void OnEnable() {
			GameInput.controls.Menu.Quit.Enable();
		}

		private void OnDisable() {
			GameInput.controls.Menu.Quit.Disable();
		}
	}
}