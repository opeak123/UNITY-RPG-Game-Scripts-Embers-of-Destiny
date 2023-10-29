using UnityEngine;
using Cinemachine;
public class ReaperCinamacine : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineTrackedDolly dolly;
    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        dolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
    }
    public void AutoDollyEnabled()
    {
        dolly.m_AutoDolly.m_Enabled = true;
    }
    public void ResetCameraTrack()
    {
        CinemachineTrackedDolly trackedDolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
        if (trackedDolly != null)
        {
            trackedDolly.m_PathPosition = 0.0f;
            if(trackedDolly.m_PathPosition == 0f)
            {
                dolly.m_AutoDolly.m_Enabled = false;
            }
        }
    }
}
