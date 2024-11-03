using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float speed;


    // Start is called before the first frame update
    void Start()
    {
        Invoke("deleteBullet",1);
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.Translate(Vector2.left*speed*Time.deltaTime);
    }

    void deleteBullet()
    {
        Destroy(gameObject);
    }
}
