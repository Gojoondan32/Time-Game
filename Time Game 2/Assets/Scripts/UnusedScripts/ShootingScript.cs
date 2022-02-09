using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    [SerializeField] private int damageDealt = 20;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Ray mouseRay = GetComponentInChildren<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit info;

            //Shoot a raycast
            if(Physics.Raycast(mouseRay, out info))
            {
                Debug.DrawLine(transform.position, info.point, Color.red, 5.0f);
                Health enemyHealth = info.transform.GetComponent<Health>();

                if(enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damageDealt);
                }
            }
        }
    }
}
