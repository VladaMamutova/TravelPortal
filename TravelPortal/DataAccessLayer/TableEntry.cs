namespace TravelPortal.DataAccessLayer
{
    public interface ITableEntry
    {
        int GetId();
        string GetName();
        bool IsReadyToInsert();
        string GetParameterList();
        string GetIdentifiedParameterList();
        bool Equals(ITableEntry record);
    }
}