using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualObjectsRenderer : MonoBehaviour {
  public Camera mainCamera;
  public Camera rendererCamera;

  public RenderTexture rendererTexture;

  private Rect rendererWindow;

  void Start() {
    int width = Screen.width;
    int height = Screen.height;
    rendererWindow = new Rect(0.0f, 0.0f, width, height);
    rendererTexture.width = width;
    rendererTexture.height = height;
    rendererCamera.targetTexture = rendererTexture;
  }

  void Update() {
    rendererCamera.projectionMatrix = mainCamera.projectionMatrix;
  }

  void OnGUI() {
    GUI.DrawTexture(rendererWindow, rendererTexture);
  }
}
