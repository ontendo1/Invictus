using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChill : MonoBehaviour
{
    BoxCollider2D bxC;
    bool onHorizontalGround;
    void Start()
    {
        bxC = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        onHorizontalGround = bxC.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }
    public bool checkHorizontalGround()
    {
        return onHorizontalGround;
    }
}
