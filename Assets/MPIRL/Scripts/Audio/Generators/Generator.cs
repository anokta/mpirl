using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Generator : MonoBehaviour {
  protected void OnEnable() {
    Sequencer.OnNextBeat += OnNextBeat;    
  }
  
  protected void OnDisable() {
    Sequencer.OnNextBeat -= OnNextBeat;    
  }

  protected abstract void OnNextBeat(int section, int bar, int beat, double dspTime, double beatTime);
  protected abstract void PlayNote(int noteIndex, double dspTime);
}
