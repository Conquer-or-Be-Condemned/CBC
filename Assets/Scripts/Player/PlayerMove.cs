using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermove : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f; //좌우 이동속도값
    [SerializeField] private float jumpSpeed = 20.0f; //점프 이동속도값
    [SerializeField] private bool isGround = true; //점프 가능여부 판단 변수
    public int playerHp = 4;

    private Rigidbody2D _rigid;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();

        _rigid.gravityScale = 10.0f;
    }

    void Update()
    {
	    FacingDirection();
        Move();
        Jump();
    }
	void FixedUpdate()
	{
		
	}

    //플레이어 이동관련 함수
    void Move()
    {
		//x축 방향으로 정해진 속도만큼 이동
		float x = Input.GetAxisRaw("Horizontal");
		_rigid.velocity = new Vector2(x * speed, _rigid.velocity.y);

		//플레이어 좌우 방향 조절
		// if (Input.GetButtonDown("Horizontal"))
		// {
		// 	spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
		// }
		
		//플레이어 이동 애니메이션
		if (_rigid.velocity.x == 0)
		{
			_animator.SetBool("isRun", false);
		}
		else
		{
			_animator.SetBool("isRun", true);
		}
	}

	//플레이어의 점프관련 함수
	void Jump()
    {
        //공중이 아닌 상태에서 space바를 누르면 점프
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))&&isGround)
        {
            _rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            isGround = false;
            _animator.SetBool("isJump", true);   
		}
        
        //Raycast를 이용하는 경우
        /*
        if(_rigid.velocity.y < 0)
        {
            RaycastHit2D rayHit = Physics2D.Raycast(_rigid.position, Vector2.down, 1, LayerMask.GetMask("Ground"));
            if (rayHit.collider != null)
            {
                if(rayHit.distance < 0.2)
                {
                    animator.SetBool("isJump", false);
                }
            }
        }
        */
    }


    //땅과의 충돌을 체크하는 함수
    void OnCollisionEnter2D(Collision2D collision)
    {
        //Ground태그인 물체에 접촉해 있을때(점프상태가 아닐시)
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround=true;
            _animator.SetBool("isJump", false);
        }
        //몬스터에게 피해를 입은 경우
        if(collision.gameObject.tag == "Enemy")
        {
            OnDamaged(collision.transform.position);
        }
    }
    //피격관련 함수
    void OnDamaged(Vector2 nowPos)
    {
        playerHp--;
        gameObject.layer = 8;
        _spriteRenderer.color = new Color(1, 1, 1, 0.5f);

        //피격시 이동
        int dict = 0;
        if (transform.position.x - nowPos.x > 0)
            dict = 1;
        else
            dict = -1;
        _rigid.AddForce(new Vector2(dict,1)*10,ForceMode2D.Impulse);

        //피격 애니
        _animator.SetTrigger("isDamaged");

        Invoke("OffDamaged", 2);
    }
    void OffDamaged()
    {
        gameObject.layer = 7;
        _spriteRenderer.color = new Color(1, 1, 1,1f);
	}
    //by jd player flip
    void FacingDirection()
    {
	    if(_rigid.velocity.x < 0) _spriteRenderer.flipX = true;//look at the dir of vec
	    else _spriteRenderer.flipX = false;
    }
}