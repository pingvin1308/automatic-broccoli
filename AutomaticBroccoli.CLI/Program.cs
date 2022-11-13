using System.Text;
using AutomaticBroccoli.CLI;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.Unicode;

Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
Console.ReadKey();

var stoppingTokenSource = new CancellationTokenSource();

Console.CancelKeyPress += (sender, e) => stoppingTokenSource.Cancel();

var app = new Application();
app.Run(stoppingTokenSource.Token);
