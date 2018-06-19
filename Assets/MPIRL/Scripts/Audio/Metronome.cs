using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metronome : MonoBehaviour {
  public Sequencer sequencer;

  private AudioSource source;

  private void Awake() {
    source = GetComponent<AudioSource>();
  }

  void OnEnable() {
    sequencer.OnNextBeat += OnNextBeat; 
  }

  void OnDisable() {
    sequencer.OnNextBeat -= OnNextBeat;
  }

  private void OnNextBeat(int section, int bar, int beat, double dspTime) {
    //source.Stop();
    source.pitch = beat == 0 ? 2.0f : 1.0f;
    source.PlayScheduled(dspTime);
  }
}
