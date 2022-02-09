using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windblade : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    Vector3 direction;
    
    // Start is called before the first frame update
    void Start()
    {
        direction = PlayerMovement.moveDir;
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        transform.localScale += new Vector3(0.001f, 0.001f, 0f);

        //Rotate the windblade to face in the direction its moving
        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg - 90;

        Quaternion rotationToApply = Quaternion.AngleAxis(angle, Vector3.up);

        transform.eulerAngles = Vector3.up * angle;

       // transform.rotation = Quaternion.Slerp(transform.rotation, rotationToApply, Time.deltaTime * 50f);
    }
}
