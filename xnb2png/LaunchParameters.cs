using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xnb2png {
    public class LaunchParameters {
        Dictionary<string, string> parameters;

        public Dictionary<string, string> Parameters {
            get {
                return parameters;
            }
        }

        public int Count {
            get {
                return parameters.Count;
            }
        }

        public LaunchParameters() {
            parameters = new Dictionary<string, string>();
        }

        public string this[string key] {
            get {
                if (parameters.ContainsKey(key)) {
                    return parameters[key];
                }
                return String.Empty;
            }
            set {
                if (parameters.ContainsKey(key)) {
                    parameters[key] = value;
                } else {
                    parameters.Add(key, value);
                }
            }
        }
    }
}
