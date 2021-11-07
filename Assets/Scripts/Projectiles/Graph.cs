using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
	public LUT m_LUT;
	public Color m_color;
	private float pixelsPerUnit;
	private SpriteRenderer spriteRenderer = null;

	private void ThickDot(Color[,] pixels, int x, int y, Color color, int diameter) {
		int maxOffset = Mathf.FloorToInt(diameter / 2f);

		for (int offsetY = -maxOffset; offsetY < maxOffset; offsetY++) {
			for (int offsetX = -maxOffset; offsetX < maxOffset; offsetX++) {
				int xClamped = Mathf.Clamp(x + offsetX, 0, pixels.GetLength(1));
				int yClamped = Mathf.Clamp(y + offsetY, 0, pixels.GetLength(0));

				pixels[yClamped, xClamped] = color;
			}
		}
	}

	public void InitGraph() {
		Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGBA32, true);
		Color[] pixels = new Color[Screen.height * Screen.width];

		for (int p = 0; p < Screen.height * Screen.width; p++) {
			pixels[p] = new Color(0, 0, 0, 0);
		}

		texture.SetPixels(pixels);
		texture.Apply(true);

		Sprite sprite = Sprite.Create(texture, new Rect(0, 0, Screen.width, Screen.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
		GetComponent<SpriteRenderer>().sprite = sprite;
	}

	public void DrawGraphPoint(float unitX, float? preY = null) {
		int pixelX = Mathf.RoundToInt(unitX * pixelsPerUnit + Screen.width / 2);
		float unitY;

		if (pixelX > Screen.width - 1) return;

		if (preY == null) {
			unitY = m_LUT.ValueAt(unitX);
		} else {
			unitY = (float) preY;
		}

		int pixelY = Mathf.RoundToInt(unitY * pixelsPerUnit + Screen.height / 2);

		if (pixelY > Screen.height - 1 || pixelY < 0) return;

		spriteRenderer.sprite.texture.SetPixel(pixelX, pixelY, m_color);
		spriteRenderer.sprite.texture.Apply(true);
	}

	public void TraceGraph() {
		Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGBA32, true);
		Color[,] pixels = new Color[Screen.height,Screen.width];

		for (int y = 0; y < Screen.height; y++) {
			for (int x = 0; x < Screen.width; x++) {
				pixels[y, x] = new Color(0, 0, 0, 0);
			}
		}

		float levelWidth = Camera.main.orthographicSize * Camera.main.aspect;
		float pixelsPerStep = m_LUT.m_precision * pixelsPerUnit;

		for (float x = -levelWidth; x <= levelWidth - m_LUT.m_precision; x+=m_LUT.m_precision) {
			float yMin = m_LUT.ValueAt(x);
			float yMax = m_LUT.ValueAt(x + m_LUT.m_precision);
			int pixelX = Mathf.RoundToInt(x * pixelsPerUnit + Screen.width / 2);

			for (int i = 0; i < pixelsPerStep; i++) {
				if (pixelX + i > Screen.width - 1) break;

				float interp = i / pixelsPerStep;
				float unitY = Mathf.Lerp(yMin, yMax, interp);
				int pixelY = Mathf.RoundToInt(unitY * pixelsPerUnit + Screen.height / 2);

				if (pixelY > Screen.height - 1 || pixelY < 0) continue;

				ThickDot(pixels, pixelX, pixelY, m_color, 3);
			}
		}

		texture.SetPixels(pixels.Flatten<Color>());
		texture.Apply(true);

		Sprite sprite = Sprite.Create(texture, new Rect(0, 0, Screen.width, Screen.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
		spriteRenderer.sprite = sprite;
	}

	private void Awake() {
		pixelsPerUnit = Screen.height / (Camera.main.orthographicSize * 2);
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
}
