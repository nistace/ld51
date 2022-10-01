using LD51.Data;
using LD51.Data.Misc;
using LD51.Data.World;
using Utils.Extensions;

namespace LD51.Game {
	public class OverviewGameState : AbstractStateBehaviour {
		public static OverviewGameState state { get; } = new OverviewGameState();

		private OverviewGameState() { }

		protected override void EnableState() {
			base.EnableState();
			WorldObjectManager.SetHoverWithMouseEnabled(true);
			GameCamera.main.SetTarget(null);
		}

		protected override void DisableState() {
			base.DisableState();
			WorldObjectManager.SetHoverWithMouseEnabled(false);
		}

		protected override void SetListenersEnabled(bool enabled) {
			WorldObject.onClicked.SetListenerActive(HandleBuildingClicked, enabled);
			GameCamera.main.SetControlsEnabled(enabled);
		}

		private static void HandleBuildingClicked(WorldObject worldObject) {
			if (!worldObject.TryGetComponent<TensieSpawnerModule>(out var tensieSpawner)) return;
			CountdownBeforeCharacterState.state.PrepareState(tensieSpawner);
			ChangeState(CountdownBeforeCharacterState.state);
		}
	}
}