using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
   public ParticleSystem particleSys; 

   public void PlayOnLevelFail()
   {
        gameObject.SetActive(true);
   }
}
