using Interactuar;
using System.Collections;
using UnityEngine;

public class Fuego : MonoBehaviour
{
    private Olla olla;
    private Fuego fuego;
    
    void Awake()
    {
        gameObject.tag = "Quemador";
        fuego = GetComponent<Fuego>();
        fuego.enabled = false;

        Eventos.Olla += Info;
    }
    private void OnEnable()
    {
        if (!olla)
            transform.GetChild(0).gameObject.SetActive(true);
        else StartCoroutine(nameof(EncenderQuemador));
    }
    public void Info(Olla _olla,bool t=false)
    {
        olla = _olla;

        if (_olla && fuego.enabled)
        {
            if (_olla.Temperatura <= 99f)
            {
                transform.GetChild(0).gameObject.SetActive(false);
                StartCoroutine(nameof(EncenderQuemador));
            }
            else StopCoroutine(nameof(EncenderQuemador));
        }
        else StopCoroutine(nameof(EncenderQuemador));

        if (!_olla && fuego.enabled) transform.GetChild(0).gameObject.SetActive(true);
    }
    IEnumerator EncenderQuemador()
    {
        while (olla.Temperatura<=100f)
        {
            olla.Hirviendo(0.025f);
            yield return null;
        }
    }
    private void OnDisable()
    {
        StopCoroutine(nameof(EncenderQuemador));
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
