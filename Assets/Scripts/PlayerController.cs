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
    [SerializeField] bool lockCursor = true;
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

    void Start()
    {

        powerBar = GameObject.Find("Power").GetComponent<Slider>();
        powerBar.value = 0f;
        powerBar.maxValue = 20f;
        controller = GetComponent<CharacterController>();
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        showWeapon1 = false;
        showWeapon2 = false;
    }

    void Update()
    {
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

}