using UnityEngine;
using UnityEngine.Networking;
using Unity.Netcode;
using Unity.Netcode.Components;
using System.Collections;

public class VentajaPlayer : NetworkBehaviour {

    public float velocidadMovimiento = 4f; // Velocidad de movimiento del jugador
    public float fuerzaSalto = 10f; // Fuerza de salto del jugador
    private Rigidbody rb; // Referencia al componente Rigidbody adjunto al jugador
    private Renderer r; // Referencia al componente Renderer adjunto al jugador
    
    private float ventajaNerf = 2; // Variable que representa el factor de ventaja o nerf
    private IEnumerator coroutina; // Coroutine utilizada para aplicar el efecto de ventaja o nerf

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Obtiene la referencia al componente Rigidbody
        r = GetComponent<Renderer>(); // Obtiene la referencia al componente Renderer
    }

    [ServerRpc]
    void MueveteServerRpc(Vector3 movimiento, ServerRpcParams serverRpcParams = default){
        transform.position += movimiento; // Método de RPC en el servidor para mover al jugador en el servidor
    }

    [ClientRpc]
    public void VentajaNerfClientRpc(float ventajaNerf, float tiempoVN, Color ventajaNerfColor){
        coroutina = Aplicador(ventajaNerf, tiempoVN, ventajaNerfColor); // Método de RPC en el cliente para aplicar el efecto de ventaja o nerf en todos los clientes
        StartCoroutine(coroutina); // Inicia la corrutina para aplicar el efecto
    }

    private void Update() {
        if (IsOwner) {
            MueveteServerRpc(new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")) * velocidadMovimiento * ventajaNerf * Time.deltaTime);
            // Verifica si el jugador es el propietario (autoridad) y envía un RPC al servidor para mover al jugador según la entrada
        }
    }

    private IEnumerator Aplicador(float ventajaNerf, float tiempoVN, Color ventajaNerfColor) {
        float velocidadInicial = this.ventajaNerf; // Almacena el factor de ventaja o nerf inicial
        this.ventajaNerf = ventajaNerf; // Aplica el nuevo factor de ventaja o nerf
        Color colorInicial = r.material.color; // Almacena el color inicial del jugador
        r.material.color = ventajaNerfColor; // Aplica el nuevo color al jugador
        yield return new WaitForSeconds(tiempoVN); // Espera durante una duración especificada
        this.ventajaNerf = velocidadInicial; // Restaura el factor de ventaja o nerf inicial
        r.material.color = colorInicial; // Restaura el color inicial del jugador
    }
}
