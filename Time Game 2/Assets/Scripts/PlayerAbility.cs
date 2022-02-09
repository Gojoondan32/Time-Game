using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAbility : MonoBehaviour
{
    public static float comboScore = 0;

    [Header("UI References")]
    [SerializeField] private Image content;
    [SerializeField] private Image timeGreyScale;
    [SerializeField] private ParticleSystem timeCompleteParticle;
    

    private float timeCooldown = 2f;
    [SerializeField] private float timeStart = 0f;

    private float fillAmount;
    private float fillSpeed = 0.1f;
    [SerializeField] private float timeScore = 5f;

    
    // Start is called before the first frame update
    void Start()
    {
        comboScore = 0;
        content.fillAmount = 0f;

        timeGreyScale.gameObject.SetActive(false);
        timeCompleteParticle.Play(true);
    }

    // Update is called once per frame
    void Update()
    {
        //Check if the player has enough combo points to stop time
        if(TimeCanStop(comboScore) && Input.GetKeyDown(KeyCode.Q))
        {
            Time.timeScale = 0;
            comboScore = 0;
            timeGreyScale.gameObject.SetActive(true);
        }

        //Only allow time to be stopped for 2 seconds before going back to normal
        if (Time.timeScale == 0 && timeCooldown >= timeStart)
        {
            timeStart += Time.unscaledDeltaTime;
        }
        else
        {
            Time.timeScale = 1;
            timeStart = 0f;
            timeGreyScale.gameObject.SetActive(false);
        }
        if(fillAmount != comboScore)
        {
            UpdateFillAmount();
            fillAmount = comboScore;
        }
        
        
    }

    private void UpdateFillAmount()
    {
        Debug.Log(comboScore.ToString());
        /*
        if(fillAmount != content.fillAmount)
        {
            content.fillAmount = Mathf.Lerp(content.fillAmount, fillAmount, Time.deltaTime * fillSpeed);
        }
        */
        content.fillAmount = comboScore / timeScore;
    }
    
    private bool TimeCanStop(float comboScore)
    {
        if(comboScore >= timeScore)
        {
            comboScore = timeScore;
            timeCompleteParticle.Play();
;
            return true;
        }
        timeCompleteParticle.Stop();
        return false;
    }

    
}
