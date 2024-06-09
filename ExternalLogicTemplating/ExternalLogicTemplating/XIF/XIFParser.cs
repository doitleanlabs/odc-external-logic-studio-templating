using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OutSystems.Model.Implementation.Extensions.Xif;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ExternalLogicTemplating.XIF {
    internal class XIFParser {
        public string parseXIF(byte[] xifFile) {
            XDocument xif = XifUtils.ReadXif(xifFile);

            var technologiesNode = xif.Descendants("Technologies").FirstOrDefault();
            if (technologiesNode != null) {
                technologiesNode.Remove();
            }

            // Remove Entities node
            var entitiesNode = xif.Descendants("Entities").FirstOrDefault();
            if (entitiesNode != null) {
                entitiesNode.Remove();
            }

            // Convert XML to JSON
            var json = ParseElement(xif.Root);

            string modifiedJson = TransformJson(JsonConvert.SerializeObject(json, Formatting.Indented));

            return modifiedJson;
        }

        private object ParseElement(XElement element) {
            var childElements = element.Elements().ToList();
            if (!childElements.Any()) {
                return element.Value;
            }

            var elementDictionary = new Dictionary<string, object>();
            foreach (var childElement in childElements) {
                if (childElement.Name.LocalName == "Empty" || childElement.Name.LocalName == "Integer") {
                    continue; // Skip elements that don't have direct representation in JSON
                }

                if (elementDictionary.ContainsKey(childElement.Name.LocalName)) {
                    var existingValue = elementDictionary[childElement.Name.LocalName];
                    if (existingValue is List<object> existingList) {
                        existingList.Add(ParseElement(childElement));
                    } else {
                        elementDictionary[childElement.Name.LocalName] = new List<object> { existingValue, ParseElement(childElement) };
                    }
                } else {
                    elementDictionary[childElement.Name.LocalName] = ParseElement(childElement);
                }
            }

            foreach (var attribute in element.Attributes()) {
                elementDictionary[attribute.Name.LocalName] = attribute.Value;
            }

            return elementDictionary;
        }

        /*
         * For single attribute entities the node Structures.Structure.Attribute is an object, and not a list. 
         * THis method transforms the JSON to always have a list in that node
         */
        public static string TransformJson(string json) {
            JObject jsonObject = JObject.Parse(json);

            JToken structures = jsonObject["Structures"];
            if (structures != null) {
                JArray structureArray = (JArray)structures["Structure"];
                if (structureArray != null) {
                    foreach (JToken structure in structureArray) {
                        JToken attributes = structure["Attribute"];
                        if (attributes != null) {
                            if (attributes.Type == JTokenType.Object) {
                                // Convert object to list with single item
                                structure["Attribute"] = new JArray(attributes);
                            }
                            // If already a list, do nothing
                        }
                    }
                }
            }

            return jsonObject.ToString();
        }
    }
}
