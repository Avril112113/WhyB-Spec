using System;
using System.Text;

namespace GenerateSpec
{
    public class GenerateCs : Generator
    {
        private SpecData _spec;
        private int _indent;
        private StringBuilder _sb;

        public override string GetOutputName()
        {
            return "WhyB.cs";
        }

        public override string Generate(SpecData spec)
        {
            _spec = spec;
            _indent = 0;
            _sb = new StringBuilder();

            _sb.AppendLine("/* This file was generated from GenerateSpec/GeneratorCS.cs */");
            _sb.AppendLine();
            _sb.AppendLine("using Newtonsoft.Json;");
            _sb.AppendLine("using System;");
            _sb.AppendLine();
            _sb.AppendLine("/// <summary>", _indent);
            _sb.AppendLine($"/// WhyB Version: {_spec.Version}");
            _sb.AppendLine("/// </summary>", _indent);
            _sb.AppendLine("namespace WhyB {");
            _indent++;

            _sb.AppendLine("public interface IWhyBBackend {", _indent);
            _indent++;

            foreach (var (name, request) in _spec.Requests) {
                GenerateRequestMethodInterface(name, request, false);
                _sb.AppendLine();
            }

            _sb.Replace(Environment.NewLine, string.Empty, _sb.Length-2, 2);
            _indent--;
            _sb.AppendLine("}", _indent);

            _sb.AppendLine();
            
            _sb.AppendLine("public interface IWhyBFrontend {", _indent);
            _indent++;

            foreach (var (name, eventInfo) in _spec.Events) {
                GenerateEventMethodInterface(name, eventInfo, false);
                _sb.AppendLine();
            }

            _sb.Replace(Environment.NewLine, string.Empty, _sb.Length-2, 2);
            _indent--;
            _sb.AppendLine("}", _indent);

            _sb.AppendLine();

            _sb.AppendLine("public interface IWhyBBackendSingleArgs {", _indent);
            _indent++;

            foreach (var (name, request) in _spec.Requests) {
                GenerateRequestMethodInterface(name, request, true);
                _sb.AppendLine();
            }

            _sb.Replace(Environment.NewLine, string.Empty, _sb.Length - 2, 2);
            _indent--;
            _sb.AppendLine("}", _indent);

            _sb.AppendLine();

            _sb.AppendLine("public interface IWhyBFrontendSingleArgs {", _indent);
            _indent++;

            foreach (var (name, eventInfo) in _spec.Events) {
                GenerateEventMethodInterface(name, eventInfo, true);
                _sb.AppendLine();
            }

            _sb.Replace(Environment.NewLine, string.Empty, _sb.Length - 2, 2);
            _indent--;
            _sb.AppendLine("}", _indent);

            _sb.AppendLine();

            _sb.AppendLine("#region Extensions");
            foreach (var (extensionName, extension) in _spec.Extensions) {
                GenerateExtension(extensionName, extension);
                _sb.AppendLine();
            }
            _sb.Replace(Environment.NewLine, string.Empty, _sb.Length - 2, 2);
            _sb.AppendLine("#endregion Extensions");

            _sb.AppendLine();

            _sb.AppendLine("#region RequestClasses");
            foreach (var (name, request) in _spec.Requests) {
                GenerateRequestArgsClass(name, request);
                _sb.AppendLine();
            }
            foreach (var (extensionName, extension) in _spec.Extensions) {
                foreach (var (name, request) in extension.Requests) {
                    GenerateRequestArgsClass(name, request);
                    _sb.AppendLine();
                }
            }
            _sb.Replace(Environment.NewLine, string.Empty, _sb.Length - 2, 2);
            _sb.AppendLine("#endregion RequestClasses");

            _sb.AppendLine();
            
            _sb.AppendLine("#region EventClasses");
            foreach (var (name, eventInfo) in _spec.Events) {
                GenerateEventClass(name, eventInfo);
                _sb.AppendLine();
            }
            foreach (var (extensionName, extension) in _spec.Extensions) {
                foreach (var (name, eventInfo) in extension.Events) {
                    GenerateEventClass(name, eventInfo);
                    _sb.AppendLine();
                }
            }
            _sb.Replace(Environment.NewLine, string.Empty, _sb.Length - 2, 2);
            _sb.AppendLine("#endregion EventClasses");

            _indent--;
            _sb.AppendLine("}", _indent);

            return _sb.ToString();
        }

