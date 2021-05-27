using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessing : MonoBehaviour
{
    PostProcessVolume postProcessing;
    AmbientOcclusion ao;
    Bloom b;
    ColorGrading cg;
    MotionBlur mb;
    ScreenSpaceReflections ssr;
    Vignette v;

    private void Awake()
    {
        if (gameObject.GetComponent<PostProcessVolume>())
            postProcessing = gameObject.GetComponent<PostProcessVolume>();
        else
        postProcessing = FindObjectOfType<PostProcessVolume>();

        postProcessing.profile.TryGetSettings(out ao);
        postProcessing.profile.TryGetSettings(out b);
        postProcessing.profile.TryGetSettings(out cg);
        postProcessing.profile.TryGetSettings(out mb);
        postProcessing.profile.TryGetSettings(out ssr);
        postProcessing.profile.TryGetSettings(out v);
    }
    public void EstadoOcclusion(bool estado)
    {
        ao.active = estado;
    }
    public void EstadoBoom(bool estado)
    {
        b.active = estado;
    }
    public void EstadoColorGradiend(bool estado)
    {
        cg.active = estado;
    }
    public void EstadoMotionBlur(bool estado)
    {
        mb.active = estado;
    }
    public void EstadoScreenSpaceReflections(bool estado)
    {
        ssr.active = estado;
    }
    public void EstadoVignette(bool estado)
    {
        v.active = estado;
    }
}
