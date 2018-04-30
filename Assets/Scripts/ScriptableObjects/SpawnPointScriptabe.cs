using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpawnPointScriptabe : ScriptableObject
{
    public Transform m_TransformLocation;
    public string m_SpawnName;
    public float m_SpawnWidth;
    public float m_SpawnLength;

    public Vector3 GetSpawnPosition()
    {
        var spawnCenter = m_TransformLocation.position;
        return new Vector3(spawnCenter.x + Random.Range(-m_SpawnWidth, m_SpawnWidth), 
                           spawnCenter.y,
                           spawnCenter.z + Random.Range(-m_SpawnLength, m_SpawnLength));
    }
}
