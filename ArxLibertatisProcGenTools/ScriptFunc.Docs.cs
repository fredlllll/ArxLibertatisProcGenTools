using ArxLibertatisEditorIO.WellDoneIO;
using ArxLibertatisProcGenTools.Generators;
using ArxLibertatisProcGenTools.Modifiers;
using ArxLibertatisProcGenTools.Shapes;
using ArxLibertatisProcGenTools.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ArxLibertatisProcGenTools
{
    public static partial class ScriptFunc
    {
        public static bool Markdown { get; set; } = false;

        private static void WriteLine(string text)
        {
            string line = text;
            if (Markdown)
            {
                //replace leading spaces
                if (line.StartsWith(' '))
                {
                    int count = text.TakeWhile(x => x == ' ').Count();
                    var oldLine = line;
                    line = "";
                    for (int i = 0; i < count; i++)
                    {
                        line += "&nbsp;";
                    }
                    line += oldLine[count..];
                }
                //escape backticks
                line = line.Replace("`", "\\`");
            }
            else
            {
                line = line.Replace("<br>","");
            }
            Console.WriteLine(line);
        }


        private static void PrintMethods(IEnumerable<MethodInfo> methods)
        {
            foreach (var method in methods)
            {
                WriteLine($"    {(method.IsStatic ? "static " : "")}{method.ReturnType.Name} {method.Name}({string.Join(", ", method.GetParameters().Select(x => x.ToString()))})<br>");
                var desc = method.GetCustomAttribute<DescriptionAttribute>();
                if (desc != null)
                {
                    WriteLine($"      {desc.Description}<br>");
                }
            }
        }

        private static void PrintProperties(IEnumerable<PropertyInfo> props)
        {
            foreach (var prop in props)
            {
                WriteLine($"    {(prop.GetMethod.IsStatic ? "static " : "")}{prop.PropertyType.Name} {prop.Name}<br>");
                var desc = prop.GetCustomAttribute<DescriptionAttribute>();
                if (desc != null)
                {
                    WriteLine($"      {desc.Description}<br>");
                }
            }
        }

        private static void PrintTypes(IEnumerable<Type> types)
        {
            foreach (var t in types)
            {
                WriteLine($"**{t.FullName}**<br>");
                var desc = t.GetCustomAttribute<DescriptionAttribute>();
                if (desc != null)
                {
                    WriteLine($"{desc.Description}<br>");
                }
                WriteLine("  Constructors:<br>");
                var constructors = t.GetConstructors();
                foreach (var constructor in constructors)
                {
                    WriteLine($"    {t.Name}({string.Join(", ", constructor.GetParameters().Select(x => x.ToString()))})<br>");
                }

                WriteLine("  Properties:<br>");
                var props = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                PrintProperties(props);

                WriteLine("  Methods:<br>");
                var methods = t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(m => !m.IsSpecialName);
                PrintMethods(methods);
                WriteLine("<br>");
            }
        }
        public static void PrintPsDocs()
        {
            WriteLine("# Powershell Documentation:");
            WriteLine("## This is a listing of all relevant classes and functions for generating levels in powershell scripts");

            Type baseInterface = typeof(IMeshGenerator);
            WriteLine("### Mesh Generators are classes that will generate polygons that will be added to the level. These are available:");
            var childTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => baseInterface.IsAssignableFrom(p) && p != baseInterface);
            PrintTypes(childTypes);

            WriteLine("### Light generators generate lights that will be added to the level. These are available:");
            baseInterface = typeof(ILightGenerator);
            childTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => baseInterface.IsAssignableFrom(p) && p != baseInterface);
            PrintTypes(childTypes);

            WriteLine("### Texture generators generate texture names depending on position. These are available:");
            baseInterface = typeof(ITextureGenerator);
            childTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => baseInterface.IsAssignableFrom(p) && p != baseInterface);
            PrintTypes(childTypes);

            WriteLine("### Modifiers modify the currently existing polygons. These are available:");
            baseInterface = typeof(IModifier);
            childTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => baseInterface.IsAssignableFrom(p) && p != baseInterface);
            PrintTypes(childTypes);

            WriteLine("### Shapes can be used in a lot of ways to shape the output of other classes. These are available:");
            baseInterface = typeof(IShape);
            childTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => baseInterface.IsAssignableFrom(p) && p != baseInterface);
            PrintTypes(childTypes);

            WriteLine("### Values are similar to Shapes, just one dimensional. These are available:");
            baseInterface = typeof(IValue);
            childTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => baseInterface.IsAssignableFrom(p) && p != baseInterface);
            PrintTypes(childTypes);

            WriteLine($"### In addition to these classes, there is the {nameof(ScriptFunc)} class, which contains static functions that make scripting easier:");
            WriteLine("  Properties:<br>");
            var props = typeof(ScriptFunc).GetProperties(BindingFlags.Public | BindingFlags.Static);
            PrintProperties(props);
            WriteLine("  Methods:<br>");
            var methods = typeof(ScriptFunc).GetMethods(BindingFlags.Static | BindingFlags.Public).Where(m => !m.IsSpecialName);
            PrintMethods(methods);

            WriteLine("<br>");
            WriteLine($"Additionally you may change the {nameof(WellDoneArxLevel)} of the {nameof(ScriptFunc)} class however you like. You might need to access classes from {nameof(ArxLibertatisEditorIO)} for this<br>");
        }
    }
}
