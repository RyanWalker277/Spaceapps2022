// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Events;
// using UnityEngine.XR.ARFoundation;
// using UnityEngine.XR.ARSubsystems;

// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Events;
// using UnityEngine.XR.ARFoundation;
// using UnityEngine.XR.ARSubsystems;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine.Networking;
// using SimpleJSON;
// using UnityEngine.UI;
// using TMPro;
// using Microsoft.Geospatial;
// using Microsoft.Maps.Unity;

//     /// <summary>
//     /// Listens for touch events and performs an AR raycast from the screen touch point.
//     /// AR raycasts will only hit detected trackables like feature points and planes.
//     ///
//     /// If a raycast hits a trackable, the <see cref="placedPrefab"/> is instantiated
//     /// and moved to the hit position.
//     /// </summary>
//     [RequireComponent(typeof(ARRaycastManager))]
//     public class PlaceOnPlane : MonoBehaviour
//     {
//         [SerializeField]
//         [Tooltip("Instantiates this prefab on a plane at the touch location.")]
//         MapRenderer m_PlacedPrefab;

//     UnityEvent placementUpdate;

//     [SerializeField]
//     GameObject visualObject;

//     /// <summary>
//     /// The prefab to instantiate on touch.
//     /// </summary>
//     public MapRenderer placedPrefab
//         {
//             get { return m_PlacedPrefab; }
//             set { m_PlacedPrefab = value; }
//         }

//         /// <summary>
//         /// The object instantiated as a result of a successful raycast intersection with a plane.
//         /// </summary>
//         public MapRenderer spawnedObject { get; private set; }

//         void Awake()
//         {
//             m_RaycastManager = GetComponent<ARRaycastManager>();

//             if (placementUpdate == null)
//                 placementUpdate = new UnityEvent();

//                 placementUpdate.AddListener(DiableVisual);
//         }

//         bool TryGetTouchPosition(out Vector2 touchPosition)
//         {
//             if (Input.touchCount > 0)
//             {
//                 touchPosition = Input.GetTouch(0).position;
//                 return true;
//             }

//             touchPosition = default;
//             return false;
//         }

//         void Update()
//         {
//             if (!TryGetTouchPosition(out Vector2 touchPosition))
//                 return;

//             if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
//             {
//                 // Raycast hits are sorted by distance, so the first one
//                 // will be the closest hit.
//                 var hitPose = s_Hits[0].pose;
            
//                 if (spawnedObject == null)
//                 {
//                     spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);
                    
//                 }
//                 else
//                 {
//                     //repositioning of the object 
//                     // spawnedObject.transform.position = hitPose.position;
//                     spawnedObject.SetMapScene(new MapSceneOfLocationAndZoomLevel(new LatLon(46 , 46 ), 15.0f));
//                 }
//                     placementUpdate.Invoke();
//             }

            

//         }

//     public void DiableVisual()
//     {
//         visualObject.SetActive(false);
//     }

//         static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

//         ARRaycastManager m_RaycastManager;
//     }



using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.UI;
using TMPro;
using Microsoft.Geospatial;
using Microsoft.Maps.Unity;

public class PlaceOnPlane : MonoBehaviour
{
    public MapRenderer map;
    public MapRenderer spawn_map;
    public MapPin mapin;
    public MapPin spawn_mapin;
    private readonly string basePokeURL = "http://api.open-notify.org/iss-now.json";
        
        [SerializeField]
        [Tooltip("Instantiates this prefab on a plane at the touch location.")]
        MapRenderer m_PlacedPrefab;

    UnityEvent placementUpdate;

    [SerializeField]
    GameObject visualObject;

    public MapRenderer placedPrefab
        {
            get { return m_PlacedPrefab; }
            set { m_PlacedPrefab = value; }
        }

        public MapRenderer spawnedObject { get; private set; }

        void Awake()
        {
            m_RaycastManager = GetComponent<ARRaycastManager>();

            if (placementUpdate == null)
                placementUpdate = new UnityEvent();

                placementUpdate.AddListener(DiableVisual);
        }

        bool TryGetTouchPosition(out Vector2 touchPosition)
        {
            if (Input.touchCount > 0)
            {
                touchPosition = Input.GetTouch(0).position;
                return true;
            }

            touchPosition = default;
            return false;
        }

        void Update()
        {
            if (!TryGetTouchPosition(out Vector2 touchPosition))
                return;

            if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                var hitPose = s_Hits[0].pose;
            
                if (spawnedObject == null)
                {
                    spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);
                    spawn_map = spawnedObject;
                    
                }
                else
                {
                    spawnedObject.transform.position = hitPose.position;
                }
                    placementUpdate.Invoke();
            }
            
        }
    
    
    public void DiableVisual()
    {
        visualObject.SetActive(false);
    }

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    ARRaycastManager m_RaycastManager;

    public void OnButtonRandomPokemon()
    {
        spawn_mapin = Instantiate<MapPin>(mapin);
        spawn_mapin.transform.parent = spawn_map.transform;
        StartCoroutine(GetPokemonAtIndex());
    }

    public IEnumerator GetPokemonAtIndex()
    {
                string pokemonURL = basePokeURL;
                UnityWebRequest pokeInfoRequest = UnityWebRequest.Get(pokemonURL);

                yield return pokeInfoRequest.SendWebRequest();

                if (pokeInfoRequest.isNetworkError || pokeInfoRequest.isHttpError)
                {
                    Debug.LogError(pokeInfoRequest.error);
                    yield break;
                }

                JSONNode pokeInfo = JSON.Parse(pokeInfoRequest.downloadHandler.text);

                string s = "E04";

                double pokeName = pokeInfo["iss_position"]["longitude"];
                double latitude = pokeInfo["iss_position"]["latitude"];
                spawn_map.SetMapScene(new MapSceneOfLocationAndZoomLevel(new LatLon(pokeName,latitude), 15.0f));
                spawn_mapin.Location = new LatLon(pokeName,latitude);
    }

}