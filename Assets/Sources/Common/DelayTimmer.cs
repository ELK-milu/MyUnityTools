using System.Threading.Tasks;
using System;

public class DelayTimmer
{ 
    public static async void StartTimmer(float delay, Action action)
    {
        await Task.Delay((int)(delay * 1000));
        action?.Invoke();
    }
}
