using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class birdMovement : MonoBehaviour
{
    [SerializeField] float spd = 5f;
    [SerializeField] float endDashTime = 0.2f;
    [SerializeField] float dashSpd = 0.2f;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Move();
        StartDash();
        bool ph = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        if (ph) transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
    }

    private void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector2 playerVelocity = new Vector2(h * spd, v * spd);
        rb.velocity = playerVelocity;
    }

    private void StartDash()
    {
        if (spd > 5f) return;
        if (Input.GetKeyDown(KeyCode.C))
        {
            Invoke("EndDash", endDashTime);
            spd = dashSpd;
        }
    }
    void EndDash()
    {
        spd = 5f;
    }
}
