using _3Dimensions.TrafficSystem.Runtime;
using FishNet.Object;
using FishNet.Object.Synchronizing;

namespace _3Dimensions.TrafficSystemFishNet.Runtime
{
    public class TrafficLightsControllerNetworked : NetworkBehaviour
    {
        public TrafficLightsController trafficLightsController;
        private readonly SyncVar<int> _sectionGreen = new SyncVar<int>();

        public override void OnStartClient()
        {
            base.OnStartClient();
            trafficLightsController.Simulate = IsServerStarted;
            trafficLightsController.currentSectionIndex = _sectionGreen.Value;
            
            _sectionGreen.OnChange += SectionGreenOnChange;
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            _sectionGreen.OnChange -= SectionGreenOnChange;
        }

        private void SectionGreenOnChange(int prev, int next, bool asServer)
        {
            //Activate section on client
            if (!asServer) trafficLightsController.OverrideCurrentSection(next);
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            trafficLightsController.Simulate = true;
            // InstanceFinder.TimeManager.OnTick += TimeManagerOnOnTick;
            trafficLightsController.OnSectionIndexChanged += OnSectionIndexChanged;
        }

        public override void OnStopServer()
        {
            base.OnStopServer();
            trafficLightsController.Simulate = false;
            // InstanceFinder.TimeManager.OnTick -= TimeManagerOnOnTick;
            trafficLightsController.OnSectionIndexChanged -= OnSectionIndexChanged;
        }
        
        private void OnSectionIndexChanged(int newSectionIndex)
        {
            if (IsServerStarted) _sectionGreen.Value = newSectionIndex;
        }
    }
}
