namespace PFramework
{
    public class RootUIModel : PModel
    {
        public override void Open()
        {
            // GetData("");
            ModelCallback?.Invoke();            
        }
    }
}