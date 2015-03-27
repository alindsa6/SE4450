using UnityEngine;
using System.Collections;

public class SawCollider : MonoBehaviour {

   public ToolModel thisTool;
   public GameObject injector;
   public GameObject scalpel;
   public HandController hc;
   // Use this for initialization
   void Start ()
   {
      scalpel = GameObject.Find("scalpelTrayModel");
      injector = GameObject.Find("injectorTrayModel");   
   }
   
   // Update is called once per frame
   void Update () {
   
   }

   void OnCollisionEnter(Collision col)
   {
      if (col.gameObject.name == "Bip01 R Finger1Nub")
      {
         Debug.Log("you selected the saw");
         foreach (Renderer renderer in this.gameObject.GetComponentsInChildren(typeof(Renderer)))
         {
            renderer.enabled = false;
            hc.toolModel = thisTool;
         }

         foreach (Renderer renderer in injector.gameObject.GetComponentsInChildren(typeof(Renderer)))
         {
            renderer.enabled = true;
         }
         foreach (Renderer renderer in scalpel.gameObject.GetComponentsInChildren(typeof(Renderer)))
         {
            renderer.enabled = true;
         }
      }
   }
}
