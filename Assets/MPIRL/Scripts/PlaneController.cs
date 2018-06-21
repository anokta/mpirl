using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class PlaneController : MonoBehaviour {
  public BoxCollider boxCollider;

  public MeshRenderer visualRenderer;
  public PulseVisualizer pulseVisualizer;

  public AmplitudeAnalyzer amplitudeAnalyzer;

  public Instrument instrument;

  [HideInInspector]
  public Generator generator = null;
  
  public float amplitudeReactSpeed = 2.5f;
  public float colliderHeight = 0.02f;
  public float scaleSpeed = 8.0f;
  public float idleAlpha = 0.5f;
  public float hitCoolTime = 0.1f;

  private DetectedPlane detectedPlane;
  private Anchor anchor;

  private BarelyAPI.MarkovChain hitNoteGenerator;
  private float lastHitTime = 0.0f;

  private Color mainColor;

  void Awake() {
    transform.localScale = Vector3.zero;
    hitNoteGenerator = new BarelyAPI.MarkovChain(8, 1);
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
      visualRenderer.enabled = false;
      return;
    }

    boxCollider.enabled = true;
    visualRenderer.enabled = true;

    var targetScale = new Vector3(0.5f * detectedPlane.ExtentX, colliderHeight, 0.5f * detectedPlane.ExtentZ);
    transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);

    float colorMultiplier = Mathf.Min(2.0f, Mathf.Max(idleAlpha, amplitudeAnalyzer.GetAmplitude()));
    visualRenderer.material.color = Color.Lerp(visualRenderer.material.color, colorMultiplier * mainColor, 
                                               amplitudeReactSpeed * Time.deltaTime);
    if (colorMultiplier > idleAlpha) {
      pulseVisualizer.Pulse();
    }
	}

  public void Initialize(DetectedPlane plane) {
    mainColor = ColorGenerator.Generate(idleAlpha);
    visualRenderer.material.color = idleAlpha * mainColor;

    detectedPlane = plane;
    anchor = detectedPlane.CreateAnchor(detectedPlane.CenterPose);
    transform.parent = anchor.transform;
    transform.localPosition = -0.5f * colliderHeight * Vector3.up;
    transform.localRotation = Quaternion.identity;
  }

  void OnCollisionEnter(Collision collision) {
    if (Time.time - lastHitTime > hitCoolTime && collision.transform.tag == "Ball") {
      collision.transform.GetComponent<Renderer>().material.color = visualRenderer.material.color;

      int noteIndex = hitNoteGenerator.CurrentState;
      float noteVolume = 0.15f * collision.relativeVelocity.magnitude;
      instrument.PlayInteractable(noteIndex, noteVolume);

      hitNoteGenerator.GenerateNextState();
      lastHitTime = Time.time;
    }
  }
}
