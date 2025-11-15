using System.Collections;
using UnityEngine;

public class VFXPlay : MonoBehaviour
{
    [SerializeField] ParticleSystem m_particle;


    public void Play()
    {
        StartCoroutine(IEPlay());
    }

    IEnumerator IEPlay()
    {
        if(!m_particle.isPlaying)
            m_particle.Play();
            

        yield return new WaitForSeconds(0.5f);

        this.gameObject.SetActive(false);
    }
}
