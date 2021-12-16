﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BLEController : MonoBehaviour
{
    public const string MacAddrSZ = "BC:97:40:40:87:46";
    public const string MacAddrCD = "BC:97:40:40:87:CB";
    private int test = 1;
    private Text _testLog;
    private Text _MacAddr;

    private Text _bleStatusLabel;
    private Text _gear1, _gear2, _steer, _power, _fBrake, _rBrake, _reset, _cadence, _speed, _crank, _angle;

    private Button _ConnectBtn;
    private Button _DisconnectBtn;
    private Button _SendBtn;

    private string _bleStatusUp = "Bluetooth Connected";
    private string _bleStatusDown = "Bluetooth Unconnected";

    private BLEAdaperComponent bleAdaper;
    private BleSid02ControllerMessage bleSid02ControllerMessage;

    private int gear1;
    private int gear2;
    private int resistence;
    private uint vibsel;
    private Text _arg1, _arg2, _arg3, _arg4;

    public float angle;
    public float fBrake;
    public float cadence;
    
    void Start()
    {
        bleAdaper = new BLEAdaperComponent();
        bleAdaper.AddOnGameCmdHandler(receiveMessage);
        GameObject _bleCanvas = GameObject.Find("BleCanvas");

        _testLog = _bleCanvas.transform.Find("TestLog/Logs").GetComponent<Text>();
        _bleStatusLabel = _bleCanvas.transform.Find("ScanMessage/StatusLabel").GetComponent<Text>();
        _MacAddr = _bleCanvas.transform.Find("MacInputField/MacAddr").GetComponent<Text>();

        _angle = _bleCanvas.transform.Find("ReceivedInfo/gear1").GetComponent<Text>();
        _gear2 = _bleCanvas.transform.Find("ReceivedInfo/gear2").GetComponent<Text>();
        _steer = _bleCanvas.transform.Find("ReceivedInfo/steer").GetComponent<Text>();
        _power = _bleCanvas.transform.Find("ReceivedInfo/power").GetComponent<Text>();
        _fBrake = _bleCanvas.transform.Find("ReceivedInfo/fBrake").GetComponent<Text>();
        _rBrake = _bleCanvas.transform.Find("ReceivedInfo/rBrake").GetComponent<Text>();
        _crank = _bleCanvas.transform.Find("ReceivedInfo/reset").GetComponent<Text>();
        _cadence = _bleCanvas.transform.Find("ReceivedInfo/cadence").GetComponent<Text>();
        _speed = _bleCanvas.transform.Find("ReceivedInfo/speed").GetComponent<Text>();

        _ConnectBtn = _bleCanvas.transform.Find("ConnectButton").GetComponent<Button>();
        _ConnectBtn.onClick.AddListener(onConnectClicked);

        _DisconnectBtn = _bleCanvas.transform.Find("DisconnectButton").GetComponent<Button>();
        _DisconnectBtn.onClick.AddListener(onUnconnectClicked);

        _SendBtn = _bleCanvas.transform.Find("SendButton").GetComponent<Button>();
        _SendBtn.onClick.AddListener(sendMessage);

        _arg1 = _bleCanvas.transform.Find("SendInfo/Arg1InputField/Arg1").GetComponent<Text>();
        _arg1 = _bleCanvas.transform.Find("SendInfo/Arg1InputField/Arg1").GetComponent<Text>();
        _arg1 = _bleCanvas.transform.Find("SendInfo/Arg1InputField/Arg1").GetComponent<Text>();
        _arg1 = _bleCanvas.transform.Find("SendInfo/Arg1InputField/Arg1").GetComponent<Text>();
    }

    private void onConnectClicked()
    {
        _ = bleAdaper.ScanToConnectBK(MacAddrSZ);
        Debug.Log("button clicked");
        _testLog.text = ("connect is called");
    }

    private void onUnconnectClicked()
    {
        _testLog.text = ("unconnect is called");
        if (bleAdaper.Connected.Value) bleAdaper.DisconnectBK();
        else Debug.Log("unconnected");
    }

    private void sendMessage()
    {
        sbyte[] rawData = BleProtocalsHelper.CreateSid03GameParamsMessage(int.Parse(_arg1.text), int.Parse(_arg2.text), int.Parse(_arg3.text), uint.Parse(_arg4.text), uint.Parse(_arg4.text));
        //sbyte[] rawData = BLEProtocalsHelper.CreateGame2BleMessage(gear1, gear2, resistence, vibsel);
        if (bleAdaper.Connected.Value)
        {
            bleAdaper.SendToBk(rawData);
        }
        else Debug.Log("Bluetooth Unconnected");
        
    }

    public void receiveMessage(BleSid02ControllerMessage? bleSid02)
    {
        _testLog.text = ("message received");
        bleSid02ControllerMessage = (BleSid02ControllerMessage)bleSid02;
        angle = bleSid02ControllerMessage.Angle;
        cadence = bleSid02ControllerMessage.RealtimeCadence;
        fBrake = bleSid02ControllerMessage.FBrake;

        _angle.text = bleSid02ControllerMessage.Angle.ToString();
        _speed.text = bleSid02ControllerMessage.Speed.ToString();
        _cadence.text = bleSid02ControllerMessage.RealtimeCadence.ToString();
        _crank.text = bleSid02ControllerMessage.Crank.ToString();
        _rBrake.text = bleSid02ControllerMessage.RBrake.ToString();
        _fBrake.text = bleSid02ControllerMessage.FBrake.ToString();

        //string byteArray = System.Text.Encoding.ASCII.GetString(bytes);
        //var tmp = BleProtocalsHelper.ParseBleMessage(bytes);
        //if (tmp != null)
        //{
        //    (BleSid02ControllerMessage)bleSid02
        //    ble2GameMessage = (Ble2GameMessage)tmp;
        //    _gear1.text = ble2GameMessage.gear1.ToString();
        //    _gear2.text = ble2GameMessage.gear2.ToString();
        //    _steer.text = ble2GameMessage.power.ToString();
        //    _power.text = ble2GameMessage.power.ToString();
        //    _rBrake.text = ble2GameMessage.rBrake.ToString();
        //    _reset.text = ble2GameMessage.reset.ToString();
        //    _fBrake.text = ble2GameMessage.fBrake.ToString();
        //    _cadence.text = ble2GameMessage.cadence.ToString();
        //    _speed.text = ble2GameMessage.speed.ToString();
        //}
    }

    // Update is called once per frame
    void Update()
    {
        _bleStatusLabel.text = (bleAdaper.Connected.Value) ? _bleStatusUp : _bleStatusDown;
    }

}
