using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LD51.Menu.Ui {
	public class MenuUi : MonoBehaviour {
		private static MenuUi instance { get; set; }

		[SerializeField] protected GameTimeBarUi _barUi;
		[SerializeField] protected Button          _startButton;

		public static GameTimeBarUi barUi => instance._barUi;

		public static UnityEvent onStartClicked => instance._startButton.onClick;

		private void Awake() {
			instance = this;
		}
	}
}