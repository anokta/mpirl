using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class PlaneController : MonoBehaviour {
  public float colliderHeight = 0.025f;

  private DetectedPlane detectedPlane;

  private BoxCollider boxCollider;
  private MeshRenderer colliderRenderer;

  private Anchor anchor;

  private Vector3 targetScale;

  void Awake() {
    colliderRenderer = GetComponentInChildren<MeshRenderer>();
    boxCollider = GetComponent<BoxCollider>();
    transform.localScale = Vector3.zero;
  }

  public void Initialize(DetectedPlane plane) {
    detectedPlane = plane;
    anchor = detectedPlane.CreateAnchor(detectedPlane.CenterPose);
    transform.parent = anchor.transform;
    transform.localPosition = -0.25f * colliderHeight * Vector3.up;
    transform.localRotation = Quaternion.identity;
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

    targetScale = 0.5f *  new Vector3(detectedPlane.ExtentX, colliderHeight, detectedPlane.ExtentZ);
    transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * 8.0f);
	}
}
