using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollutantManager : MonoBehaviour
{
    //used for creating and storing pollutants
    //when creating it randoms a pollutant type and gives it a reward for recycling based on the type
    [SerializeField] Pollutant[] PollutantOptions = new Pollutant[7];
    //0 = glass
    //1 - 3 = GW
    //4 onwards = Plastic
    [SerializeField] Transform[] polltantSpawnPoints;       //a centre point (the pollutants spawn in a radius around)

    //the radius of the trash spawn in an area
    private int[] spawnRadiusArray = new int[] { 10, 20, 40, 25 };

    const float WATERHEIGHT = 0.7f;  //height of water so pollutants look like theyre floating

    private int currentLevelNum = 0;

    System.Random rand = new System.Random();

    public List<Pollutant> activePollutants = new List<Pollutant>();

    private Dictionary<int, int[]> spawnRatios = new Dictionary<int, int[]>
    {
//      lvl             G   gw  P
        {0 , new int[] { 50, 100, 101} },
        {1 , new int[] { 50, 100, 101} },
        {2 , new int[] { 45, 90, 100} },
        {3 , new int[] { 15, 30, 100} },
        {4 , new int[] {33, 66, 100} },

    };

    private float timer;
    private float duration = 5;

    void Start()
    {
        currentLevelNum = 0;

        //EventManager.OnDelegateEvent SpawnPollutantDelegate = SpawnPollutant;
        //EventManager.Instance.AddListener(EventManager.EVENT_TYPE.LEVEL_UP, SpawnPollutantDelegate);
        //EventManager.Instance.AddListener(EventManager.EVENT_TYPE.GAME_START, SpawnPollutantDelegate);
        EventManager.OnDelegateEvent NextLevelDelegate = NextLevel;
        EventManager.Instance.AddListener(EventManager.EVENT_TYPE.COMPLETE_QUEST, NextLevelDelegate);


        //spawns them at the start
        BatchSpawnPollutant();
        timer = duration;
    }

    private void Update()
    {
        //timer decrease
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            //spawn a new pollutant every time the timer hits 0
            SpawnIndividualPollutant();
            //resets timer
            timer = duration;
        }
    }

    public void SpawnIndividualPollutant()
    {
        //spawns a pollutant in the current levels
        for (int i = 0; i < currentLevelNum + 1; i++)
        {
            //creates a pollutant
            Pollutant spawnedObj = new Pollutant();
            //a new position is created within the circle of the spawn point
            Vector2 newPosition = (Random.insideUnitCircle * spawnRadiusArray[i]) + new Vector2(polltantSpawnPoints[i].position.x, polltantSpawnPoints[i].position.z);
            //randoms a pollutant and spawns it at the new position

            //INCLUDE SPAWN RATE
            spawnedObj = Instantiate(PollutantOptions[SpawnRateCalculator(currentLevelNum)], new Vector3(newPosition.x, WATERHEIGHT, newPosition.y), Quaternion.identity);
            activePollutants.Add(spawnedObj);
        }
    }

    public void BatchSpawnPollutant()
    {
        //spawn point changes depending on level
        //spawn radius changes depending on level
        print(currentLevelNum);
        for (int i = 0; i < 10; i++)
        {
            //creates a pollutant
            Pollutant spawnedObj = new Pollutant();
            //a transform has a radius of 10 around it
            //a new position is created within that circle
            if (currentLevelNum > 4)
            {
                //stop printing after all areas
            }
            else
            {
                Vector2 newPosition = (Random.insideUnitCircle * spawnRadiusArray[currentLevelNum]) + new Vector2(polltantSpawnPoints[currentLevelNum].position.x, polltantSpawnPoints[currentLevelNum].position.z);
                //randoms a pollutant and spawns it at the new position

                //INCLUDE SPAWN RATE
                spawnedObj = Instantiate(PollutantOptions[SpawnRateCalculator(currentLevelNum)], new Vector3(newPosition.x, WATERHEIGHT, newPosition.y), Quaternion.identity);
                activePollutants.Add(spawnedObj);
            }
        }

    }

    public void NextLevel(EventManager.EVENT_TYPE eventType, Component sender, object Params = null)
    {
        //gets the levelnum from event
        currentLevelNum = (int)Params;

        ////Destroy the active pollutants before more spawn
        //for (int i = 0; i < activePollutants.Count; i++)
        //{
        //    Destroy(activePollutants[i]);
        //    activePollutants.RemoveAt(i);
        //}

        //spawns the pollutants for the next level
        BatchSpawnPollutant();

    }

    private int SpawnRateCalculator(int levelNum)
    {
        //gets a pollutant type based on the ratio at which they spawn in the each level
        int rndm = rand.Next(1, 100);

        //checks glass spawn probability for that level
        if (1 <= rndm && rndm <= spawnRatios[levelNum][0])
        {
            //return glass
            return 0;
        }
        //checks gw probability
        else if (spawnRatios[levelNum][0] <= rndm && rndm <= spawnRatios[levelNum][2])
        {
            int rndmGW = rand.Next(1, 4);
            return rndmGW;
        }
        //otherwise spawns plastic
        else
        {
            int rndmPlastic = rand.Next(4, 8);
            return rndmPlastic;
        }
    }
}
