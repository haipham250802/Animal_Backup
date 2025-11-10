using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackScene : MonoBehaviour
{
    [SerializeField] int indexScene;
    public void BackToScene()
    {
        SceneManager.LoadScene(indexScene);
    }
}
