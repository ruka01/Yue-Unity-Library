using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

[RequireComponent(typeof(RawImage))]
public class Webcam : MonoBehaviour
{
    private WebCamDevice[] devices;
    private WebCamTexture webCamTexture;

    [SerializeField] private TMP_Dropdown deviceDropdown;

    [SerializeField] private TextMeshProUGUI deviceStatusText;

    private bool isScaled = false;

    private void Awake()
    {
        devices = WebCamTexture.devices;
        webCamTexture = new WebCamTexture();

        if (devices.Length > 0)
            ChangeDevice(0);

        deviceDropdown.ClearOptions();
        List<string> deviceNames = new List<string>();
        foreach (WebCamDevice dev in devices)
        {
            deviceNames.Add(dev.name);
        }
        deviceDropdown.AddOptions(deviceNames);
        deviceDropdown.onValueChanged.AddListener(ChangeDevice);
        GetComponent<RawImage>().texture = webCamTexture;
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        webCamTexture.Play();
    }

    private void OnDisable()
    {
        webCamTexture.Stop();    
    }

    public void ChangeDevice(int deviceIndex)
    {
        if (deviceIndex < devices.Length)
        {
            webCamTexture.Stop();
            webCamTexture.deviceName = devices[deviceIndex].name;
            webCamTexture.Play();

            if (!webCamTexture.isPlaying)
                deviceStatusText.text = Yue.Text.ColorText.TMPro("Webcam device error.",
                    "error.", Color.red);
            else
                deviceStatusText.text = Yue.Text.ColorText.TMPro("Webcam OK", 
                    "OK", Color.green);
        }
    }

    public void ToggleWebcamSize()
    {
        isScaled = !isScaled;

        if (isScaled)
            ScaleWebcam(1f);
        else
            ScaleWebcam(0.5f);
    }

    private void ScaleWebcam(float scale)
    {
        float s = Mathf.Clamp(scale, 0f, 1f);
        transform.localScale = new Vector3(s, s, s);
    }

    public void RefreshWebcam()
    {
        webCamTexture.Stop();
        devices = WebCamTexture.devices;
        if (devices.Length > 0)
            ChangeDevice(0);

        deviceDropdown.ClearOptions();
        deviceDropdown.onValueChanged.RemoveAllListeners();
        List<string> deviceNames = new List<string>();
        foreach (WebCamDevice dev in devices)
        {
            deviceNames.Add(dev.name);
        }
        deviceDropdown.AddOptions(deviceNames);
        deviceDropdown.onValueChanged.AddListener(ChangeDevice);

        webCamTexture.Play();
    }
}
