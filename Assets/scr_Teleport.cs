using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scr_Teleport : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D collision)
    {
        SceneManager.LoadScene("OverWorld_BossBattle");   
    }


}
