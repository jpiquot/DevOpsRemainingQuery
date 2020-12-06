namespace DevOpsRemainingQuery.DevOps
{
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using CsvHelper;
    using CsvHelper.Configuration;

    using Microsoft.VisualStudio.Services.WebApi;

    /// <summary>
    /// Export class used to export query data.
    /// </summary>
    internal class Export
    {
        private readonly Query query;

        /// <summary>
        /// Initializes a new instance of the <see cref="Export"/> class.
        /// </summary>
        /// <param name="query">The query to export.</param>
        public Export(Query query)
        {
            this.query = query;
        }

        /// <summary>
        /// Export to a file.
        /// </summary>
        /// <param name="fileName">The output file name.</param>
        /// <param name="withHeaders">Add column headers in the first line of the file.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task ToFile(string fileName, bool withHeaders = true)
        {
            var data = await this.query.GetData();
            var conf = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                Encoding = Encoding.Unicode,
            };
            using var writer = new StreamWriter(fileName);
            using var csv = new CsvWriter(writer, conf);

            if (withHeaders)
            {
                foreach (var field in data.Fields)
                {
                    csv.WriteField(field);
                }

                await csv.NextRecordAsync();
            }

            foreach (var record in data.Records)
            {
                foreach (var value in from field in record
                                      let value = field switch
                                      {
                                          IdentityRef identity => identity.DisplayName,
                                          _ => field
                                      }
                                      select value)
                {
                    csv.WriteField(value);
                }

                await csv.NextRecordAsync();
            }

            await csv.FlushAsync();
        }
    }
}