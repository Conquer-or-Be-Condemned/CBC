using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class bombscript : MonoBehaviour
{
    [SerializeField] private GameObject bomb;  
   // [SerializeField] private GameObject effect;
    [SerializeField] public float bombDamage = 100f;
  //  [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float explosionRange = 20f;
    [SerializeField] private Animator animator;  // Animator 컴포넌트 참조
    public bool isbam=false;
    
    void Start()
    { 
        animator = GetComponent<Animator>();
        animator.SetBool("isboom", false);
        bomb.SetActive(true);  // bomb 오브젝트 활성화
     //effect.SetActive(false);  // effect 오브젝트 비활성화
        StartCoroutine(ActivateBombSequence());  // bomb 활성화 후 비활성화 및 effect 활성화 관리
    }

    IEnumerator ActivateBombSequence()
    {
        yield return new WaitForSeconds(2f);  // 2초 대기
        // isbomb 파라미터를 true로 설정
        isbam = true;
        yield return new WaitForSeconds(0.6f); 
        
        Collider2D[] monsters = Physics2D.OverlapCircleAll(GetComponent<Rigidbody2D>().position, explosionRange);
        foreach (var monster in monsters)
        {
            if (monster.CompareTag("Enemy"))
            {
                monster.GetComponent<Monster>().TakeDamage(bombDamage);
            }
        }
        Destroy(gameObject);
        //bomb.SetActive(false);  // bomb 오브젝트 숨김
        //effect.SetActive(true);  // effect 활성화
      
       // yield return new WaitForSeconds(2f);  // 2초 대기
      //  effect.SetActive(false);
        //Destroy(gameObject);
        
    }

    private void FixedUpdate()
    {
        if (isbam == true)
        {
            animator.SetBool("isboom", true);  // isboom 파라미터를 true로 설정
            isbam = false;  // 파라미터를 한 번만 설정하고 이후로는 반복하지 않도록
        }
    }

}