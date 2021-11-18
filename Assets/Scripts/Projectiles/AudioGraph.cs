using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioLowPassFilter))]
public class AudioGraph : MonoBehaviour
{
	public int baseFreq = 1000;
	public int freqOffsetPerUnit = 40;
	public int samplerate = 44100;
	public float fadeTime = 1f;
	private AudioSource audioSource;
	private float length;
	private LUT lut;
	private float surfaceWidth;
	private int curReadPosition;
	public bool playing = false;
	private AudioClip audioClip;
	private float halfSurfaceWidth;
	private bool funcComplex;
	private bool fadeActive = false;
	private float fadeTimer;
	private float volume;
	private AudioLowPassFilter lowPassFilter;

	private static float RangeToNeg1To1(float value, float min, float max)
	{
		float clamped = Mathf.Clamp(value, min, max);
		float zeroToOne = (clamped - min) / (max - min);

		return 2 * zeroToOne - 1;
	}

	private void OnAudioFilterRead(float[] data, int channels)
	{
		if (lut == null) return;
		curReadPosition += data.Length;

		float preCalc1 = surfaceWidth / (length - fadeTime);

		for (int i = 0; i < data.Length; i++)
		{
			int curSample = curReadPosition + i;

			float curClipTime = curSample / (float)samplerate;
			float curUnit = curClipTime * preCalc1 - halfSurfaceWidth;

			float y;

			y = lut.ValueAt(Mathf.Clamp(curUnit, -halfSurfaceWidth, halfSurfaceWidth));

			// if (funcComplex) {
			// 	y = lut.ValueAt(curUnit);
			// } else {
			// 	y = lut.m_func.Process(curUnit);
			// }

			int freqOffset = Mathf.RoundToInt(RangeToNeg1To1(y, -10, 10) * 500);

			int freq = baseFreq + freqOffset;
			int funcX = (freq * curSample) / samplerate;
			float raw = lut.m_func.Process(funcX);

			if (float.IsNaN(raw))
			{
				raw = 0;
			}

			float scaled = RangeToNeg1To1(raw, -10, 10);

			data[i] = scaled;

			if (channels == 2)
			{
				data[i + 1] = data[i];
				i++;
			}
		}
	}

	private void OnAudioSetPosition(int newPosition)
	{
		curReadPosition = newPosition;
	}

	// private AudioClip GenerateClip()
	// {
	// 	AudioClip ac = AudioClip.Create("wave", Mathf.RoundToInt(samplerate * length), 1, samplerate, true, OnAudioRead, OnAudioSetPosition);

	// 	return ac;
	// }

	public void PlayGraph(LUT movelut, float moveTime, bool play = true)
	{
		playing = true;
		audioSource = gameObject.GetComponent<AudioSource>();
		length = moveTime + fadeTime;
		lut = movelut;
		surfaceWidth = GameObject.FindGameObjectWithTag("GraphSurface").GetComponent<GraphSurface>().surfaceWidth;
		halfSurfaceWidth = surfaceWidth / 2f;

		funcComplex = lut.EstimateComplexity() < 1f;

		float offset = 0;
		int freq = Mathf.RoundToInt(baseFreq + offset * freqOffsetPerUnit);
		// audioClip = GenerateClip();

		audioSource.volume = volume;
		audioSource.mute = !play;
		// audioSource.PlayOneShot(audioClip);
	}

	public void Mute()
	{
		audioSource.mute = true;
	}

	public void Unmute()
	{
		audioSource.mute = false;
	}

	public void SetPan(float pan)
	{
		audioSource.panStereo = pan;
	}

	public void Fade()
	{
		fadeTimer = fadeTime;
		fadeActive = true;
	}

	private void Awake()
	{
		samplerate = AudioSettings.outputSampleRate;

		lowPassFilter = GetComponent<AudioLowPassFilter>();
		volume = transform.parent.GetComponent<AudioGraphContainer>().volume;
	}

	private void FixedUpdate()
	{
		if (fadeActive)
		{
			fadeTimer -= Time.fixedDeltaTime;

			if (fadeTimer <= 0)
			{
				Destroy(gameObject);
			}
			else
			{
				audioSource.volume = volume * (fadeTimer / fadeTime);
			}
		}
	}

	private void OnDestroy()
	{
		// AudioClip.Destroy(audioClip);
	}

	public void Play()
	{
		audioSource.UnPause();
	}

	public void Pause()
	{
		audioSource.Pause();
	}
}
