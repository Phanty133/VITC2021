using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreBoard : MonoBehaviour
{
	public GameObject scoreboardMngrObj;
	public GameObject container;
	public GameObject entryPrefab;

	private ScoreBoardManager scoreBoardManager;
	private Dictionary<Difficulty, Transform> colDict = new Dictionary<Difficulty, Transform>();

	void AddEntry(Difficulty difficulty, int score) {
		if (score == 0) return;
		if (colDict[difficulty].childCount > scoreBoardManager.maxScoreCount) return;

		GameObject entry = Instantiate(entryPrefab, new Vector3(), new Quaternion(), colDict[difficulty]);
		entry.GetComponent<TextMeshProUGUI>().text = score.ToString();
	}

	public void ClearScores() {
		for (int d = 0; d < 3; d++) {
			for (int i = 1; i < colDict[(Difficulty)d].transform.childCount; i++) {
				Destroy(colDict[(Difficulty)d].transform.GetChild(i).gameObject);
			}
		}
	}

	public void LoadScores() {
		ClearScores();

		foreach (int score in scoreBoardManager.data.easy) {
			AddEntry(Difficulty.Easy, score);
		}

		foreach (int score in scoreBoardManager.data.medium) {
			AddEntry(Difficulty.Medium, score);
		}

		foreach (int score in scoreBoardManager.data.hard) {
			AddEntry(Difficulty.Hard, score);
		}
	}

	private void OnEnable() {
		scoreBoardManager = scoreboardMngrObj.GetComponent<ScoreBoardManager>();

		for (int i = 0; i < container.transform.childCount; i++) { // cringe
			colDict[(Difficulty)i] = container.transform.GetChild(i);
		}

		LoadScores();
	}
}
