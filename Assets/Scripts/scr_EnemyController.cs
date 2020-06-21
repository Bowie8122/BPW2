using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public enum EnemyState
{
    Offline,
    Wander,
    Follow,
    Die
};

public class scr_EnemyController : MonoBehaviour
{

    GameObject player;
    public EnemyState currentstate = EnemyState.Offline;
    public float range;
    public float speed;
    private bool chooseDir = false;
    private bool dead = false;
    private Vector3 randomDir;
    public bool notInRoom = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");  
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentstate)
        {
            case (EnemyState.Offline):
                Offline();
            break;
            case (EnemyState.Wander):
                Wander();
            break;
            case (EnemyState.Follow):
                Follow();
            break;
            case (EnemyState.Die):
                Die();
            break;
        }

        if (!notInRoom)
        {
            if (IsInRange(range) && currentstate != EnemyState.Die)
            {
                currentstate = EnemyState.Follow;
            }
            else if (!IsInRange(range) && currentstate != EnemyState.Die)
            {
                currentstate = EnemyState.Wander;
            }
        } else
        {
            currentstate = EnemyState.Offline;
        }
    }

    private bool IsInRange(float range)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }

    private IEnumerator ChooseDirection()
    {
        chooseDir = true;
        yield return new WaitForSeconds(Random.Range(2f, 8f));
        randomDir = new Vector3(0, 0, Random.Range(0, 360));
        Quaternion nextRotation = Quaternion.Euler(randomDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, nextRotation, Random.Range(.5f, 2.5f));
        chooseDir = false;
    }
    void Wander()
    {
        if (!chooseDir)
        {
            //StartCoroutine(ChooseDirection());
        }

        transform.position += -transform.right * speed * Time.deltaTime;
        if (IsInRange(range))
        {
            currentstate = EnemyState.Follow;
        }
    }

    void Follow()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    void Die()
    {
    }

    void Offline()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
