using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceLevel : MonoBehaviour {

    private float exp;
    private float level;
    private float expNeeded;

    public Slider experienceBar;
    public Text levelText;
    public GameObject animatedPanel;
    private UpdateMenuScript menuAnimator;

    void Start()
    { 
        exp = 0;
        experienceBar.value = 0;
        level = 1;
        levelText.text = "Level: "+ level;
        expNeeded = 10 * level;
        menuAnimator = animatedPanel.GetComponent<UpdateMenuScript>();
    }

    public void AddExp()
    {
        exp += 1; //Adde to experience
        if(exp >= expNeeded)
        {
            AddLevel(); //when the experience is greater than the amount needed to level up increase the level
        }

        experienceBar.value = exp / expNeeded; //Update experience bar
    }

    //Add level to the level text
    private void AddLevel()
    {
        level += 1;
        exp = 0;
        experienceBar.value = 0;
        levelText.text = "Level " + level;
        expNeeded = 10 * level;

        if(level >= 2)
        {
            menuAnimator.AnimateMenu();
        }
    }
    
}
