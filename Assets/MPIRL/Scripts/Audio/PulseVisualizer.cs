using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseVisualizer : MonoBehaviour {
  public float scaleMultiplier = 1.1f;
  public float scaleSpeed = 1.5f;

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

  void Update() {
    if (Vector3.Distance(transform.localScale, initialScale) > 0.002f) {
      transform.localScale = Vector3.Lerp(transform.localScale, initialScale, scaleSpeed * Time.deltaTime);
    }
  }

  public void Pulse() {
    transform.localScale = initialScale * scaleMultiplier;
  }

  private void OnNextBeat(int section, int bar, int beat, double dspTime, double beatTime) {
    Invoke("Pulse", (float)(dspTime - AudioSettings.dspTime));
  }
}
