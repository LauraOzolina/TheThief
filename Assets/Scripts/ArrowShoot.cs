using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShoot : MonoBehaviour
{
    GameObject thePlayer;
    public GameObject arrow;
    PlayerController playerScript;
    public Transform arrow_spawner;
    public GameObject go;
    GameObject bow;
    Camera cam;
    public bool arrowExists;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = GameObject.FindWithTag("Player");
        bow = GameObject.FindWithTag("bow");
        playerScript = thePlayer.GetComponent<PlayerController>();
        arrow.SetActive(false);
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        arrowExists = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (playerScript.showWeapon1 == true)
        {
            if (arrowExists == false)
            {

                go = Instantiate(arrow, arrow_spawner.position, Quaternion.identity) as GameObject;
                Rigidbody rb = go.GetComponent<Rigidbody>();

                rb.useGravity = false;
                go.SetActive(true);
                go.transform.SetParent(arrow_spawner.transform);
                arrowExists = true;

            }
            //go.transform.rotation = Quaternion.AngleAxis(bow.transform.position.y, Vector3.up);
            if (playerScript.shootArrow == false)
            {


                go.transform.LookAt(cam.transform);
            }

        }
        if (playerScript.showWeapon1 == false)
        {
            arrowExists = false;
            Destroy(go);
        }
        if (playerScript.shootArrow == true)
        {
            go.tag = "arrowshot";
            playerScript.shootArrow = false;
            go.transform.SetParent(null);
            Rigidbody rb = go.GetComponent<Rigidbody>();
            rb.velocity = cam.transform.forward * playerScript.powerBar.value;
            rb.useGravity = true;

            //arrowExists = false;
        }


    }

}
