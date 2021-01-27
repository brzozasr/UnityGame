using System;
using System.Collections.Generic;
using System.Text;
using DefaultNamespace;
using UnityEngine;
using Random = System.Random;

namespace Spawner
{
    public class Spawner : MonoBehaviour
    {
        public GameObject canvas;
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
            GameObjects.Add("SuperDrone", drone);

            Vector3 position = new Vector3(2.27f, 0.8f, 0.4f);

            var newPlayer = Instantiate(player, position, Quaternion.identity);
            newPlayer.transform.Rotate(Vector3.up, 90.0f);
            
            // cameraBoxScript.Player = newPlayer.transform;
            
            drone.GetComponent<DroneController>().canvas = canvas.GetComponent<Canvas>();
            superDrone.GetComponent<SuperDroneController>().canvas = canvas.GetComponent<Canvas>();

            SpawnDrones();
        }

        public void Start()
        {
            List<GameObjectData> positions = new List<GameObjectData>();
            List<string> parents = new List<string>()
            {
                "Level"
            };
            Transform level = GameObject.Find("Level").transform;
            GetPositions(ref positions, level, parents);
            
            Debug.Log($"Count: {positions.Count.ToString()}");
            foreach (var god in positions)
            {
                switch (god)
                {
                    case DroneData droneData:
                        Debug.Log(droneData);
                        break;
                    case SuperDroneData droneData:
                        Debug.Log(droneData);
                        break;
                }
            }
        }

        private void SpawnDrones()
        {
            List<string> parents = new List<string>()
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
                    GetPositions(ref positions, child, parents);
                    parents.Add(parent.name);
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