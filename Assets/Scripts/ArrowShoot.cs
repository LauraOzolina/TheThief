﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ArrowShoot : MonoBehaviour
{
    public GameObject thePlayer, theCounter, theMoney;
    public GameObject arrow;
    PlayerController playerScript;
    public Transform arrow_spawner;
    public GameObject go;
    GameObject bow;
    Camera cam;
    public bool arrowExists;
    public int arrows_available;
    public Text counter;
    Text money_count;
    // Start is called before the first frame update
    void Start()
    {
        thePlayer = GameObject.FindWithTag("Player");
        theCounter = GameObject.Find("Counter");
        theMoney = GameObject.Find("Money");
        bow = GameObject.FindWithTag("bow");
        playerScript = thePlayer.GetComponent<PlayerController>();
        arrow.SetActive(false);
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        arrowExists = false;
        arrows_available = 5;

        theCounter.GetComponent<TMPro.TextMeshProUGUI>().text = "Arrows:" + arrows_available;

        theMoney.GetComponent<TMPro.TextMeshProUGUI>().text = "Money:" + 0;
    }

    // Update is called once per frame
    void Update()
    {

        theMoney.GetComponent<TMPro.TextMeshProUGUI>().text = "Money:" + playerScript.money;
        if (playerScript.showWeapon1 == true)
        {
            if ((arrowExists == false) && (arrows_available > 0))
            {

                go = Instantiate(arrow, arrow_spawner.position, Quaternion.identity) as GameObject;
                Rigidbody rb = go.GetComponent<Rigidbody>();
                go.GetComponent<BoxCollider>().enabled = false;
                rb.useGravity = false;

                go.SetActive(true);
                go.transform.SetParent(arrow_spawner.transform);
                arrowExists = true;


                theCounter.GetComponent<TMPro.TextMeshProUGUI>().text = "Arrows:" + arrows_available;
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



            Rigidbody rb = go.GetComponent<Rigidbody>();
            rb.velocity = cam.transform.forward * playerScript.powerBar.value;
            rb.useGravity = true;
            go.GetComponent<BoxCollider>().enabled = true;
            //rb.isKinematic = false;
            go.tag = "arrowshot";
            playerScript.shootArrow = false;
            go.transform.SetParent(null);
            playerScript.powerBar.value = 0f;
            arrows_available -= 1;
            theCounter.GetComponent<TMPro.TextMeshProUGUI>().text = "Arrows:" + arrows_available;
            //arrowExists = false;
        }


    }

}
