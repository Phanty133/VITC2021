using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class DifficultyScaling {
	public float speedScaling = 100f; // The smaller the value, the faster it increases
	public float baseSpeed = 2f;
	public float maxSpeed = 5f;
	public float tierScaling = 50f; // The smaller the value, the faster it increases
	public float maxTier = 3f;
	public float depthScaling = 50f;
	public float baseDepth = 1f;
	public float maxDepth = 4f;
	public float spawnScaling = 10f;
	public float baseSpawnRate = 0.1f;
	public float maxSpawnRate = 3f;
}

[System.Serializable]
public struct DifficultyScalingContainer {
	public Difficulty difficulty;
	public DifficultyScaling scalingParameters;
}

public class DifficultyManager : MonoBehaviour
{
	public bool disable = false;
	public GameObject projectileManagerObj;
	public GameObject playerObj;
	public Difficulty difficulty = Difficulty.Easy;
	public float speedScaling {
		get { return GetScaling(difficulty).speedScaling; }
	}
	public float baseSpeed {
		get { return GetScaling(difficulty).baseSpeed; }
	}
	public float maxSpeed {
		get { return GetScaling(difficulty).maxSpeed; }
	}
	public float tierScaling {
		get { return GetScaling(difficulty).tierScaling; }
	}
	public float maxTier {
		get { return GetScaling(difficulty).maxTier; }
	}
	public float depthScaling {
		get { return GetScaling(difficulty).depthScaling; }
	}
	public float baseDepth {
		get { return GetScaling(difficulty).baseDepth; }
	}
	public float maxDepth {
		get { return GetScaling(difficulty).maxDepth; }
	}
	public float spawnScaling {
		get { return GetScaling(difficulty).spawnScaling; }
	}
	public float baseSpawnRate {
		get { return GetScaling(difficulty).baseSpawnRate; }
	}
	public float maxSpawnRate {
		get { return GetScaling(difficulty).maxSpawnRate; }
	}

	public DifficultyScalingContainer[] difficultyScaling;

	private float tierPrecalc1;
	private float tierPrecalc2;
	private ProjectileManager projectileManager;
	private Player player;

	DifficultyScaling GetScaling(Difficulty diff) {
		return difficultyScaling.First(scaling => scaling.difficulty == diff).scalingParameters;
	}

	float CalcSpeed(float score) { // Speed = Base * sqrt(score / scaling)
		float speed = baseSpeed * Mathf.Sqrt(score / speedScaling);
		return Mathf.Clamp(speed, baseSpeed, maxSpeed);
	}

	float CalcTier(float score) { // (m+1)ln(x + 1) / (m * ln(m)) - 0.5
		float tier = (tierScaling + 1) * Mathf.Log(score + 1) / (tierScaling * Mathf.Log(tierScaling)) - 0.5f;
		return Mathf.Clamp(tier, 0, maxTier);
	}

	float CalcDepth(float score) { // x^(2/m)(m+1)ln(x + 1) / (m * ln(m))
		float depth = Mathf.Pow(score, 2 / depthScaling) *  (depthScaling + 1) * Mathf.Log(score + 1) / (depthScaling * Mathf.Log(depthScaling));
		return Mathf.Clamp(depth, baseDepth, maxDepth);
	}

	float CalcSpawnRate(float score) { // base * x^(2/m)(m+1)ln(x + 1) / (m * ln(m))
		float rate = baseSpawnRate * Mathf.Pow(score, 2 / spawnScaling) *  (spawnScaling + 1) * Mathf.Log(score + 1) / (spawnScaling * Mathf.Log(spawnScaling));
		return Mathf.Clamp(rate, baseSpawnRate, maxSpawnRate);
	}

	public void UpdateValues() {
		if (disable) return;
		if (!player) {
			Start();
		}

		float clampedScore = Mathf.Clamp(player.ScoreInt, 0, Mathf.Infinity);

		projectileManager.baseSpeed = CalcSpeed(clampedScore);
		projectileManager.spawnRate = CalcSpawnRate(clampedScore);
		projectileManager.tier = CalcTier(clampedScore);
		projectileManager.funcDepth = CalcDepth(clampedScore);
	}

	private void Start() {
		difficulty = GameOptions.difficulty;

		projectileManager = projectileManagerObj.GetComponent<ProjectileManager>();
		player = playerObj.GetComponent<Player>();

		UpdateValues();
	}
}
