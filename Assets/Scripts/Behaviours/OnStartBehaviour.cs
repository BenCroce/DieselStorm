using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnStartBehaviour : MonoBehaviour
{    
    public GameObject Controller;
    private GameObject _controller;

    void OnEnable()
    {
        _controller = Instantiate(Controller,transform);        
    }

    void OnDisable()
    {
        Destroy(_controller);
    }
}
