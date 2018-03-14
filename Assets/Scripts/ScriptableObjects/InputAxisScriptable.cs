using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Input Axis", menuName = "Input Axis")]
public class InputAxisScriptable : ScriptableObject
{
    public string m_AxisName;    

    public float AxisValue()
    {        
        return Input.GetAxis(m_AxisName);
    }
}
