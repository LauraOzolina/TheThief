using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Stick : MonoBehaviour
{
    GameObject arrow, arrowhit, ar, ed, go;
    public GameObject mon;
    Rigidbody rb, rbhit;
    Camera cam;
    float hitLocy;
    float hitLocx;
    float hitLocz;
    ArrowShoot arrowScript;
    Animator enemyAnim;
    public bool arrowStop;
    public bool isEnemyShot;
    public Vector3 dead_position;
    void Start()
    {

        ar = GameObject.FindWithTag("bow");

        mon.SetActive(false);
        arrowScript = ar.GetComponent<ArrowShoot>();

        arrowStop = false;
        isEnemyShot = false;

        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();

    }
    void Update()
    {
        if (arrowStop == true)
        {
            arrowhit = GameObject.FindWithTag("arrowhit");
            rbhit = arrowhit.GetComponent<Rigidbody>();


            if (isEnemyShot == true)
            {
                Debug.Log("enemy rotation");
                isEnemyShot = false;
                arrowhit.transform.rotation = Quaternion.Euler(0, 90, 0);
                rbhit.useGravity = true;
            }
            else
            {


                arrowhit.transform.rotation = Quaternion.Euler(hitLocx, hitLocy, hitLocz);
 rbhit.constraints = RigidbodyConstraints.FreezeAll;

            }
           
            arrowStop = false;
            arrowScript.arrowExists = false;
            arrowhit.tag = "done";
        }



    }
    void OnCollisionEnter(Collision col)
    {

        if (col.gameObject.tag != "Player")
        {
            Debug.Log(col.gameObject.tag);
            Debug.Log(col.gameObject);

            arrow = GameObject.FindWithTag("arrowshot");
            if (arrow != null)
            {
                arrow.tag = "arrowhit";
                arrowhit = GameObject.FindWithTag("arrowhit");
                rbhit = arrowhit.GetComponent<Rigidbody>();
                rbhit.velocity = Vector3.zero;
                hitLocx = arrowhit.transform.rotation.eulerAngles.x;
                hitLocy = arrowhit.transform.rotation.eulerAngles.y;
                hitLocz = arrowhit.transform.rotation.eulerAngles.z;

                arrowStop = true;
                if (col.gameObject.tag == "enemy")
                {
                    isEnemyShot = true;
                    Debug.Log(isEnemyShot);
                    col.gameObject.tag = "enemydead";
                    Debug.Log(col.gameObject.tag);
                    ed = GameObject.FindWithTag("enemydead");

                    enemyAnim = ed.GetComponent<Animator>();
                    ed.GetComponent<BoxCollider>().enabled = false;
                    dead_position = ed.transform.position;
                    //enemyAnim.SetBool("isDead", true);
                    enemyAnim.Play("Take 001");
                    ed.tag = "enemyfinished";
                    Debug.Log("iesauts enemy");
                    Debug.Log(dead_position);
                    //ed.GetComponent<NavMeshAgent>().enabled = false;
                    go = Instantiate(mon, dead_position, Quaternion.identity) as GameObject;
                    go.SetActive(true);
                    arrowhit.transform.SetParent(ed.transform);
                }
            }






        }
        //Stuff that happens when the collider collides with something

    }
}
