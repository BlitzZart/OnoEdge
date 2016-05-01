using UnityEngine;
using System.Collections;
using System;
using UnityStandardAssets.ImageEffects;
using System.Collections.Generic;
using UnityEngine.UI;

public class SetQuality : MonoBehaviour {

    private Antialiasing antiAliasing;
    private VignetteAndChromaticAberration vignetteCroma;

    private List<MonoBehaviour> effects;

    enum Quality {
        Low = 0, High = 1
    }

    private Quality current;


    void Start() {
        current = Quality.Low;

        effects = new List<MonoBehaviour>();
        effects.Add(Camera.main.GetComponent<Antialiasing>());
        effects.Add(Camera.main.GetComponent<VignetteAndChromaticAberration>());

    }

    public void Toggle() {
        current = (Quality)((int)++current % Enum.GetNames(typeof(Quality)).Length);

        SwitchQuality();
    }

    public void Toggle(Text text) {
        Toggle();
        text.text = current.ToString();
    }

    private void SwitchQuality() {
        switch (current) {

            case Quality.Low:
                ApplyQuality(false, false);
                break;
            case Quality.High:
                ApplyQuality(false, true);
                break;
            default:
                break;
        }
    }
    // 0 = AA, 1 = vignetteAndChromatic
    private void ApplyQuality(params bool[] values) {
        for (int i = 0; i < values.Length; i++) {
            if (effects[i] != null) {
                effects[i].enabled = values[i];
            }
        }
    }

}
