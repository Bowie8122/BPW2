using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_ArrowPickup : MonoBehaviour
{

    public int ArrowID;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponentInChildren<Scr_Bow>().AddArrow(ArrowID);
            Destroy(gameObject);
        }
    }
}
