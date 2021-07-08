using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartMemoryMatching : MonoBehaviour
{
    
    
    //Category Buttons
    public enum ECategoryButtontype
    {
        NotSet,
        MatchingGameCategoryBtn
    };

    [SerializeField] public ECategoryButtontype CategoryButton = ECategoryButtontype.NotSet;

    [HideInInspector]
    public MatchingSettings.EMatchingCategories memoryCategories = MatchingSettings.EMatchingCategories.NotSet;


    //Load the matching game if CategoryReady is True From the MatchingSettings Script
    public void ReadyMemoryGame(string gameSceneName)
    {
        var comp = gameObject.GetComponent<StartMemoryMatching>();


        if(comp.CategoryButton == ECategoryButtontype.MatchingGameCategoryBtn)
        {
            MatchingSettings.Instance.SetMatchingGameCategories(comp.memoryCategories);
        }

        if (MatchingSettings.Instance.CategoryReady())
        {
            SceneManager.LoadScene(gameSceneName);
        }

    }
 
}
