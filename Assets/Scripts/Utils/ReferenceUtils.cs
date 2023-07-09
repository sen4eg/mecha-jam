using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ReferenceUtils
{
    public static List<IReferencable> GetReferencablesWithTag(GameObject gameObject, string tag)
    {
        List<IReferencable> referencables = new List<IReferencable>();

        IReferencable[] components = gameObject.GetComponents<IReferencable>();
        foreach (IReferencable component in components)
        {
            if (HasTag(component, tag))
            {
                referencables.Add(component);
            }
        }

        return referencables;
    }

    private static bool HasTag(IReferencable referencable, string tag)
    {
        if (referencable is MonoBehaviour monoBehaviour)
        {
            if (Attribute.GetCustomAttributes(monoBehaviour.GetType(), typeof(ReferencableAttribute)) is ReferencableAttribute[] attributes)
            {
                foreach (var attribute in attributes)
                {
                    if (attribute.Tag == tag)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
    
}