using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gold {
	public class Timer {
		public float length;
		public System.Action methodToCall;
		float _time;
		public bool isLooping;

		bool isTicking;
		public bool IsTicking {
			get { return isTicking; }
		}

		public float RemainingTime {
			get { return _time; }
		}

		public Timer (System.Action action, float time, bool loop = false) {
			methodToCall = action;
			length = time;
			isLooping = loop;
		}

		public void Tick () {
			if (isTicking) {
				_time -= Time.deltaTime;

				if (_time < 0) {
					methodToCall ();
					if (isLooping) {
						Reset ();
					} else {
						Stop ();
					}
				}
			}
		}

		public void Start () {
			Reset ();
			Resume ();
		}

		public void Reset () {
			_time = length;
		}

		public void Resume () {
			isTicking = true;
		}

		public void Stop () {
			isTicking = false;
		}
	}
}