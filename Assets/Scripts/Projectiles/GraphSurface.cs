using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

class GraphPixel {
	public Vector2Int coords;
	public Color color;
	public float time;

	public GraphPixel(Vector2Int _coords, Color _color) {
		coords = _coords;
		color = _color;
		time = 0f;
	}
}

public class GraphSurface : MonoBehaviour
{
	public float fadeTime = 1f;
	public int thickness = 5;
	float pixelsPerUnit;
	Texture2D texture;
	Dictionary<Guid, Vector2Int> prevCoords = new Dictionary<Guid, Vector2Int>();
	Dictionary<Guid, List<GraphPixel>> graphCoords = new Dictionary<Guid, List<GraphPixel>>();
	private bool updateTexture = false;
	private Color[] textureData;
	public float surfaceWidth = 24f;

	public Vector2Int WorldToSurface(Vector2 worldPos) {
		return new Vector2Int(
			Mathf.RoundToInt(worldPos.x * pixelsPerUnit + Screen.width / 2),
			Mathf.RoundToInt(worldPos.y * pixelsPerUnit + Screen.height / 2)
		);
	}

	private void SetPixel(Vector2Int coords, Color color) {
		textureData[coords.y * Screen.width + coords.x] = color;
		updateTexture = true;
	}

	private void SetPixelSafe(Guid guid, Vector2Int coords, Color color) {
		if(!(coords.x > 0 && coords.x < texture.width && coords.y > 0 && coords.y < texture.height)) return;
		SetPixel(coords, color);

		GraphPixel graphPixel = new GraphPixel(coords, color);
		graphCoords[guid].Add(graphPixel);
	}

	private void SetCircle(Guid guid, Color color, int xc, int yc, int x, int y) {
		SetPixelSafe(guid, new Vector2Int(xc+x, yc+y), color);
		SetPixelSafe(guid, new Vector2Int(xc-x, yc+y), color);
		SetPixelSafe(guid, new Vector2Int(xc+x, yc-y), color);
		SetPixelSafe(guid, new Vector2Int(xc-x, yc-y), color);

		SetPixelSafe(guid, new Vector2Int(xc+y, yc+x), color);
		SetPixelSafe(guid, new Vector2Int(xc-y, yc+x), color);
		SetPixelSafe(guid, new Vector2Int(xc+y, yc-x), color);
		SetPixelSafe(guid, new Vector2Int(xc-y, yc-x), color);
	}

	private void DrawLine(Guid guid, Texture2D texture, Vector2Int pos0, Vector2Int pos1, Color color) { // An implementation of Bresenham's line algorithm
		// DrawAALine(guid, texture, pos0, pos1, color);
		// return;
		
		int x = pos0.x;
		int y = pos0.y;
		int x1 = pos1.x;
		int y1 = pos1.y;

		int w = x1 - x;
		int h = y1 - y;
		int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;

		if (w<0) dx1 = -1 ; else if (w>0) dx1 = 1;
		if (h<0) dy1 = -1 ; else if (h>0) dy1 = 1;
		if (w<0) dx2 = -1 ; else if (w>0) dx2 = 1;

		int longest = Mathf.Abs(w);
		int shortest = Mathf.Abs(h);

		if (!(longest > shortest)) {
			longest = Mathf.Abs(h);
			shortest = Mathf.Abs(w);

			if (h<0) dy2 = -1; else if (h>0) dy2 = 1;

			dx2 = 0;            
		}

		int numerator = longest >> 1;

		for (int i = 0; i <= longest; i++) {
			if (y != Screen.height && y != 0) {
				// You know what? This is lame. We can do better.
				for(int c = -(thickness/2); c < thickness/2; c++) {
					Vector2Int coords = new Vector2Int(x, y + c);
					SetPixelSafe(guid, coords, color);
				}
				// No, we can't.
				// int xr = 0;
				// int yr = thickness;
				// int d = 3 - 2 * thickness;
				// SetCircle(guid, color, x, y, xr, yr);
				// while(yr >= xr) {
				// 	xr++;
				// 	if(d > 0) {
				// 		yr--;
				// 		d += 4 * (xr - yr) + 10;
				// 	} else {
				// 		d += 4 * xr + 6;
				// 	}
				// 	SetCircle(guid, color, x, y, xr, yr);
				// }
			}

			numerator += shortest;

			if (!(numerator < longest)) {
				numerator -= longest;
				x += dx1 ;
				y += dy1 ;
			} else {
				x += dx2 ;
				y += dy2 ;
			}
		}
	}

	private Vector2Int SwapVect(Vector2Int v) {
		int x = v.x;
		v.x = v.y;
		v.y = x;
		return v;
	}

	private float FractionPart(float a) {
		if(a > 0) return a - Mathf.Floor(a);
		return 1 - (a - Mathf.Floor(a));
	} 

	private int SuperFloor(float a) {
		return (int) Mathf.Floor(a);
	}

