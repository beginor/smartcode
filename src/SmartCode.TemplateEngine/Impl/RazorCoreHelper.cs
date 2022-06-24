using System;
using System.IO;
using System.Threading.Tasks;
using RazorEngineCore;

namespace SmartCode.TemplateEngine.Impl {

    public static class RazorCoreHelper {

        private static IRazorEngine engine;

        public static void Initialize() {
            engine = new RazorEngine();
        }

        public static Task<string> CompileAndRunAsync(
            string viewPath,
            BuildContext context
        ) {
            if (!File.Exists(viewPath)) {
                throw new FileNotFoundException($"Razor file {viewPath} does not exists!");
            }
            Console.WriteLine($"Compile and run file: {viewPath}");
            var template = engine.Compile<RazorCoreTemplate<BuildContext>>(
                File.ReadAllText(viewPath),
                builder => {
                    var domain = AppDomain.CurrentDomain.BaseDirectory;
                    var dirInfo = new DirectoryInfo(domain);
                    var dllFiles = dirInfo.EnumerateFiles("*.dll");
                    foreach (var dllFile in dllFiles) {
                        var asmName = dllFile.Name.Substring(0, dllFile.Name.Length - dllFile.Extension.Length);
                        builder.AddAssemblyReferenceByName(asmName);
                    }
                }
            );
            return template.RunAsync(instance => {
                instance.ViewPath = viewPath;
                instance.Model = context;
            });
        }

        public static string CompileAndRun(
            string viewPath,
            BuildContext context
        ) {
            var task = CompileAndRunAsync(viewPath, context);
            task.Wait();
            return task.Result;
        }
    }

}
