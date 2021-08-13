using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public List<Transform> points = new List<Transform>();

    void Awake()
    {
        for (int i = 0; i < transform.childCount ; i++) points.Add(transform.GetChild(i));
    }
}