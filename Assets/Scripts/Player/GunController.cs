using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{

    public GameObject Bullet;

    public int MaxBullets = 100;
    public Vector3 FiringVelocity = new Vector3(0f, 0f, 0f);
    private GameObject[] bullets;
    private int numberOfBullets = 0;

    private AudioSource audioSFX;
    private BPMTimer Quantiser;
    private int previousTick = 0;

    public GameObject FiringPoint;
    public GameObject BulletParent;

    private void Awake()
    {
        Quantiser = GameObject.Find("Timer").GetComponent<BPMTimer>();
        audioSFX = GetComponent<AudioSource>();
        bullets = new GameObject[MaxBullets];
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {

            if (numberOfBullets < MaxBullets)
            {
                GameObject newBullet = Instantiate(Bullet, FiringPoint.transform.position, FiringPoint.transform.rotation, BulletParent.transform);
                newBullet.GetComponent<Rigidbody>().AddRelativeForce(FiringVelocity, ForceMode.Impulse);
                bullets[numberOfBullets] = newBullet;

                audioSFX.Play();
                numberOfBullets++;
            }
            else
            {
                foreach (var bullet in bullets)
                {
                    Destroy(bullet);
                }
                numberOfBullets = 0;
            }




        }
    }
}
