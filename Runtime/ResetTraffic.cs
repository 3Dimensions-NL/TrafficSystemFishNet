using UnityEngine;
using UnityEngine.UI;
namespace _3Dimensions.TrafficSystemFishNet.Runtime
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
            if (!TrafficManagerServer.Instance) return;
            TrafficManagerServer.Instance.ResetTraffic();
        }
    }
}
