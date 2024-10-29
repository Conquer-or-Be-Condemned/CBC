using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitTurretSprite : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    public Animator am;
    // Start is called before the first frame update
    void Start()
    {
        am = GetComponent<Animator>();
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
