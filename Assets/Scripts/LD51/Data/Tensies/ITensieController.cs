using UnityEngine;

namespace LD51.Data.Tensies {
	public interface ITensieController {
		TensieActionData.KeyFrame GetKeyFrame();
		void RemoveComponent();
	}
}