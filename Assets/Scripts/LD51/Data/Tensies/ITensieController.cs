using UnityEngine;

namespace LD51.Data.Tensies {
	public interface ITensieController {
		public bool IsAliveEvenWhenNoActionPerformed();
		TensieActionData.KeyFrame GetKeyFrame();
		void RemoveComponent();
	}
}