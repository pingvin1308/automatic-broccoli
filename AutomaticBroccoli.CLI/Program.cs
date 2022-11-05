using System.Text;
using AutomaticBroccoli.CLI;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.Unicode;

var stoppingTokenSource = new CancellationTokenSource();

Console.CancelKeyPress += (sender, e) => stoppingTokenSource.Cancel();

var app = new Application();
app.Run(stoppingTokenSource.Token);
