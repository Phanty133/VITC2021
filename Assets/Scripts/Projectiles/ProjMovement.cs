using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjMovement : MonoBehaviour
{
	public float speed = 1f;
	public int direction = 1;
	public GameObject graphPrefab;

	Function moveFunction;
	LUT moveLUT;
	float levelWidth;
	GameObject graphObj;
	Graph graphComp;

	private Vector2 NextPos(float xDist) {
		float nextX = transform.position.x + xDist;
		float funcVal = moveLUT.ValueAt(nextX);
		float nextY;

		if (float.IsNaN(funcVal)) {
			nextY = levelWidth * 2;
		} else if (Mathf.Abs(funcVal) > levelWidth) {
			nextY = levelWidth * 1.1f * Mathf.Sign(funcVal);
		} else {
			nextY = funcVal;
		}

		graphComp.DrawGraphPoint(nextX, nextY);

		return new Vector2(nextX, nextY);
	}

	private void Start() {
		direction = Random.Range(0, 2) == 0 ? -1 : 1;

		moveFunction = FunctionGenerator.Generate(1);
		moveLUT = new LUT(moveFunction, new Vector2(-levelWidth * 1.1f, levelWidth * 1.1f));
		// Debug.Log(moveFunction.GetNotation());

		Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);

		GetComponent<SpriteRenderer>().color = color;

		graphObj = Instantiate(graphPrefab, new Vector2(0, 0), new Quaternion());
		graphComp = graphObj.GetComponent<Graph>();
		graphComp.m_LUT = moveLUT;
		graphComp.m_color = color;
		graphComp.InitGraph();
		// graphComp.TraceGraph();

		levelWidth = Camera.main.orthographicSize * Camera.main.aspect;
		transform.position = new Vector2(levelWidth * 1.1f * -direction, 0);
	}

	private void Update() {
		if (transform.position.x * direction < levelWidth) {
			transform.position = NextPos(speed * Time.deltaTime * direction);
		} else {
			Destroy(graphObj);
			Destroy(gameObject);
		}
	}
}
