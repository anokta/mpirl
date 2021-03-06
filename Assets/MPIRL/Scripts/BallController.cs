﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {
  public AudioSource source;

  public Rigidbody rigidBody;
  
  public float destroyTimeout = 2.5f;
  public float noiseSpeed = 5.0f;
  public float maxNoiseVolume = 0.075f;

  private float initTime = 0.0f;

  private bool destroying = false;
  private float destroySpeed = 16.0f;

  void Start () {
    ++GameManager.numBalls;
  
    initTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
    source.volume = Mathf.Min(maxNoiseVolume, 
                              Mathf.Lerp(source.volume, transform.localScale.sqrMagnitude * rigidBody.velocity.magnitude, 
                              noiseSpeed * Time.deltaTime));
    if (destroying) {
      transform.localScale = 
        Vector3.Lerp(transform.localScale, Vector3.zero, destroySpeed * Time.deltaTime);
      if (transform.localScale.magnitude < 0.005f) {
        --GameManager.numBalls;
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
