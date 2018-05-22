using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResourcesListContainor<T> : MonoBehaviour, IEnumerable
{
    List<T> containorList = new List<T>();

    public int CurrentSelection = -1;

    public IEnumerator GetEnumerator()
    {
        return containorList.GetEnumerator();
    }

    public UnityEvent<int> OnChangeSelection;
}
