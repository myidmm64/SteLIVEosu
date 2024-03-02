using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    private static CinemachineVirtualCamera _vCam = null;
    public static CinemachineVirtualCamera VCam
    {
        get
        {
            if (_vCam == null)
            {
                _vCam = GameObject.FindAnyObjectByType<CinemachineVirtualCamera>();
            }
            return _vCam;
        }
    }

    private static Camera _cam = null;
    public static Camera Cam
    {
        get
        {
            if (_cam == null)
            {
                _cam = Camera.main;
            }
            return _cam;
        }
    }
}
