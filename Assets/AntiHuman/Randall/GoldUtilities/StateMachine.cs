using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gold {
	[System.Serializable]
	public class FiniteStateMachineNode {
		public System.Action OnEnter;
		public System.Action OnStay;
		public System.Action OnExit;

		FiniteStateMachineNode () { }

		public FiniteStateMachineNode (System.Action onStay, System.Action onEnter = null, System.Action onExit = null) {
			OnEnter += onEnter;
			OnStay += onStay;
			OnExit += onExit;

			OnEnter += Empty;
			OnStay += Empty;
			OnExit += Empty;
		}

		void Empty () { }
	}

	[System.Serializable]
	public class FiniteStateMachine {
		FiniteStateMachineNode _currentState;
		Dictionary<string, FiniteStateMachineNode> _states;
		bool isRunning;

		public FiniteStateMachine () {
			_states = new Dictionary<string, FiniteStateMachineNode> ();
		}

		public bool Start (string key) {
			FiniteStateMachineNode retval;
			if (_states.TryGetValue (key, out retval)) {
				_currentState = retval;
				isRunning = true;
				return true;
			}
			Debug.LogError ("WARNING - " + key + " does not exsist! Cannot start the StateMachine!");
			return false;
		}

		public void Tick () {
			if (isRunning) {
				_currentState.OnStay ();
			}
		}

		public bool Add (FiniteStateMachineNode state, string key) {
			FiniteStateMachineNode retval;
			if (_states.TryGetValue (key, out retval)) {
				return false;
			} else {
				_states.Add (key, state);
				return true;
			}
		}

		public bool ChangeState (string key) {
			FiniteStateMachineNode retval;
			if (_states.TryGetValue (key, out retval)) {
				_currentState.OnExit ();
				_currentState = retval;
				_currentState.OnEnter ();
				return true;
			}
			return false;
		}
	}
}