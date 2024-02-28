using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ShotgunHitbox : MonoBehaviour
{
    private float damage = 5;
    private bool shotIsOnCooldown = false;
    private GameObject playerCh;
    private float knockback = 10000.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Turns the shotgun's collider off on start
        GetComponent<Collider>().enabled = false;
        playerCh = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Swings the scythe when RMB is clicked
        if (Input.GetMouseButtonDown(1) && !shotIsOnCooldown)
        {
            GetComponent<Collider>().enabled = true;
            shotIsOnCooldown = true;
            Invoke("EndShot", 0.1f);
            Invoke("EndShotCooldown", 0.8f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Damages an enemy and knocks it back if it is in the scythe's range when swung.
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyBehavior enemyScrp = other.gameObject.GetComponent<EnemyBehavior>();
            enemyScrp.health -= damage;
            Rigidbody enemyRb = other.gameObject.GetComponent<Rigidbody>();
            enemyRb.AddForce(playerCh.transform.forward * knockback * Time.deltaTime, ForceMode.Impulse);
        }
    }

    // Ends the shotgun's shot after some time
    private void EndShot()
    {
        GetComponent<Collider>().enabled = false;
    }

    // Allows the player to fire again after a cooldown
    private void EndShotCooldown()
    {
        shotIsOnCooldown = false;
    }

}
    