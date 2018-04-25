using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Prototype.NetworkLobby;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Networking;

public class TeamSetupBehaviour : NetworkBehaviour
{
    public TeamSetupSingleton m_TeamSetupSingleton;
    public TeamSriptable m_TeamConfig;
    public GameObject m_TeamObject;
    
    private void Start()
    {
        if (!isServer)
            return;
        StartCoroutine(GetPlayers());
    }

    public IEnumerator GetPlayers()
    {
        while(true)
        {
            yield return new WaitForSeconds(2.0f);
            foreach (var player in FindObjectsOfType<PlayerBehaviour>())
            {
                m_TeamSetupSingleton.AddPlayer(player);
            }
            m_TeamSetupSingleton.BalanceTeams();
            break;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(TeamSetupBehaviour))]
public class TeamSetupEditor : UnityEditor.Editor
{
    private int numTeams; 
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var tar = (TeamSetupBehaviour)target;
        if (GUILayout.Button("Create New Team"))
        {            
            var newTeam = tar.m_TeamConfig.CreateInstance();            
            AssetDatabase.CreateAsset(newTeam, "Assets/Resources/newTeam" + 
                (tar.m_TeamSetupSingleton.m_Teams.Count + 1));
            AssetDatabase.SaveAssets();
            if (tar.m_TeamSetupSingleton.AddTeam(newTeam))
            {
                var teamObj = Instantiate(tar.m_TeamObject);
                teamObj.GetComponent<TeamBehaviour>().m_TeamScriptable = newTeam;
                teamObj.name = newTeam.name;
            }
        }
        if (GUILayout.Button("Clear Teams"))
        {
            foreach (var team in tar.m_TeamSetupSingleton.m_Teams)
            {
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(team));
            }
            tar.m_TeamSetupSingleton.ClearTeams();
            var teamObjs = FindObjectsOfType<TeamBehaviour>();
            for (int i = 0; i < teamObjs.Length; i++)
            {
                DestroyImmediate(teamObjs[i].gameObject);
            }
        }
    }
}
#endif
