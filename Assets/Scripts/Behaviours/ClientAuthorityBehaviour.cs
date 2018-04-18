using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class ClientAuthorityBehaviour : NetworkBehaviour
{
    [ClientRpc]
    public void RpcAssignClientAuthority(NetworkIdentity id)
    {
        var curid = this.GetComponent<NetworkIdentity>();
        var connection = curid.connectionToClient;
        curid.AssignClientAuthority(connection);        
    }
}
