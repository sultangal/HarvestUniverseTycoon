using Unity.Entities;
using Unity.Transforms;


public partial class CornAspect : IAspect
{
    public readonly Entity Entity;

    private readonly RefRO<LocalTransform> _transform;
    private LocalTransform Transform => _transform.ValueRO;

}
