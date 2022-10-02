using UnityEngine;

namespace LD51.Data.Tensies {
	public interface ITensieInteractable {
		bool IsInteractionAllowedTo(Tensie actor);
		bool ContinueInteraction(Tensie actor, ref float progress);
		void SetHovered(bool hovered);
		TensieAnimation GetTensieUpAnimation();
		Sprite GetActionIcon();
	}
}