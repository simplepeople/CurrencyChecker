using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adapters.Bps;
using Adapters.Common;
using Adapters.Idea;
using Domain.Models;


namespace CurrencyChecker
{
    public class Program
    {
        static readonly ConsoleWriter _writer = new ConsoleWriter();

        public static async Task Main(string[] args)
        {
            var banksCourses = new IBankAdapter[]
            {
                new BpsBankAdapter(),
                new IdeaBankAdapter()
            };

            var getCoursesTasks = banksCourses.Select(x => x.GetBankCourses()).ToList();

            while (getCoursesTasks.Any())
            {
                var result = await Task.WhenAny(getCoursesTasks);
                getCoursesTasks.Remove(result);
                _writer.Write(await result);
            }
        }

        public class ConsoleWriter
        {
            public void Write(DomainBankCoursesDto dto)
            {
                var builder = new StringBuilder();
                builder.Append(dto.BankCourseTime.ToString("g"));
                builder.Append(" ");
                builder.AppendLine(dto.Bank);
                dto.CurrencyPairInfos.ForEach(x => builder.AppendLine($"{x.XCurrency} buy:{x.BuyCourse} sale:{x.SellCourse}"));
                Console.WriteLine(builder.ToString());
            }
        }
    }
}
