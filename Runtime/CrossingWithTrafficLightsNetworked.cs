using System.Collections.Generic;
using _3Dimensions.TrafficSystem.Runtime;
using FishNet;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
namespace _3Dimensions.TrafficSystemFishNet.Runtime
{
    public class CrossingWithTrafficLightsNetworked : NetworkBehaviour
    {
        private readonly SyncVar<int> _sectionGreen = new SyncVar<int>();
        public CrossingWithTrafficLights trafficLights;
        [SerializeField] private bool useDetectors;
        [Tooltip("Match the index number with the correct TrafficLightSections!")] public List<TrafficTrigger> triggers;
        [SerializeField] private float sectionDurationWithTriggers = 5f;
        [SerializeField] private float sectionDuration = 10f;
        private float _elapsedTime;

        private void Start()
        {
            _elapsedTime = 0;
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            SetSection(0, _sectionGreen.Value, false);
        }
        
        public override void OnStartServer()
        {
            base.OnStartServer();
            InstanceFinder.TimeManager.OnTick += TimeManagerOnOnTick;
        }

        private void TimeManagerOnOnTick()
        {
            if (useDetectors)
            {
                //Only recalculate after some short time to prevent looping to fast
                if (_elapsedTime > sectionDurationWithTriggers)
                {
                    CalculateNextSectionBasedOnTriggers();
                    _elapsedTime = 0;
                }
                return;
            }
            if (_elapsedTime > sectionDuration)
            {
                StartNextSection();
                _elapsedTime = 0;
            }
        }

        private void LateUpdate()
        {
            _elapsedTime += Time.deltaTime;
        }

        private void CalculateNextSectionBasedOnTriggers()
        {
            // Debug.Log("Calculating next Section based on triggers");
            
            float longestWaitTime = 0;
            TrafficTrigger triggerWithLongestWaitTime = null;
            
            foreach (TrafficTrigger t in triggers)
            {
                if (t.TimeTriggered > longestWaitTime)
                {
                    longestWaitTime = t.TimeTriggered;
                    triggerWithLongestWaitTime = t;
                }
            }

            if (triggerWithLongestWaitTime == null)
            {
                //no vehicles found, just start a next section;
                StartNextSection();
            }
            else
            {
                //Active longest waiting section
                _sectionGreen.Value = triggers.IndexOf(triggerWithLongestWaitTime);
            }
        }

        private void StartNextSection()
        {
            if (_sectionGreen.Value >= trafficLights.SectionsCount - 1)
            {
                _sectionGreen.Value = 0;
            }
            else
            {
                _sectionGreen.Value++;
            }
        }

        private void SetSection(int oldValue, int newValue, bool asServer)
        {
            trafficLights.currentSection = newValue;
        }
    }
}
