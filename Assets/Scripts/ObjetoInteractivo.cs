using UnityEngine;
using Interactuar;
public class ObjetoInteractivo : MonoBehaviour
{
    #region Variable Principal
    [SerializeField] Tipo_Objeto _objeto = Tipo_Objeto.Null;
    public Tipo_Objeto Tipo_Objeto { get => _objeto; set => _objeto = value; }
    #endregion
    #region Variables Secundarias
    Tipo_Interaccion _tipo;
    Quaternion _rotObj;
    public Tipo_Interaccion Tipo_Interaccion { get { return _tipo; } set { _tipo = value; } }
    public Quaternion Rotacion { get { return _rotObj; } set { _rotObj = value; } }
    #endregion
    void Awake()
    {
        //Asignacion de valores
        if (!Tipo_Objeto.Equals(Tipo_Objeto.Null))
        {
            Objeto.Asignar(GetComponent<ObjetoInteractivo>());

            if (Tipo_Interaccion.Equals(Tipo_Interaccion.Sostener))
            {
                if(!Tipo_Objeto.Equals(Tipo_Objeto.Cucharon))Rig(1f);
                Rotacion = transform.rotation;
                
                if (Tipo_Objeto.Equals(Tipo_Objeto.Tapa))
                    gameObject.AddComponent<Outline>();
            }
        }
        GetComponent<ObjetoInteractivo>().enabled = false;
    }
    /// <summary>
    /// Si se selecciona el objeto de tipo ROTAR, usar metodo On
    /// </summary>
    public void On(Transform mano = null, Vector3 pos=default)
    {
        ActivarScriptTipoObjeto();

        switch (Tipo_Interaccion)
        {
            case Tipo_Interaccion.Rotar:
                if (Tipo_Objeto.Equals(Tipo_Objeto.Microhondas) || Tipo_Objeto.Equals(Tipo_Objeto.Estanteria) || Tipo_Objeto.Equals(Tipo_Objeto.Nevera))
                {
                    GetComponent<Puerta>().Rotar();
                    break;
                }
                if(Tipo_Objeto.Equals(Tipo_Objeto.Grifo))
                {
                    GetComponent<Grifo>().Abrir();
                }
                break;

            case Tipo_Interaccion.Sostener:
                Sostener(mano,pos);
                break;
            case Tipo_Interaccion.Desplazar:
                GetComponent<Cajon>().Desplazar();
                break;
            case Tipo_Interaccion.Encender:
                GetComponent<Estufa>().Encender();
                break;
        }
    }

