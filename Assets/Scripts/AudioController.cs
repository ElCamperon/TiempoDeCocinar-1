using Interactuar;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    [Range(0f,1f)]public float volumenMusica;
    [Range(0f, 1f)]public float volumenEfectos;
    public AudioSource[] audioMusica;
    [SerializeField] private AudioSource[] audiosEf;
    public Transform opciones;

    private AudioClip sujetar, mision, agregar, picar, salero, leche, huevo, musicaFinal;
    private AudioSource audioSource;
    private AudioSource as_Musica;
    private AudioSource apagarAudio;

    float ControladorSonido { get => opciones.GetChild(0).GetComponent<Scrollbar>().value; set => opciones.GetChild(0).GetComponent<Scrollbar>().value = value; }
    float ControladorMusica { get => opciones.GetChild(1).GetComponent<Scrollbar>().value; set => opciones.GetChild(1).GetComponent<Scrollbar>().value = value; }

    void Awake()
    {
        ControladorSonido = volumenEfectos;
        ControladorMusica = volumenMusica;

        string ruta = "Sonidos/Interaccion/audio_";
        sujetar = Resources.Load<AudioClip>(ruta + "Sujetar_Soltar");
        mision = Resources.Load<AudioClip>(ruta + "MisionCompleta");
        agregar = Resources.Load<AudioClip>(ruta + "AgregarAOlla");
        picar= Resources.Load<AudioClip>(ruta + "Picar");
        salero = Resources.Load<AudioClip>(ruta + "Salero");
        leche = Resources.Load<AudioClip>(ruta + "Leche");
        huevo = Resources.Load<AudioClip>(ruta + "Huevo");
        musicaFinal = Resources.Load<AudioClip>("Sonidos/Musica/audio_Musica3");
        audioSource = GetComponent<AudioSource>();

        audiosEf = FindObjectsOfType<AudioSource>();
        CambiarVolumenEf();

        as_Musica = transform.GetChild(0).GetComponent<AudioSource>();

        Eventos.ApagarAudio += EsperarApagarAudio;
        Eventos.TipoDeSonido += GenerarSonido;
        Eventos.Pausar += Pausar;
        Eventos.FinalizarJuego += FinalizarJuego;
    }
    public void CambiarSonido()
    {
        volumenEfectos = ControladorSonido;
        CambiarVolumenEf();
    }
    public void CambiarMusica()
    {
        volumenMusica = ControladorMusica;
        foreach (AudioSource _audio in audioMusica)
            _audio.volume = volumenMusica;
    }
    void Pausar(bool pausa)
    {
        if (pausa)
            audiosEf = FindObjectsOfType<AudioSource>();
    }
    void GenerarSonido(Tipo_Sonido tipo)
    {
        if(audioSource)
        switch (tipo)
        {
            case Tipo_Sonido.Sujetar:
                audioSource.PlayOneShot(sujetar);
                break;
            case Tipo_Sonido.MisionCompleta:
                audioSource.PlayOneShot(mision);
                break;
            case Tipo_Sonido.AgregarAOlla:
                audioSource.PlayOneShot(agregar);
                break;
            case Tipo_Sonido.Picar:
                audioSource.PlayOneShot(picar);
                break;
            case Tipo_Sonido.Salero:
                audioSource.PlayOneShot(salero);
                break;
            case Tipo_Sonido.Leche:
                audioSource.PlayOneShot(leche);
                break;
            case Tipo_Sonido.Huevo:
                audioSource.PlayOneShot(huevo);
                break;
        }
    }
    void CambiarVolumenEf()
    {
        foreach (AudioSource _audio in audiosEf)
            _audio.volume = volumenEfectos;
        foreach (AudioSource _audio in audioMusica)
            _audio.volume = volumenMusica;
    }
    void EsperarApagarAudio(AudioSource audio,float tiempo)
    {
        if (audio)
        {
            apagarAudio = audio;
            Invoke(nameof(ApagarAudio), tiempo);
        }
    }
    void ApagarAudio()
    {
        if (apagarAudio) apagarAudio.enabled = false;
    }
    void FinalizarJuego()
    {
        as_Musica.clip = musicaFinal;
        as_Musica.Play();
    }
}
