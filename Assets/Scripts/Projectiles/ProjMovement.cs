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
	public int tier;

	Function moveFunction;
	LUT moveLUT;
	float surfaceWidth;
	Graph graphComp;
	AudioGraph audioGraph;
	bool graphMuted = false;

	private Vector2 NextPos(float xDist)
	{
		float nextX = transform.position.x + xDist;
		float funcVal = moveLUT.ValueAt(nextX);
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
		audioGraph.SetVolume(0.3f);
		graphMuted = false;
	}

	public void MuteAudioGraph() {
		audioGraph.SetVolume(0);
		graphMuted = true;
	}

	public void InitProjectile(bool muted = false) {
		direction = Random.Range(0, 2) == 0 ? -1 : 1;

		surfaceWidth = GameObject.FindGameObjectWithTag("GraphSurface").GetComponent<GraphSurface>().surfaceWidth;
		moveFunction = FunctionGenerator.Generate(tier);
		// moveFunction = new Sine();
		// moveFunction.children.Add(new Unknown());
		moveLUT = new LUT(moveFunction, new Vector2(-surfaceWidth * 0.55f, surfaceWidth * 0.55f));
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

		audioGraph = GetComponent<AudioGraph>();
		graphMuted = muted;
	}

	private void Update()
	{
		if (transform.position.x * direction < surfaceWidth)
		{
			transform.position = NextPos(speed * Time.deltaTime * direction);

			if (Mathf.Abs(transform.position.x) < surfaceWidth / 2f && !audioGraph.playing) {
				audioGraph.PlayGraph(moveLUT, surfaceWidth / speed, graphMuted);
			}
		}
		else
		{
			Destroy(gameObject);
		}
	}
}
