﻿using ExternalLogicTemplating.structure;
using System.IO.Compression;
using System.Text;

namespace ExternalLogicTemplating.zip {
    public class ZipManagement {
        public byte[] generateZipFiles(string projectName, string snlFileContent, string actionFileContent, string interfaceFileContent, string structureFileContent, string projectFileContent, List<ST_Icon> icons) {
            using (var memoryStream = new MemoryStream()) {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true)) {
                    // Add the .snl file
                    AddTextFileToArchive(archive, $"{projectName}.sln", snlFileContent);

                    // Create the ProjectName folder
                    var projectFolder = archive.CreateEntry($"{projectName}/");

                    // Add files to the action folder
                    AddTextFileToArchive(archive, $"{projectName}/action/Action{projectName}.cs", actionFileContent);

                    // Add files to the interface folder
                    AddTextFileToArchive(archive, $"{projectName}/interface/Interface{projectName}.cs", interfaceFileContent);

                    foreach (ST_Icon icon in icons) {
                        // Add files to the resources folder
                        AddBinaryFileToArchive(archive, $"{projectName}/resources/{icon.FileName}", icon.File);
                    }

                    // Add files to the structure folder
                    AddTextFileToArchive(archive, $"{projectName}/structure/Structure{projectName}.cs", structureFileContent);

                    // Add the project file
                    AddTextFileToArchive(archive, $"{projectName}/{projectName}.csproj", projectFileContent);
                }

                return memoryStream.ToArray();

                // Write the zip file to disk (or any other stream)
                //using (var fileStream = new FileStream("ProjectName.zip", FileMode.Create)) {
                //    memoryStream.Seek(0, SeekOrigin.Begin);
                //    memoryStream.CopyTo(fileStream);
                //}
            }
        }
        private static void AddTextFileToArchive(ZipArchive archive, string filePath, string content) {
            var entry = archive.CreateEntry(filePath);
            using (var entryStream = entry.Open())
            using (var streamWriter = new StreamWriter(entryStream, Encoding.UTF8)) {
                streamWriter.Write(content);
            }
        }

        private static void AddBinaryFileToArchive(ZipArchive archive, string filePath, byte[] content) {
            var entry = archive.CreateEntry(filePath);
            using (var entryStream = entry.Open()) {
                entryStream.Write(content, 0, content.Length);
            }
        }
    }
}