using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class Main : MonoBehaviour
{
    public int coins;

    void Start()
    {
        NativeArray<int> a = new NativeArray<int>(coins, Allocator.Persistent);
        MyJob myJob = new MyJob();
        myJob.value = 2;
        myJob.coins = a;
        JobHandle job = myJob.Schedule(); // должно выводиться число, но что-то не так и выводиться 0
        job.Complete();
        End(a);
        a.Dispose();
    }
       
    public void End(NativeArray<int> coins)
    {
        for (int i = 0; i < coins.Length; i++)
        {
            Debug.Log(coins[i].ToString());
        }          
    }   
}

public struct MyJob : IJob
{
    public int value;

    public NativeArray<int> coins;

    public void Execute()
    {
        for (int i = 0; i < coins.Length; i++)
        {
            coins[i] *= value;
            Debug.Log($"Job : {coins[i] *= value} ");
        }
    }
}