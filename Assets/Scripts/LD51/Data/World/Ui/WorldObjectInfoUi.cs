using LD51.Data.Tensies;
using UnityEngine;

namespace LD51.Data.World.Ui {
	public class WorldObjectInfoUi : MonoBehaviour {
		[SerializeField] protected Canvas                      _canvas;
		[SerializeField] protected WorldObjectActionUi         _actionImage;
		[SerializeField] protected WorldObjectResourceAmountUi _resourceSet;

		private Canvas                      canvas      => _canvas ? _canvas : _canvas = GetComponent<Canvas>();
		public  WorldObjectActionUi         actionImage => _actionImage;
		public  WorldObjectResourceAmountUi resourceSet => _resourceSet;

		public void SetVisible(bool visible) => canvas.enabled = visible;
	}
}