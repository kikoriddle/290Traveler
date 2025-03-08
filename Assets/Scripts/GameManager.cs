using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //create an array that have four scenes
    public string[] roomScenes = { "Room1", "Room2", "Room3", "Room4" };
    public bool isShifting = false;
    // Start is called before the first frame update

    private AudioSource audioSource;
    public AudioClip transitionSound;

    private void Awake()
    {
       DontDestroyOnLoad(this);
    }

    void Start()
    {
        //shift every minuate
        audioSource = GetComponent<AudioSource>();
        InvokeRepeating("ShuffleScenes", 30f, 10f);
    }

    void ShuffleScenes()
    {
        //isShifting = true; 

        //make it play a sound later?
        string lastScene = roomScenes[roomScenes.Length - 1]; // Store the last scene

        // Shift all elements to the right
        for (int i = roomScenes.Length - 1; i > 0; i--)
        {
            roomScenes[i] = roomScenes[i - 1];
        }

        // Move the last scene to the first position
        roomScenes[0] = lastScene;

        Debug.Log("Updated Room Order: " + string.Join(", ", roomScenes));
        // Play sound when shifting rooms
        PlaySound();

    }

    public void LoadRoom(int accessPointIndex)
    {
        StartCoroutine(LoadSceneWithEffect(roomScenes[accessPointIndex]));
    }


    IEnumerator LoadSceneWithEffect(string sceneName)
    {
        //screenFade.SetActive(true); // Enable fade effect
        yield return new WaitForSeconds(0.5f); // Wait for fade
        SceneManager.LoadScene(sceneName); // Load the scene
    }

    void PlaySound()
    {
        if (transitionSound != null)
        {
            audioSource.PlayOneShot(transitionSound); // Play sound effect
        }
    }
}
