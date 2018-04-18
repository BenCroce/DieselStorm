using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientAuthorityBehaviour : NetworkBehaviour
{    
    public void CmdAssignClientAuthority(NetworkIdentity id)
    {
        var newid = this.GetComponent<NetworkIdentity>();
        var connection = newid.connectionToClient;
        id.AssignClientAuthority(connection);
    }
}
