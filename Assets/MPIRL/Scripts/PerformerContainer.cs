using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PerformerType {
  public AudioClip sample;
  public string generatorName;
  public float probability;
}

[System.Serializable]
public struct PerformerContainer {
  public int numLastPerformers;

  public PerformerType[] performers;
  
  private List<int> availablePerformers;
  private List<int> lastUsedPerformers;

  private float totalProbability;

  public void Initialize() {      
    availablePerformers = new List<int>();
    lastUsedPerformers = new List<int>();
    totalProbability = 0.0f;
    for (int i = 0; i < performers.Length; ++i) {
      availablePerformers.Add(i);
      totalProbability += performers[i].probability;
    }
  }

  public PerformerType GetPerformerType() {
    float p = BarelyAPI.RandomNumber.NextFloat(0.0f, totalProbability);
    float cumulative = 0.0f;
    int performerIndex = 0;
    for (int i = 0; i < availablePerformers.Count; ++i) {
      int index = availablePerformers[i];
      cumulative += performers[index].probability;
      if (p < cumulative) {
        performerIndex = index;
        break;
      }
    }

    var performerType = performers[performerIndex];
    totalProbability -= performerType.probability;
    availablePerformers.Remove(performerIndex);

    lastUsedPerformers.Add(performerIndex);
    if (lastUsedPerformers.Count > numLastPerformers) {
      int freeIndex = lastUsedPerformers[0];
      totalProbability += performers[freeIndex].probability;
      availablePerformers.Add(freeIndex);
      lastUsedPerformers.RemoveAt(0);
    }

    return performerType;
  }
}
