using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class Main2 : MonoBehaviour
{
    public Vector3[] pos;
    public NativeArray<Vector3>[] FinalPositions;

    private void Start()
    {
        NativeArray<Vector3> V1 = new NativeArray<Vector3>(pos, Allocator.Persistent);

        NativeArray<Vector3> V2 = new NativeArray<Vector3>(FinalPositions[pos.Length], Allocator.Persistent);

        MyJobParallelFor myJobParallel = new MyJobParallelFor();
        myJobParallel.Positions = V1;
        myJobParallel.Velocities = V1;

        myJobParallel.Positions = FinalPositions[0];
        myJobParallel.Velocities = FinalPositions[1];
        JobHandle jobHandle2 = myJobParallel.Schedule(FinalPositions.Length, 3);


        jobHandle2.Complete();

        End(V1);

        End(V2);
        V2.Dispose();
    }

    public void End(NativeArray<Vector3> vectors) 
    {
        for (int i = 0; i < vectors.Length; i++)
        {
            Debug.Log(vectors[i].ToString());
        }
    }


    public struct MyJobParallelFor : IJobParallelFor
    {
        public NativeArray<Vector3> Positions;
        public NativeArray<Vector3> Velocities;
                
        public void Execute(int index)
        {
            Positions[index] = Positions[0];
            Velocities[index] = Velocities[0];
            Debug.Log($"job : pos {Positions}, vel {Velocities}");
        }
    }   
}
