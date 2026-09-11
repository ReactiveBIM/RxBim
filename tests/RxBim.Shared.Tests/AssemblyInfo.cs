// PluginContext's shared registry requires sequential access, including across test classes.
[assembly: Xunit.CollectionBehavior(DisableTestParallelization = true)]