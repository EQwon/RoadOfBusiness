using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class text : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.D))
        {
            transform.position += new Vector3(0.5f, 0, 0);
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            transform.position += new Vector3(0, 0.5f, 0);
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            transform.position += new Vector3(-0.5f, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            transform.position += new Vector3(0, -0.5f, 0);
        }

    }
}
        