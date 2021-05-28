using System;
using UnityEngine;

namespace Interactuar
{
    #region Categorias
    /// <summary>
    /// Categoria/Caracteristica/Tipo de objeto
    /// </summary>
    public enum Tipo_Objeto
    {
        Null,
        Microhondas,
        Nevera,
        Grifo,
        Estanteria,
        Cajon,
        Cuchillo,
        Cucharon,
        Condimento,
        Recipiente,
        Estufa,
        Olla,
        Tapa,
        Alimento,
        Plato
    }
    /// <summary>
    /// Comportamiento con respecto al jugador
    /// </summary>
    public enum Tipo_Interaccion
    {
        Null,
        Sostener,
        Rotar,
        Desplazar,
        Encender
    }
    /// <summary>
    /// Comportamiento de un objeto a otro
    /// </summary>
    public enum Tipo_Accion
    {
        Null,
        Picar,
        Calentar,
        Soltar,
        Agregar,
        Girar,
        Almacenar,
        Servir
    }
    /// <summary>
    /// Categoria/Caracteristica/Tipo de alimento
    /// </summary>
    public enum Tipo_Alimento
    {
        Null,
        Huevo,
        Carne,
        Leche,
        Fruta,
        Cilantro,
        CebollaLarga,
        CebollaPicada,
        CilantroPicado,
        Salero,
        Pimentero,
        Pan
    }
    /// <summary>
    /// Tipo de incoveniente que puede ocurrir por parte del jugador para avanzar
    /// </summary>
    public enum Tipo_Mensaje
    {
        Null,
        Tapa,
        Picar,
        Incompleto,
    }
    /// <summary>
    /// Tipo de sonido a escucharse de manera global
    /// </summary>
    public enum Tipo_Sonido
    {
        Sujetar,
        MisionCompleta,
        AgregarAOlla,
        Picar,
        Salero,
        Leche,
        Huevo
    }
    #endregion
    public static class Eventos
    {
        public static Action<Olla,bool> Olla;
        public static Action <int>MisionCompletada;
        public static Action<Tipo_Mensaje> Mensaje;
        public static Action<AudioSource,float> ApagarAudio;
        public static Action<Tipo_Sonido> TipoDeSonido;
        public static Action FinalizarJuego;
        public static Action<bool> Pausar;
        public static Action Cargar;
        //public static Action<bool> Comienzo;
    }
    public class Objeto
    {
        public static ObjetoInteractivo Asignar(ObjetoInteractivo _obj)
        {
            GameObject obj =_obj.gameObject;
            Tipo_Objeto tipo = _obj.Tipo_Objeto;

            if (tipo.Equals(Tipo_Objeto.Condimento) || tipo.Equals(Tipo_Objeto.Alimento)) obj.name = obj.GetComponent<Alimento>().alimento.ToString();
            else obj.name = tipo.ToString();

            #region Valores por defecto
            obj.layer = 8;

            if (!obj.GetComponent<Collider>())
            {
                obj.AddComponent<BoxCollider>();
            }
            #endregion

            if (tipo.Equals(Tipo_Objeto.Condimento) || tipo.Equals(Tipo_Objeto.Cuchillo) || tipo.Equals(Tipo_Objeto.Alimento) || 
                tipo.Equals(Tipo_Objeto.Olla) || tipo.Equals(Tipo_Objeto.Tapa) || tipo.Equals(Tipo_Objeto.Recipiente) 
                || tipo.Equals(Tipo_Objeto.Cucharon) || tipo.Equals(Tipo_Objeto.Plato))
                _obj.Tipo_Interaccion = Tipo_Interaccion.Sostener;
            else
            if (tipo.Equals(Tipo_Objeto.Microhondas) || tipo.Equals(Tipo_Objeto.Nevera) || tipo.Equals(Tipo_Objeto.Grifo) || tipo.Equals(Tipo_Objeto.Estanteria))
                _obj.Tipo_Interaccion = Tipo_Interaccion.Rotar;
            else
                if (tipo.Equals(Tipo_Objeto.Cajon))
                _obj.Tipo_Interaccion = Tipo_Interaccion.Desplazar;
            else
                if (tipo.Equals(Tipo_Objeto.Estufa))
                _obj.Tipo_Interaccion = Tipo_Interaccion.Encender;

            return _obj;
        }
        public static bool EsAlmacenable(Tipo_Objeto activo)
        {
            if (activo.Equals(Tipo_Objeto.Condimento) || activo.Equals(Tipo_Objeto.Olla))
                return false;
            else return true;
        }
        public static string Accion(Tipo_Objeto interactuando, Tipo_Objeto aInteractuar, Tipo_Alimento alimento=Tipo_Alimento.Null)
        {
            switch (interactuando)
            {
                case Tipo_Objeto.Alimento:
                    if (aInteractuar.Equals(Tipo_Objeto.Microhondas))
                        return Tipo_Accion.Calentar.ToString();

                    if (aInteractuar.Equals(Tipo_Objeto.Olla) && (!alimento.Equals(Tipo_Alimento.CebollaPicada) || alimento.Equals(Tipo_Alimento.Pan) || alimento.Equals(Tipo_Alimento.CebollaPicada)
                        || alimento.Equals(Tipo_Alimento.Leche) || alimento.Equals(Tipo_Alimento.Huevo) || alimento.Equals(Tipo_Alimento.CilantroPicado)))
                        return Tipo_Accion.Agregar.ToString();
                    break;

                case Tipo_Objeto.Cuchillo:
                    if (aInteractuar.Equals(Tipo_Objeto.Alimento) && (alimento.Equals(Tipo_Alimento.Cilantro) || (alimento.Equals(Tipo_Alimento.CebollaLarga))))
                        return Tipo_Accion.Picar.ToString();
                    break;

                case Tipo_Objeto.Condimento:
                    if (aInteractuar.Equals(Tipo_Objeto.Olla))
                        return Tipo_Accion.Agregar.ToString();
                    break;

                case Tipo_Objeto.Plato:
                    if (aInteractuar.Equals(Tipo_Objeto.Olla))
                        return Tipo_Accion.Servir.ToString();
                    break;
            }
            return null;
        }
        public static int[] EvaluarResultado(Olla olla)
        {
            int[] var=new int[7];
            if (olla.NivelAgua <= 50) var[0] = olla.NivelAgua * 20;
            else var[0] = Mathf.Clamp(2000-(olla.NivelAgua * 20),0,1000);

            if (olla.NivelLeche <= 50) var[1] = olla.NivelLeche * 20;
            else var[1] = Mathf.Clamp(2000-(olla.NivelLeche * 20),0,1000);

            if (olla.cilantro.gameObject.activeSelf) var[2] = 1000;
            else var[2] = 0;

            if (olla.cebolla.gameObject.activeSelf) var[3] = 1000;
            else var[3] = 0;

            if (olla.NumeroDePanes <= 2) var[4] = olla.NumeroDePanes * 500;
            else var[4] = Mathf.Clamp(2000 - (olla.NumeroDePanes *500),0,1000);

            if (olla.NumeroDeHuevos <= 2) var[5] = olla.NumeroDeHuevos * 500;
            else var[5] = Mathf.Clamp(2000 - (olla.NumeroDeHuevos *500), 0, 1000);

            var[6] =var[0]+ var[1]+ var[2]+ var[3]+ var[4]+ var[5];

            return var;
        }
    }
    public class Rotar
    {
        bool _ARotado;
        int _Rotacion;
        Transform _Puerta;
        Transform _Interior;
        AudioClip[] _Audios;
        AudioSource _Audio;

