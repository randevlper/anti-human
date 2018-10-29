using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gold {
	[System.Serializable]
	public class ObjectPool {
		GameObject _objectToPool;
		Stack<GameObject> _pooledObjects;
		public int pooledAmount;
		public bool canGrow;

		public ObjectPool (GameObject obj, int num, bool grow = false) {
			_pooledObjects = new Stack<GameObject> ();
			pooledAmount = num;
			_objectToPool = obj;
			canGrow = grow;

			for (int i = 0; i < pooledAmount; i++) {
				SpawnObj ();
			}
		}

		//Need to check if this is the proper gameObject
		void Add (GameObject obj) {
			_pooledObjects.Push (obj);
		}

		public GameObject Get () {
			if (_pooledObjects.Count <= 0) {
				if (canGrow) {
					SpawnObj ();
					pooledAmount++;
					return _pooledObjects.Pop ();
				} else {
					return null;
				}
			}

			return _pooledObjects.Pop ();
		}

		void SpawnObj () {
			GameObject spawnedObj = GameObject.Instantiate (_objectToPool, Vector3.zero, Quaternion.identity);
			spawnedObj.SetActive (false);
			spawnedObj.AddComponent<ObjectPoolComponent> ().Setup (this, Add);
			_pooledObjects.Push (spawnedObj);
		}

		//Can only have one GameObject to pool
		//Can optionally have a number of objects pooled initialy
		//Option to grow
		//Option for max number of pooled items

		//Objects are sent back to the stack when they are done
	}

}