using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BLEController : MonoBehaviour
{
    public const string MacAddrSZ = "BC:97:40:40:87:46";
    public const string MacAddrCD = "BC:97:40:40:87:CB";

    private Text _bleStatusLabel;
    private Text _fBrake, _rBrake, _cadence, _angle;

    private Button _ConnectBtn;

    private string _bleStatusUp = "Connected";
    private string _bleStatusDown = "Unconnected";

    private BLEAdaperComponent bleAdaper;
    private BleSid02ControllerMessage bleSid02ControllerMessage;

    public float angle;
    public float fBrake;
    public float cadence;
    public float rBrake;
    
    void Start()
    {
        bleAdaper = new BLEAdaperComponent();
        bleAdaper.AddOnGameCmdHandler(receiveMessage);
        GameObject _bleCanvas = GameObject.Find("BleCanvas");

        _bleStatusLabel = _bleCanvas.transform.Find("BleStatus").GetComponent<Text>();

        _angle = _bleCanvas.transform.Find("angle").GetComponent<Text>();
        _fBrake = _bleCanvas.transform.Find("fbrake").GetComponent<Text>();
        _rBrake = _bleCanvas.transform.Find("rbrake").GetComponent<Text>();
        _cadence = _bleCanvas.transform.Find("cadence").GetComponent<Text>();

        _ConnectBtn = _bleCanvas.transform.Find("ConnectBtn").GetComponent<Button>();
        _ConnectBtn.onClick.AddListener(onConnectClicked);
    }

    private void onConnectClicked()
    {
        _ = bleAdaper.ScanToConnectBK(MacAddrSZ);
        Debug.Log("button clicked");
    }

    public void receiveMessage(BleSid02ControllerMessage? bleSid02)
    {
        bleSid02ControllerMessage = (BleSid02ControllerMessage)bleSid02;
        angle = bleSid02ControllerMessage.Angle;
        cadence = bleSid02ControllerMessage.RealtimeCadence;
        fBrake = bleSid02ControllerMessage.FBrake;
        rBrake = bleSid02ControllerMessage.RBrake;

        _angle.text = bleSid02ControllerMessage.Angle.ToString();
        _cadence.text = bleSid02ControllerMessage.RealtimeCadence.ToString();
        _rBrake.text = bleSid02ControllerMessage.RBrake.ToString();
        _fBrake.text = bleSid02ControllerMessage.FBrake.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        _bleStatusLabel.text = (bleAdaper.Connected.Value) ? _bleStatusUp : _bleStatusDown;
    }

}
