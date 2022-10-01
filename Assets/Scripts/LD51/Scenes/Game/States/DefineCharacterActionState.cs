using System.Collections;
using LD51.Data;
using LD51.Data.Misc;
using LD51.Data.Tensies;
using LD51.Data.World;
using Unity.VisualScripting;
using CoroutineRunner = Utils.Coroutines.CoroutineRunner;

namespace LD51.Game {
	public class DefineCharacterActionState : AbstractStateBehaviour {
		public static DefineCharacterActionState state { get; } = new DefineCharacterActionState();

		private TensieSpawnerModule spawner { get; set; }
		private Tensie              tensie  { get; set; }

		private DefineCharacterActionState() { }

		public void PrepareState(TensieSpawnerModule spawner, Tensie tensie) {
			this.spawner = spawner;
			this.tensie = tensie;
			tensie.controller = tensie.AddComponent<InputTensieController>();
			spawner.assignedTensie = tensie;
		}

		protected override void EnableState() {
			base.EnableState();
			GameCamera.main.SetTarget(tensie.transform);
			spawner.worldObject.SetSelected(true);
			tensie.SetSelected(true);
			tensie.SetGhost(false);
			CoroutineRunner.Run(DoActionRecording());
		}

		protected override void DisableState() {
			base.DisableState();
			spawner.worldObject.SetSelected(false);
			tensie.SetSelected(false);
		}

		private IEnumerator DoActionRecording() {
			do {
				yield return null;
			} while (!GameTime.justStartedNewLoop);
			tensie.controller.RemoveComponent();
			tensie.controller = tensie.AddComponent<RepeatTensieController>();
			ChangeState(OverviewGameState.state);
		}

		protected override void SetListenersEnabled(bool enabled) { }
	}
}