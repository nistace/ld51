using System;
using System.Collections.Generic;
using System.Linq;
using LD51.Data.Misc;
using UnityEngine;

namespace LD51.Data.Tensies {
	[Serializable]
	public class TensieActionData {
		[SerializeField] protected List<KeyFrame> _frames = new List<KeyFrame>();

		public void CopyFrom(TensieActionData data) {
			_frames.Clear();
			_frames.AddRange(data._frames);
		}

		public void Clear() {
			_frames.Clear();
		}

		public void Write(KeyFrame frame) => _frames.Add(frame);

		public KeyFrame Read() {
			if (_frames.Count == 0) return new KeyFrame();
			return _frames.FirstOrDefault(frame => frame.progressInLoop >= GameTime.progressInCurrentLoop) ?? _frames.Last();
		}

		[Serializable]
		public class KeyFrame {
			[SerializeField] protected float     _progressInLoop;
			[SerializeField] protected Vector3   _position;
			[SerializeField] protected Direction _direction;
			[SerializeField] protected bool      _interacting;
			[SerializeField] protected bool      _moving;

			public float progressInLoop {
				get => _progressInLoop;
				set => _progressInLoop = value;
			}

			public Vector3 position {
				get => _position;
				set => _position = value;
			}

			public Direction direction {
				get => _direction;
				set => _direction = value;
			}

			public bool interacting {
				get => _interacting;
				set => _interacting = value;
			}

			public bool moving {
				get => _moving;
				set => _moving = value;
			}
		}
	}
}