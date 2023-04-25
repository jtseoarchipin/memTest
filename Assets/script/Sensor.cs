using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    // Start is called before the first frame update
    public Sensor()
    {
        int x = 0;
        x++;
    }
    void Start()
    {
        Debug.Log("it show up");
    }

    private void OnDestroy()
    {
        Debug.Log("it's released");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