	private void DrawAALine(Guid guid, Texture2D texture, Vector2Int pos0, Vector2Int pos1, Color color) { // Fuck you, Bresenham
		bool steep = Mathf.Abs(pos1.y - pos0.y) > Mathf.Abs(pos1.x - pos0.x);

		if(steep) {
			pos0 = SwapVect(pos0);
			pos1 = SwapVect(pos1);
		}
		if(pos0.x > pos1.x) {
			int t = pos0.y;
			pos0.y = pos1.y;
			pos1.y = t;
			t = pos0.x;
			pos0.x = pos1.x;
			pos1.x = t;
		}

		float dx = pos1.x - pos0.x;
		float dy = pos1.y - pos0.y;
		float gradient = dy/dx;
		if(dx == 0) gradient = 1;

		int xpxl1 = pos0.x;
		int xpxl2 = pos1.x;
		float intersectY = pos0.y;
		// Debug.Log(gradient);

		if(steep) {
			for(int x = xpxl1; x <= xpxl2; x++) {
				Color c = color;
				c.a = 1 - FractionPart(intersectY);
				Vector2Int p = new Vector2Int(SuperFloor(intersectY), x);
				if(p.x > 0 && p.x < texture.width && p.y > 0 && p.y < texture.height) {
					SetPixel(p, c);
					GraphPixel graphPixel = new GraphPixel(p, c);
					graphCoords[guid].Add(graphPixel);
				}
				c.a = FractionPart(intersectY);
				p = new Vector2Int(SuperFloor(intersectY) - 1, x);
				if(p.x > 0 && p.x < texture.width && p.y > 0 && p.y < texture.height) {
					SetPixel(p, c);
					GraphPixel graphPixel = new GraphPixel(p, c);
					graphCoords[guid].Add(graphPixel);
				}
				intersectY += gradient;
			}
		} else {
			for(int x = xpxl1; x <= xpxl2; x++) {
				Color c = color;
				c.a = 1 - FractionPart(intersectY);
				Vector2Int p = new Vector2Int(x, SuperFloor(intersectY));
				if(p.x > 0 && p.x < texture.width && p.y > 0 && p.y < texture.height) {
					SetPixel(p, c);
					GraphPixel graphPixel = new GraphPixel(p, c);
					graphCoords[guid].Add(graphPixel);
				}
				c.a = FractionPart(intersectY);
				p = new Vector2Int(x, SuperFloor(intersectY) - 1);
				if(p.x > 0 && p.x < texture.width && p.y > 0 && p.y < texture.height) {
					SetPixel(p, c);
					GraphPixel graphPixel = new GraphPixel(p, c);
					graphCoords[guid].Add(graphPixel);
				}
				intersectY += gradient;
			}
		}
	}

	public void DrawGraphPoint(Guid guid, Vector2 pos, Color color) {
		Vector2Int surfaceCoords = WorldToSurface(pos);

		if (surfaceCoords.x > Screen.width - 1 || surfaceCoords.x < 0) return;

		if (surfaceCoords.y > Screen.height - 1 || surfaceCoords.y < 0) {
			surfaceCoords.y = Mathf.Clamp(surfaceCoords.y, 0, Screen.height - 1);
		}

		if (prevCoords.ContainsKey(guid)) {
			Vector2Int prev = prevCoords[guid];
			int dY = Mathf.Abs(surfaceCoords.y - prev.y);

			if ((surfaceCoords.y == Screen.height - 1|| surfaceCoords.y == 0) && dY == 0) {
				prevCoords[guid] = surfaceCoords;
				return;
			}

			if (dY < Screen.height * 0.75f) {
				if (!graphCoords.ContainsKey(guid)) {
					graphCoords[guid] = new List<GraphPixel>();
				}

				DrawLine(guid, texture, prev, surfaceCoords, color);
			}
		} else {
			SetPixel(surfaceCoords, color);

			graphCoords[guid] = new List<GraphPixel>();
			graphCoords[guid].Add(new GraphPixel(surfaceCoords, color));
		}

		prevCoords[guid] = surfaceCoords;
	}

	public void ClearHistory(Guid guid) {
		prevCoords.Remove(guid);
	}

	public void InitGraph() {
		texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGBA32, true);
		textureData = new Color[Screen.height * Screen.width];

		for (int p = 0; p < Screen.height * Screen.width; p++) {
			textureData[p] = new Color(0, 0, 0, 0);
		}

		texture.SetPixels(textureData);
		texture.Apply(true);

		Sprite sprite = Sprite.Create(texture, new Rect(0, 0, Screen.width, Screen.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
		GetComponent<SpriteRenderer>().sprite = sprite;
	}

	private void FadeGraphs() {
		foreach (Guid guid in new List<Guid>(graphCoords.Keys)) {
			List<GraphPixel> pixels = graphCoords[guid];
			List<GraphPixel> pixelCopy = new List<GraphPixel>(pixels);

			for (int i = 0; i < pixels.Count; i++) {
				GraphPixel pixel = pixels[i];
				Color curColor;

				pixels[i].time += Time.deltaTime;

				if (pixels[i].time >= fadeTime) {
					curColor = new Color(0, 0, 0, 0);
					pixelCopy.Remove(pixel);

					if (pixelCopy.Count == 0) {
						break;
					}
				} else {
					curColor = Color.Lerp(pixel.color, new Color(pixel.color.r, pixel.color.g, pixel.color.b, 0), pixel.time / fadeTime);
				}
				
				SetPixel(pixel.coords, curColor);
			}

			if (pixelCopy.Count == 0) {
				graphCoords.Remove(guid);
			} else {
				graphCoords[guid] = pixelCopy;
			}
		}
	}

	private void Start() {
		pixelsPerUnit = Screen.height / (Camera.main.orthographicSize * 2);

		InitGraph();
	}

	private void Update() {
		FadeGraphs();
	}

	private void LateUpdate() {
		if (updateTexture) {
			texture.SetPixels(textureData);
			texture.Apply(true);
			updateTexture = false;
		}
	}
}
