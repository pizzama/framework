using System.IO;
using UnityEngine;

[CreateAssetMenu(menuName = "SFramework/SFRameworkSO", fileName = "SFRameworkSO")]
public class SFRameworkSO : ScriptableObject
{
    [SerializeField] private bool _isSimulator;
    [SerializeField] private string _controlTemplate;
    [SerializeField] private string _modelTemplate;
    [SerializeField] private string _viewTemplate;

    public string GetControlTemplate()
    {
        return _controlTemplate;
    }

    public string GetModelTemplate()
    {
        return _modelTemplate;
    }

    public string GetViewTemplate()
    {
        return _viewTemplate;
    }
}
