using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private LayerMask groundMask;

    private Camera mainCamera;
    private WallController wallController;

    private float horizontalInput;
    private float verticalInput;
    public Transform rightWall;
    public Transform leftWall;
    public Transform frontWall;
    public Transform backWall;
    private Rigidbody playerRb;
    public float speed = 20.0f;

    public bool rocketIsOnCooldown = false;
    public bool selfDamageIsOnCooldown = true;
    public Transform rocketSpawnPoint;
    public Quaternion rocketSpawnDirection;
    public GameObject rocketPrefab;
    public GameObject rocketHolder;
    public int rocketCount = 2;
    public float health = 100;
    

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        playerRb = GetComponent<Rigidbody>();
        wallController = GameObject.Find("Front Wall").GetComponent<WallController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        // Left and right movement
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * Time.deltaTime * horizontalInput * speed, Space.World);

        // Forward and backward movement
        verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.forward * Time.deltaTime * verticalInput * speed, Space.World);
        
        // Shoots a rocket when LMB is clicked
        if (Input.GetMouseButtonDown(0) && rocketCount > 0 && !rocketIsOnCooldown)
        {
            Instantiate(rocketPrefab, rocketSpawnPoint.position, rocketSpawnPoint.rotation, rocketHolder.transform);
            --rocketCount;
        }

        if (rocketCount == 0)
        {
            rocketIsOnCooldown = true;
            Invoke("RocketReload", 0.5f);
            Invoke("RocketCooldown", 1);
        }

        if (health < 0.1f) 
        {
            Destroy(gameObject);
        }

        if (transform.position.y > 2)
        {
            transform.position = new Vector3 (transform.position.x, 2, transform.position.z);
            playerRb.AddForce(Vector3.down * 10, ForceMode.Impulse);
        }

        // Activates "facemouse" every update
        facemouse();

        // Activates wall collisions every update
        WallCollision();

    }

    // Resets the cooldown for shooting rockets
    private void RocketReload()
    {
        rocketCount = 2;
    }

    private void RocketCooldown()
    {
        rocketIsOnCooldown = false;
    }

    // Creates a raycast that hits the ground and turns the player toward its end position
    private void facemouse()
    {
        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength)) 
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.red);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
    }

    // Makes Player "collide" with walls
    void WallCollision()
    {
        if (transform.position.x > rightWall.position.x - 1)
        {
            transform.position = new Vector3(rightWall.position.x - 1, transform.position.y, transform.position.z);
        }

        if (transform.position.x < leftWall.position.x + 1)
        {
            transform.position = new Vector3(leftWall.position.x + 1, transform.position.y, transform.position.z);
        }

        if (transform.position.z < backWall.position.z + 1)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, backWall.position.z + 1);
        }

        if (transform.position.z > frontWall.position.z - 1 && wallController.wallDown)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, frontWall.position.z - 1);
        }
    }
    
}
