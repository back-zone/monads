using Back.Zone.Monads.EitherMonad;
using Back.Zone.Monads.IOMonad;

namespace Monads.Test;

public class Program
{
    public static async Task Main(string[] args)
    {
        var service = new TestService();

        var param = Task.FromResult("100");

        var zortingen2 =
            IO.From(() => int.Parse("asd"))
                .Map(Right.From<Exception, int>)
                .CheckError(ex => Left.From<Exception, int>(ex));

        Either<string, int> zortingen = new Left<string, int>(2132);

        var result =
            await service
                .ParseToInt(param)
                .FlatMapAsync(service.AsZor)
                .MapAsync(m => m.ToString())
                .CheckErrorAsync(m => m.Message);

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