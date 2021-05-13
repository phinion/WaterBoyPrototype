using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityComponent : MonoBehaviour
{
    protected Entity entity;

    protected virtual void Awake()
    {
        entity = transform.parent.GetComponent<Entity>();

        if(entity == null)
        {
            Debug.LogError("Gameobject is not an entity");
        }
    }


}
