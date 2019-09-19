using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BPSTests.Requests.GetCourses;
using BPSTests.Requests.GetCourses.BPS;

namespace BPSTests
{
    public class Program
    {
        static readonly ConsoleWriter _writer = new ConsoleWriter();

        public static async Task Main(string[] args)
        {
            var banksCourses = new ICourseGetter[]
            {
                new BPSCourseGetter()
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
