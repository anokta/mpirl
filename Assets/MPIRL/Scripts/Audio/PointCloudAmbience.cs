using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class PointCloudAmbience : MonoBehaviour {
  public float maxVolume = 0.4f;
  public float rampSpeed = 2.0f;

  public AudioSource layer1Left, layer1Right, layer2a, layer2b;

  private float gain1 = 0.0f, gain2a = 0.0f, gain2b = 0.0f;


  void OnEnable() {
    Sequencer.OnNextBeat += OnNextBeat;
  }
  
  void OnDisable() {
    Sequencer.OnNextBeat -= OnNextBeat;
  }

  void Update() {
    if (!layer1Left.isPlaying) {
      // Playback hasn't started yet.
      return;
    }

    int numPoints = Frame.PointCloud.PointCount;
    gain1 = 0.01f + numPoints > 0 ? 0.015f + 0.1f * numPoints / 125.0f : 0.0f;
    gain2a = numPoints > 25 ? 0.1f * numPoints / 250.0f : 0.0f;
    gain2b = numPoints > 50 ? 0.1f * numPoints / 500.0f : 0.0f;

    if (Mathf.Abs(layer1Left.volume - gain1) > 0.01f) {
      layer1Left.volume = Mathf.Min(maxVolume, Mathf.Lerp(layer1Left.volume, gain1, rampSpeed * Time.deltaTime));
      layer1Right.volume = Mathf.Min(maxVolume, Mathf.Lerp(layer1Right.volume, gain1, rampSpeed * Time.deltaTime));
    }
    if (Mathf.Abs(layer2a.volume - gain2a) > 0.01f) {
      layer2a.volume = Mathf.Min(maxVolume, Mathf.Lerp(layer2a.volume, gain2a, rampSpeed * Time.deltaTime));
    }
    if (Mathf.Abs(layer2b.volume - gain2b) > 0.01f) {
      layer2b.volume = Mathf.Min(maxVolume, Mathf.Lerp(layer2b.volume, gain2b, rampSpeed * Time.deltaTime));
    }
  }

  private void OnNextBeat(int section, int bar, int beat, double dspTime, double beatTime) {
    layer1Left.PlayScheduled(dspTime);
    layer1Right.PlayScheduled(dspTime);
    layer2a.PlayScheduled(dspTime);
    layer2b.PlayScheduled(dspTime);
    Sequencer.OnNextBeat -= OnNextBeat;
  }
}
