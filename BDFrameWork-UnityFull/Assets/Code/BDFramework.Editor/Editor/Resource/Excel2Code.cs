﻿using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using BDFramework.ResourceMgr;
using LitJson;
using UnityEditor;
using UnityEngine;

namespace BDFramework.Editor
{
    static public class Excel2Code
    {

        public static void GenCode()
        {
            var tablePath = Path.Combine(Application.dataPath, FrameDataHelper.FrameWorkSetting.EditorTablePath);
            var tableDir = Path.GetDirectoryName(tablePath);
            var xlslFiles = Directory.GetFiles(tableDir, "*.xlsx", SearchOption.AllDirectories);

            if (xlslFiles.Length == 0)
            {
                EditorUtility.DisplayDialog("提示","未发现xlsx文件，请注意不是xls" ,"确定");
                return;
            }
            foreach (var f in xlslFiles)
            {
                var excel = new ExcelUtility(f);
                var json = excel.GetJson();
                var statements = excel.GetLine(0);
                Json2Class(f, json, statements);
                Debug.Log("导出：" + f);
                //Json2Class(f, json, statements , true);
            }

            EditorUtility.DisplayDialog("提示","生成完成!" ,"确定");
            AssetDatabase.Refresh();
        }


        private static void Json2Class(string fileName, string json, List<object> statements , bool isForSql =false)
        {
            string className = "";
            if (isForSql)
            {
              className =   Path.GetFileName(fileName).ToLower().Replace(".xlsx", "_SQL");
            }
            else
            {
              className =    Path.GetFileName(fileName).ToLower().Replace(".xlsx", "");
            }
            string outputFile = "";
            if (isForSql)
            {
               outputFile =  fileName.Replace(".xlsx", "_SQL.cs");
            }
            else
            {
               outputFile =  fileName.Replace(".xlsx", ".cs");
            }

            //生成类服务
            CodeCompileUnit compunit = new CodeCompileUnit();
            CodeNamespace sample = new CodeNamespace("Game.Data");
            compunit.Namespaces.Add(sample);
            //引用命名空间
            sample.Imports.Add(new CodeNamespaceImport("System"));
            sample.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
            sample.Imports.Add(new CodeNamespaceImport("Game.Data"));
            //在命名空间下添加一个类
            CodeTypeDeclaration wrapProxyClass = new CodeTypeDeclaration(className);
            //把这个类添加到命名空间 
            sample.Types.Add(wrapProxyClass);
            wrapProxyClass.BaseTypes.Add(new CodeTypeReference("LocalDataBase"));

            //
            var jsonData = JsonMapper.ToObject(json)[0];

            int i = 0;
            foreach (var key in jsonData.Keys)
            {
                if (key.ToLower() == "id"  &&  key != "Id")
                {
                    
                    Debug.LogErrorFormat("<color=yellow>表格{0}字段必须为Id[大小写],请修改后生成</color>" , className);
                    break;
                }
                else if (key == "Id")
                {
                    i++;
                    continue;
                }
                var value = jsonData[key];

                //字段
                CodeMemberField field = new CodeMemberField();
                field.Attributes = MemberAttributes.Private;
                //属性
                CodeMemberProperty property = new CodeMemberProperty();
                property.Attributes = MemberAttributes.Public | MemberAttributes.Final;;
                
                CodeTypeReference type = null;
                if (value.IsArray)
                {
                    if (isForSql)
                    {
                       type = new CodeTypeReference(typeof(string)); 
                    }
                    else
                    {
                        var str = value.ToJson();
                        if (str.IndexOf("\"") > 0)
                        {
                            type = new CodeTypeReference(typeof(List<string>));
                        }
                        else
                        {
                            type = new CodeTypeReference(typeof(List<double>));
                        }
                    }


                }
                else if (value.IsInt) type = new CodeTypeReference(typeof(int));
                else if (value.IsDouble || value.IsLong) type = new CodeTypeReference(typeof(double));
                else if (value.IsBoolean) type = new CodeTypeReference(typeof(bool));
                else if (value.IsString) type = new CodeTypeReference(typeof(string));
                
                 //字段
                field.Type = type;
                field.Name = "_" + key.Trim();
                //属性
                property.Type = type;
                property.Name = key.Trim();
                property.HasGet = true;
                property.HasSet = true;
                
                property.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(),  field.Name)));
                property.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), field.Name), new CodePropertySetValueReferenceExpression()));

                property.Comments.Add(new CodeCommentStatement(statements[i].ToString()));

                //
                wrapProxyClass.Members.Add(field);
                wrapProxyClass.Members.Add(property);
                i++;
            }

            //生成代码       
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            options.BlankLinesBetweenMembers = true;

            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(outputFile))
            {

                provider.GenerateCodeFromCompileUnit(compunit, sw, options);

            }
        }

    }
}