using System.Threading.Tasks;
using UnityEngine;

public class Unit3 : MonoBehaviour
{
    public Transform[] wayPoint;
    public Transform transformPlayer;
    public bool isActive = true;
    public int a;

    private void Start()
    {
        transformPlayer = GetComponent<Transform>();

        Task task1 = Task1();
    }

    private async Task Move(int seconds)
    {
        for (int i = 0; i < wayPoint.Length; i++)
        {
            transformPlayer = wayPoint[i];
            transform.position = transformPlayer.position;
            Debug.Log($"go to:{wayPoint[i]}");
            await Task.Delay(seconds * 1000);
        }
    }

    private async Task Task1()
    {
        while (isActive)
        {
            await Move(1);
            Debug.Log($"it's:{a + 1}");
            a++;
        }
    }
}