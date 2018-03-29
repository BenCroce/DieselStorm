using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementBehaviour : MonoBehaviour
{
    public abstract void Move(float h,float v, float j);   
}
