using Unity.Entities;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class RandomAuthoring : MonoBehaviour
{
    public uint RandomSeed;
}
public class RandomBaker : Baker<RandomAuthoring>
{
    public override void Bake(RandomAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new RandomComponent
        {
            Value = Random.CreateFromIndex(authoring.RandomSeed)
        });
    }
}
