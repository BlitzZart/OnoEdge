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
        text.text = "Quality: " + current.ToString();
    }

    private void SwitchQuality() {
        switch (current) {

            case Quality.Low: // set lowest quality level
                ApplyQuality(0, 0, 0);
                break;
            case Quality.High: // set highest quality level
                ApplyQuality(1, 1, QualitySettings.names.Length - 1);
                break;
            default:
                break;
        }
    }

    // 0 = AA as bool (0/1), 1 = vignetteAndChromatic as bool (0/1), 2 = QualityLevel as int
    // last parameter is defindes QualitySetting (int value)
    private void ApplyQuality(params int[] values) {
        for (int i = 0; i < effects.Count; i++) {
            if (effects[i] != null) {
                effects[i].enabled = Convert.ToBoolean(values[i]);
            }
        }
        QualitySettings.SetQualityLevel(values[values.Length - 1], true);
    }

}
