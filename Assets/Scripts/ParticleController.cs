using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
   public ParticleSystem particleSys; 

    private void Awake()
    {
        particleSys = gameObject.GetComponent<ParticleSystem>();
    }
   public void PlayOnLevelFail()
   {
        particleSys.Play();
   }
}
