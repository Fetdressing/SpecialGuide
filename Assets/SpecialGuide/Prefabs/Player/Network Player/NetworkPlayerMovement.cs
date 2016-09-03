using UnityEngine;
using System.Collections;
using InControl;
using UnityEngine.Networking;

public class NetworkPlayerMovement : NetworkBehaviour {

	void Start () {
	
	}
	
	void Update () {
        if (!isLocalPlayer)
        {
            return;
        }

        InputDevice device = InputManager.ActiveDevice;
        transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(device.LeftStickX * 900.0f * Time.deltaTime, 0.0f));
	}

    public override void OnStartLocalPlayer()
    {
        GetComponent<Renderer>().material.color = Color.blue;
    }

}
