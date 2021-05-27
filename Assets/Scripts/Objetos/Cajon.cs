using UnityEngine;

public class Cajon : MonoBehaviour
{
    [Range(-0.5f, 0.5f)][Tooltip("Distancia de desplazamiento")]public float desplazamiento=0.3f;
    [Tooltip("Eje principal de desplazamiento")]public bool despZ=false;
    public float Desplazamiento
    {
        get { return desplazamiento; }
        set { desplazamiento = Mathf.Clamp(value, -0.5f, 0.5f); }
    }
    private bool A_Desplazado = false;

    private void Awake()
    {
        if (!A_Desplazado) ObjetosHabilitados(transform, false);
        transform.tag = "Untagged";
    }
    public void Desplazar()
    {
        float r;
        if (!A_Desplazado)
        {
            r = Desplazamiento;
            A_Desplazado = true;
        }
        else
        {
            r = Desplazamiento * -1;
            A_Desplazado = false;
        }
        if(despZ)
        transform.Translate(new Vector3(0f, 0f, r), Space.Self);
        else
            transform.Translate(new Vector3(r, 0f, 0f), Space.Self);
    }
    private void OnEnable()
    {
        transform.tag = "Almacenar";
        ObjetosHabilitados(transform, true);
    }
    private void OnDisable()
    {
        if (!A_Desplazado) 
        {
            ObjetosHabilitados(transform, false);
            transform.tag = "Untagged";
        }
    }
    void ObjetosHabilitados(Transform padre, bool habilitar)
    {
        if (padre.childCount != 0)
            for (int n = 0; n < padre.childCount; n++)
                if (padre.GetChild(n) != padre && !padre.GetChild(n).GetComponent<Olla>())
                    padre.GetChild(n).gameObject.SetActive(habilitar);
    }
}
