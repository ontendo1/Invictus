using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    Rigidbody2D rb;
    CapsuleCollider2D cc;
    public bool called = false;
    [SerializeField] float returnSpeed;
    [SerializeField] float SwordY;
    float playerVelocityX;
    float playerVelocityY;
    GameObject _player;
    GameObject swordChill;
    Transform playerTransform;
    void Start()
    {
        swordChill = FindObjectOfType<SwordColliderScript>().gameObject;
        _player =  FindObjectOfType<PlayerScript>().gameObject;
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();
    }
    void FixedUpdate()
    {
        if (called)
        {
            Rigidbody2D _playerRB = _player.GetComponent<Rigidbody2D>();
            PlayerScript general = _player.GetComponent<PlayerScript>();

            if (!general.checkSwordReturnPosition())
            {
                playerVelocityX = _playerRB.velocity.x / 10;
                playerVelocityY = _playerRB.velocity.y / 20;
            }
            else
            {
                playerVelocityX = 0;
                playerVelocityY = 0;
            }
            playerTransform = _player.transform;
            Vector3 playerPos = new Vector2(playerTransform.position.x + playerVelocityX, playerTransform.position.y + SwordY + playerVelocityY);
            Vector2 dir = playerPos - transform.position;
            rb.MovePosition((Vector2)transform.position + (dir * returnSpeed * Time.deltaTime));
        }
    }

    public void CallBackMySword()
    {
        rb.gravityScale = 0;
        BoxCollider2D swordChillBC = swordChill.GetComponent<BoxCollider2D>();
        swordChillBC.isTrigger = true;
        rb.constraints = RigidbodyConstraints2D.None;
        called = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") && !called)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
    public bool IsFreeze()
    {
        return rb.constraints == RigidbodyConstraints2D.FreezeAll;
    }
}
