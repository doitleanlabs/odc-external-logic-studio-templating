using ExternalLogicTemplating.structure;

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
                default:
                    return "";
            }
        }

        /// <summary>
        /// Return the initial content for the method, with the output declaration and return (if any)
        /// </summary>
        /// <param name="parameter">The parameter</param>
        /// <returns>The initial content for the method, with the output declaration and return (if any)</returns>
        public static string getInitialMethodContentWithReturn(ST_ActionParameterDefinition parameter) {

            switch (parameter.DataType) {
                case 1:
                    return "byte[] output = new byte[] { };" + Environment.NewLine + Environment.NewLine + "            return output;";
                case 2:
                    return "bool output = false;" + Environment.NewLine + Environment.NewLine + "            return output;";
                case 3:
                    return "DateOnly output = new DateOnly();" + Environment.NewLine + Environment.NewLine + "            return output;";
                case 4:
                    return "DateTime output = new DateTime();" + Environment.NewLine + Environment.NewLine + "            return output;";
                case 5:
                    return "decimal output = 0;" + Environment.NewLine + Environment.NewLine + "            return output;";
                case 6:
                    return "int output = 0;" + Environment.NewLine + Environment.NewLine + "            return output;";
                case 7:
                    return "long output = 0;" + Environment.NewLine + Environment.NewLine + "            return output;";
                case 8:
                    return "string output = \"\";" + Environment.NewLine + Environment.NewLine + "            return output;";
                case 9:
                    return parameter.RecordDefinition + " output = new " + parameter.RecordDefinition + "();" + Environment.NewLine + Environment.NewLine + "            return output;";
                case 10:
                    return parameter.RecordDefinition + " output = new " + parameter.RecordDefinition + "();" + Environment.NewLine + Environment.NewLine + "            return output;";
                default:
                    return "";
            }
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
            string actionParametes = string.Join(", ", action.Parameters.Select(obj => $"{getDataType(obj.DataType, obj.RecordDefinition)} {obj.Name}")); ;

            return actionParametes;
        }
    }
}
