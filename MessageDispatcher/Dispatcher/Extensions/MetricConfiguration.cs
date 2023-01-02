using App.Metrics;

namespace Dispatcher.Extensions;

public static class MetricConfiguration
{
    public static IHostBuilder ConfigureMetric(this IHostBuilder Builder)
    {
        var metrics = new MetricsBuilder()
            .OutputMetrics.AsPrometheusPlainText()
            .Build();

        // _ = Builder.ConfigureMetrics(metrics)
        //     .UseMetrics();

        return Builder;
    }
}