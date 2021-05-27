using System.Linq;
using UnityEngine;

public class ExteriorEstanteria : MonoBehaviour
{
    [SerializeField]private int[] hijo;
    [SerializeField]private Vector3[] posHijo;
    private Quaternion[] rotHijo;

    /// <summary>
    /// Guarda las variables del transform de los hijos
    /// </summary>
    private void Awake()
    {
        hijo = new int[transform.childCount];
        posHijo = new Vector3[hijo.Length];
        rotHijo = new Quaternion[hijo.Length];

        for (int n = 0; n < transform.childCount; n++)
            if (transform.GetChild(n) != transform)
            {
                hijo[n]= transform.GetChild(n).GetInstanceID();
                posHijo[n] = transform.GetChild(n).localPosition;
                rotHijo[n] = transform.GetChild(n).rotation;
            }
    }
    /// <summary>
    /// Verifica si un objeto fue hijo de este objeto
    /// </summary>
    /// ID del objeto que comprueba si fue hijo
    /// <param name="id"></param>
    /// <returns></returns>
    public bool ComprobarParentesco(int id)
    {
        if (hijo.Contains(id))
            return true;
        else return false;
    }
    /// <summary>
    /// Si fue hijo, regresa a tener sus valores iniciales
    /// </summary>
    /// Hijo a comprobar
    /// <param name="_hijo"></param>
    public void Parentar(Transform _hijo)
    {
        int id = _hijo.transform.GetInstanceID();

        for (int n=0; n < hijo.Length; n++)
        {
            if (hijo[n] == id)
            {
                _hijo.SetParent(transform);
                _hijo.localPosition = posHijo[n];
                _hijo.rotation = rotHijo[n];
                _hijo.GetComponent<BoxCollider>().enabled = true;
                break;
            }
        }
    }
}
