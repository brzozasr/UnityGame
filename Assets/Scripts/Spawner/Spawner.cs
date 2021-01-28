using System;
using System.Collections.Generic;
using System.Text;
using DefaultNamespace;
using DefaultNamespace.DAO;
using UnityEngine;
using Random = System.Random;

namespace Spawner
{
    public class Spawner : MonoBehaviour
    {
        public Canvas canvas;
        public GameObject player;
        public GameObject pointWidgetS;
        public GameObject pointWidgetM;
        public GameObject pointWidgetL;
        public GameObject pointWidgetXl;
        public GameObject chip;
        public GameObject playerBullet;
        public GameObject muzzleFlashContainer;
        public GameObject droneBullet;
        public GameObject explosionDrone;
        public GameObject firstAidKitBiohazard;
        public GameObject firstAidKitGreen;
        public GameObject firstAidKitRed;
        public GameObject firstAidKitWhite;
        public GameObject drone;
        public GameObject superDrone;
        public GameObject megaDrone;
        public CameraController cameraBoxScript;

        private List<Vector3> _drones;
        private List<Vector3> _superDrones;
        private List<Vector3> _megaDrones;

        private Random _random;
        private Dictionary<string, GameObject> GameObjects;

        public void Awake()
        {
            GameObjects = new Dictionary<string, GameObject>();
            GameObjects.Add("Drone", drone);
            GameObjects.Add("SuperDrone", superDrone);
            GameObjects.Add("MegaDrone", megaDrone);
            
            GameObjects.Add("FirstAidKitBiohazard", firstAidKitBiohazard);
            GameObjects.Add("FirstAidKitGreen", firstAidKitGreen);
            GameObjects.Add("FirstAidKitRed", firstAidKitRed);
            GameObjects.Add("FirstAidKitWhite", firstAidKitWhite);
            
            GameObjects.Add("PointWidgetS", pointWidgetS);
            GameObjects.Add("PointWidgetM", pointWidgetM);
            GameObjects.Add("PointWidgetL", pointWidgetL);
            GameObjects.Add("PointWidgetXL", pointWidgetXl);
            
            GameObjects.Add("Chip", chip);
            
            GameObjects.Add("Player", player);

            /*Vector3 position = new Vector3(2.27f, 0.8f, 0.4f);

            var newPlayer = Instantiate(player, position, Quaternion.identity);
            newPlayer.transform.Rotate(Vector3.up, 90.0f);
            
            cameraBoxScript.Player = newPlayer.transform;*/

            drone.GetComponent<DroneController>().canvas = canvas;
            superDrone.GetComponent<SuperDroneController>().canvas = canvas;
            megaDrone.GetComponent<MegaDroneController>().canvas = canvas;

            
            List<string> parents = new List<string>()
            {
                "Level",
            };
            
            Vector3 playerPosition = new Vector3(2.27f, 0.8f, 0.4f);
            PlayerData playerData = new PlayerData(player, playerPosition, parents, 50.0f, 5);
            
            playerData.UpdateGoData();
            var newPlayerObject = Instantiate(playerData.Go, playerData.Position, Quaternion.identity);
                
            GameObject parent = GetParent(playerData.Parents);
            newPlayerObject.transform.SetParent(parent.transform);
            newPlayerObject.transform.localPosition = playerData.Position;
            newPlayerObject.name = playerData.Go.name;
            newPlayerObject.transform.Rotate(Vector3.up, 90.0f);
            cameraBoxScript.Player = newPlayerObject.transform;
            
            SpawnDrones();
        }

        public void Start()
        {
            return;
            List<GameObjectData> positions = new List<GameObjectData>();
            List<string> parents = new List<string>()
            {
                "Level"
            };
            Transform level = GameObject.Find("Level").transform;
            GetPositions(ref positions, level, parents);

            SqlDroneDAO sqlDroneDao = new SqlDroneDAO();
            Debug.Log($"Count: {positions.Count.ToString()}");
            foreach (var god in positions)
            {
                switch (god)
                {
                    case DroneData droneData:
                        Debug.Log(droneData);
                        sqlDroneDao.Save(god);
                        break;
                    case SuperDroneData superDroneData:
                        Debug.Log(superDroneData);
                        sqlDroneDao.Save(god);
                        break;
                    case MegaDroneData megaDroneData:
                        Debug.Log(megaDroneData);
                        sqlDroneDao.Save(god);
                        break;
                    case FirstAidKitData firstAidKitData:
                        Debug.Log(firstAidKitData);
                        break;
                    case PointWidgetData pointWidgetData:
                        Debug.Log(pointWidgetData);
                        break;
                    case ChipData chipData:
                        Debug.Log(chipData);
                        break;
                    case PlayerData playerData:
                        Debug.Log(playerData);
                        break;
                }
            }
        }

