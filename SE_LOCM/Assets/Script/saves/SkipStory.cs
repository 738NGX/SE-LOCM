using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;
using UnityEngine.UI;

public class SkipStory : MonoBehaviour
{
    private Button skipButton;

    void Start()
    {
        skipButton = gameObject.GetComponent<Button>();
        StorySceneLoader storyLoader = FindObjectOfType<StorySceneLoader>();

        if (storyLoader != null && skipButton != null)
        {
            skipButton.onClick.AddListener(TryStopFungusMusic);
            skipButton.onClick.AddListener(storyLoader.Transmission);
        }
        else
        {
            Debug.LogError("SkipStory: StorySceneLoader或按钮未找到!");
        }
    }

    public void TryStopFungusMusic()
    {
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (var source in audioSources)
        {
            source.Stop();
        }
    }
}
