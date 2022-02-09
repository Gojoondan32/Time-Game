using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WaypointSingleton
{
    private static WaypointSingleton instance;
    private List<GameObject> waypoints = new List<GameObject>();
    public List<GameObject> Waypoints
    {
        get
        {
            return waypoints;
        }
    }

    //Keep a track of all the waypoints in the scene
    public static WaypointSingleton Singleton
    {
        get
        {
            if(instance == null)
            {
                instance = new WaypointSingleton();
                instance.Waypoints.AddRange(GameObject.FindGameObjectsWithTag("Waypoints"));

                //Reorder the waypoints into alpabetical order
                instance.waypoints = instance.waypoints.OrderBy(waypoint => waypoint.name).ToList();
            }
            return instance;
        }
    }
}
