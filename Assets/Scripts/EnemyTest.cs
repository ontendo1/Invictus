using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetChilds<T>
{
    T[] obj;
    public void Add(T item)
    {

    }
}

public class EnemyTest : MonoBehaviour
{
    Rigidbody2D rb;
    CapsuleCollider2D cc;
    Animator anim;
    [SerializeField] float followSpeed;
    [SerializeField] float isFar;
    [SerializeField] float Speed;
    bool canWalk = true;
    bool backIsSafe;
    float playerVelocityX;

    float playerVelocityY;
    float far;
    float dirX;
    [SerializeField] GameObject _player;
    Transform playerTransform;
    Animator[] childAnims;
    CircleCollider2D[] childCircleCollider;
    Vector2 dir;
    Animator legAnim;
    CircleCollider2D legCollider;
    void Start()
    {
        childAnims = GetComponentsInChildren<Animator>();
        childCircleCollider = GetComponentsInChildren<CircleCollider2D>();
        foreach (Animator _childAnims in childAnims)
        {
            if (_childAnims.name == "objGuardianLegs")
            {
                legAnim = _childAnims;
            }
        }
        foreach (CircleCollider2D _legCollider2D in childCircleCollider)
        {
            if (_legCollider2D.name == "objGuardianLegs")
            {
                legCollider = _legCollider2D;
            }
        }
        _player = FindObjectOfType<PlayerScript>().gameObject;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        cc = GetComponent<CapsuleCollider2D>();
        InvokeRepeating("TellMeFar", 4f, 0.4f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerTransform = _player.transform;
        Speed = Mathf.Abs(rb.velocity.x);
        Vector3 playerPos = new Vector2(playerTransform.position.x + playerVelocityX, transform.position.y);
        dir = new Vector2(playerPos.x - transform.position.x, 0f);
        dirX = Mathf.Sign(dir.x);
        if (TellMeFar() > isFar)
        {
            if (canWalk)
                rb.velocity = new Vector2(dirX * followSpeed, rb.velocity.y);
            else if (Speed != 0) rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else
        {
            if (canWalk)
            {
                rb.velocity = new Vector2(0f, rb.velocity.y);
                Invoke("canWalkTrue", 1.5f);
                canWalk = false;

            }
            if (Speed != 0 && !backIsSafe) rb.velocity = new Vector2(0, rb.velocity.y);


        }
        transform.localScale = new Vector3(dirX, transform.localScale.y, transform.localScale.z);
    }

    void Update()
    {
        backIsSafe = legCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));

        legAnim.SetFloat("Speed", Speed);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!canWalk)
            {
                rb.velocity = new Vector2(-dirX * followSpeed * 2, rb.velocity.y);
            }
        }
    }
    public float TellMeFar()
    {
        far = playerTransform.position.x - transform.position.x;
        return Mathf.Abs(far);
    }
    void canWalkTrue()
    {
        canWalk = true;
    }
}
