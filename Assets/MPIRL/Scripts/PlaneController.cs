using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class PlaneController : MonoBehaviour {
  public BoxCollider boxCollider;
  public MeshRenderer colliderRenderer;

  public float colliderHeight = 0.02f;
  public float scaleSpeed = 8.0f;
  public float idleAlpha = 0.5f;

  private DetectedPlane detectedPlane;
  private Anchor anchor;

  void Awake() {
    transform.localScale = Vector3.zero;
  }

  void Update() {
    if (detectedPlane == null) {
      return;
    }

    if (detectedPlane.SubsumedBy != null) {
      GameObject.Destroy(anchor.gameObject);
      return;
    }
    
    if (detectedPlane.TrackingState != TrackingState.Tracking) {
      boxCollider.enabled = false;
      colliderRenderer.enabled = false;
      return;
    }

    boxCollider.enabled = true;
    colliderRenderer.enabled = true;

    var targetScale = new Vector3(0.5f * detectedPlane.ExtentX, colliderHeight, 0.5f * detectedPlane.ExtentZ);
    transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);
	}

  public void Initialize(DetectedPlane plane) {
    colliderRenderer.material.color = ColorGenerator.Generate(idleAlpha);

    detectedPlane = plane;
    anchor = detectedPlane.CreateAnchor(detectedPlane.CenterPose);
    transform.parent = anchor.transform;
    transform.localPosition = -0.5f * colliderHeight * Vector3.up;
    transform.localRotation = Quaternion.identity;
  }
}
