using System.ServiceModel;

namespace ERPService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICurrencyMstService" in both code and config file together.
    [ServiceContract]
    public interface ICurrencyMstService
    {
        [OperationContract]
        string GetCurrencyCodeName(int currencyPk);
    }
}
