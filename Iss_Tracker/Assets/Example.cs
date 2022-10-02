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


public class Example : MonoBehaviour
{
    public MapRenderer map;
    public MapRenderer spawn_map;
    public MapPin mapin;
    public MapPin spawn_mapin;
    private readonly string basePokeURL = "http://api.open-notify.org/iss-now.json";

    void Start()
    {
        spawn_map= Instantiate(map, new Vector3(0, 0), Quaternion.identity);
        spawn_mapin = Instantiate<MapPin>(mapin);
        spawn_mapin.transform.parent = spawn_map.transform;
    }

    public void OnButtonRandomPokemon()
    {
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

                // double pokeName = pokeInfo["iss_position"]["longitude"];
                // double latitude = pokeInfo["iss_position"]["latitude"];
                double pokeName = 10;
                double latitude = 10;
                spawn_map.SetMapScene(new MapSceneOfLocationAndZoomLevel(new LatLon(pokeName,latitude), 15.0f));
                // map.SetMapScene(new MapSceneOfLocationAndZoomLevel(new LatLon(pokeName,latitude ), 15.0f));
                spawn_mapin.Location = new LatLon(pokeName,latitude);
                Debug.Log(pokeName);
                Debug.Log(latitude);
    }

}
