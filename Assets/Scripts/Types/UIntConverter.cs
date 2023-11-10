using System;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

public class UIntConverter : IYamlTypeConverter
{
    public bool Accepts(Type type)
    {
        return type == typeof(uint);
    }

    public object ReadYaml(IParser parser, Type type)
    {
        var scalar = (Scalar)parser.Current;
        uint value = uint.Parse(scalar.Value);
        parser.MoveNext();
        return value;
    }

    public void WriteYaml(IEmitter emitter, object value, Type type)
    {
        var u = (uint)value;
        emitter.Emit(new Scalar(
            null,
            "!u",
            u.ToString(),
            ScalarStyle.Plain,
            false,
            false
        ));
    }
}