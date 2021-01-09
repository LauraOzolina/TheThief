using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform playerCamera = null;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] float walkSpeed = 6.0f;
    [SerializeField] float gravity = -13.0f;
    [SerializeField] [Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
    [SerializeField] [Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;
    [SerializeField] public GameObject weapon1;
    [SerializeField] public GameObject weapon2;
    public bool lockCursor = true;
    [SerializeField] int time;
    public bool showWeapon1;
    public bool showWeapon2;
    float cameraPitch = 0.0f;
    float velocityY = 0.0f;
    CharacterController controller = null;
    public Slider powerBar;
    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;
    public bool shootArrow = false;
    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;
    public int money;
    private bool canpickup;
    Outline outlinescript;
    ArrowShoot arrowScript;
    GameObject ar, dagger, en, dm, mes;
    Animator enemyAnim;
    Enemies enemyScript;
    public bool stabbingEnemy;
    public float playerHealth;
    GameObject[] objs;
    Text message;
    void Start()
    {
        ar = GameObject.FindWithTag("bow");
        en = GameObject.FindWithTag("enemy");
        dm = GameObject.FindWithTag("deadmenu");
        mes = GameObject.Find("Message");
        enemyScript = en.GetComponent<Enemies>();
        dagger = GameObject.FindWithTag("dagger");
        powerBar = GameObject.Find("Power").GetComponent<Slider>();
        powerBar.value = 0f;
        powerBar.maxValue = 20f;
        dm.SetActive(false);
        arrowScript = ar.GetComponent<ArrowShoot>();
        controller = GetComponent<CharacterController>();
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        showWeapon1 = false;
        showWeapon2 = false;
        canpickup = false;
        stabbingEnemy = false;
        money = 0;
        playerHealth = 100;
        objs = GameObject.FindGameObjectsWithTag("enemy");
    }

    void Update()
    {
        if (playerHealth == 0f)
        {
            dm.SetActive(true);
        }
        if (money == 500)
        {
            Debug.Log(mes);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            mes.GetComponent<TMPro.TextMeshProUGUI>().text = "Congrats!";
            dm.SetActive(true);
        }
        if (showWeapon1 == false)
        {
            weapon1.SetActive(false);
        }
        if (showWeapon2 == false)
        {
            weapon2.SetActive(false);
        }
        if (showWeapon1 == true)
        {
            weapon1.SetActive(true);
        }
        if (showWeapon2 == true)
        {
            weapon2.SetActive(true);
        }
        //shows/hides bow
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (showWeapon1 == true)
            {
                showWeapon1 = false;
            }
            else
            {
                showWeapon1 = true;
            }

            showWeapon2 = false;
        }
        //shows/hides dagger
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (showWeapon2 == true)
            {
                showWeapon2 = false;
            }
            else
            {
                showWeapon2 = true;
            }
            showWeapon1 = false;
        }

        //if left mouse button pressed and bow out increase power
        if (showWeapon1 == true)
        {


            if (Input.GetMouseButton(0))
            {
                StartCoroutine(PowerBar());
            }
            if (Input.GetMouseButtonUp(0))
            {
                StopCoroutine(PowerBar());
                shootArrow = true;

            }
        }

        //if left mouse button pressed and knife out - stab
        if (showWeapon2 == true)
        {

            if (Input.GetMouseButton(0))
            {
                StartCoroutine(DaggerMove());
                StopCoroutine(DaggerMove());
            }


        }
        //picks up arrow if its close
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (canpickup == true)
            {
                GameObject[] objs = GameObject.FindGameObjectsWithTag("done");
                GameObject closestArrow = null;
                float closestDistance = 9999;
                bool first = true;

                foreach (var obj in objs)
                {
                    Debug.Log(obj);
                    float distance = Vector3.Distance(obj.transform.position, transform.position);
                    if (first)
                    {
                        closestDistance = distance;
                        closestArrow = obj;
                        first = false;
                    }
                    else if (distance < closestDistance)
                    {
                        closestArrow = obj;
                        closestDistance = distance;
                    }


                }
                Debug.Log(closestArrow);
                Destroy(closestArrow);
                arrowScript.arrows_available += 1;
                arrowScript.theCounter.GetComponent<TMPro.TextMeshProUGUI>().text = "Arrows:" + arrowScript.arrows_available;

                Debug.Log("destroy bulta");
                canpickup = false;
            }

        }
        UpdateMouseLook();
        UpdateMovement();
    }

    void UpdateMouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }

    void UpdateMovement()
    {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        if (controller.isGrounded)
            velocityY = 0.0f;

        velocityY += gravity * Time.deltaTime;

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * walkSpeed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);

    }

    IEnumerator PowerBar()
    {
        yield return new WaitForSeconds(time);
        powerBar.value += 0.5f;

        // do stuff
    }

    IEnumerator DaggerMove()
    {

        dagger.transform.Translate(Vector3.left * 0.09f);

        dagger.transform.Translate(Vector3.right * 0.09f);

        GameObject closestEnemy = null;
        float closestDistance = 9999;
        bool first = true;

        foreach (var obj in objs)
        {
            Debug.Log(obj);
            float distance = Vector3.Distance(obj.transform.position, transform.position);
            if (first)
            {
                closestDistance = distance;
                closestEnemy = obj;
                first = false;
            }
            else if (distance < closestDistance)
            {
                closestEnemy = obj;
                closestDistance = distance;
            }


        }


        stabbingEnemy = true;
        yield return new WaitForSeconds(0f);
        stabbingEnemy = false;
    }
    void OnTriggerEnter(Collider col)
    {


        if (col.tag == "money")
        {
            Destroy(col.gameObject);
            Debug.Log("kolizija ar maisu");
            money += 100;
        }
        if (col.tag == "done")
        {
            outlinescript = col.GetComponent<Outline>();
            Debug.Log(outlinescript.outlineColor);

            outlinescript.outlineFillMaterial.SetColor("_OutlineColor", Color.green);
            Debug.Log("bulta");
            canpickup = true;

        }
    }

    void OnTriggerExit(Collider col)
    {


        if (col.tag == "done")
        {
            outlinescript = col.GetComponent<Outline>();
            Debug.Log(outlinescript.outlineColor);

            outlinescript.outlineFillMaterial.SetColor("_OutlineColor", Color.red);
            Debug.Log("bulta");
            canpickup = false;

        }
    }

}