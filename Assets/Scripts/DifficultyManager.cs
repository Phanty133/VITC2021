using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
	public GameObject projectileManagerObj;
	public GameObject playerObj;
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

	private float tierPrecalc1;
	private float tierPrecalc2;
	private ProjectileManager projectileManager;
	private Player player;

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
		projectileManager.baseSpeed = CalcSpeed(player.score);
		projectileManager.spawnRate = CalcSpawnRate(player.score);
		projectileManager.tier = CalcTier(player.score);
		projectileManager.funcDepth = CalcDepth(player.score);
	}

	private void Start() {
		projectileManager = projectileManagerObj.GetComponent<ProjectileManager>();
		player = playerObj.GetComponent<Player>();

		UpdateValues();
	}
}
