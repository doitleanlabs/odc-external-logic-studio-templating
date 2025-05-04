using ExternalLogicTemplating.structure;
using System.Collections.Generic;

namespace ExternalLogicTemplating.util {
    public class Util {
        /// <summary>
        /// Return the output parameter of an action (if any)
        /// </summary>
        /// <param name="action">The action</param>
        /// <returns>The output parameter</returns>
        public static ST_ActionParameterDefinition getOutputParameter(ST_ActionDefinition action) {
            return action.Parameters.Where(item => !item.IsInput).FirstOrDefault<ST_ActionParameterDefinition>();
        }

        /// <summary>
        /// Return the parameter type based on its DataType and RecordDefinition attributes
        /// </summary>
        /// <param name="parameter">The parameter</param>
        /// <returns>The attribute type</returns>
        public static string getDataType(int dataType, string recordDefinition) {

            switch (dataType) {
                case 1:
                    return "byte[]";
                case 2:
                    return "bool";
                case 3:
                    return "DateTime";
                case 4:
                    return "DateTime";
                case 5:
                    return "decimal";
                case 6:
                    return "int";
                case 7:
                    return "long";
                case 8:
                    return "string";
                case 9:
                    return recordDefinition;
                case 10:
                    return "List<" + recordDefinition + ">";
                case 12:
                    return "DateTime";
                default:
                    return "";
            }
        }

        /// <summary>
        /// Return the initial content for the method, with the output parameters initialization
        /// </summary>
        /// <param name="action">The action</param>
        /// <returns>The initial content for the method, with the output declaration and return (if any)</returns>
        public static string getInitialMethodContentWithReturn(ST_ActionDefinition action) {
            string output = "";

            foreach (var item in action.Parameters.Where(item => !item.IsInput))
            {
                output += getOutputInitialization(item);
            }

            return output;
        }

        /// <summary>
        /// Return the initialization content for the output parameter
        /// </summary>
        /// <param name="parameter">The parameter</param>
        /// <returns>The initial content for the method, with the output declaration and return (if any)</returns>
        public static string getOutputInitialization(ST_ActionParameterDefinition parameter)
        {
            string output = parameter.Name;
            switch (parameter.DataType)
            {
                case 1:
                    output += " = new byte[] { };" + Environment.NewLine + Environment.NewLine;
                    break;
                case 2:
                    output += " = false;" + Environment.NewLine + Environment.NewLine;
                    break;
                case 3:
                    output += " = new DateOnly();" + Environment.NewLine + Environment.NewLine;
                    break;
                case 4:
                    output += " = new DateTime();" + Environment.NewLine + Environment.NewLine;
                    break;
                case 5:
                    output += " = 0;" + Environment.NewLine + Environment.NewLine;
                    break;
                case 6:
                    output += " = 0;" + Environment.NewLine + Environment.NewLine;
                    break;
                case 7:
                    output += " = 0;" + Environment.NewLine + Environment.NewLine;
                    break;
                case 8:
                    output += " = \"\";" + Environment.NewLine + Environment.NewLine;
                    break;
                case 9:
                    output += " = new " + parameter.RecordDefinition + "();" + Environment.NewLine + Environment.NewLine;
                    break;
                case 10:
                    output += " = new List<" + parameter.RecordDefinition + ">();" + Environment.NewLine + Environment.NewLine;
                    break;
                case 12:
                    output += " = new DateTime();" + Environment.NewLine + Environment.NewLine;
                    break;
            }

            return output;
        }

        /// <summary>
        /// Return the parameter "ReturnType" value based on its DataType and RecordDefinition attributes
        /// </summary>
        /// <param name="parameter">The parameter</param>
        /// <returns>The ReturnType value</returns>
        public static string getOSDataType(int dataType) {

            switch (dataType) {
                case 1:
                    return "OSDataType.BinaryData";
                case 2:
                    return "OSDataType.Boolean";
                case 3:
                    return "OSDataType.Date";
                case 4:
                    return "OSDataType.DateTime";
                case 5:
                    return "OSDataType.Decimal";
                case 6:
                    return "OSDataType.Integer";
                case 7:
                    return "OSDataType.LongInteger";
                case 8:
                    return "OSDataType.Text";
                case 9:
                    return "OSDataType.InferredFromDotNetType";
                case 10:
                    return "OSDataType.InferredFromDotNetType";
                case 12:
                    return "OSDataType.Time";
                default:
                    return "";
            }
        }

        /// <summary>
        /// Returns the action parameters
        /// </summary>
        /// <param name="action">The action</param>
        /// <returns>The action parameters definition</returns>
        public static string buildActionParameters(ST_ActionDefinition action) {
            //string actionParameters = string.Join(", ", action.Parameters.Where(obj => obj.IsInput).Select(obj => $"{getDataType(obj.DataType, obj.RecordDefinition)} {obj.Name}"));
            string actionParameters = string.Join(", ",
                    action.Parameters
                        .Where(p => p.IsInput)
                        .OrderBy(p => p.Order)
                        .Select(p => $"{getDataType(p.DataType, p.RecordDefinition)} {p.Name}")
                    .Concat(
                        action.Parameters
                            .Where(p => !p.IsInput)
                            .OrderBy(p => p.Order)
                            .Select(p => $"out {getDataType(p.DataType, p.RecordDefinition)} {p.Name}")
                    )
                );

            return actionParameters;
        }
    }
}
