using Interactuar;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Transform advertencia;
    public Transform ver;
    public Transform accion;
    public Transform mision;
    public Transform progreso;
    public Transform misionCompletada;
    public Transform tv;
    public Transform calificacion;
    public Transform opciones;
    public Transform indicaciones;
    public Transform dialogo;

    public Image diana;
    public GameObject opcion;
    public GameObject cargar;

    public Toggle[] tg_efectos;
    public TMP_Dropdown d_Controlador;

    //bool comienzo;
    private GameController gameController;

    #region Info

    //Dialogo
    private string Dialogo { get => dialogo.GetChild(1).GetComponent<TMP_Text>().text; set => dialogo.GetChild(1).GetComponent<TMP_Text>().text = value; }
    //Advertencia
    private string Advertencia { get => advertencia.GetChild(1).GetComponent<TMP_Text>().text; set => advertencia.GetChild(1).GetComponent<TMP_Text>().text = value; }

    //Objeto en visualizacion
    private string Viendo { set => ver.GetChild(1).GetComponent<TMP_Text>().text = value; }
    private Sprite IconViendo { set => ver.GetChild(2).GetComponent<Image>().sprite = value; }

    //Accion actual
    private string Accion { get => accion.GetChild(0).GetComponent<TMP_Text>().text; set => accion.GetChild(0).GetComponent<TMP_Text>().text = value; }

    //Mision
    public Sprite IconMision { set => mision.GetChild(1).GetComponent<Image>().sprite = value; }
    private string Numero { set => mision.GetChild(2).GetComponent<TMP_Text>().text = value; }
    private string Mision { set => mision.GetChild(3).GetComponent<TMP_Text>().text = value; }

    //Informacion de olla
    public float NivelAgua { set => progreso.GetChild(5).GetComponent<Image>().fillAmount = value; }
    public string Agua { set => progreso.GetChild(5).GetChild(0).GetComponent<TMP_Text>().text = value + "%"; }
    public float NivelLeche { set => progreso.GetChild(6).GetComponent<Image>().fillAmount = value; }
    public string Leche { set => progreso.GetChild(6).GetChild(0).GetComponent<TMP_Text>().text = value + "%"; }
    private Sprite Cilantro { set => progreso.GetChild(7).GetComponent<Image>().sprite = value; }
    private Sprite Cebolla { set => progreso.GetChild(8).GetComponent<Image>().sprite = value; }
    public string Huevo { set => progreso.GetChild(9).GetComponent<TMP_Text>().text = value; }
    public string Pan { set => progreso.GetChild(10).GetComponent<TMP_Text>().text = value/* + " ud"*/; }
    public string Sal { set => progreso.GetChild(11).GetComponent<TMP_Text>().text = value/* + " cda"*/; }
    public float Hervir { set => progreso.GetChild(12).GetComponent<Image>().fillAmount = value/* + "°C"*/; }

    //TV
    private string Paso { set => tv.GetChild(0).GetComponent<TMP_Text>().text = value; }
    private string Pasos { set => tv.GetChild(1).GetComponent<TMP_Text>().text = value; }

    //Mision completada
    private float Progreso { set => misionCompletada.GetChild(1).GetComponent<Image>().fillAmount = value; }

    //Calificacion
    private string PuntajeAgua { set => calificacion.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = value; }
    private string PuntajeLeche { set => calificacion.GetChild(1).GetChild(1).GetComponent<TMP_Text>().text = value; }
    private string PuntajeCilantro { set => calificacion.GetChild(1).GetChild(2).GetComponent<TMP_Text>().text = value; }
    private string PuntajeCebolla { set => calificacion.GetChild(1).GetChild(3).GetComponent<TMP_Text>().text = value; }
    private string PuntajePan { set => calificacion.GetChild(1).GetChild(4).GetComponent<TMP_Text>().text = value; }
    private string PuntajeHuevo { set => calificacion.GetChild(1).GetChild(5).GetComponent<TMP_Text>().text = value; }
    private string Puntaje { set => calificacion.GetChild(1).GetChild(6).GetComponent<TMP_Text>().text = value; }
    private Sprite IconMisionComplete { set => misionCompletada.GetChild(2).GetComponent<Image>().sprite = value; }
    #endregion

    private readonly Sprite[] iconsMision = new Sprite[4];
    private readonly Sprite[] IconsVer = new Sprite[2];
    private readonly Sprite[] IconsDiana = new Sprite[2];
    private Sprite completo;

    private void Awake()
    {
        if (GetComponent<GameController>())
            gameController = GetComponent<GameController>();
        else
            gameController = FindObjectOfType<GameController>();

        for (int n = 0; n < 4; n++)
            iconsMision[n] = Resources.Load<Sprite>("UI/Iconos/Icon_Mision" + (n + 1).ToString());
        for (int n = 0; n < 2; n++)
            IconsVer[n] = Resources.Load<Sprite>("UI/Iconos/Icon_Ver" + (n + 1).ToString());
        for (int n = 0; n < 2; n++)
            IconsDiana[n] = Resources.Load<Sprite>("UI/Iconos/Icon_Puntero" + (n + 1).ToString());

        completo = Resources.Load<Sprite>("UI/Iconos/Completo");

        TipoAccion();
        LimpiarInfoVer();
        LimpiarInfoOlla();
        LimpiarMision();
        LimpiarInfoAdvertencia();
        LimpiarPuntaje();

        EstadoPanelesInicio();

        Eventos.Pausar += Pausar;
        Eventos.Cargar += Cargar;
        //Eventos.Comienzo += Comienzo;
    }
    //void Comienzo(bool c)
    //{
    //    comienzo = true;
    //}
    void EstadoPanelesInicio()
    {
        DesactivarInterfaz();
        dialogo.gameObject.SetActive(true);
        diana.gameObject.SetActive(true);
    }
    void DesactivarInterfaz()
    {
        diana.gameObject.SetActive(false);
        ver.gameObject.SetActive(false);
        mision.gameObject.SetActive(false);
        progreso.gameObject.SetActive(false);
        opciones.gameObject.SetActive(false);
        opcion.SetActive(false);
        indicaciones.gameObject.SetActive(false);
        //dialogo.gameObject.SetActive(false);
        accion.gameObject.SetActive(false);
        misionCompletada.gameObject.SetActive(false);
    }
    public void Pausar(bool pausar)
    {
        if (!indicaciones.gameObject.activeSelf /*&& !comienzo/*|| !dialogo.gameObject.activeSelf*/)
        {
            if (pausar)
            {
                DesactivarInterfaz();

                if (opciones) opciones.gameObject.SetActive(true);
            }
            else
            {
                diana.gameObject.SetActive(true);
                if (opcion) opcion.SetActive(true);
                if (ver) ver.gameObject.SetActive(true);
                if (mision) mision.gameObject.SetActive(true);
                if (progreso) progreso.gameObject.SetActive(true);

                if (opciones) opciones.gameObject.SetActive(false);
            }
        }
    }
    private void Cargar()
    {
        cargar.gameObject.SetActive(false);
        StartCoroutine(nameof(Carga));
    }
    /// <summary>
    /// Escribir dialogo del Chef
    /// </summary>
    /// Texto-Dialogo
    /// <param name="d"></param>
    /// Verifica si es la ultima cadena de texto
    /// <param name="f"></param>
    public void MostrarDialogo(string d, bool f)
    {
        StopCoroutine(nameof(EscribirDialogo));
        Dialogo = null;

        if(!dialogo.gameObject.activeSelf)
            dialogo.gameObject.SetActive(true);

        StartCoroutine(EscribirDialogo("CHEF: " + d, f));
    }
    IEnumerator EscribirDialogo(string d, bool f)
    {
        foreach (char t in d.ToCharArray())
        {
            Dialogo += t;
            yield return null;
        }
        if (f)
        {
            yield return new WaitForSeconds(d.Length);
            LimpiarInfoDialogo();
        }
    }
    public void MostrarAdvertencia(string a)
    {
        Advertencia = null;
        advertencia.gameObject.SetActive(true);

        StartCoroutine(EscribirAdvertencia(a));
    }
    IEnumerator EscribirAdvertencia(string a)
    {
        foreach (char t in a.ToCharArray())
        {
            Advertencia += t;
            yield return null;
        }
        yield return new WaitForSeconds(4.5f);
        LimpiarInfoAdvertencia();
    }
    public void CambiarMision(int pos, string mis, string paso)
    {
        //Mision
        Numero = NumeroMision(pos);
        Mision = mis;
        IconMision = iconsMision[pos];

        //TV
        Paso = NumeroPosicion(pos);
        Pasos = paso;

        //Mision Completada
        if (pos > 0)
        {
            IconMisionComplete = iconsMision[pos - 1];
            Progreso = 0.25f * pos;
            misionCompletada.gameObject.SetActive(true);

            Invoke(nameof(LimpiarMision), 3f);
            Eventos.TipoDeSonido(Tipo_Sonido.MisionCompleta);
        }
    }
    string NumeroMision(int pos)
    {
        switch (pos)
        {
            case 0:
                return "Primera mision";
            case 1:
                return "Segunda mision";
            case 2:
                return "Tercera mision";
            case 3:
                return "Cuarta mision";
            default:
                return "mision";
        }
    }
    string NumeroPosicion(int pos)
    {
        switch (pos)
        {
            case 0:
                return "Primer paso";
            case 1:
                return "Segundo paso";
            case 2:
                return "Tercer paso";
            case 3:
                return "Cuarto paso";
            default:
                return "Paso";
        }
    }
    public void LimpiarInfoOlla()
    {
        progreso.gameObject.SetActive(true);

        NivelAgua = 0f;
        Agua = "0";
        NivelLeche = 0f;
        Leche = "0";
        Hervir = 0f;
        Huevo = "0";
        Pan = "0";
        Sal = "0";
    }
    public void ObjetoViendo(string objeto)
    {
        if (string.IsNullOrEmpty(objeto))
        {
            IconViendo = IconsVer[1];
            Viendo = null;
        }
        else
        {
            IconViendo = IconsVer[0];
            Viendo = objeto;
        }
    }
    public void LimpiarInfoVer()
    {
        ver.gameObject.SetActive(true);
        IconViendo = IconsVer[1];
        //ver.GetComponent<Image>().enabled = false;
        Viendo = null;
    }
    public bool EstadoAccion()
    {
        if (Accion == null)
            return false;
        else return true;
    }
    public void TipoAccion(string _accion = null)
    {
        if (_accion == null)
        {
            accion.gameObject.SetActive(false);
            diana.sprite = IconsDiana[0];
        }
        else
        {
            Accion = _accion;
            accion.gameObject.SetActive(true);
            diana.sprite = IconsDiana[1];
        }
    }
    public void Completo(Tipo_Alimento alimento)
    {
        switch (alimento)
        {
            case Tipo_Alimento.CilantroPicado:
                Cilantro = completo;
                break;
            case Tipo_Alimento.CebollaPicada:
                Cebolla = completo;
                break;
        }
    }
    public void CompletarPartida(Olla olla)
    {
        int[] resultado = Objeto.EvaluarResultado(olla);

        diana.gameObject.SetActive(false);
        advertencia.gameObject.SetActive(false);
        ver.gameObject.SetActive(false);
        accion.gameObject.SetActive(false);
        mision.gameObject.SetActive(false);
        progreso.gameObject.SetActive(false);
        misionCompletada.gameObject.SetActive(false);
        tv.gameObject.SetActive(false);
        opciones.gameObject.SetActive(false);

        PuntajeAgua = resultado[0].ToString();
        PuntajeLeche = resultado[1].ToString();
        PuntajeCilantro = resultado[2].ToString();
        PuntajeCebolla = resultado[3].ToString();
        PuntajePan = resultado[4].ToString();
        PuntajeHuevo = resultado[5].ToString();
        Puntaje = resultado[6].ToString();

        calificacion.gameObject.SetActive(true);

    }
    void LimpiarMision()
    {
        Progreso = 0f;
        misionCompletada.gameObject.SetActive(false);
        mision.gameObject.SetActive(true);
    }
    public void LimpiarInfoDialogo()
    {
        dialogo.gameObject.SetActive(false);
        Dialogo = null;
    }
    void LimpiarInfoAdvertencia()
    {
        advertencia.gameObject.SetActive(false);
        Advertencia = null;
    }
    void LimpiarPuntaje()
    {
        string p = "0";
        PuntajeAgua = p;
        PuntajeCebolla = p;
        PuntajeCilantro = p;
        PuntajeHuevo = p;
        PuntajeLeche = p;
        PuntajePan = p;
        Puntaje = p;

        calificacion.gameObject.SetActive(false);
    }
    public void RetirarIndicaciones()
    {
        indicaciones.gameObject.SetActive(false);
        opcion.SetActive(true);
        Eventos.Pausar(false);
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }
    IEnumerator Carga()
    {
        accion.gameObject.SetActive(false);
        cargar.gameObject.SetActive(true);
        diana.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);
        if (!Input.GetMouseButtonDown(0)) cargar.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);
        cargar.gameObject.SetActive(false);
    }
    public void LimitarFPS(int var)
    {
        int tf;

        switch (var)
        {
            case 1:
                tf = 30;
                break;
            case 2:
                tf = 60;
                break;
            case 3:
                tf = 80;
                break;
            case 4:
                tf = 100;
                break;
            case 5:
                tf = 120;
                break;
            default:
                tf = -1;
                break;
        }
        Application.targetFrameRate = tf;

        if (tf <= 80 && tf != 0)
        {
            gameController.EstadoSkybox(false);
        }
        else
            gameController.EstadoSkybox(true);
    }
    public void EstadoMejoraVisual(int option)
    {
        switch (option)
        {
            //Default
            case 0:
                for(int v = 0; v < tg_efectos.Length; v++)
                {
                    if (v == 0 || v == 2 || v == 5)
                        tg_efectos[v].isOn = true;
                    else
                        tg_efectos[v].isOn = false;
                }
                break;
                //Activar
            case 1:
                if (tg_efectos.Length > 0 && d_Controlador != null)
                    foreach (Toggle tg in tg_efectos)
                        tg.isOn = true;
                break;
                //Desactivar
            case 2:
                if (tg_efectos.Length > 0 && d_Controlador != null)
                    foreach (Toggle tg in tg_efectos)
                        tg.isOn = false;
                break;
                //Default
            //default:
            //    break;

        }
    }
    public void ComprobarEstadoEfectos(bool estado)
    {
        bool e = EstadoEfectos(estado);

        if (e && estado == true)
            d_Controlador.value = 1;
        else if (e && estado == false)
            d_Controlador.value = 2;
        else d_Controlador.value = 3;

    }
    bool EstadoEfectos(bool estado)
    {
        foreach (Toggle tg in tg_efectos)
        {
            if (tg.isOn!=estado)
                return false;
        }
        return true;
    }
}
