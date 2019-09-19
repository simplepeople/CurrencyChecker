namespace BPSTests.Requests.GetCourses.BPS
{
    public class CardlessRequest : IRequest
    {
        public string type { get; } = "CARD";
        public string date { get; set; }
    }
}
