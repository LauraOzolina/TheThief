using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemies : MonoBehaviour
{
    public NavMeshAgent enemy;
    public Transform player;
    private Transform target = null;
    public int maxRange;
    public int minRange;
    public Animator anim;
    public GameObject go, engo, thePlayer,loot, theMoney;
    PlayerController playerScript;
    public int ecount, xpos, zpos;
    public float health;
    public bool attack_status;
    public Vector3 dead_position;
    public GameObject mon;
    // Start is called before the first frame update
    void Start()
    {

        thePlayer = GameObject.FindWithTag("Player");

        playerScript = thePlayer.GetComponent<PlayerController>();
        ecount = 0;
        anim = GetComponent<Animator>();
        anim.SetBool("isRuning", false);
        Debug.Log("setojam false");
        engo = GameObject.FindWithTag("enemy");
        health = 100f;
        attack_status = false;
        theMoney = GameObject.Find("Money");
        Debug.Log(mon);
    }

    // Update is called once per frame
    void Update()
    {
        theMoney.GetComponent<TMPro.TextMeshProUGUI>().text = "Money:" + playerScript.money;
        float dist = Vector3.Distance(transform.position, player.position);
        //print("Distance to other: " + dist);
        if (transform.tag == "enemy")
        {
            if ((dist < 10) & (dist >= 2))
            {

                //target = other.transform;

                //anim.SetBool("isRuning", true);
                anim.Play("Treadmill Running");
                enemy.SetDestination(player.position);




            }
            else if (dist < 2)
            {

                if (playerScript.stabbingEnemy == true)
                {
                    anim.Play("Rib Hit");
                    Debug.Log(health);
                    health -= 20f;
                    if (health == 0f)
                    {
                        anim.Play("Take 001");
                        transform.tag = "enemyfinished";
                        dead_position = transform.position;
                     
                        loot = Instantiate(mon, dead_position, Quaternion.identity) as GameObject;
                        loot.SetActive(true);

                        Debug.Log(loot);
                    }
                    
                    playerScript.stabbingEnemy = false;
                }
                else
                {
                    anim.Play("Punching");
                    if (playerScript.playerHealth > 0f)
                    {
                        if (attack_status == false)
                        {
                            StartCoroutine(Attack());
                        }
                    }
                    else
                    {
                        playerScript.lockCursor = false;
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                        Debug.Log("You died!");
                        StopCoroutine(Attack());

                    }

                }
                enemy.velocity = Vector3.zero;
            }
            else
            {
                target = null;

                // anim.SetBool("isRuning", false);
                anim.Play("Breathing Idle");


                enemy.velocity = Vector3.zero;

                return;
            }
        }



    }

    IEnumerator Attack()
    {
        if (attack_status == false)
        {
            attack_status = true;
            Debug.Log("Attack!");
            playerScript.playerHealth -= 20f;
            Debug.Log(playerScript.playerHealth);
            // do stuff 


            yield return new WaitForSeconds(1f);
            attack_status = false;
        }
    }


}
