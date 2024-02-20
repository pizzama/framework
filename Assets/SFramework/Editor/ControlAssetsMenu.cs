using System;
using UnityEditor;
using UnityEngine;
using SFramework.Extension;
using SFramework.Tools;
using System.Collections.Generic;
using System.CodeDom;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.IO;

namespace SFramework
{
	[InitializeOnLoad]
	public class ControlAssetsMenu
    {
        private const string Mark_MenuName = "Assets/SFramework/Collect All Controls";
		private const string StaticControlNameSpace = "SFramework.Statics";
		private const string StaticControlClassName = "SFStaticsControl";

        [MenuItem(Mark_MenuName)]
		public static void MarkPTABDir()
		{
			var compileUnit = new CodeCompileUnit();
			//add name space
			var codeNamespace = new CodeNamespace(StaticControlNameSpace);
			compileUnit.Namespaces.Add(codeNamespace); 

			//add class
			var codeType = new CodeTypeDeclaration(StaticControlClassName);
			codeNamespace.Types.Add(codeType);

			//add member
			List<Type> controls = ReflectionTools.GetTypesFormTypeWithAllAssembly(typeof(SControl));
			for (int i = 0; i < controls.Count; i++)
			{
				Type cType = controls[i];
				var bundleNameField = new CodeMemberField
				{
					Attributes = MemberAttributes.Public | MemberAttributes.Const,
					Name = cType.FullName.RemoveInvalidateChars(),
					Type = new CodeTypeReference(typeof(System.String))
				};
				bundleNameField.InitExpression = new CodePrimitiveExpression(cType.FullName);
				codeType.Members.Add(bundleNameField);
			}

			var provider = new CSharpCodeProvider();
			var options = new CodeGeneratorOptions
			{
				BlankLinesBetweenMembers = false,
				BracingStyle = "C"
			};

			var path = Path.GetFullPath(Application.dataPath + Path.DirectorySeparatorChar + "SFStaticAsset");
			if (!Directory.Exists(path)) Directory.CreateDirectory(path);
			path = path + "/" + StaticControlClassName + ".cs";
			var writer = new StreamWriter(File.Open(path, FileMode.Create));

			provider.GenerateCodeFromCompileUnit(compileUnit, writer, options);
			writer.Close();
			AssetDatabase.Refresh();

		}


	}
}
