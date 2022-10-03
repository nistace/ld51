using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils.Extensions;

namespace LD51.Menu.Ui {
	public class LoadingScreenUi : MonoBehaviour {
		private static LoadingScreenUi instance { get; set; }

		[SerializeField] protected Image _black;
		[SerializeField] protected float _fadeSpeed   = 1;
		[SerializeField] protected bool  _initiallyIn = true;

		private void Start() {
			if (!instance) {
				instance = this;
				_black.color = _initiallyIn ? Color.black : Color.clear;
			}
		}

		public static IEnumerator FadeIn(UnityAction onDone = null) {
			instance._black.enabled = true;
			for (var lerp = 0f; lerp < 1; lerp += instance._fadeSpeed * Time.deltaTime) {
				instance._black.color = Color.black.With(a: lerp);
				yield return null;
			}
			instance._black.color = Color.black;
			onDone?.Invoke();
		}

		public static IEnumerator FadeOut(UnityAction onDone = null) {
			for (var lerp = 1f; lerp > 0; lerp -= instance._fadeSpeed * Time.deltaTime) {
				instance._black.color = Color.black.With(a: lerp);
				yield return null;
			}
			instance._black.color = Color.clear;
			instance._black.enabled = false;
			onDone?.Invoke();
		}
	}
}