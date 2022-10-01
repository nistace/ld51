using LD51.Data;
using UnityEngine;
using UnityEngine.UI;

namespace LD51.Menu.Ui {
	public class GameTimeBarUi : MonoBehaviour {
		[SerializeField] protected Image _fillImage;

		public Color color {
			get => _fillImage.color;
			set => _fillImage.color = value;
		}

		private void Update() {
			_fillImage.fillAmount = GameTime.progressInCurrentLoop;
		}
	}
}