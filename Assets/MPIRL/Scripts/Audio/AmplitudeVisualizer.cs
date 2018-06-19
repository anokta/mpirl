using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmplitudeVisualizer : MonoBehaviour {
  public MeshRenderer meshRenderer;

  public float reactSpeed = 4.0f;
  
  private float amplitude = 0.0f;

  private float initialAlpha = 0.0f;

  void Awake() {
    initialAlpha = meshRenderer.material.color.a;
  }

  void Update () {
    float targetAlpha = amplitude > initialAlpha ? amplitude : initialAlpha;
    Color c = meshRenderer.material.color;
    c.a = Mathf.Lerp(c.a, targetAlpha, reactSpeed * Time.deltaTime);
    meshRenderer.material.color = c;
	}

  void OnAudioFilterRead(float[] data, int channels) {
    float total = 0.0f;
    for (int i = 0; i < data.Length; ++i) {
      total += data[i];
    }
    amplitude = total;
  }
}
