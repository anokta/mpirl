﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {
  public float timeout = 2.5f;

  private AudioSource source;

  private float initTime = 0.0f;

  private bool destroying = false;
  private float destroySpeed = 16.0f;

  private int noteOffset;

  private void Awake() {
    source = GetComponent<AudioSource>();
    noteOffset = Random.Range(-8, 8);
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

    if (Time.time - initTime > timeout) {
      destroying = true;
      return;
    }
	}

  private void OnCollisionEnter(Collision collision) {
    int index = noteOffset + Random.Range(0, 8);
    int octaveShift = Mathf.FloorToInt((float)index / 8.0f);
    int offset = (index + 32) % Scale.majorScale.Length;
    source.Stop();
    source.pitch = Mathf.Pow(2.0f, octaveShift) * Scale.majorScale[offset];
    source.Play();
  }

  void OnCollisionExit(Collision collision) {
    initTime = Time.time;
  }
}
