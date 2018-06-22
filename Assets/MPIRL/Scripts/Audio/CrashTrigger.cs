using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashTrigger : MonoBehaviour {
  public AudioSource source;
  //public AudioClip[] crash;

  private SectionType sectionType = SectionType.NONE;

  //private int index = 0;

    void OnEnable() {
    Sequencer.OnNextBeat += OnNextBeat;
  }
  
  void OnDisable() {
    Sequencer.OnNextBeat -= OnNextBeat;
  }
  
  private void OnNextBeat(int section, int bar, int beat, double dspTime, double beatTime) {
    if (bar == 0 && beat == 0) {
      sectionType = SongStructure.GetSection(section);
      if (sectionType == SectionType.CHORUS && PerformerSelector.numPerformers > 5) {
      //source.clip = crash[index];
      source.PlayScheduled(dspTime);
      //index = (index + 1) % crash.Length;
      }
    }
  }

}
