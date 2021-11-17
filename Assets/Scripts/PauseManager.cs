using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
	public GameObject pauseMenu;
	public GameObject audioContainer;

	public void OpenPauseMenu() {
		Pause();
		pauseMenu.SetActive(true);
	}

	public void ClosePauseMenu() {
		Unpause();
		pauseMenu.SetActive(false);
	}

	public void Pause() {
		Time.timeScale = 0f;

		for (int i = 0; i < audioContainer.transform.childCount; i++) {
			audioContainer.transform.GetChild(i).GetComponent<AudioGraph>().Pause();
		}
	}

	public void Unpause() {
		Time.timeScale = 1f;

		for (int i = 0; i < audioContainer.transform.childCount; i++) {
			audioContainer.transform.GetChild(i).GetComponent<AudioGraph>().Play();
		}
	}

	public void ReturnToMainMenu() {
		SceneManager.LoadScene("MainMenu");
	}

	public void ExitGame() {
		if (!Application.isEditor) {
			Application.Quit();
		}
	}
}
