using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

public class Unit2 : MonoBehaviour
{
    async void Start()
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        Debug.Log("Start timer");
        Task task1 = Task1(1, cancellationToken);
        Task task2 = Task2(60, cancellationToken);

        await Task.WhenAll(task1, task2);

        //cancellationTokenSource.Cancel(); // что значит - в случае отмены токена
        //cancellationTokenSource.Dispose();

        Debug.Log("end all Tasks");

    }

    private async Task Task1(int seconds, CancellationToken cancellationToken)
    {
        await Task.Delay(seconds * 1000, cancellationToken); 
        Debug.Log("end Task1");
    }

    private async Task Task2(int frames, CancellationToken cancellationToken)
    {
        for (int i = 0; i < frames; i++)
        {
            await Task.Yield();
            Debug.Log("end Task2");
            if (cancellationToken.IsCancellationRequested)            
                return;
        }        
    }
}
