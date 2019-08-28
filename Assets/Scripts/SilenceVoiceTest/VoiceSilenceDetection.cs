using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class VoiceSilenceDetection : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private int sampleValue = 1024;

    [SerializeField]
    private float silenceThresholdValue = 0f;

    [SerializeField]
    string microphoneDevice;

    float[] clipSampleData;
    bool isSpeaking = false;

    public delegate void SilenceDetected();
    public static event SilenceDetected OnSilenceDetected;

    private bool isForDetect = true;

    private bool isSilenced = true;

    private void Awake()
    {
        Logic.OnStartDetection += SetDectection;
    }

    // Start is called before the first frame update
    void Start()
    {
        clipSampleData = new float[sampleValue];

        CheckMicrophones();

        if (!InitMicrophone()) return;

        StartRecording(microphoneDevice);
    }

    void Update()
    {
        if (!isForDetect) return;


        audioSource.GetSpectrumData(clipSampleData, 0, FFTWindow.Rectangular);
        float currentAverageVolume = clipSampleData.Average();

        Debug.Log(currentAverageVolume);

        if (currentAverageVolume > silenceThresholdValue)
        {
            isSpeaking = true;
            isSilenced = false;
            Debug.Log("Speaking");
            return;
        }

        if (isSilenced) return;

        else if (isSpeaking)
        {
            isSpeaking = false;
            isSilenced = true;
            Debug.Log("Not Speaking");
            isForDetect = false;
            OnSilenceDetected?.Invoke();
        }
    }

    void CheckMicrophones()
    {
        foreach (string device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
        }
    }

    bool InitMicrophone()
    {
        if (string.IsNullOrEmpty(microphoneDevice))
        {
            if (Microphone.devices.Length != 0)
            {
                microphoneDevice = Microphone.devices[0];
                Debug.Log(microphoneDevice);
                return true;
            }

            else
            {
                Debug.LogWarning("No devices detected");
                return false;
            }
        }

        else return true;
    }

    void StartRecording(string device)
    {
        audioSource.clip = Microphone.Start(device, true, 60, 16000);
        while (!(Microphone.GetPosition(null) > 0)) { }
        audioSource.Play();
    }

    void StopRecording(string device)
    {
        Microphone.End(device);
    }

    void SetDectection(bool flag) => isForDetect = flag;
}