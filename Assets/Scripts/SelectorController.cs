using Interactuar;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

/// <summary>
/// Detecta la interaccion del jugador con los objetos de la escena
/// </summary>
public class SelectorController : MonoBehaviour
{
    [Header("Deteccion")]
    [Tooltip("Nombre del layer de objetos de tipo seleccionables")] public LayerMask layer;
    [Tooltip("Distancia maxima entre camara y objeto")] public float distanciaMax = 1f;
    [Tooltip("Padre de los objetos seleccionados")] public Transform target;

    [Header("Animacion")]
    [Tooltip("Controlador de animaciones de la mano")] public Animator animRig;
    [Tooltip("Manipulador del esqueleto")] public Rig rig;
    
    private Transform parent; //Padre-Escena
    private ObjetoInteractivo temp; //Almacena el posible objeto a interactuar
    private UIController uIController; //Controlador de la interfaz
    private ObjetoInteractivo activo; //Objeto actual seleccionado
    private ObjetoInteractivo activar; //Objeto con el que se va a interactuar
    float tiempoAccion; //Tiempo de espera para ejecutar una interaccion entre 2 objetos
    string txtAccion; //Accion posible entre 2 objetos
    bool soltar = false;
    //Posicion segun el tamaño de la olla
    private readonly Vector3[] posOllas = new Vector3[4] { new Vector3(-0.000446f, 0.000383f, -0.0007f), new Vector3(0f, 0f, 0f), new Vector3(-0.00049f, -3e-05f, 0.0003f), new Vector3(-0.00036f, -0.00044f, 0.00047f) }; 
    
    //Padres de objetos a sujetar
    private Transform ParentCubierto { get => target.GetChild(0).transform; }
    private Transform ParentCondimento { get => target.GetChild(1).transform; }
    private Transform ParentOlla { get => target.GetChild(2).transform; }
    private Transform ParentPan{ get => target.GetChild(3).transform; }
    private Transform ParentPlato { get => target.GetChild(4).transform; }
    private Transform ParentHuevo { get => target.GetChild(5).transform; }
    private Transform ParentTapa { get => target.GetChild(6).transform; }
    private float Rango { get => rig.weight; set => rig.weight = value; }
    private void Awake()
    {
        parent = transform.parent;
        uIController = FindObjectOfType<UIController>();
    }
    void FixedUpdate()
    {
        //Raycast
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        #region Down
        if(!activo)
        if (Physics.Raycast(ray, out RaycastHit hit, distanciaMax, layer))
        {
            //Asigna temporalmente el posible objeto activo y muestra su informacion
            if (temp != hit.transform.GetComponent<ObjetoInteractivo>())
            {
                if (temp)
                    temp.enabled = false;

                temp = hit.transform.GetComponent<ObjetoInteractivo>();
                temp.enabled = true;

                uIController.ObjetoViendo(hit.transform.name);
            }
            //Inicia la interaccion del jugador con el objeto
            if (tiempoAccion<0.4f)
            tiempoAccion += Time.fixedDeltaTime;
            else
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(nameof(DirigirManoAnim));
                Invoke(nameof(Sujetar),.13f);
                tiempoAccion = 0f;
            }
        }
        else
            LimpiarActivo();
        #endregion

