using ExternalLogicTemplating.structure;
using ExternalLogicTemplating.action;

namespace ExternalLogicTemplatingTest {
    public class UnitTestExternalLogicTemplating {
        ActionsExternalLogicTemplating logic;

        public UnitTestExternalLogicTemplating() {
            logic = new ActionsExternalLogicTemplating();
        }
        [Fact]
        public void TestFileGeneration() {
            ST_Project project = new ST_Project();
            project.ProjectName = "MyFirstExternalLogic";
            List<ST_Icon> icons = new List<ST_Icon>() {
                new ST_Icon("plus_bold_icon.png", File.ReadAllBytes(@"C:\Users\victo\Documents\Drive DiL\ONE 2024\icons\9021545_plus_bold_icon.png")) {},
                new ST_Icon("minus_bold_icon.png", File.ReadAllBytes(@"C:\Users\victo\Documents\Drive DiL\ONE 2024\icons\9021673_minus_bold_icon.png")) {}
            };
            project.Icons = icons;

            project.ProjectIcon = new ST_Icon("ActionIcon.png", new byte[] { });

            /* ACTIONS */
            List<ST_ActionDefinition> actions = new List<ST_ActionDefinition>();
            ST_ActionDefinition action = new ST_ActionDefinition();
            action.Name = "MyFirstAction";
            action.Description = "Action description";
            action.Icon = new ST_Icon("plus_bold_icon.png", new byte[] { });

            action.Parameters = new List<ST_ActionParameterDefinition>();

            ST_ActionParameterDefinition parameter1 = new ST_ActionParameterDefinition();
            parameter1.Name = "Decimal";
            parameter1.DataType = 5;
            parameter1.IsInput = true;
            action.Parameters.Add(parameter1);

            ST_ActionParameterDefinition parameter3 = new ST_ActionParameterDefinition();
            parameter3.Name = "Parameter3";
            parameter3.DataType = 9;
            parameter3.IsInput = true;
            parameter3.RecordDefinition = "ST_Binary";
            action.Parameters.Add(parameter3);

            ST_ActionParameterDefinition outputParameter = new ST_ActionParameterDefinition();
            outputParameter.Name = "Parameter2";
            outputParameter.DataType = 8;
            outputParameter.IsInput = false;
            action.Parameters.Add(outputParameter);

            actions.Add(action);


            /* STRUCTURES */
            List<ST_StructureDefinition> structures = new List<ST_StructureDefinition>();

            ST_StructureDefinition structureBinary = new ST_StructureDefinition();
            structureBinary.Name = "ST_Binary";
            structureBinary.Description = "Binary structure";
            structureBinary.Attributes = new List<ST_StructureAttributeDefinition>();

            ST_StructureAttributeDefinition binaryAttribute = new ST_StructureAttributeDefinition();
            binaryAttribute.Name = "binaryAttribute";
            binaryAttribute.Description = "Binary Attribute";
            binaryAttribute.IsMandatory = true;
            binaryAttribute.DataType = 1;
            structureBinary.Attributes.Add(binaryAttribute);
            structures.Add(structureBinary);

            ST_StructureDefinition structureDecimal = new ST_StructureDefinition();
            structureDecimal.Name = "ST_Decimal";
            structureDecimal.Description = "Decimal structure";
            structureDecimal.Attributes = new List<ST_StructureAttributeDefinition>();

            ST_StructureAttributeDefinition decimalAttribute = new ST_StructureAttributeDefinition();
            decimalAttribute.Name = "decimalAttribute";
            decimalAttribute.Description = "Decimal Attribute";
            decimalAttribute.IsMandatory = true;
            decimalAttribute.DataType = 5;
            structureDecimal.Attributes.Add(decimalAttribute);
            structures.Add(structureDecimal);



            ST_StructureDefinition structureBoolean = new ST_StructureDefinition();
            structureBoolean.Name = "ST_Boolean";
            structureBoolean.Description = "Boolean structure";
            structureBoolean.Attributes = new List<ST_StructureAttributeDefinition>();

            ST_StructureAttributeDefinition booleanAttribute = new ST_StructureAttributeDefinition();
            booleanAttribute.Name = "booleanAttribute";
            booleanAttribute.Description = "Boolean Attribute";
            booleanAttribute.DataType = 2;
            structureBoolean.Attributes.Add(booleanAttribute);
            structures.Add(structureBoolean);




            ST_StructureDefinition structureWithStructure = new ST_StructureDefinition();
            structureWithStructure.Name = "ST_WithStructure";
            structureWithStructure.Description = "StructureWithStructure structure";
            structureWithStructure.Attributes = new List<ST_StructureAttributeDefinition>();

            ST_StructureAttributeDefinition structureWithStructureAttribute = new ST_StructureAttributeDefinition();
            structureWithStructureAttribute.Name = "structureWithStructureAttribute";
            structureWithStructureAttribute.Description = "StructureWithStructure Attribute";
            structureWithStructureAttribute.DataType = 9;
            structureWithStructureAttribute.RecordDefinition = "ST_Boolean";
            structureWithStructure.Attributes.Add(structureWithStructureAttribute);
            structures.Add(structureWithStructure);





            ST_StructureDefinition structureWithStructureList = new ST_StructureDefinition();
            structureWithStructureList.Name = "ST_WithStructure2";
            structureWithStructureList.Description = "StructureWithStructure structure";
            structureWithStructureList.Attributes = new List<ST_StructureAttributeDefinition>();

            ST_StructureAttributeDefinition structureWithStructureListAttribute = new ST_StructureAttributeDefinition();
            structureWithStructureListAttribute.Name = "structureWithStructureAttribute";
            structureWithStructureListAttribute.Description = "StructureWithStructure Attribute";
            structureWithStructureListAttribute.DataType = 10;
            structureWithStructureListAttribute.RecordDefinition = "ST_Binary";
            structureWithStructureList.Attributes.Add(structureWithStructureListAttribute);
            structures.Add(structureWithStructureList);



            ST_StructureDefinition structureText = new ST_StructureDefinition();
            structureText.Name = "ST_Text";
            structureText.Description = "Text structure";
            structureText.Attributes = new List<ST_StructureAttributeDefinition>();

            ST_StructureAttributeDefinition textAttribute = new ST_StructureAttributeDefinition();
            textAttribute.Name = "textAttribute";
            textAttribute.Description = "Text Attribute";
            textAttribute.DataType = 8;
            textAttribute.Length = 150;
            structureText.Attributes.Add(textAttribute);
            structures.Add(structureText);



            // Call action
            byte[] projectZip = logic.GenerateFiles(project, actions, structures);

            File.WriteAllBytes(@"C:\Users\victo\Documents\Drive DiL\ONE 2024\temp zip files\fromTest.zip", projectZip);

            //logic.ReadXIF(File.ReadAllBytes(@"C:\Users\victo\Documents\Drive DiL\ONE 2024\OS11 Extension\BaseExtension.xif"));

            Assert.Equal(1, 1);
        }

