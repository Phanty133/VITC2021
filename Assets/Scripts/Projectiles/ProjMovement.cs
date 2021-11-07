using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjMovement : MonoBehaviour
{
	public float speed = 1f;
	public int direction = 1;
	public Color color;
	public bool randomColor = true;

	Function moveFunction;
	LUT moveLUT;
	float levelWidth;
	Graph graphComp;

	private Vector2 NextPos(float xDist)
	{
		float nextX = transform.position.x + xDist;
		float funcVal = moveLUT.ValueAt(nextX);
		float nextY;

		if (float.IsNaN(funcVal))
		{
			nextY = levelWidth * 2;
		}
		else if (Mathf.Abs(funcVal) > levelWidth)
		{
			nextY = levelWidth * 1.1f * Mathf.Sign(funcVal);
		}
		else
		{
			nextY = funcVal;
		}

		graphComp.DrawGraphPoint(nextX, nextY);

		return new Vector2(nextX, nextY);
	}

	private void Start()
	{
		direction = Random.Range(0, 2) == 0 ? -1 : 1;

		moveFunction = FunctionGenerator.Generate(1);
		moveLUT = new LUT(moveFunction, new Vector2(-levelWidth * 1.1f, levelWidth * 1.1f));
		// Debug.Log(moveFunction.GetNotation());

		if (randomColor) {
			color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
		}

		GetComponent<SpriteRenderer>().color = color;
		
		Material m = new Material(GetComponent<TrailRenderer>().materials[0]);
		GetComponent<SpriteRenderer>().material = m;
		GetComponent<SpriteRenderer>().material.SetColor("_BaseColor", color);
		GetComponent<TrailRenderer>().materials[0] = m;
		GetComponent<TrailRenderer>().materials[0].SetColor("_BaseColor", color);

		graphComp = GetComponent<Graph>();
		graphComp.m_LUT = moveLUT;
		graphComp.m_color = color;
		// graphComp.TraceGraph();

		levelWidth = Camera.main.orthographicSize * Camera.main.aspect;
		transform.position = new Vector2(levelWidth * 1.1f * -direction, 0);
	}

	private void Update()
	{
		if (transform.position.x * direction < levelWidth)
		{
			transform.position = NextPos(speed * Time.deltaTime * direction);
		} else {
			Destroy(gameObject);
		}
	}
}
