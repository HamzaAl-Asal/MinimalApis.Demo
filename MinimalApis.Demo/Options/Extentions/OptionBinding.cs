namespace MinimalApis.Demo.Options.Extentions
{
    public static class OptionBinding
    {
        public static TOptions GetBindedOptions<TOptions>(IConfiguration configuration, string sectionName)
            where TOptions : new()
        {
            var options = new TOptions();
            configuration.GetSection(sectionName)
                .Bind(options);

            return options;
        }
    }
}