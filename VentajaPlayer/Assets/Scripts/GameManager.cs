using UnityEngine;
using UnityEngine.Networking;
using Unity.Netcode;
using Unity.Netcode.Components;
using System.Collections;
using System.Collections.Generic;

public class GameManager : NetworkBehaviour {

    private Color nerfPlayerCol = Color.red; // Color del efecto de nerf para el jugador
    private Color ventajaPlayerCol = Color.green; // Color del efecto de ventaja para el jugador
    private float tiempoVN = 10f; // Duración del buff o nerf
    private float coroutina = 20f; // Tiempo para terminar la corrutina

    private float nerfPlayerVel = 1f; // Velocidad reducida para el jugador con nerf
    private float ventajaPlayerVel = 2f; // Velocidad aumentada para el jugador con ventaja

    private List<NetworkClient> players = new List<NetworkClient>();// Lista de jugadores conectados en la red

    private int aleatorio; // Número aleatorio para determinar si el jugador obtiene un efecto de nerf o ventaja
    private int playerAleatorio; // Índice aleatorio del jugador al que se le aplicará el efecto

    void OnGUI() {
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

    IEnumerator VentajaNerf() {
        Debug.Log("Esperando Ventaja o Nerf");
        yield return new WaitForSeconds(coroutina);

        // Obtiene la lista de jugadores conectados en la red
        players = new List<NetworkClient>();
        foreach (NetworkClient id in NetworkManager.Singleton.ConnectedClientsList)
        {
            players.Add(id);
        }
        // Genera un número aleatorio para determinar si el jugador obtiene un efecto de nerf o ventaja
        aleatorio = Random.Range(0, 2);
        // Obtiene un índice aleatorio para seleccionar un jugador al que se le aplicará el efecto
        playerAleatorio = Random.Range(0, players.Count);
        Debug.Log("El aleatorio es: " + aleatorio);

        // Aplica el efecto de nerf al jugador seleccionado
        if (aleatorio == 0)
        {
            players[playerAleatorio].PlayerObject.GetComponent<VentajaPlayer>().VentajaNerfClientRpc(nerfPlayerVel, tiempoVN, nerfPlayerCol);
            Debug.Log("Estoy nerf");
        }
        // Aplica el efecto de ventaja al jugador seleccionado
        else if (aleatorio == 1)
        {
            players[playerAleatorio].PlayerObject.GetComponent<VentajaPlayer>().VentajaNerfClientRpc(ventajaPlayerVel, tiempoVN, ventajaPlayerCol);
            Debug.Log("Tengo ventaja");
        }
    }
}
