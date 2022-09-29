using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SprintBarScript : MonoBehaviour
{
    public static SprintBarScript instance;
    [SerializeField] GameObject player;
    [SerializeField] Image bar;
    [SerializeField] Text text;

    private PlayerMotor playerMotor;


    // Start is called before the first frame update
    void Start()
    {
        //singleton pattern, ensures there is only one in the scene
        if (instance == null)
        {
            instance = this;
        }
        //if i'm not the instance
        else
        {
            //end me please
            Destroy(gameObject);
        }

        player.TryGetComponent<PlayerMotor>(out playerMotor);
        text.text = playerMotor.SprintStam.ToString();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        DisplaySprintMeter();
        text.text = playerMotor.SprintStam.ToString();
    }

    void DisplaySprintMeter()
    {
        bar.fillAmount = playerMotor.SprintStam / playerMotor.MaxSprintStam;
    }
}
