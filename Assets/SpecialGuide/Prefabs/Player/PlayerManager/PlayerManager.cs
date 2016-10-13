using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{

    public class GameObjectWrapper 
    {
        public GameObjectWrapper(GameObject gameObject)
        {
            this.gameObject = gameObject;
            transform = this.gameObject.transform;
        }

        public GameObject gameObject { get; private set; }
        public Transform transform { get; private set; }
    }

    public GameObject playerOnePrefab;
    public GameObject playerTwoPrefab;
    public GameObject guidePrefab;

    private GameObjectWrapper playerOne;
    private GameObjectWrapper playerTwo;
    private GameObjectWrapper guide;

    private GameObject[] playerControllers = new GameObject[2];

    public SpawnPoint[] spawnPoints; //dessa skall ligga i den ordningen som spawnesen kommer, dvs sista spawnet ska ligga sist i listan

	void Start () {
        InstansiatePrefabs();
        spawnPoints[0].isReached = true;
        SpawnPlayers();
    }
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.G)) Respawn();
    }

    private void InstansiatePrefabs()
    {
        Debug.Log("A");
        playerOne = new GameObjectWrapper(GameObject.Instantiate(playerOnePrefab));
        playerOne.gameObject.tag = "Player1";
        playerTwo = new GameObjectWrapper(GameObject.Instantiate(playerTwoPrefab));
        playerTwo.gameObject.tag = "Player2";
        guide = new GameObjectWrapper(GameObject.Instantiate(guidePrefab));
        guide.gameObject.tag = "Guide";
    }

    public void Respawn()
    {
        //eventuellt att den tar bort liv eller liknande?
        SpawnPlayers();
    }

    private void SpawnPlayers()
    {
        if (spawnPoints[0] == null) return;
        spawnPoints[0].isReached = true;
        SpawnPoint bestSpawn = spawnPoints[0];
        for(int i = 0; i < spawnPoints.Length; i++)
        {
            if(spawnPoints[i].isReached)
            {
                bestSpawn = spawnPoints[i];
            }
            else
            {
                break;
            }
        }

        PositionPlayersAtSpawn(bestSpawn);

        try {
            playerOne.transform.GetComponent<UnitBase>().Reset();
            playerTwo.transform.GetComponent<UnitBase>().Reset();
            guide.transform.GetComponent<UnitBase>().Reset();
        }
        catch
        {
            Debug.Log("Alla spelare/guide gick ej att resetta");
        }
    }

    private void PositionPlayersAtSpawn(SpawnPoint sp)
    {
        playerOne.transform.position = sp.spawnPosPlayer1;
        playerTwo.transform.position = sp.spawnPosPlayer2;
        guide.transform.position = sp.spawnPosGuide;
    }


}
