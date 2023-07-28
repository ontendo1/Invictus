using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridRotationController : MonoBehaviour
{
    [SerializeField] GameObject grid;
    Vector3 rot = new Vector3(0f, 0f, 0f);
    public Transform[] childs;
    public Rigidbody2D[] rigids;
    int i = 0;
    bool rotation = false;
    void Start()
    {
        childs = grid.GetComponentsInChildren<Transform>();
        rigids = grid.GetComponentsInChildren<Rigidbody2D>();
    }
    void Update()
    {
        Clicked();
    }

    private void Clicked()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (!rotation)
            {
                rotation = true;
                Physics2D.gravity = new Vector2(0f, 0f);
                foreach (Rigidbody2D ob in rigids)
                {
                    if (ob.bodyType != RigidbodyType2D.Static)
                        ob.velocity = new Vector2(0f, 0f);
                }
                if (GameObject.Find("swordChill"))
                {
                    GameObject swordChill = GameObject.Find("swordChill");
                    SwordColliderScript swordColliderScript = swordChill.GetComponent<SwordColliderScript>();
                    swordColliderScript.rotateOffset();
                }
            }
        }

        if (rotation)
        {
            if (i < 90)
            {
                grid.transform.localEulerAngles += new Vector3(0f, 0f, 2f);
                foreach (Transform ob in childs)
                {
                    if (ob.name != "Tilemap" && ob.tag != "anyChild")
                        ob.localEulerAngles += new Vector3(0f, 0f, -1f);
                }
                i++;
            }
            else
            {
                Physics2D.gravity = new Vector2(0f, -54f);

                rotation = false;
                i = 0;
            }
        }
    }
}