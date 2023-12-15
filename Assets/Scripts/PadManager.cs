using System;
using System.Reflection;
using ArcanepadSDK.Models;
using ArcanepadSDK.Types;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PadManager : MonoBehaviour
{
    public Button AttackButton;
    public Button CalibrateQuaternionButton;
    public Button CalibratePointerTopLeftButton;
    public Button CalibratePointerBottomRight;
    public TextMeshProUGUI LogsText;

    async void Awake()
    {
        // INITIALIZE LIBRARY
        Arcane.Init(new ArcaneInitParams(deviceType: ArcaneDeviceType.pad, padOrientation: AOrientation.Portrait));

        // WAIT UNTIL CLIENT IS INITIALIZED
        await Arcane.ArcaneClientInitialized();

        // ATTACK
        AttackButton.onClick.AddListener(OnAttackButtonPress);

        // LISTEN FOR AN EVENT SENT TO THIS PAD
        Arcane.Pad.On("TakeDamage", new Action<TakeDamageEvent>(TakeDamage));

        // CALIBRATE ROTATION
        CalibrateQuaternionButton.onClick.AddListener(() => Arcane.Pad.CalibrateQuaternion());

        // GET PAD POINTER DATA (POSTION X, Y WHERE THE PAD IS POINTING TO THE SCREEN)
        Arcane.Pad.StartGetPointer();
        Arcane.Pad.OnGetPointer((GetPointerEvent e) => LogsText.text = "Pointer: x:" + e.x + " | y: " + e.y);

        // CALIBRATE POINTER TOP LEFT
        CalibratePointerTopLeftButton.onClick.AddListener(() => Arcane.Pad.CalibratePointer(true));
        // CALIBRATE POINTER BOTTOM RIGHT
        CalibratePointerBottomRight.onClick.AddListener(() => Arcane.Pad.CalibratePointer(false));
    }

    void TakeDamage(TakeDamageEvent e)
    {
        // MAKE PAD VIBRATE 100 MS TIMES DAMAGE. IF DAMAGE IS 3 IT WILL VIBRATE 300 ms
        Arcane.Pad.Vibrate(100 * e.damage);
    }

    void OnAttackButtonPress()
    {
        // EMIT EVENT FROM PAD TO VIEW
        Arcane.Msg.EmitToViews(new ArcaneBaseEvent("Attack"));
    }
}
