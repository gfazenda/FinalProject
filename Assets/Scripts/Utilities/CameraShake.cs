using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
public class CameraShake : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
            CameraShaker.Instance.ShakeOnce(4f,4f,.1f,1f);
        }

    }
}

