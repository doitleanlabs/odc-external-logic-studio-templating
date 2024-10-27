using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;

namespace ExternalLogicTemplating.Merge {
    internal class MergeLogic {
        public string MergeActionsForGitHub(string NewFile, string ExistingFile, List<string> methodsToOverride) {
            try {
                SyntaxTree treeExistingFile = CSharpSyntaxTree.ParseText(ExistingFile);
                CompilationUnitSyntax existingRoot = treeExistingFile.GetCompilationUnitRoot();
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

                        List<MethodDeclarationSyntax> updatedMethods = new List<MethodDeclarationSyntax>();

                        // Replace the existing method signatures with the new ones
                        foreach (KeyValuePair<string, MethodDeclarationSyntax> newMethodKvp in newMethods) {
                            MethodDeclarationSyntax existingMethod;
                            if (existingMethods.TryGetValue(newMethodKvp.Key, out existingMethod)) {
                                // Check if the method should be overridden completely
                                if (methodsToOverride.Contains(newMethodKvp.Key)) {
                                    // Replace the entire method with the new one
                                    updatedMethods.Add(newMethodKvp.Value);
                                } else {
                                    // Replace the existing method signature with the new one but keep the existing body
                                    MethodDeclarationSyntax updatedMethod = newMethodKvp.Value.WithBody(existingMethod.Body);
                                    updatedMethods.Add(updatedMethod);
                                }
                            } else {
                                // Add new method if it does not exist in the existing code
                                updatedMethods.Add(newMethodKvp.Value);
                            }
                        }

                        // Preserve any existing methods that are not in the new file
                        foreach (var existingMethod in existingMethods.Values) {
                            if (!newMethods.ContainsKey(existingMethod.Identifier.Text)) {
                                updatedMethods.Add(existingMethod);
                            }
                        }

                        // Replace the existing class node with the updated class node
                        existingClass = existingClass.WithMembers(SyntaxFactory.List<MemberDeclarationSyntax>(updatedMethods));
                        existingRoot = existingRoot.ReplaceNode(existingClasses[newClassKvp.Key], existingClass);
                    } else {
                        // Add new class if it does not exist in the existing code
                        existingRoot = existingRoot.AddMembers(newClassKvp.Value);
                    }
                }

                string mergedFile = System.Net.WebUtility.HtmlDecode(existingRoot.ToFullString());
                return mergedFile;
            } catch (Exception e) {
                return "";
            }
        }

        public string MergeActionsForAI(string NewFile, string ExistingFile) {
            // Parse the existing and new file content into syntax trees
            var existingTree = CSharpSyntaxTree.ParseText(ExistingFile);
            var newTree = CSharpSyntaxTree.ParseText(NewFile);

            // Get the root syntax nodes of both files
            var existingRoot = existingTree.GetRoot();
            var newRoot = newTree.GetRoot();

            // Extract the methods from the new file
            var newMethods = newRoot.DescendantNodes()
                                    .OfType<MethodDeclarationSyntax>()
                                    .ToList();

            // Create a new root for the merged file by copying the existing root
            var mergedRoot = (CompilationUnitSyntax)existingRoot;

            // Extract the class declaration from the existing file
            var existingClass = mergedRoot.DescendantNodes()
                                          .OfType<ClassDeclarationSyntax>()
                                          .FirstOrDefault();

            if (existingClass == null) {
                throw new InvalidOperationException("The existing file does not contain a class definition.");
            }

            // Remove any existing methods that are being updated by new methods
            var existingMethods = existingClass.Members.OfType<MethodDeclarationSyntax>().ToList();
            foreach (var newMethod in newMethods) {
                var methodName = newMethod.Identifier.Text;
                existingMethods = existingMethods
                    .Where(m => m.Identifier.Text != methodName)
                    .ToList();
            }

            // Add the new methods to the existing class
            var updatedClass = existingClass
                .WithMembers(SyntaxFactory.List<MemberDeclarationSyntax>(existingMethods)
                .AddRange(newMethods));

            // Replace the old class declaration with the updated class declaration in the merged root
            mergedRoot = mergedRoot.ReplaceNode(existingClass, updatedClass);

            // Convert the merged syntax tree back into a source code string
            var mergedFileContent = mergedRoot.NormalizeWhitespace().ToFullString();

            return mergedFileContent;
        }
    }
}
