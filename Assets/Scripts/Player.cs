using System;
using ArcanepadExample;
using ArcanepadSDK;
using ArcanepadSDK.Models;
using UnityEngine;

public class Player : MonoBehaviour
{
    public ArcanePad Pad { get; private set; }
    public Transform pointer;

    public void Initialize(ArcanePad pad)
    {
        Pad = pad;

        // USER NAME AND COLOR
        Debug.Log("User Name: " + Pad.User.name);
        Debug.Log("User Color: #" + Pad.User.color);


        // LISTEN EVENT SENT FROM PAD TO VIEW, IN DIFFERENT WAYS: 
        // Pad.On("Attack", (ArcaneBaseEvent e) => { });           // UNTYPED LAMBDA
        // Pad.On("Attack", new Action<ArcaneBaseEvent>(Attack));  // UNTYPED FUNCTION
        // Pad.On("Attack", (AttackEvent e) => { });               // TYPED LAMBDA
        Pad.On("Attack", new Action<AttackEvent>(Attack));         // TYPED FUNCTION

        Pad.StartGetQuaternion();
        Pad.OnGetQuaternion(new Action<GetQuaternionEvent>(RotatePlayer));

        Pad.StartGetPointer();
        Pad.OnGetPointer((GetPointerEvent e) => Debug.Log(e.x + " | " + e.y));
    }

    void Attack(ArcaneBaseEvent e)
    {
        Debug.Log(Pad.User.name + " has attacked");

        // MAKE PAD VIBRATE FROM VIEW
        Pad.Vibrate(1000);

        // EMIT EVENT TO PAD FROM VIEW
        Pad.Emit(new TakeDamageEvent(3));               // TYPED
        // Pad.Emit(new ArcaneBaseEvent("TakeDamage")); // UNTYPED (CAN'T DEFINE MEMBERS SO IT ONLY WORKS FOR EVENTS WITHOUT DATA)
    }

    void RotatePlayer(GetQuaternionEvent e)
    {
        transform.rotation = new Quaternion(e.x, e.y, e.z, e.w);
    }
}