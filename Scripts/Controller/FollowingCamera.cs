using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    public float     m_fDistanceAway  = 7.0f;
    public float     m_fDistanceUp    = 4.0f;
    public Transform m_transFollow = null;

    void LateUpdate()
    {
        Vector3 vecUp      = Vector3.up * m_fDistanceUp;
        Vector3 vecForward = Vector3.forward * m_fDistanceAway;

        transform.position = m_transFollow.position + vecUp - vecForward;
    }
}
