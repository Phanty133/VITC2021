using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public GameObject bgMusicObj;
	public GameObject graphContainerObj;
	public GameObject loseMenuMngrObj;
	public GameObject projMngrObj;
	public GameObject playerObj;
	public GameObject diffMngrObj;
	public GameObject scoreBoardMngrObj;
	public bool GameActive {
		get { return gameActive; }
	}
	public bool isMainMenu = false;

	private bool gameActive;
	private ProjectileManager projectileManager;
	private LoseMenuManager loseMenuManager;
	private Player player;

	void Start()
	{
		// FunctionGenerator funcGen = new FunctionGenerator();
		bgMusicObj.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("volume_music", 0.5f);

		float sfxVolume = PlayerPrefs.GetFloat("volume_sfx", 0.3f);

		if (isMainMenu) sfxVolume /= 2f;

		graphContainerObj.GetComponent<AudioGraphContainer>().SetVolume(sfxVolume);

		if (!isMainMenu) {
			loseMenuManager = loseMenuMngrObj.GetComponent<LoseMenuManager>();
			player = playerObj.GetComponent<Player>();
		}

		projectileManager = projMngrObj.GetComponent<ProjectileManager>();

		StartGame();
	}

	public void LoseGame() {
		loseMenuManager.OpenLoseMenu();
		projectileManager.StopSpawning();
		scoreBoardMngrObj.GetComponent<ScoreBoardManager>().AddScore(GameOptions.difficulty, player.ScoreInt);

		gameActive = false;
	}

	public void StartGame() {
		if (!isMainMenu) {
			loseMenuManager.CloseLoseMenu();
			player.ResetPlayer();
			diffMngrObj.GetComponent<DifficultyManager>().UpdateValues();
		}
		
		projectileManager.ClearProjectiles();
		projectileManager.StartSpawning();

		gameActive = true;
	}
}
