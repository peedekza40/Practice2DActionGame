using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Optimizer : MonoBehaviour
{
    private List<Transform> SiblingObjects = new List<Transform>();

    private void Start() 
    {
        for(int i = 0; i < transform.parent.childCount; i++){
            var child = transform.parent.GetChild(i);
            if(child.name != transform.name)
            {
                SiblingObjects.Add(child);
            }
        }
        
        SetActiveSibling(false);
    }

    private void OnBecameVisible() 
    {
        SetActiveSibling(true);
    }

    private void OnBecameInvisible() 
    {
        SetActiveSibling(false);
    }

    private void SetActiveSibling(bool isActive)
    {
        foreach(var siblingObject in SiblingObjects)
        {
            siblingObject.gameObject.SetActive(isActive);
        }
    }
}
