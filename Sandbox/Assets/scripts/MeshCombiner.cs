using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCombiner : MonoBehaviour
{
    [SerializeField] private List<MeshFilter> sourceMeshFilters;
    [SerializeField] private MeshFilter targetMeshFilter;
    [SerializeField] private MeshCollider targetMeshCollider;

    [ContextMenu("Cobine Meshes")]
    private void CombineMeshes()
    {
        var combine = new CombineInstance[sourceMeshFilters.Count];

        for (int i = 0; i < sourceMeshFilters.Count; i++)
        {
            combine[i].mesh = sourceMeshFilters[i].sharedMesh;
            combine[i].transform = sourceMeshFilters[i].transform.localToWorldMatrix;
        }

        var mesh = new Mesh();
        mesh.CombineMeshes(combine);
        targetMeshFilter.mesh = mesh;
        targetMeshCollider.sharedMesh = mesh;
    }

}
