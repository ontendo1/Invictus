using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegAnimationScript : MonoBehaviour
{
    GameObject _player;
    GameObject _playerChillObj;
    [SerializeField] GameObject objDashEff;
    [SerializeField] GameObject myGrid;
    PlayerScript playerScript;
    Animator anim;
    SpriteRenderer spr;
    PlayerChill playerChillScript;
    void Start()
    {
        _player = FindObjectOfType<PlayerScript>().gameObject;
        _playerChillObj = FindObjectOfType<PlayerChill>().gameObject;
        playerChillScript = _playerChillObj.GetComponent<PlayerChill>();
        playerScript = _player.GetComponent<PlayerScript>();
        anim = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        anim.SetBool("onGround", playerScript.isGround);
        anim.SetBool("haveSword", playerScript.haveSword);
        anim.SetBool("onHorizontalGround", playerChillScript.checkHorizontalGround());
        anim.SetFloat("Speed", Mathf.Abs(playerScript.rb.velocity.x));
        spr.enabled = isVisible();
        anim.enabled = isVisible();
    }
    public bool isVisible()
    {
        return playerScript.inChildLegAnim;
    }
    public void DashEff()
    {
        if (spr.enabled && anim.enabled)
        {
            GameObject _legDashEff;
            _legDashEff = Instantiate(objDashEff, transform.position, Quaternion.identity);
            SpriteRenderer _dashEffSR = _legDashEff.GetComponent<SpriteRenderer>();
            _legDashEff.transform.SetParent(myGrid.transform);
            _legDashEff.transform.localScale = transform.localScale;
            _dashEffSR.sprite = spr.sprite;
        }

    }
}
