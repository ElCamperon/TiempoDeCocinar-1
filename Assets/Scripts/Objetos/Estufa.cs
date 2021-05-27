using UnityEngine;
using Interactuar;
public class Estufa : MonoBehaviour
{
    public Transform quemador;
    Transform particula;

    Outline lineQuemador;
    private bool encendido=false;

    private AudioClip[] sonidos;
    private AudioSource audioSource;
    private Fuego fuego;
    private void Awake()
    {
        //Outline
        if(!quemador.GetComponent<Outline>())quemador.gameObject.AddComponent<Outline>();
        lineQuemador = quemador.gameObject.GetComponent<Outline>();

        if (!quemador.GetComponent<Fuego>()) quemador.gameObject.AddComponent<Fuego>();
        fuego = quemador.GetComponent<Fuego>();
        fuego.enabled = false;

        //Outline de Quemador
        lineQuemador.enabled = true;
        lineQuemador.enabled = false;

        //Particula
        particula = quemador.GetChild(0);
        particula.gameObject.SetActive(false);
        //particula.gameObject.SetActive(false);

        //Sonido
        sonidos = Resources.LoadAll<AudioClip>("Sonidos/Estufa");

        if (!GetComponent<AudioSource>())
            gameObject.AddComponent<AudioSource>();

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = sonidos[0];

        Eventos.FinalizarJuego += FinalizarJuego;
    }
    public void Encender()
    {
        if (encendido)
        {
            //Particula
            particula.gameObject.SetActive(false);
            if (fuego) fuego.enabled = false;

            //Audio
            audioSource.PlayOneShot(sonidos[1]);
            Invoke(nameof(Stop), 0.5f);
            
            encendido = false;
        }
        else
        {
            //Particula
            if (fuego) fuego.enabled = true;

            //Audio
            audioSource.enabled = true;
            audioSource.PlayOneShot(sonidos[1]);
            Invoke(nameof(Play),0.5f);

            encendido = true;
        }
    }
    void Play()
    {
        audioSource.clip = sonidos[0];
        audioSource.Play();
    }
    void Stop()
    {
        audioSource.Stop();
    }
    void FinalizarJuego()
    {
        audioSource.enabled = false;
    }
    private void OnEnable()
    {
        lineQuemador.enabled = true;
    }
    private void OnDisable()
    {
        lineQuemador.enabled = false;
        if(!encendido) audioSource.enabled =false;
    }
}
