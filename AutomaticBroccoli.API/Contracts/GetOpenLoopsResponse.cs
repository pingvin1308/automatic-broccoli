namespace AutomaticBroccoli.API.Contracts;

public sealed class GetOpenLoopsResponse
{
    public GetOpenLoopDto[] OpenLoops { get; set; }
    public int Total { get; set; }
}