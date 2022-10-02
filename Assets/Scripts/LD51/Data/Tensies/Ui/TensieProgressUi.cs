using UnityEngine;
using UnityEngine.UI;

namespace LD51.Data.Tensies.Ui {
	public class TensieProgressUi : MonoBehaviour {
		[SerializeField] protected Image _background;
		[SerializeField] protected Image _transparent;
		[SerializeField] protected Image _fillImage;

		public void Show(Sprite icon, float progress) {
			_background.fillAmount = progress;
			_transparent.sprite = icon;
			_fillImage.sprite = icon;
			_fillImage.fillAmount = progress;
			gameObject.SetActive(true);
		}

		public void Hide() => gameObject.SetActive(false);
	}
}