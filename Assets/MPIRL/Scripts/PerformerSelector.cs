using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformerSelector : MonoBehaviour {
  public PerformerContainer drums;

  public PerformerContainer bass;

  public PerformerContainer melody;

  // TODO: GameManager should handle this.
  private int numDrums, numBass, numMelody;

  private void Awake() {
    numDrums = 0;
    numBass = 0;
    numMelody = 0;
    
    drums.Initialize();
    bass.Initialize();
    melody.Initialize();
  }

  public PerformerType GetNextPerformerType() {
    // TODO: Use numbers to calculate relative p.
    float p = BarelyAPI.RandomNumber.NextFloat();
    if (p < 0.35f) {
      ++numDrums;
      return drums.GetPerformerType();
    } else if (p < 0.6f) {
      ++numBass;
      return bass.GetPerformerType();
    } else {
      ++numMelody;
      return melody.GetPerformerType();
    }
  }
}
