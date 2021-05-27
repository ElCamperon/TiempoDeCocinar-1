using System.Collections;
using UnityEngine;

public class Agua : MonoBehaviour
{
    Transform aguaOlla = null;
    Olla olla = null;
    private float alt;

    private void OnDisable()
    {
        StopCoroutine(nameof(AgregarAgua));
        aguaOlla = null;
        olla = null;
    }
    public void Info(Transform _agua,bool tieneTapa,Olla _olla)
    {
        if (_olla)
        {
            aguaOlla = _agua;
            olla = _olla;

            if (olla.NivelAgua + olla.NivelLeche < 100)
            {
                if (olla && !tieneTapa) StartCoroutine(nameof(AgregarAgua));
                else StopCoroutine(nameof(AgregarAgua));
            }
            else
            {
                StopCoroutine(nameof(AgregarAgua));
                Destroy(GetComponent<Agua>());
            }
        }
    }

    IEnumerator AgregarAgua()
    {
        while(olla.altLiquido<aguaOlla.transform.position.y)
        {
            olla.Llenar(0.00008f);
            yield return null;
        }
    }
}
