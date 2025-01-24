using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSingleton : Singleton<BaseSingleton>{

    protected override bool ShouldDestroyInScene(string sceneName)
    {
        return sceneName == "MainMenuScene";
    }

}
