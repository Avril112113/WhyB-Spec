using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace GenerateSpec
{
    public class GenerateMd : Generator
    {
        private readonly struct TocElement
        {
            public readonly string Name;
            public readonly string Target;
            public readonly int Depth;

            public TocElement(string target, string name, int depth = 0)
            {
                Target = target;
                Name = name;
                Depth = depth;
            }

            public TocElement(string target, int depth=0) : this(target, target, depth)
            {
            }
        }

        private StringBuilder _head;
        private StringBuilder _body;
        private List<TocElement> _toc;
        private SpecData _spec;

        public override string GetOutputName()
        {
            return "spec.md";
        }

        public override string Generate(SpecData spec)
        {
            _spec = spec;

            _head = new StringBuilder();
            _toc = new List<TocElement>();
            _head.AppendLine("[comment]: # \"This file was generated from GenerateSpec/GeneratorMD.cs\"  ");
            _head.AppendLine("# **Specification**  ");
            _toc.Add(new TocElement("Specification"));
            _head.AppendLine($"Version: {_spec.Version}  ");
            _head.AppendLine($"{_spec.Desc}  ");

            _body = new StringBuilder();

            _body.AppendLine("# **Mandates**  ");
            _toc.Add(new TocElement("Mandates"));
            foreach (string mandate in _spec.Mandates) {
                _body.AppendLine($"{mandate}  ");
            }
    
            _body.AppendLine();
            _body.AppendLine();

            _body.AppendLine("# **Codes**  ");
            _toc.Add(new TocElement("Codes"));
            int longestDesc = _spec.Codes.Values.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur).Length;
            _body.AppendLine("|  Code   |  Meaning  ");
            _body.AppendLine($"|:-------:|:-{new string('-', longestDesc - 1)}  ");
            foreach (var (code, desc) in _spec.Codes) {
                _body.AppendLine($"| {code}  | {desc}  ");
            }

            _body.AppendLine();
            _body.AppendLine();

            _body.AppendLine("# **Requests**  ");
            _toc.Add(new TocElement("Requests"));
            foreach (var (name, request) in _spec.Requests) {
                _toc.Add(new TocElement(name, $"**`{name}`**", 1));
                GenerateRequest(name, request);
            }

            _body.AppendLine("# **Events**  ");
            _toc.Add(new TocElement("Events"));
            foreach (var (name, eventInfo) in _spec.Events) {
                _toc.Add(new TocElement(name, $"**`{name}`**", 1));
                GenerateEvent(name, eventInfo);
            }

            _toc.Add(new TocElement("Extensions", $"**Extensions**"));
            _body.AppendLine("# **Extensions**  ");
            _body.AppendLine($"{_spec.ExtensionsDesc}  ");
            _body.AppendLine();
            foreach (var (extensionName, extension) in _spec.Extensions) {
                _toc.Add(new TocElement(extensionName, $"**{extensionName}**", 1));
                _body.AppendLine($"# {extensionName}  ");
                _body.AppendLine($"{extension.Desc}  ");
                _body.AppendLine();

                if (extension.Requests.Count > 0) {
                    _body.AppendLine($"# Requests ({extensionName})  ");
                    _toc.Add(new TocElement($"Requests ({extensionName})", "Requests", 2));
                    foreach (var (name, request) in extension.Requests) {
                        _toc.Add(new TocElement(name, $"**`{name}`**", 3));
                        GenerateRequest(name, request);
                    }
                }

                if (extension.Events.Count > 0) {
                    _body.AppendLine($"# Events ({extensionName})  ");
                    _toc.Add(new TocElement($"Events ({extensionName})", "Events", 2));
                    foreach (var (name, eventInfo) in extension.Events) {
                        _toc.Add(new TocElement(name, $"**`{name}`**", 3));
                        GenerateEvent(name, eventInfo);
                    }
                }

                _body.AppendLine();
            }

            // Finish up head
            _head.AppendLine();
            foreach (TocElement elem in _toc) {
                _head.AppendLine($"{new string('\t', elem.Depth)}- [{elem.Name}](#{HttpUtility.UrlPathEncode(elem.Target)})  ");
            }
            _head.AppendLine();
            _head.AppendLine();

            _body.Insert(0, _head);
            return _body.ToString();
        }

        private void GenerateRequest(string name, Request request)
        {
            _body.AppendLine($"## **`{name}`**  ");
            _body.AppendLine($"{request.Desc}  ");
            _body.AppendLine("**Request:**  ");
            _body.AppendLine("```jsonc");
            _body.AppendLine("{");
            _body.AppendLine("\t\"jsonrpc\": \"2.0\",");
            _body.AppendLine($"\t\"method\": \"{name}\",");
            if (request.Params.Count > 0) {
                _body.AppendLine("\t\"params\": {");
                foreach (var (paramName, valueInfo) in request.Params) {
                    _body.AppendLine($"\t\t{ValueToString(paramName, valueInfo, 2)},");
                }
                _body.Replace(",", "", _body.Length - 2, 2);
                _body.AppendLine("\t},");
            }
            _body.AppendLine("\t\"id\": 1");
            _body.AppendLine("}");
            _body.AppendLine("```  ");
            _body.AppendLine("**Response:**  ");
            _body.AppendLine("```jsonc");
            _body.AppendLine("{");
            _body.AppendLine("\t\"jsonrpc\": \"2.0\",");
            _body.AppendLine($"\t{ValueToString("result", request.Result, 1)},");
            _body.AppendLine("\t\"id\": 1");
            _body.AppendLine("}");
            _body.AppendLine("```  ");

            _body.AppendLine();
            _body.AppendLine();
        }

        private void GenerateEvent(string name, Event eventInfo)
        {
            _body.AppendLine($"## **`{name}`**  ");
            _body.AppendLine($"{eventInfo.Desc}  ");

            _body.AppendLine("**Event Data:**  ");
            _body.AppendLine("```jsonc");
            _body.AppendLine("{");
            foreach (var (valueName, valueInfo) in eventInfo.Data) {
                _body.AppendLine($"\t{ValueToString(valueName, valueInfo, 1)},");
            }
            _body.Replace(",", "", _body.Length - 2, 2);
            _body.AppendLine("}");
            _body.AppendLine("```  ");

            _body.AppendLine();
            _body.AppendLine();
        }

        private static string ValueToString(string name, ValueInfo valueInfo, int indent=0)
        {
            return string.Format("// {3}{0}\"{1}\": \"{2}\"", Environment.NewLine.Indent(indent), name, valueInfo.Type, valueInfo.Desc.Indent(indent, "// "));
        }
    }
}
