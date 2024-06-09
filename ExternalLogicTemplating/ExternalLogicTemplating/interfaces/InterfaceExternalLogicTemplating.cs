using OutSystems.ExternalLibraries.SDK;
using ExternalLogicTemplating.structure;

namespace ExternalLogicTemplating.interfaces {
    [OSInterface(
       Description = "ExternalLogic code generation",
       IconResourceName = "ExternalLogicTemplating.resources.icons.icon_gear.png",
       Name = "ExternalLogicTemplating"
    )]
    public interface InterfaceExternalLogicTemplating {
        [OSAction(
            Description = "Generates all files for the giving Actions",
            IconResourceName = "ExternalLogicTemplating.resources.icons.icon_generate.png",
            ReturnDescription = "Binary with the zip of the source code",
            ReturnName = "SourceCode",
            ReturnType = OSDataType.InferredFromDotNetType
        )]
        byte[] GenerateFiles(ST_Project Project, List<ST_ActionDefinition> Actions, List<ST_StructureDefinition> Structures);

        [OSAction(
            Description = "Check if a file is a text file",
            IconResourceName = "ExternalLogicTemplating.resources.icons.icon_magnifier.png",
            ReturnDescription = "True if the file is a text file",
            ReturnName = "IsTextFile",
            ReturnType = OSDataType.Boolean
        )]
        bool IsBinaryTextFile(byte[] File);

        [OSAction(
            Description = "Merge 2 cs files (current code + updated code) keeping current methods logic but updating methods signature",
            IconResourceName = "ExternalLogicTemplating.resources.icons.icon_merge.png",
            ReturnDescription = "New merged file",
            ReturnName = "MergedFile",
            ReturnType = OSDataType.Text
        )]
        string MergeActionFiles(string NewFile, string ExistingFile);

        [OSAction(
            Description = "Read  XIF file to get the Actions and Structures",
            IconResourceName = "ExternalLogicTemplating.resources.icons.icon_extension.png",
            ReturnDescription = "XIF in JSON format",
            ReturnName = "XifJson",
            ReturnType = OSDataType.Text
        )]
        string ReadXIF(byte[] XifFile);
    }
}
