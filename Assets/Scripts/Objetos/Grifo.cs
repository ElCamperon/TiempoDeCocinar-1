using Interactuar;
using UnityEngine;
public class Grifo : MonoBehaviour
{
    public Transform particulaAgua;
    public Agua agua;
    private bool activo = false;

    private AudioClip[] sonidos;
    private AudioSource audioSource;
    private Olla olla;

    private void Awake()
    {
        particulaAgua.gameObject.SetActive(false);

        sonidos = Resources.LoadAll<AudioClip>("Sonidos/Grifo");

        if (!GetComponent<AudioSource>())
            gameObject.AddComponent<AudioSource>();

        audioSource = GetComponent<AudioSource>();
        EstadoAgua(false);

        Eventos.Olla += HollaActiva;
        Eventos.FinalizarJuego += FinalizarJuego;
    }

    public void Abrir()
    {
        if (activo)
        {
            //Agua
            EstadoAgua(false);

            //Audio
            audioSource.Stop();
            audioSource.PlayOneShot(sonidos[1]);

            activo = false;
        }else
        {
            //Agua
            EstadoAgua(true);

            //Audio
            audioSource.Stop();
            audioSource.PlayOneShot(sonidos[0]);
            Invoke(nameof(Play), 0.5f);

            activo = true;
        }
    }
    void EstadoAgua(bool isOn)
    {
        if (agua)
        {
            agua.enabled = isOn;

            if (isOn && olla) agua.Info(olla.liquido,olla.TieneTapa(), olla);
            else agua.Info(null,true, null);
        }

        particulaAgua.gameObject.SetActive(isOn);
    }
    void Play()
    {
        audioSource.clip = sonidos[2];
        audioSource.Play();
    }
    //Holla en grifo
    void HollaActiva(Olla _olla,bool tieneTapa)
    {
        olla = _olla;

        if (agua)
        {
            if (!_olla) agua.Info(null,true, null);
            else if (olla && activo) agua.Info(olla.liquido,tieneTapa, olla);
        }
    }
    void FinalizarJuego()
    {
        audioSource.enabled = false;
    }
}
