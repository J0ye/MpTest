using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody))]
public class SimplePlayer : NetworkBehaviour
{
    [Range(1f, 50f)]
    public float speed = 10f;
    [Header("Attack Settings")]
    public GameObject projectilePrefab;
    [Range(0f, 50f)]
    public float bulletForce = 1f;
    [Range(0f, 5f)]
    public float cooldown = 0.5f;
    [Header("Respawn Settings")]
    [Range(0f, 5f)]
    public float timeToRespawn = 0.5f;
    public Vector3 respawnPosition = Vector3.zero;
    public bool debug = false;

    private Rigidbody rb;
    private bool shotReady = true;
    private bool mirrorGravity = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (!isLocalPlayer)
        {
            gameObject.tag = "Enemy";
            Material newMat = Resources.Load<Material>("Materials/Enemy");
            gameObject.GetComponent<MeshRenderer>().material = newMat;
        }

    }


    void Update()
    {
        if(isLocalPlayer)
        {
            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxis("Horizontal");
            if(v != 0 || h != 0)    rb.velocity = new Vector3(h * speed, rb.velocity.y, v * speed);
            
            if(Input.GetButtonDown("Fire1") && shotReady) Shoot(); // Shoot on Press Enter

            if(Input.GetButtonDown("Jump")) mirrorGravity = !mirrorGravity; // Mirror gravity on Press Spacebar
        }

        if(mirrorGravity)
        {
            rb.AddForce(Vector3.up * (Physics.gravity.y * -2), ForceMode.Force);
        }

        if(debug) TowardsEnemy();             
    }

    [Command]
    public void Hit()
    {
        gameObject.SetActive(false);
        gameObject.transform.position = respawnPosition;
        StartCoroutine(Respawn());
        if (debug) Debug.Log(gameObject.name + " hase been hit.");
    }

    [Command]
    private void Shoot()
    {
        var bullet = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        NetworkServer.Spawn(bullet);
        bullet.GetComponent<Rigidbody>().AddForce(TowardsEnemy() * bulletForce, ForceMode.Impulse);
        StartCoroutine(ReadyShot());
        
    }

    private IEnumerator ReadyShot()
    {
        shotReady = false;
        yield return new WaitForSeconds(cooldown);
        shotReady = true;
    }

    private IEnumerator Respawn()
    {     
        yield return new WaitForSeconds(timeToRespawn);

        gameObject.SetActive(true);
    }

    private Vector3 TowardsEnemy()
    {
        GameObject[] enemys;
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        Vector3 dir = Vector3.up;
        if(enemys.Length > 0)
        {            
            int rand = UnityEngine.Random.Range(0, enemys.Length);
            dir = enemys[rand].transform.position - transform.position;
        }
        if(debug) Debug.DrawRay(transform.position, dir*2, Color.red, 0.1f, false);
        return dir;        
    }
}
