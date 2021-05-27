using UnityEngine;
using Interactuar;

public class Misiones : MonoBehaviour
{
    [TextArea()] public string[] misiones=new string[4];
    public string[] tituloMisiones = new string[4];

    private UIController ui;
    public Transform final;
    int misionActual=0;
    void Start()
    {
        ui = GetComponent<UIController>();
        final.gameObject.SetActive(false);
        AsignarMision();

        Eventos.MisionCompletada += AsignarMision;
        Eventos.FinalizarJuego+=JuegoTerminado;
    }
    void AsignarMision(int id=0)
    {
        switch (misionActual)
        {
            case 0:
                ui.CambiarMision(0, tituloMisiones[0], misiones[0]);
                break;
            case 1:
                EliminarOllas(id);
                ui.CambiarMision(1, tituloMisiones[1], misiones[1]);
                break;
            case 2:
                ui.CambiarMision(2, tituloMisiones[2], misiones[2]);
                break;
            case 3:
                ui.CambiarMision(3, tituloMisiones[3], misiones[3]);
                break;
        }
        misionActual++;
        //if (misionActual < tituloMisiones.Length-1)
        //{
        //    if (misionActual.Equals(1)) EliminarOllas(id);
        //    ui.CambiarMision(misionActual, tituloMisiones[misionActual], misiones[misionActual]);
        //    misionActual++;
        //}
    }
    void JuegoTerminado()
    {
        final.gameObject.SetActive(true);

        Camera camara = Camera.main;
        camara.targetDisplay = 1;
    }
    void EliminarOllas(int id)
    {
        if (!id.Equals(0))
        {
            Olla[] ollas = FindObjectsOfType<Olla>();

            foreach(Olla olla in ollas)
            {
                if (!olla.GetInstanceID().Equals(id))
                    Destroy(olla.GetComponent<Olla>());
            }
        }
    }
}
