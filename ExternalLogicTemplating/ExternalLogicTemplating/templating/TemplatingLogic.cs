﻿using ExternalLogicTemplating.structure;
using ExternalLogicTemplating.util;
using ExternalLogicTemplating.zip;
using System.Dynamic;

namespace ExternalLogicTemplating.templating {
    internal class TemplatingLogic {
        public TemplatingLogic() {
            // Register handlebar helpers
            TemplatingUtil.registerHelpers();
        }

        public byte[] GenerateZipFiles(ST_Project Project, List<ST_ActionDefinition> Actions, List<ST_StructureDefinition> Structures) {
            byte[] zipFile;

            // Solution File
            string projectGuid = Guid.NewGuid().ToString().ToUpper();
            string solutionGuid = Guid.NewGuid().ToString().ToUpper();
            string projectTypeGuid = "{9A19103F-16F7-4668-BE54-9A1E7A4F7556}";
            ZipManagement zipManagement = new ZipManagement();

            var solutionContext = new {
                ProjectName = Project.ProjectName,
                ProjectGuid = "{" + projectGuid + "}",
                SolutionGuid = "{" + solutionGuid + "}",
                ProjectTypeGuid = projectTypeGuid
            };

            string solutionFile = TemplatingUtil.generateFile(FileToBeProcessed.Solution, solutionContext);


            // Project File
            dynamic projectContext = new ExpandoObject();

            projectContext.Icons = new List<ExpandoObject>();

            foreach (ST_Icon icon in Project.Icons) {
                dynamic i = new ExpandoObject();
                i.FileName = icon.FileName;

                projectContext.Icons.Add(i);
            }

            string projectFile = TemplatingUtil.generateFile(FileToBeProcessed.Project, projectContext);

            // Interface File
            string interfaceFile = GenerateInterfaceFile(Project, Actions);

            // Action File
            string actionFile = GenerateActionFile(Project, Actions);

            //Structures File
            string structureFile = GenerateStructureFile(Project, Structures);

            zipFile = zipManagement.generateZipFiles(Project.ProjectName, solutionFile, actionFile, interfaceFile, structureFile, projectFile, Project.Icons);

            return zipFile;
        }

        /// <summary>
        /// Generates the Interface class file
        /// </summary>
        /// <param name="project">Project data</param>
        /// <param name="Actions">Project actions</param>
        /// <returns>Interface file</returns>
        private string GenerateInterfaceFile(ST_Project project, List<ST_ActionDefinition> Actions) {
            dynamic interfaceContext = new ExpandoObject();

            // Project data
            interfaceContext.ProjectName = project.ProjectName;
            interfaceContext.ProjectDescription = project.ProjectDescription;
            interfaceContext.ProjectAnnotationData = new Dictionary<string, string>();
            interfaceContext.ProjectAnnotationData.Add("Name", "\"" + project.ProjectName + "\"");
            interfaceContext.ProjectAnnotationData.Add("Description", "\"" + project.ProjectDescription + "\"");

            if (!String.IsNullOrEmpty(project.ProjectIcon.FileName)) {
                interfaceContext.ProjectAnnotationData.Add("IconResourceName", $"\"{project.ProjectName}.resources." + project.ProjectIcon.FileName + "\"");
            }

            // Actions
            interfaceContext.Actions = new List<ExpandoObject>();
            foreach (ST_ActionDefinition action in Actions) {
                ST_ActionParameterDefinition outputParameter = Util.getOutputParameter(action);

                dynamic actionData = new ExpandoObject();
                actionData.Name = action.Name;
                actionData.ActionAnnotationData = new Dictionary<string, string>();

                actionData.ActionAnnotationData.Add("Description", "\"" + project.ProjectDescription + "\"");
                if (!String.IsNullOrEmpty(project.ProjectIcon.FileName))
                    actionData.ActionAnnotationData.Add("IconResourceName", $"\"{project.ProjectName}.resources." + project.ProjectIcon.FileName + "\"");

                // If it has output parameter, set its data
                if (outputParameter.Name != null) {
                    actionData.ActionAnnotationData.Add("ReturnDescription", "\"Return Description\"");
                    actionData.ActionAnnotationData.Add("ReturnName", "\"" + outputParameter.Name + "\"");
                    actionData.ActionAnnotationData.Add("ReturnType", Util.getOSDataType(outputParameter.DataType));

                    actionData.Output = Util.getDataType(outputParameter.DataType, outputParameter.RecordDefinition);
                } else {
                    actionData.Output = "void";
                }

                actionData.ActionDefinition = Util.buildActionParameters(action);

                interfaceContext.Actions.Add(actionData);
            }

            string interfaceFile = TemplatingUtil.generateFile(FileToBeProcessed.Interface, interfaceContext);
            return interfaceFile;
        }

