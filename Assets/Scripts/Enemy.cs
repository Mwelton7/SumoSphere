using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    private Rigidbody enemyRb;
    private GameObject player;
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.Find("Player");
        //if player is not too far off the ground or too close to the edge enemy will chase
        if (player.transform.position.y < 3.0f && player.transform.position.x > -10.0f && player.transform.position.x < 10.0f
            && player.transform.position.z > -10.0f && player.transform.position.z < 10.0f)
        {
            Vector3 lookDirection = (player.transform.position - transform.position).normalized;
            enemyRb.AddForce(lookDirection * speed);
            
        }
        //otherwise head to wards origin point
        else
        {
            Vector3 lookDirection = new Vector3(0f, 0f, 0f);
            lookDirection = (transform.position + lookDirection).normalized * -1;
            enemyRb.AddForce(lookDirection * speed);
        }
        

        
        //if enemy has fallen of stage destroy
        if(transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }
}
