using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Input Button", menuName = "Input Button")]
public class InputButtonScriptable : ScriptableObject {

    public string m_ButtonName;

    public bool IsButtonDown()
    {
        return Input.GetButton(m_ButtonName);
    }

    public bool WasButtonPressed()
    {
        return Input.GetButtonDown(m_ButtonName);
    }

    public bool WasButtonReleased()
    {
        return Input.GetButtonUp(m_ButtonName);
    }
}
