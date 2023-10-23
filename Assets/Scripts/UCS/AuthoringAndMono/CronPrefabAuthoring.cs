using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class CornPrefabMono : MonoBehaviour
{
    public GameObject cornPrefabGameObject1;
    public GameObject cornPrefabGameObject2;
}

public class CornPrefabBaker : Baker<CornPrefabMono>
{
    public override void Bake(CornPrefabMono authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity, new CornPrefabComponent
        {
            cornPrefabEntity1 = GetEntity(authoring.cornPrefabGameObject1, TransformUsageFlags.Dynamic),
            cornPrefabEntity2 = GetEntity(authoring.cornPrefabGameObject2, TransformUsageFlags.Dynamic)
        });
    }
}

public struct CornPrefabComponent : IComponentData
{
    public Entity cornPrefabEntity1;
    public Entity cornPrefabEntity2;
}