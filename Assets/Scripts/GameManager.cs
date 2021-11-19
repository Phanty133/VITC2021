using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public GameObject bgMusicObj;
	public GameObject graphContainerObj;

	void Start()
	{
		// FunctionGenerator funcGen = new FunctionGenerator();
		bgMusicObj.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("volume_music", 0.5f);
		graphContainerObj.GetComponent<AudioGraphContainer>().SetVolume(PlayerPrefs.GetFloat("volume_sfx", 0.3f));
	}
}
