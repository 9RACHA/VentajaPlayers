using UnityEngine;
using UnityEngine.Networking;
using Unity.Netcode;
using Unity.Netcode.Components;
using System.Collections;
using System.Collections.Generic;

public class GameManager : NetworkBehaviour {

    private Color nerfPlayerCol = Color.red;
    private Color ventajaPlayerCol = Color.green;
    private float tiempoVN = 10f; // Duración de la ventaja o desventaja
    private float coroutina = 20f; // Duración total de la coroutine (20 segundos)

    private float nerfPlayerVel = 1f; // Factor de velocidad para la desventaja
    private float ventajaPlayerVel = 2f; // Factor de velocidad para la ventaja

    private List<NetworkClient> players = new List<NetworkClient>(); // Lista de clientes conectados

    private int aleatorio; // Valor aleatorio para determinar si es ventaja o desventaja
    private int playerAleatorio; // Índice aleatorio para seleccionar un jugador de la lista

    void OnGUI(){
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));

        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer){
            StartButtons(); // Muestra los botones para iniciar como host o cliente
        }
        else{
            StatusLabels(); // Muestra información sobre el estado de la conexión
        }

        GUILayout.EndArea();
    }

    void StartButtons(){
        if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost(); // Inicia como host
        StartCoroutine("VentajaNerf"); // Inicia la coroutine para aplicar ventajas y desventajas
        if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient(); // Inicia como cliente
        StartCoroutine("VentajaNerf"); // Inicia la coroutine para aplicar ventajas y desventajas
    }

    static void StatusLabels() {
        var mode = NetworkManager.Singleton.IsHost ?
            "Host" : NetworkManager.Singleton.IsClient ? "Client" : "Client";

        GUILayout.Label("Transport: " +
            NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode: " + mode);
    }

    IEnumerator VentajaNerf() {
        Debug.Log("Esperando Ventaja o Nerf");
        yield return new WaitForSeconds(coroutina); // Espera durante el tiempo total de la coroutine

        players = new List<NetworkClient>();
        foreach (NetworkClient id in NetworkManager.Singleton.ConnectedClientsList){
            players.Add(id); // Obtiene la lista de clientes conectados
        }

        aleatorio = Random.Range(0, 2); // Genera un número aleatorio (0 o 1)
        playerAleatorio = Random.Range(0, players.Count); // Selecciona un índice aleatorio de la lista de jugadores

        Debug.Log("el aleatorio es: " + aleatorio);

        if (aleatorio == 0){
            // Aplica una desventaja al jugador seleccionado de forma aleatoria
            players[playerAleatorio].PlayerObject.GetComponent<VentajaPlayer>().VentajaNerfClientRpc(nerfPlayerVel, tiempoVN, nerfPlayerCol);
            Debug.Log("Estoy nerf");
        }
        else if (aleatorio == 1){
            // Aplica una ventaja al jugador seleccionado de forma aleatoria
            players[playerAleatorio].PlayerObject.GetComponent<VentajaPlayer>().VentajaNerfClientRpc(ventajaPlayerVel, tiempoVN, ventajaPlayerCol);
            Debug.Log("Tengo ventaja");
        }
    }
}

    