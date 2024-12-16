using _3Dimensions.TrafficSystem.Runtime;
using FishNet;
using FishNet.Object;
using UnityEngine;
namespace _3Dimensions.TrafficSystemFishNet.Runtime
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
            vehicleAi.enabled = IsServerStarted;
            route.enabled = IsServerStarted;
        }
    }
}
