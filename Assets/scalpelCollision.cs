using UnityEngine;
using System.Collections;

public class scalpelCollision : MonoBehaviour
{
   public GameObject injector;
   public HandController hc;
   public ToolModel tool;

   // Use this for initialization
   void Start()
   {
      injector = GameObject.Find("injectorTrayModel");
   }

   // Update is called once per frame
   void Update()
   {

   }

   void OnCollisionEnter(Collision col)
   {
      if (col.gameObject.name == "Bip01 R Finger1Nub")
      {
         Debug.Log("you selected the Scalpel");
         foreach (Renderer renderer in this.gameObject.GetComponentsInChildren(typeof(Renderer)))
         {
            renderer.enabled = false;
            hc.toolModel = tool;
         }

         foreach (Renderer renderer in injector.gameObject.GetComponentsInChildren(typeof(Renderer)))
         {
            renderer.enabled = true;
         }
      }
   }
}
