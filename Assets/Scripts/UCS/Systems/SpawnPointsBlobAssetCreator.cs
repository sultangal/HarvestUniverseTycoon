using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEditor.PackageManager;
using static UnityEditor.Progress;

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


        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var g in SystemAPI.Query<RefRW<SpawnPointsComponent>>())
        {
            for (int i = 0; i < g.ValueRO.Value.Value.points.Length; i++)
            {
                var point = g.ValueRO.Value.Value.points[i];

                //float3 position = new(
                //    randomComponent.ValueRW.Value.NextFloat(),
                //    randomComponent.ValueRW.Value.NextFloat(),
                //    randomComponent.ValueRW.Value.NextFloat());
                //position *= 0.02f; //Const multiplier
                //position += point;

                LocalTransform newTransform = new()
                {
                    Position = point * 5f,
                    Rotation = quaternion.LookRotation(math.normalize(point), new(0f, 1f, 0f)),
                    Scale = 1.0f
                };
                
                newTransform.Rotation = math.mul(newTransform.Rotation, quaternion.RotateX(math.radians(90f)));

                Entity newCorn;
                float vertColor = g.ValueRO.Value.Value.vcolor_2_Items[i].x;
                if (vertColor == 0.0f)
                    newCorn = ecb.Instantiate(cornComponent.ValueRO.cornPrefabEntity1);
                else
                    newCorn = ecb.Instantiate(cornComponent.ValueRO.cornPrefabEntity2);

                //var randomComponent = ecb.com GetComponentRW<RandomComponent>(newCorn);


                //item.Rotate(new(0.0f, UnityEngine.Random.value * 360f, 0.0f));
                //item.position += GameManager.Instance.GameSessionData_.CurentPlanetPosition;
                //Items.Add(item);

                ecb.SetComponent(newCorn, newTransform);
            }

        }

        ecb.Playback(state.EntityManager);

        /*
        //var entity = SystemAPI.ManagedAPI.GetSingletonEntity<SpawnPointsSourceComponent>();
        //var component = SystemAPI.ManagedAPI.GetComponent<SpawnPointsSourceComponent>(entity);

        ////var ecb = new EntityCommandBuffer(Allocator.Temp);

        //var builder = new BlobBuilder(Allocator.Temp);

        //ref var spawnPoints = ref builder.ConstructRoot<SpawnPointsBlob>();
        //var pointsBuilder = builder.Allocate(ref spawnPoints.points, component.meshForPointsSource_2_Items.vertices.Length);
        //var vcolor_2_Items_Builder = builder.Allocate(ref spawnPoints.vcolor_2_Items, component.meshForPointsSource_2_Items.colors.Length);
        //var vcolor_3_Items_Builder = builder.Allocate(ref spawnPoints.vcolor_3_Items, component.meshForPointsSource_3_Items.colors.Length);
        //var vcolor_4_Items_Builder = builder.Allocate(ref spawnPoints.vcolor_4_Items, component.meshForPointsSource_4_Items.colors.Length);
        //var vcolor_5_Items_Builder = builder.Allocate(ref spawnPoints.vcolor_5_Items, component.meshForPointsSource_5_Items.colors.Length);
        //var vcolor_6_Items_Builder = builder.Allocate(ref spawnPoints.vcolor_6_Items, component.meshForPointsSource_6_Items.colors.Length);
        //var vcolor_7_Items_Builder = builder.Allocate(ref spawnPoints.vcolor_7_Items, component.meshForPointsSource_7_Items.colors.Length);
        //var vcolor_8_Items_Builder = builder.Allocate(ref spawnPoints.vcolor_8_Items, component.meshForPointsSource_8_Items.colors.Length);

        //for (int i = 0; i < component.meshForPointsSource_2_Items.vertices.Length; i++)
        //{
        //    pointsBuilder[i] = component.meshForPointsSource_2_Items.vertices[i];

        //    vcolor_2_Items_Builder[i].x = component.meshForPointsSource_2_Items.colors[i].r;
        //    vcolor_2_Items_Builder[i].y = component.meshForPointsSource_2_Items.colors[i].g;
        //    vcolor_2_Items_Builder[i].z = component.meshForPointsSource_2_Items.colors[i].b;

        //    vcolor_3_Items_Builder[i].x = component.meshForPointsSource_3_Items.colors[i].r;
        //    vcolor_3_Items_Builder[i].y = component.meshForPointsSource_3_Items.colors[i].g;
        //    vcolor_3_Items_Builder[i].z = component.meshForPointsSource_3_Items.colors[i].b;

        //    vcolor_4_Items_Builder[i].x = component.meshForPointsSource_4_Items.colors[i].r;
        //    vcolor_4_Items_Builder[i].y = component.meshForPointsSource_4_Items.colors[i].g;
        //    vcolor_4_Items_Builder[i].z = component.meshForPointsSource_4_Items.colors[i].b;

        //    vcolor_5_Items_Builder[i].x = component.meshForPointsSource_5_Items.colors[i].r;
        //    vcolor_5_Items_Builder[i].y = component.meshForPointsSource_5_Items.colors[i].g;
        //    vcolor_5_Items_Builder[i].z = component.meshForPointsSource_5_Items.colors[i].b;

        //    vcolor_6_Items_Builder[i].x = component.meshForPointsSource_6_Items.colors[i].r;
        //    vcolor_6_Items_Builder[i].y = component.meshForPointsSource_6_Items.colors[i].g;
        //    vcolor_6_Items_Builder[i].z = component.meshForPointsSource_6_Items.colors[i].b;

        //    vcolor_7_Items_Builder[i].x = component.meshForPointsSource_7_Items.colors[i].r;
        //    vcolor_7_Items_Builder[i].y = component.meshForPointsSource_7_Items.colors[i].g;
        //    vcolor_7_Items_Builder[i].z = component.meshForPointsSource_7_Items.colors[i].b;

        //    vcolor_8_Items_Builder[i].x = component.meshForPointsSource_8_Items.colors[i].r;
        //    vcolor_8_Items_Builder[i].y = component.meshForPointsSource_8_Items.colors[i].g;
        //    vcolor_8_Items_Builder[i].z = component.meshForPointsSource_8_Items.colors[i].b;
        //}

        //var blobAsset = builder.CreateBlobAssetReference<SpawnPointsBlob>(Allocator.Persistent);

        //var newEntity = EntityManager.CreateEntity(typeof(SpawnPointsComponent));

        //SystemAPI.SetComponent(newEntity, new SpawnPointsComponent { Value = blobAsset });

        //builder.Dispose();

        //EntityManager.DestroyEntity(entity);

        //ecb.Playback(EntityManager);
        */


    }
}
