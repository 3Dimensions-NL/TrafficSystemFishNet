using System;
using _3Dimensions.TrafficSystem.Runtime;
using FishNet.Object;
using UnityEngine;
using UnityEngine.Events;
namespace _3Dimensions.TrafficSystemFishNet.Runtime
{
    public class TrafficSpawnerServer : NetworkBehaviour
    {
        [SerializeField] private TrafficSpawner trafficSpawner;

        public UnityEvent onNetworkSpawn;
        public bool logDebugs;

        private void Awake()
        {
            trafficSpawner.canSpawn = false;
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            trafficSpawner.canSpawn = true;
            trafficSpawner.OnInstantiateGameObject += OnInstantiateGameObject;
        }

        public override void OnStopServer()
        {
            base.OnStopServer();
            trafficSpawner.canSpawn = false;
            trafficSpawner.OnInstantiateGameObject -= OnInstantiateGameObject;
        }
        
        private void OnInstantiateGameObject(GameObject spawnedGameObject)
        {
            if (logDebugs) Debug.Log("Spawning " + spawnedGameObject + " on network.");
            Spawn(spawnedGameObject);
            onNetworkSpawn?.Invoke();
        }
    }
}
