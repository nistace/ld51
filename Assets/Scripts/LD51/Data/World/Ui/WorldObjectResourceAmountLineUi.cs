using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LD51.Data.Tensies {
	public class WorldObjectResourceAmountLineUi : MonoBehaviour {
		[SerializeField] protected Image    _icon;
		[SerializeField] protected TMP_Text _amountText;

		public void Show(Sprite sprite, int? amount) {
			_icon.sprite = sprite;
			_amountText.gameObject.SetActive(amount.HasValue);
			_amountText.text = $"{amount ?? 0}";
			gameObject.SetActive(true);
		}

		public void Hide() => gameObject.SetActive(false);
	}
}