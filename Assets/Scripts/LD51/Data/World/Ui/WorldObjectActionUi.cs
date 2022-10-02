using UnityEngine;
using UnityEngine.UI;

namespace LD51.Data.Tensies {
	public class WorldObjectActionUi : MonoBehaviour {
		[SerializeField] protected Image _iconImage;

		public void Show(Sprite icon) {
			_iconImage.sprite = icon;
			gameObject.SetActive(icon);
		}

		public void Hide() => gameObject.SetActive(false);
	}
}