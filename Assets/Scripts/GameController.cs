using UnityEngine;
using UnityEngine.SceneManagement;
using Interactuar;
using System.Collections;
public class GameController : MonoBehaviour
{
    RotacionDeCamara camara;
    JugadorController jugador;
    SelectorController selector;
    Material skybox;
    bool estadoSkybox=false;
    
    private void Awake()
    {
        camara = FindObjectOfType<RotacionDeCamara>();
        jugador = FindObjectOfType<JugadorController>();
        selector = FindObjectOfType<SelectorController>();
        skybox = Resources.Load<Material>("Materiales/FS002_Day_Sunless");

        Pausa(true);

        Eventos.Pausar += Pausa;
        Eventos.FinalizarJuego += JuegoTerminado;
    }
    public void ContinuarJuego()
    {
        Eventos.Pausar(false);
    }
    void JuegoTerminado()
    {
        if (camara) camara.enabled = false;
        if (jugador) jugador.enabled = false;
        if (selector) selector.enabled = false;
    }
    void Pausa(bool pausar)
    {
        //if (camara) camara.enabled = !pausar;
        //if (jugador) jugador.enabled = !pausar;
        //if (selector) selector.enabled = !pausar;
        if (pausar)
        {
            Cursor.lockState = CursorLockMode.None;
            jugador.enabled = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            jugador.enabled = true;
        }

        Cursor.visible = pausar;
    }
    public void Salir()
    {
        Application.Quit();
    }
    public void Reiniciar(int idEscena)
    {
        //Time.timeScale = 1f;

        //if (camara) camara.enabled = true;
        //if (jugador) jugador.enabled = true;
        //if (selector) selector.enabled = true;

        StartCoroutine(nameof(RecargarEscena));
    }
    /// <summary>
    /// Reiniciar escena sin tirones
    /// </summary>
    /// <returns></returns>
    IEnumerator RecargarEscena()
    {
        //int nivel;
        //
        //if (SceneManager.GetActiveScene().buildIndex == 0)
        //    nivel = 1;
        //else nivel = 0;

        AsyncOperation cargar = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        while (cargar.isDone)
            yield return null;
    }
    public void EstadoSkybox(bool estado)
    {
        if (estado && !estadoSkybox)
            StartCoroutine(nameof(GirarSkybox));
        else if(!estado && estadoSkybox)
            StopCoroutine(nameof(GirarSkybox));

        estadoSkybox = estado;
        print(estadoSkybox);
    }
    IEnumerator GirarSkybox()
    {
        float var = skybox.GetFloat("_Rotation");
        if (var >= 359) var = 0f;

        while (var < 360)
        {
            yield return null;
            var+=0.1f;
            skybox.SetFloat("_Rotation", var);

            if (skybox.GetFloat("_Rotation") >= 359)
                var = 0f;
        }
    }
}
