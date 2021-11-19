using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
	public GameObject mainMenu;
	public GameObject optionsMenu;
	public GameObject diffMenu;

	public void OpenDifficultySelect() {
		mainMenu.SetActive(false);
		diffMenu.SetActive(true);
	}

	public void StartGameEasy() {
		GameOptions.difficulty = Difficulty.Easy;
		StartGame();
	}

	public void StartGameMedium() {
		GameOptions.difficulty = Difficulty.Medium;
		StartGame();
	}

	public void StartGameHard() {
		GameOptions.difficulty = Difficulty.Hard;
		StartGame();
	}

	private void StartGame() {
		SceneManager.LoadScene("TestScene");
	}

	public void QuitGame() {
		Application.Quit();
	}

	public void OpenOptions() {
		mainMenu.SetActive(false);
		optionsMenu.SetActive(true);
	}

	public void OpenMainMenu() {
		mainMenu.SetActive(true);
		diffMenu.SetActive(false);
		optionsMenu.SetActive(false);
	}

	private void Awake() {
		OpenMainMenu();
	}
}
