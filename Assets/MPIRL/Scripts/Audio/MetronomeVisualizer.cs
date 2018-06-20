using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetronomeVisualizer : MonoBehaviour {
  public float scaleMultiplier = 1.1f;

  private Vector3 initialScale;

  private void Awake() {
    initialScale = transform.localScale;
  }

  void OnEnable() {
    Sequencer.OnNextBeat += OnNextBeat; 
  }

  void OnDisable() {
    Sequencer.OnNextBeat -= OnNextBeat;
  }

  private void Update() {
    if (Vector3.Distance(transform.localScale, initialScale) > 0.002f) {
      transform.localScale = Vector3.Lerp(transform.localScale, initialScale, 2.0f * Time.deltaTime);
    }
  }

  private void OnNextBeat(int section, int bar, int beat, double dspTime, double beatTime) {
    Invoke("Pulse", (float)(beatTime - AudioSettings.dspTime));
  }

  private void Pulse() {
    transform.localScale = initialScale * scaleMultiplier;
  }
}
