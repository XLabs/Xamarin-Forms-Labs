using System.Diagnostics.CodeAnalysis;

namespace XLabs.Serialization
{
    /// <summary>
    /// The protobuf Serializer interface.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public interface IProtoBufSerializer : ISerializer
    {
    }
}
