using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
    GameObject arrow, arrowhit, ar, ed;
    Rigidbody rb, rbhit;
    Camera cam;
    float hitLocy;
    float hitLocx;
    float hitLocz;
    ArrowShoot arrowScript;
    Animator enemyAnim;
    public bool arrowStop;
    public bool isEnemyShot;
    void Start()
    {

        ar = GameObject.FindWithTag("bow");

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
            rbhit.constraints = RigidbodyConstraints.FreezeAll;

            arrowhit.transform.rotation = Quaternion.Euler(hitLocx, hitLocy, hitLocz);
            arrowStop = false;
            arrowScript.arrowExists = false;
            arrowhit.tag = "done";
        }



    }
    void OnCollisionEnter(Collision col)
    {

        if (col.gameObject.tag != "Player")
        {

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
                    enemyAnim.SetBool("isDead", true);
                    Debug.Log("iesauts enemy");
                }
            }






        }
        //Stuff that happens when the collider collides with something

    }
}
