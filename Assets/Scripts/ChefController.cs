using UnityEngine;
using Interactuar;
using System.Collections;

public class ChefController : MonoBehaviour
{
    UIController uIController;
    //ADVERTENCIA
    private readonly string[] advertenciasTapa = new string[3] { "Quizas deberias retirar la tapa primero...", "Te falto un paso importante, retirar la tapa", "Tienes un obstaculo de por medio" };
    private readonly string[] advertenciasIncompleto = new string[3] { "No te apresures, aun te falta algun paso ", "Estaria bien cocinar primero...", "Me gusta tu motivación pero tomalo con calma" };
    private readonly string[] advertenciasPicar = new string[2] { "Picala primero con un cuchillo", "Asi no es como se deberia hacerce" };

    //AVANCE
    private readonly string[] avanceCebolla = new string[3] { "La cebolla le aporta a aroma particularmente exquicito al plato", "La cebolla es perfecta para agregar al caldo de la changua", "La cebolla te dara un buen color a tu changua" };
    private readonly string[] avanceLeche = new string[2] { "La leche le dara su sabor dulce tan caracteristico", "Probablemente la leche sea el ingrediente mas importante en nuestra receta de hoy" };
    private readonly string[] avanceCilantro = new string[3] { "El sabor del cilantro es ligeramente picante", "El cilantro tiene un sabor mucho más fuerte que el perejil", "El cilantro a diferencia del perejil, tiene un aroma mas penetrante y muy característico."};
    private readonly string[] avanceHuevo = new string[2] { "Dato Curioso: Para saber si un huevo está crudo o cocido, dale vueltas; si gira fácilmente está duro, si se tambalea, está crudo", "Sabias que la changua es tambien llamada caldo de huevo?" };
    private readonly string[] avanceSal = new string[2] { "Sin duda alguna el aporte de sal ayudara a mejorar el sabor de la changua","Ten cuidado de no pasarte con la cantidad de sal" };
    private readonly string[] avancePan = new string[2] { "El agregar tostadas o pan al plato es propio de la región de Santader", "Puedes combinar el pan/tostada con 1/2 cucharada de mantequilla y 1 porcion de queso mozarella por persona en la proxima preparacion de este plato" };

    //MISION
    private readonly string[] misiones = new string[3] { "¡Muy bien!, haz completado la primera etapa en la preparacion del plato, ¡sigue asi!", "¡Que bien huele! No te falta mucho para saber que su sabor es aun mejor, ¡sigue asi!", "¡Un paso mas!, estoy feliz de tu progreso, ahora solo te queda mi parte favorita como Chef, servir el plato" };
    int mision = 0;
    bool mostrardialogoInicial = true;

    //DIALOGO INICIAL
    private readonly string[] dialogoInicial = new string[3] {
        "Bienvenido, he buscado un aprendiz por mucho tiempo y te he encontrado a ti, asegúrate de escuchar mis consejos para avanzar hasta convertirte en un/a gran Chef.",
        "Empezaremos por preparar un plato tradicional de la región de Santander--Colombia, no te preocupes, te guiare paso a paso hasta completarlo, buena suerte.",
         "Acercate a la TV, te dare las indicaciones."   };

    //PUNTUACION
    //private readonly string[] puntacionAgua = new string[4] { "No agregaste agua, pesima disicion", "No te acercaste a la cantidad de agua indicada", "¡Muy bien! Agregaste una cantidad de agua cerca a la indicada", "¡Perfecto! Acertaste a la cantidad de agua" };
    private readonly string[] dialogoFinal = new string[5] { "Lamentablemente obtuviste un pesimo desempeño, deverias practicar, practicar y seguir practicando, quizas algun dia lo logres... quizas no.", 
        "Tienes mucho trabajo para mejorar, no te conformes con este resultado a no ser que odies comer, toma aire e intentalo de nuevo", 
        "No esta tan mal, seguiste algunos pasos correctamente pero otros no, deberias seguir intentandolo, seguramente puedes hacer mejor", 
        "¡Buen trabajo! Seguiste la mayoria de mis indicaciones e hiciste esta diliciosa Changua santandereana, definitivamente tienes la habilidad de un Chef",
        "¡Extraordinario! Lo hiciste sorprendemente perfecto, me enorgulleces pequeño aprendiz al superar todas mis expectativas, eres un Chef en cuerpo y alma" };

