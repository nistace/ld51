using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;

namespace LD51.Data {
	[CreateAssetMenu]
	public class SpriteAtlas : ScriptableObject {
		[SerializeField] protected Texture2D _texture;
		[SerializeField] protected TextAsset _jsonFile;
		[SerializeField] protected Vector2   _pivot = new Vector2(.5f, 0);

		private Layer[] _layers = { };

		public Layer this[string name] {
			get {
				if (_layers.Any(t => t.name == name)) return _layers.First(t => t.name == name);
				Array.Resize(ref _layers, _layers.Length + 1);
				_layers[^1] = new Layer(name);
				return _layers[^1];
			}
		}

		public IEnumerator Build(UnityAction<float> progress = null, UnityAction onDone = null) {
			var json = JsonConvert.DeserializeObject<JsonTextureDescriptor>(_jsonFile.text);
			_layers = Array.Empty<Layer>();
			progress?.Invoke(.3f);
			yield return null;
			for (var frameIndex = 0; frameIndex < json.frames.Length; ++frameIndex) {
				var frame = json.frames[frameIndex];
				this[frame.layerName][frame.tagName][frame.frameIndex] = Sprite.Create(_texture, frame.GetRect(_texture.height), _pivot, 16);
				progress?.Invoke(.3f + .7f * frameIndex / json.frames.Length);
				yield return null;
			}
			onDone?.Invoke();
		}

		public Sprite GetAnimationSprite(string layer, string tag, int frame) => this[layer][tag][frame];

		[Serializable]
		public class Layer {
			[SerializeField] protected string _name;
			[SerializeField] protected Tag[]  _frames;

			public string             name   => _name;
			public IReadOnlyList<Tag> frames => _frames;

			public Tag this[string name] {
				get {
					if (_frames.Any(t => t.name == name)) return _frames.First(t => t.name == name);
					Array.Resize(ref _frames, _frames.Length + 1);
					_frames[^1] = new Tag(name);
					return _frames[^1];
				}
			}

			public Layer() : this("New battler") { }

			public Layer(string name) {
				_name = name;
				_frames = new Tag[] { };
			}
		}

		[Serializable]
		public class Tag {
			[SerializeField] protected string   _name;
			[SerializeField] protected Sprite[] _sprites;

			public string                name        => _name;
			public int                   countFrames => _sprites.Length;
			public IReadOnlyList<Sprite> frames      => _sprites;

			public Sprite this[int index] {
				get {
					if (_sprites.Length == 0) {
						Debug.LogWarning("No sprite in Tag" + _name);
						return null;
					}
					return _sprites[index % _sprites.Length];
				}
				internal set {
					if (_sprites.Length <= index) Array.Resize(ref _sprites, index + 1);
					_sprites[index] = value;
				}
			}

			public Tag() : this("New animation") { }

			public Tag(string name) {
				_name = name;
				_sprites = new Sprite[] { };
			}
		}

		[Serializable]
		public class JsonTextureDescriptor {
			[JsonProperty("frames"), SerializeField] protected Frame[] _frames = Array.Empty<Frame>();

			[JsonIgnore] public Frame[] frames => _frames;

			[Serializable]
			public class Frame {
				[JsonProperty("filename"), SerializeField] protected string _filename;
				[JsonProperty("frame"), SerializeField]    protected Rect   _frame;

				[JsonIgnore] public string filename   => _filename;
				[JsonIgnore] public string layerName  => filename.Split(".")[0];
				[JsonIgnore] public string tagName    => filename.Split(".")[1];
				[JsonIgnore] public int    frameIndex => int.Parse(filename.Split(".")[2]);

				public UnityEngine.Rect GetRect(int textureHeight) => new UnityEngine.Rect(_frame.x, textureHeight - _frame.h - _frame.y, _frame.w, _frame.h);

				[Serializable]
				public class Rect {
					[JsonProperty("x"), SerializeField] protected int _x;
					[JsonProperty("y"), SerializeField] protected int _y;
					[JsonProperty("w"), SerializeField] protected int _w;
					[JsonProperty("h"), SerializeField] protected int _h;

					[JsonIgnore] public int x => _x;
					[JsonIgnore] public int y => _y;
					[JsonIgnore] public int w => _w;
					[JsonIgnore] public int h => _h;
				}
			}
		}
	}
}