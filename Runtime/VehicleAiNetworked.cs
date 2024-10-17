using FishNet;
using FishNet.Object;
using UnityEngine;

namespace _3Dimensions.TrafficSystem
{
    public class VehicleAiNetworked : NetworkBehaviour
    {
        [SerializeField] private VehicleAi vehicleAi;
        [SerializeField] private TrafficRoute route;

        private void Awake()
        {
            if (!vehicleAi) vehicleAi = GetComponent<VehicleAi>();
            if (!route) route = GetComponent<TrafficRoute>();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            vehicleAi.enabled = IsServer;
            route.enabled = IsServer;
            
            if (IsServer) TimeManager.OnTick += OnTick;
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            if (IsServer) TimeManager.OnTick -= OnTick;
        }

        private void OnDestroy()
        {
            if (InstanceFinder.NetworkManager)
            {
                if (InstanceFinder.NetworkManager.IsServer)
                {
                    InstanceFinder.ServerManager.Despawn(gameObject);
                }
            }
        }
        
        private void OnTick()
        {
            // if (IsServer) vehicleAi.ApplyUpdate((float)TimeManager.TickDelta);
        }
    }
}
