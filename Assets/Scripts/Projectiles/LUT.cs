using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LUT
{
	public Function m_func;
	public float m_precision;
	public Vector2 m_range;
	private int m_keyprecision = 1;

	Dictionary<float, float> m_table = new Dictionary<float, float>();

	private float RoundToKeyPrecision(float v) {
		return (float)System.Math.Round((double)v, m_keyprecision);
	}

	private float RoundToKey(float v) {
		float minX = Mathf.FloorToInt(v / m_precision) * m_precision;
		float maxX = Mathf.CeilToInt(v / m_precision) * m_precision;
		float interp = (v - minX) / m_precision;

		return interp < 0.5f ? minX : maxX;
	}

	private void LogLUT() {
		string table = "";

		foreach (float k in m_table.Keys) {
			table += string.Format("{0} : {1}", k, m_table[k]) + "\n";
		}

		Debug.Log(table);
	}

	public LUT(Function f, Vector2 range, float precision = 0.1f) {
		m_func = f;
		m_precision = precision;
		m_range = new Vector2(RoundToKey(range.x), RoundToKey(range.y));
		Generate();

		LogLUT();
	}

	public void ProcessTask(float x) {
		m_table[x] = m_func.Process(x);
	}

	public void Generate() {
		for (float x = m_range.x; x <= m_range.y; x = Mathf.Clamp(x + m_precision, m_range.x, m_range.y)) {
			m_table.Add(RoundToKeyPrecision(x), m_func.Process(x));

			if (x == m_range.y) break;
		}
	}

	public float ValueAt(float x) {
		if (x < m_range.x || x > m_range.y) {
			return m_func.Process(x);
		}

		float minKnownX = Mathf.FloorToInt(x / m_precision) * m_precision;
		float maxKnownX = Mathf.CeilToInt(x / m_precision) * m_precision;
		float interp = (x - minKnownX) / m_precision;

		float minKnownY = m_table[RoundToKeyPrecision(minKnownX)];
		float maxKnownY = m_table[RoundToKeyPrecision(maxKnownX)];
		
		return Mathf.Lerp(minKnownY, maxKnownY, interp);
	}

	public float EstimateComplexity() {
		List<float> difs = new List<float>();

		for (float x = m_range.x; x < m_range.y; x = Mathf.Clamp(x + m_precision, m_range.x, m_range.y)) {
			float curKey = RoundToKeyPrecision(x);
			float nextKey = RoundToKeyPrecision(x + m_precision);
			float dif = Mathf.Abs(m_table[nextKey] - m_table[curKey]);
			difs.Add(dif);
		}

		return difs.Sum() / difs.Count;
	}
}
