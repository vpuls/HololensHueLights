// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;


namespace HoloToolkit.Unity
{
    /// <summary>
    /// HandsManager determines if the hand is currently detected or not.
    /// </summary>
    public partial class HandsManager : Singleton<HandsManager>
    {
        /// <summary>
        /// HandDetected tracks the hand detected state.
        /// Returns true if the list of tracked hands is not empty.
        /// </summary>
        public bool HandDetected
        {
            get { return trackedHands.Count > 0; }
        }

        private HashSet<uint> trackedHands = new HashSet<uint>();

        void Awake()
        {
            UnityEngine.XR.WSA.Input.InteractionManager.InteractionSourceDetectedLegacy += InteractionManager_SourceDetected;
            UnityEngine.XR.WSA.Input.InteractionManager.InteractionSourceLostLegacy += InteractionManager_SourceLost;
        }

        private void InteractionManager_SourceDetected(UnityEngine.XR.WSA.Input.InteractionSourceState state)
        {
            // Check to see that the source is a hand.
            if (state.source.kind != UnityEngine.XR.WSA.Input.InteractionSourceKind.Hand)
            {
                return;
            }

            trackedHands.Add(state.source.id);
        }

        private void InteractionManager_SourceLost(UnityEngine.XR.WSA.Input.InteractionSourceState state)
        {
            // Check to see that the source is a hand.
            if (state.source.kind != UnityEngine.XR.WSA.Input.InteractionSourceKind.Hand)
            {
                return;
            }

            if (trackedHands.Contains(state.source.id))
            {
                trackedHands.Remove(state.source.id);
            }
        }

        void OnDestroy()
        {
            UnityEngine.XR.WSA.Input.InteractionManager.InteractionSourceDetectedLegacy -= InteractionManager_SourceDetected;
            UnityEngine.XR.WSA.Input.InteractionManager.InteractionSourceLostLegacy -= InteractionManager_SourceLost;
        }
    }
}