        private void SpawnDrones()
        {
            /*List<string> parents = new List<string>()
            {
                "Level",
                "BridgeS01"
            };
            List<string> parents2 = new List<string>()
            {
                "Level",
            };

            List<GameObjectData> positions3 = new List<GameObjectData>();
            Vector3 dronePosition2 = new Vector3(0.29f, 1.75f, 0.0f);
            DroneData drone2 = new DroneData(drone, dronePosition2, parents, 1.5f, 2.0f, 2);
            positions3.Add(drone2);
            
            Vector3 dronePosition3 = new Vector3(13.33f, 12.46f, 0.0f);
            SuperDroneData drone3 = new SuperDroneData(superDrone, dronePosition3, parents2, 3.0f, 4.0f, 4, 1.0f, 2.0f);
            positions3.Add(drone3);
            
            Vector3 dronePosition4 = new Vector3(19.3f, 1.49f, 0.0f);
            MegaDroneData drone4 = new MegaDroneData(megaDrone, dronePosition4, parents2, 3.0f, 4.0f, 4, 1.0f, 2.0f, 1.5f);
            positions3.Add(drone4);*/
            
            
            SqlDroneDAO sqlDroneDao = new SqlDroneDAO();
            List<GameObjectData> positions3 = sqlDroneDao.Load(0);
            
            foreach (var gObject in positions3)
            {
                // Update gameObject prefab's properties
                gObject.UpdateGoData();
                
                var newObject = Instantiate(gObject.Go, gObject.Position, Quaternion.identity);
                
                GameObject parent = GetParent(gObject.Parents);
                newObject.transform.SetParent(parent.transform);
                newObject.transform.localPosition = gObject.Position;
                newObject.name = gObject.Go.name;
            }
        }

        private GameObject GetParent(List<string> parents)
        {
            return GameObject.Find($"/{String.Join("/", parents)}");
        }

        private void GetPositions(ref List<GameObjectData> positions, Transform parent, List<string> parents)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                var child = parent.GetChild(i);
                string gameObjectName = child.gameObject.name;

