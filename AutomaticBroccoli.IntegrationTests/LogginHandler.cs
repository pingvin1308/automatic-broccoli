using Xunit.Abstractions;

namespace AutomaticBroccoli.IntegrationTests
{
    internal class LoggingHandler : DelegatingHandler
    {
        private ITestOutputHelper _outputHelper;

        public LoggingHandler(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken);
        }
    }
}