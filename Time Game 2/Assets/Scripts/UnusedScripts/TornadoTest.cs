using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoTest : MonoBehaviour
{
    [SerializeField] private float tornadoRadius = 8f;
    [SerializeField] private float tornadoSpeed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LayerMask playerMask = LayerMask.GetMask("Player");
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, tornadoRadius, playerMask);

        foreach (Collider player in playerInRange)
        {
            Vector3 direction = (this.gameObject.transform.position - player.transform.position).normalized;
            player.GetComponent<CharacterController>().Move(direction * tornadoSpeed * Time.deltaTime);
            
            Debug.Log("Pulling player in");
        }
    }
}
