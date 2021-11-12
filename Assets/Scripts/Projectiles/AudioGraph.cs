using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioGraph : MonoBehaviour
{
	public int baseFreq = 1000;
	public int freqOffsetPerUnit = 40;
	public int samplerate = 32000;
	private AudioSource audioSource;
	private float length;
	private LUT lut;
	private float surfaceWidth;
	private int curReadPosition;
	public bool playing = false;
	private AudioClip audioClip;
	private float halfSurfaceWidth;
	private bool funcComplex;

	private static float RangeToNeg1To1(float value, float min, float max) {
		float clamped = Mathf.Clamp(value, min, max);
		float zeroToOne = (clamped - min) / (max - min);

		return 2 * zeroToOne - 1;
	}

	private void OnAudioRead(float[] data) {
		curReadPosition += data.Length;

		float preCalc1 = surfaceWidth / length;

		for (int i = 0; i < data.Length; i++) {
			int curSample = curReadPosition + i;

			float curClipTime = curSample / (float)samplerate;
			float curUnit = curClipTime * preCalc1 - halfSurfaceWidth;

			float y;

			if (funcComplex) {
				y = lut.ValueAt(curUnit);
			} else {
				y = lut.m_func.Process(curUnit);
			}

			int freqOffset = Mathf.RoundToInt(RangeToNeg1To1(y, -10, 10) * 500);
			
			int freq = baseFreq + freqOffset;
			int funcX = (freq * curSample) / samplerate;
			float raw = lut.m_func.Process(funcX + Mathf.Epsilon);

			data[i] = RangeToNeg1To1(raw, -10, 10);
		}
	}

	private void OnAudioSetPosition(int newPosition) {
		curReadPosition = newPosition;
	}

	private AudioClip GenerateClip() {
		AudioClip ac = AudioClip.Create("wave", Mathf.RoundToInt(samplerate * length), 1, samplerate, true, OnAudioRead, OnAudioSetPosition);

		return ac;
	}

	public void PlayGraph(LUT movelut, float moveTime, bool muted = false) {
		playing = true;
		audioSource = gameObject.GetComponent<AudioSource>();
		length = moveTime;
		lut = movelut;
		surfaceWidth = GameObject.FindGameObjectWithTag("GraphSurface").GetComponent<GraphSurface>().surfaceWidth;
		halfSurfaceWidth = surfaceWidth / 2f;

		funcComplex = lut.EstimateComplexity() < 1f;

		float offset = 0;
		int freq = Mathf.RoundToInt(baseFreq + offset * freqOffsetPerUnit);
		audioClip = GenerateClip();

		audioSource.volume = muted ? 0 : 0.3f;
		audioSource.PlayOneShot(audioClip);
	}

	public void SetVolume(float vol) {
		audioSource.volume = vol;
	}

	public void SetPan(float pan) {
		// audioSource.panStereo = pan;
	}

	private void OnDestroy() {
		AudioClip.Destroy(audioClip);
	}
}