        [Fact]
        public void MergeActionsFileForGitHub() {
            string existingFile = File.ReadAllText(@"C:\Users\victo\Documents\Drive DiL\ONE 2024\temp zip files\MyFirstProject_v29\MyFirstProject\actions\ActionsMyFirstProject.cs", System.Text.Encoding.UTF8);
            string newFile = File.ReadAllText(@"C:\Users\victo\Documents\Drive DiL\ONE 2024\Test Data\New Class with AI.cs", System.Text.Encoding.UTF8);

            List<string> MethodsToOverride = new List<string>();
            MethodsToOverride.Add("SumValues");
            MethodsToOverride.Add("New123");

            string mergedFile = logic.MergeActionsFileForGitHub(newFile, existingFile, MethodsToOverride);

            File.WriteAllText(@"C:\Users\victo\Documents\Drive DiL\ONE 2024\Test Data\MergedActions.cs", mergedFile);

            Assert.Equal(1, 1);
        }

        [Fact]
        public void MergeActionsFileForAI() {
            string newFile = File.ReadAllText(@"C:\Users\victo\Documents\Drive DiL\ONE 2024\Test Data\AI Generated Method.cs", System.Text.Encoding.UTF8);
            string existingFile = File.ReadAllText(@"C:\Users\victo\Documents\Drive DiL\ONE 2024\temp zip files\MyFirstProject_v24\MyFirstProject\actions\ActionsMyFirstProject.cs", System.Text.Encoding.UTF8);

            string mergedFile = logic.MergeActionsFileForAI(newFile, existingFile);

            File.WriteAllText(@"C:\Users\victo\Documents\Drive DiL\ONE 2024\Test Data\AI Generated Method Merged.cs", mergedFile);

            Assert.Equal(1, 1);
        }

        [Fact]
        public void ReadXIF() {
            string XML = logic.ReadXIF(File.ReadAllBytes(@"C:\Users\victo\Documents\Drive DiL\ONE 2024\OS11 Extension\ardoJSON_v1.xif"));

            Console.WriteLine(XML);
        }
    }
}