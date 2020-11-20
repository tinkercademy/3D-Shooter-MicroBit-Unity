using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipControllerVersion2 : MonoBehaviour
{
    [Header("Movement")]
    public float thrustAmount;
    [HideInInspector] public float thrustInput;

    public float yawSpeed;
    public float pitchSpeed;
    [HideInInspector] public Vector3 steeringInput;

    public float leanAmount_X;
    public float leanAmount_Y;

    [Header("Camera")]
    public float cameraTurnAmount;

    [Header("Physics")]
    public Rigidbody rb;

    public Transform shipModel;

    [Header("Bullet")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 0;

    [Header("One Bullet")]
    public GameObject bulletSpawn;
    private float countdown = 0;
    public float cooldown = 0;

    [Header("Two Bullet")]
    public GameObject twobulletSpawn1;
    public GameObject twobulletSpawn2;
    private float countdown1 = 0;
    public float cooldown1 = 0;

    [Header("Message Listener")]
    public SampleMessageListener msg;

    [Header("Mesh Object")]
    public GameObject invisibleShip;
    public GameObject originalShip;
    public float countdownInvisible;
    public float invisibleTime;
    public float cooldownSkillInvisible;

    [Header("Pause Menu")]
    public GameObject pauseMenu;

    [Header("Respawn")]
    public Transform spawnpoint;

    [Header("Skill Image")]
    public Image skillImageDisplay;

    private void Update()
    {
        countdown -= Time.deltaTime;
        countdown1 -= Time.deltaTime;

        countdownInvisible -= Time.deltaTime;
        if (countdownInvisible < 0f)
            countdownInvisible = 0f;

        skillImageDisplay.fillAmount = (cooldownSkillInvisible - countdownInvisible) / cooldownSkillInvisible;

        if (countdownInvisible < cooldownSkillInvisible - invisibleTime)
        {
            originalShip.SetActive(true);
            invisibleShip.SetActive(false);
        }
        else
        {
            originalShip.SetActive(false);
            invisibleShip.SetActive(true);
        }

        switch (msg.item)
        {
            case 1:
                //Jet Boost ( included inside ShipControllerInputSmoothing )
                SoundManager.instance.PlaySFX("Jet");
                break;
            case 2:
                BulletSpawn();
                break;
            case 3:
                //invisible
                if (countdownInvisible == 0f)
                    countdownInvisible = cooldownSkillInvisible;
                break;
            case 4:
                //two bullet
                TwoBulletSpawn();
                break;
            case 5:
                //pause
                if (Time.timeScale != 0)
                {
                    Time.timeScale = 0;
                    pauseMenu.SetActive(true);
                }
                break;
            case 6:
                //Reset
                transform.position = spawnpoint.position;
                break;
            default:
                SoundManager.instance.StopSFX("Jet");
                break;
        }
    }

    void FixedUpdate()
    {
        MoveSpaceship();
        TurnSpaceship();
    }

    public void UpdateInputData(Vector3 newSteering, float newThrust)
    {
        steeringInput = newSteering;
        thrustInput = newThrust;
    }

    void MoveSpaceship()
    {
        rb.velocity = transform.forward * thrustAmount * (Mathf.Max(thrustInput, .2f));
    }

    void TurnSpaceship()
    {
        Vector3 newTorque = new Vector3(steeringInput.x * pitchSpeed, -steeringInput.z * yawSpeed, 0);
        rb.AddRelativeTorque(newTorque);

        rb.rotation = Quaternion.Slerp(rb.rotation, Quaternion.Euler(new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0)), .5f);

        VisualSpaceshipTurn();
    }

    void VisualSpaceshipTurn()
    {
        shipModel.localEulerAngles = new Vector3(steeringInput.x * leanAmount_Y, shipModel.localEulerAngles.y, steeringInput.z * leanAmount_X);
    }

    void BulletSpawn()
    {
        if (countdown > 0)
            return;

        SoundManager.instance.StopSFX("Bullet");

        GameObject bullet = GameObject.Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        bullet.transform.Rotate(Vector3.left * -90f);
        bullet.GetComponent<Rigidbody>().velocity = bulletSpawn.transform.forward * bulletSpeed;
        countdown = cooldown;

        SoundManager.instance.PlaySFX("Bullet");
    }

    void TwoBulletSpawn()
    {
        if (countdown1 > 0)
            return;

        SoundManager.instance.StopSFX("Bullet");

        GameObject bullet = GameObject.Instantiate(bulletPrefab, twobulletSpawn1.transform.position, twobulletSpawn1.transform.rotation);
        bullet.transform.Rotate(Vector3.left * -90f);
        bullet.GetComponent<Rigidbody>().velocity = bulletSpawn.transform.forward * bulletSpeed;

        GameObject bullet1 = GameObject.Instantiate(bulletPrefab, twobulletSpawn2.transform.position, twobulletSpawn2.transform.rotation);
        bullet1.transform.Rotate(Vector3.left * -90f);
        bullet1.GetComponent<Rigidbody>().velocity = bulletSpawn.transform.forward * bulletSpeed;
        countdown1 = cooldown1;

        SoundManager.instance.PlaySFX("Bullet");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            transform.position = spawnpoint.position;
            SoundManager.instance.PlaySFX("Crash");
        }
    }
}