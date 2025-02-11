using System.Text;

public class SCreateTemplateScript
{
    private string _nameSpace;
    private string _name;

    public SCreateTemplateScript(string nameScpace, string name)
    {
        _nameSpace = nameScpace;
        _name = name;
    }

    public string GetName()
    {
        return _name;
    }

    public string CreateTemplateControl()
    {
        var writer = new StringBuilder();
        writer.AppendLine("using SFramework;");
        writer.AppendLine("using SFramework.Game;");
        writer.AppendLine();

        writer.AppendLine(
            $"namespace {((string.IsNullOrWhiteSpace(_nameSpace)) ? "Game" : _nameSpace)}");
        writer.AppendLine("{");
        writer.AppendLine($"\tpublic class {_name}Control : RootControl");
        writer.AppendLine("\t{");
        
        writer.AppendLine("\t\tpublic override ViewOpenType GetViewOpenType()");
        writer.AppendLine("\t\t{");
        writer.AppendLine("\t\t\treturn ViewOpenType.Single;");
        writer.AppendLine("\t\t}");
        
        writer.AppendLine("\t\tprotected override void opening()");
        writer.AppendLine("\t\t{");
        writer.AppendLine("\t\t\t// Code Here");
        writer.AppendLine("\t\t}");

        writer.AppendLine("\t\tprotected override void alreadyOpened()");
        writer.AppendLine("\t\t{");
        writer.AppendLine("\t\t\tbase.alreadyOpened();");
        writer.AppendLine("\t\t\t// Code Here");
        writer.AppendLine("\t\t}");

        writer.AppendLine("\t\tprotected override void closing()");
        writer.AppendLine("\t\t{");
        writer.AppendLine("\t\t\t// Code Here");
        writer.AppendLine("\t\t}");

        writer.AppendLine("\t}");
        writer.AppendLine("}");

        return writer.ToString();
    }

    public string CreateTemplateModel()
    {
        var writer = new StringBuilder();
        writer.AppendLine("using SFramework.Game;");
        writer.AppendLine("using Cysharp.Threading.Tasks;");
        writer.AppendLine();

        writer.AppendLine(
            $"namespace {((string.IsNullOrWhiteSpace(_nameSpace)) ? "Game" : _nameSpace)}");
        writer.AppendLine("{");
        writer.AppendLine($"\tpublic class {_name}Model : RootModel");
        writer.AppendLine("\t{");
        writer.AppendLine("\t\tprotected override void opening()");
        writer.AppendLine("\t\t{");
        writer.AppendLine("\t\t\t GetData().Forget();");
        writer.AppendLine("\t\t}");

        writer.AppendLine("\t\tprotected override void closing()");
        writer.AppendLine("\t\t{");
        writer.AppendLine("\t\t\t// Code Here");
        writer.AppendLine("\t\t}");

        writer.AppendLine("\t}");
        writer.AppendLine("}");

        return writer.ToString();
    }

    public string CreateTemplateView(int index)
    {
        var writer = new StringBuilder();
        writer.AppendLine("using UnityEngine;");
        writer.AppendLine("using SFramework;");
        writer.AppendLine("using SFramework.Game;");
        writer.AppendLine("using UnityEngine.UI;");
        writer.AppendLine();

        writer.AppendLine(
            $"namespace {((string.IsNullOrWhiteSpace(_nameSpace)) ? "Game" : _nameSpace)}");
        writer.AppendLine("{");
        switch (index)
        {
            case 0:
                writer.AppendLine($"\tpublic class {_name}View : SSCENEView");
                break;
            case 1:
                writer.AppendLine($"\tpublic class {_name}View : SUIView");
                break;
            default:
                writer.AppendLine($"\tpublic class {_name}View : RootView");
                break;
        }
        
        writer.AppendLine("\t{");
        if (index == 1)
        {
            writer.AppendLine("\t\tprotected override UILayer GetViewLayer()");
            writer.AppendLine("\t\t{");
            writer.AppendLine("\t\t\treturn UILayer.Popup;");
            writer.AppendLine("\t\t}");
        }

        writer.AppendLine("\t\tprotected override void opening()");
        writer.AppendLine("\t\t{");
        writer.AppendLine("\t\t\t// Code Here");
        writer.AppendLine("\t\t}");

        writer.AppendLine("\t\tprotected override void closing()");
        writer.AppendLine("\t\t{");
        writer.AppendLine("\t\t\t// Code Here");
        writer.AppendLine("\t\t}");

        writer.AppendLine("\t}");
        writer.AppendLine("}");

        return writer.ToString();
    }

}
