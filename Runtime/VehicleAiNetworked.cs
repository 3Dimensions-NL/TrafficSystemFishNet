using _3Dimensions.TrafficSystem.Runtime;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
namespace _3Dimensions.TrafficSystemFishNet.Runtime
{
    public class VehicleAiNetworked : NetworkBehaviour
    {
        [SerializeField] private VehicleAi vehicleAi;
        [SerializeField] private TrafficRoute route;
        
        private readonly SyncVar<bool> _obstacleDetected = new SyncVar<bool>();

        private void Awake()
        {
            if (!vehicleAi) vehicleAi = GetComponent<VehicleAi>();
            if (!route) route = GetComponent<TrafficRoute>();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            vehicleAi.simulate = IsServerStarted;
            route.enabled = IsServerStarted;
            
            _obstacleDetected.OnChange += ObstacleDetectedOnChange;
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            vehicleAi.simulate = IsServerStarted;
            route.enabled = IsServerStarted;
            
            _obstacleDetected.OnChange -= ObstacleDetectedOnChange;
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            TimeManager.OnTick += TimeManagerOnOnTick;
        }

        public override void OnStopServer()
        {
            base.OnStopServer();
            TimeManager.OnTick -= TimeManagerOnOnTick;
        }

        private void TimeManagerOnOnTick()
        {
            if (vehicleAi.hasObstacleInFront != _obstacleDetected.Value)
            {
                _obstacleDetected.Value = vehicleAi.hasObstacleInFront;
            }
        }

        private void ObstacleDetectedOnChange(bool prev, bool next, bool aServer)
        {
            if (aServer) return;
            vehicleAi.hasObstacleInFront = next;
        }
    }
}
