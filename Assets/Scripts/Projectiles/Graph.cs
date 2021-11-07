using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Graph : MonoBehaviour
{
	public LUT m_LUT;
	[HideInInspector]
	public Color m_color;
	private GraphSurface graphSurface;
	public Guid guid;

	public void DrawGraphPoint(float unitX, float? preY = null) {
		float unitY;

		if (preY == null) {
			unitY = m_LUT.ValueAt(unitX);
		} else {
			unitY = (float) preY;
		}

		graphSurface.DrawGraphPoint(guid, new Vector2(unitX, unitY), m_color);
	}

	private void Awake() {
		guid = Guid.NewGuid();
		graphSurface = GameObject.FindGameObjectWithTag("GraphSurface").GetComponent<GraphSurface>();
	}
}
