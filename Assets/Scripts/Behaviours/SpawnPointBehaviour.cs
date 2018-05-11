using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointBehaviour : MonoBehaviour
{
    public SpawnPointScriptabe m_SpawnPointScriptable;

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if(m_SpawnPointScriptable != null)
        {
            m_SpawnPointScriptable.m_TransformLocation = this.transform;
            Gizmos.DrawCube(transform.position, new Vector3(m_SpawnPointScriptable.m_SpawnLength,
                                                            0,
                                                            m_SpawnPointScriptable.m_SpawnWidth));
        }
    }
#endif
}
