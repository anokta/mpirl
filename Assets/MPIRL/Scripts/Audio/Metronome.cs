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
    Sequencer.OnNextBeat += OnNextBeat; 
  }

  void OnDisable() {
    Sequencer.OnNextBeat -= OnNextBeat;
  }

  private void Update() {
    if (Input.GetKeyDown(KeyCode.Space)) {
      if (sequencer.IsPlaying) {
        sequencer.Stop();
      } else {
        sequencer.Play(AudioSettings.dspTime);
      }
    }
  }

  private void OnNextBeat(int section, int bar, int beat, double dspTime, double beatTime) {
    source.Stop();
    source.pitch = beat == 0 ? 2.0f : 1.0f;
    source.PlayScheduled(dspTime);
  }
}
