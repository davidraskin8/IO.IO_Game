using UnityEngine;
using System.Collections;

public class UpdateMenuScript : MonoBehaviour
{

    public GameObject upgradeMenuPanel;
    private Animator anim; 
    private bool isUpgrade = false;

    void Start()
    {
        anim = upgradeMenuPanel.GetComponent<Animator>();
        anim.SetBool("isUpgrade", false);
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.U) && !isUpgrade)
        {
            AnimateMenu();
        }
        else if (Input.GetKeyUp(KeyCode.U) && isUpgrade)
        {
            DeAnimateMenu();
        }
    }
    
    public void AnimateMenu()
    {
        anim.SetBool("isUpgrade", true); //Start the animation for the upgrade menu
        isUpgrade = true;
        Debug.Log("Lowering Menu!!");
    }

    public void DeAnimateMenu()
    {
        anim.SetBool("isUpgrade", false); //Start the deanimation for the upgrade menu
        isUpgrade = false;
        Debug.Log("Raising Menu!!");
    }

}
