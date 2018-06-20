using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {
  public AudioSource source;

  public Rigidbody rigidBody;
  
  public float destroyTimeout = 2.5f;

  private float initTime = 0.0f;

  private bool destroying = false;
  private float destroySpeed = 16.0f;
  
  void Start () {
    initTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
    source.volume = 
      Mathf.Min(0.2f, Mathf.Lerp(source.volume, transform.localScale.sqrMagnitude * rigidBody.velocity.magnitude, 6.0f * Time.deltaTime));
    if (destroying) {
      transform.localScale = 
        Vector3.Lerp(transform.localScale, Vector3.zero, destroySpeed * Time.deltaTime);
      if (transform.localScale.magnitude < 0.005f) {
        GameObject.Destroy(gameObject);
      }
      return;
    }

    if (Time.time - initTime > destroyTimeout) {
      destroying = true;
      return;
    }
	}

  void OnCollisionExit(Collision collision) {
    initTime = Time.time;
  }
}
