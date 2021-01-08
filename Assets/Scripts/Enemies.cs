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
    GameObject go,engo,thePlayer;
    PlayerController playerScript;
    public int ecount, xpos, zpos;
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
    
      

    }

    // Update is called once per frame
    void Update()
    {

        float dist = Vector3.Distance(transform.position, player.position);
        //print("Distance to other: " + dist);
        if (transform.tag == "enemy")
        {
            if ((dist < 10)&(dist < 10)& (dist >= 2))
            {

                //target = other.transform;

                //anim.SetBool("isRuning", true);
                anim.Play("Treadmill Running");
                enemy.SetDestination(player.position);




            }
            else if(dist < 2)
            {
               
                if (playerScript.stabbingEnemy == true)
                {
                    anim.Play("Rib Hit");
                }
                else
                {
 anim.Play("Punching");
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




}
