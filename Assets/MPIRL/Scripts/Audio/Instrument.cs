using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instrument : MonoBehaviour {
  public AudioSource main, interactable;

  public int noteOffset = 0;

  public readonly int scaleLength = Scale.majorScale.Length;

  public void PlayNote(double dspTime, int index, float volume = 1.0f) {
      main.pitch = GetPitch(index);
      main.volume = volume;
      main.PlayScheduled(dspTime);
  }

  public void PlayInteractable(int index, float volume = 1.0f) {
    interactable.pitch = GetPitch(index);
    interactable.volume = volume;
    interactable.PlayOneShot(main.clip, volume);
  }

  public void SetSample(AudioClip clip) {
    main.clip = clip;
  }

  private float GetPitch(int index) {
    int noteIndex = noteOffset + index;
    int octaveShift = Mathf.FloorToInt((float)noteIndex / scaleLength);
    int offset = (noteIndex + 4 * scaleLength) % scaleLength;
    return Mathf.Pow(2.0f, octaveShift) * Scale.majorScale[offset];
  }
}
