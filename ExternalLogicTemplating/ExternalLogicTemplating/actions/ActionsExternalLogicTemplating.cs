using ExternalLogicTemplating.interfaces;
using ExternalLogicTemplating.Merge;
using ExternalLogicTemplating.structure;
using ExternalLogicTemplating.templating;
using ExternalLogicTemplating.XIF;
using System.Text.RegularExpressions;

namespace ExternalLogicTemplating.action {
    public class ActionsExternalLogicTemplating : InterfaceExternalLogicTemplating {
        /*
         * 
         * ProjectName.snl
         * ProjectName
         *   action
         *     InterfaceProjectName.cs
         *   interface
         *   resources
         *   structure
         *   ProjectName.csproj
         */
        public ActionsExternalLogicTemplating() {

        }

        public byte[] GenerateFiles(ST_Project Project, List<ST_ActionDefinition> Actions, List<ST_StructureDefinition> Structures) {
            byte[] zipFile = new byte[0];

            TemplatingLogic templatingLogic = new TemplatingLogic();

            zipFile = templatingLogic.GenerateZipFiles(Project, Actions, Structures);

            return zipFile;
        }

        public bool IsBinaryTextFile(byte[] File) {
            const int MaxControlCharactersPercentage = 10;

            int controlCharacterCount = 0;
            int totalCharacters = Math.Min(File.Length, 1024); // Check only the first 1024 bytes

            for (int i = 0; i < totalCharacters; i++) {
                byte b = File[i];
                if (b < 32 && b != 9 && b != 10 && b != 13) // Allow tab, newline, and carriage return
                {
                    controlCharacterCount++;
                }
            }

            double controlCharacterPercentage = (double)controlCharacterCount / totalCharacters * 100;
            return controlCharacterPercentage < MaxControlCharactersPercentage;
        }

        public string MergeActionsFileForGitHub(string NewFile, string ExistingFile, List<string> MethodsToOverride) {
            MergeLogic mergeLogic = new MergeLogic();

            string mergedFile = mergeLogic.MergeActionsForGitHub(NewFile, ExistingFile, MethodsToOverride);

            return mergedFile;
        }

        public string MergeActionsFileForAI(string NewFile, string ExistingFile) {
            MergeLogic mergeLogic = new MergeLogic();

            string mergedFile = mergeLogic.MergeActionsForAI(NewFile, ExistingFile);

            return mergedFile;
        }

        public string ReadXIF(byte[] XifFile) {
            XIFParser XIFParser = new XIFParser();

            string xifJson = XIFParser.parseXIF(XifFile);

            return xifJson;
        }
    }
}
