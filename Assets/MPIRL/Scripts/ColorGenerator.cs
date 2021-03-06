﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorGenerator {
  private static readonly float minHue = 0.0f;
  private static readonly float maxHue = 1.0f;
  private static readonly float minSaturation = 0.75f;
  private static readonly float maxSaturation = 1.0f;
  private static readonly float minValue = 0.75f;
  private static readonly float maxValue = 1.0f;

  public static Color Generate() {
    return Random.ColorHSV(minHue, maxHue, minSaturation, maxSaturation, minValue, maxValue);
  }
}

