using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBehaviour : MonoBehaviour
{
    public TankStats ts;
    public ModifierScriptable mod;
    public void RaiseEvent()
    {
        ts.TakeDamage(mod);
    }
    public void onStatChanged(UnityEngine.Object[] args)
    {
        Debug.Log(args[0].name + " has changed");
    }
}
