using _3Dimensions.TrafficSystem.Runtime;
using FishNet.Object;
using UnityEngine;
namespace _3Dimensions.TrafficSystemFishNet.Runtime
{
    public class TrafficManagerServer : NetworkBehaviour
    {
        public static TrafficManagerServer Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType<TrafficManagerServer>();
                }

                return _instance;
            }
        }
        private static TrafficManagerServer _instance;
        
        
        [SerializeField] private TrafficManager trafficManager;
        public override void OnStartClient()
        {
            base.OnStartClient();
            trafficManager.gameObject.SetActive(IsServer);
        }

        [ServerRpc(RequireOwnership = false)]
        public void ResetTraffic()
        {
            if (trafficManager)
            {
                trafficManager.ResetTraffic();
            }
        }
    }
}
