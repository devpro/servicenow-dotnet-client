using CommandLine;

namespace RabbidsIncubator.ServiceNowClient.ConsoleApp
{
    public class CommandLineOptions
    {
        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool IsVerbose { get; set; }
    }
}
