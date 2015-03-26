using UnityEngine;
using System.Collections;

public class toolCollision : MonoBehaviour
{
   public HandController hc;
   public ToolModel tool1;

   public GameObject scalpel;

   // Use this for initialization
   void Start()
   {
      scalpel = GameObject.Find("scalpelTrayModel");
   }

   // Update is called once per frame
   void Update()
   {

   }

   void OnCollisionEnter(Collision col)
   {
      if (col.gameObject.name == "Bip01 R Finger1Nub")
      {
         Debug.Log("you selected the injector");
         foreach (Renderer renderer in this.gameObject.GetComponentsInChildren(typeof(Renderer)))
         {
            renderer.enabled = false;
            hc.toolModel = tool1;
         }

         foreach (Renderer renderer in scalpel.gameObject.GetComponentsInChildren(typeof(Renderer)))
         {
            renderer.enabled = true;
         }
      }
   }
}
