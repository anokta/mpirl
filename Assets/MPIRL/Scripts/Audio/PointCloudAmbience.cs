using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class PointCloudAmbience : MonoBehaviour {
  public AudioSource layer1, layer2;

  private float gain1 = 0.0f, gain2 = 0.0f;

  private float rampSpeed = 4.0f;

  void Awake() {
    layer1.volume = gain1;
    layer2.volume = gain2;
  }
  
  void OnEnable() {
    Sequencer.OnNextBeat += OnNextBeat;
  }
  
  void OnDisable() {
    Sequencer.OnNextBeat -= OnNextBeat;
  }

  void Update() {
    int numPoints = Frame.PointCloud.PointCount;
    gain1 = 0.05f + numPoints > 0 ? 0.2f + 0.25f * numPoints / 75.0f : 0.0f;
    gain2 = numPoints > 10 ? 0.25f * numPoints / 150.0f : 0.0f;

    if (Mathf.Abs(layer1.volume - gain1) > 0.01f) {
      layer1.volume = Mathf.Min(0.5f, Mathf.Lerp(layer1.volume, gain1, rampSpeed * Time.deltaTime));
    }
    if (Mathf.Abs(layer2.volume - gain2) > 0.01f) {
      layer2.volume = Mathf.Min(0.5f, Mathf.Lerp(layer2.volume, gain2, rampSpeed * Time.deltaTime));
    }
  }

  private void OnNextBeat(int section, int bar, int beat, double dspTime) {
    layer1.PlayScheduled(dspTime);
    layer2.PlayScheduled(dspTime);
    Sequencer.OnNextBeat -= OnNextBeat;
  }
}