    //AGREGO INGREDIENTE
    bool agregoLeche = false;
    bool agregoCebolla = false;
    bool agregoCilantro = false;
    bool agregoHuevo = false;
    bool agregoSal = false;
    bool agregoPan = false;
    void Awake()
    {
        if (GetComponent<UIController>())
            uIController = GetComponent<UIController>();
        else uIController = FindObjectOfType<UIController>();

        Eventos.Mensaje += Mensaje;
        //StartCoroutine(nameof(DialogoInicial));
    }
    public void MostrarDialogoInicial()
    {
        if (mostrardialogoInicial)
        {
            StartCoroutine(nameof(DialogoInicial));
            mostrardialogoInicial = false;
        }
    }
    public void MostrarDialogoFinal(int[] puntuacion)
    {
        int p_total = puntuacion[puntuacion.Length - 1];
        if (p_total <= 1750)
        {
            uIController.MostrarDialogo(dialogoFinal[0], true);
        }
        else
        if (p_total > 1750 && p_total <= 3500)
        {
            uIController.MostrarDialogo(dialogoFinal[1], true);
        }
        else
        if (p_total > 3500 && p_total <= 5250)
        {
            uIController.MostrarDialogo(dialogoFinal[2], true);
        }
        else
        if (p_total > 5250 && p_total <= 7000)
        {
            uIController.MostrarDialogo(dialogoFinal[3], true);
        }
        else
            if (p_total == 7000)
        {
            uIController.MostrarDialogo(dialogoFinal[4], true);
        }
        else
            Debug.LogWarning("Puntaje final fuera del rango de midicion: " + p_total);
        //string[] dialogoFinal = new string[7];
        //
        //if (puntuacion[0] == 0)
        //    dialogoFinal[0] = puntacionAgua[0];
        //
        //else if(puntuacion[0]>0 && puntuacion[0]<500)
        //    dialogoFinal[0] = puntacionAgua[1];
        //
        //else if (puntuacion[0] >= 500 && puntuacion[0] < 1000)
        //    dialogoFinal[0] = puntacionAgua[2];
        //
        //else if (puntuacion[0] ==1000)
        //    dialogoFinal[0] = puntacionAgua[3];
    }
    IEnumerator DialogoInicial()
    {
        bool finalizar=false;

        for (int d = 0; d < dialogoInicial.Length; d++)
        {
            float delay = dialogoInicial[d].Length * 0.05f;

            if (d == dialogoInicial.Length - 1)
                finalizar = true;

            uIController.MostrarDialogo(dialogoInicial[d],finalizar);
            yield return new WaitForSeconds(delay);

            if (finalizar)
                uIController.LimpiarInfoDialogo();
        }
    }
    private void Mensaje(Tipo_Mensaje mensaje)
    {
        switch (mensaje)
        {
            case Tipo_Mensaje.Tapa:
                uIController.MostrarAdvertencia(advertenciasTapa[Random.Range(0, advertenciasTapa.Length)]);
                break;

            case Tipo_Mensaje.Picar:
                uIController.MostrarAdvertencia(advertenciasPicar[Random.Range(0, advertenciasPicar.Length)]);
                break;

            case Tipo_Mensaje.Incompleto:
                uIController.MostrarAdvertencia(advertenciasIncompleto[Random.Range(0, advertenciasIncompleto.Length)]);
                break;
            case Tipo_Mensaje.AgregarLeche:
                if (!agregoLeche) 
                {
                    uIController.MostrarDialogo( avanceLeche[Random.Range(0, avanceLeche.Length)],true);
                    agregoLeche =true;
                }
                break;
            case Tipo_Mensaje.AgregarCebolla:
                if (!agregoCebolla) 
                {
                    uIController.MostrarDialogo(avanceCebolla[Random.Range(0, avanceCebolla.Length)], true);
                    agregoCebolla =true;
                }
                break;
            case Tipo_Mensaje.AgregarCilantro:
                if (!agregoCilantro) 
                {
                    uIController.MostrarDialogo(avanceCilantro[Random.Range(0, avanceCilantro.Length)], true);
                    agregoCilantro = true;
                }
                break;
            case Tipo_Mensaje.AgregarHuevo:
                if (!agregoHuevo)
                {
                    uIController.MostrarDialogo(avanceHuevo[Random.Range(0, avanceHuevo.Length)], true);
                    agregoHuevo = true;
                }
                break;
            case Tipo_Mensaje.AgregarSal:
                if (!agregoSal)
                {
                    uIController.MostrarDialogo(avanceSal[Random.Range(0, avanceSal.Length)], true);
                    agregoSal = true;
                }
                break;
            case Tipo_Mensaje.AgregarPan:
                if (!agregoPan)
                {
                    uIController.MostrarDialogo(avancePan[Random.Range(0, avancePan.Length)], true);
                    agregoPan = true;
                }
                break;
            case Tipo_Mensaje.Mision:
                uIController.MostrarDialogo(misiones[mision], true);
                mision++;
                break;
        }
        //else Debug.LogWarning("Falta mensaje del Chef");
    }
}
