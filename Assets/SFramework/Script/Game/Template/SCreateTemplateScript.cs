using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

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
        writer.AppendLine("using UnityEngine;");
        writer.AppendLine("using SFramework;");
        writer.AppendLine();

        writer.AppendLine(
            $"namespace {((string.IsNullOrWhiteSpace(_nameSpace)) ? "Game" : _nameSpace)}");
        writer.AppendLine("{");
        writer.AppendLine($"\tpublic class {_name}Control : SControl");
        writer.AppendLine("\t{");
        writer.AppendLine("\t\tprotected override void opening()");
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
        writer.AppendLine("using UnityEngine;");
        writer.AppendLine("using SFramework;");
        writer.AppendLine();

        writer.AppendLine(
            $"namespace {((string.IsNullOrWhiteSpace(_nameSpace)) ? "Game" : _nameSpace)}");
        writer.AppendLine("{");
        writer.AppendLine($"\tpublic class {_name}Model : SControl");
        writer.AppendLine("\t{");
        writer.AppendLine("\t\tprotected override void opening()");
        writer.AppendLine("\t\t{");
        writer.AppendLine("\t\t\t GetData(\"\").Forget();");
        writer.AppendLine("\t\t}");
        writer.AppendLine("\t}");
        writer.AppendLine("}");

        return writer.ToString();
    }

    public string CreateTemplateView(int index)
    {
        var writer = new StringBuilder();
        writer.AppendLine("using UnityEngine;");
        writer.AppendLine("using SFramework.Game;");
        writer.AppendLine();

        writer.AppendLine(
            $"namespace {((string.IsNullOrWhiteSpace(_nameSpace)) ? "Game" : _nameSpace)}");
        writer.AppendLine("{");
        if(index == 1)
        {
            writer.AppendLine($"\tpublic class {_name}View : SSCENEView");
        }
        else if(index == 2)
        {
            writer.AppendLine($"\tpublic class {_name}View : SUIView");
        }
        else
        {
            writer.AppendLine($"\tpublic class {_name}View : SView");
        }
        writer.AppendLine("\t{");
        writer.AppendLine("\t\tprotected override ViewOpenType GetViewOpenType()");
        writer.AppendLine("\t\t{");
        writer.AppendLine("\t\t\treturn ViewOpenType.Single;");
        writer.AppendLine("\t\t}");

        writer.AppendLine("\t{");
        writer.AppendLine("\t\tprotected override UILayer GetViewLayer()");
        writer.AppendLine("\t\t{");
        writer.AppendLine("\t\t\treturn UILayer.Popup;");
        writer.AppendLine("\t\t}");

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
