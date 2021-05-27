using UnityEngine;
using Interactuar;
public class RotacionDeCamara : MonoBehaviour
{
    [SerializeField]private float sensiblidad = 120f;

    float xRotacion = 0f;
    public Transform jugador;

    bool enPausa = false;
    private void Start()
    {
        Eventos.FinalizarJuego += JuegoTerminado;
        Eventos.Pausar += Pausar;
    }
    void JuegoTerminado()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
            Eventos.Pausar(!enPausa);

        if (!enPausa)
        {
            float x = Input.GetAxis("Mouse X") * sensiblidad * Time.deltaTime;
            float y = Input.GetAxis("Mouse Y") * sensiblidad * Time.deltaTime;

            xRotacion -= y;
            xRotacion = Mathf.Clamp(xRotacion, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotacion, 0f, 0f);
            jugador.Rotate(Vector3.up * x);
        }
    }
    void Pausar(bool p)
    {
        enPausa = p;
    }
}
