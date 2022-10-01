using UnityEngine;

namespace LD51.Game {
	public abstract class AbstractStateBehaviour {
		private static AbstractStateBehaviour currentState { get; set; }

		protected virtual void EnableState() {
			SetListenersEnabled(true);
		}

		protected virtual void DisableState() {
			SetListenersEnabled(false);
		}

		protected abstract void SetListenersEnabled(bool enabled);

		public static void ChangeState(AbstractStateBehaviour newState) {
			currentState?.DisableState();
			currentState = newState;
			Debug.Log(newState.GetType().Name);
			currentState?.EnableState();
		}
	}
}