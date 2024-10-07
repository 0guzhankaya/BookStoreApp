using BookStore.API.Utilities.Formatters;

namespace BookStore.API.Extensions
{
    public static class IMvcBuilderExtensions
    {
        public static IMvcBuilder AddCustomCsvFormetter(this IMvcBuilder builder) => builder.AddMvcOptions(config =>
            config.OutputFormatters.Add(new CsvOutputFormatter()));
    }
}
