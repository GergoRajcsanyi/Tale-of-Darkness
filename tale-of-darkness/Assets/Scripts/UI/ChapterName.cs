using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterName : MonoBehaviour
{
    public GameObject chapterName;

    public void AnimationFinished()
    {
        chapterName.SetActive(false);
    }
}
