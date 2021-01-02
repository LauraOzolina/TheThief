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


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("isRuning", false);
        Debug.Log("setojam false");

    }

    // Update is called once per frame
    void Update()
    {

        float dist = Vector3.Distance(transform.position, player.position);
        //print("Distance to other: " + dist);
        if (transform.tag == "enemy")
        {
            if (dist < 10)
            {
           
                //target = other.transform;

                anim.SetBool("isRuning", true);
                enemy.SetDestination(player.position);

            }
            else
            {
                target = null;

                anim.SetBool("isRuning", false);
                return;
            }
        }

        /* if (target == null) return;
           transform.LookAt(target);
           float distance = Vector3.Distance(transform.position, target.position);
           bool tooClose = distance < minRange;
           Vector3 direction = tooClose ? Vector3.back : Vector3.forward;
           transform.Translate(direction * Time.deltaTime *5);*/


    }





}
