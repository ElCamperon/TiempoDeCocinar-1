using Interactuar;
using UnityEngine;

public class Puerta : MonoBehaviour
{
    [SerializeField]Tipo_Objeto _TipoObjeto;
    [SerializeField]int _rotacion;
    [SerializeField]Transform _puerta;
    [SerializeField]Transform _interior;
    private readonly Rotar _rotar = new Rotar();
    public Tipo_Objeto TipoDeObjeto { get => _TipoObjeto; set => _TipoObjeto = value; }
    public int Rotacion { get => _rotacion; set => _rotacion = value; }
    public Transform TransformPuerta { get => _puerta; set => _puerta = value; }
    public Transform TransformInterior { get => _interior; set => _interior = value; }
    private void Awake()
    {
        _rotar.Asignar(TipoDeObjeto, Rotacion, TransformPuerta, gameObject, TransformInterior);
    }
    public void Rotar()
    {
        _rotar.RotarPuerta();
    }
}
