using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bombEffect : MonoBehaviour
{
    [SerializeField] private GameObject effect;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().enabled = false; 
        StartCoroutine(TwoSecond());    
        GetComponent<Renderer>().enabled = true; 
        effect.SetActive(true);
        StartCoroutine(ActivateBombSequence());    
    }

    IEnumerator TwoSecond()
    {
        yield return new WaitForSeconds(2f);
    }
    IEnumerator ActivateBombSequence()
    {
        yield return new WaitForSeconds(0.8f);
        Destroy(gameObject);
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
