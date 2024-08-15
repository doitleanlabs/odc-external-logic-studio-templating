using ExternalLogicTemplating.util;
using HandlebarsDotNet;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalLogicTemplating.templating {
    public enum FileToBeProcessed {
        Solution = 0,
        Project = 1,
        Interface = 2,
        Action = 3,
        Structure = 4,
        TestProject = 5,
        TestClass = 6,
        PowerShellScriptFile = 7
    }

    internal class TemplatingUtil {
        private static string TEMPLATE_RESOURCE_PATH = "ExternalLogicTemplating.resources.templates.";

        public static string generateFile(FileToBeProcessed fileToBeProcessed, dynamic data) {
            string templateFileName = "";

            switch (fileToBeProcessed) {
                case FileToBeProcessed.Solution:
                    templateFileName = "Solution.Template_Solution.sln.txt";
                    break;
                case FileToBeProcessed.Project:
                    templateFileName = "Project.Template_Project.csproj.txt";
                    break;
                case FileToBeProcessed.Interface:
                    templateFileName = "Project.Template_Interface.txt";
                    break;
                case FileToBeProcessed.Action:
                    templateFileName = "Project.Template_Action.txt";
                    break;
                case FileToBeProcessed.Structure:
                    templateFileName = "Project.Template_Structure.txt";
                    break;
                case FileToBeProcessed.TestProject:
                    templateFileName = "TestProject.Template_TestProject.csproj.txt";
                    break;
                case FileToBeProcessed.TestClass:
                    templateFileName = "TestProject.Template_TestClass.txt";
                    break;
                case FileToBeProcessed.PowerShellScriptFile:
                    templateFileName = "CompileAndGenerateRelease.txt";
                    break;
            }

            string source = ResourcesUtils.ReadEmbeddedResource(TEMPLATE_RESOURCE_PATH + templateFileName);
            var template = Handlebars.Compile(source);

            return template(data);
        }

        public static void registerHelpers() {
            Handlebars.RegisterHelper("annotationsData", (output, options, context, arguments) => {
                Dictionary<string, string> data = (Dictionary<string, string>)arguments[0];
                var indentationLevel = (int)arguments[1]; // Pass the desired indentation level as an argument
                var indentation = CreateIndentation(indentationLevel);

                foreach (var pair in data) {
                    output.WriteSafeString($"{indentation}{pair.Key} = {pair.Value}");
                    if (pair.Key == data.Keys.Last()) {
                        output.WriteSafeString("\n");
                    } else {
                        output.WriteSafeString(",\n");
                    }
                }
            });
        }

        private static string CreateIndentation(int level, int spacesPerIndent = 4) {
            return new string(' ', level * spacesPerIndent);
        }
    }
}

