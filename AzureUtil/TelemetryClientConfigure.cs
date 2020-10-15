using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using System.Configuration;

namespace AzureUtils
{
    public class TelemetryClientConfigure
    {
        public ApplicationInsightsServiceOptions aiOptions;

        public TelemetryClientConfigure(string instrumentationKey)
        {
            aiOptions = new ApplicationInsightsServiceOptions();
            aiOptions.InstrumentationKey = instrumentationKey;
            setTelemetryClientOptions();
        }
        private void setTelemetryClientOptions()
        {
            // Disables adaptive sampling. 
            aiOptions.EnableAdaptiveSampling = false;

            // Collects Requests Telemetry
            aiOptions.EnableRequestTrackingTelemetryModule = true;
            // よくわからんけど有効
            aiOptions.EnableEventCounterCollectionModule = true;
            // Collects Depdndency Telemetry
            aiOptions.EnableDependencyTrackingTelemetryModule = true;
            // Disables QuickPulse (Live Metrics stream).
            aiOptions.EnableQuickPulseMetricStream = false;
        }
    }
}