        private void GenerateExtension(string extensionName, Extension extension)
        {
            _sb.AppendLine($"public interface IWhyBBackend{extensionName.ToPascalCase()} {{", _indent);
            _indent++;

            _sb.AppendLine($"public const string Name = \"{extensionName}\";", _indent);
            _sb.AppendLine();

            foreach (var (name, request) in extension.Requests) {
                GenerateRequestMethodInterface(name, request, false);
                _sb.AppendLine();
            }

            if (extension.Requests.Count <= 0) {
                _sb.AppendLine();
            }

            _sb.Replace(Environment.NewLine, string.Empty, _sb.Length - 2, 2);
            _indent--;
            _sb.AppendLine("}", _indent);

            _sb.AppendLine();

            _sb.AppendLine($"public interface IWhyBFrontend{extensionName.ToPascalCase()} {{", _indent);
            _indent++;

            foreach (var (name, eventInfo) in extension.Events) {
                GenerateEventMethodInterface(name, eventInfo, false);
                _sb.AppendLine();
            }

            if (extension.Events.Count <= 0) {
                _sb.AppendLine();
            }

            _sb.Replace(Environment.NewLine, string.Empty, _sb.Length - 2, 2);
            _indent--;
            _sb.AppendLine("}", _indent);


            _sb.AppendLine($"public interface IWhyBBackend{extensionName.ToPascalCase()}SingleArgs {{", _indent);
            _indent++;

            _sb.AppendLine($"public const string Name = \"{extensionName}\";", _indent);
            _sb.AppendLine();

            foreach (var (name, request) in extension.Requests) {
                GenerateRequestMethodInterface(name, request, true);
                _sb.AppendLine();
            }

            if (extension.Requests.Count <= 0) {
                _sb.AppendLine();
            }

            _sb.Replace(Environment.NewLine, string.Empty, _sb.Length - 2, 2);
            _indent--;
            _sb.AppendLine("}", _indent);

            _sb.AppendLine();

            _sb.AppendLine($"public interface IWhyBFrontend{extensionName.ToPascalCase()}SingleArgs {{", _indent);
            _indent++;

            foreach (var (name, eventInfo) in extension.Events) {
                GenerateEventMethodInterface(name, eventInfo, true);
                _sb.AppendLine();
            }

            if (extension.Events.Count <= 0) {
                _sb.AppendLine();
            }

            _sb.Replace(Environment.NewLine, string.Empty, _sb.Length - 2, 2);
            _indent--;
            _sb.AppendLine("}", _indent);
        }

