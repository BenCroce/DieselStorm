using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnPointController : NetworkBehaviour
{
    public List<SpawnPointBehaviour> m_SpawnPoints;    
    public GameEventArgs m_SetNewSpawnPoint;

    void Start()
    {
        StartCoroutine(GetSpawnPoints());
    }
    
    public void PlayerSpawned(Object[] args)
    {
        var sender = args[0] as TeamBehaviour;
        if (sender != null)
        {
            int spawnIndex = Random.Range(0, m_SpawnPoints.Count - 2);
            m_SetNewSpawnPoint.Raise(this, m_SpawnPoints[spawnIndex].transform);
            Debug.Log(m_SpawnPoints[spawnIndex].name);
            MoveListItems(m_SpawnPoints[spawnIndex]);
        }
    }

    void MoveListItems(SpawnPointBehaviour spawnPoint)
    {
        var tempList = new List<SpawnPointBehaviour>();
        foreach (var point in m_SpawnPoints)
        {
            if(point != spawnPoint)
                tempList.Add(point);
        }
        tempList.Add(spawnPoint);
        m_SpawnPoints = tempList;
    }

    IEnumerator GetSpawnPoints()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            m_SpawnPoints = FindObjectsOfType<SpawnPointBehaviour>().ToList();
            int spawnIndex = Random.Range(0, m_SpawnPoints.Count - 2);
            m_SetNewSpawnPoint.Raise(this, m_SpawnPoints[spawnIndex].transform);
            Debug.Log(m_SpawnPoints[spawnIndex].name);
            MoveListItems(m_SpawnPoints[spawnIndex]);
            break;
        }
    }
}
