using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace xnb2png {
    public static class Utils {
        public static bool CheckDirectory(string dirName, bool createIfNotExists) {
            DirectoryInfo dirInfo = new DirectoryInfo(dirName);
            if (dirInfo.Exists) {
                return true;
            } else {
                if (createIfNotExists) {
                    dirInfo.Create();
                    return true;
                } else {
                    return false;
                }
            }
        }

        public static bool CheckParameter(string text, string[] nameList) {
            for (int i = 0; i < nameList.Length; ++i) {
                if (text.Equals(nameList[i])) {
                    return true;
                }
            }
            return false;
        }
    }
}
