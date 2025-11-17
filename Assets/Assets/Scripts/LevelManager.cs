using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] float delayLoadScene = 2f;
    [SerializeField] AudioSource m_audioSource;
    [SerializeField] AudioClip m_pressAudio;


    public enum eScene
    {
        Menu = 0,
        WarmUp,
        Campaign
    }
    void Awake()
    {
        Debug.Log("Awake");
    }
    IEnumerator loadScene(eScene scene)
    {
        if(m_audioSource != null) m_audioSource.PlayOneShot(m_pressAudio);

        if(scene != eScene.Menu)
            yield return new WaitForSeconds(delayLoadScene);
        
        SceneManager.LoadScene((int)scene);
    }


    public void Replay()
    {
        StartCoroutine(loadScene((eScene)SceneManager.GetActiveScene().buildIndex));
    }

    public void LoadMenu()
    {
        Debug.Log("LoadMenu");
        StartCoroutine(loadScene(eScene.Menu));
    }

    public void LoadWarmUp()
    {
        StartCoroutine(loadScene(eScene.WarmUp));
    }

    public void LoadCampaign()
    {
        Debug.Log("LoadCampaign");
        StartCoroutine(loadScene(eScene.Campaign));
    }

 

    public void Quit()
    {
        if(m_audioSource != null) m_audioSource.PlayOneShot(m_pressAudio);
        Application.Quit();
    }

    void Update()
    {
        //! Escapte to menu scene.
        if (Keyboard.current.escapeKey.wasPressedThisFrame && (SceneManager.GetActiveScene().buildIndex != 0))
        {
            Cursor.lockState = CursorLockMode.None;
            LoadMenu();
        }
    }
}