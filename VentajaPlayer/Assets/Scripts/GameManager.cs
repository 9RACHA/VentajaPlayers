using UnityEngine;
using UnityEngine.Networking;
using Unity.Netcode;
using Unity.Netcode.Components;
using System.Collections;
using System.Collections.Generic;

public class GameManager : NetworkBehaviour {

    private Color nerfPlayerCol = Color.red;
    private Color ventajaPlayerCol = Color.green;
    private float tiempoVN = 10f; //Lo que dura el buff o nerf
    private float coroutina = 20f; //Al pasar 20s acaba la coroutina

    private float nerfPlayerVel = 1f;
    private float ventajaPlayerVel = 2f;

    private List<NetworkClient> players = new List<NetworkClient>();

    private int aleatorio;
    private int playerAleatorio;

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));

        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            StartButtons();
        }
        else
        {
            StatusLabels();
        }

        GUILayout.EndArea();
    }

     void StartButtons()
    {
        if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
        StartCoroutine("VentajaNerf");
        if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
        StartCoroutine("VentajaNerf");
    }

    static void StatusLabels()
    {
        var mode = NetworkManager.Singleton.IsHost ?
            "Host" : NetworkManager.Singleton.IsClient ? "Client" : "Client";

        GUILayout.Label("Transport: " +
            NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode: " + mode);
    }

    IEnumerator VentajaNerf(){
        Debug.Log("Esperando Ventaja o Nerf");
        yield return new WaitForSeconds(coroutina);
        players = new List<NetworkClient>();
        foreach (NetworkClient id in NetworkManager.Singleton.ConnectedClientsList)
        {
            players.Add(id);
        }
        aleatorio = Random.Range(0, 2);
        playerAleatorio = Random.Range(0, players.Count);
        Debug.Log("el aleatorio es: " + aleatorio);
        if (aleatorio == 0)
        {
            players[playerAleatorio].PlayerObject.GetComponent<VentajaPlayer>().VentajaNerfClientRpc(nerfPlayerVel, tiempoVN, nerfPlayerCol);
            Debug.Log("Estoy nerf");
        }
        else if (aleatorio == 1)
        {
            players[playerAleatorio].PlayerObject.GetComponent<VentajaPlayer>().VentajaNerfClientRpc(ventajaPlayerVel, tiempoVN, ventajaPlayerCol);
            Debug.Log("Tengo ventaja");
        }
    }
}

    