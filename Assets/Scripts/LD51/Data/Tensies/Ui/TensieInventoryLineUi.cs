using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LD51.Data.Tensies.Ui {
	public class TensieInventoryLineUi : MonoBehaviour {
		[SerializeField] protected Image    _icon;
		[SerializeField] protected TMP_Text _amountText;

		public void Show(Sprite sprite, int amount) {
			_icon.sprite = sprite;
			_amountText.text = $"{amount}";
			gameObject.SetActive(true);
		}

		public void Hide() => gameObject.SetActive(false);
	}
}