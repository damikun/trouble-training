using System.Linq;

namespace MediatR {
    public class MediatorSerializedObject {
        public MediatorSerializedObject(string fullTypeName, string data, string additionalDescription, string assemblyName = "") {
            FullTypeName = fullTypeName;
            Data = data;
            AdditionalDescription = additionalDescription;
            AssemblyName = assemblyName == "" ? "Aplication.Commands" : assemblyName;
        }

        public string FullTypeName { get; private set; }
        public string Data { get; private set; }
        public string AdditionalDescription { get; private set; }
        public string AssemblyName { get; private set; }


        /// <summary>
        /// Override for Hangfire dashboard display.
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            var commandName = FullTypeName.Split('.').Last();
            return $"{commandName} {AdditionalDescription}";
        }
    }
}