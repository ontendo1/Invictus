using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    SpriteRenderer sprRend;
    BoxCollider2D bxColl;
    CapsuleCollider2D capColl;
    CircleCollider2D circColl;
    Animator anim;
    [HideInInspector] public Rigidbody2D rb;
    [Header("Speed Parameters")]
    [SerializeField] float jsp = 10f;
    [SerializeField] float hsp = 5f;
    [SerializeField] float dashSpd = 15f;
    [SerializeField] float dashLong = 0.15f;
    [SerializeField] float hspSlow = 1f;
    [SerializeField] float hspSlowNonGround = 3f;
    [SerializeField] float hspSlow2 = 2f;
    [SerializeField] bool isDashing = false;
    public bool haveSword = true;
    [SerializeField] GameObject myGrid;
    [Header("Sword Parameters")]
    [SerializeField] GameObject objSword;
    [SerializeField] GameObject objDashEff;
    [SerializeField] GameObject objLeg;
    [SerializeField] float swordSpeed = 1.2f;
    [SerializeField] float swordGrav = 0.2f;
    [SerializeField] float swordPosY = 0.1f;
    [SerializeField] float swordPosX = 0f;
    [SerializeField] bool inCatchS = false;
    [SerializeField] bool isAttack = false;
    [SerializeField] bool inAttack1 = false;
    [SerializeField] bool canAttack = true;
    [SerializeField] float attackDelay = 0.2f;
    [SerializeField] bool inAttack2 = false;
    [SerializeField] bool inThrowS = false;
    public bool inChildLegAnim = false;

    [HideInInspector] public bool canDash = true;
    [HideInInspector] public bool isGround = false;
    [HideInInspector] public bool isCapGround = false;
    [HideInInspector] public bool isDashGround = false;
    [HideInInspector] public bool canJump = false;
    [HideInInspector] public bool createDashEffect = false;
    PlayerChill playerChill;
    GameObject sword;

    void Start()
    {
        playerChill = FindObjectOfType<PlayerChill>().GetComponent<PlayerChill>();
        objLeg = GameObject.Find("legAnim");
        anim = GetComponent<Animator>();
        sprRend = GetComponent<SpriteRenderer>();
        bxColl = GetComponent<BoxCollider2D>();
        capColl = GetComponent<CapsuleCollider2D>();
        circColl = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Movin();
        Jumpin();
        Dash();
        FlipSprite();
        CheckCanJump();

        if (Input.GetKeyDown(KeyCode.B) && !inAttack1 && !inAttack2 && !inThrowS && !inCatchS)
        {
            if (haveSword)
            {
                anim.SetTrigger("isThrowS");
                inThrowS = true;
            }
            else
            {
                SwordScript swordScript = sword.GetComponent<SwordScript>();

                if (swordScript.IsFreeze())
                {
                    inCatchS = true;
                    anim.SetTrigger("callBackThrowS");
                    swordScript.CallBackMySword();
                }

            }
        }

        if (createDashEffect && Mathf.Abs(rb.velocity.x) > 5f)
        {
            if (!isDashGround && !playerChill.checkHorizontalGround())
            {
                InvokeRepeating("DashEff", 0f, 0.02f);
                createDashEffect = false;
            }
            else
            {
                CancelInvoke("DashEff");
                createDashEffect = true;
            }
        }
        else
        {
            if (Mathf.Abs(rb.velocity.x) <= 5f)
            {
                CancelInvoke("DashEff");
                createDashEffect = true;
            }
        }

        anim.SetBool("doThrowS", !Input.GetKey(KeyCode.B) && inThrowS);


        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("vSpeed", Mathf.Abs(rb.velocity.y));

        anim.SetBool("haveSword", haveSword);
        //ATTACK TYPE 1
        if (Input.GetKey(KeyCode.C) && !inAttack1 && !inAttack2 && !inThrowS && !inCatchS && canAttack && hsp == 5f)
        {
            anim.SetTrigger("isAttack1");
            inAttack1 = true;
            inAttack2 = false;

        }
        if (Mathf.Abs(rb.velocity.x) > 1 && inAttack1 && hsp != dashSpd && hsp != hspSlow && hsp != hspSlowNonGround)
        {
            hsp = dashSpd;
            Invoke("AttackType_1", dashLong + 0.05f);
        }
        isAttack = Input.GetKey(KeyCode.C) && canAttack;
        if (Input.GetKeyUp(KeyCode.C) && canAttack)
        {
            canAttack = false;
            Invoke("IsCanAttackTrue", attackDelay);
        }

        if (isGround && hsp == hspSlowNonGround)
        {
            if (inAttack1)
            {

                hsp = hspSlow;
            }
            else
            {
                hsp = 5f;
                CancelInvoke("AttackType_1");
            }
        }

        anim.SetBool("isUnattack1", isAttack);
        if (Input.GetKeyDown(KeyCode.V) && !inAttack1 && !inAttack2 && !inThrowS && !inCatchS)
        {
            anim.SetTrigger("isAttack2");
            Invoke("AttackType_2", 0.2f);

            inAttack2 = true;
            inAttack1 = false;
        }
    }
    void AttackType_1()
    {
        if (inAttack1)
        {
            if (hsp == hspSlow || hsp == hspSlowNonGround)
            {
                Invoke("AttackType_1", dashLong);
                hsp = dashSpd;
            }
            else
            {
                if (isGround)
                {
                    Invoke("AttackType_1", 0.25f);
                    hsp = hspSlow;
                    rb.velocity = new Vector2(hsp, rb.velocity.y);
                }
                else
                {
                    Invoke("AttackType_1", 0.5f);
                    hsp = hspSlowNonGround;
                    rb.velocity = new Vector2(hsp, rb.velocity.y);
                }
            }
        }
        else
        {
            hsp = 5f;
            rb.velocity = new Vector2(hsp, rb.velocity.y);
        }
    }
    void IsCanAttackTrue()
    {
        canAttack = true;
    }
    void AttackType_2()
    {
        if (inAttack2)
        {
            if (hsp == hspSlow2)
            {
                Invoke("AttackType_2", 0.02f);
            }
            else
            {
                hsp = isGround ? hspSlow2 : hspSlow2 + 2;
                Invoke("AttackType_2", 0.12f);
            }
        }
        else
        {
            hsp = 5f;
        }
    }
    void canDashTrue()
    {
        canDash = true;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "sword")
        {
            SwordScript swordScript = sword.GetComponent<SwordScript>();
            if (swordScript.called)
            {
                iCallBackTheSword();
                inCatchS = false;
                swordScript.gameObject.SetActive(false);
            }
        }
    }
    private void CheckCanJump()
    {
        isGround = bxColl.IsTouchingLayers(LayerMask.GetMask("Ground"));
        isCapGround = circColl.IsTouchingLayers(LayerMask.GetMask("Ground"));
        isDashGround = circColl.IsTouchingLayers(LayerMask.GetMask("DashEffect"));

        anim.SetBool("onGround", isGround);

        if (isGround)
        {
            canJump = true;
        }
        else
        {
            if (rb.velocity.y > Mathf.Epsilon || Input.GetAxis("Vertical") < 0f)
            {
                canJump = false;
            }
            if (canJump)
                Invoke("alarm0", 0.15f);
        }
    }
    private void FlipSprite()
    {
        if (Mathf.Abs(rb.velocity.x) > Mathf.Epsilon)
        {
            transform.localScale = new Vector3(Mathf.Sign(rb.velocity.x), 1f, 1f);
        }
    }
    private void Dash()
    {
        if (isDashing) return;
        if (Input.GetKeyDown(KeyCode.A))
        {
            Invoke("alarm1", .11f);
            isDashing = true;
        }
    }
    private void Jumpin()
    {
        if (!canJump) return;
        if (Input.GetKeyDown(KeyCode.X))
        {
            Vector2 jpVelocity = new Vector2(0f, jsp);
            rb.velocity = jpVelocity;
            canJump = false;
        }
    }
    private void Movin()
    {
        float hAxis = Input.GetAxis("Horizontal");
        Vector2 pVelocity = new Vector2(!isDashing ? hAxis * hsp : (hAxis * 4f) * hsp, rb.velocity.y);
        rb.velocity = pVelocity;
    }
    private void alarm0()
    {
        canJump = false;
    }
    private void alarm1()
    {
        isDashing = false;
    }
    private void animEnd()
    {
        anim.speed = 0;
    }
    private void AttackOneFinish()
    {
        if (inAttack1)
        {
            inAttack1 = false;
        }
        if (inChildLegAnim)
        {
            inChildLegAnim = false;
        }

    }
    private void AttackTwoFinish()
    {
        if (inAttack2)
        {
            inAttack2 = false;
        }

    }
    public bool checkSwordReturnPosition()
    {
        bool getHorizontalGround = playerChill.checkHorizontalGround();
        bool rbvelox = Mathf.Abs(rb.velocity.x) > 1;
        return getHorizontalGround && rbvelox;
    }
    public bool checkVerticalGround()
    {
        bool getHorizontalGround = playerChill.checkHorizontalGround();
        return getHorizontalGround;
    }

    public void iCallBackTheSword()
    {
        haveSword = true;
    }
    private void inCatchFalse()
    {
        inCatchS = false;
    }
    public void instantiateSword()
    {
        float vG;
        if (haveSword)
        {

            if (!checkVerticalGround()) vG = transform.localScale.x / 5;
            else vG = 0;

            inThrowS = false;
            haveSword = false;
            sword = Instantiate(objSword, new Vector2(transform.position.x + swordPosX + vG, transform.position.y + swordPosY), Quaternion.identity);
            //swordScript _swordScript = sword.GetComponent<swordScript>();
            sword.transform.SetParent(myGrid.transform);
            Rigidbody2D swordRB = sword.GetComponent<Rigidbody2D>();
            sword.transform.localScale = transform.localScale;
            swordRB.velocity = new Vector2(transform.localScale.x * swordSpeed, swordRB.velocity.y);
            swordRB.gravityScale = swordGrav;
        }
    }
    void fInChildLegAnimTrue()
    {
        if (!inChildLegAnim)
        {
            inChildLegAnim = true;
        }
    }
    void fInChildLegAnimFalse()
    {
        if (inChildLegAnim)
        {
            inChildLegAnim = false;
        }
    }
    public void DashEff()
    {
        if (Mathf.Abs(rb.velocity.x) > 1 || Mathf.Abs(rb.velocity.y) > 1)
        {
            GameObject dashEff;
            LegAnimationScript legScr;
            legScr = objLeg.GetComponent<LegAnimationScript>();
            //Bu kadar kısa sürede çok fazla instantiate kullanmak oyunun performansını düşürüyor.
            //Using Instantiate so much in such a short time reduces performance  
            dashEff = Instantiate(objDashEff, transform.position, Quaternion.identity);
            SpriteRenderer dashEffSR = dashEff.GetComponent<SpriteRenderer>();
            dashEff.transform.SetParent(myGrid.transform);
            dashEff.transform.localScale = transform.localScale;
            dashEffSR.sprite = sprRend.sprite;
            legScr.DashEff();
        }
    }
}