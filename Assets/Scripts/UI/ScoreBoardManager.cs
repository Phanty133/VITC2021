using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class ScoreBoardData {
	public List<int> easy;
	public List<int> medium;
	public List<int> hard;
	private int maxEntryCount;

	public ScoreBoardData(int entryCount = 7, List<int> easyData = null, List<int> medData = null, List<int> hardData = null) {
		maxEntryCount = entryCount;

		easy = easyData == null ? new List<int>(new int[maxEntryCount]) : easyData;
		medium = medData == null ? new List<int>(new int[maxEntryCount]) : medData;
		hard = hardData == null ? new List<int>(new int[maxEntryCount]) : hardData;
	}

	public void Log() {
		string output = "";
		
		output += "Easy: \n";
		foreach (int score in easy) {
			output += score.ToString() + "\n";
		}

		output += "Medium: \n";
		foreach (int score in medium) {
			output += score.ToString() + "\n";
		}

		output += "Hard: \n";
		foreach (int score in hard) {
			output += score.ToString() + "\n";
		}

		Debug.Log(output);
	}
}

public class ScoreBoardManager : MonoBehaviour
{
	public int maxScoreCount = 10;
	public bool saveOnAdd = true;
	public ScoreBoardData data;
	public GameObject scoreboardObj;

	string dataPath {
		get { return Application.persistentDataPath + "/scores.dat"; }
	}

	void LoadData() {
		if (File.Exists(dataPath)) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream fs = File.Open(dataPath, FileMode.Open);

			data = bf.Deserialize(fs) as ScoreBoardData;

			fs.Close();
		} else {
			data = new ScoreBoardData(maxScoreCount);
		}

		data.Log();
	}

	public void AddScore(Difficulty difficulty, int score) {
		List<int> scoreData = new List<int>();

		switch (difficulty) {
			case Difficulty.Easy:
				scoreData = data.easy;
				break;
			case Difficulty.Medium:
				scoreData = data.medium;
				break;
			case Difficulty.Hard:
				scoreData = data.hard;
				break;
		}

		int smallestScore = scoreData.FirstOrDefault(checkScore => score >= checkScore);
		int smallestScoreIndex = scoreData.FindIndex(0, score => score == smallestScore);

		if (smallestScoreIndex == -1) return;

		scoreData.Insert(smallestScoreIndex, score);
		if (scoreData.Count > maxScoreCount) scoreData.Remove(maxScoreCount); // Remove the element over max

		if (saveOnAdd) SaveData();
	}

	void SaveData() {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream fs = File.Create(dataPath);

		bf.Serialize(fs, data);
		fs.Close();
	}

	private void Start() {
		LoadData();
	}

	private void OnDestroy() {
		SaveData();
	}

	public void OpenScoreboard() {
		scoreboardObj.SetActive(true);
	}

	public void CloseScoreboard() {
		scoreboardObj.SetActive(false);
	}

	public void ClearScores() {
		data = new ScoreBoardData();
		SaveData();

		if (scoreboardObj.activeSelf) {
			scoreboardObj.GetComponent<ScoreBoard>().ClearScores();
		}
	}
}
