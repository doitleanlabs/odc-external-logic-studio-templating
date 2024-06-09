using System.Reflection;

namespace ExternalLogicTemplating.util {
    public class ResourcesUtils {
        public static string ReadEmbeddedResource(string resourceName) {
            // Get the current assembly
            Assembly assembly = Assembly.GetExecutingAssembly();

            // Read the resource
            using (Stream stream = assembly.GetManifestResourceStream(resourceName)) {
                if (stream == null) {
                    return "Resource not found.";
                }

                using (StreamReader reader = new StreamReader(stream)) {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
