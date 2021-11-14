using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
	public GameObject projPrefab;
	public GameObject difficultyManagerObj;
	public float tier = 3;
	public float funcDepth = 1f;
	public float spawnRate = 1; // Projectiles per second
	public float baseSpeed = 1f;
	public bool randomSpeed = true;
	public float randomSpeedOffset = 1f; // Max percentage offset from base speed (0f - 1f)
	public GameObject projContainer;
	public bool randomOffset = false;

	private float spawnTime;
	private float spawnTimer;
	private bool graphMuted = false;
	private DifficultyManager difficultyManager;

	public void OnListenToGraphToggle() {
		graphMuted = !graphMuted;

		for (int i = 0; i < projContainer.transform.childCount; i++) {
			GameObject child = projContainer.transform.GetChild(i).gameObject;

			if (child.GetComponent<AudioGraph>() == null) continue;

			ProjMovement projMovement = child.GetComponent<ProjMovement>();

			if (!graphMuted) {
				projMovement.UnmuteAudioGraph();
			} else {
				projMovement.MuteAudioGraph();
			}
		}
	}

	private void SpawnProjectile() {
		GameObject proj = Instantiate(projPrefab, new Vector2(0, Camera.main.orthographicSize * 2), new Quaternion(), projContainer.transform);
		ProjMovement projMovement = proj.GetComponent<ProjMovement>();

		projMovement.tier = tier;
		projMovement.funcDepth = funcDepth;

		if (randomSpeed) {
			projMovement.speed = baseSpeed * (1 + Random.Range(-randomSpeedOffset, randomSpeedOffset));
		} else {
			projMovement.speed = baseSpeed;
		}

		projMovement.InitProjectile(this, graphMuted, randomOffset);
	}

	private void Start() {
		difficultyManager = difficultyManagerObj.GetComponent<DifficultyManager>();

		spawnTime = 1 / spawnRate;
		spawnTimer = spawnTime;
		SpawnProjectile();
	}

	private void Update() {
		spawnTimer -= Time.deltaTime;

		if (spawnTimer <= 0) {
			SpawnProjectile();
			spawnTimer = spawnTime;
		}
	}

	public void OnProjectileDestroy() {
		difficultyManager.UpdateValues();
	}
}
