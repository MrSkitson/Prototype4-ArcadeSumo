using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private GameObject focalPoint;
    public float speed = 5.0f;
    public bool hasPower = false;
    private float powerupStrenght = 15.0f;
    public GameObject powerupIndicator;
    
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
    }
// If Player collides with powerup, activate powerup
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("PowerUp"))
        {
            hasPower = true;
            powerupIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerCountdownRoutine());
        }
    }
    IEnumerator PowerCountdownRoutine() {
        yield return new WaitForSeconds(7);
        hasPower = false;
        powerupIndicator.gameObject.SetActive(false);
    }
 // If Player collides with enemy
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Enemy") && hasPower)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);

            Debug.Log("Colloded with " + collision.gameObject.name + " wtith powerup set to " + hasPower);
            enemyRigidbody.AddForce(awayFromPlayer * powerupStrenght, ForceMode.Impulse);

        }
    }
}
