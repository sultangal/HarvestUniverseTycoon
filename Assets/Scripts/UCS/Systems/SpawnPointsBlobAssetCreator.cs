using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct SpawnPointsBlobAssetCreator : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<SpawnPointsComponent>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.Enabled = false;
        var entity = SystemAPI.GetSingletonEntity<CornPrefabComponent>();
        var cornComponent = SystemAPI.GetComponentRO<CornPrefabComponent>(entity);
        var randomComponent = SystemAPI.GetComponentRW<RandomComponent>(entity);
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var g in SystemAPI.Query<RefRW<SpawnPointsComponent>>())
        {
            for (int i = 0; i < g.ValueRO.Value.Value.points.Length; i++)
            {
                var point = g.ValueRO.Value.Value.points[i];

                float3 position = new(
                    randomComponent.ValueRW.Value.NextFloat(),
                    randomComponent.ValueRW.Value.NextFloat(),
                    randomComponent.ValueRW.Value.NextFloat());
                position *= 0.02f; //Const multiplier
                position += point;
                position = math.normalize(position);

                LocalTransform newTransform = new()
                {
                    Position = position * 5.0f,
                    Rotation = quaternion.LookRotationSafe(math.normalize(point), new(0f, randomComponent.ValueRW.Value.NextFloat(0.1f, 1f), 0f)),
                    Scale = randomComponent.ValueRW.Value.NextFloat(0.8f, 1.0f)
                };               
                newTransform.Rotation = math.mul(newTransform.Rotation, quaternion.RotateX(math.radians(90f)));
                Entity newCorn;
                float vertColor = g.ValueRO.Value.Value.vcolor_2_Items[i];
                if (vertColor == 0.0f)
                    newCorn = ecb.Instantiate(cornComponent.ValueRO.cornPrefabEntity1);
                else
                    newCorn = ecb.Instantiate(cornComponent.ValueRO.cornPrefabEntity2);
                ecb.SetComponent(newCorn, newTransform);
            }

        }
        ecb.Playback(state.EntityManager);
    }
}
