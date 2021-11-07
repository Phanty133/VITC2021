using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LUT
{
	public Function m_func;
	public float m_precision;
	public Vector2 m_range;

	Dictionary<float, float> m_table = new Dictionary<float, float>();

	public LUT(Function f, Vector2 range, float precision = 0.25f) {
		m_func = f;
		m_precision = precision;
		m_range = range;
		Generate();
	}

	public void ProcessTask(float x) {
		m_table[x] = m_func.Process(x);
	}

	public void Generate() {
		for (float x = m_range.x; x <= m_range.y; x = Mathf.Clamp(x + m_precision, m_range.x, m_range.y)) {
			m_table[x] = m_func.Process(x);

			if (x == m_range.y) break;
		}
	}

	public float ValueAt(float x) {
		if (x < m_range.x || x > m_range.y) {
			return m_func.Process(x);
		}

		float minKnownX = Mathf.FloorToInt(x / m_precision) * m_precision;
		float maxKnownX = Mathf.CeilToInt(x / m_precision) * m_precision;
		float interp = (maxKnownX - minKnownX) / m_precision;

		float minKnownY = m_table[minKnownX];
		float maxKnownY = m_table[maxKnownX];
		
		return Mathf.Lerp(minKnownY, maxKnownY, interp);
	}
}
