using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cutscenes : MonoBehaviour
{
    [SerializeField] private string id;

    void OnEnable()
    {
        switch (id)
        {
            case "intro":
                FindObjectOfType<ChapterChanger>().FadeToNextChapter();
                break;

            case "outro":
                FindObjectOfType<ChapterChanger>().FadeToChapter(0);
                break;
        }
    }
}