        private void GenerateRequestArgsClass(string name, Request request)
        {
            _sb.AppendLine("/// <summary>", _indent);
            _sb.AppendLine($"/// {request.Desc.Indent(_indent, "/// ")}", _indent);
            _sb.AppendLine("/// </summary>", _indent);
            _sb.AppendLine($"public class Request{name.ToPascalCase()} {{", _indent);
            _indent++;

            _sb.Append(new string('\t', _indent));
            _sb.Append($"public Request{name.ToPascalCase()}(");
            foreach (var (paramName, valueInfo) in request.Params) {
                _sb.Append($"{GetCsTypeName(_spec, valueInfo.Type, valueInfo.Optional)} {paramName.ToCamelCase()}, ");
            }
            _sb.Replace(", ", string.Empty, _sb.Length - 2, 2);
            _sb.AppendLine(")");
            _sb.AppendLine("{", _indent);
            _indent++;
            foreach (var (paramName, _) in request.Params) {
                _sb.AppendLine($"{paramName.ToPascalCase()} = {paramName.ToCamelCase()};", _indent);
            }
            _indent--;
            _sb.AppendLine("}", _indent);
            _sb.AppendLine();

            foreach (var (paramName, valueInfo) in request.Params) {
                _sb.AppendLine("/// <summary>", _indent);
                _sb.AppendLine($"/// {valueInfo.Desc.Indent(_indent, "/// ")}", _indent);
                _sb.AppendLine("/// </summary>", _indent);
                NameToJsonProperty(GetCsTypeName(_spec, valueInfo.Type, valueInfo.Optional), paramName, valueInfo.Optional);
                _sb.AppendLine();
            }
            _sb.Replace(Environment.NewLine, string.Empty, _sb.Length - 2, 2);

            _indent--;
            _sb.AppendLine("}", _indent);
        }

        private void GenerateRequestMethodInterface(string name, Request request, bool singleArgs)
        {
            _sb.AppendLine("/// <summary>", _indent);
            _sb.AppendLine($"/// {request.Desc.Indent(_indent, "/// ")}", _indent);
            _sb.AppendLine("/// </summary>", _indent);
            if (singleArgs) {
                _sb.AppendLine("/// <param name=\"request\">The request parameters</param>", _indent);
                _sb.AppendLine($"/// <returns>{request.Result.Desc.Indent(_indent, "/// ")}</returns>", _indent);
                _sb.AppendLine(string.Format("{0} {1}(Request{1} request);", GetCsTypeName(_spec, request.Result.Type, request.Result.Optional), name.ToPascalCase()), _indent);
            } else {
                foreach (var (paramName, valueInfo) in request.Params) {
                    _sb.AppendLine($"/// <param name=\"{paramName.ToCamelCase()}\">{valueInfo.Desc.Indent(_indent, "/// ")}</param>", _indent);
                }
                _sb.AppendLine($"/// <returns>{request.Result.Desc.Indent(_indent, "/// ")}</returns>", _indent);
                _sb.Append(new string('\t', _indent));
                _sb.Append(GetCsTypeName(_spec, request.Result.Type, request.Result.Optional));
                _sb.Append(' ');
                _sb.Append(name.ToPascalCase());
                _sb.Append('(');
                foreach (var (paramName, valueInfo) in request.Params) {
                    _sb.Append(GetCsTypeName(_spec, valueInfo.Type, valueInfo.Optional));
                    _sb.Append(' ');
                    _sb.Append(paramName.ToCamelCase());
                    _sb.Append(", ");
                }
                _sb.Replace(", ", string.Empty, _sb.Length - 2, 2);
                _sb.Append(");");
                _sb.AppendLine();
            }
        }

        private void GenerateEventClass(string name, Event eventInfo)
        {
            _sb.AppendLine("/// <summary>", _indent);
            _sb.AppendLine($"/// {eventInfo.Desc.Indent(_indent, "/// ")}", _indent);
            _sb.AppendLine("/// </summary>", _indent);
            _sb.AppendLine($"public class Event{name.ToPascalCase()} {{", _indent);
            _indent++;

            _sb.Append(new string('\t', _indent));
            _sb.Append($"public Event{name.ToPascalCase()}(");
            foreach (var (paramName, valueInfo) in eventInfo.Data) {
                _sb.Append($"{GetCsTypeName(_spec, valueInfo.Type, valueInfo.Optional)} {paramName.ToCamelCase()}, ");
            }
            _sb.Replace(", ", string.Empty, _sb.Length - 2, 2);
            _sb.AppendLine(")");
            _sb.AppendLine("{", _indent);
            _indent++;
            foreach (var (paramName, valueInfo) in eventInfo.Data) {
                _sb.AppendLine(string.Format("{1} = {0};", paramName.ToCamelCase(), paramName.ToPascalCase()), _indent);
            }
            _indent--;
            _sb.AppendLine("}", _indent);
            _sb.AppendLine();

            foreach (var (paramName, valueInfo) in eventInfo.Data) {
                _sb.AppendLine("/// <summary>", _indent);
                _sb.AppendLine($"/// {valueInfo.Desc.Indent(_indent, "/// ")}", _indent);
                _sb.AppendLine("/// </summary>", _indent);
                NameToJsonProperty(GetCsTypeName(_spec, valueInfo.Type, valueInfo.Optional), paramName, valueInfo.Optional);
                _sb.AppendLine();
            }
            _sb.Replace(Environment.NewLine, string.Empty, _sb.Length - 2, 2);

            _indent--;
            _sb.AppendLine("}", _indent);
        }

        private void GenerateEventMethodInterface(string name, Event eventInfo, bool singleArgs)
        {
            _sb.AppendLine("/// <summary>", _indent);
            _sb.AppendLine($"/// {eventInfo.Desc.Indent(_indent, "/// ")}", _indent);
            _sb.AppendLine("/// </summary>", _indent);
            if (singleArgs) {
                _sb.AppendLine("/// <param name=\"data\">The event parameters</param>", _indent);
                _sb.AppendLine(string.Format("void {0}(Event{0} data);", name.ToPascalCase()), _indent);
            } else {
                foreach (var (paramName, valueInfo) in eventInfo.Data) {
                    _sb.AppendLine($"/// <param name=\"{paramName.ToCamelCase()}\">{valueInfo.Desc.Indent(_indent, "/// ")}</param>", _indent);
                }
                _sb.Append(new string('\t', _indent));
                _sb.Append("void ");
                _sb.Append(name.ToPascalCase());
                _sb.Append('(');
                foreach (var (paramName, valueInfo) in eventInfo.Data) {
                    _sb.Append(GetCsTypeName(_spec, valueInfo.Type, valueInfo.Optional));
                    _sb.Append(' ');
                    _sb.Append(paramName.ToCamelCase());
                    _sb.Append(", ");
                }
                _sb.Replace(", ", string.Empty, _sb.Length - 2, 2);
                _sb.Append(");");
                _sb.AppendLine();
            }
        }

        private void NameToJsonProperty(string type, string fieldName, bool optional = true)
        {
            _sb.AppendLine($"[JsonProperty(\"{fieldName}\"{(optional ? "" : ", Required = Required.Always")})]", _indent);
            _sb.AppendLine($"public {type} {fieldName.ToPascalCase()} {{ get; set; }}", _indent);
        }

        private static string GetCsTypeName(SpecData spec, string type, bool optional)
        {
            if (spec.Types.ContainsKey(type)) {
                return spec.Types[type].Real.ToPascalCase() + (optional ? "?" : "");
            }
            return type + (optional ? "?" : "");
        }
    }
}
