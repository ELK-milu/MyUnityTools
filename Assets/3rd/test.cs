using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    [ContextMenu("Start")]
    void Start()
    {
        // IFactory operFactory = new AddFactory();
        // Operation oper = operFactory.CreateOperation();
        // oper.NumberA = 5; oper.NumberB = 6;
        // Debug.Log(oper.GetResult());

        ABManager.Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
