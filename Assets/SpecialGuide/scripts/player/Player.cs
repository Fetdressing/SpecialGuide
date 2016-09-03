using System;
using UnityEngine;
using InControl;

public class Player : MonoBehaviour
{
	public PlayerActions Actions { get; set; }
	//Renderer cachedRenderer;

    void OnDisable()
	{
		if (Actions != null)
		{
			Actions.Destroy();
		}
	}

	void Start()
	{
        DontDestroyOnLoad(transform.gameObject);
        //cachedRenderer = GetComponent<Renderer>();
    }

    void Update()
	{
		if (Actions == null)
		{
            // If no controller exists for this cube, just make it translucent.
            //cachedRenderer.material.color = new Color( 1.0f, 1.0f, 1.0f, 0.2f );
        }
        else
		{
            // Set object material color.
            //cachedRenderer.material.color = GetColorFromInput();
        }
    }


	Color GetColorFromInput()
	{
		if (Actions.Green)
		{
			return Color.green;
		}

		if (Actions.Red)
		{
			return Color.red;
		}

		if (Actions.Blue)
		{
			return Color.blue;
		}

		if (Actions.Yellow)
		{
			return Color.yellow;
		}

		return Color.white;
	}
}

