using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchingSettings : MonoBehaviour
{

    private int _settings;
    private const int SettingsNumber = 1;
    //dictionary to identify which category is chosen
    private readonly Dictionary<EMatchingCategories, string> matchingCatDirectory = new Dictionary<EMatchingCategories, string>();


    public enum EMatchingCategories
    {
        //categories
        NotSet,
        AnimalsCategory,
        FoodCategory,
        SportsCategory
    };


    public struct Settings
    {
        public EMatchingCategories MatchingCategories;
    }


    private Settings _gameSettings;

    public static MatchingSettings Instance;

    void Awake()
    {
        if(Instance == null)
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        setCategoryDirectory();
        _gameSettings = new Settings();
        ResetMemoryGameSettings();
    }


    //loading the dictionary with the Resource Path strings
    private void setCategoryDirectory()
    {
     
        matchingCatDirectory.Add(EMatchingCategories.FoodCategory, "FoodCategory");
        matchingCatDirectory.Add(EMatchingCategories.AnimalsCategory, "AnimalsCategory");
        matchingCatDirectory.Add(EMatchingCategories.SportsCategory, "SportsCategory");
    }


    public void SetMatchingGameCategories(EMatchingCategories cat)
    {
        if (_gameSettings.MatchingCategories == EMatchingCategories.NotSet)
        {
            _settings++;
        }

        _gameSettings.MatchingCategories = cat;
    }

    public EMatchingCategories getMatchingCategory()
    {
        return _gameSettings.MatchingCategories;
    }

    public void ResetMemoryGameSettings()
    {
        _settings = 0;
        _gameSettings.MatchingCategories = EMatchingCategories.NotSet;
                 
    }

    public bool CategoryReady()
    {
        return _settings == SettingsNumber;
    }

    public string GetMaterialDirectoryName()
    {
        return "Materials/";
    }

    //Get the directory of images from chosen category
    public string GetCategoryTextureDirectoryName()
    {
        if(matchingCatDirectory.ContainsKey(_gameSettings.MatchingCategories))
        {
            return "MatchingUI/MatchingCategories/" + matchingCatDirectory[_gameSettings.MatchingCategories] + "/";
        }
        else
        {
            Debug.Log("Error: Cannot get the Directory name");
            return " ";
        }
    }
}
