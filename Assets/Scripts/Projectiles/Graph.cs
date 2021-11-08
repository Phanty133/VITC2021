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
	private FuncPanel funcPanel;

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
		funcPanel = GameObject.FindGameObjectWithTag("FuncPanel").GetComponent<FuncPanel>();
	}

	public void SetGraph(LUT lut, Color color) {
		m_LUT = lut;
		m_color = color;
		funcPanel.AddFunction(guid, m_LUT.m_func, m_color);
	}

	private void OnDestroy() {
		funcPanel.RemoveFunction(guid);
	}
}
