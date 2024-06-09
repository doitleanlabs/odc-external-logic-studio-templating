using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;

namespace ExternalLogicTemplating.Merge {
    internal class MergeLogic {
        public string Merge(string NewFile, string ExistingFile) {
            SyntaxTree treeOldFile = CSharpSyntaxTree.ParseText(ExistingFile);
            CompilationUnitSyntax existingRoot = treeOldFile.GetCompilationUnitRoot();
            Dictionary<string, ClassDeclarationSyntax> existingClasses = existingRoot.DescendantNodes().OfType<ClassDeclarationSyntax>().ToDictionary(c => c.Identifier.Text);

            SyntaxTree treeNewFile = CSharpSyntaxTree.ParseText(NewFile);
            CompilationUnitSyntax newRoot = treeNewFile.GetCompilationUnitRoot();
            Dictionary<string, ClassDeclarationSyntax> newClasses = newRoot.DescendantNodes().OfType<ClassDeclarationSyntax>().ToDictionary(c => c.Identifier.Text);

            // Iterate over each class in the new code
            foreach (KeyValuePair<string, ClassDeclarationSyntax> newClassKvp in newClasses) {
                ClassDeclarationSyntax existingClass;
                // Check if the class exists in the existing code
                if (existingClasses.TryGetValue(newClassKvp.Key, out existingClass)) {
                    ClassDeclarationSyntax newClass = newClassKvp.Value;

                    // Create dictionaries of methods from the existing and new classes
                    Dictionary<string, MethodDeclarationSyntax> newMethods = newClass.Members.OfType<MethodDeclarationSyntax>().ToDictionary(m => m.Identifier.Text);

                    Dictionary<string, MethodDeclarationSyntax> existingMethods = existingClass.Members.OfType<MethodDeclarationSyntax>().ToDictionary(m => m.Identifier.Text);

                    // Replace the existing method signatures with the new ones
                    foreach (KeyValuePair<string, MethodDeclarationSyntax> newMethodKvp in newMethods) {
                        MethodDeclarationSyntax existingMethod;
                        if (existingMethods.TryGetValue(newMethodKvp.Key, out existingMethod)) {
                            // Replace the existing method signature with the new one but keep the existing body
                            MethodDeclarationSyntax updatedMethod = newMethodKvp.Value.WithBody(existingMethod.Body);
                            existingClass = existingClass.ReplaceNode(existingMethod, updatedMethod);
                        } else {
                            // Add new method if it does not exist in the existing code
                            existingClass = existingClass.AddMembers(newMethodKvp.Value);
                        }
                    }

                    // Replace the existing class node with the updated class node
                    existingRoot = existingRoot.ReplaceNode(existingClasses[newClassKvp.Key], existingClass);
                } else {
                    // Add new class if it does not exist in the existing code
                    existingRoot = existingRoot.AddMembers(newClassKvp.Value);
                }
            }

            string mergedFile = System.Net.WebUtility.HtmlDecode(existingRoot.ToFullString());
            return mergedFile;
        }
    }
}
