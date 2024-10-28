using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private Transform pos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x-0.1f, transform.position.y, transform.position.z);
        if (transform.position.x < -24)
        {
            transform.position = new Vector3(55, transform.position.y, transform.position.z);
        }
    }
}
