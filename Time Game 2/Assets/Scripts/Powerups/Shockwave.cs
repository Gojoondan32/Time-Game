using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour
{

    public float damageDealt = 50f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        BeginShockwave();
    }
    public void BeginShockwave()
    {
        this.gameObject.transform.localScale += new Vector3(0.15f, 0.01f, 0.15f);
        Destroy(this.gameObject, 3f);


    }
    
}
