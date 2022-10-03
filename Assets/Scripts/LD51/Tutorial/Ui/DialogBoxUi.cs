using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;

namespace LD51.Data.Tutorial.Ui {
	public class DialogBoxUi : MonoBehaviour {
		[SerializeField] protected Image[]  _portraitImages;
		[SerializeField] protected TMP_Text _nameText;
		[SerializeField] protected TMP_Text _scenarioText;
		[SerializeField] protected Button   _continueButton;
		[SerializeField] protected TMP_Text _continueButtonText;

		private void Start() {
			HandleNewLoop(GameScript.instance);
			GameScript.onNewLoopHandled.AddListenerOnce(HandleNewLoop);
			GameScript.onScriptPartUnlocked.AddListenerOnce(HandleScriptPartUnlocked);
			_continueButton.onClick.AddListenerOnce(HandleContinueButtonClicked);

			if (GameScript.instance.currentPartUnlocked) {
				HandleScriptPartUnlocked(GameScript.instance);
			}
			else {
				gameObject.SetActive(false);
			}
		}

		private void HandleContinueButtonClicked() {
			if (!GameScript.instance.currentPartUnlocked) return;
			if (!GameScript.instance.ContinueScript()) {
				gameObject.SetActive(false);
			}
		}

		private void HandleScriptPartUnlocked(GameScript script) {
			if (!script.TryGetCurrentScriptPart(out var scriptPart)) return;
			HandleNewLoop(script);
			gameObject.SetActive(true);
			_scenarioText.text = scriptPart.text;
			_continueButtonText.text = scriptPart.buttonText;
		}

		private void HandleNewLoop(GameScript script) {
			_portraitImages[0].sprite = SpriteAtlasLibrary.portraits["Base"][""][0];
			for (var i = 1; i < _portraitImages.Length; ++i) {
				_portraitImages[i].enabled = script.portraitProps.Count > i - 1;
				_portraitImages[i].sprite = script.portraitProps.Count > i - 1 ? SpriteAtlasLibrary.portraits[script.portraitProps[i - 1]][""][0] : null;
			}
			_nameText.text = $"{script.mayorName}, son of {script.fatherName}, grandson of {script.grandfatherName} - Mayor of Tensec";
		}
	}
}