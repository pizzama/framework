namespace PFramework
{
    public interface IBundle
    {
        void Install();
        void Open();
        void Uninstall();
        void Refresh();
        void Close();
    }
}