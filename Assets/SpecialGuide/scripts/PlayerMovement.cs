using UnityEngine;
using System.Collections;
using InControl;

public class PlayerMovement : MonoBehaviour {

    private readonly object syncLock = new object();
    static int index = 0;
    
    InputDevice device;
    Rigidbody2D thisRigidbody;

    // Use this for initialization
    void Start () {
        lock (syncLock) {
       //    Debug.Log("index: " + index);
            device = InputManager.Devices[index++];
        }

        foreach(InputDevice dev in InputManager.Devices)
        {
            Debug.Log("dev: " + dev.ToString());
        }

     //   Debug.Log("device: " + device.ToString());
        thisRigidbody = this.transform.GetComponent<Rigidbody2D>();
   //    Debug.Log("rigidbody: " + thisRigidbody.ToString());

    }

    // Update is called once per frame
    void Update () {

        foreach (InputDevice dev in InputManager.Devices)
        {
            if(device.LeftStickX.State || device.LeftStickY.State)
            {
                float force = 900.0f;
                thisRigidbody.AddForce(new Vector2(force * Time.deltaTime * device.LeftStickX.Value, force * Time.deltaTime * device.LeftStickY.Value));
            }
        }
    }
}
