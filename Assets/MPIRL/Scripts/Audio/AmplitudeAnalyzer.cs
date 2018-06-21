using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmplitudeAnalyzer : MonoBehaviour {
  public AudioSource[] sources;

  public float amplitudeFactor = 15.0f;
  
  private float multiplier = 0.0f;

  private float[] samples = null;
  private readonly int numSamples = 512;

  void Awake() {
    samples = new float[numSamples];

    float divider = 0.0f;
    for (int i = 0; i < sources.Length; ++i) {
      divider += sources[i].volume;
    }
    multiplier = divider > 0.0f ? amplitudeFactor / (numSamples * divider) : 0.0f;
  }

  public float GetAmplitude() {
    float amplitude = 0.0f;
    for (int i = 0; i < sources.Length; ++i) {
      if (sources[i].isPlaying) {
        sources[i].GetOutputData(samples, 0);
        for (int n = 0; n < numSamples; ++n) {
          amplitude += Mathf.Abs(samples[n]);
        }
      }
    }
    return amplitude * multiplier;
  }
}
