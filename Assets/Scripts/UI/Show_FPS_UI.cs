using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Show_FPS_UI : MonoBehaviour
{
    private const int FPS_SAMPLE_COUNT = 20;
    [SerializeField] private TMP_Text _fpsText;

    private readonly int[] _fpsSamples = new int[FPS_SAMPLE_COUNT];
    private int _sampleIndex;
    void Awake()
    {
        InvokeRepeating(nameof(UpdateFps), 0, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        _fpsSamples[_sampleIndex++] = (int)(1.0f / Time.deltaTime);
        if (_sampleIndex >= FPS_SAMPLE_COUNT) _sampleIndex = 0;
    }

    private void UpdateFps()
    {
        var sum = 0;
        for (var i = 0; i < FPS_SAMPLE_COUNT; i++)
        {
            sum += _fpsSamples[i];
        }

        _fpsText.text = $"FPS: {sum / FPS_SAMPLE_COUNT}";
    }
}
