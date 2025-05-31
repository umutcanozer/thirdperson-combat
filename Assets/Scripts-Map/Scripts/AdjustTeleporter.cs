using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustTeleporter : MonoBehaviour
{
   [SerializeField] private GameObject teleportCollider;

   private void OnTriggerEnter(Collider other)
   {
      if(other.CompareTag("Player")) teleportCollider.SetActive(true);
      else if(other.CompareTag("Rabbit")) Destroy(other);
   }
}
