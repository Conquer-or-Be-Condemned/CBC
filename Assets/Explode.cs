using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("ExplodeisCalled");
        _animator = GetComponent<Animator>();
        StartCoroutine(DestroyExplosion());
    }

    public void TriggerExplosion()
    {
        // Debug.Log("ExplodeisTriggered");
        if (_animator != null)
        {
            _animator.SetBool("isExplode", true);
        }
        // Invoke("DestroyExplosion",0.1f);
        // StartCoroutine(DestroyExplosion());
    }

    private IEnumerator DestroyExplosion()
    {
        yield return new WaitForSeconds(0.8f);
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
