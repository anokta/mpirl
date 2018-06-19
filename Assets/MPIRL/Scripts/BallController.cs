﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {
  public AudioSource source;
  
  public float destroyTimeout = 2.5f;

  private float initTime = 0.0f;

  private bool destroying = false;
  private float destroySpeed = 16.0f;

  void Awake() {
    source = GetComponent<AudioSource>();
  }

  void Start () {
    initTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
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

  void OnCollisionEnter(Collision collision) {
    // TEST // 
    if (collision.transform.tag == "Plane") {
      GetComponent<Renderer>().material.color = collision.transform.GetComponent<Renderer>().material.color;

    var generator = collision.transform.GetComponent<StaticBarGenerator>();

    int index = generator.noteOffset + Random.Range(0, 8);
    int octaveShift = Mathf.FloorToInt((float)index / 8.0f);
    int offset = (index + 32) % Scale.majorScale.Length;
    source.pitch = Mathf.Pow(2.0f, octaveShift) * Scale.majorScale[offset];
    source.PlayOneShot(generator.source.clip);

      
    }
  }

  void OnCollisionExit(Collision collision) {
    initTime = Time.time;
  }
}
