using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float loadSceneDelayTime = 2f;
    [SerializeField] AudioClip explosionSFX, successSFX;
    [SerializeField] GameObject explosionPrefab;

    AudioSource audioSource;

    bool isTransitioning = false;
    bool collisionDisabled = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))    //Press 'L' to instant load next scene
        {
            LoadNextLevel();
            Debug.Log("Next scene loaded by Admin");
        }
        else if (Input.GetKeyDown(KeyCode.C))       //Press 'C' to disable collisions
        {
            collisionDisabled = !collisionDisabled;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        Vector3 explosionPos = other.GetContact(0).point;
        if (isTransitioning || collisionDisabled) { return; }
        switch (other.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Fuel":
                break;
            case "Finish":
                StartLoadSequence();
                break;
            default:
                TriggerExplosion(explosionPos);
                StartCrashSequence();
                break;
        }
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(explosionSFX);
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", loadSceneDelayTime);
    }

    void StartLoadSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(successSFX);
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", loadSceneDelayTime);
    }

    void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    void InstantLoadNextLevel()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
            Debug.Log("Next scene loaded by Admin");
        }
    }

    void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void TriggerExplosion(Vector3 pos)
    {
        Instantiate(explosionPrefab, pos, Quaternion.identity);
    }

    void DisableCollision()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isTransitioning = true;
            Debug.Log("Collisions disabled by Admin");
        }
    }
}
