namespace LD51.Data.Tensies {
	public interface ITensieInteractable {
		void ContinueInteraction(Tensie actor, ref float progress);
		void SetHovered(bool hovered);
		TensieAnimation GetTensieUpAnimation();
	}
}