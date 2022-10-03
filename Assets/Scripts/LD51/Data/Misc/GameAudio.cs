using LD51.Data.Misc;
using UnityEngine;
using Utils.Extensions;

public class GameAudio : MonoBehaviour {
	[SerializeField] protected AudioSource[] _sources;
	[SerializeField] protected float[]       _volumeCoefficient = { 1, 1, 1, 1, 1 };
	[SerializeField] protected bool          _alreadyWaited;
	[SerializeField] protected float         _timeToFadeIn = .5f;

	public static float volume       { get; set; } = 1;
	public static int   desiredLevel { get; set; } = 0;

	private void Start() {
		_sources.ForEach(t => t.volume = 0);
	}

	private void Update() {
		if (!_alreadyWaited && Time.time > 1) {
			foreach (var source in _sources) {
				source.Play();
				source.loop = true;
				source.time = GameTime.timeInCurrentLoop;
				source.volume = 0;
			}
			_alreadyWaited = true;
		}

		RefreshSources();
	}

	private void RefreshSources() {
		for (var i = 0; i < _sources.Length; ++i) {
			if (GameTime.justStartedNewLoop) _sources[i].time = GameTime.timeInCurrentLoop;
			_sources[i].volume = Mathf.MoveTowards(_sources[i].volume, desiredLevel >= i ? _volumeCoefficient[i] * volume : 0, Time.deltaTime / _timeToFadeIn);
		}
	}
}