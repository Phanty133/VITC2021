using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
	public GameObject projPrefab;
	public int tier = 3;
	public float spawnRate = 1; // Projectiles per second
	public float baseSpeed = 1f;
	public bool randomSpeed = true;
	public float randomSpeedOffset = 1f; // Max offset for random speed
	public GameObject projContainer;
	public bool randomOffset = false;

	private float spawnTime;
	private float spawnTimer;
	private bool graphMuted = false;

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

		if (randomSpeed) {
			projMovement.speed = baseSpeed + Random.Range(-randomSpeedOffset, randomSpeedOffset);
		} else {
			projMovement.speed = baseSpeed;
		}

		projMovement.InitProjectile(graphMuted, randomOffset);
	}

	private void Start() {
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
}
