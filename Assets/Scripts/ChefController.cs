using UnityEngine;
using Interactuar;
using System.Collections;

public class ChefController : MonoBehaviour
{
    UIController uIController;
    private readonly string[] advertenciasTapa = new string[3] { "Quizas deberias retirar la tapa primero...", "Te falto un paso importante, retirar la tapa", "Tienes un obstaculo de por medio" };
    private readonly string[] advertenciasIncompleto = new string[3] { "No te apresures, aun te falta algun paso ", "Estaria bien cocinar primero...", "Me gusta tu motivación pero tomalo con calma" };
    private readonly string[] advertenciasPicar = new string[2] { "Picala primero con un cuchillo", "Asi no es como se deberia hacerce" };

    private readonly string[] dialogoInicial = new string[2] {
        "Bienvenido, he buscado un aprendiz por mucho tiempo y te he encontrado a ti, asegúrate de escuchar mis consejos para avanzar hasta convertirte en un/a gran Chef.",
        "Empezaremos por preparar un plato tradicional de la región de Santander--Colombia, no te preocupes, te guiare paso a paso hasta completarlo, buena suerte mi estudiante." };

    void Awake()
    {
        if (GetComponent<UIController>())
            uIController = GetComponent<UIController>();
        else uIController = FindObjectOfType<UIController>();

        Eventos.Mensaje += Mensaje;

        StartCoroutine(nameof(DialogoInicial));
        //Eventos.Comienzo += Dialogo;
        //Eventos.Comienzo(true);
    }
    //public void Siguiente()
    //{
    //    StopCoroutine(nameof(dialogoInicial));
    //    uIController.LimpiarInfoDialogo();
    //    uIController.indicaciones.gameObject.SetActive(true);
    //}
    //private void Dialogo(bool c=true)
    //{
    //    StartCoroutine(DialogoInicial());
    //}
    IEnumerator DialogoInicial()
    {
        bool finalizar=false;

        for (int d = 0; d < dialogoInicial.Length; d++)
        {
            float delay = dialogoInicial[d].Length * 0.06f;

            if (d == dialogoInicial.Length - 1)
                finalizar = true;

            uIController.MostrarDialogo(dialogoInicial[d],finalizar);
            yield return new WaitForSeconds(delay);

            if (finalizar)
            {
                uIController.LimpiarInfoDialogo();
                //Eventos.Pausar(true); Eventos.Comienzo(false);
                uIController.indicaciones.gameObject.SetActive(true);


                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
    private void Mensaje(Tipo_Mensaje ta)
    {
        string tipo=null;

        switch (ta)
        {
            case Tipo_Mensaje.Tapa:
                tipo = advertenciasTapa[Random.Range(0, advertenciasTapa.Length)];
                break;
            case Tipo_Mensaje.Picar:
                tipo = advertenciasPicar[Random.Range(0, advertenciasPicar.Length)];
                break;
            case Tipo_Mensaje.Incompleto:
                tipo = advertenciasIncompleto[Random.Range(0, advertenciasIncompleto.Length)];
                break;
        }
        if (!string.IsNullOrEmpty(tipo))
            uIController.MostrarAdvertencia("Chef: " + tipo);
        else Debug.LogWarning("Falta mensaje del Chef");
    }
}
