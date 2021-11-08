using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FuncPanel : MonoBehaviour
{
	public GameObject funcEntryPrefab;
	Transform container;
	Dictionary<Guid, GameObject> graphEntries = new Dictionary<Guid, GameObject>(); 

	private void Start() {
		container = transform.GetChild(0);	
	}

	public void AddFunction(Guid guid, Function function, Color color) {
		GameObject obj = Instantiate(funcEntryPrefab, new Vector3(), new Quaternion(), container);
		obj.GetComponent<FuncEntry>().SetGraph(function, color);

		graphEntries[guid] = obj;
	}

	public void RemoveFunction(Guid guid) {
		if (!graphEntries.ContainsKey(guid)) {
			throw new ArgumentException("Unable to remove function: Graph GUID does not exist");
		}

		Destroy(graphEntries[guid]);
		graphEntries.Remove(guid);
	}
}
