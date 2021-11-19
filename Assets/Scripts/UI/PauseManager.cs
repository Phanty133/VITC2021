using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
	public GameObject pauseMenu;
	public GameObject audioContainer;
	
	private GameManager gameManager;

	public void OpenPauseMenu() {
		if (!gameManager.GameActive) return;

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
		Time.timeScale = 1f;
		SceneManager.LoadScene("MainMenu");
	}

	public void ExitGame() {
		if (!Application.isEditor) {
			Application.Quit();
		}
	}

	private void Start() {
		gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
	}
}
