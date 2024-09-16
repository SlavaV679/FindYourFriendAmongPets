namespace FindYourFriendAmongPets.UnitTests;


public class BaseUnitTest
{
    [Fact]
    public async Task SemaphoreSlimTest()
    {
        var semaphoreSlim = new SemaphoreSlim(3);

        var tasks = Enumerable.Range(0, 10).Select(async _ =>
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                await AsyncMethod();
            }
            finally
            {
                semaphoreSlim.Release();
            }
        });

        await Task.WhenAll(tasks);
    }

    private async Task AsyncMethod()
    {
        Console.WriteLine("Before uploading");
        await Task.Delay(2000);
        Console.WriteLine("After uploading");
    }
}