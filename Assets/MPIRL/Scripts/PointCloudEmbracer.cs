using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Yes, embracing is what this is doing.
public class PointCloudEmbracer : MonoBehaviour {
  public MeshRenderer pointCloudRenderer;

  public float idleSize = 4.0f;
  public float embraceSpeed = 2.5f;

  private float currentSize;
  private float targetSize;

  void Awake() {
    currentSize = idleSize;
    targetSize = idleSize;
  }

  void OnEnable() {
    Sequencer.OnNextBeat += OnNextBeat;
  }
  
  void OnDisable() {
    Sequencer.OnNextBeat -= OnNextBeat;
  }

  void Update() {
    if (Mathf.Abs(targetSize - currentSize) > 0.005f) {
      currentSize = Mathf.Lerp(currentSize, targetSize, embraceSpeed * Time.deltaTime);
      pointCloudRenderer.material.SetFloat("_PointSize", currentSize);
    }
  }

  private void OnNextBeat(int section, int bar, int beat, double dspTime, double beatTime) {
    targetSize = beat % 2 == 0 ? (bar % 2 == 0 ? idleSize * 1.2f : idleSize * 1.15f) : idleSize * 0.85f;
  }
}
