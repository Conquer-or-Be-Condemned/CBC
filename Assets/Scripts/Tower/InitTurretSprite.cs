using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitTurretSprite : MonoBehaviour
{
    
    private SpriteRenderer _sr;
    // Start is called before the first frame update
    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        
    }

    private void OverHeat()
    {
        _sr.color = Color.red;
    }

    private void UnderHeat()
    {
        _sr.color = Color.white;
    }
    
    
    // Update is called once per frame
    void Update()
    {
        
    }

    // public void setSprite()
    // {
    //     sr.sprite = sprites[0];
    // }
}
