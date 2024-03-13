using System.Collections;
using UnityEngine.XR.Management;

namespace EnablingXR
{
    public static class XRController
    {
        /// <summary>
        /// Initializes XR
        /// </summary>
        public static IEnumerator EnableXRCoroutine()
        {
            // Make sure the XR is disabled and properly disposed. It can happen that there is an activeLoader left
            // from the previous run.
            if (XRGeneralSettings.Instance.Manager.activeLoader ||
                XRGeneralSettings.Instance.Manager.isInitializationComplete)
            {
                DisableXR();
                // Wait for the next frame, just in case
                yield return null;
            }

            // Make sure we don't have an active loader already
            if (!XRGeneralSettings.Instance.Manager.activeLoader)
            {
                yield return XRGeneralSettings.Instance.Manager.InitializeLoader();
            }

            // Make sure we have an active loader, and the manager is initialized
            if (XRGeneralSettings.Instance.Manager.activeLoader &&
                XRGeneralSettings.Instance.Manager.isInitializationComplete)
            {
                XRGeneralSettings.Instance.Manager.StartSubsystems();
            }
        }

        /// <summary>
        /// Disables XR
        /// </summary>
        public static void DisableXR()
        {
            // Make sure there is something to de-initialize
            if (XRGeneralSettings.Instance.Manager.isInitializationComplete)
            {
                XRGeneralSettings.Instance.Manager.StopSubsystems();
                XRGeneralSettings.Instance.Manager.DeinitializeLoader();
            }
        }
    }
}