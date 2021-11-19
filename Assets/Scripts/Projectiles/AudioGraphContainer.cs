using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioGraphContainer : MonoBehaviour
{
	public float volume = 0.3f;

	public void SetVolume(float newVol) {
		volume = newVol;

		for (int i = 0; i < transform.childCount; i++) {
			transform.GetChild(i).GetComponent<AudioGraph>().SetVolume(newVol);
		}
	}
}
