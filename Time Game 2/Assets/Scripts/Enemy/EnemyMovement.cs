using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private CharacterController controller;
    [SerializeField] private float speed = 1f;

    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();

        GameObject tempPlayer = GameObject.FindGameObjectWithTag("Player");
        player = tempPlayer.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Find the players position

        //Move towards the players position
        Vector3 direction = player.transform.position - transform.position;
        //Look at the player
        transform.LookAt(player.transform.position);


        controller.Move(direction * speed * Time.deltaTime);
    }
}
