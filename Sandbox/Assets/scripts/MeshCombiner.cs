using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MeshCombiner : MonoBehaviour
{
    [SerializeField] private List<MeshFilter> sourceMeshFilters;
    [SerializeField] private MeshFilter targetMeshFilter;
    [SerializeField] private MeshCollider targetMeshCollider;

    [ContextMenu("CombineMeshes")]
    private void CombineMeshes()
    {
        var combine = new CombineInstance[sourceMeshFilters.Count];

        for (int i = 0; i < sourceMeshFilters.Count; i++)
        {
            MeshUtility.Optimize(sourceMeshFilters[i].sharedMesh);
            combine[i].mesh = sourceMeshFilters[i].sharedMesh;
            combine[i].transform = sourceMeshFilters[i].transform.localToWorldMatrix;
        }

        Debug.Log("Combined " + sourceMeshFilters.Count + " meshes");
        Debug.Log("Target mesh: " + targetMeshFilter.name);
        var mesh = new Mesh();
        mesh.CombineMeshes(combine);
        MeshUtility.Optimize(mesh);

        targetMeshFilter.mesh = mesh;

        targetMeshCollider.sharedMesh = mesh;
    }

}