        #region Pressing
        //Mientras haiga un objeto activo habra otro con el cual interactuar
        if (activo)
        {
            if (Input.GetMouseButton(0))
            {
                if (Physics.Raycast(ray, out RaycastHit hit, distanciaMax, layer))
                {
                    if (activar != hit.transform.GetComponent<ObjetoInteractivo>())
                        AsignarActivar(hit.transform);
                }
                else if (activar)
                    LimpiarActivar();
            }
            #endregion
            //Interaccion entre objetos
            if (activar)
            {
                if (activar.Tipo_Objeto.Equals(Tipo_Objeto.Olla))
                {
                    tiempoAccion += Time.fixedDeltaTime;

                    if (Input.GetKey(KeyCode.F))
                    {
                        if (tiempoAccion > .7f)
                        {
                            activar.Interactuar(activo);
                            tiempoAccion = 0f;
                        }
                    }
                }
                else if (Input.GetKey(KeyCode.F))
                    activar.Interactuar(activo);

                if (txtAccion!=null && !activar.enabled)activar.enabled = true;
            }
            #region Up
            //Soltar objeto con el que se esta interactuando
            if (Input.GetMouseButtonUp(0) || soltar)
            {
                if (Physics.Raycast(ray, out RaycastHit hit))
                    SoltarActivo(hit);
                else
                    activo.Soltar(Vector3.zero, parent);

                activo = null;
                soltar = false;
            }
        }
        #endregion
    }
    void LimpiarActivo()
    {
        if (temp)
        {
            if (activo)
            {
                if (activo != temp)
                {
                    temp.enabled = false;
                    temp = null;
                    uIController.ObjetoViendo(null);
                }
            }
            else
            {
                temp.enabled = false;
                temp = null;
                uIController.ObjetoViendo(null);
            }
        }
        if (activar)
        {
            activar.enabled = false;
            activar = null;
            uIController.TipoAccion();
            uIController.ObjetoViendo(null);
        }
    }
    void LimpiarActivar()
    {
        uIController.TipoAccion(null);
        activar.enabled = false;
        activar = null;
        uIController.ObjetoViendo(null);
    }
    void AsignarActivo()
    {
        if(temp)
        {
            switch (temp.Tipo_Interaccion)
            {
                case Tipo_Interaccion.Sostener:
                    activo = temp;
                    Eventos.TipoDeSonido(Tipo_Sonido.Sujetar);
                    uIController.ObjetoViendo(null);
                    switch (activo.Tipo_Objeto)
                    {
                        case Tipo_Objeto.Cuchillo:
                            activo.On(ParentCubierto);
                            break;
                        case Tipo_Objeto.Cucharon:
                            activo.On(ParentCubierto);
                            break;
                        case Tipo_Objeto.Alimento:
                            switch (activo.GetComponent<Alimento>().alimento)
                            {
                                case Tipo_Alimento.CebollaLarga:
                                    activo.On(ParentCubierto);
                                    break;
                                case Tipo_Alimento.Cilantro:
                                    activo.On(ParentCubierto);
                                    break;
                                case Tipo_Alimento.Pan:
                                    activo.On(ParentPan);
                                    break;
                                case Tipo_Alimento.Huevo:
                                    activo.On(ParentHuevo);
                                    break;
                                case Tipo_Alimento.Leche:
                                    activo.On(ParentPlato);
                                    break;
                                default:
                                    activo.On(ParentCubierto);
                                    break;
                            }
                            break;
                        case Tipo_Objeto.Condimento:
                            activo.On(ParentCondimento);
                            break;
                        case Tipo_Objeto.Olla:
                            int nOlla = activo.GetComponent<Olla>().posicion;

                            if (nOlla > 0 && nOlla <= 4)
                                activo.On(ParentOlla, posOllas[nOlla - 1]);
                            Eventos.Olla(null, true);
                            break;
                        case Tipo_Objeto.Tapa:
                            activo.On(ParentTapa);
                            break;
                        case Tipo_Objeto.Recipiente:
                            activo.On(ParentOlla);
                            break;
                        case Tipo_Objeto.Plato:
                            activo.On(ParentPlato);
                            break;
                    }
                    break;
                default:
                    temp.On();
                    break;
            }
        }
    }
    void SoltarActivo(RaycastHit hit)
    {
        bool soltar = true;
        Transform _hit = hit.transform;

        if (!_hit.GetComponent<ExteriorEstanteria>() && !_hit.GetComponent<Olla>() && !_hit.transform.CompareTag("Almacenar"))
            activo.Soltar(NuevaPosicion(hit, distanciaMax), parent);
        else
        {
            if (activo.Tipo_Objeto.Equals(Tipo_Objeto.Tapa) && _hit.GetComponent<Olla>())
            {
                if (_hit.GetComponent<Olla>().EsPadre(activo.transform.GetInstanceID()))
                {
                    _hit.GetComponent<Olla>().SoltarTapa();
                    soltar = false;
                }
            }
            else
            if (soltar && _hit.transform.CompareTag("Almacenar"))
            {
                activo.Soltar(NuevaPosicion(hit, distanciaMax), _hit);
                soltar = false;
            }
            else
            if (soltar && _hit.GetComponent<ExteriorEstanteria>())
            {
                if (_hit.GetComponent<ExteriorEstanteria>().ComprobarParentesco(activo.transform.GetInstanceID()))
                {
                    _hit.GetComponent<ExteriorEstanteria>().Parentar(activo.transform);
                    soltar = false;
                }
                else
                    activo.Soltar(NuevaPosicion(hit, distanciaMax), parent);
            }
            else if (soltar)
                activo.Soltar(NuevaPosicion(hit, distanciaMax), parent);
        }
        //Asignar holla
        if (activo.Tipo_Objeto.Equals(Tipo_Objeto.Olla))
        {
            if (_hit.CompareTag("Agua") && _hit.GetComponent<Agua>())
            {
                Eventos.Olla(activo.GetComponent<Olla>(),activo.GetComponent<Olla>().TieneTapa());
                activo.GetComponent<Olla>().liquido.gameObject.SetActive(true);
            }
            else if (_hit.CompareTag("Quemador") && _hit.GetComponent<Fuego>())
            _hit.GetComponent<Fuego>().Info(activo.GetComponent<Olla>(),activo.GetComponent<Olla>().TieneTapa());
        }
        uIController.TipoAccion();
    }
    void AsignarActivar(Transform hit)
    {
        uIController.ObjetoViendo(hit.transform.name);
        activar = hit.GetComponent<ObjetoInteractivo>();

        if (activar.Tipo_Objeto.Equals(Tipo_Objeto.Alimento))
            txtAccion = Objeto.Accion(activo.Tipo_Objeto, activar.Tipo_Objeto, activar.GetComponent<Alimento>().alimento);
        else
        if (activo.Tipo_Objeto.Equals(Tipo_Objeto.Alimento))
            txtAccion = Objeto.Accion(activo.Tipo_Objeto, activar.Tipo_Objeto, activo.GetComponent<Alimento>().alimento);
        else
            txtAccion = Objeto.Accion(activo.Tipo_Objeto, activar.Tipo_Objeto);

        if (txtAccion != null)
        {
            uIController.TipoAccion(txtAccion);
            uIController.ObjetoViendo(null);
        }
    }
    Vector3 NuevaPosicion(RaycastHit hit, float distanciaMax)
    {
        Vector3 pos = hit.point;

        if (hit.distance > distanciaMax)
            pos = Vector3.zero;

        return pos;
    }
    void Sujetar()
    {
        AsignarActivo();
        StartCoroutine(nameof(RegresarManoAnim));
        if (!Input.GetMouseButton(0))
            soltar = true;
    }
    IEnumerator DirigirManoAnim()
    {
        while (Rango < 0.99)
        {
            Rango += 0.1f;

            if(Rango>.9f)
            animRig.SetTrigger("Tomar");
            yield return null;
        }
    }
    IEnumerator RegresarManoAnim()
    {
        StopCoroutine(nameof(DirigirManoAnim));
        while (Rango > 0.02)
        {
            Rango -= 0.1f;
            yield return null;
        }
    }
}