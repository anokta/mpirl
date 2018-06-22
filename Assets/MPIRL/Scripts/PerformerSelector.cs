using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformerSelector : MonoBehaviour {
  public PerformerContainer drums;

  public PerformerContainer bass;

  public PerformerContainer melody;

  public static int numPerformers = 0;
  private int numDrums = 0, numBass = 0, numMelody = 0;

  private void Awake() {
    drums.Initialize();
    bass.Initialize();
    melody.Initialize();
  }

  public PerformerType GetNextPerformerType() {
    float drumP = 0.4f / (0.5f * numDrums + 1);
    float bassP = 0.1f / (4.0f * numBass + 1);
    float melodyP = 0.5f / (0.5f * numMelody + 1);
    float p = BarelyAPI.RandomNumber.NextFloat(0.0f, bassP + drumP + melodyP);
    
    ++numPerformers;
    if (p < drumP) {
      ++numDrums;
      return drums.GetPerformerType();
    } else if (p < drumP + bassP) {
      ++numBass;
      return bass.GetPerformerType();
    } else {
      ++numMelody;
      return melody.GetPerformerType();
    }
  }
}