                if (child.childCount > 0 && !GameObjects.ContainsKey(gameObjectName))
                {
                    parents.Add(child.name);
                    GetPositions(ref positions, child, parents);
                }
                else
                {
                    if (GameObjects.ContainsKey(gameObjectName))
                    {
                        switch (gameObjectName)
                        {
                            case ("Drone"): // ToDo Implement a function to retrieve GO's specific parameters
                                (Vector3 position, float shotTimeRangeFrom, float shotTimeRangeTo, int hitPoints) =
                                    DroneData.GetDroneParameters(child);
                                
                                DroneData droneData = new DroneData(drone, position, parents, shotTimeRangeFrom,
                                    shotTimeRangeTo, hitPoints);
                                
                                positions.Add(droneData);
                                
                                break;
                            case ("SuperDrone"):
                                (Vector3 superDronePosition, float superDroneShotTimeRangeFrom, float superDroneShotTimeRangeTo, int superDroneHitPoints, float maxMoveY, float moveSpeed) =
                                    SuperDroneData.GetSuperDroneParameters(child);
                                
                                SuperDroneData superDroneData = new SuperDroneData(superDrone, superDronePosition, parents, superDroneShotTimeRangeFrom,
                                    superDroneShotTimeRangeTo, superDroneHitPoints, maxMoveY, moveSpeed);
                                
                                positions.Add(superDroneData);
                                
                                break;
                            case ("MegaDrone"):
                                (Vector3 megaDronePosition, float megaDroneShotTimeRangeFrom,
                                        float megaDroneShotTimeRangeTo, int megaDroneHitPoints, float megaMaxMoveY,
                                        float megaMoveSpeed, float megaMaxMoveX) =
                                    MegaDroneData.GetMegaDroneParameters(child);
                                
                                MegaDroneData megaDroneData = new MegaDroneData(megaDrone, megaDronePosition, parents,
                                    megaDroneShotTimeRangeFrom,
                                    megaDroneShotTimeRangeTo, megaDroneHitPoints, megaMaxMoveY, megaMoveSpeed, megaMaxMoveX);
                                
                                positions.Add(megaDroneData);
                                
                                break;
                            case ("FirstAidKitBiohazard"):
                                (Vector3 firstAidKitBiohazardPosition, int hitPointRecovery) =
                                    FirstAidKitData.GetFirstAidKitParameters(child);
                                
                                FirstAidKitData firstAidKitBiohazardData = new FirstAidKitData(firstAidKitBiohazard,
                                    firstAidKitBiohazardPosition, parents, hitPointRecovery);
                                
                                positions.Add(firstAidKitBiohazardData);
                                
                                break;
                            case ("FirstAidKitGreen"):
                                (Vector3 firstAidKitGreenPosition, int hitPointRecoveryGreen) =
                                    FirstAidKitData.GetFirstAidKitParameters(child);
                                
                                FirstAidKitData firstAidKitGreenData = new FirstAidKitData(firstAidKitGreen,
                                    firstAidKitGreenPosition, parents, hitPointRecoveryGreen);
                                
                                positions.Add(firstAidKitGreenData);
                                
                                break;
                            case ("FirstAidKitRed"):
                                (Vector3 firstAidKitRedPosition, int hitPointRecoveryRed) =
                                    FirstAidKitData.GetFirstAidKitParameters(child);
                                
                                FirstAidKitData firstAidKitRedData = new FirstAidKitData(firstAidKitGreen,
                                    firstAidKitRedPosition, parents, hitPointRecoveryRed);
                                
                                positions.Add(firstAidKitRedData);
                                
                                break;
                            case ("FirstAidKitWhite"):
                                (Vector3 firstAidKitWhitePosition, int hitPointRecoveryWhite) =
                                    FirstAidKitData.GetFirstAidKitParameters(child);
                                
                                FirstAidKitData firstAidKitWhiteData = new FirstAidKitData(firstAidKitGreen,
                                    firstAidKitWhitePosition, parents, hitPointRecoveryWhite);
                                
                                positions.Add(firstAidKitWhiteData);
                                
                                break;
                            case ("PointWidgetS"):
                                (Vector3 pointWidgetSPosition, int widgetPointsS) =
                                    PointWidgetData.GetPointWidgetParameters(child);
                                
                                PointWidgetData pointWidgetSData = new PointWidgetData(pointWidgetS,
                                    pointWidgetSPosition, parents, widgetPointsS);
                                
                                positions.Add(pointWidgetSData);
                                
                                break;
                            case ("PointWidgetM"):
                                (Vector3 pointWidgetMPosition, int widgetPointsM) =
                                    PointWidgetData.GetPointWidgetParameters(child);
                                
                                PointWidgetData pointWidgetMData = new PointWidgetData(pointWidgetM,
                                    pointWidgetMPosition, parents, widgetPointsM);
                                
                                positions.Add(pointWidgetMData);
                                
                                break;
                            case ("PointWidgetL"):
                                (Vector3 pointWidgetLPosition, int widgetPointsL) =
                                    PointWidgetData.GetPointWidgetParameters(child);
                                
                                PointWidgetData pointWidgetLData = new PointWidgetData(pointWidgetL,
                                    pointWidgetLPosition, parents, widgetPointsL);
                                
                                positions.Add(pointWidgetLData);
                                
                                break;
                            case ("PointWidgetXL"):
                                (Vector3 pointWidgetXlPosition, int widgetPointsXl) =
                                    PointWidgetData.GetPointWidgetParameters(child);
                                
                                PointWidgetData pointWidgetXlData = new PointWidgetData(pointWidgetXl,
                                    pointWidgetXlPosition, parents, widgetPointsXl);
                                
                                positions.Add(pointWidgetXlData);
                                
                                break;
                            case ("Chip"):
                                (Vector3 chipPosition, string itemName, int itemQuantity) =
                                    ChipData.GetChipParameters(child);
                                
                                ChipData chipData = new ChipData(chip,
                                    chipPosition, parents, itemName, itemQuantity);
                                
                                positions.Add(chipData);
                                
                                break;
                            case ("Player"):
                                (Vector3 playerPosition, float livePoints, int liveNumber) =
                                    PlayerData.GetPlayerParameters(child);
                                
                                PlayerData playerData = new PlayerData(player,
                                    playerPosition, parents, livePoints, liveNumber);
                                
                                positions.Add(playerData);

                                break;
                        }
                    }
                }
            }
            
            if (parents.Count > 0)
                parents.RemoveAt(parents.Count - 1);
        }
    }

    struct God
    {
        public GameObject gameObject;
        public GameObjectData gameObjectData;

        public God(GameObject gameObject, GameObjectData gameObjectData)
        {
            this.gameObject = gameObject;
            this.gameObjectData = gameObjectData;
        }
    }
}