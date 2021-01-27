using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnChangePosition : MonoBehaviour
{
    public PolygonCollider2D hole2DCollider;
    public PolygonCollider2D ground2DCollider;

    public MeshCollider generatedMeshCollider;
    Mesh generatedMesh;

    [Space(5)]

    public bool isVertical;

    private float initalScale = 1f;

    private void FixedUpdate()
    {
        if (transform.hasChanged)
        {
            var wallY = transform.localPosition.z;

            if (isVertical)
                wallY = transform.localPosition.y;


            transform.hasChanged = false;

            hole2DCollider.transform.position = new Vector2(transform.localPosition.x, wallY);
            hole2DCollider.transform.localScale = transform.localScale * initalScale;

            MakeHole2D();
            Make3DMeshCollider();
        }
    }

    private void MakeHole2D()
    {
        Vector2[] PointPositions = hole2DCollider.GetPath(0);

        for (int i = 0; i < PointPositions.Length; i++)
        {
            //PointPositions[i] += (Vector2)hole2DCollider.transform.position;
            PointPositions[i] = hole2DCollider.transform.TransformPoint(PointPositions[i]);
        }

        ground2DCollider.pathCount += 1;
        ground2DCollider.SetPath(ground2DCollider.pathCount-1, PointPositions);
    }

    private void Make3DMeshCollider()
    {
        if (generatedMesh != null) Destroy(generatedMesh);

        generatedMesh = ground2DCollider.CreateMesh(true, true);
        generatedMeshCollider.sharedMesh = generatedMesh;

        generatedMeshCollider.transform.localPosition = new Vector3(generatedMeshCollider.transform.localPosition.x, generatedMeshCollider.transform.localPosition.y, this.transform.parent.localPosition.x);
    }
}
