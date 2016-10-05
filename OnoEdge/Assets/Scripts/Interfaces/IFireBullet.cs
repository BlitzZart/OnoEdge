using UnityEngine;
using System.Collections;

// IFireBullet can only be used by NetworkTransform
public interface IFireBullet {
    // [Command]
    void CmdFireBullet(Vector3 pos, Vector3 dir, float speed);
    // [Command]
    void RpcSpawnBullet();
}
