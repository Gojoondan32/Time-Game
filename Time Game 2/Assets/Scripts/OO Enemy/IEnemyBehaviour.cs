using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyBehaviour
{
    void ChasePlayer();
    bool PlayerIsAlive();
    void AttackPlayer();
    void FacePlayer();
    bool CanAttackPlayer();


}
