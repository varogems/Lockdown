using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class G : MonoBehaviour
{
    [SerializeField] GameObject m_target;
    [SerializeField] GameObject m_spawner;


    void Start()
    {
        StartCoroutine(IESpawn());
    }

    IEnumerator IESpawn()
    {
        // while(true)
        {
            Instantiate(m_spawner, this.gameObject.transform.position, 
                    Quaternion.identity, this.transform);
                    
            yield return new WaitForSeconds(1);
        }

    }

}
