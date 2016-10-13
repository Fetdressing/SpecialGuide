using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour {
    [HideInInspector]
    public bool isReached = false;

    [HideInInspector]
    public Vector3 spawnPosPlayer1;
    [HideInInspector]
    public Vector3 spawnPosPlayer2;
    [HideInInspector]
    public Vector3 spawnPosGuide;
    private Vector3 offsetSpawnPos = new Vector3(3, 0, 0);
    // Use this for initialization
    void Start () {
        isReached = false;

        spawnPosPlayer1 = transform.position + offsetSpawnPos;
        spawnPosPlayer2 = transform.position - offsetSpawnPos;
        spawnPosGuide = transform.position;
    }
	

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Guide") //specialguide måste använda rätt tag
        {
            isReached = true; //spawnpointen är nådd
        }
    }
}
