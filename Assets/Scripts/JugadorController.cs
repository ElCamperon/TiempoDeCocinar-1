using UnityEngine;
public class JugadorController : MonoBehaviour
{
    private CharacterController controlador;
    Vector3 velocidad;
    private void Start()
    {
        controlador = GetComponent<CharacterController>();
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