        /// <summary>
        /// Generates the Action class file
        /// </summary>
        /// <param name="project">Project data</param>
        /// <param name="Actions">Project actions</param>
        /// <returns>Action file</returns>
        private string GenerateActionFile(ST_Project project, List<ST_ActionDefinition> Actions) {
            dynamic actionContext = new ExpandoObject();

            // Project data
            actionContext.ProjectName = project.ProjectName;

            // Actions
            actionContext.Actions = new List<ExpandoObject>();
            foreach (ST_ActionDefinition action in Actions) {
                ST_ActionParameterDefinition outputParameter = Util.getOutputParameter(action);

                dynamic actionData = new ExpandoObject();
                actionData.Name = action.Name;

                // If it has output parameter, set its data
                if (outputParameter.Name != null) {
                    actionData.Output = Util.getDataType(outputParameter.DataType, outputParameter.RecordDefinition);

                    // Add initial method content
                    actionData.Content = Util.getInitialMethodContentWithReturn(outputParameter);
                } else {
                    actionData.Output = "void";
                }

                actionData.ActionDefinition = Util.buildActionParameters(action);

                actionContext.Actions.Add(actionData);
            }

            string interfaceFile = TemplatingUtil.generateFile(FileToBeProcessed.Action, actionContext);
            return interfaceFile;
        }

        /// <summary>
        /// Generates the Structure class file
        /// </summary>
        /// <param name="project">Project data</param>
        /// <param name="Structures">Project structures</param>
        /// <returns>Structure file</returns>
        private string GenerateStructureFile(ST_Project project, List<ST_StructureDefinition> Structures) {
            dynamic structureContext = new ExpandoObject();

            // Project data
            structureContext.ProjectName = project.ProjectName;

            // Actions
            structureContext.Structures = new List<ExpandoObject>();
            foreach (ST_StructureDefinition structure in Structures) {
                dynamic structureData = new ExpandoObject();
                structureData.Name = structure.Name;
                structureData.Description = "\"" + structure.Description + "\"";

                structureData.Attributes = new List<ExpandoObject>();

                foreach (ST_StructureAttributeDefinition attribute in structure.Attributes) {
                    dynamic attributeData = new ExpandoObject();

                    attributeData.Name = attribute.Name;
                    attributeData.DataType = Util.getDataType(attribute.DataType, attribute.RecordDefinition);


                    // Annotations Data
                    attributeData.AttributeAnnotationData = new Dictionary<string, string>();
                    attributeData.AttributeAnnotationData.Add("DataType", Util.getOSDataType(attribute.DataType));
                    attributeData.AttributeAnnotationData.Add("Description", "\"" + attribute.Description + "\"");
                    attributeData.AttributeAnnotationData.Add("IsMandatory", attribute.IsMandatory.ToString().ToLower());

                    // Decimal
                    if (attribute.DataType == 5) {
                        attributeData.AttributeAnnotationData.Add("Decimals", 8.ToString());
                        attributeData.AttributeAnnotationData.Add("Length", 21.ToString());
                    } else if (attribute.DataType == 8) {
                        attributeData.AttributeAnnotationData.Add("Length", attribute.Length.ToString());
                    }

                    // Add structure attribute
                    structureData.Attributes.Add(attributeData);
                }

                // Add structure
                structureContext.Structures.Add(structureData);
            }

            string structureFile = TemplatingUtil.generateFile(FileToBeProcessed.Structure, structureContext);
            return structureFile;
        }
    }
}