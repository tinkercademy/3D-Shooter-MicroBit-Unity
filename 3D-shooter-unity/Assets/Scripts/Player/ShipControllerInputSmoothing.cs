using UnityEngine;

public class ShipControllerInputSmoothing : MonoBehaviour
{
    [Header("Input Smoothing")]
    //Steering
    public float steeringSmoothing;
    private Vector3 rawInputSteering;
    private Vector3 smoothInputSteering;

    //Thrust
    public float thrustSmoothing;
    private float rawInputThrust;
    private float smoothInputThrust;

    [SerializeField] private ShipControllerVersion2 ship = null;

    public SampleMessageListener msgListener;

    public float resetXFromControllerPositive;
    public float resetYFromControllerPositive;
    public float resetXFromControllerNegative;
    public float resetYFromControllerNegative;

    //data handling from the microbit
    public void OnSteering()
    {
        Vector2 rawInput = new Vector2();

        if (msgListener.posX < -1)
            rawInput.x = -1f;
        else if (msgListener.posX > 1)
            rawInput.x = 1f;

        if (msgListener.posY < -1)
            rawInput.y = -1f;
        else if (msgListener.posY > 1)
            rawInput.y = 1f;

        rawInputSteering = new Vector3(rawInput.y, 0, -rawInput.x);

    }

    public void OnThrust()
    {
        float temp = ship.thrustInput;

        if (msgListener.item == 1)
            temp += Time.deltaTime*10;
        else
            temp = 0f;

        temp = Mathf.Clamp(temp, 0, 1); // prevent to from exceeding 1 or going below 0

        rawInputThrust = temp;
    }

    void Update()
    {
        InputSmoothing();
        SetInputData();
    }

    void InputSmoothing()
    {
        OnSteering();
        OnThrust();

        //Steering
        smoothInputSteering = Vector3.Lerp(smoothInputSteering, rawInputSteering, Time.deltaTime * steeringSmoothing);

        //Thrust
        smoothInputThrust = Mathf.Lerp(smoothInputThrust, rawInputThrust, Time.deltaTime * thrustSmoothing);
    }

    void SetInputData()
    {
        ship.UpdateInputData(smoothInputSteering, smoothInputThrust);
    }
}
