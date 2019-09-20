namespace Adapters.Common
{
    public interface IBankAdapter: ICourseGetter
    {
        string BankName { get; }
    }
}