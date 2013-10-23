using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Content;

namespace xnb2png {
    class Program {
	
	//TODO: Add correct output for subdirectories.

        static LaunchParameters GetLaunchParameters(string[] args) {
            LaunchParameters parameters = new LaunchParameters();
            for (int i = 0; i < args.Length; ++i) {
                if (Utils.CheckParameter(args[i], new string[] { "/nologo", "--no-logo" })) {
                    parameters["NoLogo"] = "true";
                    continue;
                }
                if (Utils.CheckParameter(args[i], new string[] { "/i", "-i", "--input-directory" })) {
                    if (i + 1 >= args.Length) {
                        throw new Exception("Invalid parameter value for input directory.");
                    }
                    if (Utils.CheckDirectory(args[i + 1], false)) {
                        parameters["InputDirectory"] = args[++i];
                    } else {
                        throw new Exception(String.Format("Directory \"{0}\" doesn't exists.", args[i + 1]));
                    }
                    continue;
                }
                if (Utils.CheckParameter(args[i], new string[] { "/o", "-o", "--output-directory" })) {
                    if (i + 1 >= args.Length) {
                        throw new Exception("Invalid parameter value for output directory.");
                    }
                    if (Utils.CheckDirectory(args[i + 1], true)) {
                        parameters["OutputDirectory"] = args[++i];
                    } else {
                        throw new Exception(String.Format("Cannot create directory \"{0}\".", args[i + 1]));
                    }
                    continue;
                }
                if (Utils.CheckParameter(args[i], new string[] { "/r", "-r", "--recursive" })) {
                    parameters["Recursive"] = "true";
                    continue;
                }
            }
            return parameters;
        }

        static void Logo() {
            String name = Assembly.GetExecutingAssembly().GetName().Name;
            String version = Assembly.GetExecutingAssembly().GetName().Version.Major.ToString() + "." + Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString();
            Console.WriteLine(name + " " + version);
            object[] copyright = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
            if (copyright.Length != 0) {
                Console.WriteLine(((AssemblyCopyrightAttribute)copyright[0]).Copyright);
            }
        }

        static void PrintUsage() {
            Console.WriteLine("Usage:");
            Console.WriteLine(Assembly.GetExecutingAssembly().GetName().Name + " [flags] /i input_directory /o output_directory");
            Console.WriteLine("\t/nologo, --no-logo           Removes logo displaying.");
            Console.WriteLine("\t/r, -r, --recursive          Process directories recursively.");
            Console.WriteLine("\t/o, -o, --output-directory   Sets output directory.");
            Console.WriteLine("\t/i, -i, --input-directory    Sets input directory.");
        }

        static LaunchParameters parameters;

        static void Main(string[] args) {

            try {
                parameters = GetLaunchParameters(args);
                if (!parameters["NoLogo"].Equals("true")) {
                    Logo();
                }
                if (args.Length == 0) {
                    PrintUsage();
                }
                if (parameters["InputDirectory"].Equals(String.Empty) || parameters["OutputDirectory"].Equals(String.Empty)) {
                    PrintUsage();
                }

                Prepare();

                DirectoryListBuilder list = new DirectoryListBuilder(parameters["InputDirectory"], "*.xnb", parameters["Recursive"].Equals("true"));
                string p = parameters["InputDirectory"];
                if (p[p.Length - 1] != '\\') {
                    p += '\\';
                }
                int counter = 0;
                int count = list.Files.Count;
                foreach (string file in list.Files) {
                    string f = file.Substring(p.Length);
                    f = f.Substring(0, f.Length - 4);
                    ProcessFile(f, parameters["OutputDirectory"]);

                    ++counter;
                    if (counter % 10 == 0) {
                        Console.Write(String.Format("{0:##.##} %\r", ((float)counter / (float)count * 100.0f)));
                    }
                }

                Shutdown();
            } catch (Exception e) {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        static ContentManager mgr;

        static void Prepare() {
            mgr = new ContentManager(new CustomServiceProvider(), parameters["InputDirectory"]);
        }

        static void Shutdown() {
            mgr.Dispose();
        }

        static void ProcessFile(string fileName, string outputDir) {
            Texture2D tex;
            try {
                tex = mgr.Load<Texture2D>(fileName);
                string targetDir = parameters["OutputDirectory"];
                if (targetDir[targetDir.Length - 1] != '\\') {
                    targetDir += '\\';
                }
                string subdir = fileName.Substring(0, fileName.LastIndexOf('\\'));
                Utils.CheckDirectory(targetDir + subdir, true);
                string target = targetDir + fileName + ".png";
                tex.SaveAsPng(new FileStream(target, FileMode.Create), tex.Width, tex.Height);
                mgr.Unload();
                tex.Dispose();
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }
    }
}
