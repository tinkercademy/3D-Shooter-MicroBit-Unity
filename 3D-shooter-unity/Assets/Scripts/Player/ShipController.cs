using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public float forwardSpeed = 25f;
    public float strafeSpeed = 7.5f;
    public float hoverSpeed = 5f;

    private float activeforwardSpeed;
    private float activestrafeSpeed;
    private float activehoverSpeed;

    private float forwardAcceleration = 2.5f;
    private float strafeAcceleration = 2f;
    private float hoverAcceleration = 2f;

    public float lookRateSpeed = 90f;
    private Vector2 lookInput;
    private Vector2 screenCenter;
    private Vector2 mouseDistance;

    private float rollInput;
    public float rollSpeed = 90f;
    public float rollAcceleration = 3.5f;

    public Rigidbody rb;
    public GameObject bulletSpawn;
    public GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        screenCenter.x = Screen.width * 0.5f;
        screenCenter.y = Screen.height * 0.5f;

        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        lookInput.x = Input.mousePosition.x;
        lookInput.y = Input.mousePosition.y;

        mouseDistance.x = (lookInput.x - screenCenter.x) / screenCenter.y;
        mouseDistance.y = (lookInput.y - screenCenter.y) / screenCenter.y;

        mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1f);

        rollInput = Mathf.Lerp(rollInput, Input.GetAxisRaw("Roll"), rollAcceleration * Time.deltaTime);

        transform.Rotate(-mouseDistance.y * lookRateSpeed * Time.deltaTime, mouseDistance.x * lookRateSpeed * Time.deltaTime, rollInput * rollSpeed * Time.deltaTime, Space.Self);

        activeforwardSpeed = Mathf.Lerp(activeforwardSpeed, Input.GetAxis("Vertical") * forwardSpeed, forwardAcceleration * Time.deltaTime);
        activestrafeSpeed = Mathf.Lerp(activestrafeSpeed, Input.GetAxis("Horizontal") * strafeSpeed, strafeAcceleration * Time.deltaTime);
        activehoverSpeed = Mathf.Lerp(activehoverSpeed, Input.GetAxis("Hover") * hoverSpeed, hoverAcceleration * Time.deltaTime);

        transform.position += transform.forward * activeforwardSpeed * Time.deltaTime;
        transform.position += (transform.right * activestrafeSpeed * Time.deltaTime) + (transform.up * activehoverSpeed * Time.deltaTime);
    }

    private void OnMouseDown()
    {
        GameObject bullet = GameObject.Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        bullet.transform.Rotate(Vector3.left * -90f);
        bullet.GetComponent<Rigidbody>().velocity = bulletSpawn.transform.forward * 20f;
    }

    private void JetBoost()
    {
        GameObject bullet = GameObject.Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        bullet.transform.Rotate(Vector3.left * -90f);
        bullet.GetComponent<Rigidbody>().velocity = bulletSpawn.transform.forward * 20f;
    }
}
