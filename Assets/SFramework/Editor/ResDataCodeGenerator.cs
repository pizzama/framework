using System.CodeDom;
using System.IO;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using SFramework.Tools;

namespace SFramework
{
    public static class ResDataCodeGenerator
    {
        private const string BUNDLENAME = "BundleName";
        private const string PREFIX = "RES_";//using prefix fix the first number character
        public static void WriteClass(TextWriter writer, string ns, string publicClassName)
        {
            var assetBundleInfos = new Dictionary<string, string[]>();

            var allNames = AssetDatabase.GetAllAssetBundleNames();
            foreach (var assetName in allNames)
            {
                var assetPaths = AssetDatabase.GetAssetPathsFromAssetBundle(assetName);
                if (!assetBundleInfos.ContainsKey(assetName))
                {
                    assetBundleInfos[assetName] = assetPaths;
                }
                else
                    Debug.LogWarning("have the same bundle path:" + assetName);
            }

            var compileUnit = new CodeCompileUnit();
            var codeNamespace = new CodeNamespace(ns);
            compileUnit.Namespaces.Add(codeNamespace);

            //create public class
            var publicCodeType = new CodeTypeDeclaration(publicClassName);
            codeNamespace.Types.Add(publicCodeType);

            foreach (var assetBundleInfo in assetBundleInfos)
            {
                var className = assetBundleInfo.Key;
                var bundleName = className.Substring(0, 1).ToLower() + className.Substring(1);
                if (int.TryParse(bundleName[0].ToString(), out _))
                {
                    continue;
                }

                className = className.Substring(0, 1).ToUpper() +
                            className.Substring(1)
                                .RemoveInvalidateChars();

                var codeType = new CodeTypeDeclaration(className);
                codeNamespace.Types.Add(codeType);

                var bundleNameField = new CodeMemberField
                {
                    Attributes = MemberAttributes.Public | MemberAttributes.Const,
                    Name = BUNDLENAME,
                    Type = new CodeTypeReference(typeof(System.String))
                };
                codeType.Members.Add(bundleNameField);
                bundleNameField.InitExpression = new CodePrimitiveExpression(bundleName.ToLowerInvariant());

                var checkRepeatDict = new Dictionary<string, string>();
                foreach (var asset in assetBundleInfo.Value)
                {
                    var assetField = new CodeMemberField
                    { Attributes = MemberAttributes.Const | MemberAttributes.Public };

                    var content = Path.GetFileName(asset);
                    var noContent = Path.GetFileNameWithoutExtension(asset);
                    assetField.Name = PREFIX + content.RemoveInvalidateChars();

                    assetField.Type = new CodeTypeReference(typeof(System.String));
                    if (!assetField.Name.StartsWith("[") && !assetField.Name.StartsWith(" [") &&
                        !checkRepeatDict.ContainsKey(assetField.Name))
                    {
                        checkRepeatDict.Add(assetField.Name, asset);
                        codeType.Members.Add(assetField);
                    }

                    assetField.InitExpression = new CodePrimitiveExpression(noContent);

                    //define public memebers
                    var pubAssetField = new CodeMemberField { Attributes = MemberAttributes.Const | MemberAttributes.Public };
                    pubAssetField.Type = new CodeTypeReference(typeof(System.String));
                    pubAssetField.Name = className + "_" + content.RemoveInvalidateChars();
                    pubAssetField.InitExpression = new CodePrimitiveExpression(bundleName.ToLowerInvariant() + "/" + noContent);
                    publicCodeType.Members.Add(pubAssetField);
                }

                checkRepeatDict.Clear();
            }

            var provider = new CSharpCodeProvider();
            var options = new CodeGeneratorOptions
            {
                BlankLinesBetweenMembers = false,
                BracingStyle = "C"
            };

            provider.GenerateCodeFromCompileUnit(compileUnit, writer, options);
        }
    }
}