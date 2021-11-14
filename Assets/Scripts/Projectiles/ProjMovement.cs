using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjMovement : MonoBehaviour
{
	public float speed = 1f;
	public int direction = 1;
	public Color color;
	public bool randomColor = true;
	[HideInInspector]
	public float tier;
	public float funcDepth;
	public GameObject audioGraphPrefab;
	public float minComplexity = 0.1f;

	Function moveFunction;
	LUT moveLUT;
	float surfaceWidth;
	Graph graphComp;
	AudioGraph audioGraph;
	bool graphMuted = false;
	GameObject audioGraphObj;
	float halfSurfaceWidth;
	ProjectileManager projectileManager;

	private Vector2 NextPos(float xDist)
	{
		float nextX = transform.position.x + xDist;
		float funcVal = moveFunction.Process(nextX); // moveLUT.ValueAt(nextX);
		float nextY;

		if (float.IsNaN(funcVal))
		{
			nextY = surfaceWidth * 2;
		}
		else if (Mathf.Abs(funcVal) > surfaceWidth)
		{
			nextY = surfaceWidth * 1.1f * Mathf.Sign(funcVal);
		}
		else
		{
			nextY = funcVal;
		}

		graphComp.DrawGraphPoint(nextX, nextY);

		return new Vector2(nextX, nextY);
	}

	public void UnmuteAudioGraph() {
		audioGraph.Unmute();
		graphMuted = false;
	}

	public void MuteAudioGraph() {
		audioGraph.Mute();
		graphMuted = true;
	}

	public void InitProjectile(ProjectileManager projMgr, bool muted = false, bool randomOffset = false) {
		projectileManager = projMgr;
		direction = Random.Range(0, 2) == 0 ? -1 : 1;
		surfaceWidth = GameObject.FindGameObjectWithTag("GraphSurface").GetComponent<GraphSurface>().surfaceWidth;
		halfSurfaceWidth = surfaceWidth / 2f;

		float complexity = float.NaN;

		while (float.IsNaN(complexity) || complexity < minComplexity) {
			Function randFunc = FunctionGenerator.Generate(tier, funcDepth);

			if (randomOffset && randFunc.id != "add") {
				moveFunction = new Add(randFunc, new Constant());
			} else {
				moveFunction = randFunc;
			}
			
			moveLUT = new LUT(moveFunction, new Vector2(-halfSurfaceWidth, halfSurfaceWidth));
			complexity = moveLUT.EstimateComplexity();

			if (float.IsNaN(complexity)) {
				Debug.Log("regen");
			}
		}

		// moveFunction = new Unknown();
		// moveLUT = new LUT(moveFunction, new Vector2(-halfSurfaceWidth, halfSurfaceWidth));
		// Debug.Log(moveFunction.GetNotation());
		Debug.Log(moveLUT.EstimateComplexity());

		if (randomColor)
		{
			color = new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), 1f);
		}

		GetComponent<SpriteRenderer>().color = color;

		Material m = new Material(GetComponent<SpriteRenderer>().material);
		GetComponent<SpriteRenderer>().material = m;
		GetComponent<SpriteRenderer>().material.SetColor("_BaseColor", color);
		// GetComponent<TrailRenderer>().materials[0] = m;
		// GetComponent<TrailRenderer>().materials[0].SetColor("_BaseColor", color);

		graphComp = GetComponent<Graph>();
		graphComp.SetGraph(moveLUT, color);
		// graphComp.TraceGraph();

		transform.position = new Vector2(surfaceWidth * 0.55f * -direction, 0);

		Transform audioGraphContainer = GameObject.FindGameObjectWithTag("GraphAudio").transform;
		audioGraphObj = Instantiate(audioGraphPrefab, new Vector3(), new Quaternion(), audioGraphContainer);
		audioGraph = audioGraphObj.GetComponent<AudioGraph>();
		graphMuted = muted;
	}

	private void Update()
	{
		if (transform.position.x * direction < halfSurfaceWidth)
		{
			transform.position = NextPos(speed * Time.deltaTime * direction);

			if (Mathf.Abs(transform.position.x) < halfSurfaceWidth && !audioGraph.playing) {
				audioGraph.PlayGraph(moveLUT, surfaceWidth / speed, graphMuted);
			} else if (audioGraph.playing) {
				audioGraph.SetPan(transform.position.x / halfSurfaceWidth);
			}
		}
		else
		{
			audioGraph.Fade();
			Destroy(gameObject);
		}
	}

	private void OnDestroy() {
		projectileManager.OnProjectileDestroy();
	}
}
