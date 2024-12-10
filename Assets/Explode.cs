using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    public LayerMask layerMask;
    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("ExplodeisCalled");
        Collider2D hits = Physics2D.OverlapCircle(transform.position, 100, layerMask);
        if (hits != null)
        {
            float distance = Vector2.Distance(transform.position, hits.transform.position);
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.MissileExplosion, distance, 100);
        }

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
