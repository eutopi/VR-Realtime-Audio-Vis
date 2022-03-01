using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowSampleRate : MonoBehaviour
{
    public GameObject spectrogramGenerator;
    public Text sampleRateText;
    private SpectrogramGenerator spectrogramScript;
    // Start is called before the first frame update
    void Start()
    {
        spectrogramScript = spectrogramGenerator.GetComponent<SpectrogramGenerator>();
        sampleRateText = GetComponent<Text>();
        sampleRateText.text = "X: " + (float)AudioSettings.outputSampleRate / 2f / spectrogramScript.spectrum.Length + "Hz per bin";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
