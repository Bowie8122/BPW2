using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public enum BossState
{
    Offline,
    Follow,
    RocketAttack,
    CarMode
};

public class scr_BossController : MonoBehaviour
{

    GameObject player;
    public BossState currentstate = BossState.Offline;
    public float range;
    public float speed;
    public bool notInRoom = true;
    public bool invincible = false;
    private bool DB = false;
    Vector3 StartingPos;
    private Vector3 randomDir;
    public Sprite[] States;
    public SpriteRenderer Sprite;
    public GameObject CarObject;
    public int Health;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartingPos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentstate)
        {
            case (BossState.Offline):
                Offline();
            break;
            case (BossState.Follow):
                Follow();
            break;
            case (BossState.RocketAttack):
                RocketMode();
            break;
            case (BossState.CarMode):
                CarMode();
            break;
        }

        if (!notInRoom)
        {
            if (currentstate == BossState.Offline)
            {
                currentstate = BossState.Follow;
                StartCoroutine(StateTimer(Random.Range(4f, 8f)));
            }
        } else
        {
            currentstate = BossState.Offline;
        }
    }

    void Follow()
    {
        if (invincible) { invincible = false; }
        if (Sprite.sprite != States[0]) { Sprite.sprite = States[0]; }
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        StartCoroutine(StateTimer(Random.Range(4f, 8f)));
    }

    void Offline()
    {

    }

    public void GetHit()
    {
        if (!invincible)
        {
            Health--;
            Debug.Log("OUWCH");
            invincible = true;
            DB = false;
            StartCoroutine(StateTimer(.5f));
            if (Health < 0)
            {
                Destroy(gameObject);
            }
        }
    }
    void RocketMode()
    {
        if (!invincible) { invincible = true; }
        if (randomDir == new Vector3(0,0,0))
        {
            randomDir = new Vector3(0, 0, Random.Range(0, 360));
            Quaternion nextRotation = Quaternion.Euler(randomDir);
            transform.rotation = Quaternion.Lerp(transform.rotation, nextRotation, Random.Range(.5f, 2.5f));
            if (Sprite.sprite != States[1]) { Sprite.sprite = States[1]; }
            StartCoroutine(StateTimer(1.5f));
        }
        transform.position += -transform.right * speed* 4 * Time.deltaTime;
    }

    void CarMode()
    {
        if (!DB)
        {
            DB = true;
            int rng = Random.Range(1, 12);
            if (!invincible) { invincible = true; }
            if (Sprite.sprite != States[2]) { Sprite.sprite = States[2]; }
            StartCoroutine(SpawnCar(rng));
            DB = false;
            StartCoroutine(StateTimer(2f + (.2f*rng)));
        }
    }

    IEnumerator SpawnCar(int rng)
    {
        for (int i = 0; i < rng; i++)
        {
            Vector3 XPosition = new Vector3(15, 0, 0);
            Vector3 YPosition = new Vector3(0, Random.Range(-4, 4), 0);
            GameObject Car = Instantiate(CarObject, transform.position + XPosition + YPosition, Quaternion.identity, transform);
            Destroy(Car, 5f);
            yield return new WaitForSeconds(.2f);
        }
    }
    IEnumerator StateTimer(float time)
    {
        if (!DB)
        {
            DB = true;
            yield return new WaitForSeconds(time);
            SwitchToRandomState();
            DB = false;
        }
    }
    void SwitchToRandomState()
    {
        BossState TempState = PickState();
        if (currentstate == TempState) { TempState = PickState(); } //Gives 2x less chance to get a double value
        //Move the Boss to the middle of the room
        gameObject.transform.position = StartingPos;
        randomDir = new Vector3(0, 0, 0);
        Quaternion nextRotation = Quaternion.Euler(randomDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, nextRotation, Random.Range(.5f, 2.5f));
        currentstate = TempState;
        Debug.Log(currentstate);
    }

    BossState PickState()
    {
        BossState[] states = new BossState[]{ BossState.Follow, BossState.RocketAttack, BossState.CarMode };
        return states[Random.Range(0,states.Length)];
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Scene_Overworld");
        }
    }
}
