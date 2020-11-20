using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
using UnityEngine.UI;

[System.Serializable] public class CameraEvent : UnityEvent<bool> { }
[System.Serializable] public class EffectEvent : UnityEvent<bool, bool> { }
[System.Serializable] public class ParticleEvent : UnityEvent<bool, int> { }

public class DragonAi : MonoBehaviour
{

    [HideInInspector] public CameraEvent OnBossReveal;
    [HideInInspector] public EffectEvent GroundContact;
    [HideInInspector] public ParticleEvent GroundDetection;

    [Header("Pathing")]
    [SerializeField] CinemachineSmoothPath path = default;
    [SerializeField] CinemachineDollyCart cart = default;
    [SerializeField] LayerMask terrainLayer = default;
    ShipControllerVersion2 playerShip;

    [HideInInspector] public Vector3 startPosition, endPosition;

    RaycastHit hitInfo;
    public int totalHealth = 100;
    int currentHealth;
    public HealthBar healthUI;

    [SerializeField] private Text textTime = null;
    [SerializeField] private Text realtimetakenUI = null;
    private float timetaken = 0;
    [SerializeField] private GameObject endText = null;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = totalHealth;

        playerShip = Object.FindObjectOfType<ShipControllerVersion2>();

        SoundManager.instance.PlaySFX("Dragon Roar");

        healthUI.SetMaxHealth(totalHealth);

        AI();
    }

    private void Update()
    {
        timetaken += Time.deltaTime;
        realtimetakenUI.text = "Time:" + timetaken.ToString("0.0"); //force the display time to nearest 1 decimal place
    }

    void AI()
    {
        UpdatePath();
        StartCoroutine(FollowPath()); //start the coroutine (allows the process to wait until certain condition is met through using yield return)
    }

    IEnumerator FollowPath()
    {
        while (true)
        {
            //play leaving ground effect

            yield return new WaitUntil(() => cart.m_Position >= 0.06f);
            GroundContact.Invoke(true, true); //trigger the event
            yield return new WaitUntil(() => cart.m_Position >= 0.23f);
            GroundContact.Invoke(false, true); //trigger the event

            // wait to reenter ground

            yield return new WaitUntil(() => cart.m_Position >= 0.60f);
            GroundContact.Invoke(true, false); //trigger the event
            yield return new WaitUntil(() => cart.m_Position >= 0.90f);
            GroundContact.Invoke(false, false); //trigger the event
            OnBossReveal.Invoke(false); //trigger the event

            // wait a beat to come out of ground again
            yield return new WaitUntil(() => cart.m_Position >= 0.99f);
            yield return new WaitForSeconds(Random.Range(1, 2));

            //reset path
            UpdatePath();
            yield return new WaitUntil(() => cart.m_Position <= 0.05f);
        }
    }

    void UpdatePath()
    {
        //predict player position based on the moving velocity direction 
        Vector3 playerPosition = playerShip.transform.position + (playerShip.rb.velocity * 8);
        playerPosition.y = Mathf.Max(10, playerPosition.y);
        Vector3 randomRange = Random.insideUnitSphere * 100; //get a random point in the sphere
        randomRange.y = 0;
        startPosition = playerPosition + randomRange;
        endPosition = playerPosition - randomRange;

        //get the hit point on the terrain to mark as the start and end position
        if (Physics.Raycast(startPosition, Vector3.down, out hitInfo, 1000, terrainLayer.value))
        {
            startPosition = hitInfo.point;
        }

        if (Physics.Raycast(endPosition, Vector3.down, out hitInfo, 1000, terrainLayer.value))
        {
            endPosition = hitInfo.point;
            GroundDetection.Invoke(false, hitInfo.transform.CompareTag("Terrain") ? 0 : 1);
        }

        //set the way point in the cinemachine smooth path
        path.m_Waypoints[0].position = startPosition + (Vector3.down * 15);
        path.m_Waypoints[1].position = playerPosition + (Vector3.up * 10);
        path.m_Waypoints[2].position = endPosition + (Vector3.down * 45);

        path.InvalidateDistanceCache();
        cart.m_Position = 0;

        //speed
        cart.m_Speed = cart.m_Path.PathLength / 1500;

        OnBossReveal.Invoke(true);

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            Destroy(other.gameObject);
            currentHealth -= 10;
            healthUI.SetHealth(currentHealth);

            //end game condition
            if (currentHealth <= 0)
            {
                Destroy(gameObject.transform.root.gameObject);
                endText.SetActive(true);
                Time.timeScale = 0;
                int mins = (int)timetaken / 60;
                int sec = (int)timetaken % 60;

                if (mins == 0)
                    textTime.text = sec.ToString() + "s";
                else if (sec == 0)
                    textTime.text = mins.ToString() + "mins";
                else
                    textTime.text = mins.ToString() + "mins" + sec.ToString() + "s";
            }
        }
        else if (other.gameObject.tag == "Terrain")
        {
            SoundManager.instance.PlaySFX("Earth Impact");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(startPosition, 1);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(endPosition, 1);
    }
}