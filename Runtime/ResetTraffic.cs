using UnityEngine;
using UnityEngine.UI;
using _3Dimensions.TrafficSystem;

namespace _3Dimensions.TrafficSystemFishNet
{
    public class ResetTraffic : MonoBehaviour
    {
        [SerializeField] private Button button;

        private void OnEnable()
        {
            button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            if (!TrafficSystemServer.Instance) return;
            TrafficSystemServer.Instance.ResetTraffic();
        }
    }
}
