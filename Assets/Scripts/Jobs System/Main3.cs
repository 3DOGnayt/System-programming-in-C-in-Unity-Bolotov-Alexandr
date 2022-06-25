using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class Main3 : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private int _count;
        
    [SerializeField] private float _speed;

    [SerializeField] private float startDistance;
    [SerializeField] private float startVelocity;
    [SerializeField] private float startMass;

    private NativeArray<Vector3> positions;
    private NativeArray<Vector3> velocities;
    private NativeArray<Vector3> accelerations;
    private NativeArray<float> masses;


    private TransformAccessArray transformAccessArray;

    private void Start()
    {
        positions = new NativeArray<Vector3>(_count, Allocator.Persistent);
        velocities = new NativeArray<Vector3>(_count, Allocator.Persistent);
        accelerations = new NativeArray<Vector3>(_count, Allocator.Persistent);
        masses = new NativeArray<float>(_count, Allocator.Persistent);

        Transform[] transforms = new Transform[_count];
        for (int i = 0; i < _count; i++)
        {
            positions[i] = Random.insideUnitSphere * Random.Range(0, startDistance);
            velocities[i] = Random.insideUnitSphere * Random.Range(0, startVelocity);
            accelerations[i] = new Vector3();
            masses[i] = Random.Range(1, startMass);
            transforms[i] =  Instantiate(_gameObject).transform;
        }
        transformAccessArray = new TransformAccessArray(transforms);
    }

    private void Update()
    {
        GravitationJob gravitationJob = new GravitationJob()
        {
            Positions = positions,
            Velocities = velocities,
            Accelerations = accelerations,
            Masses = masses,
            GravitationModifier = _speed,
            DeltaTime = Time.deltaTime
        };
        JobHandle gravitationHandle =
        gravitationJob.Schedule(_count, 0);
        MyJobTransform moveJob = new MyJobTransform()
        {
            Positions = positions,
            Velocities = velocities,
            Accelerations = accelerations,
            DeltaTime = Time.deltaTime
        };
        JobHandle moveHandle = moveJob.Schedule(transformAccessArray,
        gravitationHandle);
        moveHandle.Complete();
    }

    private void OnDestroy()
    {
        positions.Dispose();
        velocities.Dispose();
        accelerations.Dispose();
        masses.Dispose();
        transformAccessArray.Dispose();
    }


    public struct GravitationJob : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<Vector3> Positions;
        [ReadOnly]
        public NativeArray<Vector3> Velocities;
        public NativeArray<Vector3> Accelerations;
        [ReadOnly]
        public NativeArray<float> Masses;
        [ReadOnly]
        public float GravitationModifier;
        [ReadOnly]
        public float DeltaTime;

        public void Execute(int index)
        {
            for (int i = 0; i < Positions.Length; i++)
            {
                if (i == index) continue;
                float distance = Vector3.Distance(Positions[i], Positions[index]);
                Vector3 direction = Positions[i] - Positions[index];
                Vector3 gravitation = (direction * Masses[i] * GravitationModifier) / (Masses[index] * Mathf.Pow(distance, 2));
                Accelerations[index] += gravitation * DeltaTime;
            }
        }
    }


    public struct MyJobTransform : IJobParallelForTransform
    {
        public NativeArray<Vector3> Positions;
        public NativeArray<Vector3> Velocities;
        public NativeArray<Vector3> Accelerations;
        [ReadOnly]
        public float DeltaTime;

        public void Execute(int index, TransformAccess transform)
        {
            Vector3 velocity = Velocities[index] + Accelerations[index];
            transform.position += velocity * DeltaTime;
            Positions[index] = transform.position;
            Velocities[index] = velocity;
            Accelerations[index] = Vector3.zero;
        }


        //public float speed;
        //public float deltaTime;

        //public Vector3 direction;

        //public void Execute(int index, TransformAccess transform)
        //{
        //    transform.position += direction.normalized * speed * deltaTime;
        //}
    }
}
