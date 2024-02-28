using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{

    public float speed = 10.0f;
    private float maxSpeed = 10.0f;
    private float minDistance = 2.0f;
    private float failSafeDistance = -20.0f;
    private float distanceFromPlayer;
    public float health = 100;
    private GameObject Player;
    private Rigidbody enemyRb;
    
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        enemyRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveToPlayer();

        // Stops the enemies from moving too fast
        if (enemyRb.velocity.magnitude > maxSpeed)
        {
            enemyRb.velocity = Vector3.ClampMagnitude(enemyRb.velocity, maxSpeed);
        }

        // Destroys enemies if they glitch through the floor
        if (transform.position.y < failSafeDistance)
        {
            Destroy(gameObject);
        }

        if (health < 1)
        {
            Destroy(gameObject);
        }

        distanceFromPlayer = Vector3.Distance(Player.transform.position, transform.position);
    }

    // Makes the enemies look at and move toward the player, stopping if they are too close
    void MoveToPlayer()
    {
        transform.LookAt(new Vector3 (Player.transform.position.x, transform.position.y, Player.transform.position.z));
        if (distanceFromPlayer > minDistance)
        {
            enemyRb.AddForce(transform.forward * speed);
        }
        else
        {
            enemyRb.AddForce(-transform.forward * speed);
        }
        
    }
}
