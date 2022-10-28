using Back.Zone.Monads.IOMonad;

namespace Monads.Test;

public class Program
{
    public static async Task Main(string[] args)
    {
        var service = new TestService();

        var param = Task.FromResult("100");


        Console.WriteLine(result);
    }
}

public sealed class TestService
{
    private async Task<int> AsyncParser(string str) => await Task.FromResult(int.Parse(str));

    public async Task<IO<int>> AsZor(int i) => await IO.PureAsync(Task.FromResult(i)).MapAsync(m => m * 2);

    public async Task<IO<int>> ParseToInt(Task<string> str) =>
        await IO
            .PureAsync(str)
            .MapAsync(AsyncParser);
}