    public void Soltar(Vector3 pos, Transform parent)
    {
        transform.SetParent(parent);

        if(!Tipo_Objeto.Equals(Tipo_Objeto.Cucharon))
        transform.rotation = Rotacion;

        if (pos == Vector3.zero)
            pos = transform.position;

        transform.position = pos + new Vector3(0f, 0.1f, 0f);

        GetComponent<BoxCollider>().enabled = true;

        if (Tipo_Objeto.Equals(Tipo_Objeto.Equals(Tipo_Objeto.Olla)))
            GetComponent<Olla>().Sostener(false);
        Rig();
    }
    void Rig(float timeDestroy=5f)
    {
        if (!GetComponent<Rigidbody>())
            gameObject.AddComponent<Rigidbody>();
        GetComponent<Rigidbody>().mass = 5f;

        Invoke(nameof(DestruirRig),timeDestroy);
    }
    private void DestruirRig()
    {
        //if(GetComponent<Rigidbody>())
        Destroy(GetComponent<Rigidbody>());
    }
    private void Sostener(Transform mano, Vector3 pos = default)
    {
        if (Tipo_Objeto.Equals(Tipo_Objeto.Tapa) && GetComponentInParent<Olla>())
            GetComponentInParent<Olla>().SinTapa();

        if (GetComponent<Rigidbody>()) DestruirRig();//Destroy(GetComponent<Rigidbody>());
        GetComponent<BoxCollider>().enabled = false;
        transform.SetParent(mano);

        if (!Tipo_Objeto.Equals(Tipo_Objeto.Olla)) transform.localPosition = Vector3.zero;
        else transform.localPosition = pos;

        transform.localRotation = Quaternion.identity;
    }
    public void Interactuar(ObjetoInteractivo activo = null)
    {
        //Activo
        if (activo.Tipo_Objeto.Equals(Tipo_Objeto.Alimento) || activo.Tipo_Objeto.Equals(Tipo_Objeto.Condimento))
            activo.GetComponent<Alimento>().Activo(GetComponent<ObjetoInteractivo>());

        //Activar
        else if (Tipo_Objeto.Equals(Tipo_Objeto.Alimento))
            GetComponent<Alimento>().Activar(activo);

        else if (Tipo_Objeto.Equals(Tipo_Objeto.Olla) && activo.Tipo_Objeto.Equals(Tipo_Objeto.Plato))
            GetComponent<Olla>().Servir();


    }
    private void OnValidate()
    {
        switch (Tipo_Objeto)
        {
            case Tipo_Objeto.Null:
                break;
            case Tipo_Objeto.Nevera:
                if (!GetComponent<Puerta>()) gameObject.AddComponent<Puerta>();
                break;
            case Tipo_Objeto.Microhondas:
                if (!GetComponent<Puerta>()) gameObject.AddComponent<Puerta>();
                break;
            case Tipo_Objeto.Estanteria:
                if (!GetComponent<Puerta>()) gameObject.AddComponent<Puerta>();
                //else GetComponent<Puerta>().TransformPuerta = transform;
                break;
            case Tipo_Objeto.Cajon:
                if (!GetComponent<Cajon>()) gameObject.AddComponent<Cajon>();
                break;
            case Tipo_Objeto.Estufa:
                if (!GetComponent<Estufa>()) gameObject.AddComponent<Estufa>();
                break;
            case Tipo_Objeto.Alimento:
                if (!GetComponent<Alimento>()) gameObject.AddComponent<Alimento>();
                break;
            case Tipo_Objeto.Condimento:
                if (!GetComponent<Alimento>()) gameObject.AddComponent<Alimento>();
                break;
        }
    }
    private void OnEnable()
    {
        if (Tipo_Objeto.Equals(Tipo_Objeto.Estufa))
            GetComponent<Estufa>().enabled = true;
        else if (Tipo_Objeto.Equals(Tipo_Objeto.Alimento))
            GetComponent<Alimento>().enabled = true;
        else if(Tipo_Objeto.Equals(Tipo_Objeto.Tapa))
            GetComponent<Outline>().enabled = true;
    }
    private void ActivarScriptTipoObjeto()
    {
        switch (Tipo_Objeto)
        {
            case Tipo_Objeto.Nevera:
                GetComponent<Puerta>().enabled = true;
                break;
            case Tipo_Objeto.Microhondas:
                GetComponent<Puerta>().enabled = true;
                break;
            case Tipo_Objeto.Estanteria:
                GetComponent<Puerta>().enabled = true;
                break;
            case Tipo_Objeto.Cajon:
                GetComponent<Cajon>().enabled =true;
                break;
            case Tipo_Objeto.Estufa:
                GetComponent<Estufa>().enabled = true;
                break;
            case Tipo_Objeto.Olla:
                GetComponent<Olla>().enabled = true;
                break;
            case Tipo_Objeto.Alimento:
                GetComponent<Alimento>().enabled = true;
                break;
            case Tipo_Objeto.Condimento:
                GetComponent<Alimento>().enabled = true;
                break;
        }
    }
    private void OnDisable()
    {
        switch (Tipo_Objeto)
        {
            case Tipo_Objeto.Nevera:
                GetComponent<Puerta>().enabled = false;
                break;
            case Tipo_Objeto.Microhondas:
                GetComponent<Puerta>().enabled = false;
                break;
            case Tipo_Objeto.Estanteria:
                GetComponent<Puerta>().enabled = false;
                break;
            case Tipo_Objeto.Cajon:
                GetComponent<Cajon>().enabled = false;
                break;
            case Tipo_Objeto.Estufa:
                GetComponent<Estufa>().enabled = false;
                break;
            case Tipo_Objeto.Olla:
                GetComponent<Olla>().enabled = false;
                break;
            case Tipo_Objeto.Alimento:
                GetComponent<Alimento>().enabled = false;
                break;
            case Tipo_Objeto.Tapa:
                GetComponent<Outline>().enabled = false;
                break;
            case Tipo_Objeto.Condimento:
                GetComponent<Alimento>().enabled = false;
                break;
        }
    }
}
