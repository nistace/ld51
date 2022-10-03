using System.Collections;
using LD51.Data.Misc;
using UnityEngine;
using Utils.Extensions;

public class GameAudio : MonoBehaviour {
	[SerializeField] protected AudioSource[] _sources;
	[SerializeField] protected float[]       _volumeCoefficient = { 1, 1, 1, 1, 1 };
	[SerializeField] protected float         _volume;
	[SerializeField] protected bool          _alreadyWaited;
	[SerializeField] protected float         _timeToFadeIn = .5f;

	public static int desiredLevel { get; set; }

	private int currentLevel { get; set; }

	private void Start() {
		_sources.ForEach(t => t.volume = 0);
		currentLevel = -1;
	}

	private void Update() {
		if (!_alreadyWaited) {
			if (Time.time > 3) {
				_alreadyWaited = true;
				RefreshSources();
			}
			return;
		}
		if (!GameTime.justStartedNewLoop) return;
		RefreshSources();
	}

	private void RefreshSources() {
		foreach (var source in _sources) {
			source.Play();
			source.loop = true;
			source.time = GameTime.timeInCurrentLoop;
		}
		for (var i = currentLevel + 1; i <= desiredLevel; ++i) {
			if (i >= 0 && i < _sources.Length) StartCoroutine(FadeInSource(_sources[i], _volumeCoefficient[i]));
		}
		for (var i = currentLevel; i > desiredLevel; --i) {
			if (i >= 0 && i < _sources.Length) StartCoroutine(FadeOutSource(_sources[i], _volumeCoefficient[i]));
		}
		currentLevel = desiredLevel;
	}

	private IEnumerator FadeInSource(AudioSource source, float coefficient) {
		for (float lerp = 0; lerp < 1; lerp += Time.deltaTime / _timeToFadeIn) {
			source.volume = coefficient * lerp * _volume;
			yield return null;
		}
		source.volume = coefficient * _volume;
	}

	private IEnumerator FadeOutSource(AudioSource source, float coefficient) {
		for (float lerp = 1; lerp > 0; lerp -= Time.deltaTime / _timeToFadeIn) {
			source.volume = coefficient * lerp * _volume;
			yield return null;
		}
		source.volume = 0;
	}
}