using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
	public GameObject musicSliderObj;
	public GameObject sfxSliderObj;
	public GameObject bgMusicObj;
	public GameObject audioGraphContainerObj;
	private Slider musicSlider;
	private Slider sfxSlider;
	private AudioSource bgMusic;
	private AudioGraphContainer audioGraphContainer;

	private void Awake() {
		musicSlider = musicSliderObj.GetComponent<Slider>();
		sfxSlider = sfxSliderObj.GetComponent<Slider>();
		bgMusic = bgMusicObj.GetComponent<AudioSource>();
		audioGraphContainer = audioGraphContainerObj.GetComponent<AudioGraphContainer>();

		musicSlider.onValueChanged.AddListener(OnMusicVolumeChange);
		sfxSlider.onValueChanged.AddListener(OnSFXVolumeChange);
	}

	public void OnMusicVolumeChange(float newVal) {
		PlayerPrefs.SetFloat("volume_music", newVal);
		bgMusic.volume = newVal;
	}

	public void OnSFXVolumeChange(float newVal) {
		PlayerPrefs.SetFloat("volume_sfx", newVal);
		audioGraphContainer.SetVolume(newVal);
	}
}
