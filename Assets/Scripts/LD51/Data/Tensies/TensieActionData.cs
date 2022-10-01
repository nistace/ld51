using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LD51.Data.Tensies {
	[Serializable]
	public class TensieActionData {
		[SerializeField] protected List<KeyFrame> _frames = new List<KeyFrame>();

		public void Clear() {
			_frames.Clear();
		}

		public void Write(KeyFrame frame) => _frames.Add(frame);

		public KeyFrame Read() => _frames.FirstOrDefault(frame => frame.progressInLoop >= GameTime.progressInCurrentLoop) ?? _frames.Last();

		[Serializable]
		public class KeyFrame {
			[SerializeField] protected float           _progressInLoop;
			[SerializeField] protected Vector3         _position;
			[SerializeField] protected TensieAnimation _animation;
			[SerializeField] protected Direction       _direction;
			[SerializeField] protected bool            _interacting;

			public float progressInLoop => _progressInLoop;

			public TensieAnimation animation {
				get => _animation;
				set => _animation = value;
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

			public KeyFrame() {
				_progressInLoop = GameTime.progressInCurrentLoop;
			}
		}
	}
}