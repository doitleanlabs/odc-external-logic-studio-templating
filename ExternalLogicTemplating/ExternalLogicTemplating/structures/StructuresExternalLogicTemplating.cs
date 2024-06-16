using OutSystems.ExternalLibraries.SDK;

namespace ExternalLogicTemplating.structure {

    [OSStructure(
        Description = "Project"
    )]
    public struct ST_Project {
        [OSStructureField(
            DataType = OSDataType.Text,
            Length = 40,
            Description = "Project name",
            IsMandatory = true
        )]
        public string ProjectName;

        [OSStructureField(
            DataType = OSDataType.Text,
            Length = 2000,
            Description = "Project description",
            IsMandatory = true
        )]
        public string ProjectDescription;

        [OSStructureField(
            DataType = OSDataType.InferredFromDotNetType,
            Length = 2000,
            Description = "External Logic icon",
            IsMandatory = false
        )]
        public ST_Icon ProjectIcon;

        [OSStructureField(
            DataType = OSDataType.InferredFromDotNetType,
            Length = 2000,
            Description = "All icons used by the project",
            IsMandatory = false
        )]
        public List<ST_Icon> Icons;


    }

    [OSStructure(
        Description = "Structure with the definition of an Action"
    )]
    public struct ST_ActionDefinition {
        [OSStructureField(
            DataType = OSDataType.Text,
            Length = 40,
            Description = "Action name",
            IsMandatory = true
        )]
        public string Name;

        [OSStructureField(
            DataType = OSDataType.Text,
            Length = 2000,
            Description = "Action description",
            IsMandatory = false
        )]
        public string Description;

        [OSStructureField(
            DataType = OSDataType.InferredFromDotNetType,
            Description = "Action parameters",
            IsMandatory = true
        )]
        public List<ST_ActionParameterDefinition> Parameters;

        [OSStructureField(
            DataType = OSDataType.InferredFromDotNetType,
            Description = "Action icon",
            IsMandatory = false
        )]
        public ST_Icon Icon;
    }

    [OSStructure(
        Description = "Structure with the definition of a Parameter"
    )]
    public struct ST_ActionParameterDefinition {
        [OSStructureField(
            DataType = OSDataType.Text,
            Length = 40,
            Description = "Action name",
            IsMandatory = true
        )]
        public string Name;

        [OSStructureField(
            DataType = OSDataType.Text,
            Length = 2000,
            Description = "Action description",
            IsMandatory = false
        )]
        public string Description;

        [OSStructureField(
            DataType = OSDataType.Boolean,
            Description = "If the parameter is input or output",
            IsMandatory = false
        )]
        public bool IsInput;

        [OSStructureField(
            DataType = OSDataType.Integer,
            Description = "Parameter data type",
            IsMandatory = true
        )]
        public int DataType;

        [OSStructureField(
            DataType = OSDataType.Text,
            Length = 40,
            Description = "Parameter record definition (when Record or Record List)"
        )]
        public string RecordDefinition;

        [OSStructureField(
            DataType = OSDataType.Integer,
            Description = "Parameter order",
            IsMandatory = true
        )]
        public int Order;
    }

    [OSStructure(
        Description = "Structure with the definition of an Structure"
    )]
    public struct ST_StructureDefinition {
        [OSStructureField(
            DataType = OSDataType.Text,
            Length = 40,
            Description = "Structure name",
            IsMandatory = true
        )]
        public string Name;

        [OSStructureField(
            DataType = OSDataType.Text,
            Length = 2000,
            Description = "Structure description",
            IsMandatory = false
        )]
        public string Description;

        [OSStructureField(
            DataType = OSDataType.InferredFromDotNetType,
            Description = "Structure attributes",
            IsMandatory = true
        )]
        public List<ST_StructureAttributeDefinition> Attributes;
    }

    [OSStructure(
        Description = "Structure with the definition of an Attribute"
    )]
    public struct ST_StructureAttributeDefinition {
        [OSStructureField(
            DataType = OSDataType.Text,
            Length = 40,
            Description = "Action name",
            IsMandatory = true
        )]
        public string Name;

        [OSStructureField(
            DataType = OSDataType.Text,
            Length = 2000,
            Description = "Action description",
            IsMandatory = false
        )]
        public string Description;

        [OSStructureField(
            DataType = OSDataType.Integer,
            Description = "Parameter data type",
            IsMandatory = true
        )]
        public int DataType;

        [OSStructureField(
           DataType = OSDataType.Integer,
           Description = "Attribute length"
        )]
        public int Length;

        [OSStructureField(
            DataType = OSDataType.Boolean,
            Description = "If the attribute is mandatory",
            IsMandatory = true
        )]
        public bool IsMandatory;

        [OSStructureField(
            DataType = OSDataType.Text,
            Length = 40,
            Description = "Parameter record definition (when Record or Record List)"
        )]
        public string RecordDefinition;
    }

    [OSStructure(
        Description = "Icon definition"
    )]
    public struct ST_Icon {
        [OSStructureField(
            DataType = OSDataType.Text,
            Length = 40,
            Description = "Icon file name",
            IsMandatory = true
        )]
        public string FileName;

        [OSStructureField(
            DataType = OSDataType.InferredFromDotNetType,
            Length = 2000,
            Description = "File binary data",
            IsMandatory = true
        )]
        public byte[] File;

        public ST_Icon(string FileName, byte[] File) {
            this.FileName = FileName;
            this.File = File;
        }
    }
}
