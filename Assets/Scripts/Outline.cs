using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ListVector3
{
    public List<Vector3> data;
}
public class Outline : MonoBehaviour {
  private static readonly HashSet<Mesh> registeredMeshes = new HashSet<Mesh>();

  public enum Mode {
    OutlineAll,
    OutlineVisible,
    OutlineHidden,
    OutlineAndSilhouette,
    SilhouetteOnly
  }

  public Mode OutlineMode {
    get { return outlineMode; }
        set { outlineMode = value;
        //set { outlineMode = Mode.OutlineAll; 
        }
  }

  public Color OutlineColor {
    get { return outlineColor; }
    set {outlineColor = value;}
  }

  public float OutlineWidth {
    get { return outlineWidth; }
    set {outlineWidth = value;}
  }

  public Mode outlineMode=Mode.OutlineAll;
  public Color outlineColor = Color.white;
  [Range(0f, 10f)]
  public float outlineWidth = 10f;

  [SerializeField, HideInInspector]
  private List<Mesh> bakeKeys = new List<Mesh>();

  [SerializeField, HideInInspector]
  private List<ListVector3> bakeValues = new List<ListVector3>();

  private Material outlineMaskMaterial;
  private Material outlineFillMaterial;

  void Awake() {
    // Instantiate outline materials
    outlineMaskMaterial = Instantiate(Resources.Load<Material>("Materiales/OutlineMask"));
    outlineFillMaterial = Instantiate(Resources.Load<Material>("Materiales/OutlineFill"));

    outlineMaskMaterial.name = "OutlineMask (Instance)";
    outlineFillMaterial.name = "OutlineFill (Instance)";

    // Retrieve or generate smooth normals
    LoadSmoothNormals();
  }

  void OnEnable() {
        UpdateMaterialProperties();

        Renderer render = GetComponent<Renderer>();
        var material = render.sharedMaterials.ToList();

        material.Add(outlineMaskMaterial);
        material.Add(outlineFillMaterial);

        render.materials = material.ToArray();
    }

  void OnValidate() {
    // Clear cache when baking is disabled or corrupted
    if (bakeKeys.Count != 0 || bakeKeys.Count != bakeValues.Count) {
      bakeKeys.Clear();
      bakeValues.Clear();
    }
  }

  void OnDisable() {
        Renderer render = GetComponent<Renderer>();
        var material = render.sharedMaterials.ToList();

        material.Remove(outlineMaskMaterial);
        material.Remove(outlineFillMaterial);

        render.materials = material.ToArray();
  }
    #region Prueba
    //void OnDestroy() {
    //
    //  // Destroy material instances
    //  Destroy(outlineMaskMaterial);
    //  Destroy(outlineFillMaterial);
    //}

    /*void Bake() {

      // Generate smooth normals for each mesh
      var bakedMeshes = new HashSet<Mesh>();

      foreach (var meshFilter in GetComponentsInChildren<MeshFilter>()) {

        // Skip duplicates
        if (!bakedMeshes.Add(meshFilter.sharedMesh)) {
          continue;
        }

        // Serialize smooth normals
        var smoothNormals = SmoothNormals(meshFilter.sharedMesh);

        bakeKeys.Add(meshFilter.sharedMesh);
        bakeValues.Add(new ListVector3() { data = smoothNormals });
      }
    }*/
    #endregion
    void LoadSmoothNormals() {

    // Retrieve or generate smooth normals
    foreach (var meshFilter in GetComponentsInChildren<MeshFilter>()) {

      // Skip if smooth normals have already been adopted
      if (!registeredMeshes.Add(meshFilter.sharedMesh)) {
        continue;
      }

      // Retrieve or generate smooth normals
      var index = bakeKeys.IndexOf(meshFilter.sharedMesh);
      var smoothNormals = (index >= 0) ? bakeValues[index].data : SmoothNormals(meshFilter.sharedMesh);

      // Store smooth normals in UV3
      meshFilter.sharedMesh.SetUVs(3, smoothNormals);
    }

    // Clear UV3 on skinned mesh renderers
    foreach (var skinnedMeshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>()) {
      if (registeredMeshes.Add(skinnedMeshRenderer.sharedMesh)) {
        skinnedMeshRenderer.sharedMesh.uv4 = new Vector2[skinnedMeshRenderer.sharedMesh.vertexCount];
      }
    }
  }

  List<Vector3> SmoothNormals(Mesh mesh) {
        try
        {
            // Group vertices by location
            var groups = mesh.vertices.Select((vertex, index) => new KeyValuePair<Vector3, int>(vertex, index)).GroupBy(pair => pair.Key);

            // Copy normals to a new list
            var smoothNormals = new List<Vector3>(mesh.normals);

            // Average normals for grouped vertices
            foreach (var group in groups)
            {

                // Skip single vertices
                if (group.Count() == 1)
                {
                    continue;
                }

                // Calculate the average normal
                var smoothNormal = Vector3.zero;

                foreach (var pair in group)
                {
                    smoothNormal += mesh.normals[pair.Value];
                }

                smoothNormal.Normalize();

                // Assign smooth normal to each vertex
                foreach (var pair in group)
                {
                    smoothNormals[pair.Value] = smoothNormal;
                }
            }

            return smoothNormals;
        }
        catch
        {
            Debug.LogWarning("El objeto: " + gameObject.name + ", no posee MESH");
            return new List<Vector3>();
        }
  }

  void UpdateMaterialProperties() {

    // Apply properties according to mode
    outlineFillMaterial.SetColor("_OutlineColor", outlineColor);

    switch (outlineMode) {
      case Mode.OutlineAll:
        outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
        outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
        outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
        break;

      case Mode.OutlineVisible:
        outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
        outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
        outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
        break;

      case Mode.OutlineHidden:
        outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
        outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Greater);
        outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
        break;

      case Mode.OutlineAndSilhouette:
        outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
        outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
        outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
        break;

      case Mode.SilhouetteOnly:
        outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
        outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Greater);
        outlineFillMaterial.SetFloat("_OutlineWidth", 0);
        break;
    }
  }
}
