using LD51.Data.Tensies;
using UnityEngine;

namespace LD51.Data.Tensies.Ui {
	public class TensieInfoUi : MonoBehaviour {
		[SerializeField] protected GameObject        _questionMarkObject;
		[SerializeField] protected TensieInventoryUi _inventory;
		[SerializeField] protected TensieProgressUi  _progress;

		public TensieProgressUi  progress  => _progress;
		public TensieInventoryUi inventory => _inventory;

		public void SetQuestionMarkVisible(bool visible) => _questionMarkObject.SetActive(visible);
	}
}