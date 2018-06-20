using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Generator : MonoBehaviour {
  public Instrument instrument;

  protected virtual void Awake() {
    instrument = GetComponent<Instrument>();
  }

  protected void OnEnable() {
    Sequencer.OnNextBeat += OnNextBeat;    
  }

   protected void OnDisable() {
    Sequencer.OnNextBeat -= OnNextBeat;    
  }

  protected abstract void OnNextBeat(int section, int bar, int beat, double dspTime, double beatTime);
}
