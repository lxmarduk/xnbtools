using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace xnb2png {
    public class DirectoryListBuilder {
        string path;
        string mask;
        bool recursive;
        List<string> filesCatalog;

        public List<string> Files {
            get {
                return filesCatalog;
            }
        }

        public DirectoryListBuilder(string path, string mask, bool recursive) {
            this.path = path;
            this.mask = mask;
            this.recursive = recursive;
            filesCatalog = new List<string>();
            if (recursive) {
                List<string> directories = buildDirectoryIndex(path, recursive);
                foreach (string dir in directories) {
                    filesCatalog.AddRange(buildFileIndex(dir, mask));
                }
            } else {
                filesCatalog.AddRange(buildFileIndex(path, mask));
            }
        }

        private List<string> buildDirectoryIndex(string path, bool recursive) {
            DirectoryInfo dir = new DirectoryInfo(path);
            List<string> result = new List<string>();
            if (dir.Exists) {
                DirectoryInfo[] directories = dir.GetDirectories();
                if (directories.Length > 0) {
                    for (int i = 0; i < directories.Length; ++i) {
                        //result.Add(directories[i].FullName);
                        if (recursive) {
                            result.AddRange(buildDirectoryIndex(directories[i].FullName, recursive));
                        }
                    }
                } else {
                    result.Add(dir.FullName);
                }
                return result;
            } else {
                throw new DirectoryNotFoundException(String.Format("Directory \"{0}\" does not exists.", path));
            }
        }

        private List<string> buildFileIndex(string path, string mask) {
            DirectoryInfo dir = new DirectoryInfo(path);
            List<string> result = new List<string>();
            if (dir.Exists) {
                FileInfo[] files = dir.GetFiles(mask);
                for (int i = 0; i < files.Length; ++i) {
                    result.Add(files[i].FullName);
                }
                return result;
            } else {
                throw new DirectoryNotFoundException(String.Format("Directory \"{0}\" does not exists.", path));
            }
        }
    }
}
