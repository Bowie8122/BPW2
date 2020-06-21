using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Bow : MonoBehaviour
{
    public Rigidbody2D rb;
    public Camera cam;

    public GameObject Bow;
    public GameObject Player;

    private Vector2 mousePos;

    public Transform firepoint;
    public GameObject Arrow;
    public float bulletforce = 20f;

    public int[] Arrows;
    public GameObject[] ArrowGui;

    // Update is called once per frame
    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Bow.transform.position = Player.transform.position;

        //Fire Bow
        if (Input.GetButtonDown("Fire1"))
        {
            //Check if the player has a arrow
            if (Arrows[0] != 0)
            {
                ShootArrow();
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;// - 90f;
        rb.rotation = angle;
    }

    void ShootArrow()
    {
        GameObject arrow = Instantiate(Arrow, firepoint.position, firepoint.rotation);
        Rigidbody2D arrowrb = arrow.GetComponent<Rigidbody2D>();
        arrowrb.AddForce(firepoint.up * bulletforce, ForceMode2D.Impulse);
        //Remove the Arrow from the list
        Arrows[0] = Arrows[1];
        Arrows[1] = Arrows[2];
        Arrows[2] = 0;
        UpdateArrowGui();
    }

    public void AddArrow(int id)
    {
        for (int i = 0; i < Arrows.Length; i++)
        {
            if (Arrows[i] == 0)
            {
                Arrows[i] = 1;
                break;
            }
        }
        UpdateArrowGui();
    }

    void UpdateArrowGui()
    {
        for (int i = 0; i < Arrows.Length; i++)
        {
            if (Arrows[i] == 0)
            {
                ArrowGui[i].SetActive(false);
            } else
            {
                ArrowGui[i].SetActive(true);
            }
        }
    }
}
