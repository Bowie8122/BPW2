using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Arrow : MonoBehaviour
{

    public GameObject ArrowDrop;
    void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(ArrowDrop, gameObject.transform.position, gameObject.transform.rotation, null);
        Destroy(gameObject); 
        if (collision.transform.tag == "Enemy")
        {
            Destroy(collision.gameObject);
        }
        if (collision.transform.tag == "Boss")
        {
            collision.gameObject.GetComponentInChildren<scr_BossController>().GetHit();
        }
    }
}
