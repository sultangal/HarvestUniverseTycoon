using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class SpawnPointsSourceMono : MonoBehaviour
{
    public Mesh meshForPointsSource_2_Items;
    public Mesh meshForPointsSource_3_Items;
    public Mesh meshForPointsSource_4_Items;
    public Mesh meshForPointsSource_5_Items;
    public Mesh meshForPointsSource_6_Items;
    public Mesh meshForPointsSource_7_Items;
    public Mesh meshForPointsSource_8_Items;
}
public class SpawnPointsSourceBaker : Baker<SpawnPointsSourceMono>
{
    public override void Bake(SpawnPointsSourceMono authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);

        using (var builder = new BlobBuilder(Allocator.Temp))
        {

            ref var spawnPoints = ref builder.ConstructRoot<SpawnPointsBlob>();
            var pointsBuilder = builder.Allocate(ref spawnPoints.points, authoring.meshForPointsSource_2_Items.vertices.Length);
            var vcolor_2_Items_Builder = builder.Allocate(ref spawnPoints.vcolor_2_Items, authoring.meshForPointsSource_2_Items.colors.Length);
            var vcolor_3_Items_Builder = builder.Allocate(ref spawnPoints.vcolor_3_Items, authoring.meshForPointsSource_3_Items.colors.Length);
            var vcolor_4_Items_Builder = builder.Allocate(ref spawnPoints.vcolor_4_Items, authoring.meshForPointsSource_4_Items.colors.Length);
            var vcolor_5_Items_Builder = builder.Allocate(ref spawnPoints.vcolor_5_Items, authoring.meshForPointsSource_5_Items.colors.Length);
            var vcolor_6_Items_Builder = builder.Allocate(ref spawnPoints.vcolor_6_Items, authoring.meshForPointsSource_6_Items.colors.Length);
            var vcolor_7_Items_Builder = builder.Allocate(ref spawnPoints.vcolor_7_Items, authoring.meshForPointsSource_7_Items.colors.Length);
            var vcolor_8_Items_Builder = builder.Allocate(ref spawnPoints.vcolor_8_Items, authoring.meshForPointsSource_8_Items.colors.Length);

            for (int i = 0; i < authoring.meshForPointsSource_2_Items.vertices.Length; i++)
            {
                pointsBuilder[i] = authoring.meshForPointsSource_2_Items.vertices[i];

                vcolor_2_Items_Builder[i].x = authoring.meshForPointsSource_2_Items.colors[i].r;
                vcolor_2_Items_Builder[i].y = authoring.meshForPointsSource_2_Items.colors[i].g;
                vcolor_2_Items_Builder[i].z = authoring.meshForPointsSource_2_Items.colors[i].b;
                                              
                vcolor_3_Items_Builder[i].x = authoring.meshForPointsSource_3_Items.colors[i].r;
                vcolor_3_Items_Builder[i].y = authoring.meshForPointsSource_3_Items.colors[i].g;
                vcolor_3_Items_Builder[i].z = authoring.meshForPointsSource_3_Items.colors[i].b;
                                             
                vcolor_4_Items_Builder[i].x = authoring.meshForPointsSource_4_Items.colors[i].r;
                vcolor_4_Items_Builder[i].y = authoring.meshForPointsSource_4_Items.colors[i].g;
                vcolor_4_Items_Builder[i].z = authoring.meshForPointsSource_4_Items.colors[i].b;
                                              
                vcolor_5_Items_Builder[i].x = authoring.meshForPointsSource_5_Items.colors[i].r;
                vcolor_5_Items_Builder[i].y = authoring.meshForPointsSource_5_Items.colors[i].g;
                vcolor_5_Items_Builder[i].z = authoring.meshForPointsSource_5_Items.colors[i].b;
                                              
                vcolor_6_Items_Builder[i].x = authoring.meshForPointsSource_6_Items.colors[i].r;
                vcolor_6_Items_Builder[i].y = authoring.meshForPointsSource_6_Items.colors[i].g;
                vcolor_6_Items_Builder[i].z = authoring.meshForPointsSource_6_Items.colors[i].b;
                                             
                vcolor_7_Items_Builder[i].x = authoring.meshForPointsSource_7_Items.colors[i].r;
                vcolor_7_Items_Builder[i].y = authoring.meshForPointsSource_7_Items.colors[i].g;
                vcolor_7_Items_Builder[i].z = authoring.meshForPointsSource_7_Items.colors[i].b;
                                            
                vcolor_8_Items_Builder[i].x = authoring.meshForPointsSource_8_Items.colors[i].r;
                vcolor_8_Items_Builder[i].y = authoring.meshForPointsSource_8_Items.colors[i].g;
                vcolor_8_Items_Builder[i].z = authoring.meshForPointsSource_8_Items.colors[i].b;
            }

            var blobAsset = builder.CreateBlobAssetReference<SpawnPointsBlob>(Allocator.Persistent);         

            AddBlobAsset<SpawnPointsBlob>(ref blobAsset, out _);
            AddComponent(entity, new SpawnPointsComponent { Value = blobAsset });
        }

    }    
}

public struct SpawnPointsComponent : IComponentData
{
    public BlobAssetReference<SpawnPointsBlob> Value;
}

public struct SpawnPointsBlob
{
    public BlobArray<float3> points;
    public BlobArray<float3> vcolor_2_Items;
    public BlobArray<float3> vcolor_3_Items;
    public BlobArray<float3> vcolor_4_Items;
    public BlobArray<float3> vcolor_5_Items;
    public BlobArray<float3> vcolor_6_Items;
    public BlobArray<float3> vcolor_7_Items;
    public BlobArray<float3> vcolor_8_Items;

}