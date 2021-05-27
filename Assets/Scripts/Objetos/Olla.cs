using Interactuar;
using System.Collections;
using UnityEngine;

public class Olla : MonoBehaviour
{
    public int posicion;
    public Transform tapa;
    public Transform liquido;
    public Transform cebolla;
    public Transform cilantro;
    public Transform[] huevo;
    public Transform[] pan;
    [Tooltip("Liquido(Agua - Leche)")] public float altLiquido;
    float cantAgua;
    float cantLeche;
    float hervir;
    int sal;
    int n_huevo;
    int n_pan;

    int mision = 0;


    Vector3 posTapa;
    Quaternion rotTapa;

    private bool conTapa = true;
    public int NivelAgua { get => (int)(cantAgua * 100 / altLiquido); }
    public int NivelLeche { get => (int)((cantLeche * 100) / altLiquido); set => NivelLeche = value; }
    public int NumeroDePanes { get => n_pan; }
    public int NumeroDeHuevos { get => n_huevo; }
    public int CucharadasDeSal { get => sal; }
    public float Temperatura { get => hervir; set => hervir = value; }

    Color colorLeche = Color.white;
    Color ColorMatLeche { set => liquido.GetComponent<Renderer>().material.SetColor("_BaseColor", value); }

    UIController ui;
    void Awake()
    {
        EstadoInicial();
        ui = FindObjectOfType<UIController>();
    }
    /// <summary>
    /// Estado inicial de la olla
    /// </summary>
    void EstadoInicial()
    {
        posTapa = tapa.localPosition;
        rotTapa = tapa.rotation;

        liquido.transform.localPosition = Vector3.zero;

        colorLeche.a = 0f;

        if (!liquido.GetComponent<BoxCollider>()) liquido.gameObject.AddComponent<BoxCollider>();
        liquido.GetComponent<BoxCollider>().isTrigger = true;

        liquido.gameObject.SetActive(false);
        tapa.gameObject.SetActive(true);
        cebolla.gameObject.SetActive(false);
        cilantro.gameObject.SetActive(false);
        foreach (Transform h in huevo) h.gameObject.SetActive(false);
        foreach (Transform p in pan) p.gameObject.SetActive(false);
    }
   public void SoltarTapa()
    {
        tapa.GetComponent<BoxCollider>().enabled = true;
        liquido.GetComponent<BoxCollider>().enabled = false;
        tapa.parent = transform;
        tapa.localPosition = posTapa;
        tapa.rotation = rotTapa;

        conTapa = true;
        Eventos.Olla(GetComponent<Olla>(), true);
    }
    public Transform Liquido()
    {
        return liquido;
    }
    public bool TieneTapa()
    {
        return conTapa;
    }
    public void SinTapa()
    {
        conTapa = false;
        liquido.GetComponent<BoxCollider>().enabled = true;
        Eventos.Olla(GetComponent<Olla>(), false);
    }
    public void Agregar(Tipo_Alimento alimento)
    {
        switch (alimento)
        {
            case Tipo_Alimento.Leche:
                if (!liquido.gameObject.activeSelf)
                    liquido.gameObject.SetActive(true);

                if(NivelLeche + NivelAgua < 100)
                {
                    StartCoroutine(nameof(AgregarLeche));

                    if (colorLeche.a < 0.6f) colorLeche.a += 0.2f;
                    ColorMatLeche = colorLeche;
                }
                break;

            case Tipo_Alimento.CebollaPicada:
                if (!cebolla.gameObject.activeSelf)
                {
                    cebolla.gameObject.SetActive(true);
                    cebolla.transform.localPosition = new Vector3(0f, liquido.transform.localPosition.y, 0f);

                    ui.Completo(alimento);
                    if (NivelAgua > 0 || NivelLeche > 0) Eventos.TipoDeSonido(Tipo_Sonido.AgregarAOlla);
                }
                break;

            case Tipo_Alimento.CilantroPicado:
                cilantro.gameObject.SetActive(true);
                cilantro.transform.localPosition = new Vector3(0f, liquido.transform.localPosition.y, 0f);

                ui.Completo(alimento);
                if (NivelAgua > 0 || NivelLeche > 0) Eventos.TipoDeSonido(Tipo_Sonido.AgregarAOlla);
                break;

            case Tipo_Alimento.Huevo:
                if (n_huevo < huevo.Length)
                {
                    huevo[n_huevo].gameObject.SetActive(true);
                    huevo[n_huevo].localPosition = new Vector3(0f, liquido.transform.localPosition.y, 0f);
                }
                n_huevo++;
                ui.Huevo = n_huevo.ToString();
                break;

            case Tipo_Alimento.Salero:
                StartCoroutine(nameof(AgregarSal));
                break;

            case Tipo_Alimento.Pan:
                if (n_pan < pan.Length)
                {
                    pan[n_pan].gameObject.SetActive(true);
                    pan[n_pan].localPosition = new Vector3(0f, liquido.transform.localPosition.y, 0f);

                    if(NivelAgua>0 || NivelLeche>0) Eventos.TipoDeSonido(Tipo_Sonido.AgregarAOlla);
                }
                n_pan++;
                ui.Pan = n_pan.ToString();
                break;
        }

        ComprobarMisionCompletada();
    }
    public void Hirviendo(float tHervir)
    {
        Temperatura += tHervir;
        ui.Hervir = Temperatura/100f;

        ComprobarMisionCompletada();
    }
    public void Llenar(float alt)
    {
        if (liquido.transform.localPosition.y < altLiquido)
        {
            cantAgua += alt;
            ui.Agua = NivelAgua.ToString();
            ui.NivelAgua = NivelAgua / 100f; 
            AlturaElementos(alt);
        }
    }
    void AlturaElementos(float alt)
    {
        liquido.Translate(0f, alt, 0f, Space.Self);
        if (cebolla) cebolla.Translate(0f, alt, 0f, Space.Self);
        if(cilantro) cilantro.Translate(0f, alt, 0f, Space.Self);
        foreach(Transform h in huevo)h.Translate(0f, alt, 0f, Space.Self);
        foreach (Transform p in pan) p.Translate(0f, alt, 0f, Space.Self);
    }
    public void Servir()
    {
        if (mision == 3)
        {
            ui.CompletarPartida(GetComponent<Olla>());
            Eventos.FinalizarJuego();
        }
        else
            Eventos.Mensaje(Tipo_Mensaje.Incompleto);
    }
    public bool EsPadre(int id)
    {
        if (id == tapa.transform.GetInstanceID())
            return true;
        else return false;
    }
    public void Sostener(bool sostener)
    {
        liquido.GetComponent<BoxCollider>().enabled = !sostener;
    }
    private void OnDestroy()
    {
        GetComponent<ObjetoInteractivo>().Tipo_Objeto = Tipo_Objeto.Recipiente;
        Destroy(liquido.gameObject);
        Destroy(cebolla.gameObject);
        Destroy(cilantro.gameObject);
        for (int i = 0; i < huevo.Length; i++) Destroy(huevo[i].gameObject);
        for (int i = 0; i < pan.Length; i++) Destroy(pan[i].gameObject);
    }
    IEnumerator AgregarLeche()
    {
        float l = 0f;
        float n = (altLiquido*0.1f)/10;
        float v = altLiquido * 0.1f;

        while (l < v)
        {
            l += n;
            cantLeche += n;

            AlturaElementos(n);

            ui.Leche = NivelLeche.ToString();
            ui.NivelLeche = NivelLeche / 100f;

            if (NivelLeche + NivelAgua > 100)
            {
                cantLeche = (NivelLeche - ((NivelLeche + NivelAgua) - 100)) * altLiquido / 100;
                ui.Leche = NivelLeche.ToString();
                ui.NivelLeche = NivelLeche / 100f;
                StopCoroutine(nameof(AgregarLeche));
            }
            yield return null;
        }
    }
    IEnumerator AgregarSal()
    {
        yield return new WaitForSeconds(1.3f);
        sal++;
        ui.Sal = sal.ToString();
    }
    void ComprobarMisionCompletada()
    {
        switch (mision)
        {
            case 0:
                if (hervir > 0 && (NivelAgua > 0 || NivelLeche > 0))
                {
                    Eventos.MisionCompletada(GetComponent<Olla>().GetInstanceID());
                    mision++;
                }
                break;
            case 1:
                if(cebolla.gameObject.activeSelf && cilantro.gameObject.activeSelf && sal>0)
                {
                    Eventos.MisionCompletada(0);
                    mision++;
                }
                break;
            case 2:
                if (huevo[0].gameObject.activeSelf && n_pan>0)
                {
                    Eventos.MisionCompletada(0);
                    mision++;
                }
                break;
        }
    }
}