        public bool ARotado { get => _ARotado; set => _ARotado = value; }
        public int Rotacion { get => _Rotacion; set => _Rotacion = Mathf.Clamp(value, -120, 120); }
        public Transform Puerta { get => _Puerta; set => _Puerta = value; }
        public Transform Interior { get => _Interior; set => _Interior = value; }
        public AudioClip[] Audios { get => _Audios; set => _Audios = value; }
        public AudioSource Audio { get => _Audio; set => _Audio = value; }
        public void Asignar(Tipo_Objeto objeto, int rotacion, Transform puerta, GameObject electrodomestico,Transform interior=null)
        {
            Rotacion = rotacion;
            ARotado = false;
            Puerta = puerta;
            Interior = interior;

            string _ruta = BuscarRuta(objeto);
            if (_ruta != null) Audios = Resources.LoadAll<AudioClip>(_ruta);

            if (interior)
            {
                ObjetosHabilitados(interior, false);
                interior.tag = "Untagged";
            }
            if (!Puerta) Puerta = electrodomestico.transform;

            if (!electrodomestico.GetComponent<AudioSource>())
            {
                electrodomestico.AddComponent<AudioSource>();
                Audio = electrodomestico.GetComponent<AudioSource>();
                IgualarParametrosAudio();
            }
            else Audio = electrodomestico.GetComponent<AudioSource>();
        }
        public void EfSonido(int s)
        {
            if (Audio && s < Audios.Length)
            {
                Audio.enabled = true;
                Audio.PlayOneShot(Audios[s]);
            }
        }
        public void RotarPuerta()
        {
            if (!ARotado)
            {
                EfSonido(0);
                Puerta.Rotate(new Vector3(0f, Rotacion, 0f), Space.Self);

                if (Interior)
                {
                    Interior.tag = "Almacenar";
                    ObjetosHabilitados(Interior, true);
                }

                ARotado = true;
            }
            else
            {
                EfSonido(1);
                Puerta.Rotate(new Vector3(0f, -Rotacion, 0f), Space.Self);
                if(Audio) Eventos.ApagarAudio(Audio, 3f);

                if (Interior)
                {
                    Interior.tag = "Untagged";
                    ObjetosHabilitados(Interior, false);
                }
                ARotado = false;
            }
        }
        string BuscarRuta(Tipo_Objeto objeto)
        {
            switch (objeto)
            {
                case Tipo_Objeto.Microhondas:
                    return "Sonidos/Microhondas";
                case Tipo_Objeto.Estanteria:
                    return "Sonidos/Estanteria";
                case Tipo_Objeto.Nevera:
                    return "Sonidos/Nevera";
                default:
                    return null;
            }
        }
        void IgualarParametrosAudio()
        {
            //AudioSource audio = Resources.Load<Transform>("Prefabs/AudioSource").GetComponent<AudioSource>();
            Audio.rolloffMode = AudioRolloffMode.Custom;//audio.rolloffMode;
            Audio.maxDistance = 2f;//2.6203f;//audio.maxDistance;
            Audio.minDistance = 0f;//0.279263f;//audio.minDistance;
            Audio.spatialBlend = 1f;
            Audio.bypassEffects = true;
            Audio.enabled = false;
        }
        void ObjetosHabilitados(Transform padre, bool habilitar)
        {
            if (padre.childCount > 0)
                for (int n = 0; n < padre.childCount; n++)
                {
                    //padre.GetChild(n).gameObject.SetActive(habilitar);
                    if (padre.GetChild(n) != padre && !padre.GetChild(n).GetComponent<Olla>())
                    {
                        padre.GetChild(n).gameObject.SetActive(habilitar);
                    }
                }
        }
    }
}
