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

	void Start () {
        InstansiatePrefabs();
    }
	
	void Update () {

    }

    private void InstansiatePrefabs()
    {
        playerOne = new GameObjectWrapper(GameObject.Instantiate(playerOnePrefab));
        playerOne.gameObject.tag = "Player1";
        playerTwo = new GameObjectWrapper(GameObject.Instantiate(playerTwoPrefab));
        playerTwo.gameObject.tag = "Player2";
        guide = new GameObjectWrapper(GameObject.Instantiate(guidePrefab));
        guide.gameObject.tag = "Guide";
    }

    private void spawnPlayers()
    {

    }
}
