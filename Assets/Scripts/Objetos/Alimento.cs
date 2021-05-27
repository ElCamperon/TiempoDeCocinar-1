using Interactuar;
using UnityEngine;

public class Alimento : MonoBehaviour
{
    public Tipo_Alimento alimento;

    /// <summary>
    /// Usar si el objeto actual en la mano del usuario es de tipo Alimento
    /// </summary>
    /// Objeto afectado por el objeto que contiene este script
    /// <param name="activar"></param>
    public void Activo(ObjetoInteractivo activar)
    {
        if (activar.Tipo_Objeto.Equals(Tipo_Objeto.Olla))
        {
            if (alimento.Equals(Tipo_Alimento.CebollaLarga) || alimento.Equals(Tipo_Alimento.Cilantro))
                Eventos.Mensaje(Tipo_Mensaje.Picar);

            else 
            {
                if (!activar.GetComponent<Olla>().TieneTapa())
                {
                    switch (alimento)
                    {
                        case Tipo_Alimento.Salero:
                            Eventos.TipoDeSonido(Tipo_Sonido.Salero);
                            Eventos.Cargar();
                            break;
                        case Tipo_Alimento.Leche:
                            Eventos.TipoDeSonido(Tipo_Sonido.Leche);
                            Eventos.Cargar();
                            break;
                        case Tipo_Alimento.Pimentero:
                            break;
                        case Tipo_Alimento.Huevo:
                            Eventos.TipoDeSonido(Tipo_Sonido.Huevo);
                            Eventos.Cargar();
                            Destroy(gameObject);
                            break;
                        default:
                            Destroy(gameObject);
                            break;
                    }
                    activar.GetComponent<Olla>().Agregar(alimento);
                }
                else
                    Eventos.Mensaje(Tipo_Mensaje.Tapa);
            }
        }
    }
    /// <summary>
    /// Usar si el objeto a interactuar es de tipo Alimento
    /// </summary>
    /// Objeto que se vera afectado por el objeto que contiene este script
    /// <param name="activar"></param>
    public void Activar(ObjetoInteractivo activo)
    {
        switch (alimento)
        {
            case Tipo_Alimento.CebollaLarga:
                if(activo.Tipo_Objeto.Equals(Tipo_Objeto.Cuchillo))
                {
                    Eventos.TipoDeSonido(Tipo_Sonido.Picar);
                    Eventos.Cargar();
                    Quaternion rot = Quaternion.Euler(0f, 0f, 0f);
                    Instantiate(Resources.Load<GameObject>("Prefabs/Cebolla_Picada"), transform.GetChild(0).transform.position, rot);
                    Destroy(gameObject);
                }
                break;
            case Tipo_Alimento.Cilantro:
                {
                    Eventos.TipoDeSonido(Tipo_Sonido.Picar);
                    Eventos.Cargar();
                    Instantiate(Resources.Load<GameObject>("Prefabs/Cilantro_Picado"), transform.GetChild(0).transform.position, transform.rotation);
                    Destroy(gameObject);
                }
                break;
        }
    }
}
