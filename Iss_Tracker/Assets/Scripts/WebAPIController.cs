using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.UI;
using TMPro;
using Microsoft.Geospatial;
using Microsoft.Maps.Unity;

public class WebAPIController : MonoBehaviour
{
    public MapRenderer map;
    public MapPin mapin;
    private readonly string basePokeURL = "http://api.open-notify.org/iss-now.json";

    private void Awake()
    {
    }

    public void OnButtonRandomPokemon()
    {
        StartCoroutine(GetPokemonAtIndex());
    }

    IEnumerator GetPokemonAtIndex()
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

        double pokeName = pokeInfo["iss_position"]["longitude"];
        double latitude = pokeInfo["iss_position"]["latitude"];

        map.SetMapScene(new MapSceneOfLocationAndZoomLevel(new LatLon(pokeName,latitude ), 15.0f));
        mapin.Location = new LatLon(pokeName,latitude);
        // Debug.Log(mapin.Location);
        Debug.Log(pokeName);
        Debug.Log(latitude);

    }

    public void AnimateToPlace(string location)
    {

    }
}
