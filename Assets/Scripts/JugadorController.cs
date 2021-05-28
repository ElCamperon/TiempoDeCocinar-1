using UnityEngine;
using Interactuar;
public class JugadorController : MonoBehaviour
{
    private CharacterController controlador;
    Vector3 velocidad;

    Transform BrazoJugador { get => transform.GetChild(0).GetChild(0).transform; }

    private void Awake()
    {
        controlador = GetComponent<CharacterController>();

        Eventos.Pausar += OcultarBrazo;
    }
    void OcultarBrazo(bool estado)
    {
        if (estado)
            BrazoJugador.gameObject.SetActive(false);
        else
            BrazoJugador.gameObject.SetActive(true);
    }
    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controlador.Move(move * 3f * Time.deltaTime);

        velocidad.y += -9.81f * Time.deltaTime;
        controlador.Move(velocidad * Time.deltaTime);
    }
}

