using FishNet.Object;
using UnityEngine;

namespace _3Dimensions.TrafficSystem
{
    public class TrafficSystemServer : NetworkBehaviour
    {
        public static TrafficSystemServer Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType<TrafficSystemServer>();
                }

                return _instance;
            }
        }
        private static TrafficSystemServer _instance;
        
        
        [SerializeField] private TrafficSystem trafficSystem;
        public override void OnStartClient()
        {
            base.OnStartClient();
            trafficSystem.gameObject.SetActive(IsServer);
        }

        [ServerRpc(RequireOwnership = false)]
        public void ResetTraffic()
        {
            if (trafficSystem)
            {
                trafficSystem.ResetTraffic();
            }
        }
    }
}
