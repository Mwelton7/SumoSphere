using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    public float speed = 5.0f;
    private GameObject focalPoint;
    public bool hasPowerup;
    public float powerupStrength;
    public GameObject powerupIndicator;  //Grow powerup
    public GameObject walls;             //Walls "+" powerup
    private GameObject player;
    public GameObject island;
    private int count = 0;
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
        
    }

    // Update is called once per frame
    void Update()
    {
        //Find Player and if it has fallen off the stage reset to 0,0,0 and 0 velocity
        player = GameObject.Find("Player");
        if (player.transform.position.y < -10.0f)
        {
            playerRb.velocity = new Vector3(0f, 0f, 0f);
            player.transform.position = new Vector3(0, 3.0f, 0);
        }
        //move towards the transform of the focalpoint and forward input
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);

        //Move indicator to base of player
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
        //Z to jump
        if (Input.GetKeyDown("z"))
        {
            playerRb.velocity = new Vector3(0f, 0f, 0f);
            playerRb.AddForce(0f, 15f, 0f, ForceMode.Impulse);
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        //If player hits powerup activate indicator add 1 to powerup count 
        if (other.CompareTag("Powerup"))
        {
            count++;
            hasPowerup = true;
            powerupIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);
            //grow the player by 2x
            playerRb.gameObject.transform.localScale += new Vector3(2.0f, 2.0f, 2.0f);
            //start the timer routine
            GameObject growIsland = Instantiate(island, island.transform.position, island.transform.rotation);
            Destroy(growIsland, 7.0f);
                StartCoroutine(PowerupCountdownRoutine());

            
            
        }
        //if powerup tag is wall instantiate walls and destroy after 7 seconds
        if (other.CompareTag("Wall"))
        {
            GameObject wall = Instantiate(walls, walls.transform.position, walls.transform.rotation);
            Destroy(wall, 7.0f);
            Destroy(other.gameObject);
        }
    }
    IEnumerator PowerupCountdownRoutine()
    {
        //wait 7 seconds and then subtract 1 from count 
        yield return new WaitForSeconds(7);
        count--;
        //if the player has collided with the same powerup in the last 7 seconds count will be > 0
        //if this is the case make the timer reset to 7 seconds
        if (count > 0)
        {
            yield return new WaitForSeconds(7);
        }
        //if it made it this far the player has not collided with a powerup in 7 seconds set flag to false and shrink player back
        hasPowerup = false;
        powerupIndicator.gameObject.SetActive(false);
        playerRb.gameObject.transform.localScale += new Vector3(-2.0f, -2.0f, -2.0f);


    }

    private void OnCollisionEnter(Collision collision)
    {   //if player has powerup and collides with an enemy force impulse in opposite direction
        if(collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);

            Debug.Log("Player collided with " + collision.gameObject + " with powerup set to " + hasPowerup);

            enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }
    }
}
