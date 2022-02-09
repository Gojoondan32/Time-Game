using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    //Access an instance to create the popup at the correct position with the correct damage amount
    public static DamagePopup Create(Vector3 position, float damageAmount)
    {
        Transform damagePopupTransform = Instantiate(DamagePopupManager.i.damagePopup, position, Quaternion.identity);
        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damageAmount);

        return damagePopup;
    }
    
    
    
    private TextMeshPro text;
    private float disappearTimer;
    private Color textColor;
    

    private void Awake()
    {
        text = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(float damageAmount)
    {
        text.SetText(damageAmount.ToString());
        textColor = text.color;
        disappearTimer = 1f;
    }

    private void Update()
    {
        float moveYSpeed = 3.5f;
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

        disappearTimer -= Time.deltaTime;
        if(disappearTimer <= 0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            text.color = textColor;

            if(textColor.a <= 0)
            {
                Destroy(gameObject);
            }
        }
        LookAtPlayer();
    }

    private void LookAtPlayer()
    {
        Vector3 targetDirection = PlayerManager.instance.gameObject.transform.position - transform.position;
        float turnSpeed = 1f * Time.deltaTime;
        transform.LookAt(targetDirection);

        //Vector3 directionToTarget = Vector3.RotateTowards(transform.forward, targetDirection, turnSpeed, 0f);

        //transform.rotation = Quaternion.LookRotation(directionToTarget);
    